using Engine.ECS.Archetypes.Model;
using Engine.ECS.Archetypes.QueryDefinition;

namespace Engine.ECS.Querying;

public readonly ref struct QueryIterator<T1> where T1 : unmanaged
{
    private readonly Query _query;
    internal QueryIterator(Query query) { _query = query; }

    public void ForEach(ForEachAction<T1> action)
    {
        foreach (var match in _query.GetMatches())
        {
            var colIdx1 = match.ColumnIndices[0];
            var chunks = match.Archetype.Chunks;
            for (var i = 0; i < chunks.Count; i++)
            {
                var chunk = chunks[i];
                var c1 = (Column<T1>)chunk.Columns[colIdx1];
                for (var row = 0; row < chunk.Count; row++)
                    action(ref c1.Ref(row));
            }
        }
    }

    public void ForEach(ForEachWithEntityAction<T1> action)
    {
        foreach (var match in _query.GetMatches())
        {
            var colIdx1 = match.ColumnIndices[0];
            var chunks = match.Archetype.Chunks;
            for (var i = 0; i < chunks.Count; i++)
            {
                var chunk = chunks[i];
                var c1 = (Column<T1>)chunk.Columns[colIdx1];
                for (var row = 0; row < chunk.Count; row++)
                    action(chunk.Entities[row], ref c1.Ref(row));
            }
        }
    }
}

public readonly ref struct QueryIterator<T1, T2> where T1 : unmanaged where T2 : unmanaged
{
    private readonly Query _query;
    internal QueryIterator(Query query) { _query = query; }

    public void ForEach(ForEachAction<T1, T2> action)
    {
        foreach (var match in _query.GetMatches())
        {
            var colIdx1 = match.ColumnIndices[0];
            var colIdx2 = match.ColumnIndices[1];
            var chunks = match.Archetype.Chunks;
            for (var i = 0; i < chunks.Count; i++)
            {
                var chunk = chunks[i];
                var c1 = (Column<T1>)chunk.Columns[colIdx1];
                var c2 = (Column<T2>)chunk.Columns[colIdx2];
                for (var row = 0; row < chunk.Count; row++)
                    action(ref c1.Ref(row), ref c2.Ref(row));
            }
        }
    }

    public void ForEach(ForEachWithEntityAction<T1, T2> action)
    {
        foreach (var match in _query.GetMatches())
        {
            var colIdx1 = match.ColumnIndices[0];
            var colIdx2 = match.ColumnIndices[1];
            var chunks = match.Archetype.Chunks;
            for (var i = 0; i < chunks.Count; i++)
            {
                var chunk = chunks[i];
                var c1 = (Column<T1>)chunk.Columns[colIdx1];
                var c2 = (Column<T2>)chunk.Columns[colIdx2];
                for (var row = 0; row < chunk.Count; row++)
                    action(chunk.Entities[row], ref c1.Ref(row), ref c2.Ref(row));
            }
        }
    }
}

public readonly ref struct QueryIterator<T1, T2, T3> where T1 : unmanaged where T2 : unmanaged where T3 : unmanaged
{
    private readonly Query _query;
    internal QueryIterator(Query query) { _query = query; }

    public void ForEach(ForEachAction<T1, T2, T3> action)
    {
        foreach (var match in _query.GetMatches())
        {
            var colIdx1 = match.ColumnIndices[0];
            var colIdx2 = match.ColumnIndices[1];
            var colIdx3 = match.ColumnIndices[2];
            var chunks = match.Archetype.Chunks;
            for (var i = 0; i < chunks.Count; i++)
            {
                var chunk = chunks[i];
                var c1 = (Column<T1>)chunk.Columns[colIdx1];
                var c2 = (Column<T2>)chunk.Columns[colIdx2];
                var c3 = (Column<T3>)chunk.Columns[colIdx3];
                for (var row = 0; row < chunk.Count; row++)
                    action(ref c1.Ref(row), ref c2.Ref(row), ref c3.Ref(row));
            }
        }
    }

    public void ForEach(ForEachWithEntityAction<T1, T2, T3> action)
    {
        foreach (var match in _query.GetMatches())
        {
            var colIdx1 = match.ColumnIndices[0];
            var colIdx2 = match.ColumnIndices[1];
            var colIdx3 = match.ColumnIndices[2];
            var chunks = match.Archetype.Chunks;
            for (var i = 0; i < chunks.Count; i++)
            {
                var chunk = chunks[i];
                var c1 = (Column<T1>)chunk.Columns[colIdx1];
                var c2 = (Column<T2>)chunk.Columns[colIdx2];
                var c3 = (Column<T3>)chunk.Columns[colIdx3];
                for (var row = 0; row < chunk.Count; row++)
                    action(chunk.Entities[row], ref c1.Ref(row), ref c2.Ref(row), ref c3.Ref(row));
            }
        }
    }
}

