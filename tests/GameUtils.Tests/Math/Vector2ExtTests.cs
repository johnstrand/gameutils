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
}
