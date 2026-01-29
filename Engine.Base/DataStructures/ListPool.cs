using System.Collections.Concurrent;

namespace Engine.Base.DataStructures;

public static class ListPool<T>
{
    private static readonly ConcurrentBag<List<T>> _pool = [];

    public static List<T> Rent()
    {
        if (_pool.TryTake(out var list))
            return list;
        return [];
    }

    public static void Return(List<T> list)
    {
        list.Clear();
        _pool.Add(list);
    }
}