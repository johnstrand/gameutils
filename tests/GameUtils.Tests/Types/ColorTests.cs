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
    }
}
