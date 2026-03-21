namespace GameUtils.Entity;

/// <summary>
/// A lightweight generic finite state machine. Transitions are evaluated in the order they were added.
/// </summary>
/// <remarks>
/// Creates a new state machine with the given initial state.
/// </remarks>
public class StateMachine<TState>(TState initialState) where TState : notnull
{
    private readonly record struct Transition(TState From, TState To, Func<bool> Condition);

    private readonly List<Transition> _transitions = [];
    private readonly Dictionary<TState, Action> _onEnter = [];
    private readonly Dictionary<TState, Action> _onExit = [];

    /// <summary>
    /// The current state of the machine.
    /// </summary>
    public TState CurrentState { get; private set; } = initialState;

    /// <summary>
    /// Registers an automatic transition from <paramref name="from"/> to <paramref name="to"/>
    /// that fires when <paramref name="condition"/> returns true.
    /// </summary>
    public StateMachine<TState> AddTransition(TState from, TState to, Func<bool> condition)
    {
        _transitions.Add(new Transition(from, to, condition));
        return this;
    }

    /// <summary>
    /// Registers a callback to be invoked when the machine enters <paramref name="state"/>.
    /// </summary>
    public StateMachine<TState> OnEnter(TState state, Action callback)
    {
        _onEnter[state] = callback;
        return this;
    }

    /// <summary>
    /// Registers a callback to be invoked when the machine exits <paramref name="state"/>.
    /// </summary>
    public StateMachine<TState> OnExit(TState state, Action callback)
    {
        _onExit[state] = callback;
        return this;
    }

    /// <summary>
    /// Evaluates all transitions from the current state and performs the first one whose condition is met.
    /// </summary>
    public void Update()
    {
        foreach (var transition in _transitions)
        {
            if (!transition.From.Equals(CurrentState))
            {
                continue;
            }

            if (!transition.Condition())
            {
                continue;
            }

            ChangeState(transition.To);
            return;
        }
    }

    /// <summary>
    /// Immediately transitions to <paramref name="state"/>, bypassing all transition conditions.
    /// </summary>
    public void ForceState(TState state)
    {
        ChangeState(state);
    }

    private void ChangeState(TState next)
    {
        if (_onExit.TryGetValue(CurrentState, out var exitCallback))
        {
            exitCallback();
        }

        CurrentState = next;

        if (_onEnter.TryGetValue(CurrentState, out var enterCallback))
        {
            enterCallback();
        }
    }
}
