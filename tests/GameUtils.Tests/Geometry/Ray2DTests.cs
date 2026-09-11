using System;
using System.Numerics;
using GameUtils.Types.Geometry;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GameUtils.Tests.Geometry;

[TestClass]
public class Ray2DTests
{
    [TestMethod]
    public void Constructor_ValidDirection_NormalizesDirection()
    {
        var origin = new Vector2(1, 2);
        var dir = new Vector2(3, 4);
        var ray = new Ray2D(origin, dir);

        Assert.AreEqual(origin, ray.Origin);
        Assert.AreEqual(Vector2.Normalize(dir), ray.Direction);
    }

    [TestMethod]
    public void Constructor_ZeroDirection_ThrowsArgumentException()
    {
        Assert.ThrowsExactly<ArgumentException>(() => new Ray2D(new Vector2(0, 0), Vector2.Zero));
    }

    [TestMethod]
    public void At_ReturnsCorrectPointAlongRay()
    {
        var ray = new Ray2D(new Vector2(1, 1), new Vector2(1, 0));

        Assert.AreEqual(new Vector2(1, 1), ray.At(0f));
        Assert.AreEqual(new Vector2(6, 1), ray.At(5f));
        Assert.AreEqual(new Vector2(-2, 1), ray.At(-3f));
    }

    [TestMethod]
    public void Intersects_Line_HittingSegment_ReturnsTrueAndPoint()
    {
        var ray = new Ray2D(new Vector2(0, 0), new Vector2(1, 0));
        var line = new Line(new Vector2(5, -5), new Vector2(5, 5));

        bool hit = ray.Intersects(line, out float t, out Vector2? point);

        Assert.IsTrue(hit);
        Assert.AreEqual(5f, t, 1e-5f);
        Assert.IsNotNull(point);
        Assert.AreEqual(new Vector2(5, 0), point.Value);
    }

    [TestMethod]
    public void Intersects_Line_ParallelLine_ReturnsFalse()
    {
        var ray = new Ray2D(new Vector2(0, 0), new Vector2(1, 0));
        var line = new Line(new Vector2(0, 5), new Vector2(10, 5));

        bool hit = ray.Intersects(line, out _, out Vector2? point);

        Assert.IsFalse(hit);
        Assert.IsNull(point);
    }

    [TestMethod]
    public void Intersects_Line_OutsideSegmentBounds_ReturnsFalse()
    {
        var ray = new Ray2D(new Vector2(0, 0), new Vector2(1, 0));
        var line = new Line(new Vector2(5, 2), new Vector2(5, 10));

        bool hit = ray.Intersects(line, out _, out Vector2? point);

        Assert.IsFalse(hit);
        Assert.IsNull(point);
    }

    [TestMethod]
    public void Intersects_Line_BehindRay_ReturnsFalse()
    {
        var ray = new Ray2D(new Vector2(0, 0), new Vector2(1, 0));
        var line = new Line(new Vector2(-5, -5), new Vector2(-5, 5));

        bool hit = ray.Intersects(line, out _, out Vector2? point);

        Assert.IsFalse(hit);
        Assert.IsNull(point);
    }

    [TestMethod]
    public void Intersects_Circle_HittingCircle_ReturnsTrueAndNearestPoint()
    {
        var ray = new Ray2D(new Vector2(0, 0), new Vector2(1, 0));
        var circle = new Circle(new Vector2(10, 0), 3f);

        bool hit = ray.Intersects(circle, out float t, out Vector2? point);

        Assert.IsTrue(hit);
        Assert.AreEqual(7f, t, 1e-5f);
        Assert.IsNotNull(point);
        Assert.AreEqual(new Vector2(7, 0), point.Value);
    }

    [TestMethod]
    public void Intersects_Circle_OriginInsideCircle_ReturnsTrueAndExitPoint()
    {
        var ray = new Ray2D(new Vector2(10, 0), new Vector2(1, 0));
        var circle = new Circle(new Vector2(10, 0), 5f);

        bool hit = ray.Intersects(circle, out float t, out Vector2? point);

        Assert.IsTrue(hit);
        Assert.AreEqual(5f, t, 1e-5f);
        Assert.IsNotNull(point);
        Assert.AreEqual(new Vector2(15, 0), point.Value);
    }

    [TestMethod]
    public void Intersects_Circle_MissingCircle_ReturnsFalse()
    {
        var ray = new Ray2D(new Vector2(0, 0), new Vector2(1, 0));
        var circle = new Circle(new Vector2(5, 10), 2f);

        bool hit = ray.Intersects(circle, out _, out Vector2? point);

        Assert.IsFalse(hit);
        Assert.IsNull(point);
    }

    [TestMethod]
    public void Intersects_Circle_PointingAway_ReturnsFalse()
    {
        var ray = new Ray2D(new Vector2(0, 0), new Vector2(-1, 0));
        var circle = new Circle(new Vector2(10, 0), 3f);

        bool hit = ray.Intersects(circle, out _, out Vector2? point);

        Assert.IsFalse(hit);
        Assert.IsNull(point);
    }

    [TestMethod]
    public void Intersects_AABB_HittingBox_ReturnsTrueAndEntryPoint()
    {
        var ray = new Ray2D(new Vector2(-5, 5), new Vector2(1, 0));
        var aabb = new AABB(new Vector2(0, 0), new Vector2(10, 10));

        bool hit = ray.Intersects(aabb, out float t, out Vector2? point);

        Assert.IsTrue(hit);
        Assert.AreEqual(5f, t, 1e-5f);
        Assert.IsNotNull(point);
        Assert.AreEqual(new Vector2(0, 5), point.Value);
    }

    [TestMethod]
    public void Intersects_AABB_OriginInsideBox_ReturnsTrueAndExitPoint()
    {
        var ray = new Ray2D(new Vector2(5, 5), new Vector2(1, 0));
        var aabb = new AABB(new Vector2(0, 0), new Vector2(10, 10));

        bool hit = ray.Intersects(aabb, out float t, out Vector2? point);

        Assert.IsTrue(hit);
        Assert.AreEqual(5f, t, 1e-5f);
        Assert.IsNotNull(point);
        Assert.AreEqual(new Vector2(10, 5), point.Value);
    }

    [TestMethod]
    public void Intersects_AABB_MissingBox_ReturnsFalse()
    {
        var ray = new Ray2D(new Vector2(-5, 20), new Vector2(1, 0));
        var aabb = new AABB(new Vector2(0, 0), new Vector2(10, 10));

        bool hit = ray.Intersects(aabb, out _, out Vector2? point);

        Assert.IsFalse(hit);
        Assert.IsNull(point);
    }

    [TestMethod]
    public void Intersects_AABB_AxisAlignedRay_HittingBox_ReturnsTrue()
    {
        var ray = new Ray2D(new Vector2(5, -5), new Vector2(0, 1));
        var aabb = new AABB(new Vector2(0, 0), new Vector2(10, 10));

        bool hit = ray.Intersects(aabb, out float t, out Vector2? point);

        Assert.IsTrue(hit);
        Assert.AreEqual(5f, t, 1e-5f);
        Assert.IsNotNull(point);
        Assert.AreEqual(new Vector2(5, 0), point.Value);
    }
}
