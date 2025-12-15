using Engine.DataStructures;
using Engine.ECS.Archetypes;
using Engine.ECS.Archetypes.QueryDefinition;

namespace Engine.ECS;

public readonly partial struct FluentQueryBuilder
{
    public FluentQueryBuilder<T1> With<T1>() where T1 : unmanaged
    {
        var newWith = ListPool<int>.Rent(WithIds);
        newWith.AddRange([World.GetTypeId<T1>()]);
        return new FluentQueryBuilder<T1>(new FluentQueryBuilder(World, Registry, newWith, WithoutIds, IsParallel));
    }
    public FluentQueryBuilder Without<T1>() where T1 : unmanaged
    {
        var newWithout = ListPool<int>.Rent(WithoutIds);
        newWithout.AddRange([World.GetTypeId<T1>()]);
        return new FluentQueryBuilder(World, Registry, WithIds, newWithout, IsParallel);
    }
    public FluentQueryBuilder<T1, T2> With<T1, T2>() where T1 : unmanaged where T2 : unmanaged
    {
        var newWith = ListPool<int>.Rent(WithIds);
        newWith.AddRange([World.GetTypeId<T1>(), World.GetTypeId<T2>()]);
        return new FluentQueryBuilder<T1, T2>(new FluentQueryBuilder(World, Registry, newWith, WithoutIds, IsParallel));
    }
    public FluentQueryBuilder Without<T1, T2>() where T1 : unmanaged where T2 : unmanaged
    {
        var newWithout = ListPool<int>.Rent(WithoutIds);
        newWithout.AddRange([World.GetTypeId<T1>(), World.GetTypeId<T2>()]);
        return new FluentQueryBuilder(World, Registry, WithIds, newWithout, IsParallel);
    }
    public FluentQueryBuilder<T1, T2, T3> With<T1, T2, T3>() where T1 : unmanaged where T2 : unmanaged where T3 : unmanaged
    {
        var newWith = ListPool<int>.Rent(WithIds);
        newWith.AddRange([World.GetTypeId<T1>(), World.GetTypeId<T2>(), World.GetTypeId<T3>()]);
        return new FluentQueryBuilder<T1, T2, T3>(new FluentQueryBuilder(World, Registry, newWith, WithoutIds, IsParallel));
    }
    public FluentQueryBuilder Without<T1, T2, T3>() where T1 : unmanaged where T2 : unmanaged where T3 : unmanaged
    {
        var newWithout = ListPool<int>.Rent(WithoutIds);
        newWithout.AddRange([World.GetTypeId<T1>(), World.GetTypeId<T2>(), World.GetTypeId<T3>()]);
        return new FluentQueryBuilder(World, Registry, WithIds, newWithout, IsParallel);
    }
    public FluentQueryBuilder<T1, T2, T3, T4> With<T1, T2, T3, T4>() where T1 : unmanaged where T2 : unmanaged where T3 : unmanaged where T4 : unmanaged
    {
        var newWith = ListPool<int>.Rent(WithIds);
        newWith.AddRange([World.GetTypeId<T1>(), World.GetTypeId<T2>(), World.GetTypeId<T3>(), World.GetTypeId<T4>()]);
        return new FluentQueryBuilder<T1, T2, T3, T4>(new FluentQueryBuilder(World, Registry, newWith, WithoutIds, IsParallel));
    }
    public FluentQueryBuilder Without<T1, T2, T3, T4>() where T1 : unmanaged where T2 : unmanaged where T3 : unmanaged where T4 : unmanaged
    {
        var newWithout = ListPool<int>.Rent(WithoutIds);
        newWithout.AddRange([World.GetTypeId<T1>(), World.GetTypeId<T2>(), World.GetTypeId<T3>(), World.GetTypeId<T4>()]);
        return new FluentQueryBuilder(World, Registry, WithIds, newWithout, IsParallel);
    }
    public FluentQueryBuilder<T1, T2, T3, T4, T5> With<T1, T2, T3, T4, T5>() where T1 : unmanaged where T2 : unmanaged where T3 : unmanaged where T4 : unmanaged where T5 : unmanaged
    {
        var newWith = ListPool<int>.Rent(WithIds);
        newWith.AddRange([World.GetTypeId<T1>(), World.GetTypeId<T2>(), World.GetTypeId<T3>(), World.GetTypeId<T4>(), World.GetTypeId<T5>()]);
        return new FluentQueryBuilder<T1, T2, T3, T4, T5>(new FluentQueryBuilder(World, Registry, newWith, WithoutIds, IsParallel));
    }
    public FluentQueryBuilder Without<T1, T2, T3, T4, T5>() where T1 : unmanaged where T2 : unmanaged where T3 : unmanaged where T4 : unmanaged where T5 : unmanaged
    {
        var newWithout = ListPool<int>.Rent(WithoutIds);
        newWithout.AddRange([World.GetTypeId<T1>(), World.GetTypeId<T2>(), World.GetTypeId<T3>(), World.GetTypeId<T4>(), World.GetTypeId<T5>()]);
        return new FluentQueryBuilder(World, Registry, WithIds, newWithout, IsParallel);
    }
    public FluentQueryBuilder<T1, T2, T3, T4, T5, T6> With<T1, T2, T3, T4, T5, T6>() where T1 : unmanaged where T2 : unmanaged where T3 : unmanaged where T4 : unmanaged where T5 : unmanaged where T6 : unmanaged
    {
        var newWith = ListPool<int>.Rent(WithIds);
        newWith.AddRange([World.GetTypeId<T1>(), World.GetTypeId<T2>(), World.GetTypeId<T3>(), World.GetTypeId<T4>(), World.GetTypeId<T5>(), World.GetTypeId<T6>()]);
        return new FluentQueryBuilder<T1, T2, T3, T4, T5, T6>(new FluentQueryBuilder(World, Registry, newWith, WithoutIds, IsParallel));
    }
    public FluentQueryBuilder Without<T1, T2, T3, T4, T5, T6>() where T1 : unmanaged where T2 : unmanaged where T3 : unmanaged where T4 : unmanaged where T5 : unmanaged where T6 : unmanaged
    {
        var newWithout = ListPool<int>.Rent(WithoutIds);
        newWithout.AddRange([World.GetTypeId<T1>(), World.GetTypeId<T2>(), World.GetTypeId<T3>(), World.GetTypeId<T4>(), World.GetTypeId<T5>(), World.GetTypeId<T6>()]);
        return new FluentQueryBuilder(World, Registry, WithIds, newWithout, IsParallel);
    }
    public FluentQueryBuilder<T1, T2, T3, T4, T5, T6, T7> With<T1, T2, T3, T4, T5, T6, T7>() where T1 : unmanaged where T2 : unmanaged where T3 : unmanaged where T4 : unmanaged where T5 : unmanaged where T6 : unmanaged where T7 : unmanaged
    {
        var newWith = ListPool<int>.Rent(WithIds);
        newWith.AddRange([World.GetTypeId<T1>(), World.GetTypeId<T2>(), World.GetTypeId<T3>(), World.GetTypeId<T4>(), World.GetTypeId<T5>(), World.GetTypeId<T6>(), World.GetTypeId<T7>()]);
        return new FluentQueryBuilder<T1, T2, T3, T4, T5, T6, T7>(new FluentQueryBuilder(World, Registry, newWith, WithoutIds, IsParallel));
    }
    public FluentQueryBuilder Without<T1, T2, T3, T4, T5, T6, T7>() where T1 : unmanaged where T2 : unmanaged where T3 : unmanaged where T4 : unmanaged where T5 : unmanaged where T6 : unmanaged where T7 : unmanaged
    {
        var newWithout = ListPool<int>.Rent(WithoutIds);
        newWithout.AddRange([World.GetTypeId<T1>(), World.GetTypeId<T2>(), World.GetTypeId<T3>(), World.GetTypeId<T4>(), World.GetTypeId<T5>(), World.GetTypeId<T6>(), World.GetTypeId<T7>()]);
        return new FluentQueryBuilder(World, Registry, WithIds, newWithout, IsParallel);
    }
    public FluentQueryBuilder<T1, T2, T3, T4, T5, T6, T7, T8> With<T1, T2, T3, T4, T5, T6, T7, T8>() where T1 : unmanaged where T2 : unmanaged where T3 : unmanaged where T4 : unmanaged where T5 : unmanaged where T6 : unmanaged where T7 : unmanaged where T8 : unmanaged
    {
        var newWith = ListPool<int>.Rent(WithIds);
        newWith.AddRange([World.GetTypeId<T1>(), World.GetTypeId<T2>(), World.GetTypeId<T3>(), World.GetTypeId<T4>(), World.GetTypeId<T5>(), World.GetTypeId<T6>(), World.GetTypeId<T7>(), World.GetTypeId<T8>()]);
        return new FluentQueryBuilder<T1, T2, T3, T4, T5, T6, T7, T8>(new FluentQueryBuilder(World, Registry, newWith, WithoutIds, IsParallel));
    }
    public FluentQueryBuilder Without<T1, T2, T3, T4, T5, T6, T7, T8>() where T1 : unmanaged where T2 : unmanaged where T3 : unmanaged where T4 : unmanaged where T5 : unmanaged where T6 : unmanaged where T7 : unmanaged where T8 : unmanaged
    {
        var newWithout = ListPool<int>.Rent(WithoutIds);
        newWithout.AddRange([World.GetTypeId<T1>(), World.GetTypeId<T2>(), World.GetTypeId<T3>(), World.GetTypeId<T4>(), World.GetTypeId<T5>(), World.GetTypeId<T6>(), World.GetTypeId<T7>(), World.GetTypeId<T8>()]);
        return new FluentQueryBuilder(World, Registry, WithIds, newWithout, IsParallel);
    }
}

