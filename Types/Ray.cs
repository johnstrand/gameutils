using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace GameUtils;

public record struct Quad
{
    public Vector2 TopLeft;
    public Vector2 TopRight;
    public Vector2 BottomLeft;
    public Vector2 BottomRight;

    private readonly Line[] _edges = new Line[4];

    public Quad(Vector2 topLeft, Vector2 topRight, Vector2 bottomLeft, Vector2 bottomRight)
    {
        TopLeft = topLeft;
        TopRight = topRight;
        BottomLeft = bottomLeft;
        BottomRight = bottomRight;

        SetEdges();
    }

    public Quad(float x1, float y1, float x2, float y2, float x3, float y3, float x4, float y4)
    {
        TopLeft = new(x1, y1);
        TopRight = new(x2, y2);
        BottomLeft = new(x3, y3);
        BottomRight = new(x4, y4);

        SetEdges();
    }

    public Quad(float x, float y, float width, float height)
    {
        TopLeft = new(x, y);
        TopRight = new(x + width, y);
        BottomLeft = new(x, y + height);
        BottomRight = new(x + width, y + height);

        SetEdges();
    }

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

    public readonly bool Intersects(Line line, [NotNullWhen(true)] out Vector2? nearest)
    {
        return line.IntersectsAny(_edges, out nearest);
    }
}

public readonly record struct Line
{
    public readonly Vector2 Start;
    public readonly Vector2 End;
    public readonly float Length;
    public readonly Vector2 Midpoint => (Start + End) / 2f;

    /// <summary>
    /// Creates a line from <paramref name="start"/> to <paramref name="end"/>
    /// </summary>
    public Line(Vector2 start, Vector2 end)
    {
        Start = start;
        End = end;
        Length = Vector2.Distance(start, end);
    }

    /// <summary>
    /// Creates a line from <paramref name="start"/> in the direction of <paramref name="direction"/> with a length of <paramref name="length"/>
    /// </summary>
    public Line(Vector2 start, Vector2 direction, float length)
    {
        Start = start;
        End = start + (Vector2.Normalize(direction) * length);
        Length = length;
    }

    /// <summary>
    /// Creates a line from <paramref name="start"/> in the direction of <paramref name="angle"/> with a length of <paramref name="length"/>
    /// </summary>
    public Line(Vector2 start, float angle, float length)
    {
        var (sin, cos) = MathF.SinCos(angle);
        Start = start;
        End = start + new Vector2(cos, sin) * length;
        Length = length;
    }

    /// <summary>
    /// Creates a line from <paramref name="x1"/>, <paramref name="y1"/> to <paramref name="x2"/>, <paramref name="y2"/>
    /// </summary>
    public Line(float x1, float y1, float x2, float y2)
    {
        Start = new(x1, y1);
        End = new(x2, y2);
        Length = Vector2.Distance(Start, End);
    }

    /// <summary>
    /// Gets the normal vectors for this line
    /// </summary>
    public readonly (Vector2 a, Vector2 b) GetNormals()
    {
        var direction = Vector2.Normalize(End - Start);
        var normal = new Vector2(-direction.Y, direction.X);
        return (normal, -normal);
    }

    /// <summary>
    /// Casts a line from <paramref name="start"/> to <paramref name="target"/> with an optional maximum length of <paramref name="maxLength"/>
    /// </summary>
    public static Line Cast(Vector2 start, Vector2 target, float? maxLength = null)
    {
        var direction = target - start;
        var length = direction.Length();
        if (maxLength.HasValue && length > maxLength)
        {
            direction = Vector2.Normalize(direction);
            length = maxLength.Value;
        }

        return new Line(start, start + (direction * length));
    }

    /// <summary>
    /// Checks if this line intersects any of the given <paramref name="rays"/> and returns the nearest intersection point in <paramref name="nearest"/>
    /// </summary>
    public readonly bool IntersectsAny(IEnumerable<Line> rays, [NotNullWhen(true)] out Vector2? nearest)
    {
        nearest = null;
        var nearestDistance = float.MaxValue;
        foreach (var ray in rays)
        {
            if (Intersects(ray, out var intersectionPoint))
            {
                var distance = Vector2.Distance(Start, intersectionPoint.Value);
                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearest = intersectionPoint;
                }
            }
        }

        return nearest.HasValue;
    }

    /// <summary>
    /// Checks if this line intersects the given line
    /// </summary>
    public readonly bool Intersects(Line other)
    {
        var a = End - Start;
        var b = other.End - other.Start;
        var c = other.Start - Start;

        var alphaNumerator = (b.Y * c.X) - (b.X * c.Y);
        var betaNumerator = (a.X * c.Y) - (a.Y * c.X);
        var denominator = (a.Y * b.X) - (a.X * b.Y);

        if (denominator == 0)
        {
            return false;
        }

        if (denominator > 0)
        {
            if (alphaNumerator < 0 || alphaNumerator > denominator || betaNumerator < 0 || betaNumerator > denominator)
            {
                return false;
            }
        }
        else if (alphaNumerator > 0 || alphaNumerator < denominator || betaNumerator > 0 || betaNumerator < denominator)
        {
            return false;
        }

        return true;
    }

    public readonly bool Intersects(Line other, [NotNullWhen(true)] out Vector2? intersectionPoint)
    {
        var thisDelta = End - Start;
        var otherDelta = other.End - other.Start;
        var startDelta = other.Start - Start;

        var alphaNumerator = (otherDelta.Y * startDelta.X) - (otherDelta.X * startDelta.Y);
        var betaNumerator = (thisDelta.X * startDelta.Y) - (thisDelta.Y * startDelta.X);
        var denominator = (thisDelta.Y * otherDelta.X) - (thisDelta.X * otherDelta.Y);

        if (denominator == 0)
        {
            intersectionPoint = null;
            return false;
        }

        if (denominator > 0)
        {
            if (alphaNumerator < 0 || alphaNumerator > denominator || betaNumerator < 0 || betaNumerator > denominator)
            {
                intersectionPoint = null;
                return false;
            }
        }
        else if (alphaNumerator > 0 || alphaNumerator < denominator || betaNumerator > 0 || betaNumerator < denominator)
        {
            intersectionPoint = null;
            return false;
        }

        intersectionPoint = Start + (alphaNumerator / denominator * thisDelta);
        return true;
    }
}
