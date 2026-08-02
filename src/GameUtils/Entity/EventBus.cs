namespace GameUtils.Entity;

/// <summary>
/// A lightweight, type-safe publish/subscribe event bus.
/// Decouples game systems by allowing them to communicate without direct references.
/// </summary>
public class EventBus
{
    private readonly Dictionary<Type, object> _handlers = [];
    private readonly Dictionary<Type, object> _snapshots = [];

    /// <summary>
    /// Subscribes <paramref name="handler"/> to events of type <typeparamref name="TEvent"/>.
    /// </summary>
    public void Subscribe<TEvent>(Action<TEvent> handler)
    {
        ArgumentNullException.ThrowIfNull(handler);
        var type = typeof(TEvent);

        ref var listObj = ref System.Runtime.InteropServices.CollectionsMarshal.GetValueRefOrAddDefault(_handlers, type, out bool exists);
        if (!exists)
        {
            listObj = new List<Action<TEvent>>();
        }
        var list = (List<Action<TEvent>>)listObj!;
        list.Add(handler);
        _snapshots.Remove(type);
    }

    /// <summary>
    /// Unsubscribes <paramref name="handler"/> from events of type <typeparamref name="TEvent"/>.
    /// Does nothing if the handler was not subscribed.
    /// </summary>
    public void Unsubscribe<TEvent>(Action<TEvent> handler)
    {
        ArgumentNullException.ThrowIfNull(handler);
        var type = typeof(TEvent);

        if (_handlers.TryGetValue(type, out var listObj))
        {
            var list = (List<Action<TEvent>>)listObj;
            list.Remove(handler);
            _snapshots.Remove(type);
        }
    }

    /// <summary>
    /// Publishes an event to all subscribers of type <typeparamref name="TEvent"/>.
    /// Handlers are invoked synchronously in subscription order.
    /// Safe to call Subscribe/Unsubscribe from within a handler.
    /// </summary>
    public void Publish<TEvent>(TEvent eventData)
    {
        var type = typeof(TEvent);
        if (!_snapshots.TryGetValue(type, out var snapshot))
        {
            if (!_handlers.TryGetValue(type, out var listObj))
            {
                return;
            }
            snapshot = ((List<Action<TEvent>>)listObj).ToArray();
            _snapshots[type] = snapshot;
        }

        var typedSnapshot = (Action<TEvent>[])snapshot;
        foreach (var handler in typedSnapshot)
        {
            handler(eventData);
        }
    }

    /// <summary>
    /// Removes all subscribers for all event types.
    /// </summary>
    public void Clear()
    {
        _handlers.Clear();
        _snapshots.Clear();
    }

    /// <summary>
    /// Removes all subscribers for <typeparamref name="TEvent"/>.
    /// </summary>
    public void Clear<TEvent>()
    {
        var type = typeof(TEvent);
        _handlers.Remove(type);
        _snapshots.Remove(type);
    }
}
