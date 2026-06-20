using GameUtils.Extensions;

namespace GameUtils.Term;

/// <summary>
/// Utility method for calculating and displying progress.
/// </summary>
public static class Progress
{
    /// <summary>
    /// Given the current count, total count, and start time, returns the estimated time remaining.
    /// Returns <see cref="TimeSpan.MaxValue"/> when the rate is zero (i.e., no progress yet).
    /// </summary>
    public static TimeSpan TimeRemaining(int current, int total, DateTimeOffset start)
    {
        var rate = Rate(current, start);
        if (rate <= 0)
        {
            return TimeSpan.MaxValue;
        }

        var remaining = (total - current) / rate;
        return TimeSpan.FromSeconds(remaining);
    }

    /// <summary>
    /// Given the current count and start time, returns the current rate.
    /// Returns 0 when elapsed time is zero or negative.
    /// </summary>
    public static double Rate(int current, DateTimeOffset start)
    {
        var elapsed = DateTimeOffset.UtcNow - start;
        if (elapsed.TotalSeconds <= 0)
        {
            return 0;
        }

        return current / elapsed.TotalSeconds;
    }

    /// <summary>
    /// Given the current count and total count, returns the percent complete.
    /// Returns 0 when total is zero.
    /// </summary>
    public static int PercentComplete(int current, int total)
    {
        if (total == 0)
        {
            return 0;
        }

        return current * 100 / total;
    }

    /// <summary>
    /// Returns a progress bar with the given width, completed character, and pending character.
    /// </summary>
    /// <param name="current">The current count (in absolute terms)</param>
    /// <param name="total">Total elements to process</param>
    /// <param name="width">The total width of the progress bar</param>
    /// <param name="completed">The character or string to be used to indicate the completed part</param>
    /// <param name="pending">The character or string to be used to indicate the remaining part</param>
    public static string Bar(int current, int total, int width, string completed, string pending = " ")
    {
        ArgumentOutOfRangeException.ThrowIfNegative(current);
        ArgumentOutOfRangeException.ThrowIfLessThan(total, 1);
        ArgumentOutOfRangeException.ThrowIfLessThan(width, 1);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(current, total);

        var doneWidth = current * width / total;
        var pendingWidth = width - doneWidth;

        return completed.Repeat(doneWidth) + pending.Repeat(pendingWidth);
    }
}
