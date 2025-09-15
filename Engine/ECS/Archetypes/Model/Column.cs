using System.Runtime.InteropServices;

namespace Engine.ECS.Archetypes.Model;

public abstract class Column(int typeId) : IDisposable
{
    public readonly int TypeId = typeId;

    public abstract void MoveFrom(Column src, int srcIndex, int dstIndex);
    public abstract void SetDefault(int index);
    public abstract void Dispose();
}

public unsafe sealed class Column<T>(int typeId, int capacity) : Column(typeId) where T : unmanaged
{
    private T* _data;
    private bool _disposed;

    public Column() : this(0, 0)
    {
        _data = null;
    }

    public void Initialize(int cap)
    {
        if (_data != null) return;
        capacity = cap;
        _data = (T*)NativeMemory.Alloc((nuint)(capacity * sizeof(T)));
    }

    public ref T Ref(int index) => ref _data[index];

    public override void MoveFrom(Column src, int srcIndex, int dstIndex)
    {
        var s = (Column<T>)src;
        _data[dstIndex] = s._data[srcIndex];
    }

    public override void SetDefault(int index) => _data[index] = default;

    public override void Dispose()
    {
        if (_disposed) return;
        if (_data != null)
        {
            NativeMemory.Free(_data);
            _data = null;
        }
        _disposed = true;
        GC.SuppressFinalize(this);
    }

    ~Column() => Dispose();
}