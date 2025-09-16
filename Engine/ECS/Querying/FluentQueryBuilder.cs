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

    internal FluentQueryBuilder(IWorldApi world, QueryRegistry registry)
    {
        World = world;
        Registry = registry;
        WithIds = null;
        WithoutIds = null;
    }

    internal FluentQueryBuilder(IWorldApi world, QueryRegistry registry, List<int>? withIds, List<int>? withoutIds)
    {
        World = world;
        Registry = registry;
        WithIds = withIds;
        WithoutIds = withoutIds;
    }
}
