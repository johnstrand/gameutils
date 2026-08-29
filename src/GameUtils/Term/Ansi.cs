using GameUtils.Extensions;
using System.Text;

namespace GameUtils.Term;

/// <summary>
/// Utility class for ANSI escape sequences.
/// </summary>
public static class Ansi
{
    /// <summary>
    /// The ANSI sequence start character.
    /// </summary>
    public const string SEQUENCE_START = "\u001b[";

    /// <summary>
    /// Helper method to create an ANSI sequence.
    /// </summary>
    public static string CreateSequence(string type, params object[] codes)
    {
        return SEQUENCE_START + string.Join(";", codes) + type;
    }

    /// <summary>
    /// Resets all ANSI formatting.
    /// </summary>
    public static string Reset => CreateSequence("m", 0);

    /// <summary>
    /// Formats text as bold.
    /// </summary>
    public static string Bold => CreateSequence("m", 1);

    /// <summary>
    /// Formats text as faint.
    /// </summary>
    public static string Faint => CreateSequence("m", 2);

    /// <summary>
    /// Formats text as italic.
    /// </summary>
    public static string Italic => CreateSequence("m", 3);

    /// <summary>
    /// Formats text as underlined.
    /// </summary>
    public static string Underline => CreateSequence("m", 4);

    private static readonly Dictionary<string, int> _foregroundColors = new()
    {
        ["black"] = 30,
        ["red"] = 31,
        ["green"] = 32,
        ["yellow"] = 33,
        ["blue"] = 34,
        ["magenta"] = 35,
        ["cyan"] = 36,
        ["white"] = 37,
        ["bright-black"] = 90,
        ["bright-red"] = 91,
        ["bright-green"] = 92,
        ["bright-yellow"] = 93,
        ["bright-blue"] = 94,
        ["bright-magenta"] = 95,
        ["bright-cyan"] = 96,
        ["bright-white"] = 97,
    };

    private static readonly Dictionary<string, int> _backgroundColors = new()
    {
        ["black"] = 40,
        ["red"] = 41,
        ["green"] = 42,
        ["yellow"] = 43,
        ["blue"] = 44,
        ["magenta"] = 45,
        ["cyan"] = 46,
        ["white"] = 47,
        ["bright-black"] = 100,
        ["bright-red"] = 101,
        ["bright-green"] = 102,
        ["bright-yellow"] = 103,
        ["bright-blue"] = 104,
        ["bright-magenta"] = 105,
        ["bright-cyan"] = 106,
        ["bright-white"] = 107,
    };

    /// <summary>
    /// Sets the foreground color to a named color.
    /// </summary>
    public static string Foreground(string color)
    {
        return CreateSequence("m", _foregroundColors[color]);
    }

    /// <summary>
    /// Sets the foreground color to an RGB color.
    /// </summary>
    public static string Foreground(byte r, byte g, byte b)
    {
        return CreateSequence("m", 38, 2, r, g, b);
    }

    /// <summary>
    /// Sets the background color to a named color.
    /// </summary>
    public static string Background(string color)
    {
        return CreateSequence("m", _backgroundColors[color]);
    }

    /// <summary>
    /// Sets the background color to an RGB color.
    /// </summary>
    public static string Background(byte r, byte g, byte b)
    {
        return CreateSequence("m", 48, 2, r, g, b);
    }

    /// <summary>
    /// Moves the cursor up by the specified amount.
    /// </summary>
    public static void CursorUp(int amount = 1)
    {
        Console.Write(CreateSequence("A", amount));
    }

    /// <summary>
    /// Moves the cursor down by the specified amount.
    /// </summary>
    /// <param name="amount"></param>
    public static void CursorDown(int amount = 1)
    {
        Console.Write(CreateSequence("B", amount));
    }

    /// <summary>
    /// Moves the cursor forward (right) by the specified amount.
    /// </summary>
    public static void CursorForward(int amount = 1)
    {
        Console.Write(CreateSequence("C", amount));
    }

    /// <summary>
    /// Moves the cursor backward (left) by the specified amount.
    /// </summary>
    public static void CursorBack(int amount = 1)
    {
        Console.Write(CreateSequence("D", amount));
    }

    /// <summary>
    /// Moves the cursor down n lines.
    /// </summary>
    /// <param name="amount"></param>
    public static void CursorNextLine(int amount = 1)
    {
        Console.Write(CreateSequence("E", amount));
    }

    /// <summary>
    /// Moves the cursor up n lines.
    /// </summary>
    public static void CursorPreviousLine(int amount = 1)
    {
        Console.Write(CreateSequence("F", amount));
    }

