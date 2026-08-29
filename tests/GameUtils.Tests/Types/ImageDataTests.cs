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

        [TestMethod]
        public void WriteAndRead_StreamRoundtrip_PreservesData()
        {
            int width = 4;
            int height = 3;
            var data = new System.Numerics.Vector4[width * height];
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = new System.Numerics.Vector4(i * 1.0f, i * 2.0f, i * 3.0f, i * 4.0f);
            }

            var original = new ImageData(width, height, data);

            using var ms = new MemoryStream();
            original.Write(ms);

            ms.Position = 0;
            var loaded = ImageData.Read(ms);

            Assert.AreEqual(original.Width, loaded.Width);
            Assert.AreEqual(original.Height, loaded.Height);
            Assert.AreEqual(original.Data.Length, loaded.Data.Length);

            for (int i = 0; i < original.Data.Length; i++)
            {
                Assert.AreEqual(original.Data[i], loaded.Data[i]);
            }
        }
    }
}
