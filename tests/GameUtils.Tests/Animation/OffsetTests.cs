using GameUtils.Animation;

namespace GameUtils.Tests.Animation;

[TestClass]
public class OffsetTests
{
    private const float Epsilon = 0.0001f;

    [TestMethod]
    [DataRow(0f)]
    [DataRow(1f)]
    public void AllFunctions_ShouldReturnZero_AtEnds(float x)
    {
        Assert.AreEqual(0f, Offset.Jagged(x), Epsilon);
        Assert.AreEqual(0f, Offset.Sine(x), Epsilon);
        Assert.AreEqual(0f, Offset.Pulse(x), Epsilon);
        Assert.AreEqual(0f, Offset.Triangle(x), Epsilon);
        Assert.AreEqual(0f, Offset.Wobble(x), Epsilon);
    }

    [TestMethod]
    [DataRow(0.1f)]
    [DataRow(0.25f)]
    [DataRow(0.5f)]
    [DataRow(0.75f)]
    [DataRow(0.9f)]
    public void AllFunctions_ShouldReturnValuesBetweenMinusOneAndOne_ForIntermediateValues(float x)
    {
        float jagged = Offset.Jagged(x);
        Assert.IsTrue(jagged is >= -1f and <= 1f, $"Jagged({x}) returned {jagged}");

        float sine = Offset.Sine(x);
        Assert.IsTrue(sine is >= -1f and <= 1f, $"Sine({x}) returned {sine}");

        float pulse = Offset.Pulse(x);
        Assert.IsTrue(pulse is >= -1f and <= 1f, $"Pulse({x}) returned {pulse}");

        float triangle = Offset.Triangle(x);
        Assert.IsTrue(triangle is >= -1f and <= 1f, $"Triangle({x}) returned {triangle}");

        float wobble = Offset.Wobble(x);
        Assert.IsTrue(wobble is >= -1f and <= 1f, $"Wobble({x}) returned {wobble}");
    }

    [TestMethod]
    [DataRow(-1f)]
    [DataRow(-0.5f)]
    [DataRow(1.5f)]
    [DataRow(2f)]
    public void AllFunctions_ShouldHandleOutOfBoundsValues(float x)
    {
        // Out of bounds test just checks that it doesn't throw and returns some float.
        // It might not be bounded to [-1, 1] depending on the function.
        // To avoid MSTEST0032, we assert that these floats are equal to themselves
        // or just rely on the fact that an exception wasn't thrown.
        var j = Offset.Jagged(x);
        var s = Offset.Sine(x);
        var p = Offset.Pulse(x);
        var t = Offset.Triangle(x);
        var w = Offset.Wobble(x);

        // Assert that they return valid numbers (not NaN)
        Assert.IsFalse(float.IsNaN(j));
        Assert.IsFalse(float.IsNaN(s));
        Assert.IsFalse(float.IsNaN(p));
        Assert.IsFalse(float.IsNaN(t));
        Assert.IsFalse(float.IsNaN(w));
    }

    [TestMethod]
    [DataRow(0f, 0f)]
    [DataRow(0.1f, -0.1f)]
    [DataRow(0.25f, -0.25f)]
    [DataRow(0.5f, 0f)]
    [DataRow(0.75f, 0.25f)]
    [DataRow(0.9f, 0.1f)]
    [DataRow(1f, 0f)]
    public void Jagged_ValidInput_ReturnsExpectedValues(float x, float expected)
    {
        Assert.AreEqual(expected, Offset.Jagged(x), Epsilon);
    }

    [TestMethod]
    public void Sine_ValidInput_ReturnsExpectedWaveValues()
    {
        Assert.AreEqual(0.7071f, Offset.Sine(0.125f), Epsilon);
        Assert.AreEqual(1f, Offset.Sine(0.25f), Epsilon);
        Assert.AreEqual(0.7071f, Offset.Sine(0.375f), Epsilon);
        Assert.AreEqual(0f, Offset.Sine(0.5f), Epsilon);
        Assert.AreEqual(-0.7071f, Offset.Sine(0.625f), Epsilon);
        Assert.AreEqual(-1f, Offset.Sine(0.75f), Epsilon);
        Assert.AreEqual(-0.7071f, Offset.Sine(0.875f), Epsilon);
    }

    [TestMethod]
    public void Pulse_KnownValues()
    {
        // For x = 0.25:
        // t = MathF.Sin(0.25 * Tau * 3) = MathF.Sin(1.5 * Pi) = -1
        // u = (1 - MathF.Cos(0.25 * Tau)) / 2 = (1 - 0) / 2 = 0.5
        // return t * u * u = -1 * 0.25 = -0.25
        Assert.AreEqual(-0.25f, Offset.Pulse(0.25f), Epsilon);

        // For x = 0.5:
        // t = MathF.Sin(0.5 * Tau * 3) = MathF.Sin(3 * Pi) = 0
        // return 0
        Assert.AreEqual(0f, Offset.Pulse(0.5f), Epsilon);

        // For x = 0.75:
        // t = MathF.Sin(0.75 * Tau * 3) = MathF.Sin(4.5 * Pi) = 1
        // u = (1 - MathF.Cos(0.75 * Tau)) / 2 = (1 - 0) / 2 = 0.5
        // return t * u * u = 1 * 0.25 = 0.25
        Assert.AreEqual(0.25f, Offset.Pulse(0.75f), Epsilon);
    }

    [TestMethod]
    public void Triangle_KnownValues()
    {
        // For x = 0.25:
        // ((0.25 + 0.25) * 4 % 4) = 2 % 4 = 2
        // Abs(2 - 2) - 1 = -1
        Assert.AreEqual(-1f, Offset.Triangle(0.25f), Epsilon);

        // For x = 0.5:
        // ((0.5 + 0.25) * 4 % 4) = 3 % 4 = 3
        // Abs(3 - 2) - 1 = 0
        Assert.AreEqual(0f, Offset.Triangle(0.5f), Epsilon);

        // For x = 0.75:
        // ((0.75 + 0.25) * 4 % 4) = 4 % 4 = 0
        // Abs(0 - 2) - 1 = 1
        Assert.AreEqual(1f, Offset.Triangle(0.75f), Epsilon);
    }
}
