using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GameUtils.Animation;

namespace GameUtils.Tests.Animation;

[TestClass]
public class TweenerTests
{
    [TestMethod]
    public void Initialization_DefaultValues_AreCorrect()
    {
        var tweener = new Tweener(0f, 10f, 2f);

        Assert.AreEqual(0f, tweener.From);
        Assert.AreEqual(10f, tweener.To);
        Assert.AreEqual(2f, tweener.Duration);
        Assert.AreEqual(0f, tweener.Value);
        Assert.IsFalse(tweener.IsComplete);
        Assert.IsFalse(tweener.IsLooping);
        Assert.IsNotNull(tweener.EasingFunction);
    }

    [TestMethod]
    public void Update_AdvancesValue_BasedOnDuration()
    {
        var tweener = new Tweener(0f, 10f, 2f);

        tweener.Update(1f);

        Assert.AreEqual(5f, tweener.Value);
        Assert.IsFalse(tweener.IsComplete);
    }

    [TestMethod]
    public void Update_Completes_WhenDurationReached()
    {
        var tweener = new Tweener(0f, 10f, 2f);
        var completed = false;
        tweener.OnComplete = () => completed = true;

        tweener.Update(2f);

        Assert.AreEqual(10f, tweener.Value);
        Assert.IsTrue(tweener.IsComplete);
        Assert.IsTrue(completed);
    }

    [TestMethod]
    public void Update_DoesNotAdvance_IfAlreadyComplete()
    {
        var tweener = new Tweener(0f, 10f, 2f);
        tweener.Update(2f); // Completes it
        var completedValue = tweener.Value;

        tweener.Update(1f); // Should do nothing

        Assert.AreEqual(completedValue, tweener.Value);
        Assert.IsTrue(tweener.IsComplete);
    }

    [TestMethod]
    public void Update_ZeroDuration_CompletesImmediately()
    {
        var tweener = new Tweener(0f, 10f, 0f);
        var completed = false;
        tweener.OnComplete = () => completed = true;

        tweener.Update(1f);

        Assert.AreEqual(10f, tweener.Value);
        Assert.IsTrue(tweener.IsComplete);
        Assert.IsTrue(completed);
    }

    [TestMethod]
    public void Update_NegativeDuration_CompletesImmediately()
    {
        var tweener = new Tweener(0f, 10f, -1f);
        var completed = false;
        tweener.OnComplete = () => completed = true;

        tweener.Update(1f);

        Assert.AreEqual(10f, tweener.Value);
        Assert.IsTrue(tweener.IsComplete);
        Assert.IsTrue(completed);
    }

    [TestMethod]
    public void Update_Looping_WrapsAroundAndFiresEvent()
    {
        var tweener = new Tweener(0f, 10f, 2f) { IsLooping = true };
        var completionCount = 0;
        tweener.OnComplete = () => completionCount++;

        // Advance past the first duration
        tweener.Update(2.5f);

        // Elapsed time should wrap, effectively being at 0.5f
        // Value should be 0 + (10 - 0) * (0.5 / 2.0) = 2.5
        Assert.AreEqual(2.5f, tweener.Value);
        Assert.IsFalse(tweener.IsComplete);
        Assert.AreEqual(1, completionCount);
    }

    [TestMethod]
    public void Reset_RestoresInitialState_WithoutChangingConfig()
    {
        var tweener = new Tweener(0f, 10f, 2f);
        tweener.Update(2f); // Completes it

        tweener.Reset();

        Assert.AreEqual(0f, tweener.Value);
        Assert.IsFalse(tweener.IsComplete);
        Assert.AreEqual(0f, tweener.From);
        Assert.AreEqual(10f, tweener.To);
        Assert.AreEqual(2f, tweener.Duration);
    }

    [TestMethod]
    public void Restart_ChangesBounds_AndResetsState()
    {
        var tweener = new Tweener(0f, 10f, 2f);
        tweener.Update(1f);

        tweener.Restart(5f, 15f);

        Assert.AreEqual(5f, tweener.From);
        Assert.AreEqual(15f, tweener.To);
        Assert.AreEqual(5f, tweener.Value); // Value is set to From on Reset
        Assert.IsFalse(tweener.IsComplete);
        Assert.AreEqual(2f, tweener.Duration); // Duration remains unchanged
    }

    [TestMethod]
    public void CustomEasing_AppliedCorrectly()
    {
        // A simple custom easing that just squares the normalized time
        float CustomEase(float t) => t * t;

        var tweener = new Tweener(0f, 10f, 2f, CustomEase);

        tweener.Update(1f); // t = 0.5

        // Expected: 0 + (10 - 0) * (0.5 * 0.5) = 10 * 0.25 = 2.5
        Assert.AreEqual(2.5f, tweener.Value);
    }
}
