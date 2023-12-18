using GameUtils.Console;

namespace GameUtils;

// This file only exists to manually test the library. It is not included in the library itself.
internal static class Program
{
    private static void Main()
    {
        var step = 5;
        for (var b = 0; b <= 255; b += step)
        {
            System.Console.Write($"{b:000} ");
            for (var r = 0; r <= 255; r += step)
            {
                Ansi.Write($"[#{r},0,{b}]█");
            }
            for (var g = 0; g <= 255; g += step)
            {
                Ansi.Write($"[#255,{g},{b}]█");
            }
            for (var r = 255; r >= 0; r -= step)
            {
                Ansi.Write($"[#{r},255,{b}]█");
            }
            System.Console.WriteLine();
        }
    }
}
