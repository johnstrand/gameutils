using System.Collections.Concurrent;

namespace GameUtils.Types;
internal enum OperationKind
{
    Add,
    Remove,
    Update
}

internal record Operation<T>(OperationKind Kind, T Entity);

internal abstract class SyncronizedCollection<T>
{
    private readonly ConcurrentQueue<Operation<T>> _pending = new();
    private readonly SemaphoreSlim _integrating = new(1, 1);

    public TS Add<TS>(TS entity) where TS : T
    {
        _pending.Enqueue(new Operation<T>(OperationKind.Add, entity));
        return entity;
    }

    public void Remove(T entity)
    {
        _pending.Enqueue(new Operation<T>(OperationKind.Remove, entity));
    }

    public void Update(T entity)
    {
        _pending.Enqueue(new Operation<T>(OperationKind.Update, entity));
    }

    public void Integrate()
    {
        _integrating.Wait();

        while (_pending.TryDequeue(out var operation))
        {
            HandleOperation(operation);
        }

        _integrating.Release();
    }

    public void WaitForSlot()
    {
        _integrating.Wait();
    }

    public void ReleaseSlot()
    {
        _integrating.Release();
    }

    public void Clear()
    {
        _integrating.Wait();
        _pending.Clear();
        _integrating.Release();
    }

    protected abstract void HandleOperation(Operation<T> operation);
}