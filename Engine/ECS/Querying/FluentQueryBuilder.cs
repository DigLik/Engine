using Engine.ECS.Abstractions;
using Engine.ECS.Archetypes.QueryDefinition;
using System.ComponentModel;

namespace Engine.ECS;

[EditorBrowsable(EditorBrowsableState.Never)]
public readonly partial struct FluentQueryBuilder
{
    internal readonly IWorldApi World;
    internal readonly Dictionary<int, Query> QueryCache;
    internal readonly List<int>? WithIds;
    internal readonly List<int>? WithoutIds;

    internal FluentQueryBuilder(IWorldApi world, Dictionary<int, Query> queryCache)
    {
        World = world;
        QueryCache = queryCache;
        WithIds = null;
        WithoutIds = null;
    }

    internal FluentQueryBuilder(IWorldApi world, Dictionary<int, Query> queryCache, List<int>? withIds, List<int>? withoutIds)
    {
        World = world;
        QueryCache = queryCache;
        WithIds = withIds;
        WithoutIds = withoutIds;
    }
}
