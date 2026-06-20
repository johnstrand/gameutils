using System;
using Xunit;
using GameUtils.Animation;

namespace GameUtils.Tests.Animation;

public class TweenerTests
{
    [Fact]
    public void Initialization_DefaultValues_AreCorrect()
    {
        var tweener = new Tweener(0f, 10f, 2f);

        Assert.Equal(0f, tweener.From);
        Assert.Equal(10f, tweener.To);
        Assert.Equal(2f, tweener.Duration);
        Assert.Equal(0f, tweener.Value);
        Assert.False(tweener.IsComplete);
        Assert.False(tweener.IsLooping);
        Assert.NotNull(tweener.EasingFunction);
    }

    [Fact]
    public void Update_AdvancesValue_BasedOnDuration()
    {
        var tweener = new Tweener(0f, 10f, 2f);

        tweener.Update(1f);

        Assert.Equal(5f, tweener.Value);
        Assert.False(tweener.IsComplete);
    }

    [Fact]
    public void Update_Completes_WhenDurationReached()
    {
        var tweener = new Tweener(0f, 10f, 2f);
        var completed = false;
        tweener.OnComplete = () => completed = true;

        tweener.Update(2f);

        Assert.Equal(10f, tweener.Value);
        Assert.True(tweener.IsComplete);
        Assert.True(completed);
    }

    [Fact]
    public void Update_DoesNotAdvance_IfAlreadyComplete()
    {
        var tweener = new Tweener(0f, 10f, 2f);
        tweener.Update(2f); // Completes it
        var completedValue = tweener.Value;

        tweener.Update(1f); // Should do nothing

        Assert.Equal(completedValue, tweener.Value);
        Assert.True(tweener.IsComplete);
    }

    [Fact]
    public void Update_ZeroDuration_CompletesImmediately()
    {
        var tweener = new Tweener(0f, 10f, 0f);
        var completed = false;
        tweener.OnComplete = () => completed = true;

        tweener.Update(1f);

        Assert.Equal(10f, tweener.Value);
        Assert.True(tweener.IsComplete);
        Assert.True(completed);
    }

    [Fact]
    public void Update_NegativeDuration_CompletesImmediately()
    {
        var tweener = new Tweener(0f, 10f, -1f);
        var completed = false;
        tweener.OnComplete = () => completed = true;

        tweener.Update(1f);

        Assert.Equal(10f, tweener.Value);
        Assert.True(tweener.IsComplete);
        Assert.True(completed);
    }

    [Fact]
    public void Update_Looping_WrapsAroundAndFiresEvent()
    {
        var tweener = new Tweener(0f, 10f, 2f) { IsLooping = true };
        var completionCount = 0;
        tweener.OnComplete = () => completionCount++;

        // Advance past the first duration
        tweener.Update(2.5f);

        // Elapsed time should wrap, effectively being at 0.5f
        // Value should be 0 + (10 - 0) * (0.5 / 2.0) = 2.5
        Assert.Equal(2.5f, tweener.Value);
        Assert.False(tweener.IsComplete);
        Assert.Equal(1, completionCount);
    }

    [Fact]
    public void Reset_RestoresInitialState_WithoutChangingConfig()
    {
        var tweener = new Tweener(0f, 10f, 2f);
        tweener.Update(2f); // Completes it

        tweener.Reset();

        Assert.Equal(0f, tweener.Value);
        Assert.False(tweener.IsComplete);
        Assert.Equal(0f, tweener.From);
        Assert.Equal(10f, tweener.To);
        Assert.Equal(2f, tweener.Duration);
    }

    [Fact]
    public void Restart_ChangesBounds_AndResetsState()
    {
        var tweener = new Tweener(0f, 10f, 2f);
        tweener.Update(1f);

        tweener.Restart(5f, 15f);

        Assert.Equal(5f, tweener.From);
        Assert.Equal(15f, tweener.To);
        Assert.Equal(5f, tweener.Value); // Value is set to From on Reset
        Assert.False(tweener.IsComplete);
        Assert.Equal(2f, tweener.Duration); // Duration remains unchanged
    }

    [Fact]
    public void CustomEasing_AppliedCorrectly()
    {
        // A simple custom easing that just squares the normalized time
        float CustomEase(float t) => t * t;

        var tweener = new Tweener(0f, 10f, 2f, CustomEase);

        tweener.Update(1f); // t = 0.5

        // Expected: 0 + (10 - 0) * (0.5 * 0.5) = 10 * 0.25 = 2.5
        Assert.Equal(2.5f, tweener.Value);
    }
}
