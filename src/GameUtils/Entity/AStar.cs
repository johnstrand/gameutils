using System.Runtime.InteropServices;

namespace GameUtils.Entity;

/// <summary>
/// Generic A* shortest-path solver. Faster than <see cref="Dijkstra{T}"/> when a good heuristic is available.
/// </summary>
/// <typeparam name="T">Node type. Must be non-null and usable as a dictionary key.</typeparam>
public class AStar<T> where T : notnull
{
    private readonly Dictionary<T, List<(T neighbor, float weight)>> _adjacency = [];
    private readonly HashSet<T> _nodes = [];
    private readonly Dictionary<T, float> _gScore = [];
    private readonly Dictionary<T, T?> _previousNodes = [];
    private readonly HashSet<T> _visited = [];
    private readonly PriorityQueue<T, float> _open = new();

    /// <summary>Creates an empty A* solver.</summary>
    public AStar() { }

    /// <summary>Creates an A* solver pre-populated with nodes and edges.</summary>
    public AStar(IEnumerable<T> nodes, IEnumerable<Edge<T>> edges)
    {
        foreach (var node in nodes)
        {
            AddNode(node);
        }

        foreach (var edge in edges)
        {
            AddEdge(edge);
        }
    }

    /// <summary>Registers a node.</summary>
    public void AddNode(T node) => _nodes.Add(node);

    /// <summary>Registers multiple nodes.</summary>
    public void AddNodes(IEnumerable<T> nodes)
    {
        foreach (var node in nodes)
        {
            AddNode(node);
        }
    }

    /// <summary>
    /// Registers an edge. Unknown nodes are registered automatically.
    /// Undirected edges are stored in both directions.
    /// </summary>
    public void AddEdge(Edge<T> edge)
    {
        _nodes.Add(edge.From);
        _nodes.Add(edge.To);

        if (!_adjacency.TryGetValue(edge.From, out var neighbors))
        {
            neighbors = _adjacency[edge.From] = [];
        }

        neighbors.Add((edge.To, edge.Weight));

        if (!edge.IsDirected)
        {
            AddEdge(edge with { From = edge.To, To = edge.From, IsDirected = true });
        }
    }

    /// <summary>Registers multiple edges.</summary>
    public void AddEdges(IEnumerable<Edge<T>> edges)
    {
        foreach (var edge in edges)
        {
            AddEdge(edge);
        }
    }

    /// <summary>
    /// Finds the shortest path from <paramref name="start"/> to <paramref name="end"/> using the supplied
    /// <paramref name="heuristic"/> (e.g. Euclidean distance). Returns <see langword="true"/> and populates
    /// <paramref name="path"/> on success.
    /// </summary>
    /// <param name="start">Start node.</param>
    /// <param name="end">Goal node.</param>
    /// <param name="heuristic">
    /// Admissible heuristic function — must never overestimate the true cost to reach <paramref name="end"/>.
    /// </param>
    /// <param name="path">The reconstructed path from start to end, inclusive.</param>
    public bool Solve(T start, T end, Func<T, T, float> heuristic, out List<T> path)
    {
        ArgumentNullException.ThrowIfNull(heuristic);
        path = [];

        if (!_nodes.Contains(start) || !_nodes.Contains(end))
        {
            return false;
        }

        _gScore.Clear();
        _previousNodes.Clear();
        _visited.Clear();
        _open.Clear();

        _gScore[start] = 0f;
        _open.Enqueue(start, heuristic(start, end));

        while (_open.Count > 0)
        {
            var current = _open.Dequeue();

            if (!_visited.Add(current))
            {
                continue;
            }

            if (current.Equals(end))
            {
                break;
            }

            if (!_adjacency.TryGetValue(current, out var neighbors))
            {
                continue;
            }

            var currentG = _gScore.GetValueOrDefault(current, float.MaxValue);
            ReadOnlySpan<(T neighbor, float weight)> neighborSpan = CollectionsMarshal.AsSpan(neighbors);
            for (int i = 0; i < neighborSpan.Length; i++)
            {
                var (neighbor, weight) = neighborSpan[i];
                if (_visited.Contains(neighbor))
                {
                    continue;
                }

                var tentativeG = currentG + weight;

                ref var neighborG = ref CollectionsMarshal.GetValueRefOrAddDefault(_gScore, neighbor, out var exists);
                if (!exists || tentativeG < neighborG)
                {
                    neighborG = tentativeG;
                    _previousNodes[neighbor] = current;
                    _open.Enqueue(neighbor, tentativeG + heuristic(neighbor, end));
                }
            }
        }

        // Reconstruct path
        var node = end;
        while (true)
        {
            path.Add(node);
            if (node.Equals(start))
            {
                break;
            }

            if (!_previousNodes.TryGetValue(node, out var prev) || prev is null)
            {
                path = [];
                return false;
            }

            node = prev;
        }

        path.Reverse();

        return path.Count > 0 && path[0].Equals(start) && path[^1].Equals(end);
    }
}
