namespace GameUtils.Entity;

/// <summary>
/// A lightweight, type-safe publish/subscribe event bus.
/// Decouples game systems by allowing them to communicate without direct references.
/// </summary>
public class EventBus
{
    private readonly Dictionary<Type, List<Delegate>> _handlers = [];
    private readonly Dictionary<Type, Delegate[]> _snapshots = [];

    /// <summary>
    /// Subscribes <paramref name="handler"/> to events of type <typeparamref name="TEvent"/>.
    /// </summary>
    public void Subscribe<TEvent>(Action<TEvent> handler)
    {
        ArgumentNullException.ThrowIfNull(handler);
        var type = typeof(TEvent);

        if (!_handlers.TryGetValue(type, out var list))
        {
            list = _handlers[type] = [];
        }

        list.Add(handler);
        _snapshots[type] = [.. list];
    }

    /// <summary>
    /// Unsubscribes <paramref name="handler"/> from events of type <typeparamref name="TEvent"/>.
    /// Does nothing if the handler was not subscribed.
    /// </summary>
    public void Unsubscribe<TEvent>(Action<TEvent> handler)
    {
        ArgumentNullException.ThrowIfNull(handler);
        var type = typeof(TEvent);

        if (_handlers.TryGetValue(type, out var list))
        {
            list.Remove(handler);
            _snapshots[type] = [.. list];
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
            return;
        }

        foreach (var handler in snapshot)
        {
            ((Action<TEvent>)handler)(eventData);
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
