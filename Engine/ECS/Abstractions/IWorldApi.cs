using Engine.ECS.Archetypes.QueryDefinition;

namespace Engine.ECS.Abstractions;

public partial interface IWorldApi
{
    Entity CreateEntity();
    void DestroyEntity(Entity e);

    ref T Add<T>(Entity e) where T : unmanaged;
    ref T Add<T>(Entity e, in T value) where T : unmanaged;

    bool Remove<T>(Entity e) where T : unmanaged;

    bool Has<T>(Entity e) where T : unmanaged;
    ref T Ref<T>(Entity e) where T : unmanaged;

    bool TryGetRef<T>(Entity e, out T component) where T : unmanaged;
    bool IsAlive(Entity e);

    QueryBuilder Builder();

    void SetParent(Entity child, Entity parent, bool cascadeDelete = true);
    void RemoveParent(Entity child);

    Entity GetParent(Entity child);
    IReadOnlyList<Entity> GetChildren(Entity parent);

    int GetTypeId<T>() where T : unmanaged;
}