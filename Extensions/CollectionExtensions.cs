namespace GameUtils.Extensions;
public static class CollectionExtensions
{
    private static readonly Random _random = new();

    public static T GetRandom<T>(this T[] array)
    {
        return array[_random.Next(array.Length)];
    }

    public static T GetRandom<T>(this List<T> list)
    {
        return list[_random.Next(list.Count)];
    }
}

public static class ObjectExtensions
{
    public static T Mutate<T>(this T obj, Func<T, T> action) where T : struct
    {
        return action(obj);
    }

    public static T Mutate<T>(this T obj, Action<T> action) where T : class
    {
        action(obj);
        return obj;
    }

    public static T Out<T>(this T obj, out T outObj)
    {
        outObj = obj;
        return obj;
    }
}
