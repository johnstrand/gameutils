using System.Numerics;

namespace GameUtils.Types.Geometry;

/// <summary>
/// A circle
/// </summary>
/// <remarks>
/// Constructs a new circle
/// </remarks>
public class Circle(Vector2 center, float radius)
{
    /// <summary>
    /// Center of the circle
    /// </summary>
    public Vector2 Center { get; } = center;

    /// <summary>
    /// Radius of the circle
    /// </summary>
    public float Radius { get; } = radius;

    /// <summary>
    /// The radius squared, for faster calculations
    /// </summary>
    public float RadiusSquared { get; } = radius * radius;

    /// <summary>
    /// Returns true if the specified point is inside the circle
    /// </summary>
    public bool Contains(Vector2 point)
    {
        return Vector2.DistanceSquared(Center, point) <= RadiusSquared;
    }

    /// <summary>
    /// Returns true if the specified AABB intersects with, or is inside the circle
    /// </summary>
    public bool Intersects(AABB aabb)
    {
        var closest = Vector2.Clamp(Center, aabb.Min, aabb.Max);
        var distance = Vector2.DistanceSquared(Center, closest);
        return distance <= RadiusSquared;
    }

    /// <summary>
    /// Returns true if the specified line intersects with the circle
    /// </summary>
    public bool Intersects(Line line)
    {
        var dir = line.End - line.Start;
        var t = Vector2.Dot(Center - line.Start, dir) / Vector2.Dot(dir, dir);
        t = Math.Clamp(t, 0f, 1f);
        var closest = line.Start + (dir * t);
        var distance = Vector2.DistanceSquared(Center, closest);
        return distance <= RadiusSquared;
    }

    /// <summary>
    /// Returns true if the specified polygon intersects with the circle
    /// </summary>
    public bool Intersects(Polygon2D polygon)
    {
        return polygon.Vertices.Any(Contains) || polygon.Edges.Any(Intersects);
    }

    /// <summary>
    /// Returns true if the specified circle intersects with the circle
    /// </summary>
    public bool Intersects(Circle circle)
    {
        var distance = Vector2.DistanceSquared(Center, circle.Center);
        return distance <= (Radius + circle.Radius) * (Radius + circle.Radius);
    }

    /// <summary>
    /// Returns true if the specified line intersects with the circle
    /// </summary>
    public bool Intersects(Vector2 start, Vector2 end)
    {
        return Intersects(new Line(start, end));
    }
}