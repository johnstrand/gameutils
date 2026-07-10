using Microsoft.VisualStudio.TestTools.UnitTesting;
using GameUtils.Math;
using System.Numerics;
using System.Collections.Generic;
using System.Linq;
using System;

namespace GameUtils.Tests.Math;

[TestClass]
public class Vector2ExtTests
{
    private const float Tolerance = 0.0001f;
  
    [TestMethod]
    public void ToVector3_DefaultZ_ReturnsVector3WithZeroZ()
    {
        var vector2 = new Vector2(3.5f, -2.1f);
        var expected = new Vector3(3.5f, -2.1f, 0f);

        var result = vector2.ToVector3();

        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void ToVector3_CustomZ_ReturnsVector3WithCustomZ()
    {
        var vector2 = new Vector2(7.8f, 1.2f);
        var customZ = 4.5f;
        var expected = new Vector3(7.8f, 1.2f, 4.5f);

        var result = vector2.ToVector3(customZ);

        Assert.AreEqual(expected, result);
    }
  
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
  
    [TestMethod]
    public void Sort_Clockwise_SortsVectorsCorrectly()
    {
        // Vectors around (0,0) which is their midpoint
        var vectors = new List<Vector2>
        {
            new Vector2(0, 1),
            new Vector2(-1, 0),
            new Vector2(0, -1),
            new Vector2(1, 0)
        };

        // Midpoint is (0,0)
        var result = vectors.Sort(clockwise: true).ToList();

        // Atan2 relative to (0,0):
        // (0, -1) -> -PI/2
        // (1, 0) -> 0
        // (0, 1) -> PI/2
        // (-1, 0) -> PI
        Assert.AreEqual(new Vector2(0, -1), result[0]);
        Assert.AreEqual(new Vector2(1, 0), result[1]);
        Assert.AreEqual(new Vector2(0, 1), result[2]);
        Assert.AreEqual(new Vector2(-1, 0), result[3]);
    }

    [TestMethod]
    public void Sort_CounterClockwise_SortsVectorsCorrectly()
    {
        var vectors = new List<Vector2>
        {
            new Vector2(0, 1),
            new Vector2(-1, 0),
            new Vector2(0, -1),
            new Vector2(1, 0)
        };

        // Midpoint is (0,0)
        var result = vectors.Sort(clockwise: false).ToList();

        // Atan2 * -1:
        // (-1, 0) -> PI * -1 = -PI
        // (0, 1) -> PI/2 * -1 = -PI/2
        // (1, 0) -> 0 * -1 = 0
        // (0, -1) -> -PI/2 * -1 = PI/2
        Assert.AreEqual(new Vector2(-1, 0), result[0]);
        Assert.AreEqual(new Vector2(0, 1), result[1]);
        Assert.AreEqual(new Vector2(1, 0), result[2]);
        Assert.AreEqual(new Vector2(0, -1), result[3]);
    }

    [TestMethod]
    public void Sort_WithSpecifiedCenter_Clockwise_SortsCorrectly()
    {
        var center = new Vector2(10, 10);
        var vectors = new List<Vector2>
        {
            new Vector2(10, 11), // relative: (0, 1)
            new Vector2(9, 10),  // relative: (-1, 0)
            new Vector2(10, 9),  // relative: (0, -1)
            new Vector2(11, 10)  // relative: (1, 0)
        };

        var result = vectors.Sort(center, clockwise: true).ToList();

        Assert.AreEqual(new Vector2(10, 9), result[0]);
        Assert.AreEqual(new Vector2(11, 10), result[1]);
        Assert.AreEqual(new Vector2(10, 11), result[2]);
        Assert.AreEqual(new Vector2(9, 10), result[3]);
    }

    [TestMethod]
    public void Sort_WithSpecifiedCenter_CounterClockwise_SortsCorrectly()
    {
        var center = new Vector2(10, 10);
        var vectors = new List<Vector2>
        {
            new Vector2(10, 11),
            new Vector2(9, 10),
            new Vector2(10, 9),
            new Vector2(11, 10)
        };

        var result = vectors.Sort(center, clockwise: false).ToList();

        Assert.AreEqual(new Vector2(9, 10), result[0]);
        Assert.AreEqual(new Vector2(10, 11), result[1]);
        Assert.AreEqual(new Vector2(11, 10), result[2]);
        Assert.AreEqual(new Vector2(10, 9), result[3]);
    }

    [TestMethod]
    public void AngleTowards_TargetIsRight_ReturnsZero()
    {
        var source = new Vector2(1f, 1f);
        var target = new Vector2(2f, 1f);
        var expected = 0f;

        var actual = source.AngleTowards(target);

        Assert.AreEqual(expected, actual, Tolerance);
    }

    [TestMethod]
    public void AngleTowards_TargetIsUp_ReturnsPiOverTwo()
    {
        var source = new Vector2(1f, 1f);
        var target = new Vector2(1f, 2f);
        var expected = MathF.PI / 2f;

        var actual = source.AngleTowards(target);

        Assert.AreEqual(expected, actual, Tolerance);
    }

    [TestMethod]
    public void AngleTowards_TargetIsLeft_ReturnsPi()
    {
        var source = new Vector2(1f, 1f);
        var target = new Vector2(0f, 1f);
        var expected = MathF.PI;

        var actual = source.AngleTowards(target);

        Assert.AreEqual(expected, actual, Tolerance);
    }

    [TestMethod]
    public void AngleTowards_TargetIsDown_ReturnsNegativePiOverTwo()
    {
        var source = new Vector2(1f, 1f);
        var target = new Vector2(1f, 0f);
        var expected = -MathF.PI / 2f;

        var actual = source.AngleTowards(target);

        Assert.AreEqual(expected, actual, Tolerance);
    }

    [TestMethod]
    public void AngleTowards_TargetIsUpRight_ReturnsPiOverFour()
    {
        var source = new Vector2(1f, 1f);
        var target = new Vector2(2f, 2f);
        var expected = MathF.PI / 4f;

        var actual = source.AngleTowards(target);

        Assert.AreEqual(expected, actual, Tolerance);
    }

    [TestMethod]
    public void AngleTowards_TargetIsSameAsSource_ReturnsNaN()
    {
        var source = new Vector2(1f, 1f);
        var target = new Vector2(1f, 1f);

        var actual = source.AngleTowards(target);

        Assert.IsTrue(float.IsNaN(actual));
    }

    [TestMethod]
    public void AngleBetween_PerpendicularVectors_ReturnsHalfPi()
    {
        Vector2 a = Vector2.UnitX;
        Vector2 b = Vector2.UnitY;
        Assert.AreEqual(MathF.PI / 2f, a.AngleBetween(b), Tolerance);
    }

    [TestMethod]
    public void AngleBetween_SameVector_ReturnsZero()
    {
        Vector2 a = new Vector2(1, 1);
        Assert.AreEqual(0f, a.AngleBetween(a), Tolerance);
    }

    [TestMethod]
    public void AngleBetween_OppositeVectors_ReturnsPi()
    {
        Vector2 a = Vector2.UnitX;
        Vector2 b = -Vector2.UnitX;
        Assert.AreEqual(MathF.PI, MathF.Abs(a.AngleBetween(b)), Tolerance);
    }

    [TestMethod]
    public void AngleBetween_Clockwise_ReturnsNegative()
    {
        Vector2 a = Vector2.UnitY;
        Vector2 b = Vector2.UnitX;
        Assert.AreEqual(-MathF.PI / 2f, a.AngleBetween(b), Tolerance);
    }

    [TestMethod]
    public void AngleBetween_ZeroVector_ReturnsNaN()
    {
        Vector2 a = Vector2.Zero;
        Vector2 b = Vector2.UnitX;
        Assert.IsTrue(float.IsNaN(a.AngleBetween(b)));
        Assert.IsTrue(float.IsNaN(b.AngleBetween(a)));
    }

    [TestMethod]
    public void GetDirection_DifferentVectors_ReturnsNormalizedDirection()
    {
        var source = new Vector2(1, 1);
        var target = new Vector2(4, 5);

        // Target - Source = (3, 4)
        // Length = 5
        // Normalized = (3/5, 4/5) = (0.6, 0.8)
        var expected = new Vector2(0.6f, 0.8f);
        var result = source.GetDirection(target);

        Assert.AreEqual(expected.X, result.X, Tolerance);
        Assert.AreEqual(expected.Y, result.Y, Tolerance);
    }

    [TestMethod]
    public void GetDirection_SameVectors_ReturnsNaNVector()
    {
        var source = new Vector2(1, 1);
        var target = new Vector2(1, 1);

        var result = source.GetDirection(target);

        Assert.IsTrue(float.IsNaN(result.X));
        Assert.IsTrue(float.IsNaN(result.Y));
    }
}
