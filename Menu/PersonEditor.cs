using NotenAppConsoleSchueler.ORM.Entity;
using NotenAppConsoleSchueler.ORM.Repository;
using NotenAppConsoleSchueler.ORM.Wrapper;
using NotenAppConsoleSchueler.ORM.Wrapper.Attribute;

namespace NotenAppConsoleSchueler.Menu;

[Repository.UsesRepository]
public class PersonEditor : Display
{
    [Repository] private static LehrerBaseRepository _lehrerBaseRepository;
    [Repository] private static SchuelerBaseRepository _schuelerBaseRepository;
    [Repository] private static AnmeldedatumRepostitory _anmeldedatumRepostitory;
    
    public override void Render()
    {
        Titel = "Neuen Benutzer hinzufuegen.";
        RenderHeader();
        Console.Write("Person Name: ");
        string fullname = Console.ReadLine()!;
        Console.Write("Benuzername: ");
        string username = Console.ReadLine()!;
        string password = StringUtils.Random(8);
        Console.WriteLine($"Das Passwort wurde automatisch generiert: {password}");
        string hashed = StringUtils.GetMD5Hash(password);
        
        List<ModalOption> modalOptions = new List<ModalOption>();
        modalOptions.Add(new ModalOption(ConsoleKey.S, "Schueler", () => SaveSchueler(fullname, username, hashed), ConsoleColor.Yellow));
        modalOptions.Add(new ModalOption(ConsoleKey.L, "Lehrer", () => SaveLehrer(), ConsoleColor.Magenta));
        Modal m = new Modal(modalOptions, "Waehlen sie zwischen Lehrer und Schueler");
        m.Render();
    }
    
    private void SaveSchueler(string fullname, string username, string password)
    {
        Anmeldedatum anmeldedatum = new Anmeldedatum();
        anmeldedatum.Benutzername = username;
        anmeldedatum.Password = password;
        _anmeldedatumRepostitory.save(anmeldedatum);
        Schueler schueler = new Schueler();
        schueler.Name = fullname;
        schueler.Anmeldedatum = anmeldedatum;
        _schuelerBaseRepository.save(schueler);
    }
    
    private void SaveLehrer()
    {
        
    }
}