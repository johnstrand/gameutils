using Microsoft.VisualStudio.TestTools.UnitTesting;
using GameUtils.Math;
using System.Numerics;

namespace GameUtils.Tests.Math;

[TestClass]
public class Vector3ExtTests
{
    private const float Tolerance = 0.0001f;
  
    [TestMethod]
    public void IsZero_ZeroVector_ReturnsTrue()
    {
        Vector3 vector = Vector3.Zero;
        Assert.IsTrue(vector.IsZero());
    }

    [TestMethod]
    public void IsZero_NonZeroVector_ReturnsFalse()
    {
        Assert.IsFalse(new Vector3(1f, 0f, 0f).IsZero());
        Assert.IsFalse(new Vector3(0f, 1f, 0f).IsZero());
        Assert.IsFalse(new Vector3(0f, 0f, 1f).IsZero());
        Assert.IsFalse(new Vector3(1f, 1f, 1f).IsZero());
        Assert.IsFalse(new Vector3(-1f, 0f, 0f).IsZero());
    }

    [TestMethod]
    public void Floor_PositiveValues_ReturnsRoundedDown()
    {
        var vector = new Vector3(1.1f, 2.5f, 3.9f);
        var expected = new Vector3(1f, 2f, 3f);
        var actual = vector.Floor();

        Assert.AreEqual(expected.X, actual.X, Tolerance);
        Assert.AreEqual(expected.Y, actual.Y, Tolerance);
        Assert.AreEqual(expected.Z, actual.Z, Tolerance);
    }

    [TestMethod]
    public void Floor_NegativeValues_ReturnsRoundedDown()
    {
        var vector = new Vector3(-1.1f, -2.5f, -3.9f);
        var expected = new Vector3(-2f, -3f, -4f);
        var actual = vector.Floor();

        Assert.AreEqual(expected.X, actual.X, Tolerance);
        Assert.AreEqual(expected.Y, actual.Y, Tolerance);
        Assert.AreEqual(expected.Z, actual.Z, Tolerance);
    }

    [TestMethod]
    public void Floor_IntegerValues_ReturnsSame()
    {
        var vector = new Vector3(1f, -2f, 0f);
        var expected = new Vector3(1f, -2f, 0f);
        var actual = vector.Floor();

        Assert.AreEqual(expected.X, actual.X, Tolerance);
        Assert.AreEqual(expected.Y, actual.Y, Tolerance);
        Assert.AreEqual(expected.Z, actual.Z, Tolerance);
    }
  
    [TestMethod]
    public void XY_ValidVector3_ReturnsVector2WithXAndY()
    {
        var vector = new Vector3(1f, 2f, 3f);
        var result = vector.XY();

        Assert.AreEqual(new Vector2(1f, 2f), result);
    }
}
