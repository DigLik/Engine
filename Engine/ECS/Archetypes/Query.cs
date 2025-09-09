namespace Engine.ECS.Archetypes;

public sealed class Query
{
    private readonly ArchetypeRegistry _registry;
    private readonly QueryDescription _description;

    private readonly int[] _originalRequiredTypeIds;
    private readonly int[] _sortedRequiredTypeIds;

    private readonly List<ArchetypeMatch> _matchedArchetypes = [];
    private readonly List<Archetype> _candidateArchetypesCache = [];
    private int _cachedVersion = -1;

    public readonly record struct ArchetypeMatch(Archetype Archetype, int[] ColumnIndices);

    internal Query(ArchetypeRegistry registry, QueryDescription description, ReadOnlySpan<int> requiredTypeIds)
    {
        _registry = registry;
        _description = description;

        _originalRequiredTypeIds = requiredTypeIds.ToArray();

        var sortedIds = requiredTypeIds.ToArray();
        Array.Sort(sortedIds);
        _sortedRequiredTypeIds = sortedIds;
    }

    internal IReadOnlyList<ArchetypeMatch> GetMatches()
    {
        if (_cachedVersion == _registry.Version)
        {
            return _matchedArchetypes;
        }

        _matchedArchetypes.Clear();
        _registry.SelectCandidates(_sortedRequiredTypeIds, _candidateArchetypesCache);

        foreach (var arch in _candidateArchetypesCache)
        {
            if (arch.Key.Mask.ContainsAll(_description.WithMask) &&
                !arch.Key.Mask.ContainsAny(_description.WithoutMask))
            {
                var columnIndices = new int[_originalRequiredTypeIds.Length];
                for (int i = 0; i < _originalRequiredTypeIds.Length; i++)
                {
                    if (!arch.TryGetColumnIndex(_originalRequiredTypeIds[i], out columnIndices[i]))
                    {
                        throw new InvalidOperationException("Inconsistency in archetype data: required component not found in selected archetype.");
                    }
                }
                _matchedArchetypes.Add(new ArchetypeMatch(arch, columnIndices));
            }
        }

        _cachedVersion = _registry.Version;
        return _matchedArchetypes;
    }
}