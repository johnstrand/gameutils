using System.Numerics;

namespace GameUtils.Math;

/// <summary>
/// Bezier curve evaluation and composite path utilities.
/// </summary>
public static class Bezier
{
    /// <summary>
    /// Evaluates a quadratic Bézier curve at parameter <paramref name="t"/> (0–1).
    /// </summary>
    /// <param name="p0">Start point.</param>
    /// <param name="p1">Control point.</param>
    /// <param name="p2">End point.</param>
    /// <param name="t">Interpolation parameter [0, 1].</param>
    public static Vector2 Quadratic(Vector2 p0, Vector2 p1, Vector2 p2, float t)
    {
        var u = 1f - t;
        return (u * u * p0) + (2f * u * t * p1) + (t * t * p2);
    }

    /// <summary>
    /// Evaluates a cubic Bézier curve at parameter <paramref name="t"/> (0–1).
    /// </summary>
    /// <param name="p0">Start point.</param>
    /// <param name="p1">First control point.</param>
    /// <param name="p2">Second control point.</param>
    /// <param name="p3">End point.</param>
    /// <param name="t">Interpolation parameter [0, 1].</param>
    public static Vector2 Cubic(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3, float t)
    {
        var u = 1f - t;
        var u2 = u * u;
        var t2 = t * t;
        return (u2 * u * p0) + (3f * u2 * t * p1) + (3f * u * t2 * p2) + (t2 * t * p3);
    }

    /// <summary>
    /// Returns the tangent (first derivative) of a cubic Bézier curve at <paramref name="t"/>.
    /// </summary>
    public static Vector2 CubicTangent(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3, float t)
    {
        var u = 1f - t;
        return (3f * u * u * (p1 - p0)) + (6f * u * t * (p2 - p1)) + (3f * t * t * (p3 - p2));
    }
}

/// <summary>
/// A composite Bézier path made up of one or more cubic segments.
/// The path is parameterised uniformly across all segments.
/// </summary>
public class BezierPath
{
    private readonly record struct Segment(Vector2 P0, Vector2 P1, Vector2 P2, Vector2 P3);

    private readonly List<Segment> _segments = [];

    /// <summary>The number of segments in the path.</summary>
    public int SegmentCount => _segments.Count;

    /// <summary>
    /// Adds a cubic Bézier segment to the path.
    /// </summary>
    public BezierPath AddSegment(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3)
    {
        _segments.Add(new Segment(p0, p1, p2, p3));
        return this;
    }

    /// <summary>
    /// Evaluates the path at normalised parameter <paramref name="t"/> across all segments.
    /// </summary>
    /// <param name="t">Overall path parameter [0, 1]. Values outside this range are clamped.</param>
    public Vector2 GetPoint(float t)
    {
        if (_segments.Count == 0)
        {
            return Vector2.Zero;
        }

        t = System.Math.Clamp(t, 0f, 1f);

        var scaledT = t * _segments.Count;
        var segIndex = (int)System.Math.Min(MathF.Floor(scaledT), _segments.Count - 1);
        var localT = scaledT - segIndex;

        var seg = _segments[segIndex];
        return Bezier.Cubic(seg.P0, seg.P1, seg.P2, seg.P3, localT);
    }

    /// <summary>
    /// Returns the tangent direction at normalised parameter <paramref name="t"/>.
    /// </summary>
    public Vector2 GetTangent(float t)
    {
        if (_segments.Count == 0)
        {
            return Vector2.Zero;
        }

        t = System.Math.Clamp(t, 0f, 1f);

        var scaledT = t * _segments.Count;
        var segIndex = (int)System.Math.Min(MathF.Floor(scaledT), _segments.Count - 1);
        var localT = scaledT - segIndex;

        var seg = _segments[segIndex];
        var tangent = Bezier.CubicTangent(seg.P0, seg.P1, seg.P2, seg.P3, localT);
        return tangent == Vector2.Zero ? Vector2.Zero : Vector2.Normalize(tangent);
    }
}
