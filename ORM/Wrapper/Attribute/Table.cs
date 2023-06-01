using JetBrains.Annotations;
using WebApplication1.ORM.Wrapper;

namespace WebApplication1.Attribute;

public class Table : System.Attribute
{
    [UsedImplicitly]
    public String? TableName { get; set; }

    public Table() {}

    public Table(String name)
    {
        TableName = name;
    }
}

[AttributeUsage(AttributeTargets.Property)]
public class Column : System.Attribute
{
    [UsedImplicitly]
    public String? ColumnName { get; set; }

    public Column() {}

    public Column(String name)
    {
        ColumnName = name;
    }
}

[AttributeUsage(AttributeTargets.Property)]
public class ID : System.Attribute
{ }