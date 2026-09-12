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
    public void Constructor_WithSortTrue_SortsClockwise()
    {
        Vector2[] vertices = [
            new Vector2(0, 0),
            new Vector2(10, 10),
            new Vector2(0, 10),
            new Vector2(10, 0)
        ];

        var polygon = new Polygon2D(vertices, sort: true);

        Assert.AreEqual(4, polygon.Vertices.Length);
        Assert.AreEqual(4, polygon.Edges.Length);
        Assert.AreEqual(4, polygon.Normals.Length);
        Assert.AreEqual(new Vector2(0, 0), polygon.BoundingBox.Min);
        Assert.AreEqual(new Vector2(10, 10), polygon.BoundingBox.Max);
    }

    [TestMethod]
    public void Constructor_WithSortFalse_PreservesOrder()
    {
        Vector2[] vertices = [
            new Vector2(0, 0),
            new Vector2(10, 0),
            new Vector2(10, 10),
            new Vector2(0, 10)
        ];

        var polygon = new Polygon2D(vertices, sort: false);

        Assert.AreEqual(new Vector2(0, 0), polygon.Vertices[0]);
        Assert.AreEqual(new Vector2(10, 0), polygon.Vertices[1]);
        Assert.AreEqual(new Vector2(10, 10), polygon.Vertices[2]);
        Assert.AreEqual(new Vector2(0, 10), polygon.Vertices[3]);
    }

    [TestMethod]
    public void Contains_PointOutsideBoundingBox_ReturnsFalse()
    {
        Vector2[] vertices = [
            new Vector2(0, 0),
            new Vector2(10, 0),
            new Vector2(10, 10),
            new Vector2(0, 10)
        ];
        var polygon = new Polygon2D(vertices, sort: false);

        Assert.IsFalse(polygon.Contains(new Vector2(20, 20)));
        Assert.IsFalse(polygon.Contains(new Vector2(-5, 5)));
        Assert.IsFalse(polygon.Contains(new Vector2(5, 15)));
    }

    [TestMethod]
    public void Contains_PointInsideBoundingBoxButOutsidePolygon_ReturnsFalse()
    {
        Vector2[] vertices = [
            new Vector2(0, 0),
            new Vector2(10, 0),
            new Vector2(0, 10)
        ];
        var polygon = new Polygon2D(vertices, sort: false);

        Assert.IsFalse(polygon.Contains(new Vector2(8, 8)));
    }

    [TestMethod]
    public void Contains_PointInsidePolygon_ReturnsTrue()
    {
        Vector2[] vertices = [
            new Vector2(0, 0),
            new Vector2(10, 0),
            new Vector2(10, 10),
            new Vector2(0, 10)
        ];
        var polygon = new Polygon2D(vertices, sort: false);

        Assert.IsTrue(polygon.Contains(new Vector2(5, 5)));
        Assert.IsTrue(polygon.Contains(new Vector2(2, 8)));
    }

    [TestMethod]
    public void Contains_RaycastIntersectingVertex_HandledCorrectly()
    {
        Vector2[] vertices = [
            new Vector2(5, 0),
            new Vector2(10, 5),
            new Vector2(5, 10),
            new Vector2(0, 5)
        ];
        var polygon = new Polygon2D(vertices, sort: false);

        Assert.IsTrue(polygon.Contains(new Vector2(2, 5)));
        Assert.IsFalse(polygon.Contains(new Vector2(-2, 5)));
    }

    [TestMethod]
    public void Contains_ConcavePolygon_HandledCorrectly()
    {
        Vector2[] vertices = [
            new Vector2(0, 0),
            new Vector2(10, 0),
            new Vector2(10, 4),
            new Vector2(4, 4),
            new Vector2(4, 10),
            new Vector2(0, 10)
        ];
        var polygon = new Polygon2D(vertices, sort: false);

        Assert.IsTrue(polygon.Contains(new Vector2(2, 2)));
        Assert.IsTrue(polygon.Contains(new Vector2(2, 8)));
        Assert.IsTrue(polygon.Contains(new Vector2(8, 2)));
        Assert.IsFalse(polygon.Contains(new Vector2(8, 8)));
    }

    [TestMethod]
    public void TranslateBy_TranslatesVerticesEdgesAndBoundingBox()
    {
        Vector2[] vertices = [
            new Vector2(0, 0),
            new Vector2(10, 0),
            new Vector2(10, 10),
            new Vector2(0, 10)
        ];
        var polygon = new Polygon2D(vertices, sort: false);

        polygon.TranslateBy(new Vector2(5, 5));

        Assert.AreEqual(new Vector2(5, 5), polygon.Vertices[0]);
        Assert.AreEqual(new Vector2(15, 5), polygon.Vertices[1]);
        Assert.AreEqual(new Vector2(15, 15), polygon.Vertices[2]);
        Assert.AreEqual(new Vector2(5, 15), polygon.Vertices[3]);
        Assert.AreEqual(new Vector2(5, 5), polygon.BoundingBox.Min);
        Assert.AreEqual(new Vector2(15, 15), polygon.BoundingBox.Max);
        Assert.IsTrue(polygon.Contains(new Vector2(10, 10)));
        Assert.IsFalse(polygon.Contains(new Vector2(2, 2)));
    }

    [TestMethod]
    public void TranslateBy_ValidTranslation_UpdatesVerticesEdgesAndBoundingBox()
    {
        var vertices = new[]
        {
            new Vector2(0, 0),
            new Vector2(10, 0),
            new Vector2(10, 10),
            new Vector2(0, 10)
        };
        var polygon = new Polygon2D(vertices, sort: false);
        var translation = new Vector2(5, -3);

        var origMin = polygon.BoundingBox.Min;
        var origMax = polygon.BoundingBox.Max;

        polygon.TranslateBy(translation);

        Assert.AreEqual(new Vector2(5, -3), polygon.Vertices[0]);
        Assert.AreEqual(new Vector2(15, -3), polygon.Vertices[1]);
        Assert.AreEqual(new Vector2(15, 7), polygon.Vertices[2]);
        Assert.AreEqual(new Vector2(5, 7), polygon.Vertices[3]);

        Assert.AreEqual(polygon.Vertices[0], polygon.Edges[0].Start);
        Assert.AreEqual(polygon.Vertices[1], polygon.Edges[0].End);
        Assert.AreEqual(polygon.Vertices[1], polygon.Edges[1].Start);
        Assert.AreEqual(polygon.Vertices[2], polygon.Edges[1].End);
        Assert.AreEqual(polygon.Vertices[2], polygon.Edges[2].Start);
        Assert.AreEqual(polygon.Vertices[3], polygon.Edges[2].End);
        Assert.AreEqual(polygon.Vertices[3], polygon.Edges[3].Start);
        Assert.AreEqual(polygon.Vertices[0], polygon.Edges[3].End);

        Assert.AreEqual(origMin + translation, polygon.BoundingBox.Min);
        Assert.AreEqual(origMax + translation, polygon.BoundingBox.Max);
    }

    [TestMethod]
    public void TranslateBy_ZeroTranslation_VerticesAndBoundingBoxUnchanged()
    {
        var vertices = new[]
        {
            new Vector2(0, 0),
            new Vector2(4, 0),
            new Vector2(2, 4)
        };
        var polygon = new Polygon2D(vertices, sort: false);
        var initialMin = polygon.BoundingBox.Min;
        var initialMax = polygon.BoundingBox.Max;

        polygon.TranslateBy(Vector2.Zero);

        Assert.AreEqual(new Vector2(0, 0), polygon.Vertices[0]);
        Assert.AreEqual(new Vector2(4, 0), polygon.Vertices[1]);
        Assert.AreEqual(new Vector2(2, 4), polygon.Vertices[2]);
        Assert.AreEqual(initialMin, polygon.BoundingBox.Min);
        Assert.AreEqual(initialMax, polygon.BoundingBox.Max);
    }

    [TestMethod]
    public void TranslateBy_MultipleTranslations_AccumulatesCorrectly()
    {
        var vertices = new[]
        {
            new Vector2(1, 1),
            new Vector2(3, 1),
            new Vector2(2, 3)
        };
        var polygon = new Polygon2D(vertices, sort: false);

        polygon.TranslateBy(new Vector2(10, 20));
        polygon.TranslateBy(new Vector2(-5, -10));

        Assert.AreEqual(new Vector2(6, 11), polygon.Vertices[0]);
        Assert.AreEqual(new Vector2(8, 11), polygon.Vertices[1]);
        Assert.AreEqual(new Vector2(7, 13), polygon.Vertices[2]);

        Assert.AreEqual(new Vector2(6, 11), polygon.BoundingBox.Min);
        Assert.AreEqual(new Vector2(8, 13), polygon.BoundingBox.Max);
    }

    [TestMethod]
    public void Intersects_Polygon_ReturnsTrueWhenOverlapping()
    {
        var p1 = new Polygon2D([new Vector2(0, 0), new Vector2(10, 0), new Vector2(10, 10), new Vector2(0, 10)], sort: false);
        var p2 = new Polygon2D([new Vector2(5, 5), new Vector2(15, 5), new Vector2(15, 15), new Vector2(5, 15)], sort: false);
        var p3 = new Polygon2D([new Vector2(20, 20), new Vector2(30, 20), new Vector2(30, 30), new Vector2(20, 30)], sort: false);

        Assert.IsTrue(p1.Intersects(p2));
        Assert.IsFalse(p1.Intersects(p3));
        Assert.IsTrue(p1.Intersects(p2, out var intersectionPoint));
        Assert.IsNotNull(intersectionPoint);
        Assert.IsFalse(p1.Intersects(p3, out var noIntersectionPoint));
        Assert.IsNull(noIntersectionPoint);
    }

    [TestMethod]
    public void Intersects_Polygon2D_DisjointBoundingBoxes_ReturnsFalse()
    {
        var poly1 = new Polygon2D(new[] { new Vector2(0, 0), new Vector2(10, 0), new Vector2(10, 10), new Vector2(0, 10) }, sort: false);
        var poly2 = new Polygon2D(new[] { new Vector2(20, 20), new Vector2(30, 20), new Vector2(30, 30), new Vector2(20, 30) }, sort: false);

        Assert.IsFalse(poly1.Intersects(poly2));
    }

    [TestMethod]
    public void Intersects_Polygon2D_OverlappingEdges_ReturnsTrue()
    {
        var poly1 = new Polygon2D(new[] { new Vector2(0, 0), new Vector2(10, 0), new Vector2(10, 10), new Vector2(0, 10) }, sort: false);
        var poly2 = new Polygon2D(new[] { new Vector2(5, -5), new Vector2(15, 5), new Vector2(5, 15) }, sort: false);

        Assert.IsTrue(poly1.Intersects(poly2));
    }

    [TestMethod]
    public void Intersects_Polygon2D_OneInsideAnotherNoEdgeIntersection_ReturnsFalse()
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
    public void Intersects_Line_ReturnsTrueWhenIntersecting()
    {
        var polygon = new Polygon2D([new Vector2(0, 0), new Vector2(10, 0), new Vector2(10, 10), new Vector2(0, 10)], sort: false);
        var line1 = new Line(new Vector2(-5, 5), new Vector2(15, 5));
        var line2 = new Line(new Vector2(20, 20), new Vector2(30, 30));

        Assert.IsTrue(polygon.Intersects(line1));
        Assert.IsFalse(polygon.Intersects(line2));
        Assert.IsTrue(polygon.Intersects(line1, out var intersectionPoint));
        Assert.IsNotNull(intersectionPoint);
        Assert.IsFalse(polygon.Intersects(line2, out var noIntersectionPoint));
        Assert.IsNull(noIntersectionPoint);
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
