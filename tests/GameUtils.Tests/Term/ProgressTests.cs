using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GameUtils.Term;

namespace GameUtils.Tests.Term;

[TestClass]
public class ProgressTests
{
    [TestMethod]
    public void Bar_NegativeCurrent_ThrowsArgumentOutOfRangeException()
    {
        Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => Progress.Bar(-1, 10, 10, "#"));
    }

    [TestMethod]
    public void Bar_TotalLessThanOne_ThrowsArgumentOutOfRangeException()
    {
        Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => Progress.Bar(0, 0, 10, "#"));
        Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => Progress.Bar(0, -5, 10, "#"));
    }

    [TestMethod]
    public void Bar_WidthLessThanOne_ThrowsArgumentOutOfRangeException()
    {
        Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => Progress.Bar(0, 10, 0, "#"));
        Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => Progress.Bar(0, 10, -1, "#"));
    }

    [TestMethod]
    public void Bar_CurrentGreaterThanTotal_ThrowsArgumentOutOfRangeException()
    {
        Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => Progress.Bar(11, 10, 10, "#"));
    }

    [TestMethod]
    public void Bar_ValidInputs_ReturnsExpectedProgressBar()
    {
        Assert.AreEqual("          ", Progress.Bar(0, 10, 10, "#", " "));
        Assert.AreEqual("#####-----", Progress.Bar(5, 10, 10, "#", "-"));
        Assert.AreEqual("##########", Progress.Bar(10, 10, 10, "#"));
        Assert.AreEqual("##...", Progress.Bar(1, 2, 5, "#", "."));
    }

    [TestMethod]
    public void PercentComplete_ValidAndZeroTotal_ReturnsExpectedPercent()
    {
        Assert.AreEqual(0, Progress.PercentComplete(0, 0));
        Assert.AreEqual(50, Progress.PercentComplete(5, 10));
        Assert.AreEqual(100, Progress.PercentComplete(10, 10));
    }

    [TestMethod]
    public void RateAndRemaining_ValidAndEdgeCases_BehavesAsExpected()
    {
        var futureStart = DateTimeOffset.UtcNow.AddMinutes(1);
        Assert.AreEqual(0, Progress.Rate(5, futureStart));
        Assert.AreEqual(TimeSpan.MaxValue, Progress.TimeRemaining(5, 10, futureStart));
    }
}
