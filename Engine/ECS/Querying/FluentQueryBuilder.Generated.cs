using Engine.ECS.Archetypes;
using Engine.ECS.Archetypes.QueryDefinition;

namespace Engine.ECS;

public readonly partial struct FluentQueryBuilder
{
    public FluentQueryBuilder<T1> With<T1>() where T1 : unmanaged
    {
        var newWith = WithIds is null ? [] : new List<int>(WithIds);
        newWith.AddRange([World.GetTypeId<T1>()]);
        return new FluentQueryBuilder<T1>(new FluentQueryBuilder(World, QueryCache, newWith, WithoutIds));
    }
    public FluentQueryBuilder Without<T1>() where T1 : unmanaged
    {
        var newWithout = WithoutIds is null ? [] : new List<int>(WithoutIds);
        newWithout.AddRange([World.GetTypeId<T1>()]);
        return new FluentQueryBuilder(World, QueryCache, WithIds, newWithout);
    }
    public FluentQueryBuilder<T1, T2> With<T1, T2>() where T1 : unmanaged where T2 : unmanaged
    {
        var newWith = WithIds is null ? [] : new List<int>(WithIds);
        newWith.AddRange([World.GetTypeId<T1>(), World.GetTypeId<T2>()]);
        return new FluentQueryBuilder<T1, T2>(new FluentQueryBuilder(World, QueryCache, newWith, WithoutIds));
    }
    public FluentQueryBuilder Without<T1, T2>() where T1 : unmanaged where T2 : unmanaged
    {
        var newWithout = WithoutIds is null ? [] : new List<int>(WithoutIds);
        newWithout.AddRange([World.GetTypeId<T1>(), World.GetTypeId<T2>()]);
        return new FluentQueryBuilder(World, QueryCache, WithIds, newWithout);
    }
    public FluentQueryBuilder<T1, T2, T3> With<T1, T2, T3>() where T1 : unmanaged where T2 : unmanaged where T3 : unmanaged
    {
        var newWith = WithIds is null ? [] : new List<int>(WithIds);
        newWith.AddRange([World.GetTypeId<T1>(), World.GetTypeId<T2>(), World.GetTypeId<T3>()]);
        return new FluentQueryBuilder<T1, T2, T3>(new FluentQueryBuilder(World, QueryCache, newWith, WithoutIds));
    }
    public FluentQueryBuilder Without<T1, T2, T3>() where T1 : unmanaged where T2 : unmanaged where T3 : unmanaged
    {
        var newWithout = WithoutIds is null ? [] : new List<int>(WithoutIds);
        newWithout.AddRange([World.GetTypeId<T1>(), World.GetTypeId<T2>(), World.GetTypeId<T3>()]);
        return new FluentQueryBuilder(World, QueryCache, WithIds, newWithout);
    }
    public FluentQueryBuilder<T1, T2, T3, T4> With<T1, T2, T3, T4>() where T1 : unmanaged where T2 : unmanaged where T3 : unmanaged where T4 : unmanaged
    {
        var newWith = WithIds is null ? [] : new List<int>(WithIds);
        newWith.AddRange([World.GetTypeId<T1>(), World.GetTypeId<T2>(), World.GetTypeId<T3>(), World.GetTypeId<T4>()]);
        return new FluentQueryBuilder<T1, T2, T3, T4>(new FluentQueryBuilder(World, QueryCache, newWith, WithoutIds));
    }
    public FluentQueryBuilder Without<T1, T2, T3, T4>() where T1 : unmanaged where T2 : unmanaged where T3 : unmanaged where T4 : unmanaged
    {
        var newWithout = WithoutIds is null ? [] : new List<int>(WithoutIds);
        newWithout.AddRange([World.GetTypeId<T1>(), World.GetTypeId<T2>(), World.GetTypeId<T3>(), World.GetTypeId<T4>()]);
        return new FluentQueryBuilder(World, QueryCache, WithIds, newWithout);
    }
    public FluentQueryBuilder<T1, T2, T3, T4, T5> With<T1, T2, T3, T4, T5>() where T1 : unmanaged where T2 : unmanaged where T3 : unmanaged where T4 : unmanaged where T5 : unmanaged
    {
        var newWith = WithIds is null ? [] : new List<int>(WithIds);
        newWith.AddRange([World.GetTypeId<T1>(), World.GetTypeId<T2>(), World.GetTypeId<T3>(), World.GetTypeId<T4>(), World.GetTypeId<T5>()]);
        return new FluentQueryBuilder<T1, T2, T3, T4, T5>(new FluentQueryBuilder(World, QueryCache, newWith, WithoutIds));
    }
    public FluentQueryBuilder Without<T1, T2, T3, T4, T5>() where T1 : unmanaged where T2 : unmanaged where T3 : unmanaged where T4 : unmanaged where T5 : unmanaged
    {
        var newWithout = WithoutIds is null ? [] : new List<int>(WithoutIds);
        newWithout.AddRange([World.GetTypeId<T1>(), World.GetTypeId<T2>(), World.GetTypeId<T3>(), World.GetTypeId<T4>(), World.GetTypeId<T5>()]);
        return new FluentQueryBuilder(World, QueryCache, WithIds, newWithout);
    }
    public FluentQueryBuilder<T1, T2, T3, T4, T5, T6> With<T1, T2, T3, T4, T5, T6>() where T1 : unmanaged where T2 : unmanaged where T3 : unmanaged where T4 : unmanaged where T5 : unmanaged where T6 : unmanaged
    {
        var newWith = WithIds is null ? [] : new List<int>(WithIds);
        newWith.AddRange([World.GetTypeId<T1>(), World.GetTypeId<T2>(), World.GetTypeId<T3>(), World.GetTypeId<T4>(), World.GetTypeId<T5>(), World.GetTypeId<T6>()]);
        return new FluentQueryBuilder<T1, T2, T3, T4, T5, T6>(new FluentQueryBuilder(World, QueryCache, newWith, WithoutIds));
    }
    public FluentQueryBuilder Without<T1, T2, T3, T4, T5, T6>() where T1 : unmanaged where T2 : unmanaged where T3 : unmanaged where T4 : unmanaged where T5 : unmanaged where T6 : unmanaged
    {
        var newWithout = WithoutIds is null ? [] : new List<int>(WithoutIds);
        newWithout.AddRange([World.GetTypeId<T1>(), World.GetTypeId<T2>(), World.GetTypeId<T3>(), World.GetTypeId<T4>(), World.GetTypeId<T5>(), World.GetTypeId<T6>()]);
        return new FluentQueryBuilder(World, QueryCache, WithIds, newWithout);
    }
    public FluentQueryBuilder<T1, T2, T3, T4, T5, T6, T7> With<T1, T2, T3, T4, T5, T6, T7>() where T1 : unmanaged where T2 : unmanaged where T3 : unmanaged where T4 : unmanaged where T5 : unmanaged where T6 : unmanaged where T7 : unmanaged
    {
        var newWith = WithIds is null ? [] : new List<int>(WithIds);
        newWith.AddRange([World.GetTypeId<T1>(), World.GetTypeId<T2>(), World.GetTypeId<T3>(), World.GetTypeId<T4>(), World.GetTypeId<T5>(), World.GetTypeId<T6>(), World.GetTypeId<T7>()]);
        return new FluentQueryBuilder<T1, T2, T3, T4, T5, T6, T7>(new FluentQueryBuilder(World, QueryCache, newWith, WithoutIds));
    }
    public FluentQueryBuilder Without<T1, T2, T3, T4, T5, T6, T7>() where T1 : unmanaged where T2 : unmanaged where T3 : unmanaged where T4 : unmanaged where T5 : unmanaged where T6 : unmanaged where T7 : unmanaged
    {
        var newWithout = WithoutIds is null ? [] : new List<int>(WithoutIds);
        newWithout.AddRange([World.GetTypeId<T1>(), World.GetTypeId<T2>(), World.GetTypeId<T3>(), World.GetTypeId<T4>(), World.GetTypeId<T5>(), World.GetTypeId<T6>(), World.GetTypeId<T7>()]);
        return new FluentQueryBuilder(World, QueryCache, WithIds, newWithout);
    }
    public FluentQueryBuilder<T1, T2, T3, T4, T5, T6, T7, T8> With<T1, T2, T3, T4, T5, T6, T7, T8>() where T1 : unmanaged where T2 : unmanaged where T3 : unmanaged where T4 : unmanaged where T5 : unmanaged where T6 : unmanaged where T7 : unmanaged where T8 : unmanaged
    {
        var newWith = WithIds is null ? [] : new List<int>(WithIds);
        newWith.AddRange([World.GetTypeId<T1>(), World.GetTypeId<T2>(), World.GetTypeId<T3>(), World.GetTypeId<T4>(), World.GetTypeId<T5>(), World.GetTypeId<T6>(), World.GetTypeId<T7>(), World.GetTypeId<T8>()]);
        return new FluentQueryBuilder<T1, T2, T3, T4, T5, T6, T7, T8>(new FluentQueryBuilder(World, QueryCache, newWith, WithoutIds));
    }
    public FluentQueryBuilder Without<T1, T2, T3, T4, T5, T6, T7, T8>() where T1 : unmanaged where T2 : unmanaged where T3 : unmanaged where T4 : unmanaged where T5 : unmanaged where T6 : unmanaged where T7 : unmanaged where T8 : unmanaged
    {
        var newWithout = WithoutIds is null ? [] : new List<int>(WithoutIds);
        newWithout.AddRange([World.GetTypeId<T1>(), World.GetTypeId<T2>(), World.GetTypeId<T3>(), World.GetTypeId<T4>(), World.GetTypeId<T5>(), World.GetTypeId<T6>(), World.GetTypeId<T7>(), World.GetTypeId<T8>()]);
        return new FluentQueryBuilder(World, QueryCache, WithIds, newWithout);
    }
}

