using Microsoft.VisualStudio.TestTools.UnitTesting;
using GameUtils.Types.Geometry;
using System;
using System.Numerics;

namespace GameUtils.Tests.Geometry;

[TestClass]
public class LineTests
{
    [TestMethod]
    public void Constructor_StartEnd_InitializesProperties()
    {
        var start = new Vector2(0, 0);
        var end = new Vector2(3, 4);
        var line = new Line(start, end);

        Assert.AreEqual(start, line.Start);
        Assert.AreEqual(end, line.End);
        Assert.AreEqual(5f, line.Length);
        Assert.AreEqual(new Vector2(1.5f, 2f), line.Midpoint);
        Assert.AreEqual(new Vector2(-4, 3), line.NormalA);
        Assert.AreEqual(new Vector2(4, -3), line.NormalB);
    }

    [TestMethod]
    public void Constructor_StartDirectionLength_InitializesProperties()
    {
        var start = new Vector2(1, 1);
        var dir = new Vector2(0, 2);
        var line = new Line(start, dir, 5f);

        Assert.AreEqual(start, line.Start);
        Assert.AreEqual(new Vector2(1, 6), line.End);
        Assert.AreEqual(5f, line.Length, 1e-5f);
        Assert.AreEqual(new Vector2(1, 3.5f), line.Midpoint);
    }

    [TestMethod]
    public void Constructor_ZeroDirection_ThrowsArgumentException()
    {
        Assert.ThrowsExactly<ArgumentException>(() => new Line(Vector2.Zero, Vector2.Zero, 5f));
    }

    [TestMethod]
    public void Constructor_StartAngleLength_InitializesProperties()
    {
        var start = new Vector2(0, 0);
        var angle = 0f;
        var line = new Line(start, angle, 10f);

        Assert.AreEqual(start, line.Start);
        Assert.AreEqual(10f, line.End.X, 1e-5f);
        Assert.AreEqual(0f, line.End.Y, 1e-5f);
        Assert.AreEqual(10f, line.Length, 1e-5f);
    }

    [TestMethod]
    public void Constructor_Floats_InitializesProperties()
    {
        var line = new Line(1f, 2f, 4f, 6f);
        Assert.AreEqual(new Vector2(1f, 2f), line.Start);
        Assert.AreEqual(new Vector2(4f, 6f), line.End);
        Assert.AreEqual(5f, line.Length);
    }

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
        var target = new Vector2(1, 5);

        var line = Line.Cast(start, target, 10f);

        Assert.AreEqual(start, line.Start);
        Assert.AreEqual(target, line.End);
        Assert.AreEqual(4f, line.Length);
    }

    [TestMethod]
    public void Cast_WithMaxLengthLessThanDistance_ReturnsClampedLine()
    {
        var start = new Vector2(0, 0);
        var target = new Vector2(10, 0);

        var line = Line.Cast(start, target, 5f);

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
    public void Intersects_NegativeDenominator_ReturnsTrueAndIntersectionPoint()
    {
        var line1 = new Line(new Vector2(10, 0), new Vector2(0, 0));
        var line2 = new Line(new Vector2(5, -5), new Vector2(5, 5));
        Assert.IsTrue(line1.Intersects(line2));
        Assert.IsTrue(line1.Intersects(line2, out var pt));
        Assert.AreEqual(new Vector2(5, 0), pt);
    }

    [TestMethod]
    public void Intersects_NegativeDenominator_OutOfBounds_ReturnsFalse()
    {
        var line1 = new Line(new Vector2(10, 0), new Vector2(0, 0));
        var line2 = new Line(new Vector2(15, -5), new Vector2(15, 5));
        Assert.IsFalse(line1.Intersects(line2));
        Assert.IsFalse(line1.Intersects(line2, out var pt));
        Assert.IsNull(pt);
    }

    [TestMethod]
    public void Intersects_ParallelNonIntersectingLines_ReturnsFalse()
    {
        var line1 = new Line(new Vector2(0, 0), new Vector2(10, 0));
        var line2 = new Line(new Vector2(0, 10), new Vector2(10, 10));
        Assert.IsFalse(line1.Intersects(line2));
        Assert.IsFalse(line1.Intersects(line2, out var pt));
        Assert.IsNull(pt);
    }

    [TestMethod]
    public void Intersects_CollinearOverlappingLines_ReturnsFalse()
    {
        var line1 = new Line(new Vector2(0, 0), new Vector2(10, 10));
        var line2 = new Line(new Vector2(5, 5), new Vector2(15, 15));
        Assert.IsFalse(line1.Intersects(line2));
        Assert.IsFalse(line1.Intersects(line2, out var pt));
        Assert.IsNull(pt);
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
        Assert.IsFalse(line1.Intersects(line2, out var pt));
        Assert.IsNull(pt);
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
