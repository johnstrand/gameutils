using GameUtils.Types.Collections;
using System.Numerics;

namespace GameUtils.Procedural;

/// <summary>
/// Generates a diamond-square map
/// </summary>
public static class Diamond
{
    /// <summary>
    /// Creates a new diamond-square map from the specified parameters
    /// </summary>
    /// <param name="size">Height and width</param>
    /// <param name="min">Min value of the initial seed</param>
    /// <param name="max">Max value of the initial seed</param>
    /// <param name="range">The initial range for the next step</param>
    /// <param name="nextRange">A method that will be passed the current range and is expected to return the range for hte next iteration</param>
    /// <param name="valueFactory">A method that will be passed an average value and a range, and is expected to return an integer map value</param>
    public static Grid<int> Create(int size, int min, int max, float range, Func<float, float> nextRange, Func<float, float, int> valueFactory)
    {
        var map = new Grid<int>(size, size);
        var r = new Random();
        map[0, 0] = r.Next(min, max);
        map[0, size - 1] = r.Next(min, max);
        map[size - 1, 0] = r.Next(min, max);
        map[size - 1, size - 1] = r.Next(min, max);

        var step = size - 1;

        while (step > 1)
        {
            for (var y = 0; y < size - 1; y += step)
            {
                for (var x = 0; x < size - 1; x += step)
                {
                    var topleft = new Vector2(x, y);
                    var topright = new Vector2(x + step, y);
                    var bottomleft = new Vector2(x, y + step);
                    var bottomright = new Vector2(x + step, y + step);

                    var mid = new Vector2(x + (step / 2), y + (step / 2));

                    var top = new Vector2(mid.X, y);
                    var left = new Vector2(x, mid.Y);
                    var right = new Vector2(x + step, mid.Y);
                    var bottom = new Vector2(mid.X, y + step);

                    map[mid] = valueFactory(Average(map, topleft, topright, bottomleft, bottomright), range);
                    map[top] = valueFactory(Average(map, topleft, topright, mid, top + new Vector2(0, -step)), range);
                    map[left] = valueFactory(Average(map, topleft, bottomleft, mid, left + new Vector2(-step, 0)), range);
                    map[right] = valueFactory(Average(map, topright, bottomright, mid, right + new Vector2(step, 0)), range);
                    map[bottom] = valueFactory(Average(map, bottomleft, bottomright, mid, bottom + new Vector2(0, step)), range);
                }
            }

            range = nextRange(range);
            step /= 2;
        }

        return map;
    }

    private static float Average(Grid<int> map, params Vector2[] vector2s)
    {
        return (float)vector2s.Where(map.IsInBounds).Select(v => map[v]).Average();
    }
}
