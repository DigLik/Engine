namespace Engine.Core.Timing;

public readonly record struct TimeSnapshot(
    float DeltaTime,
    float TotalTime,
    float UnscaledDeltaTime,
    float UnscaledTotalTime,
    float TimeScale,
    bool IsRunning
)
{
    public static TimeSnapshot FromTime(Time time)
        => new(
            time.DeltaTime,
            time.TotalTime,
            time.UnscaledDeltaTime,
            time.UnscaledTotalTime,
            time.TimeScale,
            time.IsRunning
        );
}