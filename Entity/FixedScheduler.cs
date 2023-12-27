namespace GameUtils.Entity;

/// <summary>
/// Base class for a scheduler that runs at a fixed rate.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="FixedScheduler"/> class.
/// </remarks>
public abstract class FixedScheduler(int targetRatePerSecond)
{
    private float _targetFrameTime = 1f / targetRatePerSecond;
    private bool _isRunning;
    private Task? _runningTask;

    /// <summary>
    /// Starts the scheduler. If the scheduler is already running, this method does nothing.
    /// </summary>
    public void Start()
    {
        if (_runningTask != null)
        {
            return;
        }

        _runningTask = Task.Run(() =>
        {
            _isRunning = true;

            // Keep track of how many frames we've processed. We'll use this to adjust the target frame time over time
            var frameCount = 0;

            // We'll also keep track of when we started, so we can calculate the drift
            var startTime = DateTime.Now;

            // We'll also keep track of how many seconds we will have expected to pass
            var expected = 0;
            while (_isRunning)
            {
                // Mark the start of the frame
                var frameStart = DateTime.Now;

                // Run the update
                Update();

                // Wait until the frame is over
                while ((DateTime.Now - frameStart).TotalSeconds < _targetFrameTime)
                {
                    // Just yield over and over until the frame is over
                    Thread.Yield();
                }

                // If we have processed the number of frames we expected to process in a second, let's see what the drift is
                if (++frameCount % targetRatePerSecond == 0)
                {
                    // Increment expected number of seconds
                    expected++;

                    // The drift is the difference between the time it took us to process targetRatePerSecond frames, and the time we expected it to take
                    var drift = (DateTime.Now - startTime).TotalSeconds - expected;

                    // If there is a drift, adjust the target frame time
                    if (drift != 0)
                    {
                        // We'll adjust the target frame time by 1/120th of the drift, so that we don't over-correct
                        _targetFrameTime -= (float)drift / 120f;
                    }
                }
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
    /// Updates the scheduler. This method is called at the target rate.
    /// </summary>
    public abstract void Update();
}