using System;
using System.Numerics;
using GameUtils.Types.Geometry;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GameUtils.Tests.Geometry;

[TestClass]
public class QuadTests
{
    [TestMethod]
    public void Constructor_Vector2Corners_SetsPropertiesCorrectly()
    {
        var tl = new Vector2(0, 0);
        var tr = new Vector2(10, 0);
        var bl = new Vector2(0, 10);
        var br = new Vector2(10, 10);

        var quad = new Quad(tl, tr, bl, br);

        Assert.AreEqual(tl, quad.TopLeft);
        Assert.AreEqual(tr, quad.TopRight);
        Assert.AreEqual(bl, quad.BottomLeft);
        Assert.AreEqual(br, quad.BottomRight);
    }

    [TestMethod]
    public void Constructor_FloatCorners_SetsPropertiesCorrectly()
    {
        var quad = new Quad(0f, 0f, 10f, 0f, 0f, 10f, 10f, 10f);

        Assert.AreEqual(new Vector2(0, 0), quad.TopLeft);
        Assert.AreEqual(new Vector2(10, 0), quad.TopRight);
        Assert.AreEqual(new Vector2(0, 10), quad.BottomLeft);
        Assert.AreEqual(new Vector2(10, 10), quad.BottomRight);
    }

    [TestMethod]
    public void Constructor_FloatPositionAndSize_SetsPropertiesCorrectly()
    {
        var quad = new Quad(5f, 5f, 10f, 20f);

        Assert.AreEqual(new Vector2(5, 5), quad.TopLeft);
        Assert.AreEqual(new Vector2(15, 5), quad.TopRight);
        Assert.AreEqual(new Vector2(5, 25), quad.BottomLeft);
        Assert.AreEqual(new Vector2(15, 25), quad.BottomRight);
    }

    [TestMethod]
    public void Constructor_Vector2PositionAndSize_SetsPropertiesCorrectly()
    {
        var pos = new Vector2(5, 5);
        var size = new Vector2(10, 20);

        var quad = new Quad(pos, size);

        Assert.AreEqual(new Vector2(5, 5), quad.TopLeft);
        Assert.AreEqual(new Vector2(15, 5), quad.TopRight);
        Assert.AreEqual(new Vector2(5, 25), quad.BottomLeft);
        Assert.AreEqual(new Vector2(15, 25), quad.BottomRight);
    }

    [TestMethod]
    public void Indexer_ValidIndices_ReturnsCorrectCorners()
    {
        var quad = new Quad(new Vector2(0, 0), new Vector2(10, 0), new Vector2(0, 10), new Vector2(10, 10));

        Assert.AreEqual(quad.TopLeft, quad[0]);
        Assert.AreEqual(quad.TopRight, quad[1]);
        Assert.AreEqual(quad.BottomLeft, quad[2]);
        Assert.AreEqual(quad.BottomRight, quad[3]);
    }

    [TestMethod]
    public void Indexer_InvalidIndices_ThrowsArgumentOutOfRangeException()
    {
        var quad = new Quad(0, 0, 10, 10);

        Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => _ = quad[-1]);
        Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => _ = quad[4]);
    }

    [TestMethod]
    public void Intersects_LineIntersectingQuad_ReturnsTrueAndNearestPoint()
    {
        var quad = new Quad(0, 0, 10, 10);
        var line = new Line(new Vector2(-5, 5), new Vector2(15, 5));

        bool intersects = quad.Intersects(line, out var nearest);

        Assert.IsTrue(intersects);
        Assert.IsNotNull(nearest);
        Assert.AreEqual(new Vector2(0, 5), nearest.Value);
    }

    [TestMethod]
    public void Intersects_LineOutsideQuad_ReturnsFalseAndNull()
    {
        var quad = new Quad(0, 0, 10, 10);
        var line = new Line(new Vector2(-5, -5), new Vector2(-5, 15));

        bool intersects = quad.Intersects(line, out var nearest);

        Assert.IsFalse(intersects);
        Assert.IsNull(nearest);
    }
}
