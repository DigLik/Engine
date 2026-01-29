using Engine.Core;
using Engine.Core.Tests.Base.Specs;
using Engine.Kits.Default.ECS;

namespace Engine.Kits.Default.Tests.ECS;

public class DefaultWorldComponentTests : ComponentStoreSpecs<DefaultWorld>
{
    protected override (DefaultWorld Store, Entity Entity) CreateContext()
    {
        var world = new DefaultWorld();
        var entity = world.Create();
        return (world, entity);
    }
}