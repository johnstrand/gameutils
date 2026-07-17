using System;
using GameUtils.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GameUtils.Tests.Extensions;

[TestClass]
public class ObjectExtensionsTests
{
    [TestMethod]
    public void Mutate_Struct_ReturnsMutatedValue()
    {
        int value = 5;
        int result = value.Mutate(x => x + 5);
        Assert.AreEqual(10, result);
    }

    [TestMethod]
    public void Mutate_Struct_NullAction_ThrowsArgumentNullException()
    {
        int value = 5;
        Func<int, int> action = null!;
        Assert.ThrowsExactly<ArgumentNullException>(() => value.Mutate(action));
    }
}
