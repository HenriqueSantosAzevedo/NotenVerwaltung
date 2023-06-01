using Newtonsoft.Json;
using NotenAppConsoleSchueler.ORM.Entity;
using NotenAppConsoleSchueler.ORM.Wrapper;

namespace NotenAppConsoleSchueler.Menu;

public class SchuelerView : Display
{
    private Schueler _schueler;
    
    public SchuelerView(Schueler schueler)
    {
        _schueler = schueler;
        Titel = $"Name: {_schueler.Name}"!;
    }

    public override void Render()
    {
        RenderHeader();
        string bd = $"Geburtstag: {_schueler.Geburtstag.ToString("dd. MMM yyyy")}";

        Console.CursorTop = 0;
        Console.CursorLeft = Console.WindowWidth - bd.Length - 2;
        Console.BackgroundColor = ConsoleColor.White;
        Console.ForegroundColor = ConsoleColor.Black;
        Console.WriteLine(bd);
        Console.ResetColor();
        List<ModalOption> modalOptions = new List<ModalOption>();

        if (_schueler.Klassen.Count == 0)
        {
            Console.WriteLine($"{_schueler.Name} ist in keiner Klasse.");
        }

        if (_schueler.Klassen.Count > 0)
        {
            modalOptions.Add(new ModalOption(ConsoleKey.N, "Noten Anzeigen", RenderNoten, ConsoleColor.Green));
            modalOptions.Add(new ModalOption(ConsoleKey.K, "Kurse Anzeigen", RenderKurse, ConsoleColor.DarkCyan));
        }


        modalOptions.Add(new ModalOption(ConsoleKey.X, "Zurueck", () => {}, ConsoleColor.Red));
        new Modal(modalOptions, "").Render();
    }

    public void RenderKurse()
    {
        RenderHeader();

        _schueler.Klassen.ForEach(klasse =>
        {
            clearLine(0, Console.CursorTop, v: "-");
            Console.SetCursorPosition(3, Console.CursorTop - 1);
            Console.WriteLine($"[In klasse: {klasse.KlassenName}]");
            klasse.Kurse.ForEach(kurs =>
            {
                Console.WriteLine($"{kurs.KursName} mit {kurs.Lehrer.Name}"); 
            });
        });
        
        List<ModalOption> modalOptions = new List<ModalOption>();
        modalOptions.Add(new ModalOption(ConsoleKey.X, "Zurueck", Render, ConsoleColor.Red));
        new Modal(modalOptions, "").Render();
    }

    private void RenderNoten()
    {
        RenderHeader();
        _schueler.Klassen.ForEach(klasse =>
        {
            Console.WriteLine(klasse.KlassenName);
            
            Console.Write(StringUtils.format("Kurs", 32, endWith: "| "));
            Console.Write(StringUtils.format("Note", 16, endWith: "| "));
            Console.Write(StringUtils.format("Noten Art", 32, endWith: "| "));
            Console.Write(StringUtils.format("Datum", 24, endWith: "| "));
            Console.Write(StringUtils.format("Notiz", 64, endWith: "| "));
            Console.WriteLine();
            
            klasse.Kurse.ForEach(kurs =>
            {
                clearLine(0, Console.CursorTop, v: "-");
                kurs.Leistungen.Select(leistung =>
                    StringUtils.format($"{kurs.KursName}({kurs.Lehrer.Name})", 32, endWith: "| ") +
                    StringUtils.format(leistung.Note.NotenWert.ToString(), 16, endWith: "| ") +
                    StringUtils.format(leistung.NotenArt.Bezeichnung, 32, endWith: "| ") +
                    StringUtils.format(leistung.Datum.ToString("dd. MM yyyy"), 24, endWith: "| ") +
                    StringUtils.format(leistung.Notiz, 64, endWith: "| ")
                )
                    .ToList()
                    .ForEach(Console.WriteLine);
            });
        });
        
        List<ModalOption> modalOptions = new List<ModalOption>();
        modalOptions.Add(new ModalOption(ConsoleKey.X, "Zurueck", Render, ConsoleColor.Red));
        new Modal(modalOptions, "").Render();
    }
}