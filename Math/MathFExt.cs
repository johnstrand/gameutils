namespace GameUtils;
public static class MathFExt
{
    public const float DEGREES_PER_RADIANS = MathF.PI / 180f;
    public const float RADIANS_PER_DEGREE = 180f / MathF.PI;
    public const float HALF_PI = MathF.PI / 2f;
    public static float AngleDifference(float fromAngle, float toAngle)
    {
        var diff = ((toAngle - fromAngle + MathF.PI) % MathF.Tau) - MathF.PI;
        return diff < -MathF.PI ? diff + MathF.Tau : diff;
    }

    public static float ToRadians(float degrees)
    {
        return degrees * DEGREES_PER_RADIANS;
    }

    public static float ToDegrees(float radians)
    {
        return radians * RADIANS_PER_DEGREE;
    }

    public static float CubicBezier(float t)
    {
        return (MathF.Sin((t + 1.5f) * MathF.PI) + 1f) / 2f;
    }

    public static float Remap(float value, float sourceRangeStart, float sourceRangeEnd, float targetRangeStart, float targetRangeEnd)
    {
        return (value - sourceRangeStart) / (sourceRangeEnd - sourceRangeStart) * (targetRangeEnd - targetRangeStart) + targetRangeStart;
    }

    public static float RemapClamped(float value, float sourceRangeStart, float sourceRangeEnd, float targetRangeStart, float targetRangeEnd)
    {
        return Math.Clamp(Remap(value, sourceRangeStart, sourceRangeEnd, targetRangeStart, targetRangeEnd), targetRangeStart, targetRangeEnd);
    }

    public static float Wrap(float value, float min, float max)
    {
        var range = max - min;
        return value - MathF.Floor((value - min) / range) * range;
    }

    public static float Normalize(float value, float min, float max)
    {
        return (value - min) / (max - min);
    }
}
