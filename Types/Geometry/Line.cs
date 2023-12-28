using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace GameUtils.Types.Geometry;

/// <summary>
/// Represents a line from <see cref="Start"/> to <see cref="End"/>
/// </summary>
public readonly record struct Line
{
    /// <summary>
    /// Start point of the line
    /// </summary>
    public readonly Vector2 Start;

    /// <summary>
    /// End point of the line
    /// </summary>
    public readonly Vector2 End;

    /// <summary>
    /// Length of the line
    /// </summary>
    public readonly float Length;

    /// <summary>
    /// Midpoint of the line
    /// </summary>
    public readonly Vector2 Midpoint;

    /// <summary>
    /// Normal of the line (inverse of <see cref="NormalB"/>)
    /// </summary>
    public readonly Vector2 NormalA;

    /// <summary>
    /// Normal of the line (inverse of <see cref="NormalA"/>)
    /// </summary>
    public readonly Vector2 NormalB;

    /// <summary>
    /// Creates a line from <paramref name="start"/> to <paramref name="end"/>
    /// </summary>
    public Line(Vector2 start, Vector2 end)
    {
        Start = start;
        End = end;
        Length = Vector2.Distance(start, end);
        Midpoint = (Start + End) / 2;
        NormalA = new Vector2(-(End.Y - Start.Y), End.X - Start.X);
        NormalB = new Vector2(End.Y - Start.Y, -(End.X - Start.X));
    }

    /// <summary>
    /// Creates a line from <paramref name="start"/> in the direction of <paramref name="direction"/> with a length of <paramref name="length"/>
    /// </summary>
    public Line(Vector2 start, Vector2 direction, float length)
    {
        Start = start;
        End = start + (Vector2.Normalize(direction) * length);
        Length = length;
        Midpoint = (Start + End) / 2;
        NormalA = new Vector2(-(End.Y - Start.Y), End.X - Start.X);
        NormalB = new Vector2(End.Y - Start.Y, -(End.X - Start.X));
    }

    /// <summary>
    /// Creates a line from <paramref name="start"/> in the direction of <paramref name="angle"/> with a length of <paramref name="length"/>
    /// </summary>
    public Line(Vector2 start, float angle, float length)
    {
        var (sin, cos) = MathF.SinCos(angle);
        Start = start;
        End = start + (new Vector2(cos, sin) * length);
        Length = length;
        Midpoint = (Start + End) / 2;
        NormalA = new Vector2(-(End.Y - Start.Y), End.X - Start.X);
        NormalB = new Vector2(End.Y - Start.Y, -(End.X - Start.X));
    }

    /// <summary>
    /// Creates a line from <paramref name="x1"/>, <paramref name="y1"/> to <paramref name="x2"/>, <paramref name="y2"/>
    /// </summary>
    public Line(float x1, float y1, float x2, float y2)
    {
        Start = new(x1, y1);
        End = new(x2, y2);
        Length = Vector2.Distance(Start, End);
        Midpoint = (Start + End) / 2;
        NormalA = new Vector2(-(End.Y - Start.Y), End.X - Start.X);
        NormalB = new Vector2(End.Y - Start.Y, -(End.X - Start.X));
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

    /// <summary>
    /// Returns true if this line intersects the given line and sets <paramref name="intersectionPoint"/> to the intersection point
    /// </summary>
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
