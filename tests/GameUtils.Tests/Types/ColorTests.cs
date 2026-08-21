using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GameUtils;

namespace GameUtils.Tests.Types
{
    [TestClass]
    public class ColorTests
    {
        [TestMethod]
        public void FromRgba_ParsesWhite_Correctly()
        {
            int colorInt = unchecked((int)0xFFFFFFFF);
            Color color = Color.FromRgba(colorInt);

            Assert.AreEqual(255, color.R);
            Assert.AreEqual(255, color.G);
            Assert.AreEqual(255, color.B);
            Assert.AreEqual(255, color.A);
        }

        [TestMethod]
        public void FromRgba_ParsesBlack_Correctly()
        {
            int colorInt = 0x00000000;
            Color color = Color.FromRgba(colorInt);

            Assert.AreEqual(0, color.R);
            Assert.AreEqual(0, color.G);
            Assert.AreEqual(0, color.B);
            Assert.AreEqual(0, color.A);
        }

        [TestMethod]
        public void FromRgba_ParsesMixedColor_Correctly()
        {
            int colorInt = 0x12345678;
            Color color = Color.FromRgba(colorInt);

            Assert.AreEqual(0x12, color.R);
            Assert.AreEqual(0x34, color.G);
            Assert.AreEqual(0x56, color.B);
            Assert.AreEqual(0x78, color.A);
        }

        [TestMethod]
        public void FromRgba_ParsesNegativeInteger_Correctly()
        {
            int colorInt = -2147483648; // int.MinValue, 0x80000000
            Color color = Color.FromRgba(colorInt);

            Assert.AreEqual(128, color.R);
            Assert.AreEqual(0, color.G);
            Assert.AreEqual(0, color.B);
            Assert.AreEqual(0, color.A);
        }

        [TestMethod]
        public void Lerp_WithTZero_ReturnsFirstColor()
        {
            var a = new Color(10, 20, 30, 40);
            var b = new Color(100, 200, 150, 250);
            var result = Color.Lerp(a, b, 0f);

            Assert.AreEqual(10, result.R);
            Assert.AreEqual(20, result.G);
            Assert.AreEqual(30, result.B);
            Assert.AreEqual(40, result.A);
        }

        [TestMethod]
        public void Lerp_WithTOne_ReturnsSecondColor()
        {
            var a = new Color(10, 20, 30, 40);
            var b = new Color(100, 200, 150, 250);
            var result = Color.Lerp(a, b, 1f);

            Assert.AreEqual(100, result.R);
            Assert.AreEqual(200, result.G);
            Assert.AreEqual(150, result.B);
            Assert.AreEqual(250, result.A);
        }

        [TestMethod]
        public void Lerp_WithTHalf_ReturnsMidpoint()
        {
            var a = new Color(10, 20, 30, 40);
            var b = new Color(100, 200, 150, 250);
            var result = Color.Lerp(a, b, 0.5f);

            Assert.AreEqual(55, result.R);
            Assert.AreEqual(110, result.G);
            Assert.AreEqual(90, result.B);
            Assert.AreEqual(145, result.A);
        }

        [TestMethod]
        public void Lerp_WithTBelowZero_ClampsToZero()
        {
            var a = new Color(10, 20, 30, 40);
            var b = new Color(100, 200, 150, 250);
            var result = Color.Lerp(a, b, -0.5f);

            Assert.AreEqual(10, result.R);
            Assert.AreEqual(20, result.G);
            Assert.AreEqual(30, result.B);
            Assert.AreEqual(40, result.A);
        }

        [TestMethod]
        public void Lerp_WithTAboveOne_ClampsToOne()
        {
            var a = new Color(10, 20, 30, 40);
            var b = new Color(100, 200, 150, 250);
            var result = Color.Lerp(a, b, 1.5f);

            Assert.AreEqual(100, result.R);
            Assert.AreEqual(200, result.G);
            Assert.AreEqual(150, result.B);
            Assert.AreEqual(250, result.A);
        }
    }
}
