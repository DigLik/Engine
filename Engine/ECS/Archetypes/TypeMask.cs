using System.Runtime.InteropServices;

namespace Engine.ECS.Archetypes;

public sealed class TypeMask(int capacityTypes = 64) : IEquatable<TypeMask>
{
    private ulong[] _bits = new ulong[Math.Max(1, (capacityTypes + 63) >> 6)];
    private readonly List<int> _setIds = new(8);
    private int _hash;

    public ReadOnlySpan<int> SetIds => CollectionsMarshal.AsSpan(_setIds);

    public void Add(int typeId)
    {
        Ensure(typeId);
        ulong mask = 1UL << (typeId & 63);
        int block = typeId >> 6;
        if ((_bits[block] & mask) != 0) return;
        _bits[block] |= mask;
        InsertSorted(typeId);
        _hash = 0;
    }

    public void Remove(int typeId)
    {
        if ((uint)typeId >= (uint)(_bits.Length << 6)) return;
        int block = typeId >> 6;
        ulong mask = 1UL << (typeId & 63);
        if ((_bits[block] & mask) == 0) return;
        _bits[block] &= ~mask;
        int i = _setIds.BinarySearch(typeId);
        if (i >= 0) _setIds.RemoveAt(i);
        _hash = 0;
    }

    public bool Contains(int typeId)
        => (uint)typeId < (uint)(_bits.Length << 6) && ((_bits[typeId >> 6] >> (typeId & 63)) & 1UL) != 0;

    public bool ContainsAll(TypeMask other)
    {
        int n = System.Math.Max(_bits.Length, other._bits.Length);
        for (int i = 0; i < n; i++)
        {
            ulong a = i < _bits.Length ? _bits[i] : 0UL;
            ulong b = i < other._bits.Length ? other._bits[i] : 0UL;
            if ((a & b) != b) return false;
        }
        return true;
    }

    public bool ContainsAny(TypeMask other)
    {
        int n = Math.Min(_bits.Length, other._bits.Length);
        for (int i = 0; i < n; i++)
        {
            if ((_bits[i] & other._bits[i]) != 0) return true;
        }
        return false;
    }

    public TypeMask Clone()
    {
        var m = new TypeMask(1)
        {
            _bits = (ulong[])_bits.Clone()
        };

        m._setIds.AddRange(_setIds);

        if (_setIds.Count > 0)
            m.Ensure(_setIds[^1]);

        m._hash = _hash;
        return m;
    }

    public override int GetHashCode()
    {
        if (_hash != 0) return _hash;
        unchecked
        {
            int h = 17;
            for (int i = 0; i < _bits.Length; i++) h = h * 31 + _bits[i].GetHashCode();
            _hash = h == 0 ? 1 : h;
            return _hash;
        }
    }

    public bool Equals(TypeMask? other)
    {
        if (other is null) return false;
        int n = System.Math.Max(_bits.Length, other._bits.Length);
        for (int i = 0; i < n; i++)
        {
            ulong a = i < _bits.Length ? _bits[i] : 0UL;
            ulong b = i < other._bits.Length ? other._bits[i] : 0UL;
            if (a != b) return false;
        }
        return true;
    }

    private void Ensure(int typeId)
    {
        int need = (typeId >> 6) + 1;
        if (_bits.Length >= need) return;
        Array.Resize(ref _bits, System.Math.Max(_bits.Length << 1, need));
    }

    private void InsertSorted(int typeId)
    {
        int i = _setIds.BinarySearch(typeId);
        if (i < 0) i = ~i;
        _setIds.Insert(i, typeId);
    }

    public static bool operator ==(TypeMask? a, TypeMask? b)
        => ReferenceEquals(a, b) || (a is not null && a.Equals(b));
    public static bool operator !=(TypeMask? a, TypeMask? b) => !(a == b);

    public override bool Equals(object? obj) => obj is TypeMask m && Equals(m);
}