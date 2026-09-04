using System;
using System.Numerics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GameUtils.Types.Geometry;

namespace GameUtils.Tests.Geometry;

[TestClass]
public class Ray2DTests
{
    [TestMethod]
    public void Constructor_ZeroDirection_ThrowsArgumentException()
    {
        Assert.ThrowsExactly<ArgumentException>(() => new Ray2D(new Vector2(0, 0), Vector2.Zero));
    }

    [TestMethod]
    public void Constructor_ValidDirection_NormalizesDirectionAndSetsOrigin()
    {
        var origin = new Vector2(1, 2);
        var ray = new Ray2D(origin, new Vector2(3, 4));
        Assert.AreEqual(origin, ray.Origin);
        Assert.AreEqual(new Vector2(0.6f, 0.8f), ray.Direction);
    }

    [TestMethod]
    public void At_ValidT_ReturnsExpectedPoint()
    {
        var ray = new Ray2D(new Vector2(1, 1), new Vector2(1, 0));
        Assert.AreEqual(new Vector2(6, 1), ray.At(5f));
    }

    [TestMethod]
    public void Intersects_Line_HittingSegment_ReturnsTrueAndIntersectionPoint()
    {
        var ray = new Ray2D(new Vector2(0, 5), new Vector2(1, 0));
        var line = new Line(new Vector2(5, 0), new Vector2(5, 10));
        bool result = ray.Intersects(line, out float t, out Vector2? point);
        Assert.IsTrue(result);
        Assert.AreEqual(5f, t, 1e-4f);
        Assert.IsNotNull(point);
        Assert.AreEqual(new Vector2(5, 5), point.Value);
    }

    [TestMethod]
    public void Intersects_Line_ParallelLine_ReturnsFalse()
    {
        var ray = new Ray2D(new Vector2(0, 0), new Vector2(1, 0));
        var line = new Line(new Vector2(0, 5), new Vector2(10, 5));
        bool result = ray.Intersects(line, out float t, out Vector2? point);
        Assert.IsFalse(result);
        Assert.AreEqual(0f, t);
        Assert.IsNull(point);
    }

    [TestMethod]
    public void Intersects_Line_RayPointingAway_ReturnsFalse()
    {
        var ray = new Ray2D(new Vector2(10, 5), new Vector2(1, 0));
        var line = new Line(new Vector2(5, 0), new Vector2(5, 10));
        bool result = ray.Intersects(line, out float t, out Vector2? point);
        Assert.IsFalse(result);
        Assert.IsNull(point);
    }

    [TestMethod]
    public void Intersects_Line_SegmentMissed_ReturnsFalse()
    {
        var ray = new Ray2D(new Vector2(0, 15), new Vector2(1, 0));
        var line = new Line(new Vector2(5, 0), new Vector2(5, 10));
        bool result = ray.Intersects(line, out float t, out Vector2? point);
        Assert.IsFalse(result);
        Assert.IsNull(point);
    }

    [TestMethod]
    public void Intersects_Circle_HittingCircleFromOutside_ReturnsTrueAndNearIntersectionPoint()
    {
        var ray = new Ray2D(new Vector2(0, 0), new Vector2(1, 0));
        var circle = new Circle(new Vector2(10, 0), 2f);
        bool result = ray.Intersects(circle, out float t, out Vector2? point);
        Assert.IsTrue(result);
        Assert.AreEqual(8f, t, 1e-4f);
        Assert.IsNotNull(point);
        Assert.AreEqual(new Vector2(8, 0), point.Value);
    }

    [TestMethod]
    public void Intersects_Circle_OriginInsideCircle_ReturnsTrueAndExitIntersectionPoint()
    {
        var ray = new Ray2D(new Vector2(10, 0), new Vector2(1, 0));
        var circle = new Circle(new Vector2(10, 0), 2f);
        bool result = ray.Intersects(circle, out float t, out Vector2? point);
        Assert.IsTrue(result);
        Assert.AreEqual(2f, t, 1e-4f);
        Assert.IsNotNull(point);
        Assert.AreEqual(new Vector2(12, 0), point.Value);
    }

    [TestMethod]
    public void Intersects_Circle_MissingCircle_ReturnsFalse()
    {
        var ray = new Ray2D(new Vector2(0, 10), new Vector2(1, 0));
        var circle = new Circle(new Vector2(10, 0), 2f);
        bool result = ray.Intersects(circle, out float t, out Vector2? point);
        Assert.IsFalse(result);
        Assert.IsNull(point);
    }

    [TestMethod]
    public void Intersects_Circle_PointingAwayFromCircle_ReturnsFalse()
    {
        var ray = new Ray2D(new Vector2(0, 0), new Vector2(-1, 0));
        var circle = new Circle(new Vector2(10, 0), 2f);
        bool result = ray.Intersects(circle, out float t, out Vector2? point);
        Assert.IsFalse(result);
        Assert.IsNull(point);
    }

    [TestMethod]
    public void Intersects_AABB_HittingAABBFromOutside_ReturnsTrueAndEntryIntersectionPoint()
    {
        var ray = new Ray2D(new Vector2(0, 5), new Vector2(1, 0));
        var aabb = new AABB(new Vector2(5, 0), new Vector2(10, 10));
        bool result = ray.Intersects(aabb, out float t, out Vector2? point);
        Assert.IsTrue(result);
        Assert.AreEqual(5f, t, 1e-4f);
        Assert.IsNotNull(point);
        Assert.AreEqual(new Vector2(5, 5), point.Value);
    }

    [TestMethod]
    public void Intersects_AABB_OriginInsideAABB_ReturnsTrueAndExitIntersectionPoint()
    {
        var ray = new Ray2D(new Vector2(7, 5), new Vector2(1, 0));
        var aabb = new AABB(new Vector2(5, 0), new Vector2(10, 10));
        bool result = ray.Intersects(aabb, out float t, out Vector2? point);
        Assert.IsTrue(result);
        Assert.AreEqual(3f, t, 1e-4f);
        Assert.IsNotNull(point);
        Assert.AreEqual(new Vector2(10, 5), point.Value);
    }

    [TestMethod]
    public void Intersects_AABB_MissingAABB_ReturnsFalse()
    {
        var ray = new Ray2D(new Vector2(0, 20), new Vector2(1, 0));
        var aabb = new AABB(new Vector2(5, 0), new Vector2(10, 10));
        bool result = ray.Intersects(aabb, out float t, out Vector2? point);
        Assert.IsFalse(result);
        Assert.IsNull(point);
    }

    [TestMethod]
    public void Intersects_AABB_PointingAwayFromAABB_ReturnsFalse()
    {
        var ray = new Ray2D(new Vector2(0, 5), new Vector2(-1, 0));
        var aabb = new AABB(new Vector2(5, 0), new Vector2(10, 10));
        bool result = ray.Intersects(aabb, out float t, out Vector2? point);
        Assert.IsFalse(result);
        Assert.IsNull(point);
    }

    [TestMethod]
    public void Intersects_AABB_AxisAlignedRay_ReturnsExpectedResult()
    {
        var ray = new Ray2D(new Vector2(5, -5), new Vector2(0, 1));
        var aabb = new AABB(new Vector2(0, 0), new Vector2(10, 10));
        bool result = ray.Intersects(aabb, out float t, out Vector2? point);
        Assert.IsTrue(result);
        Assert.AreEqual(5f, t, 1e-4f);
        Assert.IsNotNull(point);
        Assert.AreEqual(new Vector2(5, 0), point.Value);
    }
}
