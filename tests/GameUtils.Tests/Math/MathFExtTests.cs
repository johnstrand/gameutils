using Microsoft.VisualStudio.TestTools.UnitTesting;
using GameUtils.Math;

namespace GameUtils.Tests.Math;

[TestClass]
public class MathFExtTests
{
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
}
