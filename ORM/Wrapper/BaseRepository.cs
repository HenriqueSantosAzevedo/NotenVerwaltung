using System.Data;
using System.Data.SQLite;
using Newtonsoft.Json;
using WebApplication1.Attribute;
using WebApplication1.Database.Connection;

namespace WebApplication1.ORM.Wrapper;

public interface IBaseRepository
{
}

public abstract class BaseRepository<T, IDT>
    : IBaseRepository
    where T : Entity
    where IDT : new()
{
    private readonly ParsedEntity _parsedEntity;

    public BaseRepository()
    {
        if (RepositoryHolder.GetRepostitoryByType(typeof(T)) == null)
            RepositoryHolder.AddRepository(this);
        _parsedEntity = ((Entity)Activator.CreateInstance(typeof(T))!).createEntityTree()!;
    }

    private string GetFieldNames(ParsedEntity parsedEntity)
    {
        List<String> fields = parsedEntity
            .Fields
            .Where(field => field is not null)
            .Where(field => field!.GetType() == typeof(SimpleField))
            .Cast<SimpleField>()
            .Select(field =>
            {
                var s = $"{field.Abbreviation}.{field.Column.ColumnName}";
                if (field.Type == typeof(DateTime))
                    return $"DATETIME(ROUND({s}/ 1000), 'unixepoch') as '{s}'";

                return $"{s} as '{s}'";
            })
            .ToList();

        fields.AddRange(parsedEntity.Fields
            .Where(field => field is not null)
            .Where(field => field!.GetType().IsSubclassOf(typeof(ParsedEntity)))
            .Cast<ParsedEntity>()
            .Select(GetFieldNames)
            .ToList());

        return String.Join(", ", fields.Where(s => s.Trim().Length > 0));
    }

    private List<JoinedEntity> GetJoins()
    {
        return _parsedEntity
            .Fields
            .Where(field => field?.GetType() == typeof(JoinedEntity))
            .Cast<JoinedEntity>()
            .ToList();
    }

    private String FindAllStruc()
    {
        return
            $"select {GetFieldNames(_parsedEntity)} from {_parsedEntity.TableName} as {_parsedEntity.Abbreviation} \n" +
            String.Join(
                "",
                GetJoins()
                    .Select(entity => entity.ToString(_parsedEntity))
            );
    }

    public List<T> FindAll()
    {
        var v = $"{FindAllStruc()}";
        DataTable table;
        try
        {
            table = DatabaseService.GetDatabase().SelectSqlData(v).Tables[0];
        }
        catch (SQLiteException e)
        {
            Console.Error.WriteLine($"Failed to execute {v}");
            return new();
        }

        return ResponseBuilder.BuildList<T>(table, _parsedEntity);
    }

    public List<T> FindWhere(String whereClause)
    {
        var v = $"{FindAllStruc()} where {whereClause}";
        DataTable table;
        try
        {
            table = DatabaseService.GetDatabase().SelectSqlData(v).Tables[0];
        }
        catch (SQLiteException e)
        {
            Console.Error.WriteLine($"Failed to execute {v}");
            return new();
        }

        return ResponseBuilder.BuildList<T>(table, _parsedEntity);
    }

    private void CheckNotNull(T entity)
    {
        bool valid = _parsedEntity.Fields.Any(customField =>
        {
            string _field = "";
            if (customField.GetType() == typeof(SimpleField))
            {
                SimpleField s = (SimpleField)customField;
                _field = s.PropertyName;
            } else if (customField.GetType() == typeof(JoinedEntity))
            {
                JoinedEntity p = (JoinedEntity)customField;
                _field = p.JoinInfo.JoinOn;
            }

            if (customField.IsNotNull) return false;
            Console.WriteLine(entity.getValue(_field) is null);
            return entity.getValue(_field) is null;
        });

        if (!valid)
        {
            throw new Exception($"Entity {entity.GetType().Name} is not Valid");
        }
    }


    public IDT? save(T entity)
    {
        var foreignFields = new Dictionary<string, object>();
        var fields = _parsedEntity.Fields.Where(field => field.GetType() == typeof(SimpleField)).Cast<SimpleField>()
            .ToList();

        var values = fields
            .Select(field =>
            {
                object? v = entity.getValue(field.PropertyName);
                if (v is null)
                    return $"null";

                if (field.StorageItem.Type.StartsWith("varchar")) return $"\'{v}\'";
                return v;
            }).ToList();

        var joinedEntities = _parsedEntity.Fields
            .Where(field => field.GetType() == typeof(JoinedEntity))
            .Cast<JoinedEntity>()
            .Where(field => field.JoinInfo.GetType() == typeof(OneToOne))
            .ToList();

        joinedEntities.ForEach(field =>
        {
            OneToOne join = (OneToOne)field.JoinInfo;
            if (join.ReferencedBy == ReferencedBy.CHILD)
            {
                var foreignField = field
                    .Fields
                    .Where(customField => customField.GetType() == typeof(SimpleField))
                    .Cast<SimpleField>()
                    .First(simpleField => simpleField.Column.ColumnName == join.Foreign);

                var primaryField = field
                    .Fields
                    .Where(customField => customField.GetType() == typeof(SimpleField))
                    .Cast<SimpleField>()
                    .First(simpleField => simpleField.Column.ColumnName == join.Primary);

                Entity e = (Entity)entity.getValue(field.JoinInfo.JoinOn)!;

                foreignFields.Add(
                    primaryField.Column.ColumnName!,
                    e.getValue(foreignField.PropertyName!)!
                );
            }
        });


        var columns = String.Join(", ", fields.Select(field => field.Column.ColumnName)) +
                      (foreignFields.Keys.Count > 0 ?  $", {String.Join(", ", foreignFields.Keys)}" : "");
        
        values.AddRange(foreignFields
            .Values
            .Select((object? v) =>
                {
                    if (v is null)
                        return $"null";

                    return v;
                })
            .ToList()
        );
        
        String query = $"insert into {_parsedEntity.TableName}({columns}) values ({String.Join(", ", values)})";
        Console.WriteLine(query);
        return default;
    }
}