using Engine.ECS.Abstractions;

namespace Engine.ECS;

public sealed class CommandBuffer
{
    private readonly List<Entity> _destroyQueue = [];
    private readonly List<Action<IWorldApi>> _actions = [];

    public void DestroyEntity(Entity entity) => _destroyQueue.Add(entity);

    public void AddComponent<T>(Entity entity, T component = default) where T : unmanaged
        => _actions.Add(w => w.Add(entity, component));

    public void Playback(IWorldApi world)
    {
        foreach (var e in _destroyQueue) world.DestroyEntity(e);
        _destroyQueue.Clear();

        foreach (var act in _actions) act(world);
        _actions.Clear();
    }
}