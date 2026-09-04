using System;
using GameUtils.Entity.BehaviorTree;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GameUtils.Tests.Entity.BehaviorTree;

[TestClass]
public class SelectorTests
{
    [TestMethod]
    public void Constructor_NullChildren_ThrowsArgumentNullException()
    {
        Assert.ThrowsExactly<ArgumentNullException>(() => new Selector(null!));
    }

    [TestMethod]
    public void Tick_EmptyChildren_ReturnsFailure()
    {
        var selector = new Selector([]);
        Assert.AreEqual(NodeStatus.Failure, selector.Tick(0.1f));
    }

    [TestMethod]
    public void Tick_ChildSucceeds_StopsAndReturnsSuccess()
    {
        int count = 0;
        var selector = new Selector([
            new Leaf(_ => NodeStatus.Failure),
            new Leaf(() => count++),
            new Leaf(() => count++)
        ]);

        var status = selector.Tick(0.1f);

        Assert.AreEqual(1, count);
        Assert.AreEqual(NodeStatus.Success, status);
    }

    [TestMethod]
    public void Tick_AllChildrenFail_ReturnsFailure()
    {
        int count = 0;
        var selector = new Selector([
            new Leaf(_ => { count++; return NodeStatus.Failure; }),
            new Leaf(_ => { count++; return NodeStatus.Failure; })
        ]);

        var status = selector.Tick(0.1f);

        Assert.AreEqual(2, count);
        Assert.AreEqual(NodeStatus.Failure, status);
    }

    [TestMethod]
    public void Tick_ChildReturnsRunning_ResumesFromRunningIndexOnNextTick()
    {
        int leaf1Count = 0;
        int leaf2Count = 0;
        int leaf3Count = 0;
        var leaf2Status = NodeStatus.Running;

        var selector = new Selector([
            new Leaf(_ => { leaf1Count++; return NodeStatus.Failure; }),
            new Leaf(_ => { leaf2Count++; return leaf2Status; }),
            new Leaf(() => leaf3Count++)
        ]);

        var status1 = selector.Tick(0.1f);
        Assert.AreEqual(1, leaf1Count);
        Assert.AreEqual(1, leaf2Count);
        Assert.AreEqual(0, leaf3Count);
        Assert.AreEqual(NodeStatus.Running, status1);

        leaf2Status = NodeStatus.Success;
        var status2 = selector.Tick(0.1f);
        Assert.AreEqual(1, leaf1Count);
        Assert.AreEqual(2, leaf2Count);
        Assert.AreEqual(0, leaf3Count);
        Assert.AreEqual(NodeStatus.Success, status2);
    }

    [TestMethod]
    public void Tick_ResetsRunningIndexAfterCompletion()
    {
        int leaf1Count = 0;
        var leaf1Status = NodeStatus.Running;

        var selector = new Selector([
            new Leaf(_ => { leaf1Count++; return leaf1Status; })
        ]);

        selector.Tick(0.1f);
        Assert.AreEqual(1, leaf1Count);

        leaf1Status = NodeStatus.Success;
        var status = selector.Tick(0.1f);
        Assert.AreEqual(2, leaf1Count);
        Assert.AreEqual(NodeStatus.Success, status);

        selector.Tick(0.1f);
        Assert.AreEqual(3, leaf1Count);
    }
}
