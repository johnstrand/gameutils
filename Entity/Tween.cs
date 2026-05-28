using System.Numerics;

namespace GameUtils.Entity;

/// <summary>
/// Drives a smooth transition of a value of type <typeparamref name="T"/> over a fixed duration,
/// using a pluggable easing function.
/// <para>
/// Use the static factory methods (<see cref="Tween.Float"/>, <see cref="Tween.Vec2"/>,
/// <see cref="Tween.Vec3"/>, <see cref="Tween.Color"/>) for common types, or supply custom lerp
/// and easing delegates.
/// </para>
/// </summary>
/// <typeparam name="T">The value type being animated.</typeparam>
public class Tween<T>
{
    private readonly T _from;
    private readonly T _to;
    private readonly float _duration;
    private readonly Func<T, T, float, T> _lerp;
    private readonly Func<float, float> _easing;

    private float _elapsed;
    private bool _reversed;

    /// <summary>The current interpolated value.</summary>
    public T Value { get; private set; }

    /// <summary><see langword="true"/> when the tween has reached its destination.</summary>
    public bool IsComplete { get; private set; }

    /// <summary>
    /// Creates a new tween.
    /// </summary>
    /// <param name="from">Starting value.</param>
    /// <param name="to">Target value.</param>
    /// <param name="duration">Duration in seconds.</param>
    /// <param name="lerp">Interpolation function: <c>(from, to, t) → value</c>.</param>
    /// <param name="easing">
    /// Easing function applied to the normalized time before lerping.
    /// Use any method from <see cref="Math.Easing"/> or supply a custom one.
    /// Defaults to linear if <see langword="null"/>.
    /// </param>
    public Tween(T from, T to, float duration, Func<T, T, float, T> lerp, Func<float, float>? easing = null)
    {
        _from = from;
        _to = to;
        _duration = duration > 0 ? duration : throw new ArgumentOutOfRangeException(nameof(duration), "Duration must be positive.");
        _lerp = lerp ?? throw new ArgumentNullException(nameof(lerp));
        _easing = easing ?? (t => t);
        Value = from;
    }

    /// <summary>
    /// Advances the tween by <paramref name="delta"/> seconds and returns the current value.
    /// Does nothing and returns the final value once <see cref="IsComplete"/> is <see langword="true"/>.
    /// </summary>
    public T Update(float delta)
    {
        if (IsComplete)
        {
            return Value;
        }

        _elapsed = System.Math.Clamp(_elapsed + delta, 0f, _duration);
        var t = _elapsed / _duration;

        if (_reversed)
        {
            t = 1f - t;
        }

        Value = _lerp(_from, _to, _easing(t));

        if (_elapsed >= _duration)
        {
            IsComplete = true;
        }

        return Value;
    }

    /// <summary>Resets the tween to its starting state.</summary>
    public void Reset()
    {
        _elapsed = 0f;
        _reversed = false;
        IsComplete = false;
        Value = _from;
    }

    /// <summary>
    /// Reverses the playback direction. If the tween is complete, also restarts it.
    /// </summary>
    public void Reverse()
    {
        _reversed = !_reversed;
        if (IsComplete)
        {
            _elapsed = 0f;
            IsComplete = false;
        }
    }
}

/// <summary>
/// Static factory helpers for creating <see cref="Tween{T}"/> instances for common value types.
/// </summary>
public static class Tween
{
    /// <summary>Creates a tween that interpolates a <see cref="float"/> value.</summary>
    public static Tween<float> Float(float from, float to, float duration, Func<float, float>? easing = null)
        => new(from, to, duration, (a, b, t) => a + (b - a) * t, easing);

    /// <summary>Creates a tween that interpolates a <see cref="Vector2"/> value.</summary>
    public static Tween<Vector2> Vec2(Vector2 from, Vector2 to, float duration, Func<float, float>? easing = null)
        => new(from, to, duration, Vector2.Lerp, easing);

    /// <summary>Creates a tween that interpolates a <see cref="Vector3"/> value.</summary>
    public static Tween<Vector3> Vec3(Vector3 from, Vector3 to, float duration, Func<float, float>? easing = null)
        => new(from, to, duration, Vector3.Lerp, easing);

    /// <summary>Creates a tween that interpolates a <see cref="Color"/> value.</summary>
    public static Tween<Color> Color(Color from, Color to, float duration, Func<float, float>? easing = null)
        => new(from, to, duration, GameUtils.Color.Lerp, easing);
}
