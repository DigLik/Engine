using Engine.DataStructures;
using Engine.ECS.Archetypes.Model;

namespace Engine.ECS.Archetypes;

public sealed class ArchetypeRegistry(int chunkCapacity = 256) : IDisposable
{
    private readonly Dictionary<ArchetypeKey, Archetype> _byKey = [];
    private readonly Dictionary<int, List<Archetype>> _typeToArch = [];
    private readonly Dictionary<int, Func<int, Column>> _columnFactories = [];
    private int _nextArchetypeId = 0;

    public int Version { get; private set; }

    public int GetTypeId<T>() where T : unmanaged
    {
        int id = TypeIndex.Get<T>();
        if (!_columnFactories.ContainsKey(id))
        {
            _columnFactories[id] = (capacity) => {
                var col = new Column<T>(id, 0);
                col.Initialize(capacity);
                return col;
            };
        }
        return id;
    }

    public Archetype GetOrCreate(TypeMask mask, ReadOnlySpan<int> typeIds)
    {
        var key = new ArchetypeKey(mask);
        if (_byKey.TryGetValue(key, out var a)) return a;

        var arch = new Archetype(_nextArchetypeId++, key, typeIds, chunkCapacity);
        _byKey[key] = arch;

        foreach (var t in typeIds)
        {
            if (!_typeToArch.TryGetValue(t, out var lst))
            {
                lst = [];
                _typeToArch[t] = lst;
            }
            lst.Add(arch);
        }

        Version++;
        return arch;
    }

    public Func<int, Column> GetOrCreateColumnFactory(int typeId)
    {
        return _columnFactories.TryGetValue(typeId, out var factory)
            ? factory
            : throw new InvalidOperationException(
                $"No column factory registered for type ID {typeId}. Was GetTypeId<T>() called for the corresponding component type?"
            );
    }

    public void SelectCandidates(ReadOnlySpan<int> required, List<Archetype> results)
    {
        results.Clear();

        if (required.Length == 0)
            return;

        var requiredLists = new List<List<Archetype>>(required.Length);
        foreach (var typeId in required)
        {
            if (!_typeToArch.TryGetValue(typeId, out var archetypes))
                return;
            requiredLists.Add(archetypes);
        }

        requiredLists.Sort((a, b) => a.Count.CompareTo(b.Count));

        var smallestList = requiredLists[0];

        if (requiredLists.Count == 1)
        {
            results.AddRange(smallestList);
            return;
        }

        var iterators = new int[requiredLists.Count - 1];

        foreach (var candidate in smallestList)
        {
            bool isMatch = true;

            for (int i = 0; i < iterators.Length; i++)
            {
                var otherList = requiredLists[i + 1];
                ref int iterator = ref iterators[i];

                while (iterator < otherList.Count && otherList[iterator].Id < candidate.Id)
                    iterator++;

                if (iterator >= otherList.Count || otherList[iterator].Id != candidate.Id)
                    isMatch = false;
            }

            if (isMatch)
                results.Add(candidate);
        }
    }

    public IEnumerable<Archetype> GetAllArchetypes() => _byKey.Values;

    public void Dispose()
    {
        foreach (var archetype in _byKey.Values)
        {
            archetype.Dispose();
        }
        Clear();
    }

    public void Clear()
    {
        _byKey.Clear();
        _typeToArch.Clear();
        _nextArchetypeId = 0;
        Version = 0;
    }
}