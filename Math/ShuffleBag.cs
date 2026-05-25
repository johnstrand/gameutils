namespace GameUtils.Math;

/// <summary>
/// A weighted shuffle bag that draws items in a shuffled order, ensuring a fair distribution
/// over time. When the bag is exhausted it automatically refills and reshuffles.
/// </summary>
/// <typeparam name="T">The item type.</typeparam>
public class ShuffleBag<T>
{
    private readonly List<T> _source = [];
    private readonly List<T> _bag = [];
    private readonly Random _rng;
    private int _cursor;

    /// <summary>
    /// Creates a new shuffle bag with an optional seed.
    /// </summary>
    /// <param name="items">Items and their weights. Weight must be ≥ 1.</param>
    /// <param name="seed">Optional seed for reproducible sequences.</param>
    public ShuffleBag(IEnumerable<(T item, int weight)> items, int? seed = null)
    {
        ArgumentNullException.ThrowIfNull(items);
        _rng = seed.HasValue ? new Random(seed.Value) : Random.Shared;

        foreach (var (item, weight) in items)
        {
            if (weight < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(items), "Each item weight must be at least 1.");
            }

            for (var i = 0; i < weight; i++)
            {
                _source.Add(item);
            }
        }

        if (_source.Count == 0)
        {
            throw new ArgumentException("Shuffle bag must contain at least one item.", nameof(items));
        }

        Refill();
    }

    /// <summary>The total number of entries (weighted) in the bag.</summary>
    public int Capacity => _source.Count;

    /// <summary>The number of entries remaining in the current shuffle cycle.</summary>
    public int Remaining => _bag.Count - _cursor;

    /// <summary>
    /// Draws the next item from the bag. Automatically refills when exhausted.
    /// </summary>
    public T Next()
    {
        if (_cursor >= _bag.Count)
        {
            Refill();
        }

        return _bag[_cursor++];
    }

    /// <summary>
    /// Peeks at the next item without consuming it.
    /// </summary>
    public T Peek()
    {
        if (_cursor >= _bag.Count)
        {
            Refill();
        }

        return _bag[_cursor];
    }

    /// <summary>
    /// Resets the bag to the beginning of a new shuffle cycle immediately.
    /// </summary>
    public void Reset()
    {
        Refill();
    }

    private void Refill()
    {
        _bag.Clear();
        _bag.AddRange(_source);

        // Fisher-Yates shuffle
        for (var i = _bag.Count - 1; i > 0; i--)
        {
            var j = _rng.Next(i + 1);
            (_bag[i], _bag[j]) = (_bag[j], _bag[i]);
        }

        _cursor = 0;
    }
}
