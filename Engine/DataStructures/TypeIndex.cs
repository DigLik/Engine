using System.Runtime.CompilerServices;

namespace Engine.DataStructures;

internal static class ComponentTypeCache<T>
{
    public static int Id = -1;
}

public static class TypeIndex
{
    private static int _nextId = 0;
    private static readonly Lock _lock = new();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Get<T>()
    {
        if (ComponentTypeCache<T>.Id != -1) return ComponentTypeCache<T>.Id;
        return Register<T>();
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static int Register<T>()
    {
        lock (_lock)
        {
            if (ComponentTypeCache<T>.Id != -1) return ComponentTypeCache<T>.Id;

            int id = ++_nextId;
            ComponentTypeCache<T>.Id = id;
            return id;
        }
    }
}