// Generic parts
public readonly partial struct FluentQueryBuilder<T1> where T1 : unmanaged
{
    private readonly FluentQueryBuilder _builder;
    internal FluentQueryBuilder(FluentQueryBuilder builder) { _builder = builder; }

    private Query GetQuery()
    {
        var requiredTypes = new[] { typeof(T1) };
        var withCount = (_builder.WithIds?.Count ?? 0) + requiredTypes.Length;
        var withList = new List<int>(withCount);
        if (_builder.WithIds != null) withList.AddRange(_builder.WithIds);
        foreach (var type in requiredTypes) withList.Add(_builder.World.GetTypeId(type));
        withList.Sort();
        _builder.WithoutIds?.Sort();
        int hashCode = 17;
        unchecked
        {
            foreach (var id in withList) hashCode = hashCode * 31 + id;
            if (_builder.WithoutIds != null) foreach (var id in _builder.WithoutIds) hashCode = hashCode * 31 + id;
        }
        if (!_builder.QueryCache.TryGetValue(hashCode, out var query))
        {
            var qb = _builder.World.Builder();
            foreach (var id in withList) qb.WithTypeIds.Add(id);
            if (_builder.WithoutIds != null) foreach (var id in _builder.WithoutIds) qb.WithoutTypeIds.Add(id);
            query = qb.Build<T1>();
            _builder.QueryCache[hashCode] = query;
        }
        return query;
    }

    public void ForEach(ForEachAction<T1> action) => _builder.World.Iterate<T1>(GetQuery()).ForEach(action);
    public void ForEach(ForEachWithEntityAction<T1> action) => _builder.World.Iterate<T1>(GetQuery()).ForEach(action);
}

