using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace GameUtils.Types.Geometry;

/// <summary>
/// Represents a 2D quad
/// </summary>
public record struct Quad
{
    /// <summary>
    /// Top left corner of the quad
    /// </summary>
    public Vector2 TopLeft;

    /// <summary>
    /// Top right corner of the quad
    /// </summary>
    public Vector2 TopRight;

    /// <summary>
    /// Bottom left corner of the quad
    /// </summary>
    public Vector2 BottomLeft;

    /// <summary>
    /// Bottom right corner of the quad
    /// </summary>
    public Vector2 BottomRight;

    private readonly Line[] _edges = new Line[4];

    /// <summary>
    /// Gets the vertex at the specified index (clockwise order)
    /// </summary>
    /// <exception cref="IndexOutOfRangeException"></exception>
    public readonly Vector2 this[int index]
    {
        get
        {
            if (index < 0 || index > 3)
            {
                throw new IndexOutOfRangeException();
            }

            return index switch
            {
                0 => TopLeft,
                1 => TopRight,
                2 => BottomLeft,
                3 => BottomRight,
                _ => Vector2.Zero
            };
        }
    }

    /// <summary>
    /// Constructs a quad from the given corners
    /// </summary>
    public Quad(Vector2 topLeft, Vector2 topRight, Vector2 bottomLeft, Vector2 bottomRight)
    {
        TopLeft = topLeft;
        TopRight = topRight;
        BottomLeft = bottomLeft;
        BottomRight = bottomRight;

        SetEdges();
    }

    /// <summary>
    /// Constructs a quad from the given corners
    /// </summary>
    public Quad(float x1, float y1, float x2, float y2, float x3, float y3, float x4, float y4)
    {
        TopLeft = new(x1, y1);
        TopRight = new(x2, y2);
        BottomLeft = new(x3, y3);
        BottomRight = new(x4, y4);

        SetEdges();
    }

    /// <summary>
    /// Constructs a quad from the given position and size
    /// </summary>
    public Quad(float x, float y, float width, float height)
    {
        TopLeft = new(x, y);
        TopRight = new(x + width, y);
        BottomLeft = new(x, y + height);
        BottomRight = new(x + width, y + height);

        SetEdges();
    }

    /// <summary>
    /// Constructs a quad from the given position and size
    /// </summary>
    public Quad(Vector2 position, Vector2 size)
    {
        TopLeft = position;
        TopRight = position + new Vector2(size.X, 0);
        BottomLeft = position + new Vector2(0, size.Y);
        BottomRight = position + size;

        SetEdges();
    }

    private readonly void SetEdges()
    {
        _edges[0] = new Line(TopLeft, TopRight);
        _edges[1] = new Line(TopRight, BottomRight);
        _edges[2] = new Line(BottomRight, BottomLeft);
        _edges[3] = new Line(BottomLeft, TopLeft);
    }

    /// <summary>
    /// Returns true if the given point is inside the quad
    /// </summary>
    public readonly bool Intersects(Line line, [NotNullWhen(true)] out Vector2? nearest)
    {
        return line.IntersectsAny(_edges, out nearest);
    }
}
