namespace GameUtils.Tests.Math;

using GameUtils.Math;

[TestClass]
public class ShuffleBagTests
{
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
