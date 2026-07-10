using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GameUtils.Animation;

namespace GameUtils.Tests.Animation;

[TestClass]
public class EaseTests
{
    [TestMethod]
    public void Clamp_NullEasingFunction_ThrowsArgumentNullException()
    {
        Assert.ThrowsExactly<ArgumentNullException>(() => Ease.Clamp(null!));
    }

    [TestMethod]
    public void Clamp_ValueBelowZero_ClampsToZero()
    {
        var clampedEase = Ease.Clamp(x => x);
        Assert.AreEqual(0f, clampedEase(-0.5f));
        Assert.AreEqual(0f, clampedEase(-10f));
    }

    [TestMethod]
    public void Clamp_ValueAboveOne_ClampsToOne()
    {
        var clampedEase = Ease.Clamp(x => x);
        Assert.AreEqual(1f, clampedEase(1.5f));
        Assert.AreEqual(1f, clampedEase(10f));
    }

    [TestMethod]
    public void Clamp_ValueInRange_PassesUnchanged()
    {
        var clampedEase = Ease.Clamp(x => x);
        Assert.AreEqual(0f, clampedEase(0f));
        Assert.AreEqual(0.5f, clampedEase(0.5f));
        Assert.AreEqual(1f, clampedEase(1f));
    }

    [TestMethod]
    public void QuinticBounceIn_ValidInput_ReturnsExpectedResult()
    {
        Assert.AreEqual(0f, Ease.QuinticBounceIn(0f), 0.0001f);
        Assert.AreEqual(-0.2747936f, Ease.QuinticBounceIn(0.5f), 0.0001f);
        Assert.AreEqual(1f, Ease.QuinticBounceIn(1f), 0.0001f);
    }
  
    [TestMethod]
    public void Clamp_InvokesEasingFunction_ReturnsFunctionResult()
    {
        // Use a non-identity function to verify the inner function is actually invoked.
        var clampedEase = Ease.Clamp(x => x * 2f + 5f);

        // -1f clamps to 0f -> 0f * 2f + 5f = 5f
        Assert.AreEqual(5f, clampedEase(-1f));

        // 0.5f clamps to 0.5f -> 0.5f * 2f + 5f = 6f
        Assert.AreEqual(6f, clampedEase(0.5f));

        // 2f clamps to 1f -> 1f * 2f + 5f = 7f
        Assert.AreEqual(7f, clampedEase(2f));
    }
  
    [TestMethod]
    public void BounceIn_Zero_ReturnsZero()
    {
        Assert.AreEqual(0f, Ease.BounceIn(0f), 0.0001f);
    }

    [TestMethod]
    public void BounceIn_Half_ReturnsExpectedValue()
    {
        Assert.AreEqual(0.234375f, Ease.BounceIn(0.5f), 0.0001f);
    }

    [TestMethod]
    public void BounceIn_One_ReturnsOne()
    {
        Assert.AreEqual(1f, Ease.BounceIn(1f), 0.0001f);
    }
}
