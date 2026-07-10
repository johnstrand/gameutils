using Microsoft.VisualStudio.TestTools.UnitTesting;
using GameUtils.Math;
using System.Numerics;

namespace GameUtils.Tests.Math;

[TestClass]
public class Vector2ExtTests
{
    private const float Tolerance = 0.0001f;

    [TestMethod]
    public void Perpendicular_ValidVector_ReturnsPerpendicularVector()
    {
        // Arrange
        var vector = new Vector2(3f, 4f);

        // Act
        var result = vector.Perpendicular();

        // Assert
        Assert.AreEqual(-4f, result.X, Tolerance);
        Assert.AreEqual(3f, result.Y, Tolerance);
    }

    [TestMethod]
    public void Perpendicular_ZeroVector_ReturnsZeroVector()
    {
        // Arrange
        var vector = Vector2.Zero;

        // Act
        var result = vector.Perpendicular();

        // Assert
        Assert.AreEqual(0f, result.X, Tolerance);
        Assert.AreEqual(0f, result.Y, Tolerance);
    }

    [TestMethod]
    public void Perpendicular_NegativeVector_ReturnsPerpendicularVector()
    {
        // Arrange
        var vector = new Vector2(-2f, -5f);

        // Act
        var result = vector.Perpendicular();

        // Assert
        Assert.AreEqual(5f, result.X, Tolerance);
        Assert.AreEqual(-2f, result.Y, Tolerance);
    }

    [TestMethod]
    public void Perpendicular_AxisAlignedVector_ReturnsPerpendicularVector()
    {
        // Arrange
        var vector = new Vector2(1f, 0f);

        // Act
        var result = vector.Perpendicular();

        // Assert
        Assert.AreEqual(0f, result.X, Tolerance);
        Assert.AreEqual(1f, result.Y, Tolerance);

        // Act
        var result2 = new Vector2(0f, 1f).Perpendicular();

        // Assert
        Assert.AreEqual(-1f, result2.X, Tolerance);
        Assert.AreEqual(0f, result2.Y, Tolerance);
    }
}
