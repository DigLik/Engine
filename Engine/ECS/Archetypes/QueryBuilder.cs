namespace Engine.ECS.Archetypes;

public partial class QueryBuilder
{
    private readonly ArchetypeRegistry _registry;

    public readonly List<int> WithTypeIds = [];
    public readonly List<int> WithoutTypeIds = [];

    internal QueryBuilder(ArchetypeRegistry registry)
    {
        _registry = registry;
    }

    public QueryBuilder With<T>() where T : unmanaged
    {
        WithTypeIds.Add(_registry.GetTypeId<T>());
        return this;
    }

    public QueryBuilder Without<T>() where T : unmanaged
    {
        WithoutTypeIds.Add(_registry.GetTypeId<T>());
        return this;
    }

    internal Query BuildInternal(ReadOnlySpan<int> required)
    {
        var withMask = new TypeMask();
        foreach (var id in WithTypeIds) withMask.Add(id);
        foreach (var id in required) withMask.Add(id);

        var withoutMask = new TypeMask();
        foreach (var id in WithoutTypeIds) withoutMask.Add(id);

        var desc = new QueryDescription(withMask, withoutMask);
        return new Query(_registry, desc, required);
    }
}