namespace GameUtils.Interfaces;

/// <summary>
/// Bare-bones node interface, with only an ID. This is used to identify the node in the graph, and should be unique for each node.
/// </summary>
public interface INode<T> where T : struct, IComparable<T>
{
    /// <summary>
    /// ID of the node. This is used to identify the node in the graph, and should be unique for each node.
    /// </summary>
    public T NodeId { get; set; }
}

/// <summary>
/// Interface for a node with an integer ID. This is a common case, so this interface is provided for convenience. It inherits from <see cref="INode{Int32}"/>, so it has all the same properties and methods, but with a specific type for the NodeId.
/// </summary>
public interface INode : INode<int>;

/// <summary>
/// Edge interface, representing a connection between two nodes. It has a weight and a direction, and can be used to represent both directed and undirected graphs.
/// </summary>
public interface IEdge<T> where T : struct, IComparable<T>
{
    /// <summary>
    /// Source node of the edge. This is the node that the edge starts from. In a directed graph, this is the node that the edge points away from. In an undirected graph, this is one of the two nodes that the edge connects.
    /// </summary>
    public INode<T> From { get; set; }

    /// <summary>
    /// Destination node of the edge. This is the node that the edge points to. In an undirected graph, this is one of the two nodes that the edge connects.
    /// </summary>
    public INode<T> To { get; set; }

    /// <summary>
    /// Edge weight. This is a value that represents the cost of traversing the edge. In a weighted graph, this can be used to represent the distance between nodes, the time it takes to traverse the edge, or any other metric that is relevant to the problem being solved. In an unweighted graph, this can be set to 1 or any other constant value.
    /// </summary>
    public float Weight { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the graph is directed.
    /// </summary>
    public bool IsDirected { get; set; }
}

/// <summary>
/// Represents a graph edge with integer vertex identifiers.
/// </summary>
public interface IEdge : IEdge<int>;
