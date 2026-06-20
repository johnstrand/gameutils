using System.Numerics;

namespace GameUtils;

/// <summary>
/// Represents an RGBA color.
/// </summary>
public readonly partial struct Color
{
    /// <summary>Red channel (0–255).</summary>
    public readonly byte R;

    /// <summary>Green channel (0–255).</summary>
    public readonly byte G;

    /// <summary>Blue channel (0–255).</summary>
    public readonly byte B;

    /// <summary>Alpha channel (0 = transparent, 255 = opaque).</summary>
    public readonly byte A;

    /// <summary>Creates a color from byte channel values.</summary>
    public Color(byte r, byte g, byte b, byte a = 255)
    {
        R = r;
        G = g;
        B = b;
        A = a;
    }

    /// <summary>Creates a color from normalised float channel values (0–1).</summary>
    public Color(float r, float g, float b, float a = 1.0f)
    {
        R = (byte)MathF.Round(System.Math.Clamp(r, 0, 1) * 255.0f);
        G = (byte)MathF.Round(System.Math.Clamp(g, 0, 1) * 255.0f);
        B = (byte)MathF.Round(System.Math.Clamp(b, 0, 1) * 255.0f);
        A = (byte)MathF.Round(System.Math.Clamp(a, 0, 1) * 255.0f);
    }

    /// <summary>Creates an opaque color from a normalised RGB vector and an optional alpha value (0–1).</summary>
    public Color(Vector3 color, float alpha = 1)
    {
        color = Vector3.Clamp(color, Vector3.Zero, Vector3.One) * 255;

        R = (byte)MathF.Round(color.X);
        G = (byte)MathF.Round(color.Y);
        B = (byte)MathF.Round(color.Z);
        A = (byte)MathF.Round(System.Math.Clamp(alpha, 0, 1) * 255);
    }

    /// <summary>Creates a color from a normalised RGBA vector.</summary>
    public Color(Vector4 color)
    {
        color = Vector4.Clamp(color, Vector4.Zero, Vector4.One) * 255;

        R = (byte)MathF.Round(color.X);
        G = (byte)MathF.Round(color.Y);
        B = (byte)MathF.Round(color.Z);
        A = (byte)MathF.Round(color.W);
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

    /// <summary>Explicitly converts a normalised RGB vector to a <see cref="Color"/>.</summary>
    public static explicit operator Color(Vector3 v)
    {
        return new Color(v);
    }

    /// <summary>Explicitly converts a normalised RGBA vector to a <see cref="Color"/>.</summary>
    public static explicit operator Color(Vector4 v)
    {
        return new Color(v);
    }

    /// <summary>Explicitly converts a <see cref="Color"/> to a normalised RGB vector.</summary>
    public static explicit operator Vector3(Color c)
    {
        return new Vector3(c.R / 255f, c.G / 255f, c.B / 255f);
    }

    /// <summary>Explicitly converts a <see cref="Color"/> to a normalised RGBA vector.</summary>
    public static explicit operator Vector4(Color c)
    {
        return new Vector4(c.R / 255f, c.G / 255f, c.B / 255f, c.A / 255f);
    }

    /// <summary>
    /// Linearly interpolates between two colors by the factor <paramref name="t"/> (clamped to [0, 1]).
    /// </summary>
    public static Color Lerp(Color a, Color b, float t)
    {
        t = System.Math.Clamp(t, 0f, 1f);
        return new Color(
            (byte)MathF.Round(a.R + ((b.R - a.R) * t)),
            (byte)MathF.Round(a.G + ((b.G - a.G) * t)),
            (byte)MathF.Round(a.B + ((b.B - a.B) * t)),
            (byte)MathF.Round(a.A + ((b.A - a.A) * t)));
    }
}