public readonly partial struct FluentQueryBuilder<T1> where T1 : unmanaged
{
    private readonly FluentQueryBuilder _builder;
    internal FluentQueryBuilder(FluentQueryBuilder builder) { _builder = builder; }

    private Query GetQuery() => _builder.Registry.GetOrCreateQuery(
        _builder.WithIds,
        _builder.WithoutIds,
        [
            _builder.World.GetTypeId<T1>()
        ],
        _builder.IsParallel
    );

    public void ForEach(ForEachAction<T1> action) => _builder.World.Iterate<T1>(GetQuery()).ForEach(action);
    public void ForEach(ForEachWithEntityAction<T1> action) => _builder.World.Iterate<T1>(GetQuery()).ForEach(action);

    public FluentQueryBuilder<T1> AsParallel() => new(_builder.AsParallel());

    public FluentQueryBuilder<T1> Without<T>() where T : unmanaged
    {
        var newWithout = ListPool<int>.Rent(_builder.WithoutIds);
        newWithout.Add(_builder.World.GetTypeId<T>());
        return new FluentQueryBuilder<T1>(new FluentQueryBuilder(_builder.World, _builder.Registry, _builder.WithIds, newWithout, _builder.IsParallel));
    }

    public FluentQueryBuilder<T1, T2> With<T2>() where T2 : unmanaged
    {
        var newWith = ListPool<int>.Rent(_builder.WithIds);
        newWith.Add(_builder.World.GetTypeId<T2>());
        return new FluentQueryBuilder<T1, T2>(new FluentQueryBuilder(_builder.World, _builder.Registry, newWith, _builder.WithoutIds, _builder.IsParallel));
    }
}

