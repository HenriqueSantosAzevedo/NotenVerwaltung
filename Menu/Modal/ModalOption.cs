namespace NotenAppConsoleSchueler.Menu;

public class ModalOption
{
    private readonly ConsoleKey _key;
    private readonly ConsoleColor _foreground;
    private readonly string _message;
    private readonly Action _lambda;

    public bool IsKey(ConsoleKey input)
    {
        return input == _key;
    }

    public ModalOption(
        ConsoleKey key,
        String message,
        Action lambda,
        ConsoleColor consoleColor = ConsoleColor.White
    )
    {
        _key = key;
        _foreground = consoleColor;
        _message = message;
        _lambda = lambda;
    }

    public void Render()
    {
        Console.ResetColor();
        Console.Write("[ ");
        Console.ForegroundColor = _foreground;
        Console.Write(_key.ToString());
        Console.ResetColor();
        Console.Write(" ]  ");
        Console.WriteLine(_message);
    }

    public void Run()
    {
        _lambda.Invoke();
    }
}