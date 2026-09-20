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
        public void Read_WithTraversalPath_ThrowsUnauthorizedAccessException()
        {
            var invalidPath = Path.Combine("..", "test_image.dat");

            Assert.ThrowsExactly<UnauthorizedAccessException>(() => ImageData.Read(invalidPath));
        }

        [TestMethod]
        public void Write_WithAbsolutePathOutsideDirectory_ThrowsUnauthorizedAccessException()
        {
            var imageData = new ImageData(10, 10);
            var outsidePath = Path.Combine(Path.GetTempPath(), "outside_image.dat");

            Assert.ThrowsExactly<UnauthorizedAccessException>(() => imageData.Write(outsidePath));
        }

        [TestMethod]
        public void Read_WithAbsolutePathOutsideDirectory_ThrowsUnauthorizedAccessException()
        {
            var outsidePath = Path.Combine(Path.GetTempPath(), "outside_image.dat");

            Assert.ThrowsExactly<UnauthorizedAccessException>(() => ImageData.Read(outsidePath));
        }

        [TestMethod]
        public void WriteAndRead_WithCustomBaseDirectory_SucceedsWithinBaseDirectory()
        {
            var tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(tempDir);
            var imageFile = Path.Combine(tempDir, "subfolder", "image.dat");
            Directory.CreateDirectory(Path.GetDirectoryName(imageFile)!);

            try
            {
                var imageData = new ImageData(2, 2);
                imageData.Write(imageFile, tempDir);

                Assert.IsTrue(File.Exists(imageFile));

                var loaded = ImageData.Read(imageFile, tempDir);
                Assert.AreEqual(2, loaded.Width);
                Assert.AreEqual(2, loaded.Height);
            }
            finally
            {
                if (Directory.Exists(tempDir))
                {
                    Directory.Delete(tempDir, true);
                }
            }
        }

        [TestMethod]
        public void Write_WithCustomBaseDirectory_TraversalThrowsUnauthorizedAccessException()
        {
            var tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(tempDir);

            try
            {
                var imageData = new ImageData(2, 2);
                var escapePath = Path.Combine(tempDir, "..", "escaped.dat");

                Assert.ThrowsExactly<UnauthorizedAccessException>(() => imageData.Write(escapePath, tempDir));
            }
            finally
            {
                if (Directory.Exists(tempDir))
                {
                    Directory.Delete(tempDir, true);
                }
            }
        }

        [TestMethod]
        public void WriteAndRead_WithDotPrefixedFilename_SucceedsWithinDirectory()
        {
            var imageData = new ImageData(2, 2);
            var validPath = "..valid_image_name.dat";

            try
            {
                imageData.Write(validPath);
                Assert.IsTrue(File.Exists(validPath));

                var loaded = ImageData.Read(validPath);
                Assert.AreEqual(2, loaded.Width);
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
