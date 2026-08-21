using Microsoft.VisualStudio.TestTools.UnitTesting;
using GameUtils.Math;
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
}
