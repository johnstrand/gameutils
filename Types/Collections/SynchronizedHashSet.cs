namespace GameUtils.Types.Collections;

/// <summary>
/// Hash set implementation of a synchronized collection.
/// </summary>
/// <typeparam name="T"></typeparam>
public class SynchronizedHashSet<T> : SyncronizedCollection<T> where T : notnull
{
    private readonly HashSet<T> _set = [];

    /// <summary>
    /// Gets the internal collection.
    /// </summary>
    protected override IEnumerable<T> GetInternal()
    {
        return _set;
    }

    /// <summary>
    /// Integrates the operation into the collection.
    /// </summary>
    protected override void HandleOperation(Operation<T> operation)
    {
        if (operation.Kind == OperationKind.Add)
        {
            _set.Add(operation.Entity);
        }
        else if (operation.Kind == OperationKind.Remove)
        {
            _set.Remove(operation.Entity);
        }
    }
}