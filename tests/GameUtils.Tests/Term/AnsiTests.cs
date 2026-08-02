using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GameUtils.Term;

namespace GameUtils.Tests.Term;

[TestClass]
public class AnsiTests
{
    [TestMethod]
    public void Write_WithNoArgsAndFormatTokens_DoesNotThrow()
    {
        var text = "Test string with {0} and {1}";
        // Should not throw System.FormatException
        var result = Ansi.Write(text);
        Assert.AreEqual(text, result);
    }

    [TestMethod]
    public void WriteLine_WithNoArgsAndFormatTokens_DoesNotThrow()
    {
        var text = "Another {test} string {0}";
        // Should not throw System.FormatException
        var result = Ansi.WriteLine(text);
        Assert.AreEqual(text, result);
    }

    [TestMethod]
    public void Write_WithArgs_FormatsCorrectly()
    {
        var text = "Hello {0}";
        var result = Ansi.Write(text, "World");
        Assert.AreEqual("Hello World", result);
    }
}
