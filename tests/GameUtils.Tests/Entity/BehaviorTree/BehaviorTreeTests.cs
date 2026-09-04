using System;
using GameUtils.Entity.BehaviorTree;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GameUtils.Tests.Entity.BehaviorTree;

[TestClass]
public class BehaviorTreeTests
{
    [TestMethod]
    public void Constructor_NullRoot_ThrowsArgumentNullException()
    {
        Assert.ThrowsExactly<ArgumentNullException>(() => new GameUtils.Entity.BehaviorTree.BehaviorTree(null!));
    }

    [TestMethod]
    public void Tick_DelegatesToRootNode()
    {
        float receivedDelta = 0f;
        var leaf = new Leaf(dt =>
        {
            receivedDelta = dt;
            return NodeStatus.Success;
        });

        var tree = new GameUtils.Entity.BehaviorTree.BehaviorTree(leaf);
        var status = tree.Tick(0.5f);

        Assert.AreEqual(0.5f, receivedDelta);
        Assert.AreEqual(NodeStatus.Success, status);
    }

    [TestMethod]
    public void Tick_WithNestedNodes_EvaluatesTreeCorrectly()
    {
        int step = 0;
        var tree = new GameUtils.Entity.BehaviorTree.BehaviorTree(
            new Selector([
                new Sequence([
                    new Leaf(_ =>
                    {
                        step = 1;
                        return NodeStatus.Failure;
                    }),
                    new Leaf(_ =>
                    {
                        step = 2;
                        return NodeStatus.Success;
                    })
                ]),
                new Leaf(_ =>
                {
                    step = 3;
                    return NodeStatus.Success;
                })
            ])
        );

        var status = tree.Tick(0.16f);

        Assert.AreEqual(3, step);
        Assert.AreEqual(NodeStatus.Success, status);
    }
}
