using Microsoft.VisualStudio.TestTools.UnitTesting;
using GameUtils.Math;
using System.Numerics;

namespace GameUtils.Tests.Math;

[TestClass]
public class CatmullRomTests
{
    [TestMethod]
    public void Sample_TIsZero_ReturnsP1()
    {
        var p0 = new Vector2(0, 0);
        var p1 = new Vector2(5, 5);
        var p2 = new Vector2(10, 0);
        var p3 = new Vector2(15, -5);

        var result = CatmullRom.Sample(p0, p1, p2, p3, 0f);

        Assert.AreEqual(p1, result);
    }

    [TestMethod]
    public void Sample_TIsOne_ReturnsP2()
    {
        var p0 = new Vector2(0, 0);
        var p1 = new Vector2(5, 5);
        var p2 = new Vector2(10, 0);
        var p3 = new Vector2(15, -5);

        var result = CatmullRom.Sample(p0, p1, p2, p3, 1f);

        Assert.AreEqual(p2, result);
    }

    [TestMethod]
    public void Sample_TIsHalf_ReturnsInterpolatedMidpoint()
    {
        var p0 = new Vector2(0, 0);
        var p1 = new Vector2(0, 0);
        var p2 = new Vector2(10, 0);
        var p3 = new Vector2(10, 0);

        var result = CatmullRom.Sample(p0, p1, p2, p3, 0.5f);

        Assert.AreEqual(new Vector2(5, 0), result);
    }

    [TestMethod]
    public void Sample_TIsOutOfBounds_ExtrapolatesCorrectly()
    {
        var p0 = new Vector2(0, 0);
        var p1 = new Vector2(5, 0);
        var p2 = new Vector2(10, 0);
        var p3 = new Vector2(15, 0);

        var result = CatmullRom.Sample(p0, p1, p2, p3, 2f);
        Assert.AreEqual(new Vector2(15, 0), result);

        var result2 = CatmullRom.Sample(p0, p1, p2, p3, -1f);
        Assert.AreEqual(new Vector2(0, 0), result2);
    }
}
