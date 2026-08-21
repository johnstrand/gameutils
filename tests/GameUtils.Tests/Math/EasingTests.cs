using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GameUtils.Math;

namespace GameUtils.Tests.Math;

[TestClass]
public class EasingTests
{
    private const float Tolerance = 0.0001f;

    [TestMethod]
    [DataRow(0f, 0f)]
    [DataRow(0.5f, 0.7071f)]
    [DataRow(1f, 1f)]
    public void SineOut_ReturnsExpectedValue(float t, float expected)
    {
        float result = Easing.SineOut(t);
        Assert.AreEqual(expected, result, Tolerance);
    }
}