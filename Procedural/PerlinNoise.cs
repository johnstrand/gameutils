namespace GameUtils.Procedural;

/// <summary>
/// Classic Perlin noise and fractal Brownian motion (fBm) implementations.
/// The noise functions return values roughly in the range [-1, 1] for 3D and [0, 1] for 2D.
/// </summary>
public class PerlinNoise
{
    // The standard Ken Perlin permutation table (256 entries, repeated twice to avoid index wrapping).
    private static readonly int[] _defaultPermutation =
    [
        151,160,137, 91, 90, 15,131, 13,201, 95, 96, 53,194,233,  7,225,
        140, 36,103, 30, 69,142,  8, 99, 37,240, 21, 10, 23,190,  6,148,
        247,120,234, 75,  0, 26,197, 62, 94,252,219,203,117, 35, 11, 32,
         57,177, 33, 88,237,149, 56, 87,174, 20,125,136,171,168, 68,175,
         74,165, 71,134,139, 48, 27,166, 77,146,158,231, 83,111,229,122,
         60,211,133,230,220,105, 92, 41, 55, 46,245, 40,244,102,143, 54,
         65, 25, 63,161,  1,216, 80, 73,209, 76,132,187,208, 89, 18,169,
        200,196,135,130,116,188,159, 86,164,100,109,198,173,186,  3, 64,
         52,217,226,250,124,123,  5,202, 38,147,118,126,255, 82, 85,212,
        207,206, 59,227, 47, 16, 58, 17,182,189, 28, 42,223,183,170,213,
        119,248,152,  2, 44,154,163, 70,221,153,101,155,167, 43,172,  9,
        129, 22, 39,253, 19, 98,108,110, 79,113,224,232,178,185,112,104,
        218,246, 97,228,251, 34,242,193,238,210,144, 12,191,179,162,241,
         81, 51,145,235,249, 14,239,107, 49,192,214, 31,181,199,106,157,
        184, 84,204,176,115,121, 50, 45,127,  4,150,254,138,236,205, 93,
        222,114, 67, 29, 24, 72,243,141,128,195, 78, 66,215, 61,156,180,
    ];

    /// <summary>The default instance using Ken Perlin's standard permutation table.</summary>
    public static readonly PerlinNoise Default = new();

    private readonly int[] _p;

    /// <summary>Creates a <see cref="PerlinNoise"/> instance using Ken Perlin's standard permutation table.</summary>
    public PerlinNoise()
    {
        _p = new int[512];
        for (var i = 0; i < 256; i++)
        {
            _p[i] = _p[i + 256] = _defaultPermutation[i];
        }
    }

    /// <summary>Creates a seeded <see cref="PerlinNoise"/> instance with a deterministically shuffled permutation table.</summary>
    public PerlinNoise(int seed)
    {
        var perm = Enumerable.Range(0, 256).ToArray();
        var rng = new Random(seed);
        for (var i = perm.Length - 1; i > 0; i--)
        {
            var j = rng.Next(i + 1);
            (perm[i], perm[j]) = (perm[j], perm[i]);
        }

        _p = new int[512];
        for (var i = 0; i < 256; i++)
        {
            _p[i] = _p[i + 256] = perm[i];
        }
    }

    /// <summary>
    /// Returns a 2D Perlin noise value in approximately [0, 1] for the given coordinates.
    /// </summary>
    public float Sample(float x, float y)
    {
        return (Sample(x, y, 0) + 1f) / 2f;
    }

    /// <summary>
    /// Returns a 3D Perlin noise value in approximately [-1, 1] for the given coordinates.
    /// </summary>
    public float Sample(float x, float y, float z)
    {
        var xi = (int)MathF.Floor(x) & 255;
        var yi = (int)MathF.Floor(y) & 255;
        var zi = (int)MathF.Floor(z) & 255;

        x -= MathF.Floor(x);
        y -= MathF.Floor(y);
        z -= MathF.Floor(z);

        var u = Fade(x);
        var v = Fade(y);
        var w = Fade(z);

        var a  = _p[xi] + yi;
        var aa = _p[a] + zi;
        var ab = _p[a + 1] + zi;
        var b  = _p[xi + 1] + yi;
        var ba = _p[b] + zi;
        var bb = _p[b + 1] + zi;

        return Lerp(w,
            Lerp(v,
                Lerp(u, Grad(_p[aa],     x,     y,     z),
                        Grad(_p[ba],     x - 1, y,     z)),
                Lerp(u, Grad(_p[ab],     x,     y - 1, z),
                        Grad(_p[bb],     x - 1, y - 1, z))),
            Lerp(v,
                Lerp(u, Grad(_p[aa + 1], x,     y,     z - 1),
                        Grad(_p[ba + 1], x - 1, y,     z - 1)),
                Lerp(u, Grad(_p[ab + 1], x,     y - 1, z - 1),
                        Grad(_p[bb + 1], x - 1, y - 1, z - 1))));
    }

    /// <summary>
    /// Returns fractal Brownian motion (layered Perlin noise) in approximately [0, 1].
    /// </summary>
    /// <param name="x">X coordinate.</param>
    /// <param name="y">Y coordinate.</param>
    /// <param name="octaves">Number of noise layers to sum.</param>
    /// <param name="lacunarity">Frequency multiplier per octave (default: 2).</param>
    /// <param name="gain">Amplitude multiplier per octave (default: 0.5).</param>
    public float Fbm(float x, float y, int octaves, float lacunarity = 2f, float gain = 0.5f)
    {
        var value = 0f;
        var amplitude = 0.5f;
        var frequency = 1f;

        for (var i = 0; i < octaves; i++)
        {
            value += amplitude * Sample(x * frequency, y * frequency);
            amplitude *= gain;
            frequency *= lacunarity;
        }

        return value;
    }

    private static float Fade(float t)
    {
        return t * t * t * ((t * ((t * 6) - 15)) + 10);
    }

    private static float Lerp(float t, float a, float b)
    {
        return a + (t * (b - a));
    }

    private static float Grad(int hash, float x, float y, float z)
    {
        var h = hash & 15;
        var u = h < 8 ? x : y;
        float v;
        if (h < 4)
        {
            v = y;
        }
        else if (h is 12 or 14)
        {
            v = x;
        }
        else
        {
            v = z;
        }

        return ((h & 1) == 0 ? u : -u) + ((h & 2) == 0 ? v : -v);
    }
}
