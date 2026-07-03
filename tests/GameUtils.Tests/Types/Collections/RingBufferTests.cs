using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GameUtils.Types.Collections;

namespace GameUtils.Tests.Types.Collections;

[TestClass]
public class RingBufferTests
{
    [TestMethod]
    public void Constructor_ValidCapacity_SetsCapacityAndInitializesValues()
    {
        // Arrange & Act
        var buffer = new RingBuffer<int>(5);

        // Assert
        Assert.AreEqual(5, buffer.Capacity);
        Assert.AreEqual(0, buffer.Count);
        Assert.IsTrue(buffer.IsEmpty);
        Assert.IsFalse(buffer.IsFull);
    }

    [TestMethod]
    [DataRow(0)]
    [DataRow(-1)]
    [DataRow(-100)]
    public void Constructor_InvalidCapacity_ThrowsArgumentOutOfRangeException(int invalidCapacity)
    {
        // Arrange, Act & Assert
        Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => new RingBuffer<int>(invalidCapacity));
    }

    [TestMethod]
    public void Write_EmptyBuffer_AddsItemAndUpdatesCount()
    {
        // Arrange
        var buffer = new RingBuffer<int>(3);

        // Act
        buffer.Write(10);

        // Assert
        Assert.AreEqual(1, buffer.Count);
        Assert.IsFalse(buffer.IsEmpty);
        Assert.IsFalse(buffer.IsFull);
        Assert.AreEqual(10, buffer.Peek());
    }

    [TestMethod]
    public void Write_BufferNotFull_AddsItemsAndUpdatesCount()
    {
        // Arrange
        var buffer = new RingBuffer<int>(3);
        buffer.Write(10);

        // Act
        buffer.Write(20);

        // Assert
        Assert.AreEqual(2, buffer.Count);
        Assert.IsFalse(buffer.IsEmpty);
        Assert.IsFalse(buffer.IsFull);

        var snapshot = buffer.Snapshot();
        CollectionAssert.AreEqual(new[] { 10, 20 }, snapshot);
    }

    [TestMethod]
    public void Write_FullBuffer_OverwritesOldestItemAndAdvancesHead()
    {
        // Arrange
        var buffer = new RingBuffer<int>(3);
        buffer.Write(10);
        buffer.Write(20);
        buffer.Write(30);

        // Act
        buffer.Write(40);

        // Assert
        Assert.AreEqual(3, buffer.Count);
        Assert.IsTrue(buffer.IsFull);

        Assert.AreEqual(20, buffer.Peek());

        var snapshot = buffer.Snapshot();
        CollectionAssert.AreEqual(new[] { 20, 30, 40 }, snapshot);
    }

    [TestMethod]
    public void Write_MultipleOverwrites_MaintainsCorrectStateAndOrder()
    {
        // Arrange
        var buffer = new RingBuffer<int>(3);
        buffer.Write(1);
        buffer.Write(2);
        buffer.Write(3);

        // Act
        buffer.Write(4);
        buffer.Write(5);

        // Assert
        Assert.AreEqual(3, buffer.Count);
        Assert.IsTrue(buffer.IsFull);
        Assert.AreEqual(3, buffer.Peek());

        var snapshot = buffer.Snapshot();
        CollectionAssert.AreEqual(new[] { 3, 4, 5 }, snapshot);
    }
}
