using System;
using GameUtils.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GameUtils.Tests.Extensions;

[TestClass]
public class StringExtensionsTests
{
    [TestMethod]
    [DataRow("hello", 0, true, 'h')]
    [DataRow("hello", 4, true, 'o')]
    [DataRow("hello", -1, false, '\0')]
    [DataRow("hello", 5, false, '\0')]
    [DataRow("hello", int.MaxValue, false, '\0')]
    [DataRow("hello", int.MinValue, false, '\0')]
    [DataRow("", 0, false, '\0')]
    public void TryGet_ReturnsExpectedResult(string str, int index, bool expectedReturn, char expectedChar)
    {
        var result = str.TryGet(index, out char c);
        Assert.AreEqual(expectedReturn, result);
        Assert.AreEqual(expectedChar, c);
    }

    [TestMethod]
    public void TryGet_NullString_ThrowsArgumentNullException()
    {
        string str = null!;
        Assert.ThrowsExactly<ArgumentNullException>(() => str.TryGet(0, out _));
    }

    [TestMethod]
    [DataRow("hello", 3, "hellohellohello")]
    [DataRow("a", 5, "aaaaa")]
    [DataRow("abc", 1, "abc")]
    [DataRow("test", 0, "")]
    [DataRow("", 5, "")]
    [DataRow("", 0, "")]
    public void Repeat_ReturnsExpectedResult(string str, int count, string expectedResult)
    {
        var result = str.Repeat(count);
        Assert.AreEqual(expectedResult, result);
    }

    [TestMethod]
    public void Repeat_NullString_ThrowsArgumentNullException()
    {
        string str = null!;
        Assert.ThrowsExactly<ArgumentNullException>(() => str.Repeat(5));
    }

    [TestMethod]
    public void Repeat_NegativeCount_ThrowsArgumentOutOfRangeException()
    {
        string str = "test";
        Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => str.Repeat(-1));
    }

    [TestMethod]
    public void Repeat_CountZero_ReturnsStringEmpty()
    {
        var result = "test".Repeat(0);
        Assert.AreSame(string.Empty, result);
    }

    [TestMethod]
    public void Repeat_EmptyString_ReturnsStringEmpty()
    {
        var result = string.Empty.Repeat(5);
        Assert.AreSame(string.Empty, result);
    }

    [TestMethod]
    public void Repeat_CountOne_ReturnsOriginalString()
    {
        string str = "test";
        var result = str.Repeat(1);
        Assert.AreSame(str, result);
    }
}
