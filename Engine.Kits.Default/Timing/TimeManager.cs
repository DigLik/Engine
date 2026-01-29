using Engine.Core.Abstractions;
using Engine.Core.Timing;

namespace Engine.Kits.Default.Timing;

public sealed class TimeManager : ITimeProvider
{
    public double TimeScale { get; set; } = 1.0;
    public TimeSpan FixedStep { get; set; } = TimeSpan.FromSeconds(0.01);
    public TimeSpan MaxDeltaTime { get; set; } = TimeSpan.FromSeconds(0.5);

    private TimeSpan _totalTime;
    private long _frameCount;

    public TimeSnapshot GetSnapshot() => new(
        DeltaTime: TimeSpan.Zero,
        TotalTime: _totalTime,
        TimeScale: TimeScale,
        FrameCount: _frameCount
    );

    public void Tick(TimeSpan rawDelta)
    {
        if (rawDelta > MaxDeltaTime)
            rawDelta = MaxDeltaTime;
        _totalTime += rawDelta * TimeScale;
        _frameCount++;
    }

    public void Reset()
    {
        _totalTime = TimeSpan.Zero;
        _frameCount = 0;
    }
}