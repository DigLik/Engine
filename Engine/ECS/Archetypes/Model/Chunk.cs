namespace Engine.ECS.Archetypes.Model;

public sealed class Chunk(int capacity, Column[] columns) : IDisposable
{
    public readonly int Capacity = capacity;
    public readonly Entity[] Entities = new Entity[capacity];
    public readonly Column[] Columns = columns;

    public int Count { get; private set; }
    public bool HasSpace => Count < Capacity;

    public int AddEntity(Entity e)
    {
        var row = Count++;
        Entities[row] = e;
        return row;
    }

    public int RemoveAtSwapBack(int row)
    {
        var last = Count - 1;
        if (row < 0 || row >= Count) throw new IndexOutOfRangeException();

        if (row != last)
        {
            for (var c = 0; c < Columns.Length; c++)
                Columns[c].MoveFrom(Columns[c], last, row);

            Entities[row] = Entities[last];
        }

        for (var c = 0; c < Columns.Length; c++)
            Columns[c].SetDefault(last);

        Count--;
        return last;
    }

    public void Dispose()
    {
        foreach (var column in Columns)
            column.Dispose();
    }
}