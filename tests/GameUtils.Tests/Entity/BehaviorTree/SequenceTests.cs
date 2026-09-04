using System;
using GameUtils.Entity.BehaviorTree;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GameUtils.Tests.Entity.BehaviorTree;

[TestClass]
public class SequenceTests
{
    [TestMethod]
    public void Constructor_NullChildren_ThrowsArgumentNullException()
    {
        Assert.ThrowsExactly<ArgumentNullException>(() => new Sequence(null!));
    }

    [TestMethod]
    public void Tick_EmptyChildren_ReturnsSuccess()
    {
        var sequence = new Sequence([]);
        Assert.AreEqual(NodeStatus.Success, sequence.Tick(0.1f));
    }

    [TestMethod]
    public void Tick_AllChildrenSucceed_ReturnsSuccess()
    {
        int count = 0;
        var sequence = new Sequence([
            new Leaf(() => count++),
            new Leaf(() => count++)
        ]);

        var status = sequence.Tick(0.1f);

        Assert.AreEqual(2, count);
        Assert.AreEqual(NodeStatus.Success, status);
    }

    [TestMethod]
    public void Tick_ChildFails_StopsAndReturnsFailure()
    {
        int count = 0;
        var sequence = new Sequence([
            new Leaf(() => count++),
            new Leaf(_ => NodeStatus.Failure),
            new Leaf(() => count++)
        ]);

        var status = sequence.Tick(0.1f);

        Assert.AreEqual(1, count);
        Assert.AreEqual(NodeStatus.Failure, status);
    }

    [TestMethod]
    public void Tick_ChildReturnsRunning_ResumesFromRunningIndexOnNextTick()
    {
        int leaf1Count = 0;
        int leaf2Count = 0;
        int leaf3Count = 0;
        var leaf2Status = NodeStatus.Running;

        var sequence = new Sequence([
            new Leaf(() => leaf1Count++),
            new Leaf(_ => { leaf2Count++; return leaf2Status; }),
            new Leaf(() => leaf3Count++)
        ]);

        var status1 = sequence.Tick(0.1f);
        Assert.AreEqual(1, leaf1Count);
        Assert.AreEqual(1, leaf2Count);
        Assert.AreEqual(0, leaf3Count);
        Assert.AreEqual(NodeStatus.Running, status1);

        leaf2Status = NodeStatus.Success;
        var status2 = sequence.Tick(0.1f);
        Assert.AreEqual(1, leaf1Count);
        Assert.AreEqual(2, leaf2Count);
        Assert.AreEqual(1, leaf3Count);
        Assert.AreEqual(NodeStatus.Success, status2);
    }

    [TestMethod]
    public void Tick_ResetsRunningIndexAfterFailure()
    {
        int leaf1Count = 0;
        int leaf2Count = 0;
        var leaf1Status = NodeStatus.Running;

        var sequence = new Sequence([
            new Leaf(_ => { leaf1Count++; return leaf1Status; }),
            new Leaf(() => leaf2Count++)
        ]);

        sequence.Tick(0.1f);
        Assert.AreEqual(1, leaf1Count);

        leaf1Status = NodeStatus.Failure;
        var status = sequence.Tick(0.1f);
        Assert.AreEqual(2, leaf1Count);
        Assert.AreEqual(NodeStatus.Failure, status);

        leaf1Status = NodeStatus.Success;
        sequence.Tick(0.1f);
        Assert.AreEqual(3, leaf1Count);
        Assert.AreEqual(1, leaf2Count);
    }
}
