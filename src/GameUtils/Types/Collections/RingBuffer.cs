using System.Collections;

namespace GameUtils.Types.Collections;

/// <summary>
/// A fixed-capacity first-in-first-out circular buffer. When full, writing overwrites the oldest item.
/// Useful for rolling histories, input buffers, and moving averages.
/// </summary>
public class RingBuffer<T> : IEnumerable<T>
{
    private readonly T[] _buffer;
    private int _head;
    private int _tail;

    /// <summary>
    /// Maximum number of items the buffer can hold.
    /// </summary>
    public int Capacity { get; }

    /// <summary>
    /// Number of items currently in the buffer.
    /// </summary>
    public int Count { get; private set; }

    /// <summary>
    /// True when the buffer is empty.
    /// </summary>
    public bool IsEmpty => Count == 0;

    /// <summary>
    /// True when the buffer is full.
    /// </summary>
    public bool IsFull => Count == Capacity;

    /// <summary>
    /// Creates a new ring buffer with the specified capacity.
    /// </summary>
    public RingBuffer(int capacity)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(capacity, 1);
        Capacity = capacity;
        _buffer = new T[capacity];
    }

    /// <summary>
    /// Writes an item to the buffer. If the buffer is full, the oldest item is overwritten.
    /// </summary>
    public void Write(T item)
    {
        _buffer[_tail] = item;
        _tail = (_tail + 1) % Capacity;

        if (IsFull)
        {
            _head = (_head + 1) % Capacity;
        }
        else
        {
            Count++;
        }
    }

    /// <summary>
    /// Reads and removes the oldest item from the buffer.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when the buffer is empty.</exception>
    public T Read()
    {
        if (IsEmpty)
        {
            throw new InvalidOperationException("Buffer is empty.");
        }

        var item = _buffer[_head];
        _buffer[_head] = default!;
        _head = (_head + 1) % Capacity;
        Count--;
        return item;
    }

    /// <summary>
    /// Tries to read and remove the oldest item from the buffer.
    /// </summary>
    public bool TryRead([System.Diagnostics.CodeAnalysis.MaybeNullWhen(false)] out T item)
    {
        if (IsEmpty)
        {
            item = default;
            return false;
        }

        item = Read();
        return true;
    }

    /// <summary>
    /// Peeks at the oldest item without removing it.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when the buffer is empty.</exception>
    public T Peek()
    {
        if (IsEmpty)
        {
            throw new InvalidOperationException("Buffer is empty.");
        }

        return _buffer[_head];
    }

    /// <summary>
    /// Clears all items from the buffer.
    /// </summary>
    public void Clear()
    {
        Array.Clear(_buffer, 0, _buffer.Length);
        _head = 0;
        _tail = 0;
        Count = 0;
    }

    /// <summary>
    /// Returns a snapshot of the buffer contents in oldest-first order.
    /// </summary>
    public T[] Snapshot()
    {
        var result = new T[Count];
        for (var i = 0; i < Count; i++)
        {
            result[i] = _buffer[(_head + i) % Capacity];
        }

        return result;
    }

    /// <inheritdoc/>
    public IEnumerator<T> GetEnumerator()
    {
        for (var i = 0; i < Count; i++)
        {
            yield return _buffer[(_head + i) % Capacity];
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
