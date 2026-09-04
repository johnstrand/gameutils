using System;
using GameUtils.Entity.BehaviorTree;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GameUtils.Tests.Entity.BehaviorTree;

[TestClass]
public class InverterTests
{
    [TestMethod]
    public void Constructor_NullChild_ThrowsArgumentNullException()
    {
        Assert.ThrowsExactly<ArgumentNullException>(() => new Inverter(null!));
    }

    [TestMethod]
    public void Tick_ChildReturnsSuccess_ReturnsFailure()
    {
        var inverter = new Inverter(new Leaf(_ => NodeStatus.Success));
        Assert.AreEqual(NodeStatus.Failure, inverter.Tick(0.1f));
    }

    [TestMethod]
    public void Tick_ChildReturnsFailure_ReturnsSuccess()
    {
        var inverter = new Inverter(new Leaf(_ => NodeStatus.Failure));
        Assert.AreEqual(NodeStatus.Success, inverter.Tick(0.1f));
    }

    [TestMethod]
    public void Tick_ChildReturnsRunning_ReturnsRunning()
    {
        var inverter = new Inverter(new Leaf(_ => NodeStatus.Running));
        Assert.AreEqual(NodeStatus.Running, inverter.Tick(0.1f));
    }
}
