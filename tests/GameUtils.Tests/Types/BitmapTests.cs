using System;
using System.IO;
using GameUtils.Types;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GameUtils.Tests.Types
{
    [TestClass]
    public class BitmapTests
    {
        [TestMethod]
        public void Write_WithTraversalPath_ThrowsUnauthorizedAccessException()
        {
            var bitmap = new Bitmap(10, 10);
            var invalidPath = Path.Combine("..", "test_bitmap.bmp");

            Assert.ThrowsExactly<UnauthorizedAccessException>(() => bitmap.Write(invalidPath));
        }

        [TestMethod]
        public void Write_WithValidPath_DoesNotThrow()
        {
            var bitmap = new Bitmap(10, 10);
            var validPath = "test_bitmap_valid.bmp";
            try
            {
                bitmap.Write(validPath);
                Assert.IsTrue(File.Exists(validPath));
            }
            finally
            {
                if (File.Exists(validPath))
                {
                    File.Delete(validPath);
                }
            }
        }
    }
}
