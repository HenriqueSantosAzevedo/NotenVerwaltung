using System.Data;
using System.Security.Cryptography;
using Newtonsoft.Json;
using NotenAppConsoleSchueler.Database.Connection;
using WebApplication1.Database.Connection;

namespace NotenAppConsoleSchueler.Migration;

public class Migrator
{
    class dbMigrationEntity
    {
        public String filename { get; set; }
        public String hash { get; set; }

        public dbMigrationEntity(String filename, String hash)
        {
            this.filename = filename;
            this.hash = hash;
        }
        
        public void save()
        {
            DatabaseService.GetDatabase().InsertSqlData($"insert into migration(filename, hash, createdAt) values ('{filename}', '{hash}', datetime('now'))");
        }
    }

    public static void Migrate()
    {
        IDatabase database = DatabaseService.GetDatabase();

        database.genericSqlCommand(
            "CREATE TABLE IF NOT EXISTS migration (filename varchar not null unique, hash varchar not null, createdAt datetime);",
            false
        );
        FileInfo[] scriptFiles = new DirectoryInfo(System.IO.Directory.GetCurrentDirectory() + "/scripts").GetFiles("*.sql");

        DataSet d = DatabaseService.GetDatabase().SelectSqlData("select filename, hash as createdAt from migration");
        List<dbMigrationEntity> entities = new List<dbMigrationEntity>();
        
        foreach (DataRow row in d.Tables[0].Rows)
        {
            string[] strings = row.ItemArray.Cast<string>().ToArray();
            entities.Add(new dbMigrationEntity(strings[0], strings[1]));
        }
        
        entities.ForEach(entity =>
        {
            FileInfo file = scriptFiles.First(info => info.Name == entity.filename);
            String hash = BitConverter.ToString(MD5.Create().ComputeHash(File.ReadAllBytes(file.FullName))).Replace("-", String.Empty).ToLower(); 
            
            if (hash != entity.hash)
            {
                throw new Exception($"{entity.filename} was modified and its hash doesnt correspond to {entity.hash}.");
            }
            scriptFiles = scriptFiles.Where(info => info.Name != entity.filename).ToArray();
        });
        
        scriptFiles.ToList().ForEach((FileInfo file) =>
        {
            String sqlQuery = new StreamReader(file.OpenRead()).ReadToEnd();
            String hash = BitConverter.ToString(MD5.Create().ComputeHash(File.ReadAllBytes(file.FullName))).Replace("-", String.Empty).ToLower();
            try
            {
                database.genericSqlCommand(sqlQuery, false);
                new dbMigrationEntity(file.Name, hash).save();
            }
            catch (Exception e)
            {
                throw e;
            }
            
        });
        
        
    }
}