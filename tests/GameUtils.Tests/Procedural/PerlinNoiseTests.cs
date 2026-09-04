using GameUtils.Procedural;

namespace GameUtils.Tests.Procedural;

[TestClass]
public class PerlinNoiseTests
{
    [TestMethod]
    public void Default_ReturnsSameValueForSameCoordinates()
    {
        var noise = PerlinNoise.Default;
        var val1 = noise.Sample(1.5f, 2.5f);
        var val2 = noise.Sample(1.5f, 2.5f);

        Assert.AreEqual(val1, val2);
    }

    [TestMethod]
    public void DefaultConstructor_MatchesDefaultInstance()
    {
        var noise1 = new PerlinNoise();
        var noise2 = PerlinNoise.Default;

        Assert.AreEqual(noise2.Sample(1.5f, 2.5f), noise1.Sample(1.5f, 2.5f));
        Assert.AreEqual(noise2.Sample(1.5f, 2.5f, 3.5f), noise1.Sample(1.5f, 2.5f, 3.5f));
    }

    [TestMethod]
    public void DefaultConstructor_ReturnsSameValueForSameCoordinates()
    {
        var noise1 = new PerlinNoise();
        var noise2 = new PerlinNoise();

        var val1 = noise1.Sample(1.5f, 2.5f, 3.5f);
        var val2 = noise2.Sample(1.5f, 2.5f, 3.5f);

        Assert.AreEqual(val1, val2);
    }

    [TestMethod]
    public void SeededConstructor_SameSeedReturnsSameValues()
    {
        var noise1 = new PerlinNoise(42);
        var noise2 = new PerlinNoise(42);

        Assert.AreEqual(noise1.Sample(1.5f, 2.5f), noise2.Sample(1.5f, 2.5f));
        Assert.AreEqual(noise1.Sample(1.5f, 2.5f, 3.5f), noise2.Sample(1.5f, 2.5f, 3.5f));
    }

    [TestMethod]
    public void SeededConstructor_DifferentSeedReturnsDifferentValues()
    {
        var noise1 = new PerlinNoise(42);
        var noise2 = new PerlinNoise(43);

        // While technically possible to have the same value, it's highly improbable for a specific point.
        Assert.AreNotEqual(noise1.Sample(1.5f, 2.5f), noise2.Sample(1.5f, 2.5f));
    }

    [TestMethod]
    public void Sample2D_ReturnsValuesInExpectedRange()
    {
        var noise = PerlinNoise.Default;

        // Check a grid of points to ensure values are generally within [0, 1]
        for (float x = 0; x < 10; x += 0.5f)
        {
            for (float y = 0; y < 10; y += 0.5f)
            {
                var val = noise.Sample(x, y);
                // Note: The comment says "approximately [0, 1]". Perlin noise can sometimes slightly exceed typical bounds depending on implementation,
                // but standard normalized Perlin should stay within these bounds.
                Assert.IsTrue(val >= -0.1f && val <= 1.1f, $"Value {val} at ({x}, {y}) is outside expected approximate range [0, 1]");
            }
        }
    }

    [TestMethod]
    public void Sample3D_ReturnsValuesInExpectedRange()
    {
        var noise = PerlinNoise.Default;

        // Check a grid of points to ensure values are generally within [-1, 1]
        for (float x = 0; x < 5; x += 0.5f)
        {
            for (float y = 0; y < 5; y += 0.5f)
            {
                for (float z = 0; z < 5; z += 0.5f)
                {
                    var val = noise.Sample(x, y, z);
                    // Approximate range [-1, 1]
                    Assert.IsTrue(val >= -1.1f && val <= 1.1f, $"Value {val} at ({x}, {y}, {z}) is outside expected approximate range [-1, 1]");
                }
            }
        }
    }

    [TestMethod]
    public void Fbm_ReturnsValuesInExpectedRangeAndDiffersFromBaseSample()
    {
        var noise = PerlinNoise.Default;

        var baseVal = noise.Sample(1.5f, 2.5f);
        var fbmVal = noise.Fbm(1.5f, 2.5f, 4);

        // The value should be different due to multiple octaves being added
        Assert.AreNotEqual(baseVal, fbmVal);

        // Check range over a few samples
        for (float x = 0; x < 10; x += 1.0f)
        {
            for (float y = 0; y < 10; y += 1.0f)
            {
                var val = noise.Fbm(x, y, octaves: 4);
                // The max possible value for FBM with amplitude 0.5 and gain 0.5 is ~1.0 if all samples are 1.
                // It stays roughly in [0, 1] because base samples are in [0, 1] and 0.5 + 0.25 + 0.125... approaches 1.0
                Assert.IsTrue(val >= -0.1f && val <= 1.1f, $"FBM Value {val} at ({x}, {y}) is outside expected approximate range [0, 1]");
            }
        }
    }
}
