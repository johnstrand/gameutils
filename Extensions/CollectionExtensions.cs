namespace GameUtils.Extensions;

/// <summary>
/// Various extension methods for collections.
/// </summary>
public static class CollectionExtensions
{
    private static readonly Random _random = new();

    /// <summary>
    /// Gets a random element from the array.
    /// </summary>
    public static T GetRandom<T>(this T[] array)
    {
        return array[_random.Next(array.Length)];
    }

    /// <summary>
    /// Gets a random element from the list.
    /// </summary>
    public static T GetRandom<T>(this List<T> list)
    {
        return list[_random.Next(list.Count)];
    }

    /// <summary>
    /// Shuffles the list, returning a new list using a Fisher-Yates shuffle.
    /// </summary>
    public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
    {
        var buffer = source.ToArray();
        for (var i = buffer.Length - 1; i > 0; i--)
        {
            var j = _random.Next(i + 1);
            (buffer[i], buffer[j]) = (buffer[j], buffer[i]);
        }
        return buffer;
    }

    /// <summary>
    /// Returns each element of the list with its index, in a deconstructable tuple.
    /// </summary>
    public static IEnumerable<(T value, int index)> ToIndex<T>(this IEnumerable<T> source)
    {
        var index = 0;
        foreach (var item in source)
        {
            yield return (item, index);
            index++;
        }
    }

    /// <summary>
    /// Where-overload that provides the index of the element to the predicate.
    /// </summary>
    public static IEnumerable<T> Where<T>(this IEnumerable<T> source, Func<T, int, bool> predicate)
    {
        var index = 0;
        foreach (var item in source)
        {
            if (predicate(item, index))
            {
                yield return item;
            }
            index++;
        }
    }

    /// <summary>
    /// Combines map and filter into a single operation.
    /// </summary>
    public static IEnumerable<TOutput> SelectWhere<TInput, TOutput>(this IEnumerable<TInput> source, Func<TInput, int, bool> predicate, Func<TInput, int, TOutput> selector)
    {
        var index = 0;
        foreach (var item in source)
        {
            if (predicate(item, index))
            {
                yield return selector(item, index);
            }
            index++;
        }
    }

    /// <summary>
    /// Combines map and filter into a single operation.
    /// </summary>
    public static IEnumerable<TOutput> SelectWhere<TInput, TOutput>(this IEnumerable<TInput> source, Func<TInput, bool> predicate, Func<TInput, TOutput> selector)
    {
        return source.SelectWhere((item, _) => predicate(item), (item, _) => selector(item));
    }

    /// <summary>
    /// Combines map and filter into a single operation.
    /// </summary>
    public static IEnumerable<TOutput> SelectWhere<TInput, TOutput>(this IEnumerable<TInput> source, Func<TInput, int, bool> predicate, Func<TInput, TOutput> selector)
    {
        return source.SelectWhere(predicate, (item, _) => selector(item));
    }

    /// <summary>
    /// Gets a random element from the read-only list.
    /// </summary>
    public static T GetRandom<T>(this IReadOnlyList<T> list)
    {
        return list[_random.Next(list.Count)];
    }

    /// <summary>
    /// Calls <paramref name="action"/> on each element in the sequence.
    /// </summary>
    public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
    {
        foreach (var item in source)
        {
            action(item);
        }
    }

    /// <summary>
    /// Picks a random element from the sequence using the specified weight selector.
    /// Higher weights increase the chance of selection.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when the sequence is empty or all weights are zero.</exception>
    public static T WeightedRandom<T>(this IEnumerable<T> source, Func<T, float> weightSelector)
    {
        var items = source.ToList();
        if (items.Count == 0)
        {
            throw new InvalidOperationException("Sequence contains no elements.");
        }

        var totalWeight = items.Sum(weightSelector);
        if (totalWeight <= 0)
        {
            throw new InvalidOperationException("Total weight must be greater than zero.");
        }

        var target = (float)(_random.NextDouble() * totalWeight);
        var cumulative = 0f;

        foreach (var item in items)
        {
            cumulative += weightSelector(item);
            if (target <= cumulative)
            {
                return item;
            }
        }

        return items[^1];
    }
}
