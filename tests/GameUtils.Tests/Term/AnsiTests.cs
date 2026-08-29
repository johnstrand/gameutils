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

    [TestMethod]
    public void Format_EscapedBrackets_RendersBracket()
    {
        var result = Ansi.Format("[[Hello]");
        Assert.AreEqual("[Hello]", result);
    }

    [TestMethod]
    public void Format_ResetSequences_RendersResetSequence()
    {
        var res1 = Ansi.Format("Text[]End");
        var res2 = Ansi.Format("Text[/]End");
        var expected = $"Text{Ansi.Reset}End";

        Assert.AreEqual(expected, res1);
        Assert.AreEqual(expected, res2);
    }

    [TestMethod]
    public void Format_TextStyles_RendersStyleSequences()
    {
        Assert.AreEqual($"Bold: {Ansi.Bold}", Ansi.Format("Bold: [bold]"));
        Assert.AreEqual($"Bold: {Ansi.Bold}", Ansi.Format("Bold: [b]"));
        Assert.AreEqual($"Faint: {Ansi.Faint}", Ansi.Format("Faint: [faint]"));
        Assert.AreEqual($"Faint: {Ansi.Faint}", Ansi.Format("Faint: [f]"));
        Assert.AreEqual($"Italic: {Ansi.Italic}", Ansi.Format("Italic: [italic]"));
        Assert.AreEqual($"Italic: {Ansi.Italic}", Ansi.Format("Italic: [i]"));
        Assert.AreEqual($"Underline: {Ansi.Underline}", Ansi.Format("Underline: [underline]"));
        Assert.AreEqual($"Underline: {Ansi.Underline}", Ansi.Format("Underline: [u]"));
    }

    [TestMethod]
    public void Format_NamedColors_RendersColorSequences()
    {
        Assert.AreEqual($"Red: {Ansi.Foreground("red")}", Ansi.Format("Red: [red]"));
        Assert.AreEqual($"Red: {Ansi.Foreground("red")}", Ansi.Format("Red: [fg:red]"));
        Assert.AreEqual($"BgRed: {Ansi.Background("red")}", Ansi.Format("BgRed: [bg:red]"));
    }

    [TestMethod]
    public void Format_RgbColors_RendersRgbColorSequences()
    {
        Assert.AreEqual($"RGB Fg: {Ansi.Foreground(255, 100, 50)}", Ansi.Format("RGB Fg: [#255,100,50]"));
        Assert.AreEqual($"RGB Fg: {Ansi.Foreground(255, 100, 50)}", Ansi.Format("RGB Fg: [#fg:255,100,50]"));
        Assert.AreEqual($"RGB Bg: {Ansi.Background(10, 20, 30)}", Ansi.Format("RGB Bg: [#bg:10,20,30]"));
    }

    [TestMethod]
    public void Format_UnterminatedSequence_ThrowsArgumentException()
    {
        var ex = Assert.ThrowsExactly<ArgumentException>(() => Ansi.Format("Hello [red"));
        Assert.IsTrue(ex.Message.Contains("Invalid ANSI sequence starting at position 6"));
    }

    [TestMethod]
    public void Format_InvalidRgbSequence_ThrowsArgumentException()
    {
        var ex = Assert.ThrowsExactly<ArgumentException>(() => Ansi.Format("Hello [#fg:255,abc,0]"));
        Assert.IsTrue(ex.Message.Contains("Invalid RGB color sequence starting at position 21"));
    }

    [TestMethod]
    public void Format_UnknownSequence_ThrowsArgumentException()
    {
        var ex = Assert.ThrowsExactly<ArgumentException>(() => Ansi.Format("Hello [invalid_tag]"));
        Assert.IsTrue(ex.Message.Contains("Unknown ANSI sequence 'invalid_tag' starting at position 19"));
    }
}
