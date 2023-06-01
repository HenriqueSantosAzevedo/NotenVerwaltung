using System.Data;
using System.Data.Common;
using NotenAppConsoleSchueler.ORM.Repository;

namespace WebApplication1.Database.Connection;

public interface IDatabase
{
    public DbConnection OpenDataSource();
    
    public DbConnection? Connection { get; set; }

    public TableDataStorage TableDataStorage { get; }

    public void CloseDataSource();

    public ConnectionState getConnectionStatus();

    public Object genericSqlCommand(string QueryString, bool MustClose);
    public DataSet SelectSqlData(string QueryString, string TableName, bool MustClose);

    public DataSet SelectSqlData(string QueryString, string TableName);
    
    public DataSet SelectSqlData(string QueryString);

    public void UpdateSQLData(string UpdateString);

    public void UpdateSQLData(string UpdateString, bool MustClose);

    public void DeleteSQLData(string DeleteString);

    public void DeleteSQLData(string DeleteString, bool MustClose);

    public void InsertSqlData(string InsertString);

    public void InsertSqlData(string InsertString, bool MustClose);
}