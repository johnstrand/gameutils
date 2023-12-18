namespace GameUtils.Entity;
internal class Thinker(float interval)
{
    private float _thinkInterval = interval;

    public float UntilNextThink { get; private set; } = interval;

    public Action? OnThink { get; set; }

    public void Update(float deltaTime)
    {
        UntilNextThink -= deltaTime;

        if (UntilNextThink > 0)
        {
            return;
        }

        UntilNextThink += UntilNextThink;
        OnThink?.Invoke();
    }

    public void Reset()
    {
        UntilNextThink = _thinkInterval;
    }

    public void Reset(float nextThink)
    {
        UntilNextThink = nextThink;
        _thinkInterval = nextThink;
    }

    public void Reset(float nextThink, float thinkTime)
    {
        UntilNextThink = nextThink;
        _thinkInterval = thinkTime;
    }

    public void Wait(float time)
    {
        UntilNextThink += time;
    }
}
