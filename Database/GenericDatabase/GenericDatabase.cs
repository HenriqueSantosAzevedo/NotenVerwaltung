using System;  
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using Microsoft.CSharp.RuntimeBinder;
using WebApplication1.Database;
using WebApplication1.Database.Connection;

//https://www.c-sharpcorner.com/article/cdataservice-a-generic-database-access-class/

public class GenericDatabase : IDatabase
{
      public DbConnection? Connection { get; set; }
      private string m_ConnectionString;
      private DbConnection m_Connection;
      public string ConnectionString {  
            get { return m_ConnectionString; }  
            set { m_ConnectionString = value; }  
      }
      
      public GenericDatabase() {}
      public GenericDatabase(string ConnectionString) {  
                  this.m_ConnectionString = ConnectionString;  
      }
      ~GenericDatabase() {  
            this.CloseDataSource();  
      }
      
      public DbConnection OpenDataSource() {
            //Test if SqlConnection exists  
            if (m_Connection == null) {
                  m_Connection = new SqlConnection(m_ConnectionString);
                  try
                  {
                        m_Connection.Open();
                  } catch (Exception e) {  
                        throw e;  
                  }
            } else if (m_Connection.State != ConnectionState.Open) {  
                  m_Connection.ConnectionString = m_ConnectionString;  
                  try {
                        m_Connection.Open();  
                  } catch (Exception e)
                  {
                        throw e;
                  }  
            }  
            return m_Connection;  
      }

      public TableDataStorage TableDataStorage { get; }

      public ConnectionState getConnectionStatus()
      {
            return m_Connection.State;
      }
      
      public void CloseDataSource() {  
            if (m_Connection != null) {  
                  if (m_Connection.State == ConnectionState.Open)  
                        m_Connection.Close();  
                  m_Connection = null;  
            }  
      }

      public Object genericSqlCommand(string QueryString, bool MustClose)
      {
            try {
                  SqlCommand command = new SqlCommand(QueryString, (SqlConnection) OpenDataSource());
                  return command.ExecuteNonQuery();
            } catch (System.Exception e) {  
                  throw e;  
            } finally
            {
                  if (MustClose) CloseDataSource();
            }
      }
      public DataSet SelectSqlData(string QueryString, string TableName, bool MustClose) {  
            DataSet ds = new DataSet();  
            SqlDataAdapter ad = new SqlDataAdapter();  
            try {  
                  ad.SelectCommand = new SqlCommand(QueryString, (SqlConnection) OpenDataSource());  
                  if (TableName.Trim().Length > 0) ad.Fill(ds, TableName);  
                  else ad.Fill(ds);  
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
            SqlDataAdapter da = new SqlDataAdapter();  
            try {  
                  da.InsertCommand = new SqlCommand(InsertString, (SqlConnection) OpenDataSource());  
                  da.InsertCommand.ExecuteNonQuery();  
            } catch (Exception e) {  
                  throw e;  
            } finally {  
                  if (MustClose) CloseDataSource();  
            }  
      }  
      public void InsertSqlData(string InsertString) {  
            InsertSqlData(InsertString, false);  
      }  
      public void DeleteSQLData(string DeleteString, bool MustClose) {  
            SqlDataAdapter da = new SqlDataAdapter();  
            try {  
                  da.DeleteCommand = new SqlCommand(DeleteString, (SqlConnection) OpenDataSource());  
                  da.DeleteCommand.ExecuteNonQuery();  
            } catch (Exception e) {  
                  throw e;  
            } finally {  
                  if (MustClose) CloseDataSource();  
            }  
      }  
      public void DeleteSQLData(string DeleteString) {  
            DeleteSQLData(DeleteString, false);  
      }  
      public void UpdateSQLData(string UpdateString, bool MustClose) {  
            SqlDataAdapter da = new SqlDataAdapter();  
            try {  
                  da.UpdateCommand = new SqlCommand(UpdateString, (SqlConnection) OpenDataSource());  
                  da.UpdateCommand.ExecuteNonQuery();  
            } catch (Exception e) {  
                  throw e;  
            } finally {  
                  if (MustClose) CloseDataSource();  
            }  
      }  
      public void UpdateSQLData(string UpdateString) {  
            UpdateSQLData(UpdateString, false);  
      }  
} //Class   