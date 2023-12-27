using System.IO.Compression;
using System.Numerics;

namespace GameUtils.Types;

/// <summary>
/// Very simple image data class, compressed with GZip
/// </summary>
public class ImageData
{
    /// <summary>
    /// Width of the image in pixels
    /// </summary>
    public int Width { get; }

    /// <summary>
    /// Height of the image in pixels
    /// </summary>
    public int Height { get; }

    /// <summary>
    /// Pixel data, with X = R, Y = G, Z = B, W = A
    /// </summary>
    public Vector4[] Data { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ImageData"/> class, with pixels set to black and transparent
    /// </summary>
    public ImageData(int width, int height)
    {
        Width = width;
        Height = height;
        Data = new Vector4[width * height];
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ImageData"/> class
    /// </summary>
    public ImageData(int width, int height, Vector4[] data)
    {
        if (width <= 0)
        {
            throw new ArgumentException("Width must be greater than zero");
        }

        if (height <= 0)
        {
            throw new ArgumentException("Height must be greater than zero");
        }

        if (data.Length != width * height)
        {
            throw new ArgumentException("Data length must match width * height");
        }

        Width = width;
        Height = height;
        Data = data;
    }

    /// <summary>
    /// Gets or sets a pixel at the specified coordinates. If the coordinates are out of bounds, no operation is performed and Vector4.Zero is returned.
    /// </summary>
    public Vector4 this[int x, int y]
    {
        get => IsInBounds(x, y) ? Data[(y * Width) + x] : Vector4.Zero;
        set
        {
            if (!IsInBounds(x, y))
            {
                return;
            }

            Data[(y * Width) + x] = value;
        }
    }

    /// <summary>
    /// Gets or sets a pixel at the specified coordinates. If the coordinates are out of bounds, no operation is performed and Vector4.Zero is returned.
    /// </summary>
    public Vector4 this[Vector2 point]
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
    public void Write(Stream target)
    {
        using var compressor = new GZipStream(target, CompressionLevel.Optimal);
        using var writer = new BinaryWriter(compressor);
        writer.Write("IMGD"u8);
        writer.Write(Width);
        writer.Write(Height);
        foreach (var pixel in Data)
        {
            writer.Write(pixel.X);
            writer.Write(pixel.Y);
            writer.Write(pixel.Z);
            writer.Write(pixel.W);
        }
    }

    /// <summary>
    /// Reads an image from a file
    /// </summary>
    public static ImageData Read(string path)
    {
        using var stream = File.OpenRead(path);
        return Read(stream);
    }

    /// <summary>
    /// Reads an image from a stream
    /// </summary>
    public static ImageData Read(Stream source)
    {
        using var decompressor = new GZipStream(source, CompressionMode.Decompress);
        using var reader = new BinaryReader(decompressor);
        var magic = reader.ReadUInt32();
        if (magic != 0x44474D49) // IMGD as a little-endian uint32
        {
            throw new Exception("Invalid magic number");
        }

        var width = reader.ReadInt32();
        var height = reader.ReadInt32();
        var data = new Vector4[width * height];
        for (var i = 0; i < data.Length; i++)
        {
            data[i] = new Vector4(
                reader.ReadSingle(),
                reader.ReadSingle(),
                reader.ReadSingle(),
                reader.ReadSingle());
        }

        return new ImageData(width, height, data);
    }
}
