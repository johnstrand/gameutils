using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GameUtils.Math;
using System.Numerics;

namespace GameUtils.Tests.Math;

[TestClass]
public class MathExtTests
{
    private const float Tolerance = 0.0001f;

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

    [TestMethod]
    public void RandomBool_ProbabilityZero_AlwaysReturnsFalse()
    {
        for (int i = 0; i < 100; i++)
        {
            Assert.IsFalse(MathExt.RandomBool(0f));
        }
    }

    [TestMethod]
    public void RandomBool_ProbabilityOne_AlwaysReturnsTrue()
    {
        for (int i = 0; i < 100; i++)
        {
            Assert.IsTrue(MathExt.RandomBool(1f));
        }
    }

    [TestMethod]
    public void RandomBool_NegativeProbability_AlwaysReturnsFalse()
    {
        for (int i = 0; i < 100; i++)
        {
            Assert.IsFalse(MathExt.RandomBool(-0.5f));
        }
    }

    [TestMethod]
    public void RandomBool_GreaterThanOneProbability_AlwaysReturnsTrue()
    {
        for (int i = 0; i < 100; i++)
        {
            Assert.IsTrue(MathExt.RandomBool(1.5f));
        }
    }

    [TestMethod]
    public void RandomBool_DefaultProbability_ReturnsBothTrueAndFalseEventually()
    {
        bool sawTrue = false;
        bool sawFalse = false;

        for (int i = 0; i < 1000; i++)
        {
            if (MathExt.RandomBool()) sawTrue = true;
            else sawFalse = true;

            if (sawTrue && sawFalse) break;
        }

        Assert.IsTrue(sawTrue);
        Assert.IsTrue(sawFalse);
    }

    [TestMethod]
    public void RandomFloat_NoArgs_ReturnsValueBetweenZeroAndOne()
    {
        for (int i = 0; i < 1000; i++)
        {
            float result = MathExt.RandomFloat();
            Assert.IsTrue(result >= 0f && result < 1f, $"Result {result} is not in range [0, 1).");
        }
    }

    [TestMethod]
    public void RandomFloat_WithMinAndMax_ReturnsValueBetweenMinAndMax()
    {
        float min = -5.5f;
        float max = 10.5f;
        for (int i = 0; i < 1000; i++)
        {
            float result = MathExt.RandomFloat(min, max);
            Assert.IsTrue(result >= min && result <= max, $"Result {result} is not in range [{min}, {max}].");
        }
    }

    [TestMethod]
    public void RandomFloat_WithMinAndMax_Inverted_ReturnsValueBetweenMaxAndMin()
    {
        float min = 10.5f;
        float max = -5.5f;
        for (int i = 0; i < 1000; i++)
        {
            float result = MathExt.RandomFloat(min, max);
            // Remap handles inverted bounds correctly, mapping 0 to min and 1 to max
            // So if max < min, the result will be between max and min
            Assert.IsTrue(result >= max && result <= min, $"Result {result} is not in range [{max}, {min}].");
        }
    }

    [TestMethod]
    public void RandomInCircle_DefaultRadius_ReturnsPointWithinUnitCircle()
    {
        for (int i = 0; i < 100; i++)
        {
            Vector2 result = MathExt.RandomInCircle();
            float distance = result.Length();
            Assert.IsTrue(distance <= 1f + Tolerance, $"Expected point within circle of radius 1, but length was {distance}.");
        }
    }

    [TestMethod]
    public void RandomInCircle_CustomRadius_ReturnsPointWithinCustomCircle()
    {
        float radius = 5f;
        for (int i = 0; i < 100; i++)
        {
            Vector2 result = MathExt.RandomInCircle(radius);
            float distance = result.Length();
            Assert.IsTrue(distance <= radius + Tolerance, $"Expected point within circle of radius {radius}, but length was {distance}.");
        }
    }

    [TestMethod]
    public void RandomInCircle_ZeroRadius_ReturnsOrigin()
    {
        Vector2 result = MathExt.RandomInCircle(0f);
        Assert.AreEqual(0f, result.X, Tolerance);
        Assert.AreEqual(0f, result.Y, Tolerance);
    }
}
