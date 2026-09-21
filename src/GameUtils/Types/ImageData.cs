using System.IO.Compression;
using System.Numerics;
using System.Runtime.InteropServices;

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
    public void Write(string path, string? baseDirectory = null)
    {
        var validatedPath = ValidatePath(path, baseDirectory, isWrite: true);
        using var stream = File.OpenWrite(validatedPath);
        Write(stream);
    }

    /// <summary>
    /// Writes the image to a stream
    /// </summary>
    public void Write(Stream target)
    {
        using var compressor = new GZipStream(target, CompressionLevel.Optimal, leaveOpen: true);
        using var writer = new BinaryWriter(compressor, System.Text.Encoding.UTF8, leaveOpen: true);
        writer.Write("IMGD"u8);
        writer.Write(Width);
        writer.Write(Height);
        writer.Flush();
        ReadOnlySpan<byte> bytes = MemoryMarshal.AsBytes(Data.AsSpan());
        compressor.Write(bytes);
    }

    /// <summary>
    /// Reads an image from a file
    /// </summary>
    public static ImageData Read(string path, string? baseDirectory = null)
    {
        var validatedPath = ValidatePath(path, baseDirectory, isWrite: false);
        using var stream = File.OpenRead(validatedPath);
        return Read(stream);
    }

    /// <summary>
    /// Reads an image from a stream
    /// </summary>
    public static ImageData Read(Stream source)
    {
        using var decompressor = new GZipStream(source, CompressionMode.Decompress, leaveOpen: true);
        using var reader = new BinaryReader(decompressor, System.Text.Encoding.UTF8, leaveOpen: true);
        var magic = reader.ReadUInt32();
        if (magic != 0x44474D49) // IMGD as a little-endian uint32
        {
            throw new InvalidDataException("Invalid magic number");
        }

        var width = reader.ReadInt32();
        var height = reader.ReadInt32();
        if (width <= 0 || height <= 0)
        {
            throw new InvalidDataException("Width and height must be positive");
        }

        long totalPixels = (long)width * height;
        if (totalPixels > 67_108_864) // 1 GB max memory allocation (16 bytes per Vector4)
        {
            throw new InvalidDataException("Image size exceeds maximum allowed limit of 1GB");
        }

        var data = new Vector4[(int)totalPixels];
        Span<byte> bytes = MemoryMarshal.AsBytes(data.AsSpan());
        decompressor.ReadExactly(bytes);

        return new ImageData(width, height, data);
    }

    private static string ValidatePath(string path, string? baseDirectory, bool isWrite)
    {
        var baseDir = GetCanonicalPath(baseDirectory ?? Environment.CurrentDirectory);
        var baseDirWithSeparator = baseDir.EndsWith(Path.DirectorySeparatorChar) || baseDir.EndsWith(Path.AltDirectorySeparatorChar)
            ? baseDir
            : baseDir + Path.DirectorySeparatorChar;

        var fullPath = Path.GetFullPath(path, baseDir);
        var resolvedPath = GetCanonicalPath(fullPath);

        var comparison = RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
            ? StringComparison.OrdinalIgnoreCase
            : StringComparison.Ordinal;

        var baseDirTrimmed = baseDir.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);

        if (!resolvedPath.Equals(baseDirTrimmed, comparison) &&
            !resolvedPath.StartsWith(baseDirWithSeparator, comparison))
        {
            throw new UnauthorizedAccessException(isWrite
                ? "Cannot write outside the current directory."
                : "Cannot read outside the current directory.");
        }

        return resolvedPath;
    }

    private static string GetCanonicalPath(string path)
    {
        var fullPath = Path.GetFullPath(path);
        try
        {
            if (File.Exists(fullPath))
            {
                var target = new FileInfo(fullPath).ResolveLinkTarget(returnFinalTarget: true);
                if (target != null)
                {
                    return Path.GetFullPath(target.FullName);
                }
            }
            else if (Directory.Exists(fullPath))
            {
                var target = new DirectoryInfo(fullPath).ResolveLinkTarget(returnFinalTarget: true);
                if (target != null)
                {
                    return Path.GetFullPath(target.FullName);
                }
            }
            else
            {
                var parent = Path.GetDirectoryName(fullPath);
                if (!string.IsNullOrEmpty(parent) && Directory.Exists(parent))
                {
                    var target = new DirectoryInfo(parent).ResolveLinkTarget(returnFinalTarget: true);
                    if (target != null)
                    {
                        return Path.GetFullPath(Path.Combine(target.FullName, Path.GetFileName(fullPath)));
                    }
                }
            }
        }
        catch
        {
            // Fallback to fullPath if symlink resolution is not supported or fails
        }

        return fullPath;
    }
}
