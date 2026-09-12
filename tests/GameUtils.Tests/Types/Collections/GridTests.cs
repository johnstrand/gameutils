using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GameUtils.Types.Collections;

namespace GameUtils.Tests.Types.Collections;

[TestClass]
public class GridTests
{
    [TestMethod]
    public void Constructor_ValidDimensions_InitializesGrid()
    {
        var grid = new Grid<int>(3, 4);
        Assert.AreEqual(3, grid.Width);
        Assert.AreEqual(4, grid.Height);
        Assert.AreEqual(0, grid[0, 0]);
        Assert.AreEqual(0, grid[2, 3]);
    }

    [TestMethod]
    [DataRow(0, 5)]
    [DataRow(-1, 5)]
    [DataRow(5, 0)]
    [DataRow(5, -1)]
    public void Constructor_InvalidDimensions_ThrowsArgumentOutOfRangeException(int width, int height)
    {
        Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => new Grid<int>(width, height));
    }

    [TestMethod]
    public void Constructor_WithArrayData_InitializesGrid()
    {
        int[] data = { 1, 2, 3, 4, 5, 6 };
        var grid = new Grid<int>(3, 2, data);
        Assert.AreEqual(3, grid.Width);
        Assert.AreEqual(2, grid.Height);
        Assert.AreEqual(1, grid[0, 0]);
        Assert.AreEqual(3, grid[2, 0]);
        Assert.AreEqual(4, grid[0, 1]);
        Assert.AreEqual(6, grid[2, 1]);
    }

    [TestMethod]
    public void Constructor_WithArrayData_InvalidLength_ThrowsArgumentException()
    {
        int[] data = { 1, 2, 3 };
        Assert.ThrowsExactly<ArgumentException>(() => new Grid<int>(3, 2, data));
    }

    [TestMethod]
    [DataRow(0, 5)]
    [DataRow(-1, 5)]
    [DataRow(5, 0)]
    [DataRow(5, -1)]
    public void Constructor_WithArrayData_InvalidDimensions_ThrowsArgumentOutOfRangeException(int width, int height)
    {
        int[] data = new int[System.Math.Max(0, width * height)];
        Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => new Grid<int>(width, height, data));
    }

    [TestMethod]
    public void Indexer_Ints_GetAndSet()
    {
        var grid = new Grid<int>(2, 2);
        grid[1, 0] = 42;
        grid[0, 1] = 99;
        Assert.AreEqual(42, grid[1, 0]);
        Assert.AreEqual(99, grid[0, 1]);
    }

    [TestMethod]
    public void Indexer_Ints_OutOfBounds_ThrowsIndexOutOfRangeException()
    {
        var grid = new Grid<int>(2, 2);
        Assert.ThrowsExactly<IndexOutOfRangeException>(() => _ = grid[-1, 0]);
        Assert.ThrowsExactly<IndexOutOfRangeException>(() => _ = grid[0, -1]);
        Assert.ThrowsExactly<IndexOutOfRangeException>(() => _ = grid[0, 2]);
        Assert.ThrowsExactly<IndexOutOfRangeException>(() => _ = grid[2, 2]);

        Assert.ThrowsExactly<IndexOutOfRangeException>(() => grid[-1, 0] = 1);
        Assert.ThrowsExactly<IndexOutOfRangeException>(() => grid[0, -1] = 1);
        Assert.ThrowsExactly<IndexOutOfRangeException>(() => grid[0, 2] = 1);
        Assert.ThrowsExactly<IndexOutOfRangeException>(() => grid[2, 2] = 1);
    }

    [TestMethod]
    public void Indexer_Vector2_GetAndSet()
    {
        var grid = new Grid<string>(2, 2);
        grid[new Vector2(1, 0)] = "hello";
        Assert.AreEqual("hello", grid[new Vector2(1, 0)]);
    }

    [TestMethod]
    public void Indexer_Vector2_OutOfBounds_ThrowsIndexOutOfRangeException()
    {
        var grid = new Grid<int>(2, 2);
        Assert.ThrowsExactly<IndexOutOfRangeException>(() => _ = grid[new Vector2(-1, 0)]);
        Assert.ThrowsExactly<IndexOutOfRangeException>(() => _ = grid[new Vector2(0, 2)]);
        Assert.ThrowsExactly<IndexOutOfRangeException>(() => _ = grid[new Vector2(2, 2)]);

        Assert.ThrowsExactly<IndexOutOfRangeException>(() => grid[new Vector2(-1, 0)] = 1);
        Assert.ThrowsExactly<IndexOutOfRangeException>(() => grid[new Vector2(0, 2)] = 1);
        Assert.ThrowsExactly<IndexOutOfRangeException>(() => grid[new Vector2(2, 2)] = 1);
    }

    [TestMethod]
    public void TryGet_Ints_ReturnsExpected()
    {
        var grid = new Grid<int>(2, 2);
        grid[1, 1] = 77;
        Assert.IsTrue(grid.TryGet(1, 1, out int value));
        Assert.AreEqual(77, value);
        Assert.IsFalse(grid.TryGet(2, 1, out int outVal));
        Assert.AreEqual(0, outVal);
        Assert.IsFalse(grid.TryGet(-1, 0, out outVal));
        Assert.AreEqual(0, outVal);
    }

    [TestMethod]
    public void TryGet_Vector2_ReturnsExpected()
    {
        var grid = new Grid<int>(2, 2);
        grid[0, 1] = 88;
        Assert.IsTrue(grid.TryGet(new Vector2(0, 1), out int value));
        Assert.AreEqual(88, value);
        Assert.IsFalse(grid.TryGet(new Vector2(2, 1), out int outVal));
        Assert.AreEqual(0, outVal);
        Assert.IsFalse(grid.TryGet(new Vector2(-1, 0), out outVal));
        Assert.AreEqual(0, outVal);
    }

    [TestMethod]
    public void TrySet_Ints_ReturnsExpected()
    {
        var grid = new Grid<int>(2, 2);
        Assert.IsTrue(grid.TrySet(1, 0, 50));
        Assert.AreEqual(50, grid[1, 0]);
        Assert.IsFalse(grid.TrySet(2, 0, 100));
        Assert.IsFalse(grid.TrySet(-1, 0, 100));
    }

    [TestMethod]
    public void TrySet_Vector2_ReturnsExpected()
    {
        var grid = new Grid<int>(2, 2);
        Assert.IsTrue(grid.TrySet(new Vector2(1, 0), 50));
        Assert.AreEqual(50, grid[1, 0]);
        Assert.IsFalse(grid.TrySet(new Vector2(2, 0), 100));
        Assert.IsFalse(grid.TrySet(new Vector2(-1, 0), 100));
    }

    [TestMethod]
    public void IsInBounds_IntsAndVector2_ReturnsExpected()
    {
        var grid = new Grid<int>(3, 3);
        Assert.IsTrue(grid.IsInBounds(0, 0));
        Assert.IsTrue(grid.IsInBounds(2, 2));
        Assert.IsFalse(grid.IsInBounds(-1, 0));
        Assert.IsFalse(grid.IsInBounds(3, 0));
        Assert.IsFalse(grid.IsInBounds(0, -1));
        Assert.IsFalse(grid.IsInBounds(0, 3));
        Assert.IsTrue(grid.IsInBounds(new Vector2(0, 0)));
        Assert.IsTrue(grid.IsInBounds(new Vector2(2, 2)));
        Assert.IsFalse(grid.IsInBounds(new Vector2(-1, 0)));
        Assert.IsFalse(grid.IsInBounds(new Vector2(3, 0)));
        Assert.IsFalse(grid.IsInBounds(new Vector2(0, -1)));
        Assert.IsFalse(grid.IsInBounds(new Vector2(0, 3)));
    }

    [TestMethod]
    public void Clear_ResetsAllElementsToDefault()
    {
        var grid = new Grid<int>(2, 2);
        grid.Fill(10);
        grid.Clear();
        Assert.AreEqual(0, grid[0, 0]);
        Assert.AreEqual(0, grid[1, 0]);
        Assert.AreEqual(0, grid[0, 1]);
        Assert.AreEqual(0, grid[1, 1]);
    }

    [TestMethod]
    public void Fill_WithValue_SetsAllElements()
    {
        var grid = new Grid<int>(2, 2);
        var result = grid.Fill(5);
        Assert.AreSame(grid, result);
        Assert.AreEqual(5, grid[0, 0]);
        Assert.AreEqual(5, grid[1, 0]);
        Assert.AreEqual(5, grid[0, 1]);
        Assert.AreEqual(5, grid[1, 1]);
    }

    [TestMethod]
    public void Fill_WithXYFactory_SetsAllElements()
    {
        var grid = new Grid<int>(2, 2);
        var result = grid.Fill((x, y) => x + y * 10);
        Assert.AreSame(grid, result);
        Assert.AreEqual(0, grid[0, 0]);
        Assert.AreEqual(1, grid[1, 0]);
        Assert.AreEqual(10, grid[0, 1]);
        Assert.AreEqual(11, grid[1, 1]);
    }

    [TestMethod]
    public void Fill_WithVector2Factory_SetsAllElements()
    {
        var grid = new Grid<int>(2, 2);
        var result = grid.Fill(pos => (int)pos.X + (int)pos.Y * 10);
        Assert.AreSame(grid, result);
        Assert.AreEqual(0, grid[0, 0]);
        Assert.AreEqual(1, grid[1, 0]);
        Assert.AreEqual(10, grid[0, 1]);
        Assert.AreEqual(11, grid[1, 1]);
    }

    [TestMethod]
    public void GetEnumerator_GenericAndNonGeneric_EnumeratesAllElements()
    {
        var grid = new Grid<int>(2, 2);
        grid.Fill((x, y) => x + y * 2);
        List<int> genericList = new List<int>();
        foreach (var item in grid)
        {
            genericList.Add(item);
        }
        CollectionAssert.AreEqual(new[] { 0, 1, 2, 3 }, genericList);
        IEnumerable nonGenericEnumerable = grid;
        List<int> nonGenericList = new List<int>();
        foreach (var item in nonGenericEnumerable)
        {
            nonGenericList.Add((int)item);
        }
        CollectionAssert.AreEqual(new[] { 0, 1, 2, 3 }, nonGenericList);
    }
}
