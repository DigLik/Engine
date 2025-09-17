namespace Engine.Core.Memory;

public unsafe interface ILinearAllocator : IDisposable
{
    void Reset();

    ref T Allocate<T>() where T : unmanaged;

    Span<T> Allocate<T>(int count) where T : unmanaged;
}