using Microsoft.VisualStudio.TestTools.UnitTesting;
using GameUtils.Math;
using System.Numerics;

namespace GameUtils.Tests.Math;

[TestClass]
public class Vector2ExtTests
{
    [TestMethod]
    public void ToVector3_DefaultZ_ReturnsVector3WithZeroZ()
    {
        var vector2 = new Vector2(3.5f, -2.1f);
        var expected = new Vector3(3.5f, -2.1f, 0f);

        var result = vector2.ToVector3();

        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void ToVector3_CustomZ_ReturnsVector3WithCustomZ()
    {
        var vector2 = new Vector2(7.8f, 1.2f);
        var customZ = 4.5f;
        var expected = new Vector3(7.8f, 1.2f, 4.5f);

        var result = vector2.ToVector3(customZ);

        Assert.AreEqual(expected, result);
    }
}
