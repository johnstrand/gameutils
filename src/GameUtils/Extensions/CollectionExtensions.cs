namespace GameUtils.Extensions;

/// <summary>
/// Various extension methods for collections.
/// </summary>
public static class CollectionExtensions
{
    /// <summary>
    /// Gets a random element from the array.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when the array is empty.</exception>
    public static T GetRandom<T>(this T[] array)
    {
        if (array.Length == 0)
        {
            throw new InvalidOperationException("Sequence contains no elements.");
        }

        return array[Random.Shared.Next(array.Length)];
    }

    /// <summary>
    /// Gets a random element from the list.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when the list is empty.</exception>
    public static T GetRandom<T>(this List<T> list)
    {
        if (list.Count == 0)
        {
            throw new InvalidOperationException("Sequence contains no elements.");
        }

        return list[Random.Shared.Next(list.Count)];
    }

    /// <summary>
    /// Gets a random element from the read-only list.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when the list is empty.</exception>
    public static T GetRandom<T>(this IReadOnlyList<T> list)
    {
        if (list.Count == 0)
        {
            throw new InvalidOperationException("Sequence contains no elements.");
        }

        return list[Random.Shared.Next(list.Count)];
    }

    /// <summary>
    /// Shuffles the list, returning a new list using a Fisher-Yates shuffle.
    /// </summary>
    public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
    {
        var buffer = source.ToArray();
        for (var i = buffer.Length - 1; i > 0; i--)
        {
            var j = Random.Shared.Next(i + 1);
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
        if (source is IReadOnlyList<T> readOnlyList)
        {
            int count = readOnlyList.Count;
            if (count == 0)
            {
                throw new InvalidOperationException("Sequence contains no elements.");
            }

            var totalWeight = 0f;
            for (int i = 0; i < count; i++)
            {
                totalWeight += weightSelector(readOnlyList[i]);
            }

            if (totalWeight <= 0)
            {
                throw new InvalidOperationException("Total weight must be greater than zero.");
            }

            var target = (float)(Random.Shared.NextDouble() * totalWeight);
            var cumulative = 0f;

            for (int i = 0; i < count; i++)
            {
                var item = readOnlyList[i];
                cumulative += weightSelector(item);
                if (target <= cumulative)
                {
                    return item;
                }
            }

            return readOnlyList[count - 1];
        }

        if (source is IList<T> list)
        {
            int count = list.Count;
            if (count == 0)
            {
                throw new InvalidOperationException("Sequence contains no elements.");
            }

            var totalWeight = 0f;
            for (int i = 0; i < count; i++)
            {
                totalWeight += weightSelector(list[i]);
            }

            if (totalWeight <= 0)
            {
                throw new InvalidOperationException("Total weight must be greater than zero.");
            }

            var target = (float)(Random.Shared.NextDouble() * totalWeight);
            var cumulative = 0f;

            for (int i = 0; i < count; i++)
            {
                var item = list[i];
                cumulative += weightSelector(item);
                if (target <= cumulative)
                {
                    return item;
                }
            }

            return list[count - 1];
        }

        var itemsTotalWeight = 0f;
        var hasElements = false;
        foreach (var item in source)
        {
            hasElements = true;
            itemsTotalWeight += weightSelector(item);
        }

        if (!hasElements)
        {
            throw new InvalidOperationException("Sequence contains no elements.");
        }

        if (itemsTotalWeight <= 0)
        {
            throw new InvalidOperationException("Total weight must be greater than zero.");
        }

        var itemsTarget = (float)(Random.Shared.NextDouble() * itemsTotalWeight);
        var itemsCumulative = 0f;
        T lastItem = default!;

        foreach (var item in source)
        {
            lastItem = item;
            itemsCumulative += weightSelector(item);
            if (itemsTarget <= itemsCumulative)
            {
                return item;
            }
        }

        return lastItem;
    }
}
