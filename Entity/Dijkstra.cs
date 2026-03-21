namespace GameUtils.Entity;

/// <summary>
/// Helper class for solving the shortest path between two points. This implementation uses Dijkstra's algorithm, but might be wonky for weights that are not 1.
/// </summary>
public class Dijkstra<T> where T : notnull
{
    private readonly Dictionary<T, HashSet<T>> _edges = [];
    private readonly Dictionary<(T from, T to), float> _weights = [];
    private readonly HashSet<T> _nodes = [];

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

        var distances = _nodes.ToDictionary(k => k, _ => float.MaxValue);
        var previousNodes = _nodes.ToDictionary(k => k, _ => default(T));

        distances[start] = 0;

        var queue = new PriorityQueue<T, float>();
        queue.Enqueue(start, 0);

        while (queue.Count > 0)
        {
            var next = queue.Dequeue();

            if (next.Equals(end))
            {
                break;
            }

            if (!_edges.TryGetValue(next, out var neighbors))
            {
                continue;
            }

            foreach (var n in neighbors)
            {
                var distance = distances[next] + _weights[(next, n)];

                if (distance < distances[n])
                {
                    distances[n] = distance;
                    previousNodes[n] = next;
                    queue.Enqueue(n, distance);
                }
            }
        }

        var current = (T?)end;
        while (current != null)
        {
            path.Insert(0, current);
            current = previousNodes[current];
        }

        if (path.Count > 0 && path[0].Equals(start) && path[^1].Equals(end))
        {
            return true;
        }

        path = [];
        return false;
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

        _weights[(edge.From, edge.To)] = edge.Weight;

        if (!_edges.TryGetValue(edge.From, out var edges))
        {
            edges = _edges[edge.From] = [];
        }

        edges.Add(edge.To);

        if (edge.IsDirected)
        {
            return;
        }

        AddEdge(edge with { From = edge.To, To = edge.From });
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
        if (!_edges.Remove(edge.From, out var nodes))
        {
            return;
        }

        if (edge.IsDirected)
        {
            return;
        }

        foreach (var node in nodes)
        {
            if (!_edges.TryGetValue(node, out var edges))
            {
                continue;
            }

            edges.Remove(edge.From);
        }
    }
}

/// <summary>
/// Represents a single edge between two nodes, with an optional weight and direction flag. Directed edges are one-way, undirected edges are two-way.
/// </summary>
public readonly record struct Edge<T>(T From, T To, float Weight = 1, bool IsDirected = false);
