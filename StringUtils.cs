using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Text;

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

    public static string format(string val, int size, StringPosition stringPosition = StringPosition.START, string endWith = "")
    {
        if (val.Length >= size) return val.Substring(0, size) + endWith;
        int toFill = size - val.Length;
        if (stringPosition == StringPosition.START)
        {
            return val + String.Join(" ", new String[toFill]) + endWith;
        }
        return String.Join(" ", new String[toFill]) + val + endWith;
    }
    
    public static string ReplaceFirst(string text, string search, string replace)
    {
        int pos = text.IndexOf(search);
        if (pos < 0)
        {
            return text;
        }
        return text.Substring(0, pos) + replace + text.Substring(pos + search.Length);
    }
    
    public static string GetMD5Hash(string input)
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

public enum StringPosition
{
    START, END
}