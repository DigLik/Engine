using Engine.ECS.Abstractions;
using Engine.ECS.Querying;
using System.ComponentModel;

namespace Engine.ECS;

[EditorBrowsable(EditorBrowsableState.Never)]
public readonly partial struct FluentQueryBuilder
{
    internal readonly IWorldApi World;
    internal readonly QueryRegistry Registry;
    internal readonly List<int>? WithIds;
    internal readonly List<int>? WithoutIds;
    internal readonly bool IsParallel;

    internal FluentQueryBuilder(IWorldApi world, QueryRegistry registry)
    {
        World = world;
        Registry = registry;
        WithIds = null;
        WithoutIds = null;
        IsParallel = false;
    }

    internal FluentQueryBuilder(IWorldApi world, QueryRegistry registry, List<int>? withIds, List<int>? withoutIds, bool isParallel = false)
    {
        World = world;
        Registry = registry;
        WithIds = withIds;
        WithoutIds = withoutIds;
        IsParallel = isParallel;
    }

    public FluentQueryBuilder AsParallel()
    {
        return new FluentQueryBuilder(World, Registry, WithIds, WithoutIds, true);
    }
}
