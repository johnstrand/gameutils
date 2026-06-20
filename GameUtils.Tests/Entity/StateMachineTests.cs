using GameUtils.Entity;
using Xunit;
using System;

namespace GameUtils.Tests.Entity;

public class StateMachineTests
{
    private enum TestState
    {
        Idle,
        Running,
        Jumping,
        Falling
    }

    [Fact]
    public void Constructor_SetsInitialState()
    {
        var fsm = new StateMachine<TestState>(TestState.Idle);
        Assert.Equal(TestState.Idle, fsm.CurrentState);
    }

    [Fact]
    public void ForceState_ChangesStateImmediately()
    {
        var fsm = new StateMachine<TestState>(TestState.Idle);

        fsm.ForceState(TestState.Running);

        Assert.Equal(TestState.Running, fsm.CurrentState);
    }

    [Fact]
    public void ForceState_ToSameState_DoesNotInvokeCallbacks()
    {
        var fsm = new StateMachine<TestState>(TestState.Idle);
        int exitCount = 0;
        int enterCount = 0;

        fsm.OnExit(TestState.Idle, () => exitCount++);
        fsm.OnEnter(TestState.Idle, () => enterCount++);

        fsm.ForceState(TestState.Idle);

        Assert.Equal(0, exitCount);
        Assert.Equal(0, enterCount);
    }

    [Fact]
    public void Update_WithValidTransition_ChangesState()
    {
        var fsm = new StateMachine<TestState>(TestState.Idle);
        bool shouldRun = false;

        fsm.AddTransition(TestState.Idle, TestState.Running, () => shouldRun);

        fsm.Update();
        Assert.Equal(TestState.Idle, fsm.CurrentState); // Condition is false

        shouldRun = true;
        fsm.Update();
        Assert.Equal(TestState.Running, fsm.CurrentState); // Condition is true
    }

    [Fact]
    public void Update_EvaluatesTransitionsInOrderAdded()
    {
        var fsm = new StateMachine<TestState>(TestState.Idle);

        // Add two transitions that could both be true
        fsm.AddTransition(TestState.Idle, TestState.Running, () => true);
        fsm.AddTransition(TestState.Idle, TestState.Jumping, () => true);

        fsm.Update();

        // Should take the first transition added
        Assert.Equal(TestState.Running, fsm.CurrentState);
    }

    [Fact]
    public void Update_EvaluatesTransitionsOnlyFromCurrentState()
    {
        var fsm = new StateMachine<TestState>(TestState.Idle);

        fsm.AddTransition(TestState.Running, TestState.Jumping, () => true);

        fsm.Update();

        // Should not transition because current state is Idle, not Running
        Assert.Equal(TestState.Idle, fsm.CurrentState);
    }

    [Fact]
    public void ChangeState_InvokesExitAndEnterCallbacks()
    {
        var fsm = new StateMachine<TestState>(TestState.Idle);

        bool exitedIdle = false;
        bool enteredRunning = false;

        fsm.OnExit(TestState.Idle, () => exitedIdle = true);
        fsm.OnEnter(TestState.Running, () => enteredRunning = true);

        fsm.ForceState(TestState.Running);

        Assert.True(exitedIdle);
        Assert.True(enteredRunning);
    }

    [Fact]
    public void ChangeState_ViaUpdate_InvokesExitAndEnterCallbacks()
    {
        var fsm = new StateMachine<TestState>(TestState.Idle);

        int exitedIdleCount = 0;
        int enteredRunningCount = 0;

        fsm.OnExit(TestState.Idle, () => exitedIdleCount++);
        fsm.OnEnter(TestState.Running, () => enteredRunningCount++);

        fsm.AddTransition(TestState.Idle, TestState.Running, () => true);

        fsm.Update();

        Assert.Equal(1, exitedIdleCount);
        Assert.Equal(1, enteredRunningCount);
        Assert.Equal(TestState.Running, fsm.CurrentState);
    }

    [Fact]
    public void AddTransition_CanBeChained()
    {
        var fsm = new StateMachine<TestState>(TestState.Idle);

        fsm.AddTransition(TestState.Idle, TestState.Running, () => false)
           .AddTransition(TestState.Running, TestState.Jumping, () => false);

        Assert.NotNull(fsm);
    }

    [Fact]
    public void OnEnterAndExit_CanBeChained()
    {
        var fsm = new StateMachine<TestState>(TestState.Idle);

        fsm.OnEnter(TestState.Idle, () => { })
           .OnExit(TestState.Idle, () => { });

        Assert.NotNull(fsm);
    }
}
