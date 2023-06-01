namespace WebApplication1.Attribute;

[AttributeUsage(AttributeTargets.Property)]
public class Priority : System.Attribute
{
    public int Amount { get; set; }
    public Priority(int priority = 0)
    {
        Amount = priority;
    }
}