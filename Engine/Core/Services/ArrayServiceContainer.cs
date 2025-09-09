using Engine.DataStructures;

namespace Engine.Core.Services;

public sealed class ArrayServiceContainer(TypeIndex types) : IServiceRegistry
{
    private object?[] _slots = new object?[64];

    public void Register<T>(T service) where T : class
    {
        ArgumentNullException.ThrowIfNull(service);
        int id = types.Get<T>();
        if ((uint)id >= (uint)_slots.Length)
            Array.Resize(ref _slots, Math.Max(_slots.Length << 1, id + 1));
        _slots[id] = service;
    }

    public bool TryResolve<T>(out T? service) where T : class
    {
        int id = types.Get<T>();
        if ((uint)id < (uint)_slots.Length && _slots[id] is not null)
        {
            service = (T)_slots[id]!;
            return true;
        }
        service = null; return false;
    }

    public T Resolve<T>() where T : class
        => TryResolve<T>(out var s) ? s! : throw new InvalidOperationException($"Service {typeof(T).Name} not registered.");
}