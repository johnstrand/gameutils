using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace GameUtils.Types.Geometry;

/// <summary>
/// An infinite ray defined by an origin point and a normalised direction.
/// Useful for raycasting and line-of-sight tests.
/// </summary>
public readonly record struct Ray2D
{
    /// <summary>
    /// The origin of the ray.
    /// </summary>
    public Vector2 Origin { get; init; }

    /// <summary>
    /// The normalised direction of the ray.
    /// </summary>
    public Vector2 Direction { get; init; }

    /// <summary>
    /// Creates a ray from <paramref name="origin"/> pointing in <paramref name="direction"/> (normalised automatically).
    /// </summary>
    public Ray2D(Vector2 origin, Vector2 direction)
    {
        Origin = origin;
        Direction = Vector2.Normalize(direction);
    }

    /// <summary>
    /// Returns the point along the ray at parameter <paramref name="t"/> (i.e., Origin + Direction * t).
    /// </summary>
    public Vector2 At(float t)
    {
        return Origin + (Direction * t);
    }

    /// <summary>
    /// Tests intersection with an infinite line segment. Returns true and sets <paramref name="t"/> and
    /// <paramref name="point"/> when the ray hits the segment; <paramref name="t"/> is the distance along the ray.
    /// </summary>
    public bool Intersects(Line line, out float t, [NotNullWhen(true)] out Vector2? point)
    {
        t = 0;
        point = null;

        var d = Direction;
        var v1 = Origin - line.Start;
        var v2 = line.End - line.Start;
        var v3 = new Vector2(-d.Y, d.X);

        var dot = Vector2.Dot(v2, v3);
        if (MathF.Abs(dot) < 1e-6f)
        {
            return false;
        }

        var tVal = Cross(v2, v1) / dot;
        var uVal = Vector2.Dot(v1, v3) / dot;

        if (tVal >= 0 && uVal >= 0 && uVal <= 1)
        {
            t = tVal;
            point = At(t);
            return true;
        }

        return false;
    }

    /// <summary>
    /// Tests intersection with a circle. Returns true and the nearer <paramref name="t"/> and <paramref name="point"/> on hit.
    /// </summary>
    public bool Intersects(Circle circle, out float t, [NotNullWhen(true)] out Vector2? point)
    {
        t = 0;
        point = null;

        var oc = Origin - circle.Center;
        var b = Vector2.Dot(oc, Direction);
        var c = Vector2.Dot(oc, oc) - circle.RadiusSquared;
        var discriminant = (b * b) - c;

        if (discriminant < 0)
        {
            return false;
        }

        var sqrtD = MathF.Sqrt(discriminant);
        var t0 = -b - sqrtD;
        var t1 = -b + sqrtD;

        t = t0 >= 0 ? t0 : t1;
        if (t < 0)
        {
            return false;
        }

        point = At(t);
        return true;
    }

    /// <summary>
    /// Tests intersection with an AABB. Returns true and the entry <paramref name="t"/> and <paramref name="point"/> on hit.
    /// </summary>
    public bool Intersects(AABB aabb, out float t, [NotNullWhen(true)] out Vector2? point)
    {
        t = 0;
        point = null;

        var invDir = new Vector2(
            Direction.X != 0 ? 1f / Direction.X : float.MaxValue,
            Direction.Y != 0 ? 1f / Direction.Y : float.MaxValue);

        var tmin = (aabb.Min - Origin) * invDir;
        var tmax = (aabb.Max - Origin) * invDir;

        var t1 = Vector2.Min(tmin, tmax);
        var t2 = Vector2.Max(tmin, tmax);

        var tNear = MathF.Max(t1.X, t1.Y);
        var tFar  = MathF.Min(t2.X, t2.Y);

        if (tNear > tFar || tFar < 0)
        {
            return false;
        }

        t = tNear >= 0 ? tNear : tFar;
        point = At(t);
        return true;
    }

    private static float Cross(Vector2 a, Vector2 b)
    {
        return (a.X * b.Y) - (a.Y * b.X);
    }
}
