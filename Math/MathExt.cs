namespace GameUtils;

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
}