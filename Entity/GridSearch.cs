using GameUtils.Types.Collections;
using System.Numerics;

namespace GameUtils.Entity;

/// <summary>
/// Grid-based pathfinding and flood-fill utilities that operate on <see cref="Grid{T}"/>.
/// </summary>
public static class GridSearch
{
    private static readonly (int dx, int dy)[] CardinalNeighbors = [(0, -1), (1, 0), (0, 1), (-1, 0)];
    private static readonly (int dx, int dy)[] AllNeighbors = [(-1, -1), (0, -1), (1, -1), (-1, 0), (1, 0), (-1, 1), (0, 1), (1, 1)];

    /// <summary>
    /// Performs a breadth-first search from <paramref name="startX"/>, <paramref name="startY"/> and
    /// returns a grid of distances from the start cell. Unreachable cells have a distance of -1.
    /// </summary>
    /// <param name="grid">The grid to search.</param>
    /// <param name="startX">Start column.</param>
    /// <param name="startY">Start row.</param>
    /// <param name="passable">Returns true when a cell can be entered.</param>
    /// <param name="diagonal">When true, diagonal movement is allowed.</param>
    public static Grid<int> BreadthFirstSearch<T>(Grid<T> grid, int startX, int startY, Func<T, bool> passable, bool diagonal = false)
    {
        ArgumentNullException.ThrowIfNull(grid);
        ArgumentNullException.ThrowIfNull(passable);

        var distances = new Grid<int>(grid.Width, grid.Height).Fill(_ => -1);

        if (!grid.IsInBounds(startX, startY) || !passable(grid[startX, startY]))
        {
            return distances;
        }

        var queue = new Queue<(int x, int y)>();
        queue.Enqueue((startX, startY));
        distances[startX, startY] = 0;

        var neighbors = diagonal ? AllNeighbors : CardinalNeighbors;

        while (queue.Count > 0)
        {
            var (cx, cy) = queue.Dequeue();
            var nextDist = distances[cx, cy] + 1;

            foreach (var (dx, dy) in neighbors)
            {
                var nx = cx + dx;
                var ny = cy + dy;

                if (!grid.IsInBounds(nx, ny) || distances[nx, ny] >= 0 || !passable(grid[nx, ny]))
                {
                    continue;
                }

                distances[nx, ny] = nextDist;
                queue.Enqueue((nx, ny));
            }
        }

        return distances;
    }

    /// <summary>
    /// Performs a breadth-first search using a <see cref="Vector2"/> start position.
    /// </summary>
    public static Grid<int> BreadthFirstSearch<T>(Grid<T> grid, Vector2 start, Func<T, bool> passable, bool diagonal = false)
    {
        return BreadthFirstSearch(grid, (int)start.X, (int)start.Y, passable, diagonal);
    }

    /// <summary>
    /// Returns all cells reachable from <paramref name="startX"/>, <paramref name="startY"/> that satisfy
    /// <paramref name="passable"/>. The result is a flat list of (x, y) coordinates.
    /// </summary>
    /// <param name="grid">The grid to search.</param>
    /// <param name="startX">Start column.</param>
    /// <param name="startY">Start row.</param>
    /// <param name="passable">Returns true when a cell can be entered.</param>
    /// <param name="diagonal">When true, diagonal movement is allowed.</param>
    public static IReadOnlyList<(int x, int y)> FloodFill<T>(Grid<T> grid, int startX, int startY, Func<T, bool> passable, bool diagonal = false)
    {
        ArgumentNullException.ThrowIfNull(grid);
        ArgumentNullException.ThrowIfNull(passable);

        var result = new List<(int, int)>();

        if (!grid.IsInBounds(startX, startY) || !passable(grid[startX, startY]))
        {
            return result;
        }

        var visited = new HashSet<(int, int)>();
        var queue = new Queue<(int x, int y)>();
        queue.Enqueue((startX, startY));
        visited.Add((startX, startY));

        var neighbors = diagonal ? AllNeighbors : CardinalNeighbors;

        while (queue.Count > 0)
        {
            var (cx, cy) = queue.Dequeue();
            result.Add((cx, cy));

            foreach (var (dx, dy) in neighbors)
            {
                var nx = cx + dx;
                var ny = cy + dy;

                if (!grid.IsInBounds(nx, ny) || !visited.Add((nx, ny)) || !passable(grid[nx, ny]))
                {
                    continue;
                }

                queue.Enqueue((nx, ny));
            }
        }

        return result;
    }

    /// <summary>
    /// Returns all cells reachable from a <see cref="Vector2"/> start position.
    /// </summary>
    public static IReadOnlyList<(int x, int y)> FloodFill<T>(Grid<T> grid, Vector2 start, Func<T, bool> passable, bool diagonal = false)
    {
        return FloodFill(grid, (int)start.X, (int)start.Y, passable, diagonal);
    }
}
