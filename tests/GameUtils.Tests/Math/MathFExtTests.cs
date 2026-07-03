using Microsoft.VisualStudio.TestTools.UnitTesting;
using GameUtils.Math;
using System;

namespace GameUtils.Tests.Math;

[TestClass]
public class MathFExtTests
{
    private const float Tolerance = 0.0001f;

    [TestMethod]
    public void Wrap_ValueWithinRange_ReturnsValue()
    {
        float value = 5f;
        float min = 0f;
        float max = 10f;
        float expected = 5f;

        float actual = MathFExt.Wrap(value, min, max);

        Assert.AreEqual(expected, actual, float.Epsilon);
    }

    [TestMethod]
    public void Wrap_ValueAboveMax_WrapsCorrectly()
    {
        float value = 12f;
        float min = 0f;
        float max = 10f;
        float expected = 2f;

        float actual = MathFExt.Wrap(value, min, max);

        Assert.AreEqual(expected, actual, float.Epsilon);
    }

    [TestMethod]
    public void Wrap_ValueBelowMin_WrapsCorrectly()
    {
        float value = -3f;
        float min = 0f;
        float max = 10f;
        float expected = 7f;

        float actual = MathFExt.Wrap(value, min, max);

        Assert.AreEqual(expected, actual, float.Epsilon);
    }

    [TestMethod]
    public void Wrap_MinEqualsMax_ReturnsMin()
    {
        float value = 5f;
        float min = 10f;
        float max = 10f;
        float expected = 10f;

        float actual = MathFExt.Wrap(value, min, max);

        Assert.AreEqual(expected, actual, float.Epsilon);
    }

    [TestMethod]
    public void Wrap_ValueFarAboveMax_WrapsCorrectly()
    {
        float value = 25f;
        float min = 0f;
        float max = 10f;
        float expected = 5f;

        float actual = MathFExt.Wrap(value, min, max);

        Assert.AreEqual(expected, actual, float.Epsilon);
    }

    [TestMethod]
    public void Wrap_ValueFarBelowMin_WrapsCorrectly()
    {
        float value = -15f;
        float min = 0f;
        float max = 10f;
        float expected = 5f;

        float actual = MathFExt.Wrap(value, min, max);

        Assert.AreEqual(expected, actual, float.Epsilon);
    }

    [TestMethod]
    public void PingPong_LengthIsZero_ReturnsZero()
    {
        Assert.AreEqual(0f, MathFExt.PingPong(5f, 0f), Tolerance);
    }

    [TestMethod]
    public void PingPong_LengthIsNegative_ReturnsZero()
    {
        Assert.AreEqual(0f, MathFExt.PingPong(5f, -2f), Tolerance);
    }

    [TestMethod]
    public void PingPong_ValueWithinLength_ReturnsValue()
    {
        Assert.AreEqual(2.5f, MathFExt.PingPong(2.5f, 5f), Tolerance);
        Assert.AreEqual(0f, MathFExt.PingPong(0f, 5f), Tolerance);
        Assert.AreEqual(5f, MathFExt.PingPong(5f, 5f), Tolerance);
    }

    [TestMethod]
    public void PingPong_ValueGreaterThanLength_ReturnsBouncedValue()
    {
        Assert.AreEqual(4f, MathFExt.PingPong(6f, 5f), Tolerance);
        Assert.AreEqual(0f, MathFExt.PingPong(10f, 5f), Tolerance);
        Assert.AreEqual(2f, MathFExt.PingPong(12f, 5f), Tolerance);
    }

    [TestMethod]
    public void PingPong_ValueIsNegative_ReturnsBouncedValue()
    {
        Assert.AreEqual(1f, MathFExt.PingPong(-1f, 5f), Tolerance);
        Assert.AreEqual(4f, MathFExt.PingPong(-6f, 5f), Tolerance);
        Assert.AreEqual(0f, MathFExt.PingPong(-10f, 5f), Tolerance);
    }

    [TestMethod]
    public void ToRadians_ZeroDegrees_ReturnsZero()
    {
        float degrees = 0f;
        float expected = 0f;

        float result = MathFExt.ToRadians(degrees);

        Assert.AreEqual(expected, result, Tolerance);
    }

    [TestMethod]
    public void ToRadians_PositiveDegrees_ReturnsCorrectRadians()
    {
        Assert.AreEqual(MathF.PI / 2f, MathFExt.ToRadians(90f), Tolerance);
        Assert.AreEqual(MathF.PI, MathFExt.ToRadians(180f), Tolerance);
        Assert.AreEqual(MathF.PI * 1.5f, MathFExt.ToRadians(270f), Tolerance);
        Assert.AreEqual(MathF.Tau, MathFExt.ToRadians(360f), Tolerance);
    }

    [TestMethod]
    public void ToRadians_NegativeDegrees_ReturnsCorrectRadians()
    {
        Assert.AreEqual(-MathF.PI / 2f, MathFExt.ToRadians(-90f), Tolerance);
        Assert.AreEqual(-MathF.PI, MathFExt.ToRadians(-180f), Tolerance);
        Assert.AreEqual(-MathF.Tau, MathFExt.ToRadians(-360f), Tolerance);
    }

    [TestMethod]
    public void ToRadians_LargeDegrees_ReturnsCorrectRadians()
    {
        Assert.AreEqual(MathF.Tau * 2f, MathFExt.ToRadians(720f), Tolerance);
    }
}
