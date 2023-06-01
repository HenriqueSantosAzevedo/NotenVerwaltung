namespace NotenAppConsoleSchueler.Menu;

public abstract class BaseMenu
{
    public String Titel;
    
    public abstract List<String> GetOptionList();
    public abstract void Render();
    public abstract void OnResponse(int responseIndex);

    public bool InputIsValid(String input, int max)
    {
        try
        {
            Int32 i = Convert.ToInt32(input);
            if (i > max || i < 0) return false;
        } catch (FormatException)
        {
            return false;
        }
        return true;
    }
}