public readonly partial struct FluentQueryBuilder<T1, T2> where T1 : unmanaged where T2 : unmanaged
{
    private readonly FluentQueryBuilder _builder;
    internal FluentQueryBuilder(FluentQueryBuilder builder) { _builder = builder; }

    private Query GetQuery()
    {
        var requiredTypes = new[] { typeof(T1), typeof(T2) };
        var withCount = (_builder.WithIds?.Count ?? 0) + requiredTypes.Length;
        var withList = new List<int>(withCount);
        if (_builder.WithIds != null) withList.AddRange(_builder.WithIds);
        foreach (var type in requiredTypes) withList.Add(_builder.World.GetTypeId(type));
        withList.Sort();
        _builder.WithoutIds?.Sort();
        int hashCode = 17;
        unchecked
        {
            foreach (var id in withList) hashCode = hashCode * 31 + id;
            if (_builder.WithoutIds != null) foreach (var id in _builder.WithoutIds) hashCode = hashCode * 31 + id;
        }
        if (!_builder.QueryCache.TryGetValue(hashCode, out var query))
        {
            var qb = _builder.World.Builder();
            foreach (var id in withList) qb.WithTypeIds.Add(id);
            if (_builder.WithoutIds != null) foreach (var id in _builder.WithoutIds) qb.WithoutTypeIds.Add(id);
            query = qb.Build<T1, T2>();
            _builder.QueryCache[hashCode] = query;
        }
        return query;
    }

    public void ForEach(ForEachAction<T1, T2> action) => _builder.World.Iterate<T1, T2>(GetQuery()).ForEach(action);
    public void ForEach(ForEachWithEntityAction<T1, T2> action) => _builder.World.Iterate<T1, T2>(GetQuery()).ForEach(action);
}

