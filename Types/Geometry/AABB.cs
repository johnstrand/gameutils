using System.Numerics;

namespace GameUtils.Types.Geometry;

/// <summary>
/// Axis-aligned bounding box
/// </summary>
public class AABB
{
    /// <summary>
    /// Minimum point of the box
    /// </summary>
    public Vector2 Min { get; }

    /// <summary>
    /// Maximum point of the box
    /// </summary>
    public Vector2 Max { get; }

    /// <summary>
    /// Center point of the box (average of Min and Max)
    /// </summary>
    public Vector2 Center { get; }

    /// <summary>
    /// Size of the box (Max - Min)
    /// </summary>
    public Vector2 Size { get; }

    /// <summary>
    /// Creates a new AABB from the specified minimum and maximum points
    /// </summary>
    public AABB(Vector2 min, Vector2 max)
    {
        Min = min;
        Max = max;
        Center = (Min + Max) / 2f;
        Size = Max - Min;
    }

    /// <summary>
    /// Returns true if the specified point is inside the box
    /// </summary>
    public bool Contains(Vector2 point)
    {
        return point.X >= Min.X && point.X <= Max.X && point.Y >= Min.Y && point.Y <= Max.Y;
    }

    /// <summary>
    /// Returns true if the specified box intersects with, or is inside the box
    /// </summary>
    public bool Intersects(AABB other)
    {
        return Min.X <= other.Max.X && Max.X >= other.Min.X && Min.Y <= other.Max.Y && Max.Y >= other.Min.Y;
    }

    /// <summary>
    /// Returns true if the box intersects with the specified box, and returns the intersection points
    /// </summary>
    public bool Intersects(AABB other, out Vector2[] intersectionPoints)
    {
        intersectionPoints = [];
        if (!Intersects(other))
        {
            return false;
        }

        var points = new Vector2[4];
        var index = 0;
        if (Min.X <= other.Min.X && Max.X >= other.Min.X)
        {
            points[index++] = new Vector2(other.Min.X, Min.Y);
        }

        if (Min.X <= other.Max.X && Max.X >= other.Max.X)
        {
            points[index++] = new Vector2(other.Max.X, Min.Y);
        }

        if (Min.Y <= other.Min.Y && Max.Y >= other.Min.Y)
        {
            points[index++] = new Vector2(Min.X, other.Min.Y);
        }

        if (Min.Y <= other.Max.Y && Max.Y >= other.Max.Y)
        {
            points[index++] = new Vector2(Min.X, other.Max.Y);
        }

        intersectionPoints = points[..index];
        return true;
    }

    /// <summary>
    /// Returns true if the specified line intersects with the box
    /// </summary>
    public bool Intersects(Line line)
    {
        var dir = line.End - line.Start;
        var tmin = (Min - line.Start) / dir;
        var tmax = (Max - line.Start) / dir;
        var t1 = Vector2.Min(tmin, tmax);
        var t2 = Vector2.Max(tmin, tmax);
        var tNear = MathF.Max(t1.X, t1.Y);
        var tFar = MathF.Min(t2.X, t2.Y);
        return tNear <= tFar && tFar >= 0f && tNear <= 1f;
    }

    /// <summary>
    /// Returns true if the specified line intersects with the box, and returns the intersection point(s)
    /// </summary>
    public bool Intersects(Line line, out Vector2[] intersectionPoints)
    {
        var dir = line.End - line.Start;
        var tmin = (Min - line.Start) / dir;
        var tmax = (Max - line.Start) / dir;
        var t1 = Vector2.Min(tmin, tmax);
        var t2 = Vector2.Max(tmin, tmax);
        var tNear = MathF.Max(t1.X, t1.Y);
        var tFar = MathF.Min(t2.X, t2.Y);
        if (tNear <= tFar && tFar >= 0f && tNear <= 1f)
        {
            var points = new Vector2[2];
            var index = 0;
            if (tNear >= 0f)
            {
                points[index++] = line.Start + (dir * tNear);
            }

            if (tFar <= 1f)
            {
                points[index++] = line.Start + (dir * tFar);
            }

            intersectionPoints = points[..index];
            return true;
        }

        intersectionPoints = [];
        return false;
    }

    /// <summary>
    /// Returns true if the specified polygon intersects with the box
    /// </summary>
    public bool Intersects(Polygon2D polygon)
    {
        return polygon.Vertices.Any(Contains) || polygon.Edges.Any(Intersects);
    }

    /// <summary>
    /// Returns true if the specified circle intersects with the box
    /// </summary>
    public bool Intersects(Circle circle)
    {
        var closest = Vector2.Clamp(circle.Center, Min, Max);
        var distance = Vector2.DistanceSquared(circle.Center, closest);
        return distance <= circle.Radius * circle.Radius;
    }

    /// <summary>
    /// Returns true if the specified line intersects with the box
    /// </summary>
    public bool Intersects(Vector2 start, Vector2 end)
    {
        return Intersects(new Line(start, end));
    }

    /// <summary>
    /// Returns true if the specified circle intersects with the box
    /// </summary>
    public bool Intersects(Vector2 center, float radius)
    {
        return Intersects(new Circle(center, radius));
    }

    /// <summary>
    /// Returns true if any of the specified points are inside the box
    /// </summary>
    public bool Intersects(Vector2[] vertices)
    {
        return vertices.Any(Contains);
    }

    /// <summary>
    /// Returns true if the specified polygon intersects with the box
    /// </summary>
    public bool Intersects(Vector2[] vertices, bool sort)
    {
        return Intersects(new Polygon2D(vertices, sort));
    }
}