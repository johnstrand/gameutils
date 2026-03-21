namespace GameUtils.Math;

/// <summary>
/// A collection of math-related extra methods.
/// </summary>
public static class MathExt
{
    private static readonly Random _random = new();

    /// <summary>
    /// Returns a random float between 0 and 1.
    /// </summary>
    public static float RandomFloat()
    {
        return _random.NextSingle();
    }

    /// <summary>
    /// Returns a random float between min and max.
    /// </summary>
    public static float RandomFloat(float min, float max)
    {
        return MathFExt.Remap(_random.NextSingle(), 0, 1, min, max);
    }

    /// <summary>
    /// Returns a random integer in the range [min, max).
    /// </summary>
    public static int RandomInt(int min, int max)
    {
        return _random.Next(min, max);
    }

    /// <summary>
    /// Returns true with the given probability (0 = never, 1 = always).
    /// </summary>
    public static bool RandomBool(float probability = 0.5f)
    {
        return _random.NextSingle() < probability;
    }

    /// <summary>
    /// Returns a normally-distributed random float using the Box-Muller transform.
    /// </summary>
    public static float RandomGaussian(float mean = 0f, float stdDev = 1f)
    {
        var u1 = 1f - _random.NextSingle();
        var u2 = 1f - _random.NextSingle();
        var normal = MathF.Sqrt(-2f * MathF.Log(u1)) * MathF.Sin(MathF.Tau * u2);
        return mean + (stdDev * normal);
    }

    /// <summary>
    /// Returns a uniformly-distributed random point inside a circle of the given radius.
    /// </summary>
    public static System.Numerics.Vector2 RandomInCircle(float radius = 1f)
    {
        var angle = _random.NextSingle() * MathF.Tau;
        var r = radius * MathF.Sqrt(_random.NextSingle());
        return new System.Numerics.Vector2(r * MathF.Cos(angle), r * MathF.Sin(angle));
    }

    /// <summary>
    /// Returns a random point on the perimeter of a circle of the given radius.
    /// </summary>
    public static System.Numerics.Vector2 RandomOnCircle(float radius = 1f)
    {
        var angle = _random.NextSingle() * MathF.Tau;
        return new System.Numerics.Vector2(radius * MathF.Cos(angle), radius * MathF.Sin(angle));
    }
}