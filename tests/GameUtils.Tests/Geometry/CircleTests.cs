using Microsoft.VisualStudio.TestTools.UnitTesting;
using GameUtils.Types.Geometry;
using System.Numerics;

namespace GameUtils.Tests.Geometry;

[TestClass]
public class CircleTests
{
    [TestMethod]
    [DataRow(-0.001f)]
    [DataRow(-1f)]
    [DataRow(-100f)]
    public void Constructor_NegativeRadius_ThrowsArgumentOutOfRangeException(float radius)
    {
        Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => new Circle(new Vector2(0, 0), radius));
    }

    [TestMethod]
    [DataRow(0f, 0f, 0f)]
    [DataRow(3f, 4f, 5f)]
    [DataRow(-10f, 20f, 2.5f)]
    public void Constructor_ValidParameters_PropertiesSetCorrectly(float cx, float cy, float radius)
    {
        var center = new Vector2(cx, cy);
        var circle = new Circle(center, radius);

        Assert.AreEqual(center, circle.Center);
        Assert.AreEqual(radius, circle.Radius);
        Assert.AreEqual(radius * radius, circle.RadiusSquared);
    }

    [TestMethod]
    // Point inside circle
    [DataRow(0f, 0f, 5f, 2f, 2f, true)]
    [DataRow(10f, 10f, 5f, 12f, 12f, true)]
    [DataRow(0f, 0f, 5f, 0f, 0f, true)]
    // Point on edge
    [DataRow(0f, 0f, 5f, 5f, 0f, true)]
    [DataRow(0f, 0f, 5f, 0f, -5f, true)]
    [DataRow(0f, 0f, 5f, -5f, 0f, true)]
    [DataRow(0f, 0f, 5f, 0f, 5f, true)]
    [DataRow(1f, 1f, 5f, 4f, 5f, true)] // 3^2 + 4^2 = 25
    // Point outside circle
    [DataRow(0f, 0f, 5f, 6f, 6f, false)]
    [DataRow(0f, 0f, 5f, 5.1f, 0f, false)]
    [DataRow(10f, 10f, 5f, 20f, 20f, false)]
    // Zero-radius circle
    [DataRow(5f, 5f, 0f, 5f, 5f, true)]
    [DataRow(5f, 5f, 0f, 5.1f, 5f, false)]
    public void Contains_Point_ReturnsExpectedResult(float cx, float cy, float radius, float px, float py, bool expected)
    {
        var circle = new Circle(new Vector2(cx, cy), radius);
        var point = new Vector2(px, py);

        Assert.AreEqual(expected, circle.Contains(point));
    }

    [TestMethod]
    // AABB inside circle
    [DataRow(15f, 15f, 10f, 10f, 10f, 20f, 20f, true)]
    // Circle inside AABB
    [DataRow(15f, 15f, 5f, 0f, 0f, 30f, 30f, true)]
    // Circle intersecting AABB corner / edge
    [DataRow(5f, 5f, 5f, 8f, 8f, 20f, 20f, true)]
    // Circle touching AABB edge
    [DataRow(5f, 15f, 5f, 10f, 10f, 20f, 20f, true)]
    // Circle touching AABB corner
    [DataRow(0f, 0f, 5f, 3f, 4f, 10f, 10f, true)] // dist to corner (3,4) is 5
    // Circle strictly outside AABB
    [DataRow(-10f, -10f, 5f, 10f, 10f, 20f, 20f, false)]
    [DataRow(0f, 0f, 5f, 10f, 0f, 20f, 10f, false)]
    // Zero-radius circle inside or touching AABB
    [DataRow(15f, 15f, 0f, 10f, 10f, 20f, 20f, true)]
    [DataRow(5f, 5f, 0f, 10f, 10f, 20f, 20f, false)]
    public void Intersects_AABB_ReturnsExpectedResult(
        float cx, float cy, float radius,
        float minX, float minY, float maxX, float maxY,
        bool expected)
    {
        var circle = new Circle(new Vector2(cx, cy), radius);
        var aabb = new AABB(new Vector2(minX, minY), new Vector2(maxX, maxY));

        Assert.AreEqual(expected, circle.Intersects(aabb));
    }

    [TestMethod]
    // Passing through center
    [DataRow(0f, 0f, 5f, -10f, 0f, 10f, 0f, true)]
    // Segment completely inside circle
    [DataRow(0f, 0f, 5f, -1f, 0f, 1f, 0f, true)]
    // Line segment tangent to circle
    [DataRow(0f, 0f, 5f, -10f, 5f, 10f, 5f, true)]
    // Line segment outside circle
    [DataRow(0f, 0f, 5f, -10f, 10f, 10f, 10f, false)]
    // Line pointing at circle but short
    [DataRow(0f, 0f, 5f, 10f, 0f, 6f, 0f, false)]
    // Line segment starting inside and ending outside
    [DataRow(0f, 0f, 5f, 0f, 0f, 10f, 0f, true)]
    // Zero length line segment inside / outside
    [DataRow(0f, 0f, 5f, 2f, 2f, 2f, 2f, true)]
    [DataRow(0f, 0f, 5f, 10f, 10f, 10f, 10f, false)]
    public void Intersects_Line_ReturnsExpectedResult(
        float cx, float cy, float radius,
        float x1, float y1, float x2, float y2,
        bool expected)
    {
        var circle = new Circle(new Vector2(cx, cy), radius);
        var line = new Line(new Vector2(x1, y1), new Vector2(x2, y2));

        Assert.AreEqual(expected, circle.Intersects(line));
        Assert.AreEqual(expected, circle.Intersects(new Vector2(x1, y1), new Vector2(x2, y2)));
    }

    [TestMethod]
    // Overlapping circles
    [DataRow(0f, 0f, 5f, 6f, 0f, 5f, true)]
    // Externally tangent circles (touching)
    [DataRow(0f, 0f, 5f, 10f, 0f, 5f, true)]
    // Disjoint circles
    [DataRow(0f, 0f, 5f, 12f, 0f, 5f, false)]
    // Concentric circles (one inside another)
    [DataRow(0f, 0f, 10f, 0f, 0f, 3f, true)]
    // Coincident identical circles
    [DataRow(5f, 5f, 5f, 5f, 5f, 5f, true)]
    // Zero radius circles touching / disjoint
    [DataRow(0f, 0f, 0f, 0f, 0f, 0f, true)]
    [DataRow(0f, 0f, 0f, 1f, 0f, 0f, false)]
    public void Intersects_Circle_ReturnsExpectedResult(
        float c1x, float c1y, float r1,
        float c2x, float c2y, float r2,
        bool expected)
    {
        var circle1 = new Circle(new Vector2(c1x, c1y), r1);
        var circle2 = new Circle(new Vector2(c2x, c2y), r2);

        Assert.AreEqual(expected, circle1.Intersects(circle2));
    }

    [TestMethod]
    public void Intersects_Polygon2D_Inside_ReturnsTrue()
    {
        var circle = new Circle(new Vector2(0, 0), 5f);
        var polyInside = new Polygon2D(new[] { new Vector2(-1, -1), new Vector2(1, -1), new Vector2(0, 1) });

        Assert.IsTrue(circle.Intersects(polyInside));
    }

    [TestMethod]
    public void Intersects_Polygon2D_IntersectingEdgeOrVertex_ReturnsTrue()
    {
        var circle = new Circle(new Vector2(0, 0), 5f);
        var polyIntersecting = new Polygon2D(new[] { new Vector2(4, 0), new Vector2(8, 0), new Vector2(6, 4) });

        Assert.IsTrue(circle.Intersects(polyIntersecting));
    }

    [TestMethod]
    public void Intersects_Polygon2D_Outside_ReturnsFalse()
    {
        var circle = new Circle(new Vector2(0, 0), 5f);
        var polyOutside = new Polygon2D(new[] { new Vector2(10, 10), new Vector2(12, 10), new Vector2(11, 12) });

        Assert.IsFalse(circle.Intersects(polyOutside));
    }

    [TestMethod]
    public void Intersects_Polygon2D_BoundingBoxIntersectsButPolygonEdgesDoNot_ReturnsFalse()
    {
        var circle = new Circle(new Vector2(0, 0), 1f);
        var poly = new Polygon2D(new[] {
            new Vector2(-5, 10),
            new Vector2(10, -5),
            new Vector2(10, 10)
        });

        Assert.IsFalse(circle.Intersects(poly));
    }
}
