using GameUtils.Extensions;
using System.Text;

namespace GameUtils.Term;

public static class Ansi
{
    public const string SequenceStart = "\u001b[";

    public static string CreateSequence(string type, params object[] codes)
    {
        return $"{SequenceStart}{string.Join(";", codes)}{type}";
    }

    public static string Reset => CreateSequence("m", 0);

    public static string Bold => CreateSequence("m", 1);

    public static string Faint => CreateSequence("m", 2);

    public static string Italic => CreateSequence("m", 3);

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

    public static string Foreground(string color)
    {
        return CreateSequence("m", _foregroundColors[color]);
    }

    public static string Foreground(byte r, byte g, byte b)
    {
        return CreateSequence("m", 38, 2, r, g, b);
    }

    public static string Background(string color)
    {
        return CreateSequence("m", _backgroundColors[color]);
    }

    public static string Background(byte r, byte g, byte b)
    {
        return CreateSequence("m", 48, 2, r, g, b);
    }

    public static void CursorUp(int amount = 1)
    {
        System.Console.Write(CreateSequence("A", amount));
    }

    public static void CursorDown(int amount = 1)
    {
        System.Console.Write(CreateSequence("B", amount));
    }

    public static void CursorForward(int amount = 1)
    {
        System.Console.Write(CreateSequence("C", amount));
    }

    public static void CursorBack(int amount = 1)
    {
        System.Console.Write(CreateSequence("D", amount));
    }

    public static void CursorNextLine(int amount = 1)
    {
        System.Console.Write(CreateSequence("E", amount));
    }

    public static void CursorPreviousLine(int amount = 1)
    {
        System.Console.Write(CreateSequence("F", amount));
    }

    public static void CursorHorizontalAbsolute(int column)
    {
        System.Console.Write(CreateSequence("G", column));
    }

    public static void CursorPosition(int row, int column)
    {
        System.Console.Write(CreateSequence("H", row, column));
    }

    public static void Clear(int mode = 0)
    {
        System.Console.Write(CreateSequence("J", mode));
    }

    public static void ClearLine(int mode = 0)
    {
        System.Console.Write(CreateSequence("K", mode));
    }

    public static void ScrollUp(int amount = 1)
    {
        System.Console.Write(CreateSequence("S", amount));
    }

    public static void ScrollDown(int amount = 1)
    {
        System.Console.Write(CreateSequence("T", amount));
    }

    public static string Write(string text, params object[] args)
    {
        var formatted = Format(string.Format(text, args));
        System.Console.Write(formatted);
        return formatted;
    }

    public static string WriteLine(string text, params object[] args)
    {
        var formatted = Format(string.Format(text, args));
        System.Console.WriteLine(formatted);
        return formatted;
    }

    public static string Format(string text)
    {
        var result = new StringBuilder();

        for (var i = 0; i < text.Length; i++)
        {
            var c = text[i];

            if (c != '[')
            {
                result.Append(c);
                continue;
            }

            if (text.TryGet(i + 1, out var next) && next == '[')
            {
                result.Append('[');
                i++;
                continue;
            }

            var endOfSequence = text.IndexOf(']', i);

            if (endOfSequence == -1)
            {
                throw new ArgumentException($"Invalid ANSI sequence starting at position {i}");
            }

            var sequence = text.Substring(i + 1, endOfSequence - i - 1);
            i = endOfSequence;

            if (sequence is "/" or "")
            {
                result.Append(Reset);
                continue;
            }

            if (_foregroundColors.ContainsKey(sequence))
            {
                result.Append(Foreground(sequence));
                continue;
            }

            if (sequence is "bold" or "b")
            {
                result.Append(Bold);
                continue;
            }

            if (sequence is "faint" or "f")
            {
                result.Append(Faint);
                continue;
            }

            if (sequence is "italic" or "i")
            {
                result.Append(Italic);
                continue;
            }

            if (sequence is "underline" or "u")
            {
                result.Append(Underline);
                continue;
            }

            if (sequence.StartsWith("fg:"))
            {
                result.Append(Foreground(sequence[3..]));
                continue;
            }

            if (sequence.StartsWith("bg:"))
            {
                result.Append(Background(sequence[3..]));
                continue;
            }

            if (sequence[0] == '#' || sequence.StartsWith("#fg"))
            {
                var rgb = sequence[(sequence[0] == '#' ? 1 : 3)..].Split(',');

                if (rgb.Length != 3)
                {
                    throw new ArgumentException($"Invalid RGB color sequence starting at position {i}");
                }

                var r = byte.Parse(rgb[0]);
                var g = byte.Parse(rgb[1]);
                var b = byte.Parse(rgb[2]);
                result.Append(Foreground(r, g, b));
                continue;
            }

            if (sequence.StartsWith("#bg:"))
            {
                var rgb = sequence[3..].Split(',');

                if (rgb.Length != 3)
                {
                    throw new ArgumentException($"Invalid RGB color sequence starting at position {i}");
                }

                var r = byte.Parse(rgb[0]);
                var g = byte.Parse(rgb[1]);
                var b = byte.Parse(rgb[2]);
                result.Append(Foreground(r, g, b));
                continue;
            }

            throw new Exception($"Unknown ANSI sequence '{sequence}' starting at position {i}");
        }

        return result.ToString();
    }
}
