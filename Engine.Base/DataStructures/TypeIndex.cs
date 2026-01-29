namespace Engine.Base.DataStructures;

public static class TypeId<T>
{
    public static readonly int Value = TypeIndex.NextId();
}

file static class TypeIndex
{
    private static int _nextId = -1;

    internal static int NextId()
        => Interlocked.Increment(ref _nextId);
}