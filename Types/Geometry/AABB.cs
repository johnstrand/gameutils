using System.Numerics;

namespace GameUtils.Types.Geometry;

public class AABB
{
    public Vector2 Min { get; }
    public Vector2 Max { get; }
    public Vector2 Center { get; }
    public Vector2 Size { get; }

    public AABB(Vector2 min, Vector2 max)
    {
        Min = min;
        Max = max;
        Center = (Min + Max) / 2f;
        Size = Max - Min;
    }

    public bool Contains(Vector2 point)
    {
        return point.X >= Min.X && point.X <= Max.X && point.Y >= Min.Y && point.Y <= Max.Y;
    }

    public bool Intersects(AABB other)
    {
        return Min.X <= other.Max.X && Max.X >= other.Min.X && Min.Y <= other.Max.Y && Max.Y >= other.Min.Y;
    }

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

    public bool Intersects(Polygon2D polygon)
    {
        return polygon.Vertices.Any(Contains) || polygon.Edges.Any(Intersects);
    }

    public bool Intersects(Circle circle)
    {
        var closest = Vector2.Clamp(circle.Center, Min, Max);
        var distance = Vector2.DistanceSquared(circle.Center, closest);
        return distance <= circle.Radius * circle.Radius;
    }

    public bool Intersects(Vector2 point)
    {
        return Contains(point);
    }

    public bool Intersects(Vector2 start, Vector2 end)
    {
        return Intersects(new Line(start, end));
    }

    public bool Intersects(Vector2 center, float radius)
    {
        return Intersects(new Circle(center, radius));
    }

    public bool Intersects(Vector2[] vertices)
    {
        return vertices.Any(Contains);
    }

    public bool Intersects(Vector2[] vertices, bool sort)
    {
        return Intersects(new Polygon2D(vertices, sort));
    }
}