public readonly partial struct FluentQueryBuilder<T1, T2, T3> where T1 : unmanaged where T2 : unmanaged where T3 : unmanaged
{
    private readonly FluentQueryBuilder _builder;
    internal FluentQueryBuilder(FluentQueryBuilder builder) { _builder = builder; }

    private Query GetQuery()
    {
        var requiredTypes = new[] { typeof(T1), typeof(T2), typeof(T3) };
        var withCount = (_builder.WithIds?.Count ?? 0) + requiredTypes.Length;
        var withList = new List<int>(withCount);
        if (_builder.WithIds != null) withList.AddRange(_builder.WithIds);
        foreach (var type in requiredTypes) withList.Add(_builder.World.GetTypeId(type));
        withList.Sort();
        _builder.WithoutIds?.Sort();
        int hashCode = 17;
        unchecked
        {
            foreach (var id in withList) hashCode = hashCode * 31 + id;
            if (_builder.WithoutIds != null) foreach (var id in _builder.WithoutIds) hashCode = hashCode * 31 + id;
        }
        if (!_builder.QueryCache.TryGetValue(hashCode, out var query))
        {
            var qb = _builder.World.Builder();
            foreach (var id in withList) qb.WithTypeIds.Add(id);
            if (_builder.WithoutIds != null) foreach (var id in _builder.WithoutIds) qb.WithoutTypeIds.Add(id);
            query = qb.Build<T1, T2, T3>();
            _builder.QueryCache[hashCode] = query;
        }
        return query;
    }

    public void ForEach(ForEachAction<T1, T2, T3> action) => _builder.World.Iterate<T1, T2, T3>(GetQuery()).ForEach(action);
    public void ForEach(ForEachWithEntityAction<T1, T2, T3> action) => _builder.World.Iterate<T1, T2, T3>(GetQuery()).ForEach(action);
}

public readonly partial struct FluentQueryBuilder<T1, T2, T3, T4> where T1 : unmanaged where T2 : unmanaged where T3 : unmanaged where T4 : unmanaged
{
    private readonly FluentQueryBuilder _builder;
    internal FluentQueryBuilder(FluentQueryBuilder builder) { _builder = builder; }

    private Query GetQuery()
    {
        var requiredTypes = new[] { typeof(T1), typeof(T2), typeof(T3), typeof(T4) };
        var withCount = (_builder.WithIds?.Count ?? 0) + requiredTypes.Length;
        var withList = new List<int>(withCount);
        if (_builder.WithIds != null) withList.AddRange(_builder.WithIds);
        foreach (var type in requiredTypes) withList.Add(_builder.World.GetTypeId(type));
        withList.Sort();
        _builder.WithoutIds?.Sort();
        int hashCode = 17;
        unchecked
        {
            foreach (var id in withList) hashCode = hashCode * 31 + id;
            if (_builder.WithoutIds != null) foreach (var id in _builder.WithoutIds) hashCode = hashCode * 31 + id;
        }
        if (!_builder.QueryCache.TryGetValue(hashCode, out var query))
        {
            var qb = _builder.World.Builder();
            foreach (var id in withList) qb.WithTypeIds.Add(id);
            if (_builder.WithoutIds != null) foreach (var id in _builder.WithoutIds) qb.WithoutTypeIds.Add(id);
            query = qb.Build<T1, T2, T3, T4>();
            _builder.QueryCache[hashCode] = query;
        }
        return query;
    }

    public void ForEach(ForEachAction<T1, T2, T3, T4> action) => _builder.World.Iterate<T1, T2, T3, T4>(GetQuery()).ForEach(action);
    public void ForEach(ForEachWithEntityAction<T1, T2, T3, T4> action) => _builder.World.Iterate<T1, T2, T3, T4>(GetQuery()).ForEach(action);
}

