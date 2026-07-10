using Microsoft.VisualStudio.TestTools.UnitTesting;
using GameUtils.Math;
using System.Numerics;
using System;

namespace GameUtils.Tests.Math;

[TestClass]
public class Vector2ExtTests
{
    private const float Tolerance = 0.0001f;

    [TestMethod]
    public void GetDirection_DifferentVectors_ReturnsNormalizedDirection()
    {
        var source = new Vector2(1, 1);
        var target = new Vector2(4, 5);

        // Target - Source = (3, 4)
        // Length = 5
        // Normalized = (3/5, 4/5) = (0.6, 0.8)
        var expected = new Vector2(0.6f, 0.8f);
        var result = source.GetDirection(target);

        Assert.AreEqual(expected.X, result.X, Tolerance);
        Assert.AreEqual(expected.Y, result.Y, Tolerance);
    }

    [TestMethod]
    public void GetDirection_SameVectors_ReturnsNaNVector()
    {
        var source = new Vector2(1, 1);
        var target = new Vector2(1, 1);

        var result = source.GetDirection(target);

        Assert.IsTrue(float.IsNaN(result.X));
        Assert.IsTrue(float.IsNaN(result.Y));
    }
}
