using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GameUtils.Math;

namespace GameUtils.Tests.Math;

[TestClass]
public class MathExtTests
{
    [TestMethod]
    public void RandomInt_ValidRange_ReturnsValueInRange()
    {
        // Arrange
        int min = 10;
        int max = 20;

        // Act & Assert
        // Test multiple times to ensure we cover the range behavior consistently.
        for (int i = 0; i < 100; i++)
        {
            int result = MathExt.RandomInt(min, max);
            Assert.IsTrue(result >= min && result < max, $"Expected {result} to be in range [{min}, {max})");
        }
    }

    [TestMethod]
    public void RandomInt_MinEqualsMax_ReturnsMin()
    {
        // Arrange
        int min = 5;
        int max = 5;

        // Act
        int result = MathExt.RandomInt(min, max);

        // Assert
        Assert.AreEqual(min, result);
    }

    [TestMethod]
    public void RandomInt_MinGreaterThanMax_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        int min = 10;
        int max = 5;

        // Act & Assert
        Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => MathExt.RandomInt(min, max));
    }
}
