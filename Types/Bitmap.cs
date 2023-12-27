using System.Numerics;

namespace GameUtils.Types;

/// <summary>
/// Bitmap image class
/// </summary>
[Obsolete("This class is still buggy, don't use it yet.")]
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
        Console.WriteLine($"Padding: {padding}");
        var rowSize = (Width * 3) + padding;
        Console.WriteLine($"Row size: {rowSize}");
        var dataSize = rowSize * Height;
        Console.WriteLine($"Data size: {dataSize}");
        var fileSize = 54 + dataSize;
        Console.WriteLine($"File size: {fileSize}");

        using var writer = new BinaryWriter(stream);
        writer.Write("BM"u8); // Magic number
        writer.Write(fileSize); // File size
        writer.Write(0); // Reserved
        writer.Write(PIXEL_DATA_OFFSET); // Offset to pixel array
        writer.Write(HEADER_SIZE); // Size of info header
        writer.Write(Width); // Width
        writer.Write(Height); // Height
        writer.Write(PLANE_COUNT); // Planes
        writer.Write(BITS_PER_PIXEL); // Bits per pixel
        writer.Write(0); // Compression
        writer.Write(dataSize); // Image size
        writer.Write(PIXELS_PER_METER); // X pixels per meter
        writer.Write(PIXELS_PER_METER); // Y pixels per meter
        writer.Write(0); // Colors in color table
        writer.Write(0); // Important colors (0 = all)

        for (var offset = 0; offset < byteArray.Length; offset += Width)
        {
            writer.Write(byteArray[offset..(offset + Width)]);
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
    private const int PIXELS_PER_METER = 2835;
}