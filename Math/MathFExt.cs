namespace GameUtils.Math;

/// <summary>
/// A collection of math-related extra methods for floats
/// </summary>
public static class MathFExt
{
    /// <summary>
    /// Degrees per radians.
    /// </summary>
    public const float DEGREES_PER_RADIANS = MathF.PI / 180f;

    /// <summary>
    /// Radians per degrees.
    /// </summary>
    public const float RADIANS_PER_DEGREE = 180f / MathF.PI;

    /// <summary>
    /// Half of pi.
    /// </summary>
    public const float HALF_PI = MathF.PI / 2f;

    /// <summary>
    /// Returns the shortest difference, in radians, between two angles.
    /// </summary>
    public static float AngleDifference(float fromAngle, float toAngle)
    {
        var diff = ((toAngle - fromAngle + MathF.PI) % MathF.Tau) - MathF.PI;
        return diff < -MathF.PI ? diff + MathF.Tau : diff;
    }

    /// <summary>
    /// Converts degrees to radians.
    /// </summary>
    public static float ToRadians(float degrees)
    {
        return degrees * DEGREES_PER_RADIANS;
    }

    /// <summary>
    /// Converts radians to degrees.
    /// </summary>
    public static float ToDegrees(float radians)
    {
        return radians * RADIANS_PER_DEGREE;
    }

    /// <summary>
    /// Remaps a value from one range to another. If the value is outside the source range, the output will be outside the target range.
    /// </summary>
    public static float Remap(float value, float sourceRangeStart, float sourceRangeEnd, float targetRangeStart, float targetRangeEnd)
    {
        return ((value - sourceRangeStart) / (sourceRangeEnd - sourceRangeStart) * (targetRangeEnd - targetRangeStart)) + targetRangeStart;
    }

    /// <summary>
    /// Remaps a value from one range to another. If the value is outside the source range, the output will be clamped to the target range.
    /// </summary>
    public static float RemapClamped(float value, float sourceRangeStart, float sourceRangeEnd, float targetRangeStart, float targetRangeEnd)
    {
        return System.Math.Clamp(Remap(value, sourceRangeStart, sourceRangeEnd, targetRangeStart, targetRangeEnd), targetRangeStart, targetRangeEnd);
    }

    /// <summary>
    /// Wraps a value within the given range. Returns <paramref name="min"/> when <paramref name="min"/> equals <paramref name="max"/>.
    /// </summary>
    public static float Wrap(float value, float min, float max)
    {
        var range = max - min;
        if (MathF.Abs(range) < float.Epsilon)
        {
            return min;
        }

        return min + (value - min) - (MathF.Floor((value - min) / range) * range);
    }

    /// <summary>
    /// Given a value and a range, returns the value normalized to the range.
    /// </summary>
    public static float Normalize(float value, float min, float max)
    {
        return (value - min) / (max - min);
    }

    /// <summary>
    /// Linearly interpolates between <paramref name="a"/> and <paramref name="b"/> by the unclamped factor <paramref name="t"/>.
    /// </summary>
    public static float Lerp(float a, float b, float t)
    {
        return a + ((b - a) * t);
    }

    /// <summary>
    /// Returns the unclamped interpolation factor <i>t</i> such that <c>Lerp(a, b, t) == value</c>.
    /// Returns 0 when <paramref name="a"/> equals <paramref name="b"/>.
    /// </summary>
    public static float InverseLerp(float a, float b, float value)
    {
        return MathF.Abs(b - a) < float.Epsilon ? 0f : (value - a) / (b - a);
    }

    /// <summary>
    /// Performs smooth Hermite interpolation (3t² - 2t³) between 0 and 1 over the range [<paramref name="edge0"/>, <paramref name="edge1"/>].
    /// </summary>
    public static float Smoothstep(float edge0, float edge1, float x)
    {
        var t = System.Math.Clamp((x - edge0) / (edge1 - edge0), 0f, 1f);
        return t * t * (3f - (2f * t));
    }

    /// <summary>
    /// Performs smoother Hermite interpolation (6t⁵ - 15t⁴ + 10t³) between 0 and 1 over the range [<paramref name="edge0"/>, <paramref name="edge1"/>].
    /// </summary>
    public static float Smootherstep(float edge0, float edge1, float x)
    {
        var t = System.Math.Clamp((x - edge0) / (edge1 - edge0), 0f, 1f);
        return t * t * t * ((t * ((t * 6f) - 15f)) + 10f);
    }

    /// <summary>
    /// Bounces <paramref name="t"/> back and forth between 0 and <paramref name="length"/>.
    /// </summary>
    public static float PingPong(float t, float length)
    {
        if (length <= 0)
        {
            return 0f;
        }

        t = MathF.Abs(Wrap(t, 0, length * 2));
        return length - MathF.Abs(t - length);
    }
}
