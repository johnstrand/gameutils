namespace GameUtils.Entity;

/// <summary>
/// A lightweight, type-safe publish/subscribe event bus.
/// Decouples game systems by allowing them to communicate without direct references.
/// </summary>
public class EventBus
{
    private readonly Dictionary<Type, List<Delegate>> _handlers = [];

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
    }

    /// <summary>
    /// Unsubscribes <paramref name="handler"/> from events of type <typeparamref name="TEvent"/>.
    /// Does nothing if the handler was not subscribed.
    /// </summary>
    public void Unsubscribe<TEvent>(Action<TEvent> handler)
    {
        ArgumentNullException.ThrowIfNull(handler);

        if (_handlers.TryGetValue(typeof(TEvent), out var list))
        {
            list.Remove(handler);
        }
    }

    /// <summary>
    /// Publishes an event to all subscribers of type <typeparamref name="TEvent"/>.
    /// Handlers are invoked synchronously in subscription order.
    /// </summary>
    public void Publish<TEvent>(TEvent eventData)
    {
        if (!_handlers.TryGetValue(typeof(TEvent), out var list))
        {
            return;
        }

        foreach (var handler in list.ToArray()) // snapshot to allow safe un/subscribe during dispatch
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
    }

    /// <summary>
    /// Removes all subscribers for <typeparamref name="TEvent"/>.
    /// </summary>
    public void Clear<TEvent>()
    {
        _handlers.Remove(typeof(TEvent));
    }
}
