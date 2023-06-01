using WebApplication1.ORM.Wrapper;

namespace WebApplication1.Database;

public abstract class TableDataStorage
{
    public static Dictionary<Type, List<StorageItem>> StorageItems = new();
    
    public abstract List<StorageItem> StorageItemFor(ParsedEntity entity);
}

public class StorageItem
{
    public String Field { get; set; }
    public String Type { get; set; }
}