using System.Data;
using WebApplication1.Database;
using WebApplication1.Database.Connection;
using WebApplication1.ORM.Wrapper;

namespace KlassenBester.Database.SqLiteDatabase;

public class SQLiteTableDataStorage : TableDataStorage
{
    public override List<StorageItem> StorageItemFor(ParsedEntity entity)
    {
        if (!StorageItems.ContainsKey(entity.Type))
        {
            List<StorageItem> stItems = DatabaseService.GetDatabase()
                .SelectSqlData(
                    $"SELECT name, lower(CASE WHEN instr(type, ' ') > 0 THEN substr(type, 1, instr(type, ' ') - 1) ELSE type END) AS type FROM pragma_table_info('{entity.TableName}');"
                )
                .Tables[0]
                .Rows
                .Cast<DataRow>()
                .Select(row => new StorageItem()
                {
                    Field = (string)row[0],
                    Type = (string)row[1]
                })
                .ToList();

            StorageItems[entity.Type] = stItems;
        }
        return StorageItems[entity.Type];
    }
}