namespace GameUtils.Types.Collections;

public class SynchronizedHashSet<T> : SyncronizedCollection<T> where T : notnull
{
    private readonly HashSet<T> _set = [];

    protected override IEnumerable<T> GetInternal()
    {
        return _set;
    }

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