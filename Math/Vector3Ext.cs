using System.Numerics;

namespace GameUtils;

public static class Vector3Ext
{
    public static Vector3 Floor(this Vector3 value)
    {
        return new Vector3(MathF.Floor(value.X), MathF.Floor(value.Y), MathF.Floor(value.Z));
    }

    public static Vector3 Ceil(this Vector3 value)
    {
        return new Vector3(MathF.Ceiling(value.X), MathF.Ceiling(value.Y), MathF.Ceiling(value.Z));
    }

    public static Vector3 Round(this Vector3 value)
    {
        return new Vector3(MathF.Round(value.X), MathF.Round(value.Y), MathF.Round(value.Z));
    }

    public static void Deconstruct(this Vector3 value, out float x, out float y, out float z)
    {
        (x, y, z) = (value.X, value.Y, value.Z);
    }

    public static Vector2 XY(this Vector3 value)
    {
        return new Vector2(value.X, value.Y);
    }

    public static Vector2 XZ(this Vector3 value)
    {
        return new Vector2(value.X, value.Z);
    }

    public static Vector2 YZ(this Vector3 value)
    {
        return new Vector2(value.Y, value.Z);
    }
}
