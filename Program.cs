using NotenAppConsoleSchueler.Database.Connection;
using NotenAppConsoleSchueler.Menu;
using NotenAppConsoleSchueler.ORM.Wrapper;
using NotenAppConsoleSchueler.ORM.Wrapper.Attribute;

class Program
{
    public static void Main(String[] args)
    {
        DatabaseService ds = new DatabaseService("data source=database.sqlite", true);
        ds.Open();
        ds.Init();

        RepositoryHolder.load();
        Repository.SetRepositories();
        
        new LoginView().Render();
    }
}