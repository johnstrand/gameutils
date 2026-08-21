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
}
