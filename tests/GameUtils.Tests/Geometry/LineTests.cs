using Microsoft.VisualStudio.TestTools.UnitTesting;
using GameUtils.Types.Geometry;
using System.Numerics;

namespace GameUtils.Tests.Geometry;

[TestClass]
public class LineTests
{
    [TestMethod]
    public void Cast_WithoutMaxLength_ReturnsLineToTarget()
    {
        var start = new Vector2(0, 0);
        var target = new Vector2(3, 4);

        var line = Line.Cast(start, target);

        Assert.AreEqual(start, line.Start);
        Assert.AreEqual(target, line.End);
        Assert.AreEqual(5f, line.Length);
    }

    [TestMethod]
    public void Cast_WithMaxLengthGreaterThanDistance_ReturnsLineToTarget()
    {
        var start = new Vector2(1, 1);
        var target = new Vector2(1, 5); // Distance is 4

        var line = Line.Cast(start, target, 10f); // Max length 10

        Assert.AreEqual(start, line.Start);
        Assert.AreEqual(target, line.End);
        Assert.AreEqual(4f, line.Length);
    }

    [TestMethod]
    public void Cast_WithMaxLengthLessThanDistance_ReturnsClampedLine()
    {
        var start = new Vector2(0, 0);
        var target = new Vector2(10, 0); // Distance 10, direction X

        var line = Line.Cast(start, target, 5f); // Max length 5

        Assert.AreEqual(start, line.Start);
        Assert.AreEqual(new Vector2(5, 0), line.End);
        Assert.AreEqual(5f, line.Length);
    }

    [TestMethod]
    public void Cast_WithZeroDistance_ReturnsZeroLengthLine()
    {
        var start = new Vector2(2, 2);
        var target = new Vector2(2, 2);

        var line = Line.Cast(start, target, 5f);

        Assert.AreEqual(start, line.Start);
        Assert.AreEqual(target, line.End);
        Assert.AreEqual(0f, line.Length);
    }

    [TestMethod]
    public void Intersects_IntersectingLines_ReturnsTrue()
    {
        var line1 = new Line(new Vector2(0, 0), new Vector2(10, 10));
        var line2 = new Line(new Vector2(0, 10), new Vector2(10, 0));
        Assert.IsTrue(line1.Intersects(line2));
        Assert.IsTrue(line1.Intersects(line2, out var pt));
        Assert.AreEqual(new Vector2(5, 5), pt);
    }

    [TestMethod]
    public void Intersects_ParallelNonIntersectingLines_ReturnsFalse()
    {
        var line1 = new Line(new Vector2(0, 0), new Vector2(10, 0));
        var line2 = new Line(new Vector2(0, 10), new Vector2(10, 10));
        Assert.IsFalse(line1.Intersects(line2));
    }

    [TestMethod]
    public void Intersects_CollinearOverlappingLines_ReturnsFalse()
    {
        var line1 = new Line(new Vector2(0, 0), new Vector2(10, 10));
        var line2 = new Line(new Vector2(5, 5), new Vector2(15, 15));
        Assert.IsFalse(line1.Intersects(line2));
    }

    [TestMethod]
    public void Intersects_TIntersection_ReturnsTrue()
    {
        var line1 = new Line(new Vector2(5, 0), new Vector2(5, 10));
        var line2 = new Line(new Vector2(0, 5), new Vector2(5, 5));
        Assert.IsTrue(line1.Intersects(line2));
        Assert.IsTrue(line1.Intersects(line2, out var pt));
        Assert.AreEqual(new Vector2(5, 5), pt);
    }

    [TestMethod]
    public void Intersects_NonIntersectingSkewLines_ReturnsFalse()
    {
        var line1 = new Line(new Vector2(0, 0), new Vector2(5, 5));
        var line2 = new Line(new Vector2(6, 6), new Vector2(10, 0));
        Assert.IsFalse(line1.Intersects(line2));
    }

    [TestMethod]
    public void IntersectsAny_EmptyCollection_ReturnsFalse()
    {
        var line = new Line(new Vector2(0, 0), new Vector2(10, 0));
        var result = line.IntersectsAny(Array.Empty<Line>(), out var nearest);
        Assert.IsFalse(result);
        Assert.IsNull(nearest);
    }

    [TestMethod]
    public void IntersectsAny_NoIntersections_ReturnsFalse()
    {
        var line = new Line(new Vector2(0, 0), new Vector2(10, 0));
        var rays = new[] { new Line(new Vector2(0, 5), new Vector2(10, 5)), new Line(new Vector2(0, -5), new Vector2(10, -5)) };
        var result = line.IntersectsAny(rays, out var nearest);
        Assert.IsFalse(result);
        Assert.IsNull(nearest);
    }

    [TestMethod]
    public void IntersectsAny_SingleIntersection_ReturnsTrueAndPoint()
    {
        var line = new Line(new Vector2(0, 0), new Vector2(10, 0));
        var rays = new[] { new Line(new Vector2(5, -5), new Vector2(5, 5)) };
        var result = line.IntersectsAny(rays, out var nearest);
        Assert.IsTrue(result);
        Assert.AreEqual(new Vector2(5, 0), nearest);
    }

    [TestMethod]
    public void IntersectsAny_MultipleIntersections_ReturnsNearestPoint()
    {
        var line = new Line(new Vector2(0, 0), new Vector2(10, 0));
        var rayFar = new Line(new Vector2(8, -5), new Vector2(8, 5));
        var rayNear = new Line(new Vector2(2, -5), new Vector2(2, 5));
        var rays = new[] { rayFar, rayNear };
        var result = line.IntersectsAny(rays, out var nearest);
        Assert.IsTrue(result);
        Assert.AreEqual(new Vector2(2, 0), nearest);
    }

    [TestMethod]
    public void IntersectsAny_IntersectionAtStart_ReturnsTrueWithZeroDistance()
    {
        var line = new Line(new Vector2(0, 0), new Vector2(10, 0));
        var rays = new[] { new Line(new Vector2(0, -5), new Vector2(0, 5)) };
        var result = line.IntersectsAny(rays, out var nearest);
        Assert.IsTrue(result);
        Assert.AreEqual(new Vector2(0, 0), nearest);
    }
}
