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
        if (source.TryGetNonEnumeratedCount(out int count))
        {
            if (count == 0)
            {
                return [];
            }
            var buffer = new T[count];
            if (source is ICollection<T> collection)
            {
                collection.CopyTo(buffer, 0);
            }
            else
            {
                int index = 0;
                foreach (var item in source)
                {
                    buffer[index++] = item;
                }
            }
            var span = buffer.AsSpan();
            for (var i = span.Length - 1; i > 0; i--)
            {
                var j = Random.Shared.Next(i + 1);
                (span[i], span[j]) = (span[j], span[i]);
            }
            return buffer;
        }
        else
        {
            var list = new List<T>();
            foreach (var item in source)
            {
                list.Add(item);
            }
            var span = System.Runtime.InteropServices.CollectionsMarshal.AsSpan(list);
            for (var i = span.Length - 1; i > 0; i--)
            {
                var j = Random.Shared.Next(i + 1);
                (span[i], span[j]) = (span[j], span[i]);
            }
            return list;
        }
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

        T selected = default!;
        float fallbackTotalWeight = 0f;
        bool hasElements = false;

        foreach (var item in source)
        {
            hasElements = true;
            float weight = weightSelector(item);
            if (weight <= 0f) continue;

            fallbackTotalWeight += weight;

            if (Random.Shared.NextDouble() * fallbackTotalWeight < weight)
            {
                selected = item;
            }
        }

        if (!hasElements)
        {
            throw new InvalidOperationException("Sequence contains no elements.");
        }

        if (fallbackTotalWeight <= 0f)
        {
            throw new InvalidOperationException("Total weight must be greater than zero.");
        }

        return selected;
    }
}
