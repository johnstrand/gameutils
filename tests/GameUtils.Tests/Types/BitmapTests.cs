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
        public void Write_WithAbsolutePathOutsideCurrentDir_ThrowsUnauthorizedAccessException()
        {
            var bitmap = new Bitmap(10, 10);
            var outsidePath = Path.Combine(Path.GetTempPath(), "test_bitmap_outside.bmp");

            Assert.ThrowsExactly<UnauthorizedAccessException>(() => bitmap.Write(outsidePath));
        }

        [TestMethod]
        public void Write_WithAbsolutePathInsideCurrentDir_Succeeds()
        {
            var bitmap = new Bitmap(10, 10);
            var validAbsolutePath = Path.GetFullPath("test_bitmap_abs.bmp");
            try
            {
                bitmap.Write(validAbsolutePath);
                Assert.IsTrue(File.Exists(validAbsolutePath));
            }
            finally
            {
                if (File.Exists(validAbsolutePath))
                {
                    File.Delete(validAbsolutePath);
                }
            }
        }

        [TestMethod]
        public void Write_InSubdirectory_Succeeds()
        {
            var bitmap = new Bitmap(10, 10);
            var subDir = Path.Combine(Environment.CurrentDirectory, "test_sub_dir");
            Directory.CreateDirectory(subDir);
            var subDirPath = Path.Combine("test_sub_dir", "test_bitmap_sub.bmp");
            try
            {
                bitmap.Write(subDirPath);
                Assert.IsTrue(File.Exists(subDirPath));
            }
            finally
            {
                if (File.Exists(subDirPath))
                {
                    File.Delete(subDirPath);
                }

                if (Directory.Exists(subDir))
                {
                    Directory.Delete(subDir, true);
                }
            }
        }

        [TestMethod]
        [DataRow(-1, 10)]
        [DataRow(0, 10)]
        [DataRow(10, -1)]
        [DataRow(10, 0)]
        [DataRow(100000, 100000)]
        public void Constructor_InvalidDimensions_ThrowsArgumentOutOfRangeException(int width, int height)
        {
            Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => new Bitmap(width, height));
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
