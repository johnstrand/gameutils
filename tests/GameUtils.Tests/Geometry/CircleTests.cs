using Microsoft.VisualStudio.TestTools.UnitTesting;
using GameUtils.Types.Geometry;
using System.Numerics;

namespace GameUtils.Tests.Geometry;

[TestClass]
public class CircleTests
{
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
}
