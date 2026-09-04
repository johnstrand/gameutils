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
}
