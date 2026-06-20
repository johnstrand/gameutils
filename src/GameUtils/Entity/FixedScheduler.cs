using System.Diagnostics;

namespace GameUtils.Entity;

/// <summary>
/// Base class for a scheduler that calls <see cref="Update"/> at a fixed rate.
/// </summary>
/// <remarks>
/// Timing uses a <see cref="Stopwatch"/> (high-resolution monotonic clock) with a hybrid
/// sleep + spin-wait strategy: the thread sleeps for the bulk of each idle period, then
/// spin-waits the last millisecond to achieve sub-millisecond frame precision.
/// </remarks>
public abstract class FixedScheduler(int targetRatePerSecond)
{
    private readonly TimeSpan _interval = TimeSpan.FromSeconds(1.0 / targetRatePerSecond);
    private volatile bool _isRunning;
    private Task? _runningTask;

    /// <summary>
    /// Starts the scheduler. If the scheduler is already running, this method does nothing.
    /// </summary>
    public void Start()
    {
        if (_runningTask != null && !_runningTask.IsCompleted)
        {
            return;
        }

        _isRunning = true;
        _runningTask = Task.Run(() =>
        {
            var stopwatch = Stopwatch.StartNew();
            var nextTick = _interval;

            while (_isRunning)
            {
                try
                {
                    Update();
                }
                catch (Exception)
                {
                    // Swallow exceptions to keep the loop alive; override Update to handle errors.
                }

                // Sleep for the bulk of the remaining time, leaving ~1ms for spin-wait precision
                var remaining = nextTick - stopwatch.Elapsed;
                var sleepTime = remaining - TimeSpan.FromMilliseconds(1);

                if (sleepTime > TimeSpan.Zero)
                {
                    Thread.Sleep(sleepTime);
                }

                // Spin-wait for the final millisecond
                while (stopwatch.Elapsed < nextTick && _isRunning)
                {
                    Thread.SpinWait(1);
                }

                // Advance to the next absolute tick time.
                // If Update() ran long and we're already past nextTick, the next frame fires immediately.
                nextTick += _interval;
            }
        });
    }

    /// <summary>
    /// Stops the scheduler. If the scheduler is not running, this method does nothing.
    /// </summary>
    public async Task Stop()
    {
        if (!_isRunning || _runningTask == null)
        {
            return;
        }

        _isRunning = false;
        await _runningTask;
        _runningTask = null;
    }

    /// <summary>
    /// Called at the target rate. Override to implement per-tick logic.
    /// </summary>
    public abstract void Update();
}
