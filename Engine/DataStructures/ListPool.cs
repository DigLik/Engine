using System.Collections.Concurrent;

namespace Engine.DataStructures;

internal static class ListPool<T>
{
    private static readonly ConcurrentBag<List<T>> _pool = [];

    public static List<T> Rent()
    {
        if (_pool.TryTake(out var list))
        {
            return list;
        }
        return [];
    }

    public static List<T> Rent(List<T>? initialValues)
    {
        var list = Rent();
        if (initialValues != null)
        {
            list.AddRange(initialValues);
        }
        return list;
    }

    public static void Return(List<T> list)
    {
        list.Clear();
        _pool.Add(list);
    }
}

public readonly ref struct PooledList<T>(List<T>? list)
{
    private readonly List<T>? _list = list;
    public void Dispose()
    {
        if (_list != null)
        {
            ListPool<T>.Return(_list);
        }
    }
}