    /// <summary>
    /// Sets the cursor X position.
    /// </summary>
    public static void CursorHorizontalAbsolute(int column)
    {
        Console.Write(CreateSequence("G", column));
    }

    /// <summary>
    /// Sets the cursor position.
    /// </summary>
    public static void CursorPosition(int row, int column)
    {
        Console.Write(CreateSequence("H", row, column));
    }

    /// <summary>
    /// Clears the screen. Mode 0 clears from cursor to end of screen, mode 1 clears from cursor to beginning of screen, mode 2 (default) clears entire screen.
    /// </summary>
    /// <param name="mode"></param>
    public static void Clear(int mode = 2)
    {
        Console.Write(CreateSequence("J", mode));
    }

    /// <summary>
    /// Clears the line. Mode 0 clears from cursor to end of line, mode 1 clears from cursor to beginning of line, mode 2 (default) clears entire line.
    /// </summary>
    public static void ClearLine(int mode = 2)
    {
        Console.Write(CreateSequence("K", mode));
    }

    /// <summary>
    /// Scrolls the screen up by the specified amount.
    /// </summary>
    public static void ScrollUp(int amount = 1)
    {
        Console.Write(CreateSequence("S", amount));
    }

    /// <summary>
    /// Scrolls the screen down by the specified amount.
    /// </summary>
    /// <param name="amount"></param>
    public static void ScrollDown(int amount = 1)
    {
        Console.Write(CreateSequence("T", amount));
    }

    /// <summary>
    /// Writes text to the console, formatting it with ANSI escape sequences. Check documentation for <see cref="Format(string)"/> for more information.
    /// </summary>
    public static string Write(string text)
    {
        var formatted = Format(text);
        Console.Write(formatted);
        return formatted;
    }

    /// <summary>
    /// Writes text to the console, formatting it with ANSI escape sequences. Check documentation for <see cref="Format(string)"/> for more information.
    /// </summary>
    public static string Write(string text, params object[] args)
    {
        var formatted = Format(args != null && args.Length > 0 ? string.Format(text, args) : text);
        Console.Write(formatted);
        return formatted;
    }

    /// <summary>
    /// Writes a line to the console, formatting it with ANSI escape sequences. Check documentation for <see cref="Format(string)"/> for more information.
    /// </summary>
    public static string WriteLine(string text)
    {
        var formatted = Format(text);
        Console.WriteLine(formatted);
        return formatted;
    }

    /// <summary>
    /// Writes a line to the console, formatting it with ANSI escape sequences. Check documentation for <see cref="Format(string)"/> for more information.
    /// </summary>
    public static string WriteLine(string text, params object[] args)
    {
        var formatted = Format(args != null && args.Length > 0 ? string.Format(text, args) : text);
        Console.WriteLine(formatted);
        return formatted;
    }

    /// <summary>
    /// Formats text with ANSI escape sequences. The following sequences are supported:
    /// <list type="bullet">
    /// <item>[(color)] or [fg:(color)] - Sets the foreground to the named color</item>
    /// <item>[#(r,g,b)] or [#fg:(r,g,b)] - Sets the foreground to the RGB color (valid range is 0-255, inclusive)</item>
    /// <item>[bg:(color)] - Sets the background to the named color</item>
    /// <item>[#bg:(r,g,b)] - Sets the background to the RGB color</item>
    /// <item>[b] or [bold] - Sets the formatting to bold</item>
    /// <item>[i] or [italic] - Sets the formatting to italic</item>
    /// <item>[f] or [faint] - Sets the formatting to faint</item>
    /// <item>[u] or [underline] - Sets the formatting to underlined</item>
    /// <item>[] or [/] - Resets all formatting (future version might only reset the last formatting and make clear an explicit command)</item>
    /// </list>
    /// </summary>
    /// <remarks>
    /// This whole thing is a candidate for a rewrite. It's a bit of a mess.
    /// </remarks>
    public static string Format(string text)
    {
        var result = new StringBuilder();
        var span = text.AsSpan();
        var i = 0;

        while (i < span.Length)
        {
            var c = span[i];

            if (c != '[')
            {
                result.Append(c);
                i++;
                continue;
            }

            if (i + 1 < span.Length && span[i + 1] == '[')
            {
                result.Append('[');
                i += 2;
                continue;
            }

            var endOfSequence = span[i..].IndexOf(']');

            if (endOfSequence == -1)
            {
                throw new ArgumentException($"Invalid ANSI sequence starting at position {i}");
            }

            endOfSequence += i;
            var sequence = span.Slice(i + 1, endOfSequence - i - 1);
            i = endOfSequence + 1;

            ProcessSequence(sequence, result, i);
        }

        return result.ToString();
    }

