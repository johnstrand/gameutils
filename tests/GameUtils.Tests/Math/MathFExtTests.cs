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

    [TestMethod]
    public void Smoothstep_XBelowLowerEdge_ReturnsZero()
    {
        float edge0 = 10f;
        float edge1 = 20f;
        float x = 5f;

        float result = MathFExt.Smoothstep(edge0, edge1, x);

        Assert.AreEqual(0f, result);
    }

    [TestMethod]
    public void Smoothstep_XAboveUpperEdge_ReturnsOne()
    {
        float edge0 = 10f;
        float edge1 = 20f;
        float x = 25f;

        float result = MathFExt.Smoothstep(edge0, edge1, x);

        Assert.AreEqual(1f, result);
    }

    [TestMethod]
    public void Smoothstep_XEqualsLowerEdge_ReturnsZero()
    {
        float edge0 = 10f;
        float edge1 = 20f;
        float x = 10f;

        float result = MathFExt.Smoothstep(edge0, edge1, x);

        Assert.AreEqual(0f, result);
    }

    [TestMethod]
    public void Smoothstep_XEqualsUpperEdge_ReturnsOne()
    {
        float edge0 = 10f;
        float edge1 = 20f;
        float x = 20f;

        float result = MathFExt.Smoothstep(edge0, edge1, x);

        Assert.AreEqual(1f, result);
    }

    [TestMethod]
    public void Smoothstep_XMidwayBetweenEdges_ReturnsHalf()
    {
        float edge0 = 10f;
        float edge1 = 20f;
        float x = 15f;

        float result = MathFExt.Smoothstep(edge0, edge1, x);

        Assert.AreEqual(0.5f, result);
    }

    [TestMethod]
    public void Smoothstep_XQuarterWayBetweenEdges_ReturnsHermiteInterpolatedValue()
    {
        float edge0 = 0f;
        float edge1 = 1f;
        float x = 0.25f;

        float result = MathFExt.Smoothstep(edge0, edge1, x);

        // t = 0.25
        // 0.25 * 0.25 * (3 - 2 * 0.25) = 0.0625 * 2.5 = 0.15625
        Assert.AreEqual(0.15625f, result, Tolerance);
    }

    [TestMethod]
    public void Smoothstep_XThreeQuartersWayBetweenEdges_ReturnsHermiteInterpolatedValue()
    {
        float edge0 = 0f;
        float edge1 = 1f;
        float x = 0.75f;

        float result = MathFExt.Smoothstep(edge0, edge1, x);

        // t = 0.75
        // 0.75 * 0.75 * (3 - 2 * 0.75) = 0.5625 * 1.5 = 0.84375
        Assert.AreEqual(0.84375f, result, Delta);
    }

    [TestMethod]
    public void Remap_ValueInsideSourceRange_ReturnsMappedValue()
    {
        float value = 5f;
        float expected = 50f;
        float actual = MathFExt.Remap(value, 0f, 10f, 0f, 100f);
        Assert.AreEqual(expected, actual, Tolerance);
    }

    [TestMethod]
    public void Remap_ValueBelowSourceRange_ReturnsMappedValueBelowTargetRange()
    {
        float value = -5f;
        float expected = -50f;
        float actual = MathFExt.Remap(value, 0f, 10f, 0f, 100f);
        Assert.AreEqual(expected, actual, Tolerancef);
    }

    [TestMethod]
    public void Remap_ValueAboveSourceRange_ReturnsMappedValueAboveTargetRange()
    {
        float value = 15f;
        float expected = 150f;
        float actual = MathFExt.Remap(value, 0f, 10f, 0f, 100f);
        Assert.AreEqual(expected, actual, Tolerance);
    }

    [TestMethod]
    public void Remap_ReversedTargetRange_ReturnsMappedValue()
    {
        float value = 5f;
        float expected = 50f;
        float actual = MathFExt.Remap(value, 0f, 10f, 100f, 0f);
        Assert.AreEqual(expected, actual, Tolerancef);
    }

    [TestMethod]
    public void RemapClamped_ValueInsideSourceRange_ReturnsMappedValue()
    {
        float value = 5f;
        float expected = 50f;
        float actual = MathFExt.RemapClamped(value, 0f, 10f, 0f, 100f);
        Assert.AreEqual(expected, actual, Tolerance);
    }

    [TestMethod]
    public void RemapClamped_ValueBelowSourceRange_ReturnsClampedValue()
    {
        float value = -5f;
        float expected = 0f;
        float actual = MathFExt.RemapClamped(value, 0f, 10f, 0f, 100f);
        Assert.AreEqual(expected, actual, Tolerance);
    }

    [TestMethod]
    public void RemapClamped_ValueAboveSourceRange_ReturnsClampedValue()
    {
        float value = 15f;
        float expected = 100f;
        float actual = MathFExt.RemapClamped(value, 0f, 10f, 0f, 100f);
        Assert.AreEqual(expected, actual, Tolerance);
    }
}
