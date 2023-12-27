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
public record Operation<T>(OperationKind Kind, T Entity);

/// <summary>
/// Abstract class for a thread-safe collection that can be modified from multiple threads.
/// </summary>
public abstract class SyncronizedCollection<T> : IEnumerable<T> where T : notnull
{
    private readonly ConcurrentQueue<Operation<T>> _pending = new();
    private readonly SemaphoreSlim _integrating = new(1, 1);

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
        _integrating.Wait();

        while (_pending.TryDequeue(out var operation))
        {
            HandleOperation(operation);
        }

        _integrating.Release();
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
        _pending.Clear();
        _integrating.Release();
    }

    /// <summary>
    /// Waits for all pending operations to be integrated, then returns a snapshot of the collection.
    /// </summary>
    public IEnumerable<T> Get()
    {
        _integrating.Wait();
        var items = GetInternal().ToList();
        _integrating.Release();
        return items;
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
