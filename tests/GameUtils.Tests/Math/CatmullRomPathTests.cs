using Microsoft.VisualStudio.TestTools.UnitTesting;
using GameUtils.Math;
using System;
using System.Numerics;

namespace GameUtils.Tests.Math;

[TestClass]
public class CatmullRomPathTests
{
    [TestMethod]
    public void AddPoint_IncrementsPointCount()
    {
        var path = new CatmullRomPath();
        Assert.AreEqual(0, path.PointCount);

        path.AddPoint(new Vector2(1, 2));
        Assert.AreEqual(1, path.PointCount);

        path.AddPoint(new Vector2(3, 4));
        Assert.AreEqual(2, path.PointCount);
    }

    [TestMethod]
    public void AddPoint_ReturnsSameInstance()
    {
        var path = new CatmullRomPath();
        var result = path.AddPoint(new Vector2(1, 2));
        Assert.AreSame(path, result);
    }

    [TestMethod]
    public void AddPoints_AddsMultiplePointsAndReturnsSameInstance()
    {
        var path = new CatmullRomPath();
        var points = new Vector2[] { new Vector2(0, 0), new Vector2(10, 0), new Vector2(20, 0) };
        var result = path.AddPoints(points);

        Assert.AreSame(path, result);
        Assert.AreEqual(3, path.PointCount);
    }

    [TestMethod]
    public void SetLooping_ReturnsSameInstance()
    {
        var path = new CatmullRomPath();
        var result = path.SetLooping(true);

        Assert.AreSame(path, result);
    }

    [TestMethod]
    public void GetPoint_LessThanTwoPoints_ThrowsInvalidOperationException()
    {
        var path = new CatmullRomPath();
        Assert.ThrowsExactly<InvalidOperationException>(() => path.GetPoint(0.5f));

        path.AddPoint(new Vector2(0, 0));
        Assert.ThrowsExactly<InvalidOperationException>(() => path.GetPoint(0.5f));
    }

    [TestMethod]
    public void GetTangent_LessThanTwoPoints_ThrowsInvalidOperationException()
    {
        var path = new CatmullRomPath();
        Assert.ThrowsExactly<InvalidOperationException>(() => path.GetTangent(0.5f));

        path.AddPoint(new Vector2(0, 0));
        Assert.ThrowsExactly<InvalidOperationException>(() => path.GetTangent(0.5f));
    }

    [TestMethod]
    public void GetPoint_NonLooping_EvaluatesCorrectPointsAtEndpointsAndControlPoints()
    {
        var path = new CatmullRomPath()
            .AddPoint(new Vector2(0, 0))
            .AddPoint(new Vector2(10, 0))
            .AddPoint(new Vector2(20, 0))
            .AddPoint(new Vector2(30, 0));

        var pStart = path.GetPoint(0f);
        var pSegment1End = path.GetPoint(1f / 3f);
        var pSegment2End = path.GetPoint(2f / 3f);

        Assert.AreEqual(new Vector2(0, 0), pStart);
        Assert.AreEqual(new Vector2(10, 0), pSegment1End);
        Assert.AreEqual(new Vector2(20, 0), pSegment2End);
    }

    [TestMethod]
    public void GetPoint_Looping_EvaluatesCorrectPointsAtEndpointsAndControlPoints()
    {
        var path = new CatmullRomPath()
            .SetLooping(true)
            .AddPoint(new Vector2(0, 0))
            .AddPoint(new Vector2(10, 0))
            .AddPoint(new Vector2(10, 10))
            .AddPoint(new Vector2(0, 10));

        var p0 = path.GetPoint(0f);
        var p1 = path.GetPoint(0.25f);
        var p2 = path.GetPoint(0.5f);
        var p3 = path.GetPoint(0.75f);

        Assert.AreEqual(new Vector2(0, 0), p0);
        Assert.AreEqual(new Vector2(10, 0), p1);
        Assert.AreEqual(new Vector2(10, 10), p2);
        Assert.AreEqual(new Vector2(0, 10), p3);
    }

    [TestMethod]
    public void GetTangent_NonLooping_EvaluatesTangentCorrectly()
    {
        var path = new CatmullRomPath()
            .AddPoint(new Vector2(0, 0))
            .AddPoint(new Vector2(10, 0))
            .AddPoint(new Vector2(20, 0));

        var tangentStart = path.GetTangent(0f);

        Assert.AreEqual(new Vector2(10, 0), tangentStart);
    }

    [TestMethod]
    public void GetTangent_Looping_EvaluatesTangentCorrectly()
    {
        var path = new CatmullRomPath()
            .SetLooping(true)
            .AddPoint(new Vector2(0, 0))
            .AddPoint(new Vector2(10, 0))
            .AddPoint(new Vector2(10, 10))
            .AddPoint(new Vector2(0, 10));

        var tangentStart = path.GetTangent(0f);
        Assert.AreNotEqual(Vector2.Zero, tangentStart);
    }

    [TestMethod]
    public void SetLooping_True_AlignsPositionAndTangentAtEndpoints()
    {
        var path = new CatmullRomPath()
            .SetLooping(true)
            .AddPoint(new Vector2(0, 0))
            .AddPoint(new Vector2(10, 0))
            .AddPoint(new Vector2(10, 10))
            .AddPoint(new Vector2(0, 10));

        var pStart = path.GetPoint(0f);
        var pEnd = path.GetPoint(1f);
        Assert.AreEqual(pStart, pEnd);

        var tangentStart = path.GetTangent(0f);
        var tangentEnd = path.GetTangent(1f);
        Assert.AreEqual(tangentStart.X, tangentEnd.X, 1e-4f);
        Assert.AreEqual(tangentStart.Y, tangentEnd.Y, 1e-4f);
    }

    [TestMethod]
    public void SetLooping_CanToggleLoopingState()
    {
        var path = new CatmullRomPath()
            .AddPoint(new Vector2(0, 0))
            .AddPoint(new Vector2(10, 0))
            .AddPoint(new Vector2(10, 10))
            .AddPoint(new Vector2(0, 10));

        path.SetLooping(true);
        var pEndLoop = path.GetPoint(1f);
        Assert.AreEqual(new Vector2(0, 0), pEndLoop);

        path.SetLooping(false);
        var pEndNonLoop = path.GetPoint(1f);
        Assert.AreEqual(new Vector2(0, 10), pEndNonLoop);
    }
}
