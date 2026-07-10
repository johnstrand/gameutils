using System;
using System.Numerics;
using GameUtils.Math;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GameUtils.Tests.Math;

[TestClass]
public class Vector2ExtTests
{
    private const float Tolerance = 0.0001f;

    [TestMethod]
    public void AngleTowards_TargetIsRight_ReturnsZero()
    {
        var source = new Vector2(1f, 1f);
        var target = new Vector2(2f, 1f);
        var expected = 0f;

        var actual = source.AngleTowards(target);

        Assert.AreEqual(expected, actual, Tolerance);
    }

    [TestMethod]
    public void AngleTowards_TargetIsUp_ReturnsPiOverTwo()
    {
        var source = new Vector2(1f, 1f);
        var target = new Vector2(1f, 2f);
        var expected = MathF.PI / 2f;

        var actual = source.AngleTowards(target);

        Assert.AreEqual(expected, actual, Tolerance);
    }

    [TestMethod]
    public void AngleTowards_TargetIsLeft_ReturnsPi()
    {
        var source = new Vector2(1f, 1f);
        var target = new Vector2(0f, 1f);
        var expected = MathF.PI;

        var actual = source.AngleTowards(target);

        Assert.AreEqual(expected, actual, Tolerance);
    }

    [TestMethod]
    public void AngleTowards_TargetIsDown_ReturnsNegativePiOverTwo()
    {
        var source = new Vector2(1f, 1f);
        var target = new Vector2(1f, 0f);
        var expected = -MathF.PI / 2f;

        var actual = source.AngleTowards(target);

        Assert.AreEqual(expected, actual, Tolerance);
    }

    [TestMethod]
    public void AngleTowards_TargetIsUpRight_ReturnsPiOverFour()
    {
        var source = new Vector2(1f, 1f);
        var target = new Vector2(2f, 2f);
        var expected = MathF.PI / 4f;

        var actual = source.AngleTowards(target);

        Assert.AreEqual(expected, actual, Tolerance);
    }

    [TestMethod]
    public void AngleTowards_TargetIsSameAsSource_ReturnsNaN()
    {
        var source = new Vector2(1f, 1f);
        var target = new Vector2(1f, 1f);

        var actual = source.AngleTowards(target);

        Assert.IsTrue(float.IsNaN(actual));
    }
}
