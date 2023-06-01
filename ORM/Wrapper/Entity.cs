using System.Reflection;
using NotenAppConsoleSchueler.Database.Connection;
using NotenAppConsoleSchueler.ORM.Repository;
using NotenAppConsoleSchueler.ORM.Wrapper.Attribute;
using WebApplication1.Database;

namespace NotenAppConsoleSchueler.ORM.Wrapper;

public abstract class Entity
{
    private Dictionary<Type, ParsedEntity> _parsedEntities = new();
    
    private List<PropertyInfo> FilteredProperties()
    {
        return GetType()
            .GetProperties()
            .OrderBy(info => info.GetCustomAttributes<Priority>().FirstOrDefault(new Priority()).Amount * -1 )
            .Where(fieldInfo =>
                fieldInfo.GetCustomAttributes<Column>().ToArray().Length > 0 ||
                fieldInfo.GetCustomAttributes<Join>().ToArray().Length > 0
            )
            .ToList();
    }

    public ParsedEntity? CreateEntityTree(List<Type>? parentEntities = null, Join? joinInfo = null, bool isNotNull = false)
    {
        if (parentEntities == null) parentEntities = new List<Type>();
        if (parentEntities.Contains(GetType()))
            return null;
        parentEntities.Add(GetType());

        if (_parsedEntities.ContainsKey(GetType()))
            return _parsedEntities[GetType()];
        
        ParsedEntity parsedEntity = joinInfo == null ? new ParsedEntity(GetType()) : new JoinedEntity(GetType(), joinInfo);
        parsedEntity.IsNotNull = isNotNull;
        
        List<Table> attributes = GetType().GetCustomAttributes<Table>().ToList();
        if (attributes.Count != 1)
            throw new Exception($"Entity {GetType().Name} needs to have exactly one Table Attribute");
        parsedEntity.TableName = attributes.First().TableName;
        
        foreach (PropertyInfo propertyInfo in FilteredProperties())
        {
            Type propertyType = propertyInfo.PropertyType;
            if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(List<>))
            {
                propertyType = propertyType.GetGenericArguments()[0];
            }

            if (propertyType.IsSubclassOf(typeof(Entity)))
            {
                NotNull? notNullAttribute = propertyInfo.GetCustomAttributes<NotNull>(true).FirstOrDefault();
                Join j = propertyInfo.GetCustomAttributes<Join>(true).First();
                j.JoinType = propertyType;
                j.JoinOn = propertyInfo.Name;
                

                parsedEntity
                    .Fields
                    .Add(
                        ((Entity)Activator.CreateInstance(propertyType)!).CreateEntityTree(new List<Type>(parentEntities), j, notNullAttribute != null)!
                    );
                continue;
            }

            Column c = propertyInfo.GetCustomAttributes<Column>().First();
            if (c.ColumnName == null)
            {
                c.ColumnName = propertyInfo.Name.ToLower();
            }
            
            if (propertyInfo.GetCustomAttributes<ID>().ToList().Count > 0)
                parsedEntity.ID = c;
            
            var s = new SimpleField(propertyType, c)
            {
                PropertyName = propertyInfo.Name
            };
            parsedEntity.Fields.Add(s);
        }
        
        parsedEntity.Fields = parsedEntity.Fields.Where(field => field is not null).ToList();
        PopulateAbbreviationFields(parsedEntity);
        
        List<StorageItem> storageItems = DatabaseService.GetDatabase().TableDataStorage.StorageItemFor(parsedEntity);

        parsedEntity
            .Fields
            .Where(field => field.GetType() == typeof(SimpleField))
            .Cast<SimpleField>()
            .ToList()
            .ForEach(field => field.StorageItem = storageItems.First(item => item.Field == field.Column.ColumnName));

        return parsedEntity;
    }

    private static void PopulateAbbreviationFields(ParsedEntity parsedEntity, Dictionary<char, int>? dictionary = null)
    {
        dictionary ??= new Dictionary<char, int>();
        dictionary[parsedEntity.Type.Name.ToLower()[0]] = (dictionary.TryGetValue(parsedEntity.Type.Name.ToLower()[0], out var value) ? value : -1) + 1;
        
        
        parsedEntity.Abbreviation = $"{Convert.ToChar(parsedEntity.Type.Name[0] + 32)}{dictionary[parsedEntity.Type.Name.ToLower()[0]]}";

        parsedEntity
            .Fields
            .Where(field => field is not null)
            .Where(field => field!.GetType() == typeof(SimpleField))
            .Cast<SimpleField>()
            .ToList()
            .ForEach(field => field.Abbreviation = parsedEntity.Abbreviation);
        
        parsedEntity
            .Fields
            .Where(field => field is not null)
            .Where(field => field!.GetType().IsSubclassOf(typeof(ParsedEntity)))
            .Cast<ParsedEntity>()
            .ToList()
            .ForEach(field => PopulateAbbreviationFields(field, dictionary));
    }

    public void setValue(string field, object val)
    {
        PropertyInfo pi = GetType().GetProperties().First(info => info.Name == field);
        pi.SetValue(this, val);
    }

    public IDT getId<IDT>() where IDT : new()
    {
        List<PropertyInfo> pInfos = GetType().GetProperties().Where(
            p => p.GetCustomAttributes(typeof(ID), true).Length != 0).ToList();
        
        if (pInfos.Count == 0) return new IDT();
        return (IDT) pInfos.First().GetValue(this)!;
    }
    
    public object? getValue(string fieldName)
    {
        Console.WriteLine(fieldName);
        
        List<PropertyInfo> pInfos = GetType()
            .GetProperties()
            .Where(p => p.Name == fieldName)
            .ToList();
        return pInfos.First().GetValue(this)!;
    }
}


