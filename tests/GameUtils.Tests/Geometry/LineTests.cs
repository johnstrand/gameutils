using Microsoft.VisualStudio.TestTools.UnitTesting;
using GameUtils.Types.Geometry;
using System.Numerics;

namespace GameUtils.Tests.Geometry;

[TestClass]
public class LineTests
{
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
}
