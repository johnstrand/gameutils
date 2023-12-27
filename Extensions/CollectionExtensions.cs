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
}
