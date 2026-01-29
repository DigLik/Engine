using Engine.Core.Tests.Base.Specs;
using Engine.Kits.Default.ECS;

namespace Engine.Kits.Default.Tests.ECS;

public class DefaultWorldAllocatorTests : EntityAllocatorSpecs<DefaultWorld>
{
    protected override DefaultWorld CreateAllocator() => new();
}