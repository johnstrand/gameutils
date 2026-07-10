using Microsoft.VisualStudio.TestTools.UnitTesting;
using GameUtils.Math;
using System.Numerics;

namespace GameUtils.Tests.Math;

[TestClass]
public class Vector3ExtTests
{
    private const float Tolerance = 0.0001f;

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
}
