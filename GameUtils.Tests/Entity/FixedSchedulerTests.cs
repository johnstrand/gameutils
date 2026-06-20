using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using GameUtils.Entity;
using Xunit;

namespace GameUtils.Tests.Entity;

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

    [Fact]
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
        Assert.True(scheduler.UpdateCount >= 2, $"Expected at least 2 updates, but got {scheduler.UpdateCount}");
    }

    [Fact]
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
        Assert.Equal(countAfterStop, scheduler.UpdateCount);
    }

    [Fact]
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
        Assert.True(scheduler.UpdateCount > count1);
    }

    [Fact]
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
        Assert.True(scheduler.UpdateCount > 0, "Update should have been called despite exceptions");
    }

    [Fact]
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

        Assert.InRange(scheduler.UpdateCount, lowerBound, upperBound);
    }
}
