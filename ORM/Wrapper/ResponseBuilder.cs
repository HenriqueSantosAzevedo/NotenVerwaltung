using System.Collections;
using System.Data;
using System.Reflection;
using WebApplication1.Attribute;

namespace WebApplication1.ORM.Wrapper;

public abstract class ResponseBuilder
{
    private static IList createList(Type myType)
    {
        Type genericListType = typeof(List<>).MakeGenericType(myType);
        object? list = Activator.CreateInstance(genericListType);
        if (list == null) throw new Exception();
        return (IList)list;
    }

    private static IEnumerable<Dictionary<string, object>> DataTableSystemToTextJson(DataTable dataTable)
    {
        return dataTable.Rows.OfType<DataRow>()
            .Select(row => dataTable.Columns.OfType<DataColumn>()
                .ToDictionary(col => col.ColumnName, c => row[c]));
    }

    private static void ParseSimpleFields(
        List<SimpleField> simpleFields,
        IGrouping<object, Dictionary<string, object>> grouping,
        Entity entity
    )
    {
        simpleFields.ForEach(field =>
        {
            var val = grouping.First()[$"{field.Abbreviation}.{field.Column.ColumnName!}"];
            if (val is not DBNull)
            {
                Type fieldType = field.Type.IsGenericType ? field.Type.GetGenericArguments().First() : field.Type;
                List<Type> t = new List<Type>();
                t.Add(typeof(Int16));
                t.Add(typeof(Int32));
                t.Add(typeof(Int64));
                
                if (fieldType != val.GetType() && !t.Contains(val.GetType()))
                {
                    MethodInfo mi = typeof(Convert)
                        .GetMethods()
                        .ToHashSet()
                        .First(s => s.Name.Contains($"To{fieldType.Name}"));
                    
                    val = mi.Invoke(null, new[]{val});
                }
                entity.setValue(field.PropertyName, val);
            }
        });
    }

    private static T ParseEntity<T>(IGrouping<object, Dictionary<string, object>> grouping, ParsedEntity parsedEntity)
        where T : Entity
    {
        var entity = (T)Activator.CreateInstance(parsedEntity.Type)!;

        ParseSimpleFields(
            parsedEntity.Fields.Where(field => field.GetType() == typeof(SimpleField)).Cast<SimpleField>().ToList(),
            grouping,
            entity
        );

        parsedEntity.Fields.Where(field => field.GetType() == typeof(JoinedEntity)).Cast<JoinedEntity>().ToList()
            .ForEach(field =>
            {
                MethodInfo m = typeof(ResponseBuilder)
                    .GetMethod("ParseEntity", BindingFlags.NonPublic | BindingFlags.Static)
                    ?.MakeGenericMethod(field.JoinInfo.JoinType)!;

                if (field.JoinInfo.GetType() == typeof(OneToOne))
                {
                    entity.setValue(
                        field.JoinInfo.JoinOn,
                        m.Invoke(null, new object[] { grouping, field })!
                    );
                    return;
                }

                var v = createList(field.Type);
                grouping
                    .GroupBy(objects => objects[$"{field.Abbreviation}.{field.ID.ColumnName}"])
                    .Where(group =>
                        group.Count(objects => objects[field.GetFullName()].GetType() != typeof(DBNull)) > 0)
                    .ToList()
                    .ForEach(group => v.Add(m.Invoke(null, new object[] { group, field })!));

                entity.setValue(field.JoinInfo.JoinOn, v);
            });
        return entity;
    }


    public static List<T> BuildList<T>(DataTable dataTable, ParsedEntity entity) where T : Entity
    {
        List<IGrouping<object, Dictionary<string, object>>> sqlResponse = DataTableSystemToTextJson(dataTable)
            .GroupBy(objects => objects[$"{entity.Abbreviation}.{entity.ID.ColumnName!}"])
            .ToList();

        return sqlResponse.Select(grouping => ParseEntity<T>(grouping, entity)).ToList();
    }
}