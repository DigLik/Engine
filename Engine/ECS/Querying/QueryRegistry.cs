using Engine.DataStructures;
using Engine.ECS.Abstractions;
using Engine.ECS.Archetypes.QueryDefinition;

namespace Engine.ECS.Querying;

public sealed class QueryRegistry(IWorldApi world)
{
    private readonly IWorldApi _world = world;
    private readonly Dictionary<int, Query> _queryCache = [];

    internal Query GetOrCreateQuery(List<int>? withIds, List<int>? withoutIds, ReadOnlySpan<int> requiredTypeIds, bool isParallel)
    {
        var allWithIds = ListPool<int>.Rent(withIds);
        if (withIds != null) ListPool<int>.Return(withIds);

        try
        {
            foreach (var id in requiredTypeIds)
            {
                allWithIds.Add(id);
            }
            allWithIds.Sort();
            withoutIds?.Sort();

            int hashCode = 17;
            unchecked
            {
                foreach (var id in allWithIds) hashCode = hashCode * 31 + id;
                if (withoutIds != null) foreach (var id in withoutIds) hashCode = hashCode * 31 + id;
                hashCode = hashCode * 31 + (isParallel ? 1 : 0);
            }

            if (!_queryCache.TryGetValue(hashCode, out var query))
            {
                var qb = _world.Builder();
                if (allWithIds.Count > 0) qb.WithTypeIds.AddRange(allWithIds);
                if (withoutIds != null) qb.WithoutTypeIds.AddRange(withoutIds);

                query = qb.BuildInternal(requiredTypeIds, isParallel);
                _queryCache[hashCode] = query;
            }

            return query;
        }
        finally
        {
            ListPool<int>.Return(allWithIds);
            if (withoutIds != null) ListPool<int>.Return(withoutIds);
        }
    }

    public FluentQueryBuilder Query() => new(_world, this);
    public FluentQueryBuilder<T1> Query<T1>() where T1 : unmanaged => new FluentQueryBuilder(_world, this).With<T1>();
    public FluentQueryBuilder<T1, T2> Query<T1, T2>() where T1 : unmanaged where T2 : unmanaged => new FluentQueryBuilder(_world, this).With<T1, T2>();
    public FluentQueryBuilder<T1, T2, T3> Query<T1, T2, T3>() where T1 : unmanaged where T2 : unmanaged where T3 : unmanaged => new FluentQueryBuilder(_world, this).With<T1, T2, T3>();
    public FluentQueryBuilder<T1, T2, T3, T4> Query<T1, T2, T3, T4>() where T1 : unmanaged where T2 : unmanaged where T3 : unmanaged where T4 : unmanaged => new FluentQueryBuilder(_world, this).With<T1, T2, T3, T4>();
    public FluentQueryBuilder<T1, T2, T3, T4, T5> Query<T1, T2, T3, T4, T5>() where T1 : unmanaged where T2 : unmanaged where T3 : unmanaged where T4 : unmanaged where T5 : unmanaged => new FluentQueryBuilder(_world, this).With<T1, T2, T3, T4, T5>();
    public FluentQueryBuilder<T1, T2, T3, T4, T5, T6> Query<T1, T2, T3, T4, T5, T6>() where T1 : unmanaged where T2 : unmanaged where T3 : unmanaged where T4 : unmanaged where T5 : unmanaged where T6 : unmanaged => new FluentQueryBuilder(_world, this).With<T1, T2, T3, T4, T5, T6>();
    public FluentQueryBuilder<T1, T2, T3, T4, T5, T6, T7> Query<T1, T2, T3, T4, T5, T6, T7>() where T1 : unmanaged where T2 : unmanaged where T3 : unmanaged where T4 : unmanaged where T5 : unmanaged where T6 : unmanaged where T7 : unmanaged => new FluentQueryBuilder(_world, this).With<T1, T2, T3, T4, T5, T6, T7>();
    public FluentQueryBuilder<T1, T2, T3, T4, T5, T6, T7, T8> Query<T1, T2, T3, T4, T5, T6, T7, T8>() where T1 : unmanaged where T2 : unmanaged where T3 : unmanaged where T4 : unmanaged where T5 : unmanaged where T6 : unmanaged where T7 : unmanaged where T8 : unmanaged => new FluentQueryBuilder(_world, this).With<T1, T2, T3, T4, T5, T6, T7, T8>();
}