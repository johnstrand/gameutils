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
#pragma warning disable S3267 // intentional: foreach avoids LINQ allocations on this hot path
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

        var otherSet = other as ICollection<T> ?? other.ToHashSet();
        foreach (var item in _dictionary.Keys)
        {
            if (!otherSet.Contains(item))
            {
                _dictionary.TryRemove(item, out _);
            }
        }
    }
#pragma warning restore S3267

    /// <inheritdoc/>
    public bool IsProperSubsetOf(IEnumerable<T> other)
    {
        var otherSet = other.ToHashSet();
        return otherSet.Count > _dictionary.Count && _dictionary.Keys.All(otherSet.Contains);
    }

    /// <inheritdoc/>
    public bool IsProperSupersetOf(IEnumerable<T> other)
    {
        int dictCount = _dictionary.Count;
        if (other.TryGetNonEnumeratedCount(out int count))
        {
            return dictCount > count && other.All(_dictionary.ContainsKey);
        }

        int matchCount = 0;
        foreach (var item in other)
        {
            matchCount++;
            if (!_dictionary.ContainsKey(item) || matchCount >= dictCount)
            {
                return false;
            }
        }

        return dictCount > matchCount;
    }

    /// <inheritdoc/>
    public bool IsSubsetOf(IEnumerable<T> other)
    {
        var otherSet = other.ToHashSet();
        return _dictionary.Keys.All(otherSet.Contains);
    }

    /// <inheritdoc/>
    public bool IsSupersetOf(IEnumerable<T> other)
    {
        return other.All(_dictionary.ContainsKey);
    }

    /// <inheritdoc/>
    public bool Overlaps(IEnumerable<T> other)
    {
        return other.Any(_dictionary.ContainsKey);
    }

    /// <inheritdoc/>
    public bool Remove(T item)
    {
        return _dictionary.TryRemove(item, out _);
    }

    /// <inheritdoc/>
    public bool SetEquals(IEnumerable<T> other)
    {
        var otherSet = other.ToHashSet();
        return _dictionary.Count == otherSet.Count && _dictionary.Keys.All(otherSet.Contains);
    }

    /// <inheritdoc/>
#pragma warning disable S3267 // intentional: foreach avoids LINQ allocations on this hot path
    public void SymmetricExceptWith(IEnumerable<T> other)
    {
        var otherSet = other.ToHashSet();
        foreach (var item in otherSet)
        {
            if (!_dictionary.TryRemove(item, out _))
            {
                _dictionary.TryAdd(item, 0);
            }
        }

        foreach (var item in _dictionary.Keys)
        {
            if (!otherSet.Contains(item))
            {
                _dictionary.TryRemove(item, out _);
            }
        }
    }
#pragma warning restore S3267

    /// <inheritdoc/>
    public void UnionWith(IEnumerable<T> other)
    {
        foreach (var item in other)
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
