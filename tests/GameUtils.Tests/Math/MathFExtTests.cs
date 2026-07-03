using Microsoft.VisualStudio.TestTools.UnitTesting;
using GameUtils.Math;
using System;

namespace GameUtils.Tests.Math;

[TestClass]
public class MathFExtTests
{
    private const float Delta = 0.0001f;

    [TestMethod]
    public void ToRadians_ZeroDegrees_ReturnsZero()
    {
        float degrees = 0f;
        float expected = 0f;

        float result = MathFExt.ToRadians(degrees);

        Assert.AreEqual(expected, result, Delta);
    }

    [TestMethod]
    public void ToRadians_PositiveDegrees_ReturnsCorrectRadians()
    {
        Assert.AreEqual(MathF.PI / 2f, MathFExt.ToRadians(90f), Delta);
        Assert.AreEqual(MathF.PI, MathFExt.ToRadians(180f), Delta);
        Assert.AreEqual(MathF.PI * 1.5f, MathFExt.ToRadians(270f), Delta);
        Assert.AreEqual(MathF.Tau, MathFExt.ToRadians(360f), Delta);
    }

    [TestMethod]
    public void ToRadians_NegativeDegrees_ReturnsCorrectRadians()
    {
        Assert.AreEqual(-MathF.PI / 2f, MathFExt.ToRadians(-90f), Delta);
        Assert.AreEqual(-MathF.PI, MathFExt.ToRadians(-180f), Delta);
        Assert.AreEqual(-MathF.Tau, MathFExt.ToRadians(-360f), Delta);
    }

    [TestMethod]
    public void ToRadians_LargeDegrees_ReturnsCorrectRadians()
    {
        Assert.AreEqual(MathF.Tau * 2f, MathFExt.ToRadians(720f), Delta);
    }
}
