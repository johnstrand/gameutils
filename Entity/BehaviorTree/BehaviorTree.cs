namespace GameUtils.Entity.BehaviorTree;

/// <summary>
/// The result of evaluating a behavior tree node.
/// </summary>
public enum NodeStatus
{
    /// <summary>The node completed successfully.</summary>
    Success,

    /// <summary>The node failed.</summary>
    Failure,

    /// <summary>The node is still running and should be ticked again next frame.</summary>
    Running
}

/// <summary>
/// Base class for all behavior tree nodes.
/// </summary>
#pragma warning disable S1694 // Abstract class is intentional: subclasses hold mutable state (e.g., running index)
public abstract class BehaviorNode
{
    /// <summary>Evaluates this node and returns its status.</summary>
    public abstract NodeStatus Tick(float deltaTime);
}
#pragma warning restore S1694

/// <summary>
/// A leaf node backed by a user-supplied delegate.
/// </summary>
public class Leaf : BehaviorNode
{
    private readonly Func<float, NodeStatus> _action;

    /// <summary>Creates a leaf node from a delegate.</summary>
    public Leaf(Func<float, NodeStatus> action)
    {
        ArgumentNullException.ThrowIfNull(action);
        _action = action;
    }

    /// <summary>Creates a leaf node from a parameterless delegate that always returns <see cref="NodeStatus.Success"/>.</summary>
    public Leaf(Action action) : this(_ => { action(); return NodeStatus.Success; })
    {
        ArgumentNullException.ThrowIfNull(action);
    }

    /// <inheritdoc/>
    public override NodeStatus Tick(float deltaTime) => _action(deltaTime);
}

/// <summary>
/// Runs children in order and returns <see cref="NodeStatus.Success"/> when all succeed.
/// Stops and returns <see cref="NodeStatus.Failure"/> on the first failing child.
/// Returns <see cref="NodeStatus.Running"/> when a child is still running.
/// </summary>
public class Sequence : BehaviorNode
{
    private readonly List<BehaviorNode> _children;
    private int _runningIndex;

    /// <summary>Creates a sequence from the given children.</summary>
    public Sequence(IEnumerable<BehaviorNode> children)
    {
        ArgumentNullException.ThrowIfNull(children);
        _children = [.. children];
    }

    /// <inheritdoc/>
    public override NodeStatus Tick(float deltaTime)
    {
        for (var i = _runningIndex; i < _children.Count; i++)
        {
            var status = _children[i].Tick(deltaTime);
            if (status == NodeStatus.Running)
            {
                _runningIndex = i;
                return NodeStatus.Running;
            }

            if (status == NodeStatus.Failure)
            {
                _runningIndex = 0;
                return NodeStatus.Failure;
            }
        }

        _runningIndex = 0;
        return NodeStatus.Success;
    }
}

/// <summary>
/// Runs children in order and returns <see cref="NodeStatus.Success"/> on the first succeeding child.
/// Returns <see cref="NodeStatus.Failure"/> when all children fail.
/// Returns <see cref="NodeStatus.Running"/> when a child is still running.
/// </summary>
public class Selector : BehaviorNode
{
    private readonly List<BehaviorNode> _children;
    private int _runningIndex;

    /// <summary>Creates a selector from the given children.</summary>
    public Selector(IEnumerable<BehaviorNode> children)
    {
        ArgumentNullException.ThrowIfNull(children);
        _children = [.. children];
    }

    /// <inheritdoc/>
    public override NodeStatus Tick(float deltaTime)
    {
        for (var i = _runningIndex; i < _children.Count; i++)
        {
            var status = _children[i].Tick(deltaTime);
            if (status == NodeStatus.Running)
            {
                _runningIndex = i;
                return NodeStatus.Running;
            }

            if (status == NodeStatus.Success)
            {
                _runningIndex = 0;
                return NodeStatus.Success;
            }
        }

        _runningIndex = 0;
        return NodeStatus.Failure;
    }
}

/// <summary>
/// Inverts the result of its child: <see cref="NodeStatus.Success"/> becomes <see cref="NodeStatus.Failure"/>
/// and vice versa. <see cref="NodeStatus.Running"/> is passed through unchanged.
/// </summary>
public class Inverter : BehaviorNode
{
    private readonly BehaviorNode _child;

    /// <summary>Creates an inverter wrapping <paramref name="child"/>.</summary>
    public Inverter(BehaviorNode child)
    {
        ArgumentNullException.ThrowIfNull(child);
        _child = child;
    }

    /// <inheritdoc/>
    public override NodeStatus Tick(float deltaTime)
    {
        return _child.Tick(deltaTime) switch
        {
            NodeStatus.Success => NodeStatus.Failure,
            NodeStatus.Failure => NodeStatus.Success,
            _ => NodeStatus.Running
        };
    }
}

/// <summary>
/// The root of a behavior tree. Tick this each game frame to drive AI logic.
/// </summary>
public class BehaviorTree
{
    private readonly BehaviorNode _root;

    /// <summary>Creates a behavior tree with the given root node.</summary>
    public BehaviorTree(BehaviorNode root)
    {
        ArgumentNullException.ThrowIfNull(root);
        _root = root;
    }

    /// <summary>
    /// Evaluates the tree for one frame. Returns the root node's status.
    /// </summary>
    public NodeStatus Tick(float deltaTime) => _root.Tick(deltaTime);
}
