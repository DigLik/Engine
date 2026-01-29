namespace Engine.Base.DataStructures;

public sealed class SparseSet<T>(int initialCapacity = 64)
{
    private uint[] _sparse = new uint[initialCapacity];
    private uint[] _dense = new uint[initialCapacity];
    private T[] _values = new T[initialCapacity];

    public int Count { get; private set; }

    public ReadOnlySpan<T> Values => new(_values, 0, Count);

    public void Add(uint key, T value)
    {
        if (Contains(key))
        {
            var denseIndex = _sparse[key];
            _values[denseIndex] = value;
        }
        else
        {
            EnsureCapacity(key);

            var denseIndex = (uint)Count;
            _sparse[key] = denseIndex;
            _dense[denseIndex] = key;
            _values[denseIndex] = value;
            Count++;
        }
    }

    public bool Remove(uint key)
    {
        if (!Contains(key))
            return false;

        var denseIndexToRemove = _sparse[key];
        var lastDenseIndex = (uint)Count - 1;
        var lastKey = _dense[lastDenseIndex];

        if (denseIndexToRemove != lastDenseIndex)
        {
            _dense[denseIndexToRemove] = lastKey;
            _values[denseIndexToRemove] = _values[lastDenseIndex];
            _sparse[lastKey] = denseIndexToRemove;
        }

        Count--;
        return true;
    }

    public ref T GetRef(uint key)
    {
        if (!Contains(key))
            throw new KeyNotFoundException($"Key {key} not found.");
        return ref _values[_sparse[key]];
    }

    public bool Contains(uint key)
        => key < _sparse.Length && _sparse[key] < Count && _dense[_sparse[key]] == key;

    private void EnsureCapacity(uint key)
    {
        if (key >= _sparse.Length)
            Array.Resize(ref _sparse, (int)Math.Max((uint)_sparse.Length << 1, key + 1));

        if (Count >= _dense.Length)
        {
            var newSize = _dense.Length << 1;
            Array.Resize(ref _dense, newSize);
            Array.Resize(ref _values, newSize);
        }
    }

    public void Clear() => Count = 0;
}