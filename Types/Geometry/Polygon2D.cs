using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace GameUtils.Types.Geometry;

/// <summary>
/// Represents a 2D polygon
/// </summary>
public readonly struct Polygon2D
{
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

    /// <summary>
    /// Creates a new polygon from the specified vertices. If <paramref name="sort"/> is true, the vertices will be sorted clockwise before creating the polygon.
    /// </summary>
    public Polygon2D(Vector2[] vertices, bool sort = true)
    {
        Vertices = sort ? SortClockwise(vertices) : [.. vertices];

        Edges = new Line[Vertices.Length];
        Normals = new Vector2[Vertices.Length];

        for (var i = 0; i < Vertices.Length; i++)
        {
            Edges[i] = new Line(Vertices[i], Vertices[(i + 1) % Vertices.Length]);
            Normals[i] = Vector2.Normalize(new Vector2(Edges[i].End.Y - Edges[i].Start.Y, Edges[i].Start.X - Edges[i].End.X));
        }
    }

    private static Vector2[] SortClockwise(Vector2[] vertices)
    {
        var center = new Vector2(vertices.Sum(v => v.X) / vertices.Length, vertices.Sum(v => v.Y) / vertices.Length);
        return [.. vertices.OrderBy(v => MathF.Atan2(v.Y - center.Y, v.X - center.X))];
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

    // TODO: Implement these, they need to take the center of the polygon into account, as well as recalculating the normals
    /*
    public void RotateBy(float angle)
    {
        for (var i = 0; i < Vertices.Length; i++)
        {
            Vertices[i] = Vector2.Transform(Vertices[i], Matrix3x2.CreateRotation(angle));
        }
    }

    public void RotateTo(float angle)
    {
        var center = new Vector2(Vertices.Sum(v => v.X) / Vertices.Length, Vertices.Sum(v => v.Y) / Vertices.Length);
        for (var i = 0; i < Vertices.Length; i++)
        {
            Vertices[i] = Vector2.Transform(Vertices[i] - center, Matrix3x2.CreateRotation(angle)) + center;
        }
    }
    */

    /// <summary>
    /// Moves the polygon by the specified translation
    /// </summary>
    public void TranslateBy(Vector2 translation)
    {
        for (var i = 0; i < Vertices.Length; i++)
        {
            Vertices[i] += translation;
        }
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
        foreach (var edge in Edges)
        {
            if (Intersects(edge.Start, edge.End, line.Start, line.End))
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