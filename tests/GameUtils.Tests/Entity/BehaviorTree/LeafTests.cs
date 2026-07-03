using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GameUtils.Entity.BehaviorTree;

namespace GameUtils.Tests.Entity.BehaviorTree;

[TestClass]
public class LeafTests
{
    [TestMethod]
    public void Constructor_FuncIsNull_ThrowsArgumentNullException()
    {
        Func<float, NodeStatus> action = null!;
        Assert.ThrowsExactly<ArgumentNullException>(() => new Leaf(action));
    }

    [TestMethod]
    public void Constructor_ActionIsNull_ThrowsArgumentNullException()
    {
        Action action = null!;
        Assert.ThrowsExactly<ArgumentNullException>(() => new Leaf(action));
    }

    [TestMethod]
    public void Tick_WithFunc_ReturnsFuncStatusAndPassesDeltaTime()
    {
        float passedDeltaTime = 0f;
        Func<float, NodeStatus> func = dt =>
        {
            passedDeltaTime = dt;
            return NodeStatus.Running;
        };

        var leaf = new Leaf(func);
        var status = leaf.Tick(1.5f);

        Assert.AreEqual(1.5f, passedDeltaTime);
        Assert.AreEqual(NodeStatus.Running, status);
    }

    [TestMethod]
    public void Tick_WithAction_ExecutesActionAndReturnsSuccess()
    {
        bool actionExecuted = false;
        Action action = () => actionExecuted = true;

        var leaf = new Leaf(action);
        var status = leaf.Tick(2.0f);

        Assert.IsTrue(actionExecuted);
        Assert.AreEqual(NodeStatus.Success, status);
    }
}