    private static void ProcessSequence(ReadOnlySpan<char> sequence, StringBuilder result, int positionAfterSequence)
    {
        if (TryProcessResetOrStyle(sequence, result))
        {
            return;
        }

        if (TryProcessColorSequence(sequence, result, positionAfterSequence))
        {
            return;
        }

        throw new ArgumentException($"Unknown ANSI sequence '{sequence.ToString()}' starting at position {positionAfterSequence}");
    }

    private static bool TryProcessResetOrStyle(ReadOnlySpan<char> sequence, StringBuilder result)
    {
        if (sequence.Equals("/", StringComparison.Ordinal) || sequence.IsEmpty)
        {
            result.Append(Reset);
            return true;
        }

        if (sequence.Equals("bold", StringComparison.Ordinal) || sequence.Equals("b", StringComparison.Ordinal))
        {
            result.Append(Bold);
            return true;
        }

        if (sequence.Equals("faint", StringComparison.Ordinal) || sequence.Equals("f", StringComparison.Ordinal))
        {
            result.Append(Faint);
            return true;
        }

        if (sequence.Equals("italic", StringComparison.Ordinal) || sequence.Equals("i", StringComparison.Ordinal))
        {
            result.Append(Italic);
            return true;
        }

        if (sequence.Equals("underline", StringComparison.Ordinal) || sequence.Equals("u", StringComparison.Ordinal))
        {
            result.Append(Underline);
            return true;
        }

        return false;
    }

    private static bool TryProcessColorSequence(ReadOnlySpan<char> sequence, StringBuilder result, int positionAfterSequence)
    {
        if (sequence.StartsWith("fg:", StringComparison.Ordinal))
        {
            var colorSpan = sequence[3..];
            if (TryGetNamedColor(_foregroundColors, colorSpan, out var colorCode))
            {
                result.Append(CreateSequence("m", colorCode));
                return true;
            }
            return false;
        }

        if (sequence.StartsWith("bg:", StringComparison.Ordinal))
        {
            var colorSpan = sequence[3..];
            if (TryGetNamedColor(_backgroundColors, colorSpan, out var colorCode))
            {
                result.Append(CreateSequence("m", colorCode));
                return true;
            }
            return false;
        }

        if (sequence.StartsWith("#fg:", StringComparison.Ordinal))
        {
            if (!TryParseRgbSpan(sequence[4..], out var r, out var g, out var b))
            {
                throw new ArgumentException($"Invalid RGB color sequence starting at position {positionAfterSequence}");
            }

            result.Append(Foreground(r, g, b));
            return true;
        }

        if (sequence.StartsWith("#bg:", StringComparison.Ordinal))
        {
            if (!TryParseRgbSpan(sequence[4..], out var r, out var g, out var b))
            {
                throw new ArgumentException($"Invalid RGB color sequence starting at position {positionAfterSequence}");
            }

            result.Append(Background(r, g, b));
            return true;
        }

        if (sequence.Length > 0 && sequence[0] == '#')
        {
            if (!TryParseRgbSpan(sequence[1..], out var r, out var g, out var b))
            {
                throw new ArgumentException($"Invalid RGB color sequence starting at position {positionAfterSequence}");
            }

            result.Append(Foreground(r, g, b));
            return true;
        }

        if (TryGetNamedColor(_foregroundColors, sequence, out var fgCode))
        {
            result.Append(CreateSequence("m", fgCode));
            return true;
        }

        return false;
    }

    private static bool TryGetNamedColor(Dictionary<string, int> colorDict, ReadOnlySpan<char> nameSpan, out int code)
    {
        foreach (var kvp in colorDict)
        {
            if (nameSpan.Equals(kvp.Key, StringComparison.Ordinal))
            {
                code = kvp.Value;
                return true;
            }
        }
        code = 0;
        return false;
    }

    private static bool TryParseRgbSpan(ReadOnlySpan<char> span, out byte r, out byte g, out byte b)
    {
        r = g = b = 0;
        var comma1 = span.IndexOf(',');
        if (comma1 == -1) return false;

        var part1 = span[..comma1];
        var remaining = span[(comma1 + 1)..];

        var comma2 = remaining.IndexOf(',');
        if (comma2 == -1) return false;

        var part2 = remaining[..comma2];
        var part3 = remaining[(comma2 + 1)..];

        return byte.TryParse(part1, out r) && byte.TryParse(part2, out g) && byte.TryParse(part3, out b);
    }
}
