using Microsoft.VisualStudio.TestTools.UnitTesting;
using GameUtils.Types.Geometry;
using System.Numerics;

namespace GameUtils.Tests.Geometry;

[TestClass]
public class CircleTests
{
    [TestMethod]
    public void Contains_PointInsideCircle_ReturnsTrue()
    {
        var circle = new Circle(new Vector2(0, 0), 5);
        var point = new Vector2(2, 2);

        Assert.IsTrue(circle.Contains(point));
    }

    [TestMethod]
    public void Contains_PointOnEdge_ReturnsTrue()
    {
        var circle = new Circle(new Vector2(0, 0), 5);
        var point = new Vector2(5, 0);

        Assert.IsTrue(circle.Contains(point));
    }

    [TestMethod]
    public void Contains_PointOutsideCircle_ReturnsFalse()
    {
        var circle = new Circle(new Vector2(0, 0), 5);
        var point = new Vector2(6, 6);

        Assert.IsFalse(circle.Contains(point));
    }
}
