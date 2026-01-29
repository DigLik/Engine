using Engine.Core.Tests.Base.Specs;
using Engine.Kits.Default.Timing;

namespace Engine.Kits.Default.Tests.Timing;

public class TimeManagerTests : TimeProviderSpecs<TimeManager>
{
    protected override TimeManager CreateProvider() => new();
}