using Engine.Base.DataStructures;
using System.Runtime.CompilerServices;

namespace Engine.Kits.Default.Storage;

internal sealed class SparseSetStorage<T>(int capacity) : IComponentStorage
{
    public readonly SparseSet<T> Backend = new(capacity);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Remove(uint entityId) => Backend.Remove(entityId);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Has(uint entityId) => Backend.Contains(entityId);
}