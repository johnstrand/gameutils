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
    public void AngleBetween_PerpendicularVectors_ReturnsHalfPi()
    {
        Vector2 a = Vector2.UnitX;
        Vector2 b = Vector2.UnitY;
        Assert.AreEqual(MathF.PI / 2f, a.AngleBetween(b), Tolerance);
    }

    [TestMethod]
    public void AngleBetween_SameVector_ReturnsZero()
    {
        Vector2 a = new Vector2(1, 1);
        Assert.AreEqual(0f, a.AngleBetween(a), Tolerance);
    }

    [TestMethod]
    public void AngleBetween_OppositeVectors_ReturnsPi()
    {
        Vector2 a = Vector2.UnitX;
        Vector2 b = -Vector2.UnitX;
        Assert.AreEqual(MathF.PI, MathF.Abs(a.AngleBetween(b)), Tolerance);
    }

    [TestMethod]
    public void AngleBetween_Clockwise_ReturnsNegative()
    {
        Vector2 a = Vector2.UnitY;
        Vector2 b = Vector2.UnitX;
        Assert.AreEqual(-MathF.PI / 2f, a.AngleBetween(b), Tolerance);
    }

    [TestMethod]
    public void AngleBetween_ZeroVector_ReturnsNaN()
    {
        Vector2 a = Vector2.Zero;
        Vector2 b = Vector2.UnitX;
        Assert.IsTrue(float.IsNaN(a.AngleBetween(b)));
        Assert.IsTrue(float.IsNaN(b.AngleBetween(a)));
    }

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
