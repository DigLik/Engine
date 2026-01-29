using Engine.Core.Abstractions;
using FluentAssertions;

namespace Engine.Core.Tests.Base.Specs;

public abstract class EntityAllocatorSpecs<TAllocator> where TAllocator : IEntityAllocator
{
    protected abstract TAllocator CreateAllocator();

    [Fact]
    public void Create_ShouldReturnAliveEntity()
    {
        var allocator = CreateAllocator();
        var entity = allocator.Create();
        entity.IsNone.Should().BeFalse();
        allocator.IsAlive(entity).Should().BeTrue();
    }

    [Fact]
    public void Destroy_ShouldMakeEntityDead()
    {
        var allocator = CreateAllocator();
        var entity = allocator.Create();
        allocator.Destroy(entity);
        allocator.IsAlive(entity).Should().BeFalse();
    }

    [Fact]
    public void Create_ShouldGenerateUniqueIdsOrGenerations()
    {
        var allocator = CreateAllocator();
        var e1 = allocator.Create();
        var e2 = allocator.Create();
        e1.Should().NotBe(e2);
        if (e1.Id == e2.Id)
            e1.Generation.Should().NotBe(e2.Generation);
    }
}