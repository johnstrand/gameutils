using System.Collections;
using System.Collections.Concurrent;

namespace GameUtils.Types.Collections;

/// <summary>
/// A thread-safe hash set.
/// </summary>
public class ConcurrentHashSet<T> : ISet<T> where T : notnull
{
    private readonly ConcurrentDictionary<T, byte> _dictionary = new();

    /// <summary>
    /// The number of elements in the set.
    /// </summary>
    public int Count => _dictionary.Count;

    /// <summary>
    /// Always false.
    /// </summary>
    public bool IsReadOnly => false;

    void ICollection<T>.Add(T item)
    {
        _dictionary.TryAdd(item, 0);
    }

    /// <summary>
    /// Clears the set.
    /// </summary>
    public void Clear()
    {
        _dictionary.Clear();
    }

    /// <summary>
    /// Returns true if the set contains the specified item.
    /// </summary>
    public bool Contains(T item)
    {
        return _dictionary.ContainsKey(item);
    }

    /// <summary>
    /// Copies the set to an array.
    /// </summary>
    public void CopyTo(T[] array, int arrayIndex)
    {
        _dictionary.Keys.CopyTo(array, arrayIndex);
    }

    /// <inheritdoc/>
    public void ExceptWith(IEnumerable<T> other)
    {
        ArgumentNullException.ThrowIfNull(other);

        if (_dictionary.IsEmpty)
        {
            return;
        }

        if (this == other)
        {
            _dictionary.Clear();
            return;
        }

        foreach (var item in other)
        {
            _dictionary.TryRemove(item, out _);
        }
    }

    /// <inheritdoc/>
    public IEnumerator<T> GetEnumerator()
    {
        return _dictionary.Keys.GetEnumerator();
    }

    /// <inheritdoc/>
    public void IntersectWith(IEnumerable<T> other)
    {
        ArgumentNullException.ThrowIfNull(other);

        if (_dictionary.IsEmpty)
        {
            return;
        }

        if (this == other)
        {
            return;
        }

        var keys = other.ToHashSet();

        foreach (var item in _dictionary.Keys)
        {
            if (!keys.Contains(item))
            {
                _dictionary.TryRemove(item, out _);
            }
        }
    }

    /// <inheritdoc/>
    public bool IsProperSubsetOf(IEnumerable<T> other)
    {
        return _dictionary.Keys.ToHashSet().IsProperSubsetOf(other);
    }

    /// <inheritdoc/>
    public bool IsProperSupersetOf(IEnumerable<T> other)
    {
        return _dictionary.Keys.ToHashSet().IsProperSupersetOf(other);
    }

    /// <inheritdoc/>
    public bool IsSubsetOf(IEnumerable<T> other)
    {
        return _dictionary.Keys.ToHashSet().IsSubsetOf(other);
    }

    /// <inheritdoc/>
    public bool IsSupersetOf(IEnumerable<T> other)
    {
        return _dictionary.Keys.ToHashSet().IsSupersetOf(other);
    }

    /// <inheritdoc/>
    public bool Overlaps(IEnumerable<T> other)
    {
        return _dictionary.Keys.ToHashSet().Overlaps(other);
    }

    /// <inheritdoc/>
    public bool Remove(T item)
    {
        return _dictionary.TryRemove(item, out _);
    }

    /// <inheritdoc/>
    public bool SetEquals(IEnumerable<T> other)
    {
        return _dictionary.Keys.ToHashSet().SetEquals(other);
    }

    /// <inheritdoc/>
    public void SymmetricExceptWith(IEnumerable<T> other)
    {
        var keys = _dictionary.Keys.ToHashSet();
        keys.SymmetricExceptWith(other);
        _dictionary.Clear();
        foreach (var item in keys)
        {
            _dictionary.TryAdd(item, 0);
        }
    }

    /// <inheritdoc/>
    public void UnionWith(IEnumerable<T> other)
    {
        var keys = _dictionary.Keys.ToHashSet();
        keys.UnionWith(other);
        _dictionary.Clear();
        foreach (var item in keys)
        {
            _dictionary.TryAdd(item, 0);
        }
    }

    bool ISet<T>.Add(T item)
    {
        return _dictionary.TryAdd(item, 0);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return _dictionary.Keys.GetEnumerator();
    }
}
