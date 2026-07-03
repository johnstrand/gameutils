using Microsoft.VisualStudio.TestTools.UnitTesting;
using GameUtils.Math;

namespace GameUtils.Tests.Math;

[TestClass]
public class MathFExtTests
{
    private const float Delta = 0.0001f;

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
        Assert.AreEqual(0.15625f, result, Delta);
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
        Assert.AreEqual(expected, actual, 0.0001f);
    }

    [TestMethod]
    public void Remap_ValueBelowSourceRange_ReturnsMappedValueBelowTargetRange()
    {
        float value = -5f;
        float expected = -50f;
        float actual = MathFExt.Remap(value, 0f, 10f, 0f, 100f);
        Assert.AreEqual(expected, actual, 0.0001f);
    }

    [TestMethod]
    public void Remap_ValueAboveSourceRange_ReturnsMappedValueAboveTargetRange()
    {
        float value = 15f;
        float expected = 150f;
        float actual = MathFExt.Remap(value, 0f, 10f, 0f, 100f);
        Assert.AreEqual(expected, actual, 0.0001f);
    }

    [TestMethod]
    public void Remap_ReversedTargetRange_ReturnsMappedValue()
    {
        float value = 5f;
        float expected = 50f;
        float actual = MathFExt.Remap(value, 0f, 10f, 100f, 0f);
        Assert.AreEqual(expected, actual, 0.0001f);
    }

    [TestMethod]
    public void RemapClamped_ValueInsideSourceRange_ReturnsMappedValue()
    {
        float value = 5f;
        float expected = 50f;
        float actual = MathFExt.RemapClamped(value, 0f, 10f, 0f, 100f);
        Assert.AreEqual(expected, actual, 0.0001f);
    }

    [TestMethod]
    public void RemapClamped_ValueBelowSourceRange_ReturnsClampedValue()
    {
        float value = -5f;
        float expected = 0f;
        float actual = MathFExt.RemapClamped(value, 0f, 10f, 0f, 100f);
        Assert.AreEqual(expected, actual, 0.0001f);
    }

    [TestMethod]
    public void RemapClamped_ValueAboveSourceRange_ReturnsClampedValue()
    {
        float value = 15f;
        float expected = 100f;
        float actual = MathFExt.RemapClamped(value, 0f, 10f, 0f, 100f);
        Assert.AreEqual(expected, actual, 0.0001f);
    }
}
