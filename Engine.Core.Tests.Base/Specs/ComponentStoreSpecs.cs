using Engine.Core.Abstractions;
using FluentAssertions;

namespace Engine.Core.Tests.Base.Specs;

public abstract class ComponentStoreSpecs<TStore> where TStore : IComponentStore
{
    protected abstract (TStore Store, Entity Entity) CreateContext();

    public struct Position { public float X, Y; }
    public struct Velocity { public float X, Y; }

    [Fact]
    public void Add_ShouldStoreComponent()
    {
        var (store, entity) = CreateContext();
        var pos = new Position { X = 10, Y = 20 };
        ref var added = ref store.Add(entity, pos);
        added.X.Should().Be(10);
        store.Has<Position>(entity).Should().BeTrue();
    }

    [Fact]
    public void Get_ShouldRetrieveStoredComponent()
    {
        var (store, entity) = CreateContext();
        store.Add(entity, new Position { X = 5, Y = 5 });
        ref var got = ref store.Get<Position>(entity);
        got.X.Should().Be(5);
    }

    [Fact]
    public void Get_ShouldModifyComponentByRef()
    {
        var (store, entity) = CreateContext();
        store.Add(entity, new Position { X = 1, Y = 1 });
        ref var got = ref store.Get<Position>(entity);
        got.X = 999;
        ref var check = ref store.Get<Position>(entity);
        check.X.Should().Be(999);
    }

    [Fact]
    public void Remove_ShouldRemoveComponent()
    {
        var (store, entity) = CreateContext();
        store.Add(entity, new Position { X = 1, Y = 1 });
        var removed = store.Remove<Position>(entity);
        removed.Should().BeTrue();
        store.Has<Position>(entity).Should().BeFalse();
    }

    [Fact]
    public void Remove_MissingComponent_ShouldReturnFalse()
    {
        var (store, entity) = CreateContext();
        var removed = store.Remove<Position>(entity);
        removed.Should().BeFalse();
    }

    [Fact]
    public void Get_MissingComponent_ShouldThrow()
    {
        var (store, entity) = CreateContext();
        var action = () => store.Get<Position>(entity);
        action.Should().Throw<KeyNotFoundException>();
    }
}