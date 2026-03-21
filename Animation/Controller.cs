namespace GameUtils.Animation;

/// <summary>
/// Controls frame-based animation playback.
/// </summary>
public class Controller(int frameCount, bool isLooping = true, float framesPerSecond = 30)
{
    /// <summary>The zero-based index of the current frame.</summary>
    public int CurrentFrame { get; private set; }

    /// <summary>Total number of frames in the animation.</summary>
    public int FrameCount { get; set; } = frameCount;

    /// <summary>True while the animation is playing.</summary>
    public bool IsPlaying { get; private set; }

    /// <summary>When true the animation loops back to frame 0 after the last frame.</summary>
    public bool IsLooping { get; set; } = isLooping;

    /// <summary>
    /// Playback speed in frames per second.
    /// </summary>
    public float FramesPerSecond
    {
        get => framesPerSecond;
        set
        {
            framesPerSecond = value;
            _frameDuration = 1 / value;
        }
    }

    /// <summary>Called each time the current frame advances. The new frame index is passed as the argument.</summary>
    public Action<int>? OnFrameChanged { get; set; }

    /// <summary>Called when the animation stops (either naturally at the end or via <see cref="Stop"/>).</summary>
    public Action? OnStopped { get; set; }

    private float _subFrame;
    private float _frameDuration = 1 / framesPerSecond;

    /// <summary>Starts or resumes playback.</summary>
    public void Play()
    {
        IsPlaying = true;
    }

    /// <summary>Stops playback and resets to frame 0.</summary>
    public void Stop()
    {
        IsPlaying = false;
        CurrentFrame = 0;
        _subFrame = 0;
        OnStopped?.Invoke();
    }

    /// <summary>Pauses playback without resetting the current frame.</summary>
    public void Pause()
    {
        IsPlaying = false;
    }

    /// <summary>
    /// Advances the animation by <paramref name="deltaTime"/> seconds. Should be called every frame.
    /// </summary>
    public void Update(float deltaTime)
    {
        if (!IsPlaying)
        {
            return;
        }

        _subFrame += deltaTime;

        if (_subFrame < _frameDuration)
        {
            return;
        }

        _subFrame -= _frameDuration;

        if (!IsLooping && CurrentFrame == FrameCount - 1)
        {
            Stop();
            return;
        }

        CurrentFrame++;

        OnFrameChanged?.Invoke(CurrentFrame);
    }
}
