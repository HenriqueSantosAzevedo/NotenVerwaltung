using NotenAppConsoleSchueler.ORM.Entity;

namespace NotenAppConsoleSchueler.Menu;

public abstract class Display
{
    public static void clearLine(int posx, int posy, ConsoleColor consoleColor = ConsoleColor.Black, string v = " ")
    {
        Console.SetCursorPosition(posx, posy);
        Console.BackgroundColor = consoleColor;
        if (consoleColor == ConsoleColor.Black)
        {
            Console.ResetColor();
        }
        
        String[] r = new String[Console.WindowWidth];
        Array.Fill(r, v);
        Console.WriteLine(String.Join("", r));
    }
    
    protected String Titel;

    public abstract void Render();
    
    public void RenderHeader()
    {
        Console.Clear();
        clearLine(0, 0, ConsoleColor.White);
        Console.ForegroundColor = ConsoleColor.Black;
        Console.SetCursorPosition(1, 0);
        Console.WriteLine(Titel);
        Console.ResetColor();
    }
}