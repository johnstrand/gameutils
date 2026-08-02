using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace GameUtils.Types.Geometry;

/// <summary>
/// Represents a 2D polygon
/// </summary>
public readonly struct Polygon2D
{
    // S3887: Arrays are intentionally mutable — TranslateBy and other methods modify them in place
#pragma warning disable S3887
    /// <summary>
    /// Vertices of the polygon
    /// </summary>
    public readonly Vector2[] Vertices;

    /// <summary>
    /// Edges of the polygon
    /// </summary>
    public readonly Line[] Edges;

    /// <summary>
    /// Normals of each edge of the polygon
    /// </summary>
    public readonly Vector2[] Normals;

    private readonly AABB[] _boundingBox;
#pragma warning restore S3887

    /// <summary>
    /// The axis-aligned bounding box of the polygon
    /// </summary>
    public AABB BoundingBox => _boundingBox[0];

    /// <summary>
    /// Creates a new polygon from the specified vertices. If <paramref name="sort"/> is true, the vertices will be sorted clockwise before creating the polygon.
    /// </summary>
    /// <exception cref="ArgumentException">Thrown when <paramref name="vertices"/> is empty.</exception>
    public Polygon2D(Vector2[] vertices, bool sort = true)
    {
        if (vertices.Length == 0)
        {
            throw new ArgumentException("Vertices array must not be empty.", nameof(vertices));
        }

        Vertices = sort ? SortClockwise(vertices) : [.. vertices];

        Edges = new Line[Vertices.Length];
        Normals = new Vector2[Vertices.Length];

        var min = new Vector2(float.MaxValue);
        var max = new Vector2(float.MinValue);

        for (var i = 0; i < Vertices.Length; i++)
        {
            Edges[i] = new Line(Vertices[i], Vertices[(i + 1) % Vertices.Length]);
            Normals[i] = Vector2.Normalize(new Vector2(Edges[i].End.Y - Edges[i].Start.Y, Edges[i].Start.X - Edges[i].End.X));
            min = Vector2.Min(min, Vertices[i]);
            max = Vector2.Max(max, Vertices[i]);
        }

        _boundingBox = [new AABB(min, max)];
    }

    private static Vector2[] SortClockwise(Vector2[] vertices)
    {
        var sum = Vector2.Zero;
        for (var i = 0; i < vertices.Length; i++)
        {
            sum += vertices[i];
        }

        var center = sum / vertices.Length;

        var sorted = new Vector2[vertices.Length];
        Array.Copy(vertices, sorted, vertices.Length);
        Array.Sort(sorted, (a, b) => MathF.Atan2(a.Y - center.Y, a.X - center.X).CompareTo(MathF.Atan2(b.Y - center.Y, b.X - center.X)));

        return sorted;
    }

    /// <summary>
    /// Returns true if the specified point is inside the polygon
    /// </summary>
    public bool Contains(Vector2 point)
    {
        var inside = false;
        var j = Vertices.Length - 1;
        for (var i = 0; i < Vertices.Length; j = i++)
        {
            if ((Vertices[i].Y > point.Y) != (Vertices[j].Y > point.Y) &&
                point.X < ((Vertices[j].X - Vertices[i].X) * (point.Y - Vertices[i].Y) / (Vertices[j].Y - Vertices[i].Y)) + Vertices[i].X)
            {
                inside = !inside;
            }
        }

        return inside;
    }

    /// <summary>
    /// Moves the polygon by the specified translation
    /// </summary>
    public void TranslateBy(Vector2 translation)
    {
        var min = new Vector2(float.MaxValue);
        var max = new Vector2(float.MinValue);
        var length = Vertices.Length;

        var prevVert = Vertices[0] + translation;
        Vertices[0] = prevVert;
        min = Vector2.Min(min, prevVert);
        max = Vector2.Max(max, prevVert);

        for (var i = 1; i < length; i++)
        {
            var vert = Vertices[i] + translation;
            Vertices[i] = vert;

            min = Vector2.Min(min, vert);
            max = Vector2.Max(max, vert);

            Edges[i - 1] = new Line(prevVert, vert);
            prevVert = vert;
        }

        Edges[length - 1] = new Line(prevVert, Vertices[0]);

        _boundingBox[0] = new AABB(min, max);
    }

    /// <summary>
    /// Returns true if the specified polygon intersects with this polygon
    /// </summary>
    public bool Intersects(Polygon2D other)
    {
        for (var i = 0; i < Vertices.Length; i++)
        {
            var a1 = Vertices[i];
            var a2 = Vertices[(i + 1) % Vertices.Length];
            for (var j = 0; j < other.Vertices.Length; j++)
            {
                var b1 = other.Vertices[j];
                var b2 = other.Vertices[(j + 1) % other.Vertices.Length];
                if (Intersects(a1, a2, b1, b2))
                {
                    return true;
                }
            }
        }

        return false;
    }

    /// <summary>
    /// Returns true if the specified polygon intersects with this polygon. If true, <paramref name="intersectionPoint"/> will contain the point of intersection.
    /// If multiple intersections occur, the first one will be returned, not necessarily the closest.
    /// </summary>
    public bool Intersects(Polygon2D other, [NotNullWhen(true)] out Vector2? intersectionPoint)
    {
        intersectionPoint = null;
        for (var i = 0; i < Vertices.Length; i++)
        {
            var a1 = Vertices[i];
            var a2 = Vertices[(i + 1) % Vertices.Length];
            for (var j = 0; j < other.Vertices.Length; j++)
            {
                var b1 = other.Vertices[j];
                var b2 = other.Vertices[(j + 1) % other.Vertices.Length];
                if (Intersects(a1, a2, b1, b2, out intersectionPoint))
                {
                    return true;
                }
            }
        }

        return false;
    }

    /// <summary>
    /// Returns true if the specified circle intersects with this polygon
    /// </summary>
    public bool Intersects(Line line)
    {
        for (var i = 0; i < Edges.Length; i++)
        {
            if (Intersects(Edges[i].Start, Edges[i].End, line.Start, line.End))
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Returns true if the specified circle intersects with this polygon. If true, <paramref name="intersectionPoint"/> will contain the point of intersection.
    /// If multiple intersections occur, the first one will be returned, not necessarily the closest.
    /// </summary>
    public bool Intersects(Line line, [NotNullWhen(true)] out Vector2? intersectionPoint)
    {
        intersectionPoint = null;
        foreach (var edge in Edges)
        {
            if (Intersects(edge.Start, edge.End, line.Start, line.End, out intersectionPoint))
            {
                return true;
            }
        }

        return false;
    }

    private static bool Intersects(Vector2 a1, Vector2 a2, Vector2 b1, Vector2 b2)
    {
        var d1 = Direction(b1, b2, a1);
        var d2 = Direction(b1, b2, a2);
        var d3 = Direction(a1, a2, b1);
        var d4 = Direction(a1, a2, b2);
        return (d1 != d2 && d3 != d4) || (d1 == 0 && OnSegment(b1, b2, a1)) || (d2 == 0 && OnSegment(b1, b2, a2)) || (d3 == 0 && OnSegment(a1, a2, b1)) || (d4 == 0 && OnSegment(a1, a2, b2));
    }

    private static bool Intersects(Vector2 a1, Vector2 a2, Vector2 b1, Vector2 b2, [NotNullWhen(true)] out Vector2? intersectionPoint)
    {
        intersectionPoint = null;
        var d1 = Direction(b1, b2, a1);
        var d2 = Direction(b1, b2, a2);
        var d3 = Direction(a1, a2, b1);
        var d4 = Direction(a1, a2, b2);
        if ((d1 != d2 && d3 != d4) || (d1 == 0 && OnSegment(b1, b2, a1)) || (d2 == 0 && OnSegment(b1, b2, a2)) || (d3 == 0 && OnSegment(a1, a2, b1)) || (d4 == 0 && OnSegment(a1, a2, b2)))
        {
            var u = (((b2.X - b1.X) * (a1.Y - b1.Y)) - ((b2.Y - b1.Y) * (a1.X - b1.X))) / (((b2.Y - b1.Y) * (a2.X - a1.X)) - ((b2.X - b1.X) * (a2.Y - a1.Y)));
            intersectionPoint = new Vector2(a1.X + (u * (a2.X - a1.X)), a1.Y + (u * (a2.Y - a1.Y)));
            return true;
        }

        return false;
    }

    private static bool OnSegment(Vector2 a, Vector2 b, Vector2 c)
    {
        return MathF.Min(a.X, b.X) <= c.X && c.X <= MathF.Max(a.X, b.X) && MathF.Min(a.Y, b.Y) <= c.Y && c.Y <= MathF.Max(a.Y, b.Y);
    }

    private static int Direction(Vector2 a, Vector2 b, Vector2 c)
    {
        return MathF.Sign(((b.X - a.X) * (c.Y - a.Y)) - ((c.X - a.X) * (b.Y - a.Y)));
    }
}