using System.Numerics;

namespace GameUtils.Math;

/// <summary>
/// Extension methods for Vector3.
/// </summary>
public static class Vector3Ext
{
    /// <summary>
    /// Returns a new vector with the X, Y and Z values rounded down to the nearest integer
    /// </summary>
    public static Vector3 Floor(this Vector3 value)
    {
        return new Vector3(MathF.Floor(value.X), MathF.Floor(value.Y), MathF.Floor(value.Z));
    }

    /// <summary>
    /// Returns a new vector with the X, Y and Z values rounded up to the nearest integer
    /// </summary>
    public static Vector3 Ceil(this Vector3 value)
    {
        return new Vector3(MathF.Ceiling(value.X), MathF.Ceiling(value.Y), MathF.Ceiling(value.Z));
    }

    /// <summary>
    /// Returns a new vector with the X, Y and Z values rounded to the nearest integer
    /// </summary>
    public static Vector3 Round(this Vector3 value)
    {
        return new Vector3(MathF.Round(value.X), MathF.Round(value.Y), MathF.Round(value.Z));
    }

    /// <summary>
    /// Deconstructs a Vector3 into its X, Y and Z components
    /// </summary>
    public static void Deconstruct(this Vector3 value, out float x, out float y, out float z)
    {
        (x, y, z) = (value.X, value.Y, value.Z);
    }

    /// <summary>
    /// Creates a new Vector2 from the X and Y components of a Vector3
    /// </summary>
    public static Vector2 XY(this Vector3 value)
    {
        return new Vector2(value.X, value.Y);
    }

    /// <summary>
    /// Creates a new Vector2 from the X and Z components of a Vector3
    /// </summary>
    public static Vector2 XZ(this Vector3 value)
    {
        return new Vector2(value.X, value.Z);
    }

    /// <summary>
    /// Creates a new Vector2 from the Y and Z components of a Vector3
    /// </summary>
    public static Vector2 YZ(this Vector3 value)
    {
        return new Vector2(value.Y, value.Z);
    }

    /// <summary>
    /// Returns a copy of the vector with the X component replaced.
    /// </summary>
    public static Vector3 WithX(this Vector3 value, float x)
    {
        return new Vector3(x, value.Y, value.Z);
    }

    /// <summary>
    /// Returns a copy of the vector with the Y component replaced.
    /// </summary>
    public static Vector3 WithY(this Vector3 value, float y)
    {
        return new Vector3(value.X, y, value.Z);
    }

    /// <summary>
    /// Returns a copy of the vector with the Z component replaced.
    /// </summary>
    public static Vector3 WithZ(this Vector3 value, float z)
    {
        return new Vector3(value.X, value.Y, z);
    }

    /// <summary>
    /// Returns true if X, Y and Z are all zero.
    /// </summary>
    public static bool IsZero(this Vector3 value)
    {
        return value == Vector3.Zero;
    }
}
