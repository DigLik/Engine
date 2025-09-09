using Engine.Core.Services;
using Engine.DataStructures;
using Engine.ECS.Components;

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

    private EntityRecord[] _entities = new EntityRecord[8192];
    private uint _nextEntityId;
    private readonly Stack<uint> _free = new();

    public ArchetypeWorld(TypeIndex types, IServiceRegistry services, int chunkCapacity = 256)
    {
        _registry = new ArchetypeRegistry(types, chunkCapacity);
        Services = services;
        _hierarchy = new HierarchyService(this);
    }

    public bool IsAlive(Entity e)
        => e.Generation != 0 && (uint)e.Id < (uint)_entities.Length && _entities[e.Id].Generation == e.Generation;

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
        if (rec.Generation == 0)
        {
            rec.Generation = 1;
        }
        rec.Archetype = null;

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
        if (!IsAlive(e)) return;

        ref var rec = ref _entities[e.Id];
        var oldArchetype = rec.Archetype;
        var oldRow = rec.Row;
        var oldChunkIndex = rec.ChunkIndex;

        _hierarchy.OnEntityDestroyed(e);

        if (oldArchetype is not null)
        {
            var chunk = oldArchetype.Chunks[oldChunkIndex];
            int lastRow = chunk.RemoveAtSwapBack(oldRow);

            if (oldRow != lastRow)
            {
                Entity movedEntity = chunk.Entities[oldRow];
                _entities[movedEntity.Id].Row = oldRow;
            }
        }

        rec.Archetype = null;
        rec.Generation++;
        if (rec.Generation == 0)
        {
            rec.Generation = 1;
        }
        _free.Push(e.Id);
    }

    public bool Has<T>(Entity e) where T : unmanaged
    {
        if (!IsAlive(e)) return false;
        ref var rec = ref _entities[e.Id];
        if (rec.Archetype is null) return false;

        int id = _registry.GetTypeId<T>();
        return rec.Archetype.Key.Mask.Contains(id);
    }

    public ref T Add<T>(Entity e) where T : unmanaged
    {
        ref var rec = ref EnsureEntity(e);
        int id = _registry.GetTypeId<T>();
        var currentMask = rec.Archetype?.Key.Mask;

        var newMask = currentMask?.Clone() ?? new TypeMask();
        newMask.Add(id);

        Migrate(e, ref rec, newMask);
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
        if (rec.Archetype is null) return false;

        int id = _registry.GetTypeId<T>();
        if (!rec.Archetype.Key.Mask.Contains(id)) return false;

        var newMask = rec.Archetype.Key.Mask.Clone();
        newMask.Remove(id);

        Migrate(e, ref rec, newMask);
        return true;
    }

    public ref T Ref<T>(Entity e) where T : unmanaged
    {
        ref var rec = ref GetRecord(e);
        return ref GetRefInRecord<T>(rec);
    }

    public bool TryGetRef<T>(Entity e, out T component) where T : unmanaged
    {
        if (IsAlive(e))
        {
            ref var rec = ref _entities[e.Id];
            if (rec.Archetype is not null)
            {
                int id = _registry.GetTypeId<T>();
                if (rec.Archetype.Key.Mask.Contains(id))
                {
                    component = GetRefInRecord<T>(rec);
                    return true;
                }
            }
        }
        component = default;
        return false;
    }

    private ref EntityRecord EnsureEntity(Entity e)
    {
        if (!IsAlive(e))
        {
            if (e.Id < _entities.Length && _entities[e.Id].Generation != e.Generation)
                throw new InvalidOperationException($"Entity {e} is not alive (stale generation).");

            if (e.Id >= _entities.Length) Array.Resize(ref _entities, Math.Max(_entities.Length << 1, (int)e.Id + 1));

            ref var rec = ref _entities[e.Id];
            if (rec.Generation != e.Generation && rec.Archetype != null)
                throw new InvalidOperationException($"Cannot add component to stale entity {e}.");
        }
        return ref _entities[e.Id];
    }

    private void Migrate(Entity e, ref EntityRecord rec, TypeMask newMask)
    {
        var typeIdsSpan = newMask.SetIds;
        var targetArch = _registry.GetOrCreate(newMask, typeIdsSpan);

        var chunk = targetArch.GetOrCreateWritableChunk(_registry);
        int newRow = chunk.AddEntity(e);

        if (rec.Archetype is not null)
        {
            var srcArch = rec.Archetype;
            var srcChunk = srcArch.Chunks[rec.ChunkIndex];
            int srcRow = rec.Row;

            foreach (int tId in srcArch.Key.Mask.SetIds)
            {
                if (targetArch.TryGetColumnIndex(tId, out var dstColIdx))
                {
                    srcArch.TryGetColumnIndex(tId, out var srcColIdx);
                    chunk.Columns[dstColIdx].MoveFrom(srcChunk.Columns[srcColIdx], srcRow, newRow);
                }
            }

            int lastRow = srcChunk.RemoveAtSwapBack(srcRow);
            if (srcRow != lastRow)
            {
                Entity movedE = srcChunk.Entities[srcRow];
                _entities[movedE.Id].Row = srcRow;
            }
        }

        rec.Archetype = targetArch;
        rec.ChunkIndex = targetArch.Chunks.Count - 1;
        rec.Row = newRow;
    }

    private ref T GetRefInRecord<T>(EntityRecord rec) where T : unmanaged
    {
        var arch = rec.Archetype ?? throw new InvalidOperationException("Entity has no archetype.");
        int typeId = _registry.GetTypeId<T>();
        if (!arch.TryGetColumnIndex(typeId, out var colIdx))
            throw new InvalidOperationException("Inconsistency in archetype data: column not found.");

        var chunk = arch.Chunks[rec.ChunkIndex];
        return ref ((Column<T>)chunk.Columns[colIdx]).Ref(rec.Row);
    }

    public QueryBuilder Builder() => new(_registry);

    public void SetParent(Entity child, Entity parent, bool cascadeDelete = true) => _hierarchy.SetParent(child, parent, cascadeDelete);
    public void RemoveParent(Entity child) => _hierarchy.RemoveParent(child);
    public Entity GetParent(Entity child) => _hierarchy.GetParent(child);
    public IReadOnlyList<Entity> GetChildren(Entity parent) => _hierarchy.GetChildren(parent);

    public int GetTypeId<T>() where T : unmanaged => _registry.GetTypeId<T>();

    public void Dispose() => _registry.Dispose();
}