﻿namespace GameUtils.Types;
public class Dijkstra<T> where T : notnull
{
    private readonly Dictionary<T, HashSet<T>> _edges = [];
    private readonly Dictionary<(T from, T to), float> _weights = [];
    private readonly HashSet<T> _nodes = [];

    public Dijkstra()
    {
    }

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
    public bool Solve(T start, T end, out List<T> path)
    {
        path = [];
        if (!_nodes.Contains(start) || !_nodes.Contains(end))
        {
            return false;
        }

        var distances = _edges.Keys.ToDictionary(k => k, _ => float.MaxValue);

        // Given a node, tracks the best node to move to reach the end as quickly as possible,
        // initially set to Unknown
        var previousNodes = _edges.Keys.ToDictionary(k => k, _ => default(T));

        // List of all points to process
        var q = _edges.Keys.ToList();

        // Helper HashSet for better performance in lookup
        var existsInQueue = new HashSet<T>(q);

        distances[start] = 0;

        // Ensure that the start point is the first item in the queue
        q.Remove(start);
        q.Insert(0, start);
        var insertionIndex = 0; // Tracked index to ensure that we insert nodes at the appropriate places in the queue

        // While we have nodes to process
        while (q.Count > 0)
        {
            var next = q[0]; // Grab the first node on the list

            // Already at the end, exit loop
            if (next.Equals(end))
            {
                break;
            }

            q.RemoveAt(0); // Remove 'next' from the queue
            existsInQueue.Remove(next);

            // Fetch all nearby nodes that are still in the queue
            foreach (var n in _edges[next].Where(existsInQueue.Contains).ToList())
            {
                // Calculate distance travelled to reach the node
                var distance = distances[next] + _weights[(next, n)];

                // If we've travelled a shorter distance than before
                if (distance < distances[n])
                {
                    // Move the node as close to the front as insertionIndex allows us
                    // This ensures that it is processed before other, further away nodes,
                    // but not before closer nodes
                    q.Remove(n);
                    q.Insert(insertionIndex++, n);

                    // Record the new, shorter, distance
                    distances[n] = distance;

                    // Mark next -> as the desirable step to take
                    previousNodes[n] = next;
                }
            }

            // Ensure that the index doesn't grow uncontrollably
            insertionIndex = Math.Max(0, insertionIndex - 1);
        }

        // Time to back-track and solve generate the path
        var current = (T?)end;

        while (current != null)
        {
            path.Insert(0, current);
            current = previousNodes[current];
        }

        if (path[0].Equals(start) && path[^1].Equals(end))
        {
            return true;
        }

        path = [];
        return false;
    }

    public void AddNode(T node)
    {
        _nodes.Add(node);
    }

    public void AddNodes(IEnumerable<T> nodes)
    {
        foreach (var node in nodes)
        {
            AddNode(node);
        }
    }

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

    public void AddEdges(IEnumerable<Edge<T>> edges)
    {
        foreach (var edge in edges)
        {
            AddEdge(edge);
        }
    }

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

public readonly record struct Edge<T>(T From, T To, float Weight = 1, bool IsDirected = false);
