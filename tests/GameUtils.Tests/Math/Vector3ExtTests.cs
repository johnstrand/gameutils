using Microsoft.VisualStudio.TestTools.UnitTesting;
using GameUtils.Math;
using System.Numerics;

namespace GameUtils.Tests.Math;

[TestClass]
public class Vector3ExtTests
{
    [TestMethod]
    public void IsZero_ZeroVector_ReturnsTrue()
    {
        Vector3 vector = Vector3.Zero;
        Assert.IsTrue(vector.IsZero());
    }

    [TestMethod]
    public void IsZero_NonZeroVector_ReturnsFalse()
    {
        Assert.IsFalse(new Vector3(1f, 0f, 0f).IsZero());
        Assert.IsFalse(new Vector3(0f, 1f, 0f).IsZero());
        Assert.IsFalse(new Vector3(0f, 0f, 1f).IsZero());
        Assert.IsFalse(new Vector3(1f, 1f, 1f).IsZero());
        Assert.IsFalse(new Vector3(-1f, 0f, 0f).IsZero());
    }
}
