using System;
using System.Numerics;
using GameUtils.Types.Geometry;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GameUtils.Tests.Geometry;

[TestClass]
public class Polygon2DTests
{
    [TestMethod]
    public void Constructor_EmptyVertices_ThrowsArgumentException()
    {
        Assert.ThrowsExactly<ArgumentException>(() => new Polygon2D(Array.Empty<Vector2>()));
    }

    [TestMethod]
    public void Constructor_ValidVertices_InitializesProperties()
    {
        var vertices = new[]
        {
            new Vector2(0, 0),
            new Vector2(10, 0),
            new Vector2(10, 10),
            new Vector2(0, 10)
        };

        var poly = new Polygon2D(vertices, sort: false);

        Assert.AreEqual(4, poly.Vertices.Length);
        Assert.AreEqual(4, poly.Edges.Length);
        Assert.AreEqual(4, poly.Normals.Length);
        Assert.AreEqual(new Vector2(0, 0), poly.BoundingBox.Min);
        Assert.AreEqual(new Vector2(10, 10), poly.BoundingBox.Max);
    }

    [TestMethod]
    public void Constructor_SortClockwise_SortsVertices()
    {
        var vertices = new[]
        {
            new Vector2(0, 0),
            new Vector2(10, 10),
            new Vector2(10, 0),
            new Vector2(0, 10)
        };

        var poly = new Polygon2D(vertices, sort: true);

        Assert.AreEqual(4, poly.Vertices.Length);
    }

    [TestMethod]
    public void Contains_PointInside_ReturnsTrue()
    {
        var poly = new Polygon2D(new[]
        {
            new Vector2(0, 0),
            new Vector2(10, 0),
            new Vector2(10, 10),
            new Vector2(0, 10)
        }, sort: false);

        Assert.IsTrue(poly.Contains(new Vector2(5, 5)));
    }

    [TestMethod]
    public void Contains_PointOutside_ReturnsFalse()
    {
        var poly = new Polygon2D(new[]
        {
            new Vector2(0, 0),
            new Vector2(10, 0),
            new Vector2(10, 10),
            new Vector2(0, 10)
        }, sort: false);

        Assert.IsFalse(poly.Contains(new Vector2(15, 5)));
    }

    [TestMethod]
    public void TranslateBy_MovesPolygonAndBoundingBox()
    {
        var poly = new Polygon2D(new[]
        {
            new Vector2(0, 0),
            new Vector2(10, 0),
            new Vector2(10, 10),
            new Vector2(0, 10)
        }, sort: false);

        poly.TranslateBy(new Vector2(5, 5));

        Assert.AreEqual(new Vector2(5, 5), poly.Vertices[0]);
        Assert.AreEqual(new Vector2(5, 5), poly.BoundingBox.Min);
        Assert.AreEqual(new Vector2(15, 15), poly.BoundingBox.Max);
    }

    [TestMethod]
    public void Intersects_Polygon2D_DisjointBoundingBox_ReturnsFalse()
    {
        var poly1 = new Polygon2D(new[] { new Vector2(0, 0), new Vector2(10, 0), new Vector2(10, 10), new Vector2(0, 10) }, sort: false);
        var poly2 = new Polygon2D(new[] { new Vector2(20, 20), new Vector2(30, 20), new Vector2(30, 30), new Vector2(20, 30) }, sort: false);

        Assert.IsFalse(poly1.Intersects(poly2));
    }

    [TestMethod]
    public void Intersects_Polygon2D_OverlappingEdges_ReturnsTrue()
    {
        var poly1 = new Polygon2D(new[] { new Vector2(0, 0), new Vector2(10, 0), new Vector2(10, 10), new Vector2(0, 10) }, sort: false);
        var poly2 = new Polygon2D(new[] { new Vector2(5, 5), new Vector2(15, 5), new Vector2(15, 15), new Vector2(5, 15) }, sort: false);

        Assert.IsTrue(poly1.Intersects(poly2));
    }

    [TestMethod]
    public void Intersects_Polygon2D_OverlappingBoundingBoxButNoEdgeIntersection_ReturnsFalse()
    {
        var poly1 = new Polygon2D(new[] { new Vector2(0, 0), new Vector2(20, 0), new Vector2(20, 20), new Vector2(0, 20) }, sort: false);
        var poly2 = new Polygon2D(new[] { new Vector2(5, 5), new Vector2(10, 5), new Vector2(10, 10), new Vector2(5, 10) }, sort: false);

        Assert.IsFalse(poly1.Intersects(poly2));
    }

    [TestMethod]
    public void Intersects_Polygon2D_WithOutPoint_Intersecting_ReturnsTrueAndPoint()
    {
        var poly1 = new Polygon2D(new[] { new Vector2(0, 0), new Vector2(10, 0), new Vector2(10, 10), new Vector2(0, 10) }, sort: false);
        var poly2 = new Polygon2D(new[] { new Vector2(5, -5), new Vector2(15, 5), new Vector2(5, 15) }, sort: false);

        var result = poly1.Intersects(poly2, out var point);

        Assert.IsTrue(result);
        Assert.IsNotNull(point);
    }

    [TestMethod]
    public void Intersects_Polygon2D_WithOutPoint_NonIntersecting_ReturnsFalseAndNull()
    {
        var poly1 = new Polygon2D(new[] { new Vector2(0, 0), new Vector2(10, 0), new Vector2(10, 10), new Vector2(0, 10) }, sort: false);
        var poly2 = new Polygon2D(new[] { new Vector2(20, 20), new Vector2(30, 20), new Vector2(30, 30), new Vector2(20, 30) }, sort: false);

        var result = poly1.Intersects(poly2, out var point);

        Assert.IsFalse(result);
        Assert.IsNull(point);
    }

    [TestMethod]
    public void Intersects_Line_Intersecting_ReturnsTrue()
    {
        var poly = new Polygon2D(new[] { new Vector2(0, 0), new Vector2(10, 0), new Vector2(10, 10), new Vector2(0, 10) }, sort: false);
        var line = new Line(new Vector2(-5, 5), new Vector2(15, 5));

        Assert.IsTrue(poly.Intersects(line));
    }

    [TestMethod]
    public void Intersects_Line_NonIntersecting_ReturnsFalse()
    {
        var poly = new Polygon2D(new[] { new Vector2(0, 0), new Vector2(10, 0), new Vector2(10, 10), new Vector2(0, 10) }, sort: false);
        var line = new Line(new Vector2(-5, -5), new Vector2(15, -5));

        Assert.IsFalse(poly.Intersects(line));
    }

    [TestMethod]
    public void Intersects_Line_WithOutPoint_Intersecting_ReturnsTrueAndPoint()
    {
        var poly = new Polygon2D(new[] { new Vector2(0, 0), new Vector2(10, 0), new Vector2(10, 10), new Vector2(0, 10) }, sort: false);
        var line = new Line(new Vector2(-5, 5), new Vector2(15, 5));

        var result = poly.Intersects(line, out var point);

        Assert.IsTrue(result);
        Assert.IsNotNull(point);
        Assert.AreEqual(new Vector2(10, 5), point.Value);
    }

    [TestMethod]
    public void Intersects_Line_WithOutPoint_NonIntersecting_ReturnsFalseAndNull()
    {
        var poly = new Polygon2D(new[] { new Vector2(0, 0), new Vector2(10, 0), new Vector2(10, 10), new Vector2(0, 10) }, sort: false);
        var line = new Line(new Vector2(-5, -5), new Vector2(15, -5));

        var result = poly.Intersects(line, out var point);

        Assert.IsFalse(result);
        Assert.IsNull(point);
    }
}
