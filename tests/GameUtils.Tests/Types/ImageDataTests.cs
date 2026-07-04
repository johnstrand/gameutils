using System;
using System.IO;
using GameUtils.Types;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GameUtils.Tests.Types
{
    [TestClass]
    public class ImageDataTests
    {
        [TestMethod]
        public void Write_WithTraversalPath_ThrowsUnauthorizedAccessException()
        {
            var imageData = new ImageData(10, 10);
            var invalidPath = Path.Combine("..", "test_image.dat");

            Assert.ThrowsExactly<UnauthorizedAccessException>(() => imageData.Write(invalidPath));
        }

        [TestMethod]
        public void Write_WithValidPath_DoesNotThrow()
        {
            var imageData = new ImageData(10, 10);
            var validPath = "test_image_valid.dat";
            try
            {
                imageData.Write(validPath);
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
