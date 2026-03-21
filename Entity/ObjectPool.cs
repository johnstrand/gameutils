namespace GameUtils.Entity;

/// <summary>
/// A generic object pool that reuses instances to reduce garbage collection pressure.
/// </summary>
public class ObjectPool<T>
{
    private readonly Func<T> _factory;
    private readonly Action<T>? _reset;
    private readonly Stack<T> _pool;

    /// <summary>
    /// The number of items currently available in the pool.
    /// </summary>
    public int Count => _pool.Count;

    /// <summary>
    /// Creates a new object pool.
    /// </summary>
    /// <param name="factory">Creates new instances when the pool is empty.</param>
    /// <param name="reset">Optional action called on an item when it is returned, to restore it to a clean state.</param>
    /// <param name="initialCapacity">Number of instances to pre-allocate.</param>
    public ObjectPool(Func<T> factory, Action<T>? reset = null, int initialCapacity = 0)
    {
        _factory = factory;
        _reset = reset;
        _pool = new Stack<T>(initialCapacity);

        for (var i = 0; i < initialCapacity; i++)
        {
            _pool.Push(factory());
        }
    }

    /// <summary>
    /// Returns an item from the pool, creating a new one if the pool is empty.
    /// </summary>
    public T Rent()
    {
        return _pool.TryPop(out var item) ? item : _factory();
    }

    /// <summary>
    /// Returns an item to the pool. The optional reset action is called before the item is stored.
    /// </summary>
    public void Return(T item)
    {
        _reset?.Invoke(item);
        _pool.Push(item);
    }
}
