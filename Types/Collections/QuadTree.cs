using GameUtils.Types.Geometry;
using System.Numerics;

namespace GameUtils.Types.Collections;

/// <summary>
/// A spatial partitioning structure that recursively subdivides 2D space into four quadrants.
/// Efficient for proximity and region queries in 2D games.
/// </summary>
/// <typeparam name="T">The type of item stored in the tree.</typeparam>
public class QuadTree<T>
{
    private const int DefaultCapacity = 8;
    private const int MaxDepth = 8;

    private readonly AABB _bounds;
    private readonly int _capacity;
    private readonly int _depth;

    private readonly List<(Vector2 position, T item)> _items = [];
    private QuadTree<T>[]? _children;

    /// <summary>
    /// Creates a new <see cref="QuadTree{T}"/> covering the given region.
    /// </summary>
    /// <param name="bounds">The spatial region this node covers.</param>
    /// <param name="capacity">Maximum items per node before subdividing (default: 8).</param>
    public QuadTree(AABB bounds, int capacity = DefaultCapacity)
        : this(bounds, capacity, 0) { }

    private QuadTree(AABB bounds, int capacity, int depth)
    {
        _bounds = bounds;
        _capacity = capacity;
        _depth = depth;
    }

    /// <summary>
    /// Inserts an item at the given position. Does nothing if the position is outside the tree bounds.
    /// </summary>
    public bool Insert(T item, Vector2 position)
    {
        if (!_bounds.Contains(position))
        {
            return false;
        }

        if (_children != null)
        {
            return InsertIntoChildren(item, position);
        }

        _items.Add((position, item));

        if (_items.Count > _capacity && _depth < MaxDepth)
        {
            Subdivide();
        }

        return true;
    }

    /// <summary>
    /// Returns all items whose positions fall within <paramref name="region"/>.
    /// </summary>
    public IEnumerable<T> Query(AABB region)
    {
        if (!_bounds.Intersects(region))
        {
            yield break;
        }

        foreach (var (_, item) in _items.Where(e => region.Contains(e.position)))
        {
            yield return item;
        }

        if (_children == null)
        {
            yield break;
        }

        foreach (var child in _children)
        {
            foreach (var item in child.Query(region))
            {
                yield return item;
            }
        }
    }

    /// <summary>
    /// Returns all items within <paramref name="radius"/> of <paramref name="center"/>.
    /// </summary>
    public IEnumerable<T> Query(Vector2 center, float radius)
    {
        var searchArea = new AABB(
            new Vector2(center.X - radius, center.Y - radius),
            new Vector2(center.X + radius, center.Y + radius));

        var radiusSq = radius * radius;

        foreach (var (_, item) in _items.Where(e => searchArea.Contains(e.position) && Vector2.DistanceSquared(center, e.position) <= radiusSq))
        {
            yield return item;
        }

        if (_children == null)
        {
            yield break;
        }

        foreach (var child in _children)
        {
            foreach (var item in child.Query(center, radius))
            {
                yield return item;
            }
        }
    }

    /// <summary>
    /// Removes an item at the given position. Returns <see langword="true"/> if removed.
    /// </summary>
    public bool Remove(T item, Vector2 position)
    {
        if (!_bounds.Contains(position))
        {
            return false;
        }

        if (_children != null)
        {
            return _children.Any(child => child.Remove(item, position));
        }

        var index = _items.FindIndex(e => EqualityComparer<T>.Default.Equals(e.item, item) && e.position == position);
        if (index < 0)
        {
            return false;
        }

        _items.RemoveAt(index);
        return true;
    }

    /// <summary>
    /// Removes all items and collapses all child nodes.
    /// </summary>
    public void Clear()
    {
        _items.Clear();
        _children = null;
    }

    private bool InsertIntoChildren(T item, Vector2 position)
    {
        return _children!.Any(child => child.Insert(item, position));
    }

    private void Subdivide()
    {
        var center = _bounds.Center;
        var min = _bounds.Min;
        var max = _bounds.Max;

        _children =
        [
            new QuadTree<T>(new AABB(min, center), _capacity, _depth + 1),
            new QuadTree<T>(new AABB(new Vector2(center.X, min.Y), new Vector2(max.X, center.Y)), _capacity, _depth + 1),
            new QuadTree<T>(new AABB(new Vector2(min.X, center.Y), new Vector2(center.X, max.Y)), _capacity, _depth + 1),
            new QuadTree<T>(new AABB(center, max), _capacity, _depth + 1),
        ];

        foreach (var (pos, item) in _items)
        {
            InsertIntoChildren(item, pos);
        }

        _items.Clear();
    }
}
