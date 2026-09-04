using GameUtils;
using GameUtils.Types;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GameUtils.Tests.Types;

[TestClass]
public class GradientTests
{
    private static readonly Color Red = new(255, 0, 0, 255);
    private static readonly Color Blue = new(0, 0, 255, 255);

    [TestMethod]
    public void Evaluate_EmptyGradient_ReturnsTransparentBlack()
    {
        var gradient = new Gradient();
        var color = gradient.Evaluate(0.5f);
        Assert.AreEqual(new Color(0, 0, 0, 0), color);
    }

    [TestMethod]
    public void Evaluate_SingleStop_ReturnsStopColor()
    {
        var gradient = new Gradient();
        gradient.AddStop(0.5f, Red);

        Assert.AreEqual(Red, gradient.Evaluate(0.0f));
        Assert.AreEqual(Red, gradient.Evaluate(0.5f));
        Assert.AreEqual(Red, gradient.Evaluate(1.0f));
    }

    [TestMethod]
    public void Evaluate_TwoStops_InterpolatesCorrectly()
    {
        var gradient = new Gradient();
        gradient.AddStop(0.0f, new Color(0, 0, 0, 255));
        gradient.AddStop(1.0f, new Color(100, 200, 50, 255));

        var start = gradient.Evaluate(0.0f);
        var mid = gradient.Evaluate(0.5f);
        var end = gradient.Evaluate(1.0f);

        Assert.AreEqual(new Color(0, 0, 0, 255), start);
        Assert.AreEqual(new Color(50, 100, 25, 255), mid);
        Assert.AreEqual(new Color(100, 200, 50, 255), end);
    }

    [TestMethod]
    public void Evaluate_OutOfBounds_ClampsToNearestStop()
    {
        var gradient = new Gradient();
        gradient.AddStop(0.2f, Red);
        gradient.AddStop(0.8f, Blue);

        Assert.AreEqual(Red, gradient.Evaluate(0.0f));
        Assert.AreEqual(Red, gradient.Evaluate(0.1f));
        Assert.AreEqual(Blue, gradient.Evaluate(0.9f));
        Assert.AreEqual(Blue, gradient.Evaluate(1.0f));
    }

    [TestMethod]
    public void AddStop_UnsortedPositions_SortsStopsCorrectly()
    {
        var gradient = new Gradient();
        gradient.AddStop(1.0f, Blue);
        gradient.AddStop(0.0f, Red);

        Assert.AreEqual(Red, gradient.Evaluate(0.0f));
        Assert.AreEqual(Blue, gradient.Evaluate(1.0f));
    }

    [TestMethod]
    public void Constructor_WithStopsCollection_InitializesAndSorts()
    {
        var stops = new (float, Color)[]
        {
            (1.0f, Blue),
            (0.0f, Red)
        };
        var gradient = new Gradient(stops);

        Assert.AreEqual(Red, gradient.Evaluate(0.0f));
        Assert.AreEqual(Blue, gradient.Evaluate(1.0f));
    }

    [TestMethod]
    public void AddStop_ReturnsGradientInstanceForChaining()
    {
        var gradient = new Gradient();
        var result = gradient.AddStop(0.0f, Red);

        Assert.AreSame(gradient, result);
    }
}
