using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GameUtils.Math;

namespace GameUtils.Tests.Math;

[TestClass]
public class ShuffleBagTests
{
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

    [TestMethod]
    public void Peek_WhenCalled_ReturnsNextItemWithoutConsuming()
    {
        // Arrange
        var items = new[] { ("Item A", 1), ("Item B", 1), ("Item C", 1) };
        var bag = new ShuffleBag<string>(items);

        var initialRemaining = bag.Remaining;

        // Act
        var peekedItem = bag.Peek();
        var remainingAfterPeek = bag.Remaining;
        var nextItem = bag.Next();

        // Assert
        Assert.AreEqual(nextItem, peekedItem);
        Assert.AreEqual(initialRemaining, remainingAfterPeek);
    }

    [TestMethod]
    public void Peek_MultipleCalls_ReturnSameItem()
    {
        // Arrange
        var items = new[] { ("Item A", 1), ("Item B", 1), ("Item C", 1) };
        var bag = new ShuffleBag<string>(items);

        // Act
        var firstPeek = bag.Peek();
        var secondPeek = bag.Peek();
        var thirdPeek = bag.Peek();

        // Assert
        Assert.AreEqual(firstPeek, secondPeek);
        Assert.AreEqual(firstPeek, thirdPeek);
    }

    [TestMethod]
    public void Peek_WhenBagExhausted_RefillsAndReturnsNextItem()
    {
        // Arrange
        var items = new[] { ("Item A", 1) };
        var bag = new ShuffleBag<string>(items);

        // Act & Assert
        // Consume the only item
        var firstNext = bag.Next();
        Assert.AreEqual("Item A", firstNext);
        Assert.AreEqual(0, bag.Remaining);

        // Now the bag is exhausted, Peek should trigger a refill
        var peekedItem = bag.Peek();
        Assert.AreEqual("Item A", peekedItem);
        Assert.AreEqual(1, bag.Remaining); // Remaining should be 1 after refill

        var secondNext = bag.Next();
        Assert.AreEqual("Item A", secondNext);
    }
}
