using System;
using System.Collections.Generic;
using GameUtils.Math;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GameUtils.Tests.Math;

[TestClass]
public class ShuffleBagTests
{
    [TestMethod]
    public void Next_SingleItem_ReturnsSameItem()
    {
        // Arrange
        var bag = new ShuffleBag<string>(new[] { ("A", 1) });

        // Act
        var result1 = bag.Next();
        var result2 = bag.Next();

        // Assert
        Assert.AreEqual("A", result1);
        Assert.AreEqual("A", result2);
    }

    [TestMethod]
    public void Next_WithSeed_IsDeterministic()
    {
        // Arrange
        var items = new[] { ("A", 1), ("B", 2), ("C", 3) };
        var seed = 42;
        var bag1 = new ShuffleBag<string>(items, seed);
        var bag2 = new ShuffleBag<string>(items, seed);

        // Act & Assert
        for (int i = 0; i < 10; i++)
        {
            Assert.AreEqual(bag1.Next(), bag2.Next());
        }
    }

    [TestMethod]
    public void Next_ExhaustsAndRefills_ReturnsCorrectCounts()
    {
        // Arrange
        var bag = new ShuffleBag<string>(new[] { ("A", 2), ("B", 1) });
        var expectedCapacity = 3;

        // Act
        Assert.AreEqual(expectedCapacity, bag.Capacity);

        int aCount = 0;
        int bCount = 0;

        for (int i = 0; i < expectedCapacity; i++)
        {
            var item = bag.Next();
            if (item == "A") aCount++;
            if (item == "B") bCount++;
        }

        // Assert
        Assert.AreEqual(2, aCount);
        Assert.AreEqual(1, bCount);
        Assert.AreEqual(0, bag.Remaining);

        // Act (Refill)
        bag.Next();

        // Assert
        Assert.AreEqual(expectedCapacity - 1, bag.Remaining); // One consumed after refill
    }

    [TestMethod]
    public void Peek_DoesNotConsumeItem_ReturnsSameAsNext()
    {
        // Arrange
        var bag = new ShuffleBag<string>(new[] { ("A", 1), ("B", 1) }, seed: 42);

        // Act
        var peekedItem = bag.Peek();
        var remainingAfterPeek = bag.Remaining;
        var nextItem = bag.Next();
        var remainingAfterNext = bag.Remaining;

        // Assert
        Assert.AreEqual(peekedItem, nextItem);
        Assert.AreEqual(remainingAfterNext + 1, remainingAfterPeek);
    }

    [TestMethod]
    public void Reset_MidCycle_RefillsBagImmediately()
    {
        // Arrange
        var bag = new ShuffleBag<string>(new[] { ("A", 1), ("B", 2) });
        var capacity = bag.Capacity;

        // Consume one item
        bag.Next();
        Assert.AreEqual(capacity - 1, bag.Remaining);

        // Act
        bag.Reset();

        // Assert
        Assert.AreEqual(capacity, bag.Remaining);
    }

    [TestMethod]
    public void Constructor_NullItems_ThrowsArgumentNullException()
    {
        Assert.ThrowsExactly<ArgumentNullException>(() => new ShuffleBag<int>(null!));
    }

    [TestMethod]
    public void Constructor_WeightLessThanOne_ThrowsArgumentOutOfRangeException()
    {
        var items = new List<(int item, int weight)>
        {
            (1, 1),
            (2, 0),
            (3, 2)
        };

        Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => new ShuffleBag<int>(items));
    }

    [TestMethod]
    public void Constructor_EmptyItems_ThrowsArgumentException()
    {
        var items = new List<(int item, int weight)>();

        Assert.ThrowsExactly<ArgumentException>(() => new ShuffleBag<int>(items));
    }

    [TestMethod]
    public void Constructor_ValidItems_InstantiatesSuccessfully()
    {
        var items = new List<(int item, int weight)>
        {
            (1, 1),
            (2, 2)
        };

        var bag = new ShuffleBag<int>(items);

        Assert.IsNotNull(bag);
    }
}
