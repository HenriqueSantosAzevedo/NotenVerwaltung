namespace NotenAppConsoleSchueler.Menu;

public class Modal
{
    private List<ModalOption> _list;
    private String _message;
    
    public bool InputIsValid(ConsoleKey input)
    {
        return _list.Any(option => option.IsKey(input));
    }

    public Modal(List<ModalOption> modalOptions, String message)
    {
        _list = modalOptions;
        _message = message;
    }
    
    public void Render()
    {
        var (posx, posy) = Console.GetCursorPosition();
        Console.WriteLine(_message);
        foreach (var option in _list)
        {
            option.Render();
        }
        
        ConsoleKey key = Console.ReadKey().Key;
        
        if (InputIsValid(key))
        {
            _list.First(option => option.IsKey(key)).Run();
            return;
        }
        
        Display.clearLine(0, Console.CursorTop);
        Console.SetCursorPosition(posx, posy);
        Render();
    }
}