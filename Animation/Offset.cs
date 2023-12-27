namespace GameUtils;

/// <summary>
/// Math functions for offsetting animations. The functions are in the form of f(x) = y, where x is a value from 0 to 1 and y is a value from -1 to 1.
/// The output is always 0 when x is 0 or 1.
/// </summary>
public static class Offset
{
    /// <summary>
    /// Jagged moves around the midpoint
    /// </summary>
    public static float Jagged(float x)
    {
        return x < .25f ? -x : x < .75f ? x - .5f : -x + 1;
    }

    /// <summary>
    /// Smooth sine wave moves around the midpoint
    /// </summary>
    public static float Sine(float x)
    {
        return MathF.Sin(x * MathF.Tau);
    }

    /// <summary>
    /// Pulse wave moves around the midpoint
    /// </summary>
    public static float Pulse(float x)
    {
        var t = MathF.Sin(x * MathF.Tau * 3);
        var u = (1 - MathF.Cos(x * MathF.Tau)) / 2;

        return t * u * u;
    }

    /// <summary>
    /// Triangle wave moves around the midpoint
    /// </summary>
    public static float Triangle(float x)
    {
        return MathF.Abs(((x + .25f) * 4 % 4) - 2) - 1;
    }

    /// <summary>
    /// Wobbly wave moves around the midpoint
    /// </summary>
    public static float Wobble(float x)
    {
        float q = (1 - MathF.Cos(x * MathF.Tau)) / 2;
        return (MathF.Cos(x * 20) * q) + (MathF.Sin(x * 400) / 14 * q);
    }
}
