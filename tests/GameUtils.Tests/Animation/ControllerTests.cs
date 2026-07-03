using Microsoft.VisualStudio.TestTools.UnitTesting;
using GameUtils.Animation;
using System;

namespace GameUtils.Tests.Animation;

[TestClass]
public class ControllerTests
{
    [TestMethod]
    public void Constructor_DefaultValues_AreCorrect()
    {
        var controller = new Controller(10);
        Assert.AreEqual(10, controller.FrameCount);
        Assert.IsTrue(controller.IsLooping);
        Assert.AreEqual(30f, controller.FramesPerSecond);
        Assert.IsFalse(controller.IsPlaying);
        Assert.AreEqual(0, controller.CurrentFrame);
    }

    [TestMethod]
    public void Play_StartsPlayback_SetsIsPlayingTrue()
    {
        var controller = new Controller(10);
        controller.Play();
        Assert.IsTrue(controller.IsPlaying);
    }

    [TestMethod]
    public void Pause_StopsPlayback_SetsIsPlayingFalse()
    {
        var controller = new Controller(10);
        controller.Play();
        controller.Pause();
        Assert.IsFalse(controller.IsPlaying);
    }

    [TestMethod]
    public void Stop_ResetsState_TriggersOnStoppedEvent()
    {
        var controller = new Controller(10);
        controller.Play();
        controller.Update(1f); // Advance a bit

        bool eventFired = false;
        controller.OnStopped = () => eventFired = true;

        controller.Stop();

        Assert.IsFalse(controller.IsPlaying);
        Assert.AreEqual(0, controller.CurrentFrame);
        Assert.IsTrue(eventFired);
    }

    [TestMethod]
    public void Update_NotPlaying_DoesNothing()
    {
        var controller = new Controller(10);
        controller.Update(1f);

        Assert.AreEqual(0, controller.CurrentFrame);
        Assert.IsFalse(controller.IsPlaying);
    }

    [TestMethod]
    public void Update_AdvancesFrame_BasedOnDeltaTime()
    {
        var controller = new Controller(10, true, 1f); // 1 frame per second
        controller.Play();

        controller.Update(0.5f); // Half a frame
        Assert.AreEqual(0, controller.CurrentFrame);

        controller.Update(0.6f); // Total 1.1 frames
        Assert.AreEqual(1, controller.CurrentFrame);
    }

    [TestMethod]
    public void Update_Loops_WhenIsLoopingTrue()
    {
        var controller = new Controller(5, true, 1f); // 5 frames, 1 FPS
        controller.Play();

        controller.Update(4.5f); // frame 4
        Assert.AreEqual(4, controller.CurrentFrame);

        controller.Update(1f); // wrap around to frame 0
        Assert.AreEqual(0, controller.CurrentFrame);
        Assert.IsTrue(controller.IsPlaying);
    }

    [TestMethod]
    public void Update_StopsAtEnd_WhenIsLoopingFalse()
    {
        var controller = new Controller(5, false, 1f);
        controller.Play();

        bool eventFired = false;
        controller.OnStopped = () => eventFired = true;

        controller.Update(5.5f); // past the end

        Assert.AreEqual(0, controller.CurrentFrame); // Stop resets to 0
        Assert.IsFalse(controller.IsPlaying);
        Assert.IsTrue(eventFired);
    }

    [TestMethod]
    public void Update_MultipleFrames_TriggersEventsCorrectly()
    {
        var controller = new Controller(10, true, 1f); // 1 FPS
        controller.Play();

        int eventFiredCount = 0;
        int lastFrame = -1;
        controller.OnFrameChanged = frame =>
        {
            eventFiredCount++;
            lastFrame = frame;
        };

        controller.Update(3.5f); // Should advance 3 frames

        Assert.AreEqual(3, eventFiredCount);
        Assert.AreEqual(3, controller.CurrentFrame);
        Assert.AreEqual(3, lastFrame);
    }

    [TestMethod]
    public void FramesPerSecond_Setter_UpdatesFrameDuration()
    {
        var controller = new Controller(10, true, 10f); // 10 FPS, so duration is 0.1s
        controller.Play();

        // Change to 1 FPS, so duration is 1.0s
        controller.FramesPerSecond = 1f;

        controller.Update(0.5f);
        Assert.AreEqual(0, controller.CurrentFrame); // Has not advanced a full frame

        controller.Update(0.6f); // Total 1.1s
        Assert.AreEqual(1, controller.CurrentFrame); // Now it advanced
    }
}
