using System;
using GameUtils.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GameUtils.Tests.Entity;

[TestClass]
public class ThinkerTests
{
    [TestMethod]
    public void Constructor_SetsInitialValues()
    {
        var thinker = new Thinker(1.5f);
        Assert.AreEqual(1.5f, thinker.UntilNextThink);
    }

    [TestMethod]
    public void Update_WhenTimeNotPassed_DoesNotInvokeOnThink()
    {
        var thinker = new Thinker(1.0f);
        bool onThinkInvoked = false;
        thinker.OnThink = () => onThinkInvoked = true;
        thinker.Update(0.5f);
        Assert.IsFalse(onThinkInvoked);
        Assert.AreEqual(0.5f, thinker.UntilNextThink);
    }

    [TestMethod]
    public void Update_WhenTimePassed_InvokesOnThinkAndResetsUntilNextThink()
    {
        var thinker = new Thinker(1.0f);
        bool onThinkInvoked = false;
        thinker.OnThink = () => onThinkInvoked = true;
        thinker.Update(1.5f);
        Assert.IsTrue(onThinkInvoked);
        Assert.AreEqual(0.5f, thinker.UntilNextThink);
    }

    [TestMethod]
    public void Update_WhenIntervalIsZeroOrLess_DoesNothing()
    {
        var thinker = new Thinker(0f);
        bool onThinkInvoked = false;
        thinker.OnThink = () => onThinkInvoked = true;
        thinker.Update(1.0f);
        Assert.IsFalse(onThinkInvoked);
        Assert.AreEqual(0f, thinker.UntilNextThink);

        var thinker2 = new Thinker(-1f);
        bool onThinkInvoked2 = false;
        thinker2.OnThink = () => onThinkInvoked2 = true;
        thinker2.Update(1.0f);
        Assert.IsFalse(onThinkInvoked2);
        Assert.AreEqual(-1f, thinker2.UntilNextThink);
    }
}
