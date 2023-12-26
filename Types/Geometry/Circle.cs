using System.Numerics;

namespace GameUtils.Types.Geometry;

public class Circle
{
    public Vector2 Center { get; }
    public float Radius { get; }

    public Circle(Vector2 center, float radius)
    {
        Center = center;
        Radius = radius;
    }

    public bool Contains(Vector2 point)
    {
        return Vector2.DistanceSquared(Center, point) <= Radius * Radius;
    }

    public bool Intersects(AABB aabb)
    {
        var closest = Vector2.Clamp(Center, aabb.Min, aabb.Max);
        var distance = Vector2.DistanceSquared(Center, closest);
        return distance <= Radius * Radius;
    }

    public bool Intersects(Line line)
    {
        var dir = line.End - line.Start;
        var t = Vector2.Dot(Center - line.Start, dir) / Vector2.Dot(dir, dir);
        t = Math.Clamp(t, 0f, 1f);
        var closest = line.Start + dir * t;
        var distance = Vector2.DistanceSquared(Center, closest);
        return distance <= Radius * Radius;
    }

    public bool Intersects(Polygon2D polygon)
    {
        return polygon.Vertices.Any(Contains) || polygon.Edges.Any(Intersects);
    }

    public bool Intersects(Circle circle)
    {
        var distance = Vector2.DistanceSquared(Center, circle.Center);
        return distance <= (Radius + circle.Radius) * (Radius + circle.Radius);
    }

    public bool Intersects(Vector2 point)
    {
        return Contains(point);
    }

    public bool Intersects(Vector2 start, Vector2 end)
    {
        return Intersects(new Line(start, end));
    }
}