using Microsoft.VisualStudio.TestTools.UnitTesting;
using GameUtils.Types.Geometry;
using System.Numerics;

namespace GameUtils.Tests.Geometry;

[TestClass]
public class AABBTests
{
    [TestMethod]
    public void Intersects_PolygonContainsAABB_ReturnsTrue()
    {
        var aabb = new AABB(new Vector2(10, 10), new Vector2(20, 20));
        var polyContainsAABB = new Polygon2D(new[] {
            new Vector2(0, 0), new Vector2(30, 0), new Vector2(30, 30), new Vector2(0, 30)
        });

        Assert.IsTrue(aabb.Intersects(polyContainsAABB));
    }

    [TestMethod]
    public void Intersects_PolygonInsideAABB_ReturnsTrue()
    {
        var aabb = new AABB(new Vector2(10, 10), new Vector2(20, 20));
        var polyInsideAABB = new Polygon2D(new[] {
            new Vector2(12, 12), new Vector2(18, 12), new Vector2(15, 18)
        });

        Assert.IsTrue(aabb.Intersects(polyInsideAABB));
    }

    [TestMethod]
    public void Intersects_PolygonIntersectingAABB_ReturnsTrue()
    {
        var aabb = new AABB(new Vector2(10, 10), new Vector2(20, 20));
        var polyIntersectingAABB = new Polygon2D(new[] {
            new Vector2(5, 15), new Vector2(15, 15), new Vector2(10, 25)
        });

        Assert.IsTrue(aabb.Intersects(polyIntersectingAABB));
    }

    [TestMethod]
    public void Intersects_PolygonOutsideAABB_ReturnsFalse()
    {
        var aabb = new AABB(new Vector2(10, 10), new Vector2(20, 20));
        var polyOutsideAABB = new Polygon2D(new[] {
            new Vector2(30, 30), new Vector2(40, 30), new Vector2(35, 40)
        });

        Assert.IsFalse(aabb.Intersects(polyOutsideAABB));
    }
}