public readonly partial struct FluentQueryBuilder<T1, T2> where T1 : unmanaged where T2 : unmanaged
{
    private readonly FluentQueryBuilder _builder;
    internal FluentQueryBuilder(FluentQueryBuilder builder) { _builder = builder; }

    private Query GetQuery() => _builder.Registry.GetOrCreateQuery(
        _builder.WithIds,
        _builder.WithoutIds,
        [
            _builder.World.GetTypeId<T1>(),
            _builder.World.GetTypeId<T2>()
        ],
        _builder.IsParallel
    );

    public void ForEach(ForEachAction<T1, T2> action) => _builder.World.Iterate<T1, T2>(GetQuery()).ForEach(action);
    public void ForEach(ForEachWithEntityAction<T1, T2> action) => _builder.World.Iterate<T1, T2>(GetQuery()).ForEach(action);

    public FluentQueryBuilder<T1, T2> AsParallel() => new(_builder.AsParallel());

    public FluentQueryBuilder<T1, T2> Without<T>() where T : unmanaged
    {
        var newWithout = ListPool<int>.Rent(_builder.WithoutIds);
        newWithout.Add(_builder.World.GetTypeId<T>());
        return new FluentQueryBuilder<T1, T2>(new FluentQueryBuilder(_builder.World, _builder.Registry, _builder.WithIds, newWithout, _builder.IsParallel));
    }

    public FluentQueryBuilder<T1, T2, T3> With<T3>() where T3 : unmanaged
    {
        var newWith = ListPool<int>.Rent(_builder.WithIds);
        newWith.Add(_builder.World.GetTypeId<T3>());
        return new FluentQueryBuilder<T1, T2, T3>(new FluentQueryBuilder(_builder.World, _builder.Registry, newWith, _builder.WithoutIds, _builder.IsParallel));
    }
}

