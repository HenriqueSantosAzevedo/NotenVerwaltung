using JetBrains.Annotations;
using WebApplication1.Attribute;
using WebApplication1.Database;

namespace WebApplication1.ORM.Wrapper;

public interface ICustomField
{
    [UsedImplicitly]
    public String Abbreviation { get; set; }
    
    public Boolean IsNotNull { get; set; }
    
    public Type Type { get; set; }
}

public class SimpleField : ICustomField
{
    public bool IsNotNull { get; set; }
    public Type Type { get; set; }
    public Column Column { get; set; }

    public StorageItem StorageItem { get; set; }
    
    internal SimpleField(Type type, Column column)
    {
        Type = type;
        Column = column;
    }

    [UsedImplicitly]
    public string? Abbreviation { get; set; }
    
    [UsedImplicitly]
    public String PropertyName { get; set; }
}

public class ParsedEntity : ICustomField
{
    public bool IsNotNull { get; set; }
    public Type Type { get; set; }
    
    public List<ICustomField> Fields { get; set; }

    public ParsedEntity(Type type)
    {
        Type = type;
        Fields = new List<ICustomField?>();
    }
    
    [UsedImplicitly]
    public string? Abbreviation { get; set; }
    
    [UsedImplicitly]
    public Column ID { get; set; }
    
    [UsedImplicitly]
    public string? TableName { get; set; }

    public string GetFullName()
    {
        return $"{Abbreviation}.{ID.ColumnName}";
    }
}

public class JoinedEntity : ParsedEntity
{
    public Join JoinInfo { get; set; }
    public JoinedEntity(Type type, Join joinInfo) : base(type)
    {
        JoinInfo = joinInfo;
    }
    
    public string ToString(ParsedEntity left)
    {
        return $"{JoinInfo.ToString(left, this)}\n{String.Join("", Fields.Where(field => field?.GetType() == typeof(JoinedEntity)).Cast<JoinedEntity>().Select(field => field.ToString(this)).ToList())}";
    }
}