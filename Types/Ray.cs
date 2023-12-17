using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace GameUtils;

public record struct Quad
{
    public Vector2 TopLeft;
    public Vector2 TopRight;
    public Vector2 BottomLeft;
    public Vector2 BottomRight;

    private readonly Ray[] edges = new Ray[4];

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
        edges[0] = new Ray(TopLeft, TopRight);
        edges[1] = new Ray(TopRight, BottomRight);
        edges[2] = new Ray(BottomRight, BottomLeft);
        edges[3] = new Ray(BottomLeft, TopLeft);
    }

    public readonly bool Intersects(Ray ray, [NotNullWhen(true)] out Vector2? nearest)
    {
        return ray.IntersectsAny(edges, out nearest);
    }
}

public record struct Ray
{
    public Vector2 Start;
    public Vector2 End;
    public readonly float Length;

    public Ray(Vector2 start, Vector2 end)
    {
        Start = start;
        End = end;
        Length = Vector2.Distance(start, end);
    }

    public Ray(float x1, float y1, float x2, float y2)
    {
        Start = new(x1, y1);
        End = new(x2, y2);
        Length = Vector2.Distance(Start, End);
    }

    public static Ray Cast(Vector2 start, Vector2 target, float? maxLength = null)
    {
        var direction = target - start;
        var length = direction.Length();
        if (maxLength.HasValue && length > maxLength)
        {
            direction = Vector2.Normalize(direction);
            length = maxLength.Value;
        }

        return new Ray(start, start + (direction * length));
    }

    public readonly bool IntersectsAny(IEnumerable<Ray> rays, [NotNullWhen(true)] out Vector2? nearest)
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

    public readonly bool Intersects(Ray other)
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

    public readonly bool Intersects(Ray other, [NotNullWhen(true)] out Vector2? intersectionPoint)
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

        intersectionPoint = Start + ((alphaNumerator / denominator) * thisDelta);
        return true;
    }
}