public readonly partial struct FluentQueryBuilder<T1, T2, T3> where T1 : unmanaged where T2 : unmanaged where T3 : unmanaged
{
    private readonly FluentQueryBuilder _builder;
    internal FluentQueryBuilder(FluentQueryBuilder builder) { _builder = builder; }

    private Query GetQuery() => _builder.Registry.GetOrCreateQuery(
        _builder.WithIds, 
        _builder.WithoutIds, 
        [
            _builder.World.GetTypeId<T1>(),
            _builder.World.GetTypeId<T2>(),
            _builder.World.GetTypeId<T3>()
        ],
        _builder.IsParallel
    );

    public void ForEach(ForEachAction<T1, T2, T3> action) => _builder.World.Iterate<T1, T2, T3>(GetQuery()).ForEach(action);
    public void ForEach(ForEachWithEntityAction<T1, T2, T3> action) => _builder.World.Iterate<T1, T2, T3>(GetQuery()).ForEach(action);

    public FluentQueryBuilder<T1, T2, T3> AsParallel() => new(_builder.AsParallel());

    public FluentQueryBuilder<T1, T2, T3> Without<T>() where T : unmanaged
    {
        var newWithout = ListPool<int>.Rent(_builder.WithoutIds);
        newWithout.Add(_builder.World.GetTypeId<T>());
        return new FluentQueryBuilder<T1, T2, T3>(new FluentQueryBuilder(_builder.World, _builder.Registry, _builder.WithIds, newWithout, _builder.IsParallel));
    }

    public FluentQueryBuilder<T1, T2, T3, T4> With<T4>() where T4 : unmanaged
    {
        var newWith = ListPool<int>.Rent(_builder.WithIds);
        newWith.Add(_builder.World.GetTypeId<T4>());
        return new FluentQueryBuilder<T1, T2, T3, T4>(new FluentQueryBuilder(_builder.World, _builder.Registry, newWith, _builder.WithoutIds, _builder.IsParallel));
    }
}

public readonly partial struct FluentQueryBuilder<T1, T2, T3, T4> where T1 : unmanaged where T2 : unmanaged where T3 : unmanaged where T4 : unmanaged
{
    private readonly FluentQueryBuilder _builder;
    internal FluentQueryBuilder(FluentQueryBuilder builder) { _builder = builder; }

    private Query GetQuery() => _builder.Registry.GetOrCreateQuery(
        _builder.WithIds, 
        _builder.WithoutIds, 
        [
            _builder.World.GetTypeId<T1>(), 
            _builder.World.GetTypeId<T2>(), 
            _builder.World.GetTypeId<T3>(), 
            _builder.World.GetTypeId<T4>()
        ], 
        _builder.IsParallel
    );

    public void ForEach(ForEachAction<T1, T2, T3, T4> action) => _builder.World.Iterate<T1, T2, T3, T4>(GetQuery()).ForEach(action);
    public void ForEach(ForEachWithEntityAction<T1, T2, T3, T4> action) => _builder.World.Iterate<T1, T2, T3, T4>(GetQuery()).ForEach(action);

    public FluentQueryBuilder<T1, T2, T3, T4> AsParallel() => new(_builder.AsParallel());

    public FluentQueryBuilder<T1, T2, T3, T4> Without<T>() where T : unmanaged
    {
        var newWithout = ListPool<int>.Rent(_builder.WithoutIds);
        newWithout.Add(_builder.World.GetTypeId<T>());
        return new FluentQueryBuilder<T1, T2, T3, T4>(new FluentQueryBuilder(_builder.World, _builder.Registry, _builder.WithIds, newWithout, _builder.IsParallel));
    }

    public FluentQueryBuilder<T1, T2, T3, T4, T5> With<T5>() where T5 : unmanaged
    {
        var newWith = ListPool<int>.Rent(_builder.WithIds);
        newWith.Add(_builder.World.GetTypeId<T5>());
        return new FluentQueryBuilder<T1, T2, T3, T4, T5>(new FluentQueryBuilder(_builder.World, _builder.Registry, newWith, _builder.WithoutIds, _builder.IsParallel));
    }
}

