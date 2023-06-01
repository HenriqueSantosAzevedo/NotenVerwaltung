using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using NotenAppConsoleSchueler.ORM.Repository;
using WebApplication1.Database;
using WebApplication1.Database.Connection;

namespace KlassenBester.Database.SqLiteDatabase;

public class SQLite : IDatabase
{
    
    public DbConnection? Connection { get; set; }
    
    protected String connectionString = "";
    
    public SQLite(String connectionString)
    {
        this.connectionString = connectionString;
    }
    
    public DbConnection OpenDataSource()
    {
        if (Connection == null)
        {
            if (connectionString.Length == 0)
                throw new System.Exception("Connection string can't be empty");
            
            Connection = new SQLiteConnection(connectionString);
            try
            {
                Connection.Open();
            }
            catch (System.Exception e)
            {
                throw e;
            }
            
        } else if (Connection.State != ConnectionState.Open) {  
            Connection.ConnectionString = connectionString;  
            try {
                Connection.Open();  
            } catch (System.Exception e)
            {
                throw e;
            }  
        }

        return Connection;
    }
    
    public TableDataStorage TableDataStorage { get => new SQLiteTableDataStorage(); }

    public ConnectionState getConnectionStatus()
    {
        if (Connection == null)
            return ConnectionState.Closed;
        return Connection!.State;
    }
    
    public void CloseDataSource() {  
        if (Connection != null) {  
            if (Connection.State == ConnectionState.Open)  
                Connection.Close();  
            Connection = null;  
        }  
    }
    
    public Object genericSqlCommand(string QueryString, bool MustClose)
    {
        try {
            SQLiteCommand command = new SQLiteCommand(QueryString, (SQLiteConnection) OpenDataSource());
            return command.ExecuteScalar();
        } catch (System.Exception e) {  
            throw e;  
        } finally {  
            if (MustClose) CloseDataSource();
        }
        return null;
    }
    
    public DataSet SelectSqlData(string QueryString, string TableName, bool MustClose) {  
        DataSet ds = new DataSet();  
        SQLiteDataAdapter da = new SQLiteDataAdapter();  
        try {
            da.SelectCommand = new SQLiteCommand(QueryString, (SQLiteConnection) OpenDataSource());  
            if (TableName.Trim().Length > 0) da.Fill(ds, TableName);  
            else da.Fill(ds);  
        } catch (System.Exception e) {  
            throw e;  
        } finally {  
            if (MustClose) CloseDataSource();  
        }
        return ds;  
    }
    
    public DataSet SelectSqlData(string QueryString, string TableName) {  
        return SelectSqlData(QueryString, TableName, false);  
    }  
    
    public DataSet SelectSqlData(string QueryString) {  
        return SelectSqlData(QueryString, "", false);  
    }
    
    public void InsertSqlData(string InsertString, bool MustClose) {  
        SQLiteDataAdapter da = new SQLiteDataAdapter();    
        try {  
            da.InsertCommand = new SQLiteCommand(InsertString, (SQLiteConnection) OpenDataSource());  
            da.InsertCommand.ExecuteNonQuery();  
        } catch (System.Exception e) {  
            throw e;  
        } finally {  
            if (MustClose) CloseDataSource();  
        }  
    }  
    public void InsertSqlData(string InsertString) {  
        InsertSqlData(InsertString, false);  
    }
    
    public void DeleteSQLData(string DeleteString, bool MustClose) {  
        SQLiteDataAdapter da = new SQLiteDataAdapter();  
        try {  
            da.DeleteCommand = new SQLiteCommand(DeleteString, (SQLiteConnection) OpenDataSource());  
            da.DeleteCommand.ExecuteNonQuery();  
        } catch (System.Exception e) {  
            throw e;  
        } finally {  
            if (MustClose) CloseDataSource();  
        }  
    }  
    public void DeleteSQLData(string DeleteString) {  
        DeleteSQLData(DeleteString, false);  
    }
    
    public void UpdateSQLData(string UpdateString, bool MustClose) {  
        SQLiteDataAdapter da = new SQLiteDataAdapter();  
        try {  
            da.UpdateCommand = new SQLiteCommand(UpdateString, (SQLiteConnection) OpenDataSource());  
            da.UpdateCommand.ExecuteNonQuery();  
        } catch (System.Exception e) {  
            throw e;  
        } finally {  
            if (MustClose) CloseDataSource();  
        }  
    }  
    public void UpdateSQLData(string UpdateString) {  
        UpdateSQLData(UpdateString, false);  
    }  
}