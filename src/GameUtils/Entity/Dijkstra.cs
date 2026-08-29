using System.Runtime.InteropServices;

namespace GameUtils.Entity;

/// <summary>
/// Helper class for solving the shortest path between two points. This implementation uses Dijkstra's algorithm, but might be wonky for weights that are not 1.
/// </summary>
public class Dijkstra<T> where T : notnull
{
    private readonly Dictionary<T, List<(T neighbor, float weight)>> _adjacency = [];
    private readonly HashSet<T> _nodes = [];
    private readonly Dictionary<T, float> _distances = [];
    private readonly Dictionary<T, T?> _previousNodes = [];
    private readonly HashSet<T> _visited = [];
    private readonly PriorityQueue<T, float> _queue = new();

    /// <summary>
    /// Base constructor
    /// </summary>
    public Dijkstra()
    {
    }

    /// <summary>
    /// Constructor that takes in a list of nodes and edges
    /// </summary>
    public Dijkstra(IEnumerable<T> nodes, IEnumerable<Edge<T>> edges)
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

    /// <summary>
    /// Given a start and end point, attempts to find the shortest path between them, based on the edges and weights supplied
    /// </summary>
    public bool Solve(T start, T end, out List<T> path)
    {
        path = [];
        if (!_nodes.Contains(start) || !_nodes.Contains(end))
        {
            return false;
        }

        _distances.Clear();
        _previousNodes.Clear();
        _visited.Clear();
        _queue.Clear();

        _distances[start] = 0;
        _queue.Enqueue(start, 0);

        while (_queue.Count > 0)
        {
            var next = _queue.Dequeue();

            if (!_visited.Add(next))
            {
                continue; // stale priority-queue entry
            }

            if (next.Equals(end))
            {
                break;
            }

            if (!_adjacency.TryGetValue(next, out var neighbors))
            {
                continue;
            }

            var nextDist = _distances.GetValueOrDefault(next, float.MaxValue);
            foreach (var (n, weight) in neighbors)
            {
                if (_visited.Contains(n))
                {
                    continue;
                }

                var distance = nextDist + weight;

                ref var nDist = ref CollectionsMarshal.GetValueRefOrAddDefault(_distances, n, out var exists);
                if (!exists || distance < nDist)
                {
                    nDist = distance;
                    _previousNodes[n] = next;
                    _queue.Enqueue(n, distance);
                }
            }
        }

        // Reconstruct path
        var stack = new Stack<T>();
        var current = end;
        while (true)
        {
            stack.Push(current);
            if (current.Equals(start))
            {
                break;
            }

            if (!_previousNodes.TryGetValue(current, out var prev) || prev is null)
            {
                path = [];
                return false;
            }

            current = prev;
        }

        path = new List<T>(stack);

        return path.Count > 0 && path[0].Equals(start) && path[^1].Equals(end);
    }

    /// <summary>
    /// Registers a node with the solver
    /// </summary>
    public void AddNode(T node)
    {
        _nodes.Add(node);
    }

    /// <summary>
    /// Registers a list of nodes with the solver
    /// </summary>
    public void AddNodes(IEnumerable<T> nodes)
    {
        foreach (var node in nodes)
        {
            AddNode(node);
        }
    }

    /// <summary>
    /// Registers an edge with the solver. If the edge is undirected, it will be registered as two edges. Any unknown nodes will be registered as well.
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

        if (edge.IsDirected)
        {
            return;
        }

        AddEdge(edge with { From = edge.To, To = edge.From, IsDirected = true });
    }

    /// <summary>
    /// Registers a list of edges with the solver. If any edges are undirected, they will be registered as two edges. Any unknown nodes will be registered as well.
    /// </summary>
    public void AddEdges(IEnumerable<Edge<T>> edges)
    {
        foreach (var edge in edges)
        {
            AddEdge(edge);
        }
    }

    /// <summary>
    /// Removes an edge from the solver. If the edge is undirected, the return edge will be removed as well. Existing nodes will not be removed.
    /// </summary>
    /// <param name="edge"></param>
    public void RemoveEdge(Edge<T> edge)
    {
        if (_adjacency.TryGetValue(edge.From, out var fromNeighbors))
        {
            var index = fromNeighbors.FindIndex(n => EqualityComparer<T>.Default.Equals(n.neighbor, edge.To));
            if (index >= 0)
            {
                fromNeighbors.RemoveAt(index);
            }
        }

        if (edge.IsDirected)
        {
            return;
        }

        if (_adjacency.TryGetValue(edge.To, out var toNeighbors))
        {
            var index = toNeighbors.FindIndex(n => EqualityComparer<T>.Default.Equals(n.neighbor, edge.From));
            if (index >= 0)
            {
                toNeighbors.RemoveAt(index);
            }
        }
    }

}

/// <summary>
/// Represents a single edge between two nodes, with an optional weight and direction flag. Directed edges are one-way, undirected edges are two-way.
/// </summary>
public readonly record struct Edge<T>(T From, T To, float Weight = 1, bool IsDirected = false);
