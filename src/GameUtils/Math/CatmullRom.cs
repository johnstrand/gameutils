using System.Numerics;

namespace GameUtils.Math;

/// <summary>
/// Catmull-Rom spline helpers. Unlike Bézier curves, Catmull-Rom splines pass directly through
/// every control point, making them ideal for smooth camera paths, patrol routes, and cutscenes.
/// </summary>
public static class CatmullRom
{
    /// <summary>
    /// Evaluates a single Catmull-Rom segment at position <paramref name="t"/>.
    /// </summary>
    /// <param name="p0">The control point before the segment start.</param>
    /// <param name="p1">The segment start point.</param>
    /// <param name="p2">The segment end point.</param>
    /// <param name="p3">The control point after the segment end.</param>
    /// <param name="t">Interpolation factor in [0, 1], where 0 returns <paramref name="p1"/> and 1 returns <paramref name="p2"/>.</param>
    public static Vector2 Sample(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3, float t)
    {
        var t2 = t * t;
        var t3 = t2 * t;
        return 0.5f * (
            (2f * p1) +
            (-p0 + p2) * t +
            (2f * p0 - 5f * p1 + 4f * p2 - p3) * t2 +
            (-p0 + 3f * p1 - 3f * p2 + p3) * t3
        );
    }

    /// <summary>
    /// Returns the tangent (first derivative) of a Catmull-Rom segment at position <paramref name="t"/>.
    /// </summary>
    public static Vector2 Tangent(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3, float t)
    {
        var t2 = t * t;
        return 0.5f * (
            (-p0 + p2) +
            (4f * p0 - 10f * p1 + 8f * p2 - 2f * p3) * t +
            (-3f * p0 + 9f * p1 - 9f * p2 + 3f * p3) * t2
        );
    }
}

/// <summary>
/// A smooth path that passes through all added control points using Catmull-Rom interpolation.
/// At least two points must be added before calling <see cref="GetPoint"/> or <see cref="GetTangent"/>.
/// </summary>
public class CatmullRomPath
{
    private readonly List<Vector2> _points = [];
    private bool _loop;

    /// <summary>The number of control points currently in the path.</summary>
    public int PointCount => _points.Count;

    /// <summary>
    /// Sets whether the path loops back to the start.
    /// When <see langword="true"/>, the last point connects smoothly to the first.
    /// </summary>
    public CatmullRomPath SetLooping(bool loop)
    {
        _loop = loop;
        return this;
    }

    /// <summary>Adds a control point to the end of the path.</summary>
    public CatmullRomPath AddPoint(Vector2 point)
    {
        _points.Add(point);
        return this;
    }

    /// <summary>Adds multiple control points to the end of the path.</summary>
    public CatmullRomPath AddPoints(IEnumerable<Vector2> points)
    {
        _points.AddRange(points);
        return this;
    }

    /// <summary>
    /// Samples the path at a normalized position <paramref name="t"/> in [0, 1].
    /// <para>0 returns the first control point; 1 returns the last (or wraps when looping).</para>
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when fewer than 2 points have been added.</exception>
    public Vector2 GetPoint(float t)
    {
        GetSegmentAndLocalT(t, out var i, out var lt);
        GetNeighbors(i, out var p0, out var p1, out var p2, out var p3);
        return CatmullRom.Sample(p0, p1, p2, p3, lt);
    }

    /// <summary>
    /// Returns the tangent direction at normalized position <paramref name="t"/> in [0, 1].
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when fewer than 2 points have been added.</exception>
    public Vector2 GetTangent(float t)
    {
        GetSegmentAndLocalT(t, out var i, out var lt);
        GetNeighbors(i, out var p0, out var p1, out var p2, out var p3);
        return CatmullRom.Tangent(p0, p1, p2, p3, lt);
    }

    private void GetSegmentAndLocalT(float t, out int segmentIndex, out float localT)
    {
        if (_points.Count < 2)
        {
            throw new InvalidOperationException("CatmullRomPath requires at least 2 control points.");
        }

        var segmentCount = _loop ? _points.Count : _points.Count - 1;
        var scaled = t * segmentCount;
        segmentIndex = (int)MathF.Floor(scaled);
        localT = scaled - segmentIndex;

        if (_loop)
        {
            segmentIndex = segmentIndex % segmentCount;
        }
        else
        {
            segmentIndex = System.Math.Clamp(segmentIndex, 0, segmentCount - 1);
        }
    }

    private void GetNeighbors(int i, out Vector2 p0, out Vector2 p1, out Vector2 p2, out Vector2 p3)
    {
        var n = _points.Count;
        if (_loop)
        {
            p0 = _points[((i - 1) % n + n) % n];
            p1 = _points[i % n];
            p2 = _points[(i + 1) % n];
            p3 = _points[(i + 2) % n];
        }
        else
        {
            p1 = _points[i];
            p2 = _points[i + 1];
            p0 = i > 0 ? _points[i - 1] : p1 + (p1 - p2); // virtual ghost point
            p3 = i + 2 < n ? _points[i + 2] : p2 + (p2 - p1); // virtual ghost point
        }
    }
}
