using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using GameUtils.Types;
using System.Numerics;

namespace GameUtils.Tests.Types;

[TestClass]
public class ImageDataTests
{
    [TestMethod]
    public void Read_PathTraversal_ThrowsUnauthorizedAccessException()
    {
        // Try to access a file outside the current directory
        string invalidPath = Path.Combine(Environment.CurrentDirectory, "..", "traversal_test.dat");

        Assert.ThrowsExactly<UnauthorizedAccessException>(() => ImageData.Read(invalidPath));
    }

    [TestMethod]
    public void Write_PathTraversal_ThrowsUnauthorizedAccessException()
    {
        var img = new ImageData(1, 1);
        string invalidPath = Path.Combine(Environment.CurrentDirectory, "..", "traversal_test.dat");

        Assert.ThrowsExactly<UnauthorizedAccessException>(() => img.Write(invalidPath));
    }

    [TestMethod]
    public void ReadWrite_ValidPath_Works()
    {
        string validPath = Path.Combine(Environment.CurrentDirectory, "valid_test.dat");

        var img = new ImageData(1, 1);
        img[0, 0] = new Vector4(1, 1, 1, 1);

        // Write should not throw
        img.Write(validPath);

        // Read should not throw
        var readImg = ImageData.Read(validPath);

        Assert.AreEqual(1, readImg.Width);
        Assert.AreEqual(1, readImg.Height);
        Assert.AreEqual(new Vector4(1, 1, 1, 1), readImg[0, 0]);

        // Cleanup
        if (File.Exists(validPath))
        {
            File.Delete(validPath);
        }
    }
}
