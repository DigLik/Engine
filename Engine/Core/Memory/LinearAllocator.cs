using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Engine.Core.Memory;

public sealed unsafe class LinearAllocator : IDisposable
{
    private readonly void* _startPointer;
    private readonly long _sizeInBytes;
    private long _currentOffset;
    private bool _disposed;

    public LinearAllocator(long sizeInBytes)
    {
        if (sizeInBytes <= 0)
            throw new ArgumentOutOfRangeException(nameof(sizeInBytes), "Size must be positive.");

        _sizeInBytes = sizeInBytes;
        _startPointer = NativeMemory.Alloc((nuint)sizeInBytes);
        _currentOffset = 0;
        _disposed = false;
    }

    public ref T Allocate<T>() where T : unmanaged
    {
        int size = sizeof(T);
        long alignedOffset = Align(_currentOffset, sizeof(T));

        if (alignedOffset + size > _sizeInBytes)
            throw new OutOfMemoryException($"LinearAllocator out of memory. Requested: {size} bytes, Available: {_sizeInBytes - _currentOffset} bytes.");

        _currentOffset = alignedOffset + size;

        void* ptr = (byte*)_startPointer + alignedOffset;

        return ref Unsafe.AsRef<T>(ptr);
    }

    public Span<T> Allocate<T>(int count) where T : unmanaged
    {
        if (count <= 0) return [];

        int sizeOfT = sizeof(T);
        long totalSize = (long)count * sizeOfT;
        long alignedOffset = Align(_currentOffset, sizeOfT);

        if (alignedOffset + totalSize > _sizeInBytes)
            throw new OutOfMemoryException($"LinearAllocator out of memory. Requested: {totalSize} bytes, Available: {_sizeInBytes - _currentOffset} bytes.");

        _currentOffset = alignedOffset + totalSize;

        void* ptr = (byte*)_startPointer + alignedOffset;

        return new Span<T>(ptr, count);
    }

    public void Reset() => _currentOffset = 0;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static long Align(long value, int alignment)
    {
        return (value + alignment - 1) & ~((long)alignment - 1);
    }

    public void Dispose()
    {
        if (_disposed) return;

        if (_startPointer != null)
            NativeMemory.Free(_startPointer);

        _disposed = true;
        GC.SuppressFinalize(this);
    }

    ~LinearAllocator() => Dispose();
}