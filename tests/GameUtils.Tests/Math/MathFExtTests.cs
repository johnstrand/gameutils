using GameUtils.Math;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GameUtils.Tests.Math;

[TestClass]
public class MathFExtTests
{
    private const float Delta = 0.0001f;

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
}
