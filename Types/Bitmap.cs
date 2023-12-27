using System.Numerics;

namespace GameUtils.Types;

/// <summary>
/// Bitmap image class
/// </summary>
public class Bitmap(int width, int height)
{
    /// <summary>
    /// Width of the image in pixels
    /// </summary>
    public int Width { get; } = width;

    /// <summary>
    /// Height of the image in pixels
    /// </summary>
    public int Height { get; } = height;

    /// <summary>
    /// Pixel data
    /// </summary>
    public Vector3[] Data { get; } = new Vector3[width * height];

    /// <summary>
    /// Gets or sets a pixel at the specified coordinates. If the coordinates are out of bounds, no operation is performed and Vector3.Zero is returned.
    /// </summary>
    public Vector3 this[int x, int y]
    {
        get => IsInBounds(x, y) ? Data[(y * Width) + x] : Vector3.Zero;
        set
        {
            if (IsInBounds(x, y))
            {
                Data[(y * Width) + x] = value;
            }
        }
    }

    /// <summary>
    /// Gets or sets a pixel at the specified coordinates. If the coordinates are out of bounds, no operation is performed and Vector3.Zero is returned.
    /// </summary>
    public Vector3 this[Vector2 point]
    {
        get => this[(int)point.X, (int)point.Y];
        set => this[(int)point.X, (int)point.Y] = value;
    }

    /// <summary>
    /// Clears the image to black
    /// </summary>
    public void Clear()
    {
        Clear(Vector3.Zero);
    }

    /// <summary>
    /// Clears the image to the specified color
    /// </summary>
    public void Clear(Vector3 color)
    {
        Array.Fill(Data, color);
    }

    /// <summary>
    /// Draw a rectangle with the specified color
    /// </summary>
    public void Rectangle(Vector2 position, Vector2 size, Vector3 color)
    {
        for (var y = position.Y; y < position.Y + size.Y; y++)
        {
            for (var x = position.X; x < position.X + size.X; x++)
            {
                this[(int)x, (int)y] = color;
            }
        }
    }

    /// <summary>
    /// Draw a rectangle with the specified color
    /// </summary>
    public void Rectangle(int x, int y, int w, int h, Vector3 color)
    {
        Rectangle(new Vector2(x, y), new Vector2(w, h), color);
    }

    /// <summary>
    /// Draw a line with the specified color
    /// </summary>
    public void Line(Vector2 start, Vector2 end, Vector3 color)
    {
        // This is bad, but it works for now
        var length = (end - start).Length();
        var step = (end - start) / length;
        for (var i = 0; i < length; i++)
        {
            this[start + (step * i)] = color;
        }
    }

    /// <summary>
    /// Draw a line with the specified color
    /// </summary>
    public void Line(int x1, int y1, int x2, int y2, Vector3 color)
    {
        Line(new Vector2(x1, y1), new Vector2(x2, y2), color);
    }

    /// <summary>
    /// Draw a circle with the specified color
    /// </summary>
    public void Circle(Vector2 position, float radius, Vector3 color)
    {
        for (var y = position.Y - radius; y < position.Y + radius; y++)
        {
            for (var x = position.X - radius; x < position.X + radius; x++)
            {
                var point = new Vector2(x, y);
                if ((point - position).Length() <= radius)
                {
                    this[point] = color;
                }
            }
        }
    }

    /// <summary>
    /// Returns true if the specified coordinates are within the bounds of the image
    /// </summary>
    public bool IsInBounds(int x, int y)
    {
        return x >= 0 && x < Width && y >= 0 && y < Height;
    }

    /// <summary>
    /// Writes the image to a file
    /// </summary>
    public void Write(string path)
    {
        using var stream = File.OpenWrite(path);
        Write(stream);
    }

    /// <summary>
    /// Writes the image to a stream
    /// </summary>
    public void Write(Stream stream)
    {
        var byteArray = Data
            .Select(v => (Color)v)
            .SelectMany(c => new[] { c.B, c.G, c.R })
            .ToArray();

        var padding = (4 - (Width * 3 % 4)) % 4;
        var rowSize = Width * 3;
        var dataSize = rowSize * Height;
        var fileSize = 54 + dataSize;

        using var writer = new BinaryWriter(stream);
        writer.Write("BM"u8); // Magic number
        writer.Write(fileSize); // File size
        writer.Write(0); // Reserved
        writer.Write(PIXEL_DATA_OFFSET); // Offset to pixel array
        writer.Write(HEADER_SIZE); // Size of info header
        writer.Write(Width); // Width
        writer.Write(-Height); // Height, negative to flip the image
        writer.Write(PLANE_COUNT); // Planes
        writer.Write(BITS_PER_PIXEL); // Bits per pixel
        writer.Write(0); // Compression
        writer.Write(dataSize); // Image size
        writer.Write(0); // X pixels per meter
        writer.Write(0); // Y pixels per meter
        writer.Write(0); // Colors in color table
        writer.Write(0); // Important colors (0 = all)

        for (var offset = 0; offset < byteArray.Length; offset += rowSize)
        {
            writer.Write(byteArray[offset..(offset + rowSize)]);
            if (padding > 0)
            {
                writer.Write(new byte[padding]);
            }
        }
    }

    private const int PIXEL_DATA_OFFSET = 54;
    private const int HEADER_SIZE = 40;
    private const short BITS_PER_PIXEL = 24;
    private const short PLANE_COUNT = 1;
}
