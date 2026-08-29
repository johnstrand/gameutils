using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GameUtils.Term;

namespace GameUtils.Tests.Term;

[TestClass]
public class ProgressTests
{
    [TestMethod]
    public void TimeRemaining_ZeroCurrent_ReturnsTimeSpanMaxValue()
    {
        var start = DateTimeOffset.UtcNow.AddSeconds(-10);
        var result = Progress.TimeRemaining(0, 100, start);
        Assert.AreEqual(TimeSpan.MaxValue, result);
    }

    [TestMethod]
    public void TimeRemaining_FutureOrSameStart_ReturnsTimeSpanMaxValue()
    {
        var start = DateTimeOffset.UtcNow.AddSeconds(5);
        var result = Progress.TimeRemaining(5, 100, start);
        Assert.AreEqual(TimeSpan.MaxValue, result);
    }

    [TestMethod]
    public void TimeRemaining_ValidProgress_ReturnsEstimatedTimeRemaining()
    {
        var start = DateTimeOffset.UtcNow.AddSeconds(-10);
        var result = Progress.TimeRemaining(10, 100, start);
        Assert.AreEqual(90.0, result.TotalSeconds, 1.0);
    }

    [TestMethod]
    public void TimeRemaining_CompletedProgress_ReturnsZeroOrNegative()
    {
        var start = DateTimeOffset.UtcNow.AddSeconds(-10);
        var result = Progress.TimeRemaining(100, 100, start);
        Assert.AreEqual(TimeSpan.Zero, result);
    }

    [TestMethod]
    public void Rate_FutureOrNowStart_ReturnsZero()
    {
        var start = DateTimeOffset.UtcNow.AddSeconds(5);
        var rate = Progress.Rate(10, start);
        Assert.AreEqual(0.0, rate);
    }

    [TestMethod]
    public void Rate_ValidElapsed_ReturnsCorrectRate()
    {
        var start = DateTimeOffset.UtcNow.AddSeconds(-10);
        var rate = Progress.Rate(20, start);
        Assert.AreEqual(2.0, rate, 0.2);
    }

    [TestMethod]
    public void PercentComplete_ZeroTotal_ReturnsZero()
    {
        Assert.AreEqual(0, Progress.PercentComplete(5, 0));
    }

    [TestMethod]
    public void PercentComplete_ValidTotal_ReturnsPercentage()
    {
        Assert.AreEqual(50, Progress.PercentComplete(5, 10));
        Assert.AreEqual(100, Progress.PercentComplete(10, 10));
    }

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
    public void Bar_ValidParameters_ReturnsFormattedString()
    {
        var bar = Progress.Bar(5, 10, 10, "#", "-");
        Assert.AreEqual("#####-----", bar);
    }

    [TestMethod]
    public void Bar_InvalidParameters_ThrowsArgumentOutOfRangeException()
    {
        Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => Progress.Bar(-1, 10, 10, "#"));
        Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => Progress.Bar(5, 0, 10, "#"));
        Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => Progress.Bar(5, 10, 0, "#"));
        Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => Progress.Bar(15, 10, 10, "#"));
    }

    [TestMethod]
    public void RateAndRemaining_ValidAndEdgeCases_BehavesAsExpected()
    {
        var futureStart = DateTimeOffset.UtcNow.AddMinutes(1);
        Assert.AreEqual(0, Progress.Rate(5, futureStart));
        Assert.AreEqual(TimeSpan.MaxValue, Progress.TimeRemaining(5, 10, futureStart));
    }
}