public readonly ref struct QueryIterator<T1, T2, T3, T4> where T1 : unmanaged where T2 : unmanaged where T3 : unmanaged where T4 : unmanaged
{
    private readonly Query _query;
    internal QueryIterator(Query query) { _query = query; }

    public void ForEach(ForEachAction<T1, T2, T3, T4> action)
    {
        foreach (var match in _query.GetMatches())
        {
            var colIdx1 = match.ColumnIndices[0];
            var colIdx2 = match.ColumnIndices[1];
            var colIdx3 = match.ColumnIndices[2];
            var colIdx4 = match.ColumnIndices[3];
            var chunks = match.Archetype.Chunks;
            for (var i = 0; i < chunks.Count; i++)
            {
                var chunk = chunks[i];
                var c1 = (Column<T1>)chunk.Columns[colIdx1];
                var c2 = (Column<T2>)chunk.Columns[colIdx2];
                var c3 = (Column<T3>)chunk.Columns[colIdx3];
                var c4 = (Column<T4>)chunk.Columns[colIdx4];
                for (var row = 0; row < chunk.Count; row++)
                    action(ref c1.Ref(row), ref c2.Ref(row), ref c3.Ref(row), ref c4.Ref(row));
            }
        }
    }

    public void ForEach(ForEachWithEntityAction<T1, T2, T3, T4> action)
    {
        foreach (var match in _query.GetMatches())
        {
            var colIdx1 = match.ColumnIndices[0];
            var colIdx2 = match.ColumnIndices[1];
            var colIdx3 = match.ColumnIndices[2];
            var colIdx4 = match.ColumnIndices[3];
            var chunks = match.Archetype.Chunks;
            for (var i = 0; i < chunks.Count; i++)
            {
                var chunk = chunks[i];
                var c1 = (Column<T1>)chunk.Columns[colIdx1];
                var c2 = (Column<T2>)chunk.Columns[colIdx2];
                var c3 = (Column<T3>)chunk.Columns[colIdx3];
                var c4 = (Column<T4>)chunk.Columns[colIdx4];
                for (var row = 0; row < chunk.Count; row++)
                    action(chunk.Entities[row], ref c1.Ref(row), ref c2.Ref(row), ref c3.Ref(row), ref c4.Ref(row));
            }
        }
    }
}

