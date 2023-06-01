using NotenAppConsoleSchueler.ORM.Entity;
using NotenAppConsoleSchueler.ORM.Repository;
using NotenAppConsoleSchueler.ORM.Wrapper;
using NotenAppConsoleSchueler.ORM.Wrapper.Attribute;

namespace NotenAppConsoleSchueler.Menu;

[Repository.UsesRepository]
public class SchuelerListMenu : BaseMenu
{
    [Repository] private static SchuelerBaseRepository? _schuelerRepository;
    
    private List<Schueler> _list = new();

    public SchuelerListMenu()
    {
        if (_schuelerRepository == null)
            throw new RepositoryNotLoadedException();
        _list = _schuelerRepository.FindAll();
    }

    
    public override void Render()
    {
        Console.Clear();
        Console.WriteLine("Waehlen Sie ein schueler aus, dessen Noten Sie sehen moechten.");

        int i = 0;
        for (; i < GetOptionList().Count; i++)
        {
            Console.WriteLine($"{i}: {GetOptionList()[i]}");
        }
        Console.WriteLine($"{i}: Neu laden");
        
        Console.WriteLine("Ihre auswahl: ");
        string resp = Console.ReadLine()!;
        bool isValid = InputIsValid(resp, i);
        
        if (isValid)
        {
            OnResponse(Convert.ToInt32(resp));
            Render();
        }
        else
        {
            string errorMessage = "Ihre auswahl war fehlerhaft!";
            if (GetOptionList().Count > 1)
                errorMessage += $"Bitte ein zahl zwischen 0 und {GetOptionList().Count()}";
            Console.Error.WriteLine(errorMessage);
            Thread.Sleep(750);
            Render();
        }
    }

    public override void OnResponse(int responseIndex)
    {
        if (responseIndex < _list.Count)
        {
            SchuelerView schuelerView = new SchuelerView(_list[responseIndex]);
            schuelerView.Render();
            return;
        }
        _list = _schuelerRepository?.FindAll()!;
        Render();
    }

    public override List<string> GetOptionList()
    {
        return _list.Select(schueler => schueler.Name).Where(schueler => schueler is not null).ToList()!;
    }
}