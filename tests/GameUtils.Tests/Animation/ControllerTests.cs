using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GameUtils.Animation;

namespace GameUtils.Tests.Animation;

[TestClass]
public class ControllerTests
{
    [TestMethod]
    public void Initialization_DefaultValues_AreCorrect()
    {
        var controller = new Controller(10);
        Assert.AreEqual(10, controller.FrameCount);
        Assert.IsTrue(controller.IsLooping);
        Assert.AreEqual(30f, controller.FramesPerSecond);
        Assert.AreEqual(0, controller.CurrentFrame);
        Assert.IsFalse(controller.IsPlaying);
    }

    [TestMethod]
    public void Play_SetsIsPlayingToTrue()
    {
        var controller = new Controller(10);
        controller.Play();
        Assert.IsTrue(controller.IsPlaying);
    }

    [TestMethod]
    public void Pause_SetsIsPlayingToFalse_MaintainsState()
    {
        var controller = new Controller(10);
        controller.Play();
        controller.Update(1f / 30f * 1.5f);
        var frame = controller.CurrentFrame;
        controller.Pause();
        Assert.IsFalse(controller.IsPlaying);
        Assert.AreEqual(frame, controller.CurrentFrame);
    }

    [TestMethod]
    public void Stop_ResetsState_AndFiresOnStopped()
    {
        var controller = new Controller(10);
        controller.Play();
        controller.Update(1f / 30f * 1.5f);
        var stoppedFired = false;
        controller.OnStopped = () => stoppedFired = true;
        controller.Stop();
        Assert.IsFalse(controller.IsPlaying);
        Assert.AreEqual(0, controller.CurrentFrame);
        Assert.IsTrue(stoppedFired);
    }

    [TestMethod]
    public void Update_WhenNotPlaying_DoesNothing()
    {
        var controller = new Controller(10);
        controller.Update(1f);
        Assert.AreEqual(0, controller.CurrentFrame);
    }

    [TestMethod]
    public void Update_AdvancesFrame_AndFiresOnFrameChanged()
    {
        var controller = new Controller(10, framesPerSecond: 10);
        controller.Play();
        var frameChangedCount = 0;
        var lastFrame = -1;
        controller.OnFrameChanged = f => { frameChangedCount++; lastFrame = f; };
        controller.Update(0.15f);
        Assert.AreEqual(1, controller.CurrentFrame);
        Assert.AreEqual(1, frameChangedCount);
        Assert.AreEqual(1, lastFrame);
    }

    [TestMethod]
    public void Update_MultipleFrames_WhenDeltaTimeIsLarge()
    {
        var controller = new Controller(10, framesPerSecond: 10);
        controller.Play();
        controller.Update(0.35f);
        Assert.AreEqual(3, controller.CurrentFrame);
    }

    [TestMethod]
    public void Update_Looping_WrapsAround()
    {
        var controller = new Controller(4, isLooping: true, framesPerSecond: 10);
        controller.Play();
        controller.Update(0.55f);
        Assert.AreEqual(1, controller.CurrentFrame);
        Assert.IsTrue(controller.IsPlaying);
    }

    [TestMethod]
    public void Update_NotLooping_StopsAtEnd()
    {
        var controller = new Controller(4, isLooping: false, framesPerSecond: 10);
        controller.Play();
        var stoppedFired = false;
        controller.OnStopped = () => stoppedFired = true;
        controller.Update(0.55f);
        Assert.IsFalse(controller.IsPlaying);
        Assert.AreEqual(0, controller.CurrentFrame);
        Assert.IsTrue(stoppedFired);
    }

    [TestMethod]
    public void FramesPerSecond_Set_UpdatesFrameDuration()
    {
        var controller = new Controller(10, framesPerSecond: 10);
        controller.Play();
        controller.FramesPerSecond = 20;
        controller.Update(0.06f);
        Assert.AreEqual(1, controller.CurrentFrame);
    }
}
