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
}
