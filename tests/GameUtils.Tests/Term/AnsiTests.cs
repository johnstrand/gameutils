using GameUtils.Term;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GameUtils.Tests.Term;

[TestClass]
public class AnsiTests
{
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
}
