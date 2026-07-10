using Microsoft.VisualStudio.TestTools.UnitTesting;
using GameUtils.Math;

namespace GameUtils.Tests.Math;

[TestClass]
public class MathExtTests
{
    [TestMethod]
    public void RandomFloat_NoArgs_ReturnsValueBetweenZeroAndOne()
    {
        for (int i = 0; i < 1000; i++)
        {
            float result = MathExt.RandomFloat();
            Assert.IsTrue(result >= 0f && result < 1f, $"Result {result} is not in range [0, 1).");
        }
    }

    [TestMethod]
    public void RandomFloat_WithMinAndMax_ReturnsValueBetweenMinAndMax()
    {
        float min = -5.5f;
        float max = 10.5f;
        for (int i = 0; i < 1000; i++)
        {
            float result = MathExt.RandomFloat(min, max);
            Assert.IsTrue(result >= min && result <= max, $"Result {result} is not in range [{min}, {max}].");
        }
    }

    [TestMethod]
    public void RandomFloat_WithMinAndMax_Inverted_ReturnsValueBetweenMaxAndMin()
    {
        float min = 10.5f;
        float max = -5.5f;
        for (int i = 0; i < 1000; i++)
        {
            float result = MathExt.RandomFloat(min, max);
            // Remap handles inverted bounds correctly, mapping 0 to min and 1 to max
            // So if max < min, the result will be between max and min
            Assert.IsTrue(result >= max && result <= min, $"Result {result} is not in range [{max}, {min}].");
        }
    }
}
