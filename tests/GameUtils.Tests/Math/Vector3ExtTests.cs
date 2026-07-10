using Microsoft.VisualStudio.TestTools.UnitTesting;
using GameUtils.Math;
using System.Numerics;

namespace GameUtils.Tests.Math;

[TestClass]
public class Vector3ExtTests
{
    [TestMethod]
    public void XY_ValidVector3_ReturnsVector2WithXAndY()
    {
        var vector = new Vector3(1f, 2f, 3f);
        var result = vector.XY();

        Assert.AreEqual(new Vector2(1f, 2f), result);
    }
}
