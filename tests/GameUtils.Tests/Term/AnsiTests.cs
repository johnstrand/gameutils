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
        var result = Ansi.Write(text);
        Assert.AreEqual(text, result);
    }

    [TestMethod]
    public void WriteLine_WithNoArgsAndFormatTokens_DoesNotThrow()
    {
        var text = "Another {test} string {0}";
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

    [TestMethod]
    public void Write_WithFormatStringButNoArgs_DoesNotThrow()
    {
        string untrustedInput = "Hello {0} World";
        string result = Ansi.Write(untrustedInput);
        Assert.AreEqual("Hello {0} World", result);
    }

    [TestMethod]
    public void WriteLine_WithFormatStringButNoArgs_DoesNotThrow()
    {
        string untrustedInput = "Hello {0} World";
        string result = Ansi.WriteLine(untrustedInput);
        Assert.AreEqual("Hello {0} World", result);
    }

    [TestMethod]
    [DataRow("Hello [world")]
    [DataRow("Text [fg:notacolor]")]
    [DataRow("Text [bg:notacolor]")]
    [DataRow("Text [#fg:255,0]")]
    [DataRow("Text [#bg:invalid]")]
    [DataRow("Text [#255,255,abc]")]
    [DataRow("Text [unknown_tag]")]
    public void Format_InvalidAnsiSequence_ThrowsArgumentException(string invalidInput)
    {
        Assert.ThrowsExactly<ArgumentException>(() => Ansi.Format(invalidInput));
    }

    [TestMethod]
    [DataRow("Hello [world")]
    [DataRow("Text [fg:notacolor]")]
    [DataRow("Text [#fg:255,0]")]
    public void Write_InvalidAnsiSequence_ThrowsArgumentException(string invalidInput)
    {
        Assert.ThrowsExactly<ArgumentException>(() => Ansi.Write(invalidInput));
    }

    [TestMethod]
    [DataRow("Hello [world")]
    [DataRow("Text [bg:notacolor]")]
    [DataRow("Text [#bg:invalid]")]
    public void WriteLine_InvalidAnsiSequence_ThrowsArgumentException(string invalidInput)
    {
        Assert.ThrowsExactly<ArgumentException>(() => Ansi.WriteLine(invalidInput));
    }
}
