namespace GameUtils.Entity;

/// <summary>
/// The thinker thinks every interval. The interval can be changed at any time. The interval may be set to any unit of time, but should match whatever is passed to Update.
/// If the interval is set to 0 or less, the thinker will never think.
/// </summary>
public class Thinker(float interval)
{
    /// <summary>
    /// How often the thinker thinks.
    /// </summary>
    private float _thinkInterval = interval;

    /// <summary>
    /// Time until the next think.
    /// </summary>
    public float UntilNextThink { get; private set; } = interval;

    /// <summary>
    /// Called when the thinker thinks.
    /// </summary>
    public Action? OnThink { get; set; }

    /// <summary>
    /// Should be called every frame, to let the thinker know how much time has passed.
    /// </summary>
    /// <param name="deltaTime">Time since last update, should match the unit of time used for <see cref="Thinker(float)"/>.</param>
    public void Update(float deltaTime)
    {
        if (_thinkInterval <= 0)
        {
            return;
        }
        UntilNextThink -= deltaTime;

        if (UntilNextThink > 0)
        {
            return;
        }

        UntilNextThink += UntilNextThink;
        OnThink?.Invoke();
    }

    /// <summary>
    /// Resets time until next think to the configured interval
    /// </summary>
    public void Reset()
    {
        UntilNextThink = _thinkInterval;
    }

    /// <summary>
    /// Sets both interval and time until next think to the given value
    /// </summary>
    public void Reset(float nextThink)
    {
        UntilNextThink = nextThink;
        _thinkInterval = nextThink;
    }

    /// <summary>
    /// Set interval and time until next think independently
    /// </summary>
    public void Reset(float nextThink, float thinkTime)
    {
        UntilNextThink = nextThink;
        _thinkInterval = thinkTime;
    }

    /// <summary>
    /// Make the thinker wait a bit extra until next think (increments time until next think without affecting the interval)
    /// </summary>
    public void Wait(float time)
    {
        UntilNextThink += time;
    }
}
