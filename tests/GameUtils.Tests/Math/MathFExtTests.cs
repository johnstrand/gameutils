using Microsoft.VisualStudio.TestTools.UnitTesting;
using GameUtils.Math;
using System;

namespace GameUtils.Tests.Math;

[TestClass]
public class MathFExtTests
{
    private const float Tolerance = 0.0001f;

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
}
