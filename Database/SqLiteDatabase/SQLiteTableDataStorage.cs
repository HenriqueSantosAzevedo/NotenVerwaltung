using System.Data;
using NotenAppConsoleSchueler.Database.Connection;
using NotenAppConsoleSchueler.ORM.Repository;
using NotenAppConsoleSchueler.ORM.Wrapper;
using WebApplication1.Database;
using WebApplication1.Database.Connection;

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