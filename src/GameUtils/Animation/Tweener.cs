namespace GameUtils.Animation;

/// <summary>
/// A time-driven animated float value that interpolates from <see cref="From"/> to <see cref="To"/> over
/// <see cref="Duration"/> seconds using a configurable easing function.
/// </summary>
/// <remarks>
/// Creates a new tweener.
/// </remarks>
public class Tweener(float from, float to, float duration, Func<float, float>? easingFunction = null)
{
    /// <summary>
    /// The starting value.
    /// </summary>
    public float From { get; set; } = from;

    /// <summary>
    /// The ending value.
    /// </summary>
    public float To { get; set; } = to;

    /// <summary>
    /// The total duration of the tween in seconds.
    /// </summary>
    public float Duration { get; set; } = duration;

    /// <summary>
    /// The easing function applied to the normalized time. Defaults to linear.
    /// </summary>
    public Func<float, float> EasingFunction { get; set; } = easingFunction ?? Ease.Linear;

    /// <summary>
    /// Whether the tween loops back to the start when complete.
    /// </summary>
    public bool IsLooping { get; set; }

    /// <summary>
    /// The current interpolated value.
    /// </summary>
    public float Value { get; private set; } = from;

    /// <summary>
    /// True when the tween has reached <see cref="To"/> and is no longer updating.
    /// </summary>
    public bool IsComplete { get; private set; }

    /// <summary>
    /// Called once when the tween completes (not called each loop iteration when <see cref="IsLooping"/> is true).
    /// </summary>
    public Action? OnComplete { get; set; }

    private float _elapsed;

    /// <summary>
    /// Advances the tween by <paramref name="deltaTime"/> seconds.
    /// </summary>
    public void Update(float deltaTime)
    {
        if (IsComplete)
        {
            return;
        }

        if (Duration <= 0)
        {
            Value = To;
            IsComplete = true;
            OnComplete?.Invoke();
            return;
        }

        _elapsed += deltaTime;

        if (_elapsed >= Duration)
        {
            if (IsLooping)
            {
                _elapsed %= Duration;
                OnComplete?.Invoke();
            }
            else
            {
                _elapsed = Duration;
                IsComplete = true;
            }
        }

        var t = EasingFunction(_elapsed / Duration);
        Value = From + ((To - From) * t);

        if (IsComplete)
        {
            OnComplete?.Invoke();
        }
    }

    /// <summary>
    /// Resets elapsed time to zero and marks the tween as not complete, without changing From/To/Duration.
    /// </summary>
    public void Reset()
    {
        _elapsed = 0;
        IsComplete = false;
        Value = From;
    }

    /// <summary>
    /// Resets and immediately starts the tween from <paramref name="from"/> to <paramref name="to"/>.
    /// </summary>
    public void Restart(float from, float to)
    {
        From = from;
        To = to;
        Reset();
    }
}
