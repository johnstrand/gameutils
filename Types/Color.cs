using System.Numerics;

namespace GameUtils;
internal readonly partial struct Color
{
    public readonly byte R;        // Color red value
    public readonly byte G;        // Color green value
    public readonly byte B;        // Color blue value
    public readonly byte A;        // Color alpha value

    public Color(byte r, byte g, byte b, byte a = 255)
    {
        R = r;
        G = g;
        B = b;
        A = a;
    }

    public Color(float r, float g, float b, float a = 1.0f)
    {
        R = (byte)(Math.Clamp(r, 0, 1) * 255.0f);
        G = (byte)(Math.Clamp(g, 0, 1) * 255.0f);
        B = (byte)(Math.Clamp(b, 0, 1) * 255.0f);
        A = (byte)(Math.Clamp(a, 0, 1) * 255.0f);
    }

    public Color(Vector3 color, float alpha = 1)
    {
        color = Vector3.Clamp(color, Vector3.Zero, Vector3.One) * 255;

        R = (byte)color.X;
        G = (byte)color.Y;
        B = (byte)color.Z;
        A = (byte)(Math.Clamp(alpha, 0, 1) * 255);
    }

    public Color(Vector4 color)
    {
        color = Vector4.Clamp(color, Vector4.Zero, Vector4.One) * 255;

        R = (byte)color.X;
        G = (byte)color.Y;
        B = (byte)color.Z;
        A = (byte)color.W;
    }

    /// <summary>
    /// Creates a color from an signed integer in the format of 0xRRGGBBAA.
    /// </summary>
    public static Color FromRgba(int color)
    {
        return new Color(
            (byte)((color >> 24) & 0xFF),
            (byte)((color >> 16) & 0xFF),
            (byte)((color >> 8) & 0xFF),
            (byte)(color & 0xFF));
    }

    public static explicit operator Color(Vector3 v)
    {
        return new Color(v);
    }

    public static explicit operator Color(Vector4 v)
    {
        return new Color(v);
    }

    public static explicit operator Vector3(Color c)
    {
        return new Vector3(c.R / 255f, c.G / 255f, c.B / 255f);
    }

    public static explicit operator Vector4(Color c)
    {
        return new Vector4(c.R / 255f, c.G / 255f, c.B / 255f, c.A / 255f);
    }
}