public readonly partial struct FluentQueryBuilder<T1, T2, T3, T4, T5> where T1 : unmanaged where T2 : unmanaged where T3 : unmanaged where T4 : unmanaged where T5 : unmanaged
{
    private readonly FluentQueryBuilder _builder;
    internal FluentQueryBuilder(FluentQueryBuilder builder) { _builder = builder; }

    private Query GetQuery()
    {
        var requiredTypes = new[] { typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5) };
        var withCount = (_builder.WithIds?.Count ?? 0) + requiredTypes.Length;
        var withList = new List<int>(withCount);
        if (_builder.WithIds != null) withList.AddRange(_builder.WithIds);
        foreach (var type in requiredTypes) withList.Add(_builder.World.GetTypeId(type));
        withList.Sort();
        _builder.WithoutIds?.Sort();
        int hashCode = 17;
        unchecked
        {
            foreach (var id in withList) hashCode = hashCode * 31 + id;
            if (_builder.WithoutIds != null) foreach (var id in _builder.WithoutIds) hashCode = hashCode * 31 + id;
        }
        if (!_builder.QueryCache.TryGetValue(hashCode, out var query))
        {
            var qb = _builder.World.Builder();
            foreach (var id in withList) qb.WithTypeIds.Add(id);
            if (_builder.WithoutIds != null) foreach (var id in _builder.WithoutIds) qb.WithoutTypeIds.Add(id);
            query = qb.Build<T1, T2, T3, T4, T5>();
            _builder.QueryCache[hashCode] = query;
        }
        return query;
    }

    public void ForEach(ForEachAction<T1, T2, T3, T4, T5> action) => _builder.World.Iterate<T1, T2, T3, T4, T5>(GetQuery()).ForEach(action);
    public void ForEach(ForEachWithEntityAction<T1, T2, T3, T4, T5> action) => _builder.World.Iterate<T1, T2, T3, T4, T5>(GetQuery()).ForEach(action);
}

public readonly partial struct FluentQueryBuilder<T1, T2, T3, T4, T5, T6> where T1 : unmanaged where T2 : unmanaged where T3 : unmanaged where T4 : unmanaged where T5 : unmanaged where T6 : unmanaged
{
    private readonly FluentQueryBuilder _builder;
    internal FluentQueryBuilder(FluentQueryBuilder builder) { _builder = builder; }

    private Query GetQuery()
    {
        var requiredTypes = new[] { typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6) };
        var withCount = (_builder.WithIds?.Count ?? 0) + requiredTypes.Length;
        var withList = new List<int>(withCount);
        if (_builder.WithIds != null) withList.AddRange(_builder.WithIds);
        foreach (var type in requiredTypes) withList.Add(_builder.World.GetTypeId(type));
        withList.Sort();
        _builder.WithoutIds?.Sort();
        int hashCode = 17;
        unchecked
        {
            foreach (var id in withList) hashCode = hashCode * 31 + id;
            if (_builder.WithoutIds != null) foreach (var id in _builder.WithoutIds) hashCode = hashCode * 31 + id;
        }
        if (!_builder.QueryCache.TryGetValue(hashCode, out var query))
        {
            var qb = _builder.World.Builder();
            foreach (var id in withList) qb.WithTypeIds.Add(id);
            if (_builder.WithoutIds != null) foreach (var id in _builder.WithoutIds) qb.WithoutTypeIds.Add(id);
            query = qb.Build<T1, T2, T3, T4, T5, T6>();
            _builder.QueryCache[hashCode] = query;
        }
        return query;
    }

    public void ForEach(ForEachAction<T1, T2, T3, T4, T5, T6> action) => _builder.World.Iterate<T1, T2, T3, T4, T5, T6>(GetQuery()).ForEach(action);
    public void ForEach(ForEachWithEntityAction<T1, T2, T3, T4, T5, T6> action) => _builder.World.Iterate<T1, T2, T3, T4, T5, T6>(GetQuery()).ForEach(action);
}

