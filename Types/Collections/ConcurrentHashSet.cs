using System.Collections;
using System.Collections.Concurrent;

namespace GameUtils.Types.Collections;

/// <summary>
/// A thread-safe hash set.
/// </summary>
public class ConcurrentHashSet<T> : ICollection<T>, ISet<T> where T : notnull
{
    private readonly ConcurrentDictionary<T, byte> _dictionary = new();

    public int Count => _dictionary.Count;

    public bool IsReadOnly => false;

    void ICollection<T>.Add(T item)
    {
        _dictionary.TryAdd(item, 0);
    }

    public void Clear()
    {
        _dictionary.Clear();
    }

    public bool Contains(T item)
    {
        return _dictionary.ContainsKey(item);
    }

    public void CopyTo(T[] array, int arrayIndex)
    {
        _dictionary.Keys.CopyTo(array, arrayIndex);
    }

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

    public IEnumerator<T> GetEnumerator()
    {
        return _dictionary.Keys.GetEnumerator();
    }

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

    public bool IsProperSubsetOf(IEnumerable<T> other)
    {
        return _dictionary.Keys.ToHashSet().IsProperSubsetOf(other);
    }

    public bool IsProperSupersetOf(IEnumerable<T> other)
    {
        return _dictionary.Keys.ToHashSet().IsProperSupersetOf(other);
    }

    public bool IsSubsetOf(IEnumerable<T> other)
    {
        return _dictionary.Keys.ToHashSet().IsSubsetOf(other);
    }

    public bool IsSupersetOf(IEnumerable<T> other)
    {
        return _dictionary.Keys.ToHashSet().IsSupersetOf(other);
    }

    public bool Overlaps(IEnumerable<T> other)
    {
        return _dictionary.Keys.ToHashSet().Overlaps(other);
    }

    public bool Remove(T item)
    {
        return _dictionary.TryRemove(item, out _);
    }

    public bool SetEquals(IEnumerable<T> other)
    {
        return _dictionary.Keys.ToHashSet().SetEquals(other);
    }

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
