namespace GameUtils.Extensions;

/// <summary>
/// Generic extension methods for any time
/// </summary>
public static class ObjectExtensions
{
    /// <summary>
    /// Mutates a struct by passing it to an action and returning the new value.
    /// </summary>
    public static T Mutate<T>(this T obj, Func<T, T> action) where T : struct
    {
        return action(obj);
    }

    /// <summary>
    /// Mutates a class by passing the reference to an action and returning the same reference.
    /// </summary>
    public static T Mutate<T>(this T obj, Action<T> action) where T : class
    {
        action(obj);
        return obj;
    }

    /// <summary>
    /// Creates a copy of the object through the output parameter.
    /// </summary>
    public static T Out<T>(this T obj, out T outObj)
    {
        outObj = obj;
        return obj;
    }

    /// <summary>
    /// Creates a curried function from a function with no arguments.
    /// </summary>
    /// <returns></returns>
    public static Func<TResult> Curry<T, TResult>(this Func<T, TResult> func, T arg)
    {
        return () => func(arg);
    }

    /// <summary>
    /// Creates a curried function from a function with one argument.
    /// </summary>
    public static Func<T2, TResult> Curry<T1, T2, TResult>(this Func<T1, T2, TResult> func, T1 arg)
    {
        return arg2 => func(arg, arg2);
    }

    /// <summary>
    /// Creates a curried function from a function with two arguments.
    /// </summary>
    public static Func<T2, T3, TResult> Curry<T1, T2, T3, TResult>(this Func<T1, T2, T3, TResult> func, T1 arg)
    {
        return (arg2, arg3) => func(arg, arg2, arg3);
    }

    /// <summary>
    /// Creates a curried function from a function with three arguments.
    /// </summary>
    public static Func<T2, T3, T4, TResult> Curry<T1, T2, T3, T4, TResult>(this Func<T1, T2, T3, T4, TResult> func, T1 arg)
    {
        return (arg2, arg3, arg4) => func(arg, arg2, arg3, arg4);
    }

    /// <summary>
    /// Creates a curried function from a function with four arguments.
    /// </summary>
    public static Func<T2, T3, T4, T5, TResult> Curry<T1, T2, T3, T4, T5, TResult>(this Func<T1, T2, T3, T4, T5, TResult> func, T1 arg)
    {
        return (arg2, arg3, arg4, arg5) => func(arg, arg2, arg3, arg4, arg5);
    }
}
