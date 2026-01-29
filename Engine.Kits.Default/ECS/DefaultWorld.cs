using Engine.Base.DataStructures;
using Engine.Core;
using Engine.ECS.Abstractions;
using Engine.Kits.Default.Storage;
using System.Runtime.CompilerServices;

namespace Engine.Kits.Default.ECS;

public sealed class DefaultWorld(int initialCapacity = 1024) : IWorld
{
    private struct EntityMeta
    {
        public uint Generation;
        public bool IsAlive;
    }

    private EntityMeta[] _entities = new EntityMeta[initialCapacity];
    private readonly Queue<uint> _freeIds = new();
    private uint _nextId = 1;
    private IComponentStorage?[] _storages = new IComponentStorage?[64];

    #region IEntityAllocator

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Entity Create()
    {
        uint id;
        if (_freeIds.TryDequeue(out var freeId))
        {
            id = freeId;
        }
        else
        {
            id = _nextId++;
            EnsureEntityCapacity(id);
        }

        ref var meta = ref _entities[id];
        meta.IsAlive = true;

        return new Entity(id, meta.Generation);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Destroy(Entity entity)
    {
        if (entity.Id >= _entities.Length)
            return;

        ref var meta = ref _entities[entity.Id];

        if (!meta.IsAlive || meta.Generation != entity.Generation)
            return;

        meta.IsAlive = false;
        meta.Generation++;
        _freeIds.Enqueue(entity.Id);

        var storageSpan = _storages.AsSpan();
        for (int i = 0; i < storageSpan.Length; i++)
            storageSpan[i]?.Remove(entity.Id);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool IsAlive(Entity entity)
    {
        if (entity.Id == 0 || entity.Id >= _entities.Length)
            return false;
        ref var meta = ref _entities[entity.Id];
        return meta.IsAlive && meta.Generation == entity.Generation;
    }

    private void EnsureEntityCapacity(uint id)
    {
        if (id >= _entities.Length)
        {
            var newSize = Math.Max(id + 1, (uint)_entities.Length * 2);
            Array.Resize(ref _entities, (int)newSize);
        }
    }

    #endregion

    #region IComponentStore

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private SparseSetStorage<T> GetStorage<T>(bool createIfMissing) where T : unmanaged
    {
        var typeId = TypeId<T>.Value;

        if (typeId >= _storages.Length)
        {
            if (!createIfMissing)
                ThrowKeyNotFound<T>();
            Array.Resize(ref _storages, Math.Max(typeId + 1, _storages.Length * 2));
        }

        ref var storageRef = ref _storages[typeId];

        if (storageRef == null)
        {
            if (!createIfMissing)
                ThrowKeyNotFound<T>();
            storageRef = new SparseSetStorage<T>(64);
        }

        return Unsafe.As<IComponentStorage, SparseSetStorage<T>>(ref storageRef);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ref T Add<T>(Entity entity, in T component) where T : unmanaged
    {
        var storage = GetStorage<T>(true);
        storage.Backend.Add(entity.Id, component);
        return ref storage.Backend.GetRef(entity.Id);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ref T Get<T>(Entity entity) where T : unmanaged
    {
        var storage = GetStorage<T>(false);
        return ref storage.Backend.GetRef(entity.Id);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Has<T>(Entity entity) where T : unmanaged
    {
        var typeId = TypeId<T>.Value;
        if (typeId >= _storages.Length)
            return false;

        var storage = _storages[typeId];
        return storage != null && storage.Has(entity.Id);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Remove<T>(Entity entity) where T : unmanaged
    {
        var typeId = TypeId<T>.Value;
        if (typeId >= _storages.Length)
            return false;

        var storage = _storages[typeId];
        return storage != null && storage.Remove(entity.Id);
    }

    private static void ThrowKeyNotFound<T>()
        => throw new KeyNotFoundException($"Component storage for {typeof(T).Name} not found.");

    #endregion
}