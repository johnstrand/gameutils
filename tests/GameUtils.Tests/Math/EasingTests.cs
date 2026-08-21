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
}