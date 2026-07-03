using GameUtils.Math;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GameUtils.Tests.Math;

[TestClass]
public class MathFExtTests
{
    private const float Delta = 0.0001f;
    private const float Tolerance = 0.0001f;

    [TestMethod]
    public void AngleDifference_SameAngles_ReturnsZero()
    {
        Assert.AreEqual(0f, MathFExt.AngleDifference(0f, 0f), Delta);
        Assert.AreEqual(0f, MathFExt.AngleDifference(MathF.PI, MathF.PI), Delta);
        Assert.AreEqual(0f, MathFExt.AngleDifference(-MathF.PI, -MathF.PI), Delta);
        Assert.AreEqual(0f, MathFExt.AngleDifference(MathF.Tau, MathF.Tau), Delta);
    }

    [TestMethod]
    public void AngleDifference_OppositeAngles_ReturnsPI()
    {
        // When difference is exactly PI or -PI, we expect -PI since it wraps to [-PI, PI)
        Assert.AreEqual(-MathF.PI, MathFExt.AngleDifference(0f, MathF.PI), Delta);
        Assert.AreEqual(-MathF.PI, MathFExt.AngleDifference(0f, -MathF.PI), Delta);
        Assert.AreEqual(-MathF.PI, MathFExt.AngleDifference(-MathF.PI / 2f, MathF.PI / 2f), Delta);
        Assert.AreEqual(-MathF.PI, MathFExt.AngleDifference(MathF.PI / 2f, -MathF.PI / 2f), Delta);
    }

    [TestMethod]
    public void AngleDifference_AcuteAngles_ReturnsDifference()
    {
        Assert.AreEqual(MathF.PI / 2f, MathFExt.AngleDifference(0f, MathF.PI / 2f), Delta);
        Assert.AreEqual(-MathF.PI / 2f, MathFExt.AngleDifference(MathF.PI / 2f, 0f), Delta);
        Assert.AreEqual(MathF.PI / 4f, MathFExt.AngleDifference(MathF.PI / 4f, MathF.PI / 2f), Delta);
        Assert.AreEqual(-MathF.PI / 4f, MathFExt.AngleDifference(MathF.PI / 2f, MathF.PI / 4f), Delta);
    }

    [TestMethod]
    public void AngleDifference_AnglesAcrossWrapBoundary_ReturnsShortestDifference()
    {
        // from PI-0.1 to -PI+0.1 (which is PI+0.1 going forward)
        // difference should be 0.2
        Assert.AreEqual(0.2f, MathFExt.AngleDifference(MathF.PI - 0.1f, -MathF.PI + 0.1f), Delta);

        // from -PI+0.1 to PI-0.1
        // difference should be -0.2
        Assert.AreEqual(-0.2f, MathFExt.AngleDifference(-MathF.PI + 0.1f, MathF.PI - 0.1f), Delta);
    }

    [TestMethod]
    public void AngleDifference_LargeAngles_ReturnsWrappedDifference()
    {
        Assert.AreEqual(MathF.PI / 2f, MathFExt.AngleDifference(0f, MathF.Tau + (MathF.PI / 2f)), Delta);
        Assert.AreEqual(-MathF.PI / 2f, MathFExt.AngleDifference(MathF.Tau + (MathF.PI / 2f), 0f), Delta);

        Assert.AreEqual(-MathF.PI / 2f, MathFExt.AngleDifference(0f, -MathF.Tau - (MathF.PI / 2f)), Delta);
        Assert.AreEqual(MathF.PI / 2f, MathFExt.AngleDifference(-MathF.Tau - (MathF.PI / 2f), 0f), Delta);

        Assert.AreEqual(0f, MathFExt.AngleDifference(MathF.Tau, MathF.Tau + MathF.Tau), Delta);
    }

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
}
