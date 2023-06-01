using System.Security.Cryptography;
using System.Text;
using NotenAppConsoleSchueler.Database.Exception;
using NotenAppConsoleSchueler.ORM.Entity;
using NotenAppConsoleSchueler.ORM.Repository;
using NotenAppConsoleSchueler.ORM.Wrapper.Attribute;

namespace NotenAppConsoleSchueler.Menu;

[Repository.UsesRepository]
public class LoginView : Display
{
    [Repository] private static AnmeldedatumRepostitory anmeldedatumRepostitory;
    [Repository] private static LehrerBaseRepository lehrerBaseRepository;
    [Repository] private static SchuelerBaseRepository schuelerBaseRepository;

    
    public override void Render()
    {
        Titel = "Bitte Anmelden";
        RenderHeader();
        
        Console.Write("Benuzername: ");
        string username = Console.ReadLine()!;
        
        Console.Write("Enter your password: ");
        string password = GetPassword();
        
        Console.Write(password);

        Anmeldedatum anmeldedaten;
        try
        {
            anmeldedaten = anmeldedatumRepostitory
                .FindWhere($"benutzername = '?' and password_hash = '?'", username, password).First();
        }
        catch (SQLInjectionException)
        {
            Console.Error.WriteLine("Bitte benutzen Sie nur gueltige zeichen!");
            Thread.Sleep(1950);
            Render();
            return;
        }
        catch (InvalidOperationException)
        {
            Console.Error.WriteLine("Benutzer konnte nicht gefunden werden.");
            Thread.Sleep(5000);
            Render();
            return;
        }

        List<Lehrer> l = lehrerBaseRepository.FindAll();
        List<Schueler> s = schuelerBaseRepository.FindAll();
        if (l.Any(lehrer => lehrer.Anmeldedatum.ID == anmeldedaten.ID))
        {
            new LehrerView(l.First(lehrer => lehrer.Anmeldedatum.ID == anmeldedaten.ID)).Render();
        } else if (s.Any(schueler => schueler.Anmeldedatum.ID == anmeldedaten.ID))
        {
            new SchuelerView(s.First(schueler => schueler.Anmeldedatum.ID == anmeldedaten.ID)).Render();
        }
        Render();
    }
    
    static string GetPassword()
    {
        string password = "";
        ConsoleKeyInfo key;
        do
        {
            key = Console.ReadKey(true);
            // Append the pressed key to the password string
            if (key.Key != ConsoleKey.Enter && key.Key != ConsoleKey.Backspace)
            {
                password += key.KeyChar;
                Console.Write("*"); // Mask the input with an asterisk
            }
            else if (key.Key == ConsoleKey.Backspace && password.Length > 0)
            {
                // Handle backspace by removing the last character from the password
                password = password.Substring(0, password.Length - 1);
                Console.Write("\b \b"); // Erase the character from the console
            }
        } while (key.Key != ConsoleKey.Enter);

        Console.WriteLine(); // Move to the next line after pressing Enter
        password = GetMD5Hash(password);
        return password;
    }
    
    static string GetMD5Hash(string input)
    {
        using (MD5 md5 = MD5.Create())
        {
            byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            StringBuilder builder = new StringBuilder();

            for (int i = 0; i < hashBytes.Length; i++)
            {
                builder.Append(hashBytes[i].ToString("x2"));
            }

            return builder.ToString();
        }
    }
}