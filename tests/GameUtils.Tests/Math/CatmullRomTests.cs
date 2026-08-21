using Microsoft.VisualStudio.TestTools.UnitTesting;
using GameUtils.Math;
using System.Numerics;

namespace GameUtils.Tests.Math;

[TestClass]
public class CatmullRomTests
{
    [TestMethod]
    public void Tangent_TIsZero_ReturnsTangentAtStartPoint()
    {
        var p0 = new Vector2(0, 0);
        var p1 = new Vector2(10, 0);
        var p2 = new Vector2(20, 0);
        var p3 = new Vector2(30, 0);

        var result = CatmullRom.Tangent(p0, p1, p2, p3, 0f);
        Assert.AreEqual(new Vector2(10, 0), result);
    }

    [TestMethod]
    public void Tangent_TIsOne_ReturnsTangentAtEndPoint()
    {
        var p0 = new Vector2(0, 0);
        var p1 = new Vector2(10, 0);
        var p2 = new Vector2(20, 0);
        var p3 = new Vector2(30, 0);

        var result = CatmullRom.Tangent(p0, p1, p2, p3, 1f);
        Assert.AreEqual(new Vector2(10, 0), result);
    }

    [TestMethod]
    public void Tangent_TIsHalf_ReturnsTangentAtMidPoint()
    {
        var p0 = new Vector2(0, 0);
        var p1 = new Vector2(0, 10);
        var p2 = new Vector2(10, 10);
        var p3 = new Vector2(10, 0);

        var result = CatmullRom.Tangent(p0, p1, p2, p3, 0.5f);
        Assert.AreEqual(new Vector2(12.5f, 0f), result);
    }

    [TestMethod]
    public void Sample_TIsZero_ReturnsStartPoint()
    {
        var p0 = new Vector2(0, 0);
        var p1 = new Vector2(10, 0);
        var p2 = new Vector2(20, 0);
        var p3 = new Vector2(30, 0);

        var result = CatmullRom.Sample(p0, p1, p2, p3, 0f);
        Assert.AreEqual(p1, result);
    }

    [TestMethod]
    public void Sample_TIsOne_ReturnsEndPoint()
    {
        var p0 = new Vector2(0, 0);
        var p1 = new Vector2(10, 0);
        var p2 = new Vector2(20, 0);
        var p3 = new Vector2(30, 0);

        var result = CatmullRom.Sample(p0, p1, p2, p3, 1f);
        Assert.AreEqual(p2, result);
    }
}
