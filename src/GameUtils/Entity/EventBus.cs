namespace GameUtils.Entity;

/// <summary>
/// A lightweight, type-safe publish/subscribe event bus.
/// Decouples game systems by allowing them to communicate without direct references.
/// </summary>
public class EventBus
{
    private readonly Dictionary<Type, object> _handlers = [];

    /// <summary>
    /// Subscribes <paramref name="handler"/> to events of type <typeparamref name="TEvent"/>.
    /// </summary>
    public void Subscribe<TEvent>(Action<TEvent> handler)
    {
        ArgumentNullException.ThrowIfNull(handler);
        var type = typeof(TEvent);

        ref var obj = ref System.Runtime.InteropServices.CollectionsMarshal.GetValueRefOrAddDefault(_handlers, type, out bool exists);
        if (!exists || obj is not Action<TEvent>[] oldArray)
        {
            obj = new Action<TEvent>[] { handler };
        }
        else
        {
            var newArray = new Action<TEvent>[oldArray.Length + 1];
            Array.Copy(oldArray, newArray, oldArray.Length);
            newArray[oldArray.Length] = handler;
            obj = newArray;
        }
    }

    /// <summary>
    /// Unsubscribes <paramref name="handler"/> from events of type <typeparamref name="TEvent"/>.
    /// Does nothing if the handler was not subscribed.
    /// </summary>
    public void Unsubscribe<TEvent>(Action<TEvent> handler)
    {
        ArgumentNullException.ThrowIfNull(handler);
        var type = typeof(TEvent);

        if (_handlers.TryGetValue(type, out var obj) && obj is Action<TEvent>[] oldArray)
        {
            int index = Array.IndexOf(oldArray, handler);
            if (index < 0)
            {
                return;
            }

            if (oldArray.Length == 1)
            {
                _handlers.Remove(type);
            }
            else
            {
                var newArray = new Action<TEvent>[oldArray.Length - 1];
                Array.Copy(oldArray, 0, newArray, 0, index);
                Array.Copy(oldArray, index + 1, newArray, index, oldArray.Length - index - 1);
                _handlers[type] = newArray;
            }
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
        if (!_handlers.TryGetValue(type, out var obj))
        {
            return;
        }

        var handlers = (Action<TEvent>[])obj;
        foreach (var handler in handlers)
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
    }

    /// <summary>
    /// Removes all subscribers for <typeparamref name="TEvent"/>.
    /// </summary>
    public void Clear<TEvent>()
    {
        var type = typeof(TEvent);
        _handlers.Remove(type);
    }
}
