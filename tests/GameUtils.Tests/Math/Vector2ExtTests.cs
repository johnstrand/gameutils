using Microsoft.VisualStudio.TestTools.UnitTesting;
using GameUtils.Math;
using System.Numerics;
using System.Collections.Generic;
using System.Linq;

namespace GameUtils.Tests.Math;

[TestClass]
public class Vector2ExtTests
{
    [TestMethod]
    public void Sort_Clockwise_SortsVectorsCorrectly()
    {
        // Vectors around (0,0) which is their midpoint
        var vectors = new List<Vector2>
        {
            new Vector2(0, 1),
            new Vector2(-1, 0),
            new Vector2(0, -1),
            new Vector2(1, 0)
        };

        // Midpoint is (0,0)
        var result = vectors.Sort(clockwise: true).ToList();

        // Atan2 relative to (0,0):
        // (0, -1) -> -PI/2
        // (1, 0) -> 0
        // (0, 1) -> PI/2
        // (-1, 0) -> PI
        Assert.AreEqual(new Vector2(0, -1), result[0]);
        Assert.AreEqual(new Vector2(1, 0), result[1]);
        Assert.AreEqual(new Vector2(0, 1), result[2]);
        Assert.AreEqual(new Vector2(-1, 0), result[3]);
    }

    [TestMethod]
    public void Sort_CounterClockwise_SortsVectorsCorrectly()
    {
        var vectors = new List<Vector2>
        {
            new Vector2(0, 1),
            new Vector2(-1, 0),
            new Vector2(0, -1),
            new Vector2(1, 0)
        };

        // Midpoint is (0,0)
        var result = vectors.Sort(clockwise: false).ToList();

        // Atan2 * -1:
        // (-1, 0) -> PI * -1 = -PI
        // (0, 1) -> PI/2 * -1 = -PI/2
        // (1, 0) -> 0 * -1 = 0
        // (0, -1) -> -PI/2 * -1 = PI/2
        Assert.AreEqual(new Vector2(-1, 0), result[0]);
        Assert.AreEqual(new Vector2(0, 1), result[1]);
        Assert.AreEqual(new Vector2(1, 0), result[2]);
        Assert.AreEqual(new Vector2(0, -1), result[3]);
    }

    [TestMethod]
    public void Sort_WithSpecifiedCenter_Clockwise_SortsCorrectly()
    {
        var center = new Vector2(10, 10);
        var vectors = new List<Vector2>
        {
            new Vector2(10, 11), // relative: (0, 1)
            new Vector2(9, 10),  // relative: (-1, 0)
            new Vector2(10, 9),  // relative: (0, -1)
            new Vector2(11, 10)  // relative: (1, 0)
        };

        var result = vectors.Sort(center, clockwise: true).ToList();

        Assert.AreEqual(new Vector2(10, 9), result[0]);
        Assert.AreEqual(new Vector2(11, 10), result[1]);
        Assert.AreEqual(new Vector2(10, 11), result[2]);
        Assert.AreEqual(new Vector2(9, 10), result[3]);
    }

    [TestMethod]
    public void Sort_WithSpecifiedCenter_CounterClockwise_SortsCorrectly()
    {
        var center = new Vector2(10, 10);
        var vectors = new List<Vector2>
        {
            new Vector2(10, 11),
            new Vector2(9, 10),
            new Vector2(10, 9),
            new Vector2(11, 10)
        };

        var result = vectors.Sort(center, clockwise: false).ToList();

        Assert.AreEqual(new Vector2(9, 10), result[0]);
        Assert.AreEqual(new Vector2(10, 11), result[1]);
        Assert.AreEqual(new Vector2(11, 10), result[2]);
        Assert.AreEqual(new Vector2(10, 9), result[3]);
    }
}