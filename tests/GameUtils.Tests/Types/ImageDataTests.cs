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
        public void Read_WithNegativeDimensions_ThrowsInvalidDataException()
        {
            using var ms = new MemoryStream();
            using (var compressor = new System.IO.Compression.GZipStream(ms, System.IO.Compression.CompressionLevel.Optimal, true))
            using (var writer = new BinaryWriter(compressor))
            {
                writer.Write("IMGD"u8);
                writer.Write(-1);
                writer.Write(10);
            }

            ms.Position = 0;

            Assert.ThrowsExactly<InvalidDataException>(() => ImageData.Read(ms));
        }

        [TestMethod]
        public void Read_WithExcessiveDimensions_ThrowsInvalidDataException()
        {
            using var ms = new MemoryStream();
            using (var compressor = new System.IO.Compression.GZipStream(ms, System.IO.Compression.CompressionLevel.Optimal, true))
            using (var writer = new BinaryWriter(compressor))
            {
                writer.Write("IMGD"u8);
                writer.Write(20000);
                writer.Write(20000); // 400M pixels > 67.1M limit
            }

            ms.Position = 0;

            Assert.ThrowsExactly<InvalidDataException>(() => ImageData.Read(ms));
        }

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
