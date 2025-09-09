namespace Engine.Core;

public class Time
{
    public float DeltaTime { get; private set; }
    public float TotalTime { get; private set; }
    public float UnscaledDeltaTime { get; private set; }
    public float UnscaledTotalTime { get; private set; }

    private float _timeScale = 1f;
    public float TimeScale
    {
        get => _timeScale;
        set => _timeScale = Math.Clamp(value, 0f, 100f);
    }

    public bool IsRunning { get; private set; } = true;

    public Time() { }

    public Time(float deltaTime, float totalTime)
    {
        DeltaTime = deltaTime;
        TotalTime = totalTime;
        UnscaledDeltaTime = deltaTime;
        UnscaledTotalTime = totalTime;
    }

    public void Update(float deltaTime)
    {
        UnscaledDeltaTime = deltaTime;
        UnscaledTotalTime += deltaTime;

        if (!IsRunning) return;

        DeltaTime = deltaTime * _timeScale;
        TotalTime += DeltaTime;
    }

    public void TickUnscaled(float deltaTime)
    {
        UnscaledDeltaTime = deltaTime;
        UnscaledTotalTime += deltaTime;
        DeltaTime = deltaTime * _timeScale;
    }

    public void Pause() => IsRunning = false;

    public void Resume() => IsRunning = true;

    public void Reset()
    {
        DeltaTime = 0;
        TotalTime = 0;
        UnscaledDeltaTime = 0;
        UnscaledTotalTime = 0;
        _timeScale = 1;
        IsRunning = true;
    }
}