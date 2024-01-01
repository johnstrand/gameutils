using System.Numerics;

namespace GameUtils;

/// <summary>
/// Extension methods for Vector2.
/// </summary>
public static class Vector2Ext
{
    /// <summary>
    /// Rotates a vector around origin (0,0) by the specified angle in radians.
    /// </summary>
    public static Vector2 Rotate(this Vector2 value, float radians)
    {
        var (sin, cos) = MathF.SinCos(radians);
        return new Vector2((value.X * cos) - (value.Y * sin), (value.X * sin) + (value.Y * cos));
    }

    /// <summary>
    /// Rotates a vector around origin by the specified angle in radians.
    /// </summary>
    public static Vector2 Rotate(this Vector2 value, float radians, Vector2 origin)
    {
        var (sin, cos) = MathF.SinCos(radians);
        var (x, y) = (value.X - origin.X, value.Y - origin.Y);
        return new Vector2((x * cos) - (y * sin) + origin.X, (x * sin) + (y * cos) + origin.Y);
    }

    /// <summary>
    /// Adds <paramref name="v"/> to both X and Y of <paramref name="value"/>
    /// </summary>
    public static Vector2 Add(this Vector2 value, float v)
    {
        return new Vector2(value.X + v, value.Y + v);
    }

    /// <summary>
    /// Adds <paramref name="x"/> and <paramref name="y"/> to X and Y of <paramref name="value"/>
    /// </summary>
    public static Vector2 Add(this Vector2 value, float x, float y)
    {
        return new Vector2(value.X + x, value.Y + y);
    }

    /// <summary>
    /// Returns a new vector with the X and Y values rounded down to the nearest integer
    /// </summary>
    public static Vector2 Floor(this Vector2 value)
    {
        return new Vector2(MathF.Floor(value.X), MathF.Floor(value.Y));
    }

    /// <summary>
    /// Returns a new vector with the X and Y values rounded up to the nearest integer
    /// </summary>
    public static Vector2 Ceil(this Vector2 value)
    {
        return new Vector2(MathF.Ceiling(value.X), MathF.Ceiling(value.Y));
    }

    /// <summary>
    /// Returns a new vector with the X and Y values rounded to the nearest integer
    /// </summary>
    public static Vector2 Round(this Vector2 value)
    {
        return new Vector2(MathF.Round(value.X), MathF.Round(value.Y));
    }

    /// <summary>
    /// Calculates the midpoint (average) of a list of vectors
    /// </summary>
    public static Vector2 Midpoint(this IEnumerable<Vector2> source)
    {
        var sum = Vector2.Zero;
        var count = 0;
        foreach (var item in source)
        {
            sum += item;
            count++;
        }

        return sum / count;
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
    /// Returns the angle between two vectors in radians (i.e., the result of Vector2.UnitX and Vector2.UnitY is PI / 2)
    /// </summary>
    public static float AngleBetween(this Vector2 a, Vector2 b)
    {
        var dot = Vector2.Dot(Vector2.Normalize(a), Vector2.Normalize(b));
        var det = (a.X * b.Y) - (a.Y * b.X);
        return MathF.Atan2(det, dot);
    }

    /// <summary>
    /// Deconstructs a vector into its X and Y components
    /// </summary>
    public static void Deconstruct(this Vector2 value, out float x, out float y)
    {
        (x, y) = (value.X, value.Y);
    }

    /// <summary>
    /// Converts a Vector2 to a Vector3, with the default Z value of 0
    /// </summary>
    public static Vector3 ToVector3(this Vector2 value, float z = 0)
    {
        return new Vector3(value.X, value.Y, z);
    }

    /// <summary>
    /// Sorts a list of vectors clockwise (or counter clockwise, if so desired) around the average center of the vectors.
    /// </summary>
    public static IEnumerable<Vector2> Sort(this IEnumerable<Vector2> source, bool clockwise = true)
    {
        var center = source.Midpoint();
        return Sort(source, center, clockwise);
    }

    /// <summary>
    /// Sorts a list of vectors clockwise (or counter clockwise, if so desired) around the specified center.
    /// </summary>
    public static IEnumerable<Vector2> Sort(this IEnumerable<Vector2> source, Vector2 center, bool clockwise = true)
    {
        var sortMultiplier = clockwise ? 1 : -1;
        return source.OrderBy(v => MathF.Atan2(v.Y - center.Y, v.X - center.X) * sortMultiplier);
    }
}
