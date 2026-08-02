using GameUtils.Types.Collections;

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
            var halfStep = step / 2;
            for (var y = 0; y < size - 1; y += step)
            {
                for (var x = 0; x < size - 1; x += step)
                {
                    var mx = x + halfStep;
                    var my = y + halfStep;

                    map[mx, my] = valueFactory((map[x, y] + map[x + step, y] + map[x, y + step] + map[x + step, y + step]) * 0.25f, range);
                    map[mx, y] = valueFactory(AverageInt(map, x, y, x + step, y, mx, my, mx, y - step, size), range);
                    map[x, my] = valueFactory(AverageInt(map, x, y, x, y + step, mx, my, x - step, my, size), range);
                    map[x + step, my] = valueFactory(AverageInt(map, x + step, y, x + step, y + step, mx, my, x + step + step, my, size), range);
                    map[mx, y + step] = valueFactory(AverageInt(map, x, y + step, x + step, y + step, mx, my, mx, y + step + step, size), range);
                }
            }

            range = nextRange(range);
            step = halfStep;
        }

        return map;
    }

    private static float AverageInt(Grid<int> map, int ax, int ay, int bx, int by, int cx, int cy, int dx, int dy, int size)
    {
        var sum = 0f;
        var count = 0;
        if (ax >= 0 && ax < size && ay >= 0 && ay < size) { sum += map[ax, ay]; count++; }
        if (bx >= 0 && bx < size && by >= 0 && by < size) { sum += map[bx, by]; count++; }
        if (cx >= 0 && cx < size && cy >= 0 && cy < size) { sum += map[cx, cy]; count++; }
        if (dx >= 0 && dx < size && dy >= 0 && dy < size) { sum += map[dx, dy]; count++; }
        return count == 0 ? 0f : sum / count;
    }
}
