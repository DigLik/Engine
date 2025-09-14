namespace Engine.ECS.Archetypes;

public sealed class Archetype : IDisposable
{
    public readonly int Id;
    public readonly ArchetypeKey Key;
    private readonly Dictionary<int, int> _typeToColumn = [];
    private readonly Column[] _columnsTemplate;
    private readonly List<Chunk> _chunks = [];
    private readonly int _chunkCapacity;

    internal readonly Dictionary<int, Archetype> AddTransitions = [];
    internal readonly Dictionary<int, Archetype> RemoveTransitions = [];

    public IReadOnlyList<Chunk> Chunks => _chunks;

    public Archetype(int id, ArchetypeKey key, ReadOnlySpan<int> typeIds, int chunkCapacity = 256)
    {
        Id = id;
        Key = key;
        _chunkCapacity = chunkCapacity;

        _columnsTemplate = new Column[typeIds.Length];
        for (int i = 0; i < typeIds.Length; i++)
        {
            int tId = typeIds[i];
            _typeToColumn[tId] = i;
            _columnsTemplate[i] = new ColumnStub(tId);
        }
    }

    private sealed class ColumnStub(int typeId) : Column(typeId)
    {
        public override void MoveFrom(Column src, int srcIndex, int dstIndex) => throw new NotSupportedException();
        public override void SetDefault(int index) => throw new NotSupportedException();
        public override void Dispose() { }
    }

    public Chunk GetOrCreateWritableChunk(ArchetypeRegistry registry)
    {
        if (_chunks.Count > 0 && _chunks[^1].HasSpace) return _chunks[^1];

        var realColumns = new Column[_columnsTemplate.Length];
        for (int i = 0; i < realColumns.Length; i++)
        {
            var template = _columnsTemplate[i];
            var factory = registry.GetOrCreateColumnFactory(template.TypeId);
            realColumns[i] = factory(_chunkCapacity);
        }

        var chunk = new Chunk(_chunkCapacity, realColumns);
        _chunks.Add(chunk);
        return chunk;
    }

    public bool TryGetColumnIndex(int typeId, out int idx) => _typeToColumn.TryGetValue(typeId, out idx);

    public void Dispose()
    {
        foreach (var chunk in _chunks)
            chunk.Dispose();

        _chunks.Clear();
    }
}