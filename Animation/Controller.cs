namespace GameUtils.Animation;
internal class Controller(int frameCount, bool isLooping = true, float framesPerSecond = 30)
{
    public int CurrentFrame { get; private set; }
    public int FrameCount { get; set; } = frameCount;
    public bool IsPlaying { get; private set; }
    public bool IsLooping { get; set; } = isLooping;
    public float FramesPerSecond
    {
        get => framesPerSecond;
        set
        {
            framesPerSecond = value;
            _frameDuration = 1 / value;
        }
    }

    public Action<int>? OnFrameChanged { get; set; }
    public Action? OnStopped { get; set; }

    private float _subFrame;
    private float _frameDuration = 1 / framesPerSecond;

    public void Play()
    {
        IsPlaying = true;
    }

    public void Stop()
    {
        IsPlaying = false;
        CurrentFrame = 0;
        _subFrame = 0;
        OnStopped?.Invoke();
    }

    public void Pause()
    {
        IsPlaying = false;
    }

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
            IsPlaying = false;
            return;
        }

        CurrentFrame++;

        OnFrameChanged?.Invoke(CurrentFrame);
    }
}