public readonly partial struct FluentQueryBuilder<T1, T2, T3, T4, T5> where T1 : unmanaged where T2 : unmanaged where T3 : unmanaged where T4 : unmanaged where T5 : unmanaged
{
    private readonly FluentQueryBuilder _builder;
    internal FluentQueryBuilder(FluentQueryBuilder builder) { _builder = builder; }

    private Query GetQuery() => _builder.Registry.GetOrCreateQuery(
        _builder.WithIds, 
        _builder.WithoutIds, 
        [
            _builder.World.GetTypeId<T1>(),
            _builder.World.GetTypeId<T2>(),
            _builder.World.GetTypeId<T3>(),
            _builder.World.GetTypeId<T4>(),
            _builder.World.GetTypeId<T5>()
            ], 
        _builder.IsParallel
    );

    public void ForEach(ForEachAction<T1, T2, T3, T4, T5> action) => _builder.World.Iterate<T1, T2, T3, T4, T5>(GetQuery()).ForEach(action);
    public void ForEach(ForEachWithEntityAction<T1, T2, T3, T4, T5> action) => _builder.World.Iterate<T1, T2, T3, T4, T5>(GetQuery()).ForEach(action);

    public FluentQueryBuilder<T1, T2, T3, T4, T5> AsParallel() => new(_builder.AsParallel());

    public FluentQueryBuilder<T1, T2, T3, T4, T5> Without<T>() where T : unmanaged
    {
        var newWithout = ListPool<int>.Rent(_builder.WithoutIds);
        newWithout.Add(_builder.World.GetTypeId<T>());
        return new FluentQueryBuilder<T1, T2, T3, T4, T5>(new FluentQueryBuilder(_builder.World, _builder.Registry, _builder.WithIds, newWithout, _builder.IsParallel));
    }

    public FluentQueryBuilder<T1, T2, T3, T4, T5, T6> With<T6>() where T6 : unmanaged
    {
        var newWith = ListPool<int>.Rent(_builder.WithIds);
        newWith.Add(_builder.World.GetTypeId<T6>());
        return new FluentQueryBuilder<T1, T2, T3, T4, T5, T6>(new FluentQueryBuilder(_builder.World, _builder.Registry, newWith, _builder.WithoutIds, _builder.IsParallel));
    }
}

public readonly partial struct FluentQueryBuilder<T1, T2, T3, T4, T5, T6> where T1 : unmanaged where T2 : unmanaged where T3 : unmanaged where T4 : unmanaged where T5 : unmanaged where T6 : unmanaged
{
    private readonly FluentQueryBuilder _builder;
    internal FluentQueryBuilder(FluentQueryBuilder builder) { _builder = builder; }

    private Query GetQuery() => _builder.Registry.GetOrCreateQuery(
        _builder.WithIds, 
        _builder.WithoutIds, 
        [
            _builder.World.GetTypeId<T1>(), 
            _builder.World.GetTypeId<T2>(), 
            _builder.World.GetTypeId<T3>(), 
            _builder.World.GetTypeId<T4>(), 
            _builder.World.GetTypeId<T5>(), 
            _builder.World.GetTypeId<T6>()
        ], 
        _builder.IsParallel
    );

    public void ForEach(ForEachAction<T1, T2, T3, T4, T5, T6> action) => _builder.World.Iterate<T1, T2, T3, T4, T5, T6>(GetQuery()).ForEach(action);
    public void ForEach(ForEachWithEntityAction<T1, T2, T3, T4, T5, T6> action) => _builder.World.Iterate<T1, T2, T3, T4, T5, T6>(GetQuery()).ForEach(action);

    public FluentQueryBuilder<T1, T2, T3, T4, T5, T6> AsParallel() => new(_builder.AsParallel());

    public FluentQueryBuilder<T1, T2, T3, T4, T5, T6> Without<T>() where T : unmanaged
    {
        var newWithout = ListPool<int>.Rent(_builder.WithoutIds);
        newWithout.Add(_builder.World.GetTypeId<T>());
        return new FluentQueryBuilder<T1, T2, T3, T4, T5, T6>(new FluentQueryBuilder(_builder.World, _builder.Registry, _builder.WithIds, newWithout, _builder.IsParallel));
    }

    public FluentQueryBuilder<T1, T2, T3, T4, T5, T6, T7> With<T7>() where T7 : unmanaged
    {
        var newWith = ListPool<int>.Rent(_builder.WithIds);
        newWith.Add(_builder.World.GetTypeId<T7>());
        return new FluentQueryBuilder<T1, T2, T3, T4, T5, T6, T7>(new FluentQueryBuilder(_builder.World, _builder.Registry, newWith, _builder.WithoutIds, _builder.IsParallel));
    }
}

