using System.Numerics;
using GameUtils.Types.Geometry;

namespace GameUtils.Types.Collections;

/// <summary>
/// A grid-based spatial hash for fast O(1) amortized insertion and proximity queries.
/// Best suited for large numbers of uniformly distributed objects (bullets, particles, units).
/// For non-uniform distributions, prefer <see cref="QuadTree{T}"/>.
/// </summary>
/// <typeparam name="T">The type of item stored in the hash.</typeparam>
public class SpatialHash<T>
{
    private readonly float _invCellSize;

    /// <summary>The world-space size of each grid cell.</summary>
    public float CellSize { get; }

    private readonly Dictionary<(int x, int y), List<(Vector2 position, T item)>> _cells = [];

    /// <summary>Creates a new <see cref="SpatialHash{T}"/> with the specified cell size.</summary>
    /// <param name="cellSize">
    /// World-space size of each grid cell. Should be roughly equal to the largest query radius
    /// or the typical object spacing for best performance.
    /// </param>
    public SpatialHash(float cellSize)
    {
        if (cellSize <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(cellSize), "Cell size must be positive.");
        }

        CellSize = cellSize;
        _invCellSize = 1f / cellSize;
    }

    /// <summary>The number of occupied cells currently in the hash.</summary>
    public int CellCount => _cells.Count;

    /// <summary>
    /// Inserts <paramref name="item"/> at <paramref name="position"/>.
    /// The same item may be inserted multiple times at different positions.
    /// </summary>
    public void Insert(T item, Vector2 position)
    {
        var key = GetCell(position);
        if (!_cells.TryGetValue(key, out var list))
        {
            list = _cells[key] = [];
        }

        list.Add((position, item));
    }

    /// <summary>
    /// Removes the first occurrence of <paramref name="item"/> at <paramref name="position"/>.
    /// Returns <see langword="true"/> if found and removed.
    /// </summary>
    public bool Remove(T item, Vector2 position)
    {
        var key = GetCell(position);
        if (!_cells.TryGetValue(key, out var list))
        {
            return false;
        }

        var index = list.FindIndex(e => EqualityComparer<T>.Default.Equals(e.item, item) && e.position == position);
        if (index < 0)
        {
            return false;
        }

        list.RemoveAt(index);
        if (list.Count == 0)
        {
            _cells.Remove(key);
        }

        return true;
    }

    /// <summary>
    /// Returns all items whose positions fall within <paramref name="region"/>.
    /// </summary>
    public IEnumerable<T> Query(AABB region)
    {
        var (minCx, minCy) = GetCell(region.Min);
        var (maxCx, maxCy) = GetCell(region.Max);

        for (var cy = minCy; cy <= maxCy; cy++)
        {
            for (var cx = minCx; cx <= maxCx; cx++)
            {
                if (!_cells.TryGetValue((cx, cy), out var list))
                {
                    continue;
                }

                foreach (var (pos, item) in list)
                {
                    if (region.Contains(pos))
                    {
                        yield return item;
                    }
                }
            }
        }
    }

    /// <summary>
    /// Returns all items within <paramref name="radius"/> of <paramref name="center"/>.
    /// </summary>
    public IEnumerable<T> Query(Vector2 center, float radius)
    {
        var radiusSq = radius * radius;
        var searchArea = new AABB(
            new Vector2(center.X - radius, center.Y - radius),
            new Vector2(center.X + radius, center.Y + radius));

        var (minCx, minCy) = GetCell(searchArea.Min);
        var (maxCx, maxCy) = GetCell(searchArea.Max);

        for (var cy = minCy; cy <= maxCy; cy++)
        {
            for (var cx = minCx; cx <= maxCx; cx++)
            {
                if (!_cells.TryGetValue((cx, cy), out var list))
                {
                    continue;
                }

                foreach (var (pos, item) in list)
                {
                    if (Vector2.DistanceSquared(center, pos) <= radiusSq)
                    {
                        yield return item;
                    }
                }
            }
        }
    }

    /// <summary>Removes all items from the hash.</summary>
    public void Clear()
    {
        _cells.Clear();
    }

    private (int x, int y) GetCell(Vector2 position)
        => ((int)MathF.Floor(position.X * _invCellSize), (int)MathF.Floor(position.Y * _invCellSize));
}
