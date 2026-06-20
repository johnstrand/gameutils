namespace GameUtils.Extensions;

/// <summary>
/// Extension methods for strings.
/// </summary>
public static class StringExtensions
{
    /// <summary>
    /// Tries to get a character at the specified index. Returns false if the index is out of bounds.
    /// </summary>
    public static bool TryGet(this string str, int index, out char c)
    {
        ArgumentNullException.ThrowIfNull(str);
        c = '\0';

        if (index < 0 || index >= str.Length)
        {
            return false;
        }

        c = str[index];
        return true;
    }

    /// <summary>
    /// Repeats a string a specified number of times.
    /// </summary>
    public static string Repeat(this string str, int count)
    {
        ArgumentNullException.ThrowIfNull(str);
        ArgumentOutOfRangeException.ThrowIfNegative(count);
        return string.Concat(Enumerable.Repeat(str, count));
    }
}
