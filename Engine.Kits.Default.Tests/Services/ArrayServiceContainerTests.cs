using Engine.Core.Tests.Base.Specs;
using Engine.Kits.Default.Services;

namespace Engine.Kits.Default.Tests.Services;

public class ArrayServiceContainerTests : ServiceRegistrySpecs<ArrayServiceContainer>
{
    protected override ArrayServiceContainer CreateRegistry() => new();
}