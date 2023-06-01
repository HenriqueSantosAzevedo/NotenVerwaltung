using NotenAppConsoleSchueler.ORM.Wrapper;

namespace NotenAppConsoleSchueler.ORM.Repository;

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