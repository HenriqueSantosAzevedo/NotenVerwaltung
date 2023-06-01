using System.Data;
using KlassenBester.Database.Exception;
using KlassenBester.Database.SqLiteDatabase;
using Newtonsoft.Json;

namespace WebApplication1.Database.Connection;

public class DatabaseService
{

    private static IDatabase _database;

    public DatabaseService(String connectionString, Boolean isSqlite)
    {
        if (isSqlite)
        {
            _database = new SQLite(connectionString);
            return;   
        }

        _database = new GenericDatabase(connectionString);
    }
    
    ~DatabaseService() {  
        _database.CloseDataSource();  
    }

    public static IDatabase GetDatabase()
    {
        return _database;
    }

    public void Open()
    {
        _database.OpenDataSource();
    }

    public void Init()
    {
        if (_database.getConnectionStatus() != ConnectionState.Open)
        {
            throw new ConnectionClosedException();
        }
        Migrator.Migrate();
    }
}