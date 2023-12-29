using System.Numerics;

namespace GameUtils.Types.Collections;

/// <summary>
/// A 2D grid of values
/// </summary>
public class Grid<T>
{
    /// <summary>
    /// Width of the grid
    /// </summary>
    public int Width { get; }

    /// <summary>
    /// Height of the grid
    /// </summary>
    public int Height { get; }

    private readonly T[] _data;

    /// <summary>
    /// Creates a new grid with the specified width and height
    /// </summary>
    public Grid(int width, int height)
    {
        Width = width;
        Height = height;
        _data = new T[width * height];
    }

    /// <summary>
    /// Creates a new grid with the specified width and height, and fills it with the specified value
    /// </summary>
    public Grid(int width, int height, T[] data)
    {
        if (data.Length != width * height)
        {
            throw new ArgumentException("Data length must be equal to width * height");
        }
        Width = width;
        Height = height;
        _data = data;
    }

    /// <summary>
    /// Gets or sets the value at the specified position, will throw an exception if the position is out of bounds
    /// </summary>
    public T this[int x, int y]
    {
        get => _data[IndexOf(x, y)];
        set => _data[IndexOf(x, y)] = value;
    }

    /// <summary>
    /// Gets or sets the value at the specified position, will throw an exception if the position is out of bounds
    /// </summary>
    public T this[Vector2 pos]
    {
        get => _data[IndexOf(pos)];
        set => _data[IndexOf(pos)] = value;
    }

    /// <summary>
    /// Tries to get the value at the specified position, will return false if the position is out of bounds
    /// </summary>
    public bool TryGet(int x, int y, out T? value)
    {
        if (IsInBounds(x, y))
        {
            value = _data[IndexOf(x, y)];
            return true;
        }

        value = default;
        return false;
    }

    /// <summary>
    /// Tries to get the value at the specified position, will return false if the position is out of bounds
    /// </summary>
    public bool TryGet(Vector2 pos, out T? value)
    {
        if (IsInBounds((int)pos.X, (int)pos.Y))
        {
            value = _data[IndexOf(pos)];
            return true;
        }
        value = default;
        return false;
    }

    /// <summary>
    /// Tries to set the value at the specified position, will return false if the position is out of bounds
    /// </summary>
    public bool TrySet(int x, int y, T value)
    {
        if (IsInBounds(x, y))
        {
            _data[IndexOf(x, y)] = value;
            return true;
        }
        return false;
    }

    /// <summary>
    /// Tries to set the value at the specified position, will return false if the position is out of bounds
    /// </summary>
    public bool TrySet(Vector2 pos, T value)
    {
        if (IsInBounds((int)pos.X, (int)pos.Y))
        {
            _data[IndexOf(pos)] = value;
            return true;
        }
        return false;
    }

    private int IndexOf(int x, int y)
    {
        return x + (y * Width);
    }

    private int IndexOf(Vector2 pos)
    {
        return (int)pos.X + ((int)pos.Y * Width);
    }

    /// <summary>
    /// Returns true if the specified position is within the bounds of the grid
    /// </summary>
    public bool IsInBounds(int x, int y)
    {
        return x >= 0 && x < Width && y >= 0 && y < Height;
    }

    /// <summary>
    /// Returns true if the specified position is within the bounds of the grid
    /// </summary>
    public bool IsInBounds(Vector2 pos)
    {
        return pos.X >= 0 && pos.X < Width && pos.Y >= 0 && pos.Y < Height;
    }

    /// <summary>
    /// Resets all values in the grid to their default value
    /// </summary>
    public void Clear()
    {
        Array.Clear(_data, 0, _data.Length);
    }

    /// <summary>
    /// Fills the grid with the specified value
    /// </summary>
    public Grid<T> Fill(T value)
    {
        for (var i = 0; i < _data.Length; i++)
        {
            _data[i] = value;
        }

        return this;
    }

    /// <summary>
    /// Fills the grid with the output of the specified function
    /// </summary>
    public Grid<T> Fill(Func<int, int, T> factory)
    {
        for (var y = 0; y < Height; y++)
        {
            for (var x = 0; x < Width; x++)
            {
                _data[IndexOf(x, y)] = factory(x, y);
            }
        }

        return this;
    }

    /// <summary>
    /// Fills the grid with the output of the specified function
    /// </summary>
    public Grid<T> Fill(Func<Vector2, T> factory)
    {
        return Fill((x, y) => factory(new Vector2(x, y)));
    }
}
