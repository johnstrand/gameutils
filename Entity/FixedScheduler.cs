namespace GameUtils.Entity;

public abstract class FixedScheduler
{
    private int targetFps;
    private float targetFrameTime;
    private bool isRunning;
    private Task? runningTask;

    public FixedScheduler(int targetFps)
    {
        this.targetFps = targetFps;
        targetFrameTime = 1f / targetFps;
    }

    public void Start()
    {
        runningTask = Task.Run(() =>
        {
            isRunning = true;
            var frameCount = 0;
            var startTime = DateTime.Now;
            var expected = 0;
            while (isRunning)
            {
                var frameStart = DateTime.Now;
                Update();
                while((DateTime.Now - frameStart).TotalSeconds < targetFrameTime)
                {
                    Thread.Yield();
                }
                if (frameCount % targetFps == 0)
                {
                    expected++;
                    var drift = (DateTime.Now - startTime).TotalSeconds - expected;
                    if (drift != 0)
                    {
                        targetFrameTime -= (float)drift / 120f;
                    }
                }
            }
        });
    }

    public async void Stop()
    {
        if(!isRunning || runningTask == null)
        {
            return;
        }

        isRunning = false;
        await runningTask;
        runningTask = null;
    }

    public abstract void Update();
}