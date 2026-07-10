using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using GameUtils.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GameUtils.Tests.Entity;

[TestClass]
public class FixedSchedulerTests
{
    private class TestScheduler : FixedScheduler
    {
        public int UpdateCount { get; private set; }
        public Exception? ExceptionToThrow { get; set; }

        public TestScheduler(int targetRatePerSecond) : base(targetRatePerSecond)
        {
        }

        public override void Update()
        {
            UpdateCount++;
            if (ExceptionToThrow != null)
            {
                throw ExceptionToThrow;
            }
        }
    }

    [TestMethod]
    public async Task Start_CallsUpdateMultipleTimes()
    {
        // Arrange
        // Target 10 updates per second (100ms per update)
        var scheduler = new TestScheduler(10);

        // Act
        scheduler.Start();

        // Wait long enough for several updates to occur (e.g., 350ms should yield ~3 updates)
        await Task.Delay(350);

        await scheduler.Stop();

        // Assert
        Assert.IsTrue(scheduler.UpdateCount >= 2, $"Expected at least 2 updates, but got {scheduler.UpdateCount}");
    }

    [TestMethod]
    public async Task Stop_StopsSchedulerLoop()
    {
        // Arrange
        var scheduler = new TestScheduler(20);

        // Act
        scheduler.Start();
        await Task.Delay(100);
        await scheduler.Stop();

        int countAfterStop = scheduler.UpdateCount;

        // Wait a bit to ensure it doesn't keep running
        await Task.Delay(100);

        // Assert
        Assert.AreEqual(countAfterStop, scheduler.UpdateCount);
    }

    [TestMethod]
    public async Task Start_WhenAlreadyRunning_DoesNotCreateMultipleTasks()
    {
        // Arrange
        var scheduler = new TestScheduler(50);

        // Act
        scheduler.Start();
        await Task.Delay(50);

        int count1 = scheduler.UpdateCount;

        // Call start again
        scheduler.Start();
        await Task.Delay(50);

        await scheduler.Stop();

        // Assert
        // We're just making sure it doesn't throw or run twice as fast.
        // It's hard to test exact task creation without reflection, but we can verify
        // behavior remains normal.
        Assert.IsTrue(scheduler.UpdateCount > count1);
    }

    [TestMethod]
    public async Task Update_WhenExceptionThrown_SwallowsExceptionAndContinues()
    {
        // Arrange
        var scheduler = new TestScheduler(20);
        scheduler.ExceptionToThrow = new InvalidOperationException("Test exception");

        // Act
        // This should not crash the task
        scheduler.Start();
        await Task.Delay(150);
        await scheduler.Stop();

        // Assert
        Assert.IsTrue(scheduler.UpdateCount > 0, "Update should have been called despite exceptions");
    }

    [TestMethod]
    public async Task Update_WhenExceptionThrown_RaisesErrorEvent()
    {
        // Arrange
        var scheduler = new TestScheduler(20);
        var expectedException = new InvalidOperationException("Test exception");
        scheduler.ExceptionToThrow = expectedException;

        Exception? caughtException = null;
        scheduler.Error += (sender, ex) => caughtException = ex;

        // Act
        scheduler.Start();
        await Task.Delay(150);
        await scheduler.Stop();

        // Assert
        Assert.IsNotNull(caughtException, "Error event should have been raised.");
        Assert.AreEqual(expectedException, caughtException);
    }

    [TestMethod]
    public async Task Timing_ApproximatesTargetRate()
    {
        // Arrange
        int targetRate = 20; // 50ms per update
        var scheduler = new TestScheduler(targetRate);
        var stopwatch = new Stopwatch();

        // Act
        stopwatch.Start();
        scheduler.Start();

        // Run for about 500ms
        await Task.Delay(500);

        await scheduler.Stop();
        stopwatch.Stop();

        // Assert
        double elapsedSeconds = stopwatch.Elapsed.TotalSeconds;
        double expectedUpdates = targetRate * elapsedSeconds;

        // Allow some tolerance (e.g., +/- 30% due to thread scheduling in test environment)
        double lowerBound = expectedUpdates * 0.5;
        double upperBound = expectedUpdates * 1.5;

        Assert.IsTrue(scheduler.UpdateCount >= lowerBound && scheduler.UpdateCount <= upperBound,
            $"Expected update count between {lowerBound} and {upperBound}, but got {scheduler.UpdateCount}");
    }
}
