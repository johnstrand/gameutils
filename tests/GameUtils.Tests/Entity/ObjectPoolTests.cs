using System;
using GameUtils.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GameUtils.Tests.Entity;

[TestClass]
public class ObjectPoolTests
{
    private class TestObject
    {
        public bool IsReset { get; set; }
    }

    [TestMethod]
    public void Rent_EmptyPool_CreatesNewInstance()
    {
        // Arrange
        var pool = new ObjectPool<TestObject>(() => new TestObject());

        // Act
        var item = pool.Rent();

        // Assert
        Assert.IsNotNull(item);
        Assert.AreEqual(0, pool.Count);
    }

    [TestMethod]
    public void Rent_PoolHasItems_ReturnsExistingInstance()
    {
        // Arrange
        var pool = new ObjectPool<TestObject>(() => new TestObject());
        var originalItem = pool.Rent();
        pool.Return(originalItem);

        // Act
        var rentedItem = pool.Rent();

        // Assert
        Assert.AreSame(originalItem, rentedItem);
        Assert.AreEqual(0, pool.Count);
    }

    [TestMethod]
    public void Return_ValidItem_AddsToPool()
    {
        // Arrange
        var pool = new ObjectPool<TestObject>(() => new TestObject());
        var item = new TestObject();

        // Act
        pool.Return(item);

        // Assert
        Assert.AreEqual(1, pool.Count);
    }

    [TestMethod]
    public void Return_WithResetAction_InvokesReset()
    {
        // Arrange
        var pool = new ObjectPool<TestObject>(
            factory: () => new TestObject(),
            reset: obj => obj.IsReset = true
        );
        var item = new TestObject { IsReset = false };

        // Act
        pool.Return(item);

        // Assert
        Assert.IsTrue(item.IsReset);
        Assert.AreEqual(1, pool.Count);
    }

    [TestMethod]
    public void Constructor_WithInitialCapacity_PreallocatesItems()
    {
        // Arrange
        int factoryInvokeCount = 0;
        var pool = new ObjectPool<TestObject>(
            factory: () =>
            {
                factoryInvokeCount++;
                return new TestObject();
            },
            reset: null,
            initialCapacity: 5
        );

        // Assert
        Assert.AreEqual(5, pool.Count);
        Assert.AreEqual(5, factoryInvokeCount);
    }

    [TestMethod]
    public void Count_ReflectsPoolState()
    {
        // Arrange
        var pool = new ObjectPool<TestObject>(() => new TestObject());
        Assert.AreEqual(0, pool.Count);

        // Act & Assert
        var item1 = pool.Rent();
        Assert.AreEqual(0, pool.Count);

        pool.Return(item1);
        Assert.AreEqual(1, pool.Count);

        _ = pool.Rent();
        Assert.AreEqual(0, pool.Count);

        pool.Return(item1);
        var item3 = new TestObject();
        pool.Return(item3);
        Assert.AreEqual(2, pool.Count);
    }
}
