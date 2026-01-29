using Engine.Kits.Default.ECS;
using FluentAssertions;

namespace Engine.Kits.Default.Tests.ECS;

public class DefaultWorldIntegrationTests
{
    struct Data { public int Value; }

    [Fact]
    public void Destroy_Entity_ShouldRemoveComponents()
    {
        var world = new DefaultWorld();
        var entity = world.Create();
        world.Add(entity, new Data { Value = 123 });
        world.Has<Data>(entity).Should().BeTrue();
        world.Destroy(entity);
        world.Has<Data>(entity).Should().BeFalse();
    }

    [Fact]
    public void ReusedEntity_ShouldNotHaveOldComponents()
    {
        var world = new DefaultWorld();
        var entity1 = world.Create();
        world.Add(entity1, new Data { Value = 100 });

        world.Destroy(entity1);
        var entity2 = world.Create();

        if (entity1.Id == entity2.Id)
            world.Has<Data>(entity2).Should().BeFalse();
    }
}