public readonly partial struct FluentQueryBuilder<T1, T2, T3, T4, T5, T6, T7> where T1 : unmanaged where T2 : unmanaged where T3 : unmanaged where T4 : unmanaged where T5 : unmanaged where T6 : unmanaged where T7 : unmanaged
{
    private readonly FluentQueryBuilder _builder;
    internal FluentQueryBuilder(FluentQueryBuilder builder) { _builder = builder; }

    private Query GetQuery()
    {
        var requiredTypes = new[] { typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7) };
        var withCount = (_builder.WithIds?.Count ?? 0) + requiredTypes.Length;
        var withList = new List<int>(withCount);
        if (_builder.WithIds != null) withList.AddRange(_builder.WithIds);
        foreach (var type in requiredTypes) withList.Add(_builder.World.GetTypeId(type));
        withList.Sort();
        _builder.WithoutIds?.Sort();
        int hashCode = 17;
        unchecked
        {
            foreach (var id in withList) hashCode = hashCode * 31 + id;
            if (_builder.WithoutIds != null) foreach (var id in _builder.WithoutIds) hashCode = hashCode * 31 + id;
        }
        if (!_builder.QueryCache.TryGetValue(hashCode, out var query))
        {
            var qb = _builder.World.Builder();
            foreach (var id in withList) qb.WithTypeIds.Add(id);
            if (_builder.WithoutIds != null) foreach (var id in _builder.WithoutIds) qb.WithoutTypeIds.Add(id);
            query = qb.Build<T1, T2, T3, T4, T5, T6, T7>();
            _builder.QueryCache[hashCode] = query;
        }
        return query;
    }

    public void ForEach(ForEachAction<T1, T2, T3, T4, T5, T6, T7> action) => _builder.World.Iterate<T1, T2, T3, T4, T5, T6, T7>(GetQuery()).ForEach(action);
    public void ForEach(ForEachWithEntityAction<T1, T2, T3, T4, T5, T6, T7> action) => _builder.World.Iterate<T1, T2, T3, T4, T5, T6, T7>(GetQuery()).ForEach(action);
}

public readonly partial struct FluentQueryBuilder<T1, T2, T3, T4, T5, T6, T7, T8> where T1 : unmanaged where T2 : unmanaged where T3 : unmanaged where T4 : unmanaged where T5 : unmanaged where T6 : unmanaged where T7 : unmanaged where T8 : unmanaged
{
    private readonly FluentQueryBuilder _builder;
    internal FluentQueryBuilder(FluentQueryBuilder builder) { _builder = builder; }

    private Query GetQuery()
    {
        var requiredTypes = new[] { typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8) };
        var withCount = (_builder.WithIds?.Count ?? 0) + requiredTypes.Length;
        var withList = new List<int>(withCount);
        if (_builder.WithIds != null) withList.AddRange(_builder.WithIds);
        foreach (var type in requiredTypes) withList.Add(_builder.World.GetTypeId(type));
        withList.Sort();
        _builder.WithoutIds?.Sort();
        int hashCode = 17;
        unchecked
        {
            foreach (var id in withList) hashCode = hashCode * 31 + id;
            if (_builder.WithoutIds != null) foreach (var id in _builder.WithoutIds) hashCode = hashCode * 31 + id;
        }
        if (!_builder.QueryCache.TryGetValue(hashCode, out var query))
        {
            var qb = _builder.World.Builder();
            foreach (var id in withList) qb.WithTypeIds.Add(id);
            if (_builder.WithoutIds != null) foreach (var id in _builder.WithoutIds) qb.WithoutTypeIds.Add(id);
            query = qb.Build<T1, T2, T3, T4, T5, T6, T7, T8>();
            _builder.QueryCache[hashCode] = query;
        }
        return query;
    }

    public void ForEach(ForEachAction<T1, T2, T3, T4, T5, T6, T7, T8> action) => _builder.World.Iterate<T1, T2, T3, T4, T5, T6, T7, T8>(GetQuery()).ForEach(action);
    public void ForEach(ForEachWithEntityAction<T1, T2, T3, T4, T5, T6, T7, T8> action) => _builder.World.Iterate<T1, T2, T3, T4, T5, T6, T7, T8>(GetQuery()).ForEach(action);
}