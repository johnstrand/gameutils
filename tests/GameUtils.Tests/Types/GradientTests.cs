using System.Collections.Generic;
using GameUtils;
using GameUtils.Types;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GameUtils.Tests.Types
{
    [TestClass]
    public class GradientTests
    {
        [TestMethod]
        public void Evaluate_EmptyGradient_ReturnsTransparentBlack()
        {
            var gradient = new Gradient();
            var result = gradient.Evaluate(0.5f);
            Assert.AreEqual(new Color(0, 0, 0, 0), result);
        }

        [TestMethod]
        public void Evaluate_SingleStop_ReturnsStopColor()
        {
            var red = new Color(255, 0, 0, 255);
            var gradient = new Gradient().AddStop(0.5f, red);

            Assert.AreEqual(red, gradient.Evaluate(0.0f));
            Assert.AreEqual(red, gradient.Evaluate(0.5f));
            Assert.AreEqual(red, gradient.Evaluate(1.0f));
        }

        [TestMethod]
        public void AddStop_OutOrderStops_SortsStopsCorrectly()
        {
            var red = new Color(255, 0, 0, 255);
            var blue = new Color(0, 0, 255, 255);
            var green = new Color(0, 255, 0, 255);

            var gradient = new Gradient();
            gradient.AddStop(1.0f, blue);
            gradient.AddStop(0.0f, red);
            gradient.AddStop(0.5f, green);

            Assert.AreEqual(red, gradient.Evaluate(0.0f));
            Assert.AreEqual(green, gradient.Evaluate(0.5f));
            Assert.AreEqual(blue, gradient.Evaluate(1.0f));
        }

        [TestMethod]
        public void Constructor_WithIEnumerableStops_InitializesAndSortsStops()
        {
            var red = new Color(255, 0, 0, 255);
            var blue = new Color(0, 0, 255, 255);
            var stops = new List<(float position, Color color)>
            {
                (1.0f, blue),
                (0.0f, red)
            };

            var gradient = new Gradient(stops);

            Assert.AreEqual(red, gradient.Evaluate(0.0f));
            Assert.AreEqual(blue, gradient.Evaluate(1.0f));
        }

        [TestMethod]
        public void Evaluate_BelowMinPosition_ReturnsFirstStopColor()
        {
            var red = new Color(255, 0, 0, 255);
            var blue = new Color(0, 0, 255, 255);
            var gradient = new Gradient()
                .AddStop(0.2f, red)
                .AddStop(0.8f, blue);

            Assert.AreEqual(red, gradient.Evaluate(0.0f));
            Assert.AreEqual(red, gradient.Evaluate(0.1f));
        }

        [TestMethod]
        public void Evaluate_AboveMaxPosition_ReturnsLastStopColor()
        {
            var red = new Color(255, 0, 0, 255);
            var blue = new Color(0, 0, 255, 255);
            var gradient = new Gradient()
                .AddStop(0.2f, red)
                .AddStop(0.8f, blue);

            Assert.AreEqual(blue, gradient.Evaluate(0.9f));
            Assert.AreEqual(blue, gradient.Evaluate(1.0f));
        }

        [TestMethod]
        public void Evaluate_BetweenStops_InterpolatesColorLinearly()
        {
            var black = new Color(0, 0, 0, 0);
            var white = new Color(100, 200, 100, 200);
            var gradient = new Gradient()
                .AddStop(0.0f, black)
                .AddStop(1.0f, white);

            var mid = gradient.Evaluate(0.5f);
            Assert.AreEqual(50, mid.R);
            Assert.AreEqual(100, mid.G);
            Assert.AreEqual(50, mid.B);
            Assert.AreEqual(100, mid.A);
        }

        [TestMethod]
        public void Evaluate_MultipleStops_InterpolatesCorrectSegment()
        {
            var color0 = new Color(0, 0, 0, 255);
            var color1 = new Color(100, 100, 100, 255);
            var color2 = new Color(200, 200, 200, 255);

            var gradient = new Gradient()
                .AddStop(0.0f, color0)
                .AddStop(0.5f, color1)
                .AddStop(1.0f, color2);

            var firstHalfMid = gradient.Evaluate(0.25f);
            Assert.AreEqual(50, firstHalfMid.R);

            var secondHalfMid = gradient.Evaluate(0.75f);
            Assert.AreEqual(150, secondHalfMid.R);
        }
    }
}
