namespace Engine.DataStructures;

public class SparseSet<TValue>
{
    private uint[] _sparse = new uint[1024];
    private uint[] _dense = new uint[1024];
    private TValue[] _values = new TValue[1024];

    public int Count { get; private set; }

    public void Add(uint key, TValue value)
    {
        if (Contains(key))
        {
            var denseIndex = _sparse[key];
            _values[denseIndex] = value;
        }
        else
        {
            if (key >= _sparse.Length)
            {
                Array.Resize(ref _sparse, (int)Math.Max((uint)_sparse.Length << 1, key + 1));
            }

            if (Count >= _dense.Length)
            {
                var newSize = _dense.Length << 1;
                Array.Resize(ref _dense, newSize);
                Array.Resize(ref _values, newSize);
            }

            var denseIndex = (uint)Count;
            _sparse[key] = denseIndex;
            _dense[denseIndex] = key;
            _values[denseIndex] = value;
            Count++;
        }
    }

    public bool Remove(uint key)
    {
        if (!Contains(key)) return false;

        var denseIndexToRemove = _sparse[key];
        var lastDenseIndex = (uint)Count - 1;
        var lastKey = _dense[lastDenseIndex];

        if (denseIndexToRemove != lastDenseIndex)
        {
            _dense[denseIndexToRemove] = lastKey;
            _values[denseIndexToRemove] = _values[lastDenseIndex];
            _sparse[lastKey] = denseIndexToRemove;
        }

        _values[lastDenseIndex] = default!;

        Count--;
        return true;
    }

    public bool Contains(uint key)
    {
        return key < _sparse.Length && _sparse[key] < Count && _dense[_sparse[key]] == key;
    }

    public bool TryGetValue(uint key, out TValue value)
    {
        if (Contains(key))
        {
            value = _values[_sparse[key]];
            return true;
        }
        value = default!;
        return false;
    }

    public TValue this[uint key]
    {
        get
        {
            return TryGetValue(key, out var value)
                ? value
                : throw new KeyNotFoundException($"The key {key} was not present in the SparseSet.");
        }
        set => Add(key, value);
    }

    public void Clear()
    {
        Count = 0;
    }
}