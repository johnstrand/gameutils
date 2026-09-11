using System;
using System.Collections;
using System.Numerics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GameUtils.Types.Collections;

namespace GameUtils.Tests.Types.Collections;

[TestClass]
public class GridTests
{
    [TestMethod]
    public void Constructor_ValidDimensions_SetsWidthAndHeight()
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
    public void Constructor_WithData_ValidData_InitializesData()
    {
        int[] data = [1, 2, 3, 4, 5, 6];
        var grid = new Grid<int>(3, 2, data);
        Assert.AreEqual(3, grid.Width);
        Assert.AreEqual(2, grid.Height);
        Assert.AreEqual(1, grid[0, 0]);
        Assert.AreEqual(6, grid[2, 1]);
    }

    [TestMethod]
    public void Constructor_WithData_MismatchedLength_ThrowsArgumentException()
    {
        int[] data = [1, 2, 3];
        Assert.ThrowsExactly<ArgumentException>(() => new Grid<int>(3, 2, data));
    }

    [TestMethod]
    [DataRow(-1, 2)]
    [DataRow(2, -1)]
    public void Constructor_WithData_InvalidDimensions_ThrowsArgumentOutOfRangeException(int width, int height)
    {
        int[] data = [];
        Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => new Grid<int>(width, height, data));
    }

    [TestMethod]
    public void Indexer_IntCoordinates_GetAndSet()
    {
        var grid = new Grid<int>(2, 2);
        grid[1, 0] = 42;
        Assert.AreEqual(42, grid[1, 0]);
    }

    [TestMethod]
    public void Indexer_Vector2Coordinates_GetAndSet()
    {
        var grid = new Grid<string>(2, 2);
        var pos = new Vector2(1, 1);
        grid[pos] = "test";
        Assert.AreEqual("test", grid[pos]);
    }

    [TestMethod]
    public void Indexer_OutOfBounds_ThrowsIndexOutOfRangeException()
    {
        var grid = new Grid<int>(2, 2);
        Assert.ThrowsExactly<IndexOutOfRangeException>(() => _ = grid[-1, 0]);
        Assert.ThrowsExactly<IndexOutOfRangeException>(() => grid[5, 5] = 10);
        Assert.ThrowsExactly<IndexOutOfRangeException>(() => _ = grid[new Vector2(-1, 0)]);
        Assert.ThrowsExactly<IndexOutOfRangeException>(() => grid[new Vector2(5, 5)] = 10);
    }

    [TestMethod]
    public void TryGet_IntCoordinates_InBoundsAndOutOfBounds()
    {
        var grid = new Grid<int>(2, 2);
        grid[0, 1] = 99;

        Assert.IsTrue(grid.TryGet(0, 1, out var valIn));
        Assert.AreEqual(99, valIn);

        Assert.IsFalse(grid.TryGet(-1, 0, out var valOut));
        Assert.AreEqual(0, valOut);
    }

    [TestMethod]
    public void TryGet_Vector2Coordinates_InBoundsAndOutOfBounds()
    {
        var grid = new Grid<int>(2, 2);
        grid[1, 1] = 88;

        Assert.IsTrue(grid.TryGet(new Vector2(1, 1), out var valIn));
        Assert.AreEqual(88, valIn);

        Assert.IsFalse(grid.TryGet(new Vector2(2, 0), out var valOut));
        Assert.AreEqual(0, valOut);
    }

    [TestMethod]
    public void TrySet_IntCoordinates_InBoundsAndOutOfBounds()
    {
        var grid = new Grid<int>(2, 2);

        Assert.IsTrue(grid.TrySet(0, 0, 15));
        Assert.AreEqual(15, grid[0, 0]);

        Assert.IsFalse(grid.TrySet(2, 2, 99));
    }

    [TestMethod]
    public void TrySet_Vector2Coordinates_InBoundsAndOutOfBounds()
    {
        var grid = new Grid<int>(2, 2);

        Assert.IsTrue(grid.TrySet(new Vector2(0, 1), 25));
        Assert.AreEqual(25, grid[0, 1]);

        Assert.IsFalse(grid.TrySet(new Vector2(-1, 0), 99));
    }

    [TestMethod]
    public void IsInBounds_IntAndVector2_ReturnsExpectedResult()
    {
        var grid = new Grid<int>(3, 3);

        Assert.IsTrue(grid.IsInBounds(0, 0));
        Assert.IsTrue(grid.IsInBounds(2, 2));
        Assert.IsFalse(grid.IsInBounds(-1, 0));
        Assert.IsFalse(grid.IsInBounds(0, -1));
        Assert.IsFalse(grid.IsInBounds(3, 0));
        Assert.IsFalse(grid.IsInBounds(0, 3));

        Assert.IsTrue(grid.IsInBounds(new Vector2(1, 1)));
        Assert.IsFalse(grid.IsInBounds(new Vector2(-1, 1)));
        Assert.IsFalse(grid.IsInBounds(new Vector2(1, 3)));
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
    public void Fill_WithValue_FillsAllElementsAndReturnsGrid()
    {
        var grid = new Grid<int>(2, 2);
        var returned = grid.Fill(7);

        Assert.AreSame(grid, returned);
        foreach (var val in grid)
        {
            Assert.AreEqual(7, val);
        }
    }

    [TestMethod]
    public void Fill_WithXYFactory_FillsGridAndReturnsGrid()
    {
        var grid = new Grid<int>(2, 2);
        var returned = grid.Fill((x, y) => x + y * 10);

        Assert.AreSame(grid, returned);
        Assert.AreEqual(0, grid[0, 0]);
        Assert.AreEqual(1, grid[1, 0]);
        Assert.AreEqual(10, grid[0, 1]);
        Assert.AreEqual(11, grid[1, 1]);
    }

    [TestMethod]
    public void Fill_WithVector2Factory_FillsGridAndReturnsGrid()
    {
        var grid = new Grid<int>(2, 2);
        var returned = grid.Fill(pos => (int)pos.X + (int)pos.Y * 10);

        Assert.AreSame(grid, returned);
        Assert.AreEqual(0, grid[0, 0]);
        Assert.AreEqual(1, grid[1, 0]);
        Assert.AreEqual(10, grid[0, 1]);
        Assert.AreEqual(11, grid[1, 1]);
    }

    [TestMethod]
    public void GetEnumerator_EnumeratesAllElements()
    {
        var grid = new Grid<int>(2, 2, [10, 20, 30, 40]);
        var list = new System.Collections.Generic.List<int>();

        foreach (var val in grid)
        {
            list.Add(val);
        }

        CollectionAssert.AreEqual(new[] { 10, 20, 30, 40 }, list);

        IEnumerable nonGenericGrid = grid;
        var nonGenericList = new System.Collections.Generic.List<object>();
        foreach (var val in nonGenericGrid)
        {
            nonGenericList.Add(val);
        }

        CollectionAssert.AreEqual(new object[] { 10, 20, 30, 40 }, nonGenericList);
    }
}
