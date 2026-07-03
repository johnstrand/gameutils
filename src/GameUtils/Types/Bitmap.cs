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
    /// Clears the image to the specified color
    /// </summary>
    public void Clear(Color color)
    {
        Clear((Vector3)color);
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
    /// Draw a line with the specified color using Bresenham's line algorithm
    /// </summary>
    public void Line(Vector2 start, Vector2 end, Vector3 color)
    {
        var x0 = (int)start.X;
        var y0 = (int)start.Y;
        var x1 = (int)end.X;
        var y1 = (int)end.Y;

        var dx = System.Math.Abs(x1 - x0);
        var dy = System.Math.Abs(y1 - y0);
        var sx = x0 < x1 ? 1 : -1;
        var sy = y0 < y1 ? 1 : -1;
        var err = dx - dy;

        while (true)
        {
            this[x0, y0] = color;

            if (x0 == x1 && y0 == y1)
            {
                break;
            }

            var e2 = 2 * err;
            if (e2 > -dy)
            {
                err -= dy;
                x0 += sx;
            }

            if (e2 < dx)
            {
                err += dx;
                y0 += sy;
            }
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
        var radiusSq = radius * radius;
        for (var y = position.Y - radius; y < position.Y + radius; y++)
        {
            for (var x = position.X - radius; x < position.X + radius; x++)
            {
                var dx = x - position.X;
                var dy = y - position.Y;
                if ((dx * dx) + (dy * dy) <= radiusSq)
                {
                    this[(int)x, (int)y] = color;
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
        var rowSize = Width * 3;
        var padding = (4 - (rowSize % 4)) % 4;
        var dataSize = (rowSize + padding) * Height;
        var fileSize = 54 + dataSize;

        using var writer = new BinaryWriter(stream);
        writer.Write("BM"u8);
        writer.Write(fileSize);
        writer.Write(0);
        writer.Write(PIXEL_DATA_OFFSET);
        writer.Write(HEADER_SIZE);
        writer.Write(Width);
        writer.Write(-Height);
        writer.Write(PLANE_COUNT);
        writer.Write(BITS_PER_PIXEL);
        writer.Write(0);
        writer.Write(dataSize);
        writer.Write(0);
        writer.Write(0);
        writer.Write(0);
        writer.Write(0);

        var row = new byte[rowSize + padding];
        var padOffset = rowSize;
        for (var y = 0; y < Height; y++)
        {
            var rowStart = (Height - 1 - y) * Width;
            for (var x = 0; x < Width; x++)
            {
                var v = Data[rowStart + x];
                var col = (Color)v;
                var i = x * 3;
                row[i] = col.B;
                row[i + 1] = col.G;
                row[i + 2] = col.R;
            }

            Array.Clear(row, padOffset, padding);
            writer.Write(row);
        }
    }

    private const int PIXEL_DATA_OFFSET = 54;
    private const int HEADER_SIZE = 40;
    private const short BITS_PER_PIXEL = 24;
    private const short PLANE_COUNT = 1;
}
