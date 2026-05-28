using GameUtils.Types.Collections;
using System.Numerics;

namespace GameUtils.Procedural;

/// <summary>
/// Generates a diamond-square map
/// </summary>
public static class Diamond
{
    /// <summary>
    /// Creates a new diamond-square map from the specified parameters.
    /// </summary>
    /// <param name="size">Height and width. Must be a power-of-two plus one (e.g. 129, 257, 513).</param>
    /// <param name="min">Min value of the initial seed</param>
    /// <param name="max">Max value of the initial seed</param>
    /// <param name="range">The initial range for the next step</param>
    /// <param name="nextRange">A method that will be passed the current range and is expected to return the range for the next iteration</param>
    /// <param name="valueFactory">A method that will be passed an average value and a range, and is expected to return an integer map value</param>
    /// <param name="seed">Optional random seed for reproducible generation. Uses a thread-safe shared instance when null.</param>
    /// <exception cref="ArgumentException">Thrown when <paramref name="size"/> is not a power-of-two plus one.</exception>
    public static Grid<int> Create(int size, int min, int max, float range, Func<float, float> nextRange, Func<float, float, int> valueFactory, int? seed = null)
    {
        var n = size - 1;
        if (n < 1 || (n & (n - 1)) != 0)
        {
            throw new ArgumentException("Size must be a power-of-two plus one (e.g. 3, 5, 9, 17, 33, ...).", nameof(size));
        }

        var r = seed.HasValue ? new Random(seed.Value) : Random.Shared;
        var map = new Grid<int>(size, size);
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

    private static float Average(Grid<int> map, Vector2 a, Vector2 b, Vector2 c, Vector2 d)
    {
        var sum = 0f;
        var count = 0;
        if (map.IsInBounds(a)) { sum += map[a]; count++; }
        if (map.IsInBounds(b)) { sum += map[b]; count++; }
        if (map.IsInBounds(c)) { sum += map[c]; count++; }
        if (map.IsInBounds(d)) { sum += map[d]; count++; }
        return count == 0 ? 0f : sum / count;
    }
}
