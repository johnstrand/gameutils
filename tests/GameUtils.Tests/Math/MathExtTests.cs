using Microsoft.VisualStudio.TestTools.UnitTesting;
using GameUtils.Math;

namespace GameUtils.Tests.Math;

[TestClass]
public class MathExtTests
{
    [TestMethod]
    public void RandomBool_ProbabilityZero_AlwaysReturnsFalse()
    {
        for (int i = 0; i < 100; i++)
        {
            Assert.IsFalse(MathExt.RandomBool(0f));
        }
    }

    [TestMethod]
    public void RandomBool_ProbabilityOne_AlwaysReturnsTrue()
    {
        for (int i = 0; i < 100; i++)
        {
            Assert.IsTrue(MathExt.RandomBool(1f));
        }
    }

    [TestMethod]
    public void RandomBool_NegativeProbability_AlwaysReturnsFalse()
    {
        for (int i = 0; i < 100; i++)
        {
            Assert.IsFalse(MathExt.RandomBool(-0.5f));
        }
    }

    [TestMethod]
    public void RandomBool_GreaterThanOneProbability_AlwaysReturnsTrue()
    {
        for (int i = 0; i < 100; i++)
        {
            Assert.IsTrue(MathExt.RandomBool(1.5f));
        }
    }

    [TestMethod]
    public void RandomBool_DefaultProbability_ReturnsBothTrueAndFalseEventually()
    {
        bool sawTrue = false;
        bool sawFalse = false;

        for (int i = 0; i < 1000; i++)
        {
            if (MathExt.RandomBool()) sawTrue = true;
            else sawFalse = true;

            if (sawTrue && sawFalse) break;
        }

        Assert.IsTrue(sawTrue);
        Assert.IsTrue(sawFalse);
    }
}
