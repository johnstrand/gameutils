using Microsoft.VisualStudio.TestTools.UnitTesting;
using GameUtils.Math;
using System.Numerics;

namespace GameUtils.Tests.Math;

[TestClass]
public class BezierTests
{
    [TestMethod]
    public void Quadratic_TIsZero_ReturnsStartPoint()
    {
        var p0 = new Vector2(0, 0);
        var p1 = new Vector2(5, 5);
        var p2 = new Vector2(10, 0);

        var result = Bezier.Quadratic(p0, p1, p2, 0f);

        Assert.AreEqual(p0, result);
    }

    [TestMethod]
    public void Quadratic_TIsOne_ReturnsEndPoint()
    {
        var p0 = new Vector2(0, 0);
        var p1 = new Vector2(5, 5);
        var p2 = new Vector2(10, 0);

        var result = Bezier.Quadratic(p0, p1, p2, 1f);

        Assert.AreEqual(p2, result);
    }

    [TestMethod]
    public void Quadratic_TIsHalf_ReturnsMidPoint()
    {
        var p0 = new Vector2(0, 0);
        var p1 = new Vector2(5, 10); // Control point pulling it up
        var p2 = new Vector2(10, 0);

        // At t = 0.5, the curve should be halfway horizontally
        // and halfway up towards the control point vertically.
        // P(0.5) = 0.25*P0 + 0.5*P1 + 0.25*P2
        // P(0.5) = 0.25*(0,0) + 0.5*(5,10) + 0.25*(10,0)
        // P(0.5) = (0,0) + (2.5,5) + (2.5,0) = (5,5)

        var result = Bezier.Quadratic(p0, p1, p2, 0.5f);

        Assert.AreEqual(new Vector2(5, 5), result);
    }

    [TestMethod]
    public void Quadratic_TIsOutOfBounds_ExtrapolatesCorrectly()
    {
        var p0 = new Vector2(0, 0);
        var p1 = new Vector2(5, 0);
        var p2 = new Vector2(10, 0);

        // A straight line bezier for simplicity in calculating extrapolation.
        // P(t) = (1-t)^2*P0 + 2*(1-t)*t*P1 + t^2*P2
        // For t = 2:
        // P(2) = (-1)^2*(0,0) + 2*(-1)*2*(5,0) + 2^2*(10,0)
        // P(2) = (0,0) - 4*(5,0) + 4*(10,0)
        // P(2) = (0,0) - (20,0) + (40,0) = (20,0)

        var result = Bezier.Quadratic(p0, p1, p2, 2f);

        Assert.AreEqual(new Vector2(20, 0), result);

        // For t = -1:
        // P(-1) = 2^2*P0 + 2*2*(-1)*P1 + (-1)^2*P2
        // P(-1) = 4*(0,0) - 4*(5,0) + 1*(10,0)
        // P(-1) = (0,0) - (20,0) + (10,0) = (-10,0)

        var result2 = Bezier.Quadratic(p0, p1, p2, -1f);
        Assert.AreEqual(new Vector2(-10, 0), result2);
    }
}
