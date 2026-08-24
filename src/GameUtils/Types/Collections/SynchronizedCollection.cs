using System.Collections;
using System.Collections.Concurrent;

namespace GameUtils.Types.Collections;

/// <summary>
/// Type of operation to perform on a collection.
/// </summary>
public enum OperationKind
{
    /// <summary>
    /// Add an entity to the collection.
    /// </summary>
    Add,

    /// <summary>
    /// Remove an entity from the collection.
    /// </summary>
    Remove
}

/// <summary>
/// Represents an operation to perform on a collection with an entity.
/// </summary>
public readonly record struct Operation<T>(OperationKind Kind, T Entity);

/// <summary>
/// Abstract class for a thread-safe collection that can be modified from multiple threads.
/// </summary>
public abstract class SynchronizedCollection<T> : IEnumerable<T> where T : notnull
{
    private readonly ConcurrentQueue<Operation<T>> _pending = new();
    private readonly SemaphoreSlim _integrating = new(1, 1);
    private List<T>? _cachedSnapshot;
    private volatile bool _dirty = true;

    /// <summary>
    /// Schedules an entity to be added to the collection.
    /// </summary>
    public TS Add<TS>(TS entity) where TS : T
    {
        _pending.Enqueue(new Operation<T>(OperationKind.Add, entity));
        return entity;
    }

    /// <summary>
    /// Schedules an entity to be removed from the collection.
    /// </summary>
    public void Remove(T entity)
    {
        _pending.Enqueue(new Operation<T>(OperationKind.Remove, entity));
    }

    /// <summary>
    /// Integrates all pending operations into the collection.
    /// </summary>
    public void Integrate()
    {
        if (_pending.IsEmpty)
        {
            return;
        }

        _integrating.Wait();
        try
        {
            bool modified = false;
            while (_pending.TryDequeue(out var operation))
            {
                if (!modified)
                {
                    _dirty = true;
                    modified = true;
                }
                HandleOperation(operation);
            }
        }
        finally
        {
            _integrating.Release();
        }
    }

    /// <summary>
    /// Locks the collection until all pending operations have been integrated. This is useful for ensuring that the collection is in a consistent state before performing operations on it.
    /// </summary>
    public void WaitForIntegration()
    {
        _integrating.Wait();
        _integrating.Release();
    }

    /// <summary>
    /// Removes all pending operations from the queue.
    /// </summary>
    public void ClearPending()
    {
        _integrating.Wait();
        try
        {
            _pending.Clear();
        }
        finally
        {
            _integrating.Release();
        }
    }

    /// <summary>
    /// Waits for all pending operations to be integrated, then returns a snapshot of the collection.
    /// </summary>
    public IEnumerable<T> Get()
    {
        if (_dirty || _cachedSnapshot is null)
        {
            _integrating.Wait();
            try
            {
                if (_dirty || _cachedSnapshot is null)
                {
                    _cachedSnapshot ??= new List<T>();
                    _cachedSnapshot.Clear();

                    _cachedSnapshot.AddRange(GetInternal());

                    _dirty = false;
                }
            }
            finally
            {
                _integrating.Release();
            }
        }

        return _cachedSnapshot!;
    }

    /// <summary>
    /// This method is called by the <c>Get</c> method to get a snapshot of the collection.
    /// </summary>
    protected abstract IEnumerable<T> GetInternal();

    /// <summary>
    /// This method is called by the <c>Integrate</c> method for each pending operation.
    /// </summary>
    protected abstract void HandleOperation(Operation<T> operation);

    /// <inheritdoc/>
    public IEnumerator<T> GetEnumerator()
    {
        return Get().GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return Get().GetEnumerator();
    }
}
