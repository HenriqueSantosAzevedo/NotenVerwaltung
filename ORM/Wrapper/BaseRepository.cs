using System.Data;
using System.Data.SQLite;
using System.Reflection;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using NotenAppConsoleSchueler.Database.Connection;
using NotenAppConsoleSchueler.Database.Exception;
using NotenAppConsoleSchueler.ORM.Wrapper.Attribute;
using WebApplication1.Database.Connection;

namespace NotenAppConsoleSchueler.ORM.Wrapper;

public interface IBaseRepository
{
}

public abstract class BaseRepository<T, IDT>
    : IBaseRepository
    where T : Entity
    where IDT : new()
{
    private readonly ParsedEntity _parsedEntity;
    private readonly IDatabase _db = DatabaseService.GetDatabase();

    public BaseRepository()
    {
        if (RepositoryHolder.GetRepostitoryByType(typeof(T)) == null)
            RepositoryHolder.AddRepository(this);
        _parsedEntity = ((Entity)Activator.CreateInstance(typeof(T))!).CreateEntityTree()!;
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
            table = _db.SelectSqlData(v).Tables[0];
        }
        catch (SQLiteException e)
        {
            Console.Error.WriteLine($"Failed to execute {v}");
            return new();
        }

        return ResponseBuilder.BuildList<T>(table, _parsedEntity);
    }

    static bool IsValidInput(string input)
    {
        // Regex pattern to allow alphabets, digits, _, -, ,, and .
        string pattern = @"^[a-zA-Z0-9_\-,.]+$";

        // Check if the input matches the pattern
        return Regex.IsMatch(input, pattern);
    }

    public List<T> FindWhere(String whereClause, params object[] data)
    {
        foreach (var o in data)
        {
            if (o is string s && !IsValidInput(s))
            {
                throw new SQLInjectionException();
            }
            whereClause = StringUtils.ReplaceFirst(whereClause, "?", o.ToString()!);
        }
        
        var v = $"{FindAllStruc()} where {whereClause}";
        DataTable table;
        try
        {
            table = _db.SelectSqlData(v).Tables[0];
        }
        catch (SQLiteException e)
        {
            Console.Error.WriteLine($"Failed to execute {v}");
            return new();
        }

        return ResponseBuilder.BuildList<T>(table, _parsedEntity);
    }

    private object valueParser(T entity, SimpleField field)
    {
        object? v = entity.getValue(field.PropertyName);
        if (v is null)
            return $"null";
        if (field.Type == typeof(DateTime)) return ((DateTime)v).Ticks;
        if (field.StorageItem.Type.StartsWith("varchar")) return $"\'{v}\'";
        return v;
    }

    public long save(T entity)
    {
        var foreignFields = new Dictionary<string, object>();
        var fields = _parsedEntity.Fields.Where(field => field.GetType() == typeof(SimpleField)).Cast<SimpleField>()
            .ToList();
        
        long id = entity.getId<long>();
        if (id < 1)
        {
            var idColumn = _parsedEntity.ID.ColumnName;
            id = Convert.ToInt64(_db
                .SelectSqlData($"select max({idColumn}) + 1 as 'nextId' from {_parsedEntity.TableName}")
                .Tables[0]
                .Rows[0]
                .ItemArray[0]
            );

            SimpleField sf = fields.First(field => field.Column.ColumnName == idColumn);
            entity.setValue(sf.PropertyName, id);
        }

        var values = fields
            .Select(field => valueParser(entity, field)).ToList();

        var joinedEntities = _parsedEntity.Fields
            .Where(field => field.GetType() == typeof(JoinedEntity))
            .Cast<JoinedEntity>()
            .Where(field => field.JoinInfo.GetType() == typeof(OneToOne))
            .ToList();

        joinedEntities.ForEach(field =>
        {
            OneToOne join = (OneToOne)field.JoinInfo;
            if (join.ReferencedBy == ReferencedBy.THIS)
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


        Console.WriteLine(JsonConvert.SerializeObject(foreignFields, Formatting.Indented));        
        
        var columns = String.Join(", ", fields.Select(field => field.Column.ColumnName)) +
                      (foreignFields.Keys.Count > 0 ? $", {String.Join(", ", foreignFields.Keys)}" : "");

        
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
        
        
        String query = $"insert into {_parsedEntity.TableName}({columns}) values ({String.Join(", ", values)}) RETURNING {_parsedEntity.ID.ColumnName}";
        Console.WriteLine(query);
        return Convert.ToInt64(_db.genericSqlCommand(query, false));
    }
}