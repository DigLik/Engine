using Engine.DataStructures;
using Engine.ECS.Components;

namespace Engine.ECS;

internal sealed class HierarchyService(IWorldApi world)
{
    private readonly SparseSet<Entity> _childToParent = new();
    private readonly SparseSet<List<Entity>> _parentToChildren = new();
    private static readonly IReadOnlyList<Entity> EmptyChildrenList = [];

    public void SetParent(Entity child, Entity parent, bool cascadeDelete)
    {
        if (!world.IsAlive(child) || !world.IsAlive(parent))
            throw new InvalidOperationException("Both child and parent entities must be alive.");
        if (child == parent)
            throw new InvalidOperationException("Entity cannot be its own parent.");

        var current = parent;
        while (_childToParent.TryGetValue(current.Id, out var grandParent))
        {
            if (grandParent == child)
                throw new InvalidOperationException("Circular dependency detected: cannot set parent.");
            current = grandParent;
        }

        RemoveParent(child);

        world.Add(child, new Parent(parent, cascadeDelete));
        _childToParent.Add(child.Id, parent);

        if (!_parentToChildren.TryGetValue(parent.Id, out var children))
        {
            children = [];
            _parentToChildren.Add(parent.Id, children);
        }
        children.Add(child);
    }

    public void RemoveParent(Entity child)
    {
        if (!world.IsAlive(child) || !_childToParent.TryGetValue(child.Id, out var parent))
            return;

        world.Remove<Parent>(child);
        _childToParent.Remove(child.Id);

        if (_parentToChildren.TryGetValue(parent.Id, out var children))
        {
            children.Remove(child);
            if (children.Count == 0)
            {
                _parentToChildren.Remove(parent.Id);
            }
        }
    }

    public void Clear()
    {
        _childToParent.Clear();
        _parentToChildren.Clear();
    }

    public Entity GetParent(Entity child)
    {
        return _childToParent.TryGetValue(child.Id, out var parent) ? parent : Entity.Null;
    }

    public IReadOnlyList<Entity> GetChildren(Entity parent)
    {
        return _parentToChildren.TryGetValue(parent.Id, out var children) ? children : EmptyChildrenList;
    }

    public void OnEntityDestroyed(Entity destroyedEntity)
    {
        RemoveParent(destroyedEntity);
        if (_parentToChildren.TryGetValue(destroyedEntity.Id, out var children))
        {
            foreach (var child in children.ToArray())
            {
                if (world.IsAlive(child)) RemoveParent(child);
            }
        }
        _parentToChildren.Remove(destroyedEntity.Id);
    }
}