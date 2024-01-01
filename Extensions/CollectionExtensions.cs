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
    /// Shuffles the list, returning a new list.
    /// </summary>
    public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
    {
        return source.OrderBy(_ => _random.Next());
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
    /// Combines map and filter into a single operation.
    /// </summary>
    public static IEnumerable<TOutput> SelectWhere<TInput, TOutput>(this IEnumerable<TInput> source, Func<TInput, bool> predicate, Func<TInput, int, TOutput> selector)
    {
        return source.SelectWhere((item, _) => predicate(item), selector);
    }
}
