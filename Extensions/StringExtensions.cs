using System.Text;

namespace GameUtils.Extensions;

public static class StringExtensions
{
    public static bool TryGet(this string str, int index, out char c)
    {
        c = '\0';

        if (index < 0 || index >= str.Length)
        {
            return false;
        }

        c = str[index];
        return true;
    }

    public static string Repeat(this string str, int count)
    {
        ArgumentNullException.ThrowIfNull(str);
        var result = new StringBuilder();

        for (var i = 0; i < count; i++)
        {
            result.Append(str);
        }

        return result.ToString();
    }
}
