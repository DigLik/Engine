namespace Engine.Core.Timing;

public readonly record struct TimeSnapshot(
    TimeSpan DeltaTime,
    TimeSpan TotalTime,
    double TimeScale,
    long FrameCount
);