using Microsoft.VisualStudio.TestTools.UnitTesting;
using GameUtils.Types.Geometry;
using System.Numerics;

namespace GameUtils.Tests.Geometry;

[TestClass]
public class CircleTests
{
    [TestMethod]
    public void Constructor_NegativeRadius_ThrowsArgumentOutOfRangeException()
    {
        Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => new Circle(new Vector2(0, 0), -1f));
    }

    [TestMethod]
    public void Constructor_ValidParameters_PropertiesSetCorrectly()
    {
        var center = new Vector2(3, 4);
        var circle = new Circle(center, 5f);
        Assert.AreEqual(center, circle.Center);
        Assert.AreEqual(5f, circle.Radius);
        Assert.AreEqual(25f, circle.RadiusSquared);
    }

    [TestMethod]
    public void Contains_PointInsideCircle_ReturnsTrue()
    {
        var circle = new Circle(new Vector2(0, 0), 5);
        var point = new Vector2(2, 2);

        Assert.IsTrue(circle.Contains(point));
    }

    [TestMethod]
    public void Contains_PointOnEdge_ReturnsTrue()
    {
        var circle = new Circle(new Vector2(0, 0), 5);
        var point = new Vector2(5, 0);

        Assert.IsTrue(circle.Contains(point));
    }

    [TestMethod]
    public void Contains_PointOutsideCircle_ReturnsFalse()
    {
        var circle = new Circle(new Vector2(0, 0), 5);
        var point = new Vector2(6, 6);

        Assert.IsFalse(circle.Contains(point));
    }

    [TestMethod]
    public void Intersects_AABBInsideCircle_ReturnsTrue()
    {
        var circle = new Circle(new Vector2(15, 15), 10);
        var aabb = new AABB(new Vector2(10, 10), new Vector2(20, 20));
        Assert.IsTrue(circle.Intersects(aabb));
    }

    [TestMethod]
    public void Intersects_CircleInsideAABB_ReturnsTrue()
    {
        var circle = new Circle(new Vector2(15, 15), 5);
        var aabb = new AABB(new Vector2(0, 0), new Vector2(30, 30));
        Assert.IsTrue(circle.Intersects(aabb));
    }

    [TestMethod]
    public void Intersects_CircleIntersectingAABB_ReturnsTrue()
    {
        var circle = new Circle(new Vector2(5, 5), 5);
        var aabb = new AABB(new Vector2(8, 8), new Vector2(20, 20));
        Assert.IsTrue(circle.Intersects(aabb));
    }

    [TestMethod]
    public void Intersects_CircleOutsideAABB_ReturnsFalse()
    {
        var circle = new Circle(new Vector2(-10, -10), 5);
        var aabb = new AABB(new Vector2(10, 10), new Vector2(20, 20));
        Assert.IsFalse(circle.Intersects(aabb));
    }

    [TestMethod]
    public void Intersects_CircleTouchingAABB_ReturnsTrue()
    {
        var circle = new Circle(new Vector2(5, 15), 5);
        var aabb = new AABB(new Vector2(10, 10), new Vector2(20, 20));
        Assert.IsTrue(circle.Intersects(aabb));
    }

    [TestMethod]
    public void Intersects_LinePassingThrough_ReturnsTrue()
    {
        var circle = new Circle(new Vector2(0, 0), 5);
        var line = new Line(new Vector2(-10, 0), new Vector2(10, 0));
        Assert.IsTrue(circle.Intersects(line));
    }

    [TestMethod]
    public void Intersects_LineInsideCircle_ReturnsTrue()
    {
        var circle = new Circle(new Vector2(0, 0), 5);
        var line = new Line(new Vector2(-1, 0), new Vector2(1, 0));
        Assert.IsTrue(circle.Intersects(line));
    }

    [TestMethod]
    public void Intersects_LineTangentToCircle_ReturnsTrue()
    {
        var circle = new Circle(new Vector2(0, 0), 5);
        var line = new Line(new Vector2(-10, 5), new Vector2(10, 5));
        Assert.IsTrue(circle.Intersects(line));
    }

    [TestMethod]
    public void Intersects_LineOutsideCircle_ReturnsFalse()
    {
        var circle = new Circle(new Vector2(0, 0), 5);
        var line = new Line(new Vector2(-10, 10), new Vector2(10, 10));
        Assert.IsFalse(circle.Intersects(line));
    }

    [TestMethod]
    public void Intersects_LinePointingAtCircleButShort_ReturnsFalse()
    {
        var circle = new Circle(new Vector2(0, 0), 5);
        var line = new Line(new Vector2(10, 0), new Vector2(6, 0));
        Assert.IsFalse(circle.Intersects(line));
    }

    [TestMethod]
    public void Intersects_LineStartEndOverload_ReturnsTrue()
    {
        var circle = new Circle(new Vector2(0, 0), 5);
        Assert.IsTrue(circle.Intersects(new Vector2(-10, 0), new Vector2(10, 0)));
    }

    [TestMethod]
    public void Contains_NonOriginCenter_ReturnsCorrectResult()
    {
        var circle = new Circle(new Vector2(10, 10), 5f);
        Assert.IsTrue(circle.Contains(new Vector2(12, 12)));
        Assert.IsFalse(circle.Contains(new Vector2(20, 20)));
    }

    [TestMethod]
    public void Contains_ZeroRadiusCircle_OnlyContainsCenter()
    {
        var circle = new Circle(new Vector2(5, 5), 0f);
        Assert.IsTrue(circle.Contains(new Vector2(5, 5)));
        Assert.IsFalse(circle.Contains(new Vector2(5.1f, 5)));
    }

    [TestMethod]
    public void Intersects_Circle_OverlappingTouchingDisjoint_ReturnsExpectedResult()
    {
        var c1 = new Circle(new Vector2(0, 0), 5f);
        var c2 = new Circle(new Vector2(6, 0), 5f);
        var c3 = new Circle(new Vector2(10, 0), 5f);
        var c4 = new Circle(new Vector2(12, 0), 5f);
        Assert.IsTrue(c1.Intersects(c2));
        Assert.IsTrue(c1.Intersects(c3));
        Assert.IsFalse(c1.Intersects(c4));
    }

    [TestMethod]
    public void Intersects_Polygon2D_ReturnsExpectedResult()
    {
        var circle = new Circle(new Vector2(0, 0), 5f);
        var polyInside = new Polygon2D(new[] { new Vector2(-1, -1), new Vector2(1, -1), new Vector2(0, 1) });
        var polyIntersecting = new Polygon2D(new[] { new Vector2(4, 0), new Vector2(8, 0), new Vector2(6, 4) });
        var polyOutside = new Polygon2D(new[] { new Vector2(10, 10), new Vector2(12, 10), new Vector2(11, 12) });
        Assert.IsTrue(circle.Intersects(polyInside));
        Assert.IsTrue(circle.Intersects(polyIntersecting));
        Assert.IsFalse(circle.Intersects(polyOutside));
    }
}
