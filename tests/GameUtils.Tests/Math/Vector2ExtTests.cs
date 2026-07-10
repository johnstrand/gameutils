using Microsoft.VisualStudio.TestTools.UnitTesting;
using GameUtils.Math;
using System.Numerics;

namespace GameUtils.Tests.Math;

[TestClass]
public class Vector2ExtTests
{
    private const float Tolerance = 0.0001f;

    [TestMethod]
    public void Add_SinglePositiveValue_AddsToBothComponents()
    {
        var vector = new Vector2(1f, 2f);
        var result = vector.Add(3f);

        Assert.AreEqual(4f, result.X, Tolerance);
        Assert.AreEqual(5f, result.Y, Tolerance);
    }

    [TestMethod]
    public void Add_SingleNegativeValue_SubtractsFromBothComponents()
    {
        var vector = new Vector2(1f, 2f);
        var result = vector.Add(-3f);

        Assert.AreEqual(-2f, result.X, Tolerance);
        Assert.AreEqual(-1f, result.Y, Tolerance);
    }

    [TestMethod]
    public void Add_SingleZeroValue_ReturnsUnchanged()
    {
        var vector = new Vector2(1f, 2f);
        var result = vector.Add(0f);

        Assert.AreEqual(1f, result.X, Tolerance);
        Assert.AreEqual(2f, result.Y, Tolerance);
    }

    [TestMethod]
    public void Add_TwoPositiveValues_AddsToRespectiveComponents()
    {
        var vector = new Vector2(1f, 2f);
        var result = vector.Add(3f, 4f);

        Assert.AreEqual(4f, result.X, Tolerance);
        Assert.AreEqual(6f, result.Y, Tolerance);
    }

    [TestMethod]
    public void Add_TwoNegativeValues_SubtractsFromRespectiveComponents()
    {
        var vector = new Vector2(1f, 2f);
        var result = vector.Add(-3f, -4f);

        Assert.AreEqual(-2f, result.X, Tolerance);
        Assert.AreEqual(-2f, result.Y, Tolerance);
    }

    [TestMethod]
    public void Add_TwoZeroValues_ReturnsUnchanged()
    {
        var vector = new Vector2(1f, 2f);
        var result = vector.Add(0f, 0f);

        Assert.AreEqual(1f, result.X, Tolerance);
        Assert.AreEqual(2f, result.Y, Tolerance);
    }
}
