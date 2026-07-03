using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using GameUtils.Types;
using System.Numerics;

namespace GameUtils.Tests.Types;

[TestClass]
public class BitmapTests
{
    [TestMethod]
    public void Write_PathTraversal_ThrowsUnauthorizedAccessException()
    {
        var bmp = new Bitmap(1, 1);
        string invalidPath = Path.Combine(Environment.CurrentDirectory, "..", "traversal_test.bmp");

        Assert.ThrowsExactly<UnauthorizedAccessException>(() => bmp.Write(invalidPath));
    }

    [TestMethod]
    public void Write_ValidPath_Works()
    {
        string validPath = Path.Combine(Environment.CurrentDirectory, "valid_test.bmp");

        var bmp = new Bitmap(1, 1);
        bmp[0, 0] = new Vector3(1, 1, 1);

        // Write should not throw
        bmp.Write(validPath);

        Assert.IsTrue(File.Exists(validPath));

        // Cleanup
        if (File.Exists(validPath))
        {
            File.Delete(validPath);
        }
    }
}
