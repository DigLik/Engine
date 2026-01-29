using Engine.Core.Timing;

namespace Engine.Core.Abstractions;

public interface ITimeProvider
{
    TimeSpan FixedStep { get; }
    TimeSpan MaxDeltaTime { get; }

    TimeSnapshot GetSnapshot();
    void Tick(TimeSpan rawDelta);
    void Reset();
}
