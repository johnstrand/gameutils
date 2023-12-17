using System.Numerics;

namespace GameUtils;

public static class Vector2Ext
{
    public static Vector2 Floor(this Vector2 value)
    {
        return new Vector2(MathF.Floor(value.X), MathF.Floor(value.Y));
    }

    public static Vector2 Ceil(this Vector2 value)
    {
        return new Vector2(MathF.Ceiling(value.X), MathF.Ceiling(value.Y));
    }

    public static Vector2 Round(this Vector2 value)
    {
        return new Vector2(MathF.Round(value.X), MathF.Round(value.Y));
    }

    /// <summary>
    /// Returns the normalized result of <paramref name="target"/> - <paramref name="source"/>
    /// </summary>
    public static Vector2 GetDirection(this Vector2 source, Vector2 target)
    {
        return Vector2.Normalize(target - source);
    }

    /// <summary>
    /// Converts a vector to an angle in radians
    /// </summary>
    public static float ToAngle(this Vector2 vec)
    {
        return MathF.Atan2(vec.Y, vec.X);
    }

    /// <summary>
    /// Returns the angle in radians for a line going from <paramref name="source"/> to <paramref name="target"/>
    /// </summary>
    public static float AngleTowards(this Vector2 source, Vector2 target)
    {
        return source.GetDirection(target).ToAngle();
    }

    /// <summary>
    /// Returns the angle between two vectors in radians
    /// </summary>
    public static float AngleBetween(this Vector2 a, Vector2 b)
    {
        var dot = Vector2.Dot(a, b);
        var det = (a.X * b.Y) - (a.Y * b.X);
        return MathF.Atan2(det, dot);
    }

    public static void Deconstruct(this Vector2 value, out float x, out float y)
    {
        (x, y) = (value.X, value.Y);
    }
}
