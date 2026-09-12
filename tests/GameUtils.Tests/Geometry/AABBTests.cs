using Microsoft.VisualStudio.TestTools.UnitTesting;
using GameUtils.Types.Geometry;
using System.Numerics;
using System;

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

    [TestMethod]
    public void Intersects_Circle_ReturnsExpectedResult()
    {
        var aabb = new AABB(new Vector2(10, 10), new Vector2(20, 20));
        var intersectingCircle = new Circle(new Vector2(25, 15), 6f);
        var nonIntersectingCircle = new Circle(new Vector2(30, 30), 5f);

        Assert.IsTrue(aabb.Intersects(intersectingCircle));
        Assert.IsFalse(aabb.Intersects(nonIntersectingCircle));
    }

    [TestMethod]
    public void Intersects_Vector2CenterAndRadius_ReturnsExpectedResult()
    {
        var aabb = new AABB(new Vector2(10, 10), new Vector2(20, 20));

        Assert.IsTrue(aabb.Intersects(new Vector2(25, 15), 6f));
        Assert.IsFalse(aabb.Intersects(new Vector2(30, 30), 5f));
        Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => aabb.Intersects(new Vector2(15, 15), -1f));
    }

    [TestMethod]
    public void Intersects_Vector2StartAndEnd_ReturnsExpectedResult()
    {
        var aabb = new AABB(new Vector2(10, 10), new Vector2(20, 20));

        Assert.IsTrue(aabb.Intersects(new Vector2(5, 15), new Vector2(25, 15)));
        Assert.IsFalse(aabb.Intersects(new Vector2(0, 0), new Vector2(5, 5)));
    }

    [TestMethod]
    public void Contains_PointInsideBox_ReturnsTrue()
    {
        var aabb = new AABB(new Vector2(10, 10), new Vector2(20, 20));
        Assert.IsTrue(aabb.Contains(new Vector2(15, 15)));
    }

    [TestMethod]
    public void Contains_PointOnBoundary_ReturnsTrue()
    {
        var aabb = new AABB(new Vector2(10, 10), new Vector2(20, 20));
        Assert.IsTrue(aabb.Contains(new Vector2(10, 10)));
        Assert.IsTrue(aabb.Contains(new Vector2(20, 20)));
        Assert.IsTrue(aabb.Contains(new Vector2(10, 15)));
        Assert.IsTrue(aabb.Contains(new Vector2(20, 15)));
        Assert.IsTrue(aabb.Contains(new Vector2(15, 10)));
        Assert.IsTrue(aabb.Contains(new Vector2(15, 20)));
    }

    [TestMethod]
    public void Contains_PointOutsideBox_ReturnsFalse()
    {
        var aabb = new AABB(new Vector2(10, 10), new Vector2(20, 20));
        Assert.IsFalse(aabb.Contains(new Vector2(9, 15)));
        Assert.IsFalse(aabb.Contains(new Vector2(21, 15)));
        Assert.IsFalse(aabb.Contains(new Vector2(15, 9)));
        Assert.IsFalse(aabb.Contains(new Vector2(15, 21)));
        Assert.IsFalse(aabb.Contains(new Vector2(9, 9)));
        Assert.IsFalse(aabb.Contains(new Vector2(21, 21)));
    }

    [TestMethod]
    public void Contains_AABB_FullyContained_ReturnsTrue()
    {
        var outer = new AABB(new Vector2(0, 0), new Vector2(20, 20));
        var inner = new AABB(new Vector2(5, 5), new Vector2(15, 15));
        Assert.IsTrue(outer.Contains(inner));
    }

    [TestMethod]
    public void Contains_AABB_Identical_ReturnsTrue()
    {
        var box1 = new AABB(new Vector2(10, 10), new Vector2(20, 20));
        var box2 = new AABB(new Vector2(10, 10), new Vector2(20, 20));
        Assert.IsTrue(box1.Contains(box2));
    }

    [TestMethod]
    public void Contains_AABB_TouchingInternalBoundary_ReturnsTrue()
    {
        var outer = new AABB(new Vector2(0, 0), new Vector2(20, 20));
        var inner = new AABB(new Vector2(0, 0), new Vector2(10, 10));
        Assert.IsTrue(outer.Contains(inner));
    }

    [TestMethod]
    public void Contains_AABB_PartiallyOverlapping_ReturnsFalse()
    {
        var box1 = new AABB(new Vector2(0, 0), new Vector2(10, 10));
        var box2 = new AABB(new Vector2(5, 5), new Vector2(15, 15));
        Assert.IsFalse(box1.Contains(box2));
        Assert.IsFalse(box2.Contains(box1));
    }

    [TestMethod]
    public void Contains_AABB_Disjoint_ReturnsFalse()
    {
        var box1 = new AABB(new Vector2(0, 0), new Vector2(10, 10));
        var box2 = new AABB(new Vector2(20, 20), new Vector2(30, 30));
        Assert.IsFalse(box1.Contains(box2));
        Assert.IsFalse(box2.Contains(box1));
    }

    [TestMethod]
    public void Contains_AABB_EnclosingBox_ReturnsFalse()
    {
        var inner = new AABB(new Vector2(5, 5), new Vector2(15, 15));
        var outer = new AABB(new Vector2(0, 0), new Vector2(20, 20));
        Assert.IsFalse(inner.Contains(outer));
    }

    [TestMethod]
    public void Contains_AABB_ExceedsInSingleDimension_ReturnsFalse()
    {
        var baseBox = new AABB(new Vector2(10, 10), new Vector2(20, 20));

        Assert.IsFalse(baseBox.Contains(new AABB(new Vector2(9, 11), new Vector2(19, 19))));
        Assert.IsFalse(baseBox.Contains(new AABB(new Vector2(11, 11), new Vector2(21, 19))));
        Assert.IsFalse(baseBox.Contains(new AABB(new Vector2(11, 9), new Vector2(19, 19))));
        Assert.IsFalse(baseBox.Contains(new AABB(new Vector2(11, 11), new Vector2(19, 21))));
    }

    [TestMethod]
    public void Intersects_AABBOutPoints_NoIntersection_ReturnsFalseAndEmptyArray()
    {
        var box1 = new AABB(new Vector2(0, 0), new Vector2(10, 10));
        var box2 = new AABB(new Vector2(20, 20), new Vector2(30, 30));

        bool result = box1.Intersects(box2, out Vector2[] points);

        Assert.IsFalse(result);
        Assert.AreEqual(0, points.Length);
    }

    [TestMethod]
    public void Intersects_AABBOutPoints_Overlapping_ReturnsTrueAndIntersectionPoints()
    {
        var box1 = new AABB(new Vector2(0, 0), new Vector2(10, 10));
        var box2 = new AABB(new Vector2(5, 5), new Vector2(15, 15));

        bool result = box1.Intersects(box2, out Vector2[] points);

        Assert.IsTrue(result);
        Assert.AreEqual(2, points.Length);
        CollectionAssert.Contains(points, new Vector2(5, 0));
        CollectionAssert.Contains(points, new Vector2(0, 5));
    }

    [TestMethod]
    public void Intersects_AABBOutPoints_Box1ContainsBox2_ReturnsTrueAndFourPoints()
    {
        var box1 = new AABB(new Vector2(0, 0), new Vector2(20, 20));
        var box2 = new AABB(new Vector2(5, 5), new Vector2(15, 15));

        bool result = box1.Intersects(box2, out Vector2[] points);

        Assert.IsTrue(result);
        Assert.AreEqual(4, points.Length);
        CollectionAssert.Contains(points, new Vector2(5, 0));
        CollectionAssert.Contains(points, new Vector2(15, 0));
        CollectionAssert.Contains(points, new Vector2(0, 5));
        CollectionAssert.Contains(points, new Vector2(0, 15));
    }

    [TestMethod]
    public void Intersects_AABB_Overlapping_ReturnsTrue()
    {
        var box1 = new AABB(new Vector2(0, 0), new Vector2(10, 10));
        var box2 = new AABB(new Vector2(5, 5), new Vector2(15, 15));
        Assert.IsTrue(box1.Intersects(box2));
        Assert.IsTrue(box2.Intersects(box1));
    }

    [TestMethod]
    public void Intersects_AABB_Contained_ReturnsTrue()
    {
        var box1 = new AABB(new Vector2(0, 0), new Vector2(20, 20));
        var box2 = new AABB(new Vector2(5, 5), new Vector2(15, 15));
        Assert.IsTrue(box1.Intersects(box2));
        Assert.IsTrue(box2.Intersects(box1));
    }

    [TestMethod]
    public void Intersects_AABB_Identical_ReturnsTrue()
    {
        var box1 = new AABB(new Vector2(0, 0), new Vector2(10, 10));
        var box2 = new AABB(new Vector2(0, 0), new Vector2(10, 10));
        Assert.IsTrue(box1.Intersects(box2));
    }

    [TestMethod]
    public void Intersects_AABB_TouchingEdges_ReturnsTrue()
    {
        var box1 = new AABB(new Vector2(0, 0), new Vector2(10, 10));
        var box2 = new AABB(new Vector2(10, 0), new Vector2(20, 10));
        Assert.IsTrue(box1.Intersects(box2));
    }

    [TestMethod]
    public void Intersects_AABB_Disjoint_ReturnsFalse()
    {
        var box1 = new AABB(new Vector2(0, 0), new Vector2(10, 10));
        var box2 = new AABB(new Vector2(20, 20), new Vector2(30, 30));
        Assert.IsFalse(box1.Intersects(box2));
        Assert.IsFalse(box2.Intersects(box1));
    }

    [TestMethod]
    public void Constructor_NormalMinMax_SetsPropertiesCorrectly()
    {
        var min = new Vector2(10, 15);
        var max = new Vector2(20, 25);
        var aabb = new AABB(min, max);

        Assert.AreEqual(min, aabb.Min);
        Assert.AreEqual(max, aabb.Max);
        Assert.AreEqual(new Vector2(15, 20), aabb.Center);
        Assert.AreEqual(new Vector2(10, 10), aabb.Size);
    }

    [TestMethod]
    public void Constructor_ReversedMinMax_SwapsMinAndMaxCorrectly()
    {
        var min = new Vector2(20, 25);
        var max = new Vector2(10, 15);
        var aabb = new AABB(min, max);

        Assert.AreEqual(new Vector2(10, 15), aabb.Min);
        Assert.AreEqual(new Vector2(20, 25), aabb.Max);
        Assert.AreEqual(new Vector2(15, 20), aabb.Center);
        Assert.AreEqual(new Vector2(10, 10), aabb.Size);
    }

    [TestMethod]
    public void Constructor_MixedMinMax_SwapsAxesIndividuallyCorrectly()
    {
        var point1 = new Vector2(20, 15);
        var point2 = new Vector2(10, 25);
        var aabb = new AABB(point1, point2);

        Assert.AreEqual(new Vector2(10, 15), aabb.Min);
        Assert.AreEqual(new Vector2(20, 25), aabb.Max);
        Assert.AreEqual(new Vector2(15, 20), aabb.Center);
        Assert.AreEqual(new Vector2(10, 10), aabb.Size);
    }
}