public readonly ref struct QueryIterator<T1, T2, T3, T4, T5> where T1 : unmanaged where T2 : unmanaged where T3 : unmanaged where T4 : unmanaged where T5 : unmanaged
{
    private readonly Query _query;
    internal QueryIterator(Query query) { _query = query; }

    public void ForEach(ForEachAction<T1, T2, T3, T4, T5> action)
    {
        foreach (var match in _query.GetMatches())
        {
            var colIdx1 = match.ColumnIndices[0];
            var colIdx2 = match.ColumnIndices[1];
            var colIdx3 = match.ColumnIndices[2];
            var colIdx4 = match.ColumnIndices[3];
            var colIdx5 = match.ColumnIndices[4];
            var chunks = match.Archetype.Chunks;
            for (var i = 0; i < chunks.Count; i++)
            {
                var chunk = chunks[i];
                var c1 = (Column<T1>)chunk.Columns[colIdx1];
                var c2 = (Column<T2>)chunk.Columns[colIdx2];
                var c3 = (Column<T3>)chunk.Columns[colIdx3];
                var c4 = (Column<T4>)chunk.Columns[colIdx4];
                var c5 = (Column<T5>)chunk.Columns[colIdx5];
                for (var row = 0; row < chunk.Count; row++)
                    action(ref c1.Ref(row), ref c2.Ref(row), ref c3.Ref(row), ref c4.Ref(row), ref c5.Ref(row));
            }
        }
    }

    public void ForEach(ForEachWithEntityAction<T1, T2, T3, T4, T5> action)
    {
        foreach (var match in _query.GetMatches())
        {
            var colIdx1 = match.ColumnIndices[0];
            var colIdx2 = match.ColumnIndices[1];
            var colIdx3 = match.ColumnIndices[2];
            var colIdx4 = match.ColumnIndices[3];
            var colIdx5 = match.ColumnIndices[4];
            var chunks = match.Archetype.Chunks;
            for (var i = 0; i < chunks.Count; i++)
            {
                var chunk = chunks[i];
                var c1 = (Column<T1>)chunk.Columns[colIdx1];
                var c2 = (Column<T2>)chunk.Columns[colIdx2];
                var c3 = (Column<T3>)chunk.Columns[colIdx3];
                var c4 = (Column<T4>)chunk.Columns[colIdx4];
                var c5 = (Column<T5>)chunk.Columns[colIdx5];
                for (var row = 0; row < chunk.Count; row++)
                    action(chunk.Entities[row], ref c1.Ref(row), ref c2.Ref(row), ref c3.Ref(row), ref c4.Ref(row), ref c5.Ref(row));
            }
        }
    }
}

public readonly ref struct QueryIterator<T1, T2, T3, T4, T5, T6> where T1 : unmanaged where T2 : unmanaged where T3 : unmanaged where T4 : unmanaged where T5 : unmanaged where T6 : unmanaged
{
    private readonly Query _query;
    internal QueryIterator(Query query) { _query = query; }

    public void ForEach(ForEachAction<T1, T2, T3, T4, T5, T6> action)
    {
        foreach (var match in _query.GetMatches())
        {
            var colIdx1 = match.ColumnIndices[0];
            var colIdx2 = match.ColumnIndices[1];
            var colIdx3 = match.ColumnIndices[2];
            var colIdx4 = match.ColumnIndices[3];
            var colIdx5 = match.ColumnIndices[4];
            var colIdx6 = match.ColumnIndices[5];
            var chunks = match.Archetype.Chunks;
            for (var i = 0; i < chunks.Count; i++)
            {
                var chunk = chunks[i];
                var c1 = (Column<T1>)chunk.Columns[colIdx1];
                var c2 = (Column<T2>)chunk.Columns[colIdx2];
                var c3 = (Column<T3>)chunk.Columns[colIdx3];
                var c4 = (Column<T4>)chunk.Columns[colIdx4];
                var c5 = (Column<T5>)chunk.Columns[colIdx5];
                var c6 = (Column<T6>)chunk.Columns[colIdx6];
                for (var row = 0; row < chunk.Count; row++)
                    action(ref c1.Ref(row), ref c2.Ref(row), ref c3.Ref(row), ref c4.Ref(row), ref c5.Ref(row), ref c6.Ref(row));
            }
        }
    }

    public void ForEach(ForEachWithEntityAction<T1, T2, T3, T4, T5, T6> action)
    {
        foreach (var match in _query.GetMatches())
        {
            var colIdx1 = match.ColumnIndices[0];
            var colIdx2 = match.ColumnIndices[1];
            var colIdx3 = match.ColumnIndices[2];
            var colIdx4 = match.ColumnIndices[3];
            var colIdx5 = match.ColumnIndices[4];
            var colIdx6 = match.ColumnIndices[5];
            var chunks = match.Archetype.Chunks;
            for (var i = 0; i < chunks.Count; i++)
            {
                var chunk = chunks[i];
                var c1 = (Column<T1>)chunk.Columns[colIdx1];
                var c2 = (Column<T2>)chunk.Columns[colIdx2];
                var c3 = (Column<T3>)chunk.Columns[colIdx3];
                var c4 = (Column<T4>)chunk.Columns[colIdx4];
                var c5 = (Column<T5>)chunk.Columns[colIdx5];
                var c6 = (Column<T6>)chunk.Columns[colIdx6];
                for (var row = 0; row < chunk.Count; row++)
                    action(chunk.Entities[row], ref c1.Ref(row), ref c2.Ref(row), ref c3.Ref(row), ref c4.Ref(row), ref c5.Ref(row), ref c6.Ref(row));
            }
        }
    }
}

