using BepuPhysics;
using Engine.DataStructures;
using Engine.ECS;

namespace Engine.ECS.Components.Physics;

public sealed class PhysicsMap
{
    private readonly SparseSet<Entity> _bodyHandleToEntity = [];

    public void MapBody(BodyHandle handle, Entity entity)
        => _bodyHandleToEntity[(uint)handle.Value] = entity;

    public void UnmapBody(BodyHandle handle)
        => _bodyHandleToEntity.Remove((uint)handle.Value);

    public bool TryGetEntity(BodyHandle handle, out Entity entity)
        => _bodyHandleToEntity.TryGetValue((uint)handle.Value, out entity);
}