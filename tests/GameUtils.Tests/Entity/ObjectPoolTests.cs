using GameUtils.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace GameUtils.Tests.Entity;

[TestClass]
public class ObjectPoolTests
{
    private class TestItem
    {
        public bool IsReset { get; set; }
    }

    [TestMethod]
    public void Constructor_InitialCapacity_PreallocatesItems()
    {
        var pool = new ObjectPool<TestItem>(() => new TestItem(), initialCapacity: 5);
        Assert.AreEqual(5, pool.Count);
    }

    [TestMethod]
    public void Rent_EmptyPool_CreatesNewItem()
    {
        var pool = new ObjectPool<TestItem>(() => new TestItem());

        var item = pool.Rent();

        Assert.IsNotNull(item);
        Assert.AreEqual(0, pool.Count);
    }

    [TestMethod]
    public void Rent_NotEmptyPool_ReturnsExistingItem()
    {
        var pool = new ObjectPool<TestItem>(() => new TestItem(), initialCapacity: 1);

        var item = pool.Rent();

        Assert.IsNotNull(item);
        Assert.AreEqual(0, pool.Count);
    }

    [TestMethod]
    public void Return_ValidItem_AddsItemToPool()
    {
        var pool = new ObjectPool<TestItem>(() => new TestItem());
        var item = new TestItem();

        pool.Return(item);

        Assert.AreEqual(1, pool.Count);
    }

    [TestMethod]
    public void Return_WithResetAction_InvokesResetAction()
    {
        var pool = new ObjectPool<TestItem>(() => new TestItem(), item => item.IsReset = true);
        var item = new TestItem();

        pool.Return(item);

        Assert.IsTrue(item.IsReset);
        Assert.AreEqual(1, pool.Count);
    }

    [TestMethod]
    public void Rent_AfterReturn_ReturnsSameItem()
    {
        var pool = new ObjectPool<TestItem>(() => new TestItem());
        var item = new TestItem();

        pool.Return(item);
        var rentedItem = pool.Rent();

        Assert.AreSame(item, rentedItem);
        Assert.AreEqual(0, pool.Count);
    }
}
