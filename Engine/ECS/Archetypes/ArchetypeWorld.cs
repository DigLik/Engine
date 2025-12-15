using Engine.Core.Services;
using Engine.DataStructures;
using Engine.ECS.Abstractions;
using Engine.ECS.Archetypes.Model;
using Engine.ECS.Archetypes.QueryDefinition;
using Engine.ECS.Components;
using System;
using System.Runtime.CompilerServices;

namespace Engine.ECS.Archetypes;

public sealed partial class ArchetypeWorld : IWorldApi, IDisposable
{
    private readonly ArchetypeRegistry _registry;
    private readonly HierarchyService _hierarchy;
    public IServiceRegistry Services { get; }

    private struct EntityRecord
    {
        public uint Generation;
        public Archetype? Archetype;
        public int ChunkIndex;
        public int Row;
    }

    private EntityRecord[] _entities = new EntityRecord[1024];
    private uint _nextEntityId;
    private readonly Stack<uint> _free = new();
    private readonly Archetype _emptyArchetype;

    public ArchetypeWorld(IServiceRegistry services, int chunkCapacity = 256, int initialEntityCapacity = 1024)
    {
        _entities = new EntityRecord[initialEntityCapacity];
        _registry = new ArchetypeRegistry(chunkCapacity);
        Services = services;
        _hierarchy = new HierarchyService(this, initialEntityCapacity);
        _emptyArchetype = _registry.GetOrCreate(new TypeMask(), []);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool IsAlive(Entity e)
        => e.Generation != 0 && (uint)e.Id < (uint)_entities.Length && _entities[e.Id].Generation == e.Generation;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private ref EntityRecord GetRecord(Entity e)
    {
        if (!IsAlive(e))
            throw new InvalidOperationException($"Entity {e} is not alive.");
        return ref _entities[e.Id];
    }

    public Entity CreateEntity()
    {
        uint id;
        if (_free.Count > 0)
        {
            id = _free.Pop();
        }
        else
        {
            id = ++_nextEntityId;
            if (id >= _entities.Length)
                Array.Resize(ref _entities, Math.Max(_entities.Length << 1, (int)id + 1));
        }

        ref var rec = ref _entities[id];
        rec.Generation = rec.Generation == 0 ? 1 : rec.Generation + 1;
        rec.Archetype = _emptyArchetype;
        rec.Row = _emptyArchetype.GetOrCreateWritableChunk(_registry).AddEntity(new Entity(id, rec.Generation));
        rec.ChunkIndex = _emptyArchetype.Chunks.Count - 1;

        return new Entity(id, rec.Generation);
    }

    public void DestroyEntity(Entity e)
    {
        if (!IsAlive(e)) return;

        var destructionQueue = new Queue<Entity>();
        destructionQueue.Enqueue(e);

        while (destructionQueue.Count > 0)
        {
            var currentEntity = destructionQueue.Dequeue();
            if (!IsAlive(currentEntity)) continue;

            foreach (var child in _hierarchy.GetChildren(currentEntity))
            {
                if (IsAlive(child) && TryGetRef<Parent>(child, out var parentInfo) && parentInfo.CascadeDelete)
                    destructionQueue.Enqueue(child);
            }

            DestroySingleEntity(currentEntity);
        }
    }

    private void DestroySingleEntity(Entity e)
    {
        ref var rec = ref _entities[e.Id];
        var oldArchetype = rec.Archetype;

        _hierarchy.OnEntityDestroyed(e);

        if (oldArchetype is not null)
        {
            var chunk = oldArchetype.Chunks[rec.ChunkIndex];
            int lastRow = chunk.RemoveAtSwapBack(rec.Row);

            if (rec.Row != lastRow)
            {
                Entity movedEntity = chunk.Entities[rec.Row];
                _entities[movedEntity.Id].Row = rec.Row;
            }
        }

        rec.Archetype = null;
        rec.Generation++;
        if (rec.Generation == 0) rec.Generation = 1;
        _free.Push(e.Id);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Has<T>(Entity e) where T : unmanaged
    {
        if (!IsAlive(e)) return false;
        ref var rec = ref _entities[e.Id];
        int id = _registry.GetTypeId<T>();
        return rec.Archetype!.Key.Mask.Contains(id);
    }

    public ref T Add<T>(Entity e) where T : unmanaged
    {
        ref var rec = ref GetRecord(e);
        int typeId = _registry.GetTypeId<T>();

        var currentArchetype = rec.Archetype!;
        if (currentArchetype.Key.Mask.Contains(typeId))
        {
            return ref GetRefInRecord<T>(rec);
        }

        if (!currentArchetype.AddTransitions.TryGetValue(typeId, out var targetArchetype))
        {
            targetArchetype = FindOrCreateTransitionArchetype(currentArchetype, typeId, true);
        }

        Migrate(e, ref rec, targetArchetype);
        return ref GetRefInRecord<T>(rec);
    }

    public ref T Add<T>(Entity e, in T value) where T : unmanaged
    {
        ref var r = ref Add<T>(e);
        r = value;
        return ref r;
    }

    public bool Remove<T>(Entity e) where T : unmanaged
    {
        if (!IsAlive(e)) return false;
        ref var rec = ref _entities[e.Id];
        int typeId = _registry.GetTypeId<T>();

        var currentArchetype = rec.Archetype!;
        if (!currentArchetype.Key.Mask.Contains(typeId))
        {
            return false;
        }

        if (!currentArchetype.RemoveTransitions.TryGetValue(typeId, out var targetArchetype))
        {
            targetArchetype = FindOrCreateTransitionArchetype(currentArchetype, typeId, false);
        }

        Migrate(e, ref rec, targetArchetype);
        return true;
    }

    private Archetype FindOrCreateTransitionArchetype(Archetype from, int typeId, bool isAdd)
    {
        var newMask = from.Key.Mask.Clone();
        if (isAdd)
            newMask.Add(typeId);
        else
            newMask.Remove(typeId);

        var targetArchetype = _registry.GetOrCreate(newMask, newMask.SetIds);

        if (isAdd)
            from.AddTransitions[typeId] = targetArchetype;
        else
            from.RemoveTransitions[typeId] = targetArchetype;

        return targetArchetype;
    }

    private void Migrate(Entity e, ref EntityRecord rec, Archetype targetArch)
    {
        var srcArch = rec.Archetype!;
        var srcChunk = srcArch.Chunks[rec.ChunkIndex];
        int srcRow = rec.Row;

        var dstChunk = targetArch.GetOrCreateWritableChunk(_registry);
        int dstRow = dstChunk.AddEntity(e);

        foreach (int tId in srcArch.Key.Mask.SetIds)
        {
            if (targetArch.TryGetColumnIndex(tId, out var dstColIdx))
            {
                srcArch.TryGetColumnIndex(tId, out var srcColIdx);
                dstChunk.Columns[dstColIdx].MoveFrom(srcChunk.Columns[srcColIdx], srcRow, dstRow);
            }
        }

        int lastRow = srcChunk.RemoveAtSwapBack(srcRow);
        if (srcRow != lastRow)
        {
            Entity movedE = srcChunk.Entities[srcRow];
            _entities[movedE.Id].Row = srcRow;
        }

        rec.Archetype = targetArch;
        rec.ChunkIndex = targetArch.Chunks.Count - 1;
        rec.Row = dstRow;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ref T Ref<T>(Entity e) where T : unmanaged
    {
        ref var rec = ref GetRecord(e);
        return ref GetRefInRecord<T>(rec);
    }

    public bool TryGetRef<T>(Entity e, out T component) where T : unmanaged
    {
        component = default;
        if (!IsAlive(e)) return false;

        ref var rec = ref _entities[e.Id];
        if (rec.Archetype is null) return false;

        int id = _registry.GetTypeId<T>();
        if (!rec.Archetype.Key.Mask.Contains(id)) return false;

        component = GetRefInRecord<T>(rec);
        return true;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private ref T GetRefInRecord<T>(EntityRecord rec) where T : unmanaged
    {
        var arch = rec.Archetype!;
        int typeId = _registry.GetTypeId<T>();
        if (!arch.TryGetColumnIndex(typeId, out var colIdx))
            throw new InvalidOperationException($"Component {typeof(T).Name} not found in archetype.");

        var chunk = arch.Chunks[rec.ChunkIndex];
        return ref ((Column<T>)chunk.Columns[colIdx]).Ref(rec.Row);
    }

    public QueryBuilder Builder() => new(_registry);

    public void SetParent(Entity child, Entity parent, bool cascadeDelete = true) => _hierarchy.SetParent(child, parent, cascadeDelete);
    public void RemoveParent(Entity child) => _hierarchy.RemoveParent(child);
    public Entity GetParent(Entity child) => _hierarchy.GetParent(child);
    public IReadOnlyList<Entity> GetChildren(Entity parent) => _hierarchy.GetChildren(parent);

    public int GetTypeId<T>() where T : unmanaged => TypeIndex.Get<T>();

    public void Dispose() => _registry.Dispose();
}