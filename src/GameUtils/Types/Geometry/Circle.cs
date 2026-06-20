using System.Numerics;

namespace GameUtils.Types.Geometry;

/// <summary>
/// A circle
/// </summary>
/// <remarks>
/// Constructs a new circle
/// </remarks>
/// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="radius"/> is negative.</exception>
public class Circle(Vector2 center, float radius)
{
    /// <summary>
    /// Center of the circle
    /// </summary>
    public Vector2 Center { get; } = center;

    /// <summary>
    /// Radius of the circle
    /// </summary>
    public float Radius { get; } = radius >= 0 ? radius : throw new ArgumentOutOfRangeException(nameof(radius), "Radius must be non-negative.");

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
        t = System.Math.Clamp(t, 0f, 1f);
        var closest = line.Start + (dir * t);
        var distance = Vector2.DistanceSquared(Center, closest);
        return distance <= RadiusSquared;
    }

    /// <summary>
    /// Returns true if the specified polygon intersects with the circle
    /// </summary>
#pragma warning disable S3267 // LINQ would reintroduce allocations on a hot collision path
    public bool Intersects(Polygon2D polygon)
    {
        if (!Intersects(polygon.BoundingBox)) return false;

        foreach (var v in polygon.Vertices)
        {
            if (Contains(v)) return true;
        }

        foreach (var e in polygon.Edges)
        {
            if (Intersects(e)) return true;
        }

        return false;
    }
#pragma warning restore S3267

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