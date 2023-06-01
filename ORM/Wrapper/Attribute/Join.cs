using JetBrains.Annotations;
using WebApplication1.Database;
using WebApplication1.Entitiy;
using WebApplication1.ORM.Wrapper;

namespace WebApplication1.Attribute;

public enum ReferencedBy
{
    CHILD, THIS
}


[AttributeUsage(AttributeTargets.Property)]
public abstract class Join : System.Attribute
{
    [UsedImplicitly]
    public bool ReturnsList;

    [UsedImplicitly]
    public Type JoinType { get; set; }
    
    [UsedImplicitly]
    public String JoinOn { get; set; }

    protected Join(bool returnsList = false)
    {
        ReturnsList = returnsList;
    }

    public abstract string ToString(ParsedEntity left, ParsedEntity right);
}

public class ManyToMany : Join
{
    public string table, abr;

    public ManyToMany(string table) : base(true)
    {
        this.table = table;
        abr = StringUtils.Random();
    }

    public override string ToString(ParsedEntity left, ParsedEntity right)
    {
        return $"left join {table} as {abr} on {abr}.{left.ID.ColumnName?.ToLower()} = {left.Abbreviation}.{left.ID.ColumnName?.ToLower()}\n" +
               $"left join {right.TableName} as {right.Abbreviation} on {abr}.{right.ID.ColumnName?.ToLower()} = {right.Abbreviation}.{right.ID.ColumnName?.ToLower()}";
    }
}

public class OneToMany : Join
{
    public OneToMany(): base(true)
    {}
    
    public OneToMany(string primary = "", string foreign = "") : base(true)
    {
        Primary = primary.Trim().ToLower();
        Foreign = foreign.Trim().ToLower();
    }

    private string? Primary { get; set; }
    private string? Foreign { get; set; }
    
    public override string ToString(ParsedEntity left, ParsedEntity right)
    {
        return $"left join {right.TableName} as {right.Abbreviation} on {left.Abbreviation}.{(Primary?.Trim().Length > 0 ? Primary : left.ID.ColumnName)?.ToLower()} = {right.Abbreviation}.{Foreign.ToLower()}";
    }
}

public class OneToOne: Join 
{
    public OneToOne(string primary = "", string foreign = "", ReferencedBy referencedBy = Attribute.ReferencedBy.CHILD)
    {
        Primary = primary.Trim().ToLower();
        Foreign = foreign.Trim().ToLower();
        if (foreign.Trim().Length == 0)
            Foreign = Primary;
        ReferencedBy = referencedBy;
    }

    public string? Primary { get; set; }
    public string? Foreign { get; set; }
    public ReferencedBy? ReferencedBy { get; set; }

    public override string ToString(ParsedEntity left, ParsedEntity right)
    {
        if (Primary == Foreign)
            return $"left join {right.TableName} as {right.Abbreviation} using ({Primary})";

        return $"left join {right.TableName} as {right.Abbreviation}" +
               " on " +
               $"{left.Abbreviation}.{(Foreign?.Trim().Length > 0 ? Foreign : right.ID.ColumnName)?.ToLower()}" +
               " = " +
               $"{right.Abbreviation}.{(Primary?.Trim().Length > 0 ? Primary : left.ID.ColumnName)?.ToLower()}";
    }
}