public readonly partial struct FluentQueryBuilder<T1, T2, T3, T4, T5, T6, T7> where T1 : unmanaged where T2 : unmanaged where T3 : unmanaged where T4 : unmanaged where T5 : unmanaged where T6 : unmanaged where T7 : unmanaged
{
    private readonly FluentQueryBuilder _builder;
    internal FluentQueryBuilder(FluentQueryBuilder builder) { _builder = builder; }

    private Query GetQuery() => _builder.Registry.GetOrCreateQuery(
        _builder.WithIds,
        _builder.WithoutIds,
        [
            _builder.World.GetTypeId<T1>(),
            _builder.World.GetTypeId<T2>(),
            _builder.World.GetTypeId<T3>(),
            _builder.World.GetTypeId<T4>(),
            _builder.World.GetTypeId<T5>(),
            _builder.World.GetTypeId<T6>(),
            _builder.World.GetTypeId<T7>()
        ],
        _builder.IsParallel
    );

    public void ForEach(ForEachAction<T1, T2, T3, T4, T5, T6, T7> action) => _builder.World.Iterate<T1, T2, T3, T4, T5, T6, T7>(GetQuery()).ForEach(action);
    public void ForEach(ForEachWithEntityAction<T1, T2, T3, T4, T5, T6, T7> action) => _builder.World.Iterate<T1, T2, T3, T4, T5, T6, T7>(GetQuery()).ForEach(action);

    public FluentQueryBuilder<T1, T2, T3, T4, T5, T6, T7> AsParallel() => new(_builder.AsParallel());

    public FluentQueryBuilder<T1, T2, T3, T4, T5, T6, T7> Without<T>() where T : unmanaged
    {
        var newWithout = ListPool<int>.Rent(_builder.WithoutIds);
        newWithout.Add(_builder.World.GetTypeId<T>());
        return new FluentQueryBuilder<T1, T2, T3, T4, T5, T6, T7>(new FluentQueryBuilder(_builder.World, _builder.Registry, _builder.WithIds, newWithout, _builder.IsParallel));
    }

    public FluentQueryBuilder<T1, T2, T3, T4, T5, T6, T7, T8> With<T8>() where T8 : unmanaged
    {
        var newWith = ListPool<int>.Rent(_builder.WithIds);
        newWith.Add(_builder.World.GetTypeId<T8>());
        return new FluentQueryBuilder<T1, T2, T3, T4, T5, T6, T7, T8>(new FluentQueryBuilder(_builder.World, _builder.Registry, newWith, _builder.WithoutIds, _builder.IsParallel));
    }
}

public readonly partial struct FluentQueryBuilder<T1, T2, T3, T4, T5, T6, T7, T8> where T1 : unmanaged where T2 : unmanaged where T3 : unmanaged where T4 : unmanaged where T5 : unmanaged where T6 : unmanaged where T7 : unmanaged where T8 : unmanaged
{
    private readonly FluentQueryBuilder _builder;
    internal FluentQueryBuilder(FluentQueryBuilder builder) { _builder = builder; }

    private Query GetQuery() => _builder.Registry.GetOrCreateQuery(
        _builder.WithIds,
        _builder.WithoutIds,
        [
            _builder.World.GetTypeId<T1>(),
            _builder.World.GetTypeId<T2>(),
            _builder.World.GetTypeId<T3>(),
            _builder.World.GetTypeId<T4>(),
            _builder.World.GetTypeId<T5>(),
            _builder.World.GetTypeId<T6>(),
            _builder.World.GetTypeId<T7>(),
            _builder.World.GetTypeId<T8>()
        ],
        _builder.IsParallel);

    public void ForEach(ForEachAction<T1, T2, T3, T4, T5, T6, T7, T8> action) => _builder.World.Iterate<T1, T2, T3, T4, T5, T6, T7, T8>(GetQuery()).ForEach(action);
    public void ForEach(ForEachWithEntityAction<T1, T2, T3, T4, T5, T6, T7, T8> action) => _builder.World.Iterate<T1, T2, T3, T4, T5, T6, T7, T8>(GetQuery()).ForEach(action);

    public FluentQueryBuilder<T1, T2, T3, T4, T5, T6, T7, T8> AsParallel() => new(_builder.AsParallel());

    public FluentQueryBuilder<T1, T2, T3, T4, T5, T6, T7, T8> Without<T>() where T : unmanaged
    {
        var newWithout = ListPool<int>.Rent(_builder.WithoutIds);
        newWithout.Add(_builder.World.GetTypeId<T>());
        return new FluentQueryBuilder<T1, T2, T3, T4, T5, T6, T7, T8>(new FluentQueryBuilder(_builder.World, _builder.Registry, _builder.WithIds, newWithout, _builder.IsParallel));
    }
}