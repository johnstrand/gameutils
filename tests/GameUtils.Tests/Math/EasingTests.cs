using Microsoft.VisualStudio.TestTools.UnitTesting;
using GameUtils.Math;

namespace GameUtils.Tests.Math;

[TestClass]
public class EasingTests
{
    private const float Tolerance = 0.0001f;

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