public readonly ref struct QueryIterator<T1, T2, T3, T4, T5, T6, T7> where T1 : unmanaged where T2 : unmanaged where T3 : unmanaged where T4 : unmanaged where T5 : unmanaged where T6 : unmanaged where T7 : unmanaged
{
    private readonly Query _query;
    internal QueryIterator(Query query) { _query = query; }

    public void ForEach(ForEachAction<T1, T2, T3, T4, T5, T6, T7> action)
    {
        foreach (var match in _query.GetMatches())
        {
            var colIdx1 = match.ColumnIndices[0];
            var colIdx2 = match.ColumnIndices[1];
            var colIdx3 = match.ColumnIndices[2];
            var colIdx4 = match.ColumnIndices[3];
            var colIdx5 = match.ColumnIndices[4];
            var colIdx6 = match.ColumnIndices[5];
            var colIdx7 = match.ColumnIndices[6];
            var chunks = match.Archetype.Chunks;
            for (var i = 0; i < chunks.Count; i++)
            {
                var chunk = chunks[i];
                var c1 = (Column<T1>)chunk.Columns[colIdx1];
                var c2 = (Column<T2>)chunk.Columns[colIdx2];
                var c3 = (Column<T3>)chunk.Columns[colIdx3];
                var c4 = (Column<T4>)chunk.Columns[colIdx4];
                var c5 = (Column<T5>)chunk.Columns[colIdx5];
                var c6 = (Column<T6>)chunk.Columns[colIdx6];
                var c7 = (Column<T7>)chunk.Columns[colIdx7];
                for (var row = 0; row < chunk.Count; row++)
                    action(ref c1.Ref(row), ref c2.Ref(row), ref c3.Ref(row), ref c4.Ref(row), ref c5.Ref(row), ref c6.Ref(row), ref c7.Ref(row));
            }
        }
    }

    public void ForEach(ForEachWithEntityAction<T1, T2, T3, T4, T5, T6, T7> action)
    {
        foreach (var match in _query.GetMatches())
        {
            var colIdx1 = match.ColumnIndices[0];
            var colIdx2 = match.ColumnIndices[1];
            var colIdx3 = match.ColumnIndices[2];
            var colIdx4 = match.ColumnIndices[3];
            var colIdx5 = match.ColumnIndices[4];
            var colIdx6 = match.ColumnIndices[5];
            var colIdx7 = match.ColumnIndices[6];
            var chunks = match.Archetype.Chunks;
            for (var i = 0; i < chunks.Count; i++)
            {
                var chunk = chunks[i];
                var c1 = (Column<T1>)chunk.Columns[colIdx1];
                var c2 = (Column<T2>)chunk.Columns[colIdx2];
                var c3 = (Column<T3>)chunk.Columns[colIdx3];
                var c4 = (Column<T4>)chunk.Columns[colIdx4];
                var c5 = (Column<T5>)chunk.Columns[colIdx5];
                var c6 = (Column<T6>)chunk.Columns[colIdx6];
                var c7 = (Column<T7>)chunk.Columns[colIdx7];
                for (var row = 0; row < chunk.Count; row++)
                    action(chunk.Entities[row], ref c1.Ref(row), ref c2.Ref(row), ref c3.Ref(row), ref c4.Ref(row), ref c5.Ref(row), ref c6.Ref(row), ref c7.Ref(row));
            }
        }
    }
}

