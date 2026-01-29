namespace Engine.Core.Abstractions;

public interface IComponentStore
{
    ref T Add<T>(Entity entity, in T component) where T : unmanaged;

    ref T Get<T>(Entity entity) where T : unmanaged;

    bool Has<T>(Entity entity) where T : unmanaged;

    bool Remove<T>(Entity entity) where T : unmanaged;
}