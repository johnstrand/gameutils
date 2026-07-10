using Microsoft.VisualStudio.TestTools.UnitTesting;
using GameUtils.Math;
using System;
using System.Numerics;

namespace GameUtils.Tests.Math;

[TestClass]
public class MathExtTests
{
    private const float Tolerance = 0.0001f;

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
