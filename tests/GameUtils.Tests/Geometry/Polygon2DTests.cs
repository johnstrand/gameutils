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
}
