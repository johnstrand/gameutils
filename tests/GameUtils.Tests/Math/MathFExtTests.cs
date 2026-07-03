using Microsoft.VisualStudio.TestTools.UnitTesting;
using GameUtils.Math;

namespace GameUtils.Tests.Math;

[TestClass]
public class MathFExtTests
{
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