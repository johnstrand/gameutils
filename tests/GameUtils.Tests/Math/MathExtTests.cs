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
