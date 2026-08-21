using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GameUtils.Math;

namespace GameUtils.Tests.Math;

[TestClass]
public class EasingTests
{
    private const float Tolerance = 0.0001f;

    [TestMethod]
    public void CubicIn_Zero_ReturnsZero()
    {
        Assert.AreEqual(0f, Easing.CubicIn(0f), Tolerance);
    }

    [TestMethod]
    public void CubicIn_Half_ReturnsEighth()
    {
        Assert.AreEqual(0.125f, Easing.CubicIn(0.5f), Tolerance);
    }

    [TestMethod]
    public void CubicIn_One_ReturnsOne()
    {
        Assert.AreEqual(1f, Easing.CubicIn(1f), Tolerance);
    }

    [TestMethod]
    public void QuadIn_Zero_ReturnsZero()
    {
        Assert.AreEqual(0f, Easing.QuadIn(0f), Tolerance);
    }

    [TestMethod]
    public void QuadIn_Half_ReturnsQuarter()
    {
        Assert.AreEqual(0.25f, Easing.QuadIn(0.5f), Tolerance);
    }

    [TestMethod]
    public void QuadIn_One_ReturnsOne()
    {
        Assert.AreEqual(1f, Easing.QuadIn(1f), Tolerance);
    }

    [TestMethod]
    public void QuadIn_Negative_ReturnsSquare()
    {
        Assert.AreEqual(1f, Easing.QuadIn(-1f), Tolerance);
    }

    [TestMethod]
    public void QuadIn_Two_ReturnsFour()
    {
        Assert.AreEqual(4f, Easing.QuadIn(2f), Tolerance);
    }

    [TestMethod]
    [DataRow(0f, 0f)]
    [DataRow(0.5f, 0.7071f)]
    [DataRow(1f, 1f)]
    public void SineOut_ReturnsExpectedValue(float t, float expected)
    {
        float result = Easing.SineOut(t);
        Assert.AreEqual(expected, result, Tolerance);
    }

    [TestMethod]
    public void SineIn_Zero_ReturnsZero()
    {
        Assert.AreEqual(0f, Easing.SineIn(0f), Tolerance);
    }

    [TestMethod]
    public void SineIn_Half_ReturnsExpectedValue()
    {
        Assert.AreEqual(0.29289323f, Easing.SineIn(0.5f), Tolerance);
    }

    [TestMethod]
    public void SineIn_One_ReturnsOne()
    {
        Assert.AreEqual(1f, Easing.SineIn(1f), Tolerance);
    }
}
