using System;
using System.Collections.Generic;
using System.Numerics;
using GameUtils.Types.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
    public void Constructor_WithData_ValidLength_InitializesGrid()
    {
        var data = new int[] { 1, 2, 3, 4, 5, 6 };
        var grid = new Grid<int>(3, 2, data);
        Assert.AreEqual(3, grid.Width);
        Assert.AreEqual(2, grid.Height);
        Assert.AreEqual(1, grid[0, 0]);
        Assert.AreEqual(6, grid[2, 1]);
    }

    [TestMethod]
    public void Constructor_WithData_MismatchedLength_ThrowsArgumentException()
    {
        var data = new int[] { 1, 2, 3 };
        Assert.ThrowsExactly<ArgumentException>(() => new Grid<int>(3, 2, data));
    }

    [TestMethod]
    public void Indexer_IntCoordinates_GetsAndSetsValue()
    {
        var grid = new Grid<string>(2, 2);
        grid[1, 0] = "hello";
        Assert.AreEqual("hello", grid[1, 0]);
    }

    [TestMethod]
    public void Indexer_Vector2Coordinates_GetsAndSetsValue()
    {
        var grid = new Grid<string>(2, 2);
        var pos = new Vector2(1, 1);
        grid[pos] = "world";
        Assert.AreEqual("world", grid[pos]);
    }

    [TestMethod]
    public void Indexer_OutOfBounds_ThrowsIndexOutOfRangeException()
    {
        var grid = new Grid<int>(2, 2);
        Assert.ThrowsExactly<IndexOutOfRangeException>(() => _ = grid[-1, 0]);
        Assert.ThrowsExactly<IndexOutOfRangeException>(() => _ = grid[2, 2]);
        Assert.ThrowsExactly<IndexOutOfRangeException>(() => _ = grid[0, -1]);
        Assert.ThrowsExactly<IndexOutOfRangeException>(() => _ = grid[0, 3]);
    }

    [TestMethod]
    public void TryGet_IntCoordinates_ValidPosition_ReturnsTrueAndValue()
    {
        var grid = new Grid<int>(3, 3);
        grid[1, 2] = 42;

        bool result = grid.TryGet(1, 2, out int value);

        Assert.IsTrue(result);
        Assert.AreEqual(42, value);
    }

    [TestMethod]
    [DataRow(-1, 0)]
    [DataRow(3, 0)]
    [DataRow(0, -1)]
    [DataRow(0, 3)]
    public void TryGet_IntCoordinates_OutOfBounds_ReturnsFalseAndDefault(int x, int y)
    {
        var grid = new Grid<int>(3, 3);

        bool result = grid.TryGet(x, y, out int value);

        Assert.IsFalse(result);
        Assert.AreEqual(default, value);
    }

    [TestMethod]
    public void TryGet_Vector2Coordinates_ValidPosition_ReturnsTrueAndValue()
    {
        var grid = new Grid<int>(3, 3);
        var pos = new Vector2(2, 1);
        grid[pos] = 99;

        bool result = grid.TryGet(pos, out int value);

        Assert.IsTrue(result);
        Assert.AreEqual(99, value);
    }

    [TestMethod]
    public void TryGet_Vector2Coordinates_OutOfBounds_ReturnsFalseAndDefault()
    {
        var grid = new Grid<int>(3, 3);

        bool result1 = grid.TryGet(new Vector2(-1, 0), out int val1);
        bool result2 = grid.TryGet(new Vector2(3, 0), out int val2);
        bool result3 = grid.TryGet(new Vector2(0, -1), out int val3);
        bool result4 = grid.TryGet(new Vector2(0, 3), out int val4);

        Assert.IsFalse(result1);
        Assert.IsFalse(result2);
        Assert.IsFalse(result3);
        Assert.IsFalse(result4);
        Assert.AreEqual(default, val1);
        Assert.AreEqual(default, val2);
        Assert.AreEqual(default, val3);
        Assert.AreEqual(default, val4);
    }

    [TestMethod]
    public void TrySet_IntCoordinates_ValidPosition_ReturnsTrueAndSetsValue()
    {
        var grid = new Grid<int>(3, 3);

        bool result = grid.TrySet(1, 2, 77);

        Assert.IsTrue(result);
        Assert.AreEqual(77, grid[1, 2]);
    }

    [TestMethod]
    [DataRow(-1, 0)]
    [DataRow(3, 0)]
    [DataRow(0, -1)]
    [DataRow(0, 3)]
    public void TrySet_IntCoordinates_OutOfBounds_ReturnsFalseAndDoesNotModify(int x, int y)
    {
        var grid = new Grid<int>(3, 3);

        bool result = grid.TrySet(x, y, 77);

        Assert.IsFalse(result);
    }

    [TestMethod]
    public void TrySet_Vector2Coordinates_ValidPosition_ReturnsTrueAndSetsValue()
    {
        var grid = new Grid<int>(3, 3);
        var pos = new Vector2(1, 1);

        bool result = grid.TrySet(pos, 88);

        Assert.IsTrue(result);
        Assert.AreEqual(88, grid[pos]);
    }

    [TestMethod]
    public void TrySet_Vector2Coordinates_OutOfBounds_ReturnsFalse()
    {
        var grid = new Grid<int>(3, 3);

        bool result = grid.TrySet(new Vector2(-1, 0), 88);

        Assert.IsFalse(result);
    }

    [TestMethod]
    public void IsInBounds_IntAndVector2_ReturnsExpectedResult()
    {
        var grid = new Grid<int>(2, 2);

        Assert.IsTrue(grid.IsInBounds(0, 0));
        Assert.IsTrue(grid.IsInBounds(1, 1));
        Assert.IsFalse(grid.IsInBounds(-1, 0));
        Assert.IsFalse(grid.IsInBounds(2, 0));

        Assert.IsTrue(grid.IsInBounds(new Vector2(0, 0)));
        Assert.IsFalse(grid.IsInBounds(new Vector2(2, 2)));
    }

    [TestMethod]
    public void Clear_ResetsAllElementsToDefault()
    {
        var grid = new Grid<int>(2, 2);
        grid.Fill(10);
        grid.Clear();

        foreach (var val in grid)
        {
            Assert.AreEqual(0, val);
        }
    }

    [TestMethod]
    public void Fill_WithValue_FillsAllElements()
    {
        var grid = new Grid<int>(2, 2);
        grid.Fill(5);

        foreach (var val in grid)
        {
            Assert.AreEqual(5, val);
        }
    }

    [TestMethod]
    public void Fill_WithXYFactory_FillsElementsBasedOnCoordinates()
    {
        var grid = new Grid<int>(2, 2);
        grid.Fill((x, y) => x + y * 10);

        Assert.AreEqual(0, grid[0, 0]);
        Assert.AreEqual(1, grid[1, 0]);
        Assert.AreEqual(10, grid[0, 1]);
        Assert.AreEqual(11, grid[1, 1]);
    }

    [TestMethod]
    public void Fill_WithVector2Factory_FillsElementsBasedOnVector()
    {
        var grid = new Grid<int>(2, 2);
        grid.Fill(v => (int)v.X + (int)v.Y * 10);

        Assert.AreEqual(0, grid[0, 0]);
        Assert.AreEqual(1, grid[1, 0]);
        Assert.AreEqual(10, grid[0, 1]);
        Assert.AreEqual(11, grid[1, 1]);
    }

    [TestMethod]
    public void GetEnumerator_EnumeratesAllElementsInOrder()
    {
        var grid = new Grid<int>(2, 2);
        grid[0, 0] = 1;
        grid[1, 0] = 2;
        grid[0, 1] = 3;
        grid[1, 1] = 4;

        var list = new List<int>();
        foreach (var item in grid)
        {
            list.Add(item);
        }

        CollectionAssert.AreEqual(new int[] { 1, 2, 3, 4 }, list);
    }
}
