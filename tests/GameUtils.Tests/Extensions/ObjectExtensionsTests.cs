using System;
using GameUtils.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GameUtils.Tests.Extensions;

[TestClass]
public class ObjectExtensionsTests
{
    private struct DummyStruct
    {
        public int Value { get; set; }
    }

    private class DummyClass
    {
        public int Value { get; set; }
    }
  
    [TestMethod]
    public void Mutate_Struct_ReturnsMutatedValue()
    {
        int value = 5;
        int result = value.Mutate(x => x + 5);
        Assert.AreEqual(10, result);
    }

    [TestMethod]
    public void Mutate_Struct_ValidAction_ReturnsNewValue()
    {
        var initial = new DummyStruct { Value = 1 };
        var result = initial.Mutate(s => new DummyStruct { Value = s.Value + 1 });

        Assert.AreEqual(1, initial.Value);
        Assert.AreEqual(2, result.Value);
    }

    [TestMethod]
    public void Mutate_Struct_NullAction_ThrowsArgumentNullException()
    {
        int value = 5;
        Func<int, int> action = null!;
        Assert.ThrowsExactly<ArgumentNullException>(() => value.Mutate(action));
        var s = new DummyStruct();
        Assert.ThrowsExactly<ArgumentNullException>(() => s.Mutate((Func<DummyStruct, DummyStruct>)null!));
    }

    [TestMethod]
    public void Mutate_Class_ValidAction_ReturnsSameReference()
    {
        var initial = new DummyClass { Value = 1 };
        var result = initial.Mutate(c => c.Value += 1);

        Assert.AreSame(initial, result);
        Assert.AreEqual(2, initial.Value);
    }

    [TestMethod]
    public void Mutate_Class_NullAction_ThrowsArgumentNullException()
    {
        var c = new DummyClass();
        Assert.ThrowsExactly<ArgumentNullException>(() => c.Mutate((Action<DummyClass>)null!));
    }

    [TestMethod]
    public void Out_SetsOutParameterAndReturnsObject()
    {
        var obj = new DummyClass { Value = 5 };
        var result = obj.Out(out var outObj);

        Assert.AreSame(obj, result);
        Assert.AreSame(obj, outObj);
    }

    [TestMethod]
    public void Curry_0Args_ReturnsFuncResult()
    {
        Func<int, string> func = (x) => $"Value: {x}";
        var curried = func.Curry(10);

        Assert.AreEqual("Value: 10", curried());
    }

    [TestMethod]
    public void Curry_0Args_NullFunc_ThrowsArgumentNullException()
    {
        Func<int, string>? func = null;
        Assert.ThrowsExactly<ArgumentNullException>(() => func!.Curry(10));
    }

    [TestMethod]
    public void Curry_1Args_ReturnsFuncResult()
    {
        Func<int, int, string> func = (x, y) => $"Value: {x}, {y}";
        var curried = func.Curry(10);

        Assert.AreEqual("Value: 10, 20", curried(20));
    }

    [TestMethod]
    public void Curry_1Args_NullFunc_ThrowsArgumentNullException()
    {
        Func<int, int, string>? func = null;
        Assert.ThrowsExactly<ArgumentNullException>(() => func!.Curry(10));
    }

    [TestMethod]
    public void Curry_2Args_ReturnsFuncResult()
    {
        Func<int, int, int, string> func = (x, y, z) => $"Value: {x}, {y}, {z}";
        var curried = func.Curry(10);

        Assert.AreEqual("Value: 10, 20, 30", curried(20, 30));
    }

    [TestMethod]
    public void Curry_2Args_NullFunc_ThrowsArgumentNullException()
    {
        Func<int, int, int, string>? func = null;
        Assert.ThrowsExactly<ArgumentNullException>(() => func!.Curry(10));
    }

    [TestMethod]
    public void Curry_3Args_ReturnsFuncResult()
    {
        Func<int, int, int, int, string> func = (x, y, z, w) => $"Value: {x}, {y}, {z}, {w}";
        var curried = func.Curry(10);

        Assert.AreEqual("Value: 10, 20, 30, 40", curried(20, 30, 40));
    }

    [TestMethod]
    public void Curry_3Args_NullFunc_ThrowsArgumentNullException()
    {
        Func<int, int, int, int, string>? func = null;
        Assert.ThrowsExactly<ArgumentNullException>(() => func!.Curry(10));
    }

    [TestMethod]
    public void Curry_4Args_ReturnsFuncResult()
    {
        Func<int, int, int, int, int, string> func = (x, y, z, w, v) => $"Value: {x}, {y}, {z}, {w}, {v}";
        var curried = func.Curry(10);

        Assert.AreEqual("Value: 10, 20, 30, 40, 50", curried(20, 30, 40, 50));
    }

    [TestMethod]
    public void Curry_4Args_NullFunc_ThrowsArgumentNullException()
    {
        Func<int, int, int, int, int, string>? func = null;
        Assert.ThrowsExactly<ArgumentNullException>(() => func!.Curry(10));
    }
}