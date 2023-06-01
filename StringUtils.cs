using System.Linq.Expressions;

namespace NotenAppConsoleSchueler.ORM.Wrapper;

public class StringUtils
{
    public static string Random(int length = 8, string chooseFrom = "abcdefghijklmnopqrstuvwxyz")
    {
        Random r = new Random();
        return String.Join(
            "",
            new Array[length].Select(a =>
            {
                List<Expression<Func<char, string>>> l = new List<Expression<Func<char, string>>>
                {
                    (t) => t.ToString(),
                    (t) => t.ToString().ToUpper()
                };
                return l[r.Next(2)].Compile(true).Invoke(chooseFrom[r.Next(chooseFrom.Length)]);
            }).ToList()
        );
    }
    
    public static string format(string val, int size, )

}