namespace GameUtils.Entity;

internal class Delta
{
    public static Delta Instance { get; } = new();

    private DateTime _lastTime = DateTime.UtcNow;
    private readonly DateTime _startTime = DateTime.UtcNow;

    /// <summary>
    /// The number of seconds since last time this property was accessed.
    /// </summary>
    public double Elapsed => ElapsedInternal().TotalSeconds;

    /// <summary>
    /// The number of seconds since the program started.
    /// </summary>
    public double TotalElapsed => (DateTime.UtcNow - _startTime).TotalSeconds;

    private TimeSpan ElapsedInternal()
    {
        var now = DateTime.UtcNow;
        var elapsed = now - _lastTime;
        _lastTime = now;
        return elapsed;
    }
}
