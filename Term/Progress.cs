using GameUtils.Extensions;

namespace GameUtils.Term;
public static class Progress
{
    public static TimeSpan TimeRemaining(int current, int total, DateTimeOffset start)
    {
        var rate = Rate(current, start);
        var remaining = (total - current) / rate;
        return TimeSpan.FromSeconds(remaining);
    }

    public static double Rate(int current, DateTimeOffset start)
    {
        var elapsed = DateTimeOffset.UtcNow - start;
        return current / elapsed.TotalSeconds;
    }

    public static int PercentComplete(int current, int total)
    {
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
        ArgumentOutOfRangeException.ThrowIfLessThan(current, 1, nameof(current));
        ArgumentOutOfRangeException.ThrowIfLessThan(total, 1, nameof(total));
        ArgumentOutOfRangeException.ThrowIfLessThan(width, 1, nameof(width));
        ArgumentOutOfRangeException.ThrowIfGreaterThan(current, total, nameof(current));

        var doneWidth = current * width / total;
        var pendingWidth = width - doneWidth;

        return $"{completed.Repeat(doneWidth)}{pending.Repeat(pendingWidth)}";
    }
}
