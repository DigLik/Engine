using Engine.Core.Abstractions;
using Engine.Base.DataStructures;
using System.Runtime.CompilerServices;

namespace Engine.Kits.Default.Services;

public sealed class ArrayServiceContainer : IServiceRegistry
{
    private object?[] _slots = new object?[64];

    public void Register<T>(T service) where T : class
    {
        int id = TypeId<T>.Value;
        if ((uint)id >= (uint)_slots.Length)
            Array.Resize(ref _slots, Math.Max(_slots.Length << 1, id + 1));
        _slots[id] = service;
    }

    public bool TryResolve<T>(out T? service) where T : class
    {
        int id = TypeId<T>.Value;
        if ((uint)id < (uint)_slots.Length)
        {
            object? s = _slots[id];
            if (s is not null)
            {
                service = Unsafe.As<object, T>(ref s);
                return true;
            }
        }
        service = null; return false;
    }

    public T Resolve<T>() where T : class
    {
        if (TryResolve<T>(out var service))
            return service!;

        throw new InvalidOperationException($"Service of type {typeof(T).Name} is not registered.");
    }
}