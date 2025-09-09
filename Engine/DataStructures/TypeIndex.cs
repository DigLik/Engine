namespace Engine.DataStructures;

public sealed class TypeIndex
{
    private readonly Dictionary<Type, int> _ids = new(128);
    private int _next = 0;

    public int Get<T>() => Get(typeof(T));

    public int Get(Type t)
    {
        if (_ids.TryGetValue(t, out var id)) return id;
        id = ++_next;
        _ids[t] = id;
        return id;
    }
}