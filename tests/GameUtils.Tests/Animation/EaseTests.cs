using GameUtils.Animation;

namespace GameUtils.Tests.Animation;

[TestClass]
public class EaseTests
{
    private const float Epsilon = 0.0001f;

    [TestMethod]
    [DataRow(0f)]
    [DataRow(1f)]
    public void QuinticBounceIn_KnownBounds_ReturnsBound(float x)
    {
        Assert.AreEqual(x, Ease.QuinticBounceIn(x), Epsilon);
    }

    [TestMethod]
    [DataRow(0f)]
    [DataRow(1f)]
    public void QuinticBounceOut_KnownBounds_ReturnsBound(float x)
    {
        Assert.AreEqual(x, Ease.QuinticBounceOut(x), Epsilon);
    }

    [TestMethod]
    [DataRow(0f)]
    [DataRow(1f)]
    public void QuinticBounceInOut_KnownBounds_ReturnsBound(float x)
    {
        Assert.AreEqual(x, Ease.QuinticBounceInOut(x), Epsilon);
    }

    [TestMethod]
    [DataRow(-1f)]
    [DataRow(2f)]
    public void AllFunctions_OutOfBounds_DoesNotReturnNaN(float x)
    {
        Assert.IsFalse(float.IsNaN(Ease.QuinticBounceIn(x)));
        Assert.IsFalse(float.IsNaN(Ease.QuinticBounceOut(x)));
        Assert.IsFalse(float.IsNaN(Ease.QuinticBounceInOut(x)));
    }

    [TestMethod]
    public void QuinticBounceIn_KnownValues_ReturnsExpected()
    {
        Assert.AreEqual(-0.11560185f, Ease.QuinticBounceIn(0.25f), Epsilon);
        Assert.AreEqual(-0.27479362f, Ease.QuinticBounceIn(0.5f), Epsilon);
        Assert.AreEqual(-0.09808424f, Ease.QuinticBounceIn(0.75f), Epsilon);
    }

    [TestMethod]
    public void QuinticBounceOut_KnownValues_ReturnsExpected()
    {
        Assert.AreEqual(1.0980842f, Ease.QuinticBounceOut(0.25f), Epsilon);
        Assert.AreEqual(1.2747936f, Ease.QuinticBounceOut(0.5f), Epsilon);
        Assert.AreEqual(1.1156019f, Ease.QuinticBounceOut(0.75f), Epsilon);
    }

    [TestMethod]
    public void QuinticBounceInOut_KnownValues_ReturnsExpected()
    {
        Assert.AreEqual(1.0791204f, Ease.QuinticBounceInOut(0.25f), Epsilon);
        Assert.AreEqual(1.0810952f, Ease.QuinticBounceInOut(0.5f), Epsilon);
        Assert.AreEqual(0.6035781f, Ease.QuinticBounceInOut(0.75f), Epsilon);
    }
}