public readonly ref struct QueryIterator<T1, T2, T3, T4, T5, T6, T7, T8> where T1 : unmanaged where T2 : unmanaged where T3 : unmanaged where T4 : unmanaged where T5 : unmanaged where T6 : unmanaged where T7 : unmanaged where T8 : unmanaged
{
    private readonly Query _query;
    internal QueryIterator(Query query) { _query = query; }

    public void ForEach(ForEachAction<T1, T2, T3, T4, T5, T6, T7, T8> action)
    {
        foreach (var match in _query.GetMatches())
        {
            var colIdx1 = match.ColumnIndices[0];
            var colIdx2 = match.ColumnIndices[1];
            var colIdx3 = match.ColumnIndices[2];
            var colIdx4 = match.ColumnIndices[3];
            var colIdx5 = match.ColumnIndices[4];
            var colIdx6 = match.ColumnIndices[5];
            var colIdx7 = match.ColumnIndices[6];
            var colIdx8 = match.ColumnIndices[7];
            var chunks = match.Archetype.Chunks;
            for (var i = 0; i < chunks.Count; i++)
            {
                var chunk = chunks[i];
                var c1 = (Column<T1>)chunk.Columns[colIdx1];
                var c2 = (Column<T2>)chunk.Columns[colIdx2];
                var c3 = (Column<T3>)chunk.Columns[colIdx3];
                var c4 = (Column<T4>)chunk.Columns[colIdx4];
                var c5 = (Column<T5>)chunk.Columns[colIdx5];
                var c6 = (Column<T6>)chunk.Columns[colIdx6];
                var c7 = (Column<T7>)chunk.Columns[colIdx7];
                var c8 = (Column<T8>)chunk.Columns[colIdx8];
                for (var row = 0; row < chunk.Count; row++)
                    action(ref c1.Ref(row), ref c2.Ref(row), ref c3.Ref(row), ref c4.Ref(row), ref c5.Ref(row), ref c6.Ref(row), ref c7.Ref(row), ref c8.Ref(row));
            }
        }
    }

    public void ForEach(ForEachWithEntityAction<T1, T2, T3, T4, T5, T6, T7, T8> action)
    {
        foreach (var match in _query.GetMatches())
        {
            var colIdx1 = match.ColumnIndices[0];
            var colIdx2 = match.ColumnIndices[1];
            var colIdx3 = match.ColumnIndices[2];
            var colIdx4 = match.ColumnIndices[3];
            var colIdx5 = match.ColumnIndices[4];
            var colIdx6 = match.ColumnIndices[5];
            var colIdx7 = match.ColumnIndices[6];
            var colIdx8 = match.ColumnIndices[7];
            var chunks = match.Archetype.Chunks;
            for (var i = 0; i < chunks.Count; i++)
            {
                var chunk = chunks[i];
                var c1 = (Column<T1>)chunk.Columns[colIdx1];
                var c2 = (Column<T2>)chunk.Columns[colIdx2];
                var c3 = (Column<T3>)chunk.Columns[colIdx3];
                var c4 = (Column<T4>)chunk.Columns[colIdx4];
                var c5 = (Column<T5>)chunk.Columns[colIdx5];
                var c6 = (Column<T6>)chunk.Columns[colIdx6];
                var c7 = (Column<T7>)chunk.Columns[colIdx7];
                var c8 = (Column<T8>)chunk.Columns[colIdx8];
                for (var row = 0; row < chunk.Count; row++)
                    action(chunk.Entities[row], ref c1.Ref(row), ref c2.Ref(row), ref c3.Ref(row), ref c4.Ref(row), ref c5.Ref(row), ref c6.Ref(row), ref c7.Ref(row), ref c8.Ref(row));
            }
        }
    }
}