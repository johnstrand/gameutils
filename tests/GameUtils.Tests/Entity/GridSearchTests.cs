using System;
using System.Linq;
using System.Numerics;
using GameUtils.Entity;
using GameUtils.Types.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GameUtils.Tests.Entity;

[TestClass]
public class GridSearchTests
{
    [TestMethod]
    public void BreadthFirstSearch_NullGrid_ThrowsArgumentNullException()
    {
        Grid<int>? grid = null;
        Assert.ThrowsExactly<ArgumentNullException>(() => GridSearch.BreadthFirstSearch(grid!, 0, 0, _ => true));
    }

    [TestMethod]
    public void BreadthFirstSearch_NullPassable_ThrowsArgumentNullException()
    {
        var grid = new Grid<int>(5, 5);
        Assert.ThrowsExactly<ArgumentNullException>(() => GridSearch.BreadthFirstSearch(grid, 0, 0, null!));
    }

    [TestMethod]
    public void BreadthFirstSearch_OutOfBoundsStart_ReturnsEmptyDistances()
    {
        var grid = new Grid<int>(5, 5);
        var distances = GridSearch.BreadthFirstSearch(grid, -1, 0, _ => true);

        foreach (var dist in distances)
        {
            Assert.AreEqual(-1, dist);
        }
    }

    [TestMethod]
    public void BreadthFirstSearch_UnpassableStart_ReturnsEmptyDistances()
    {
        var grid = new Grid<int>(5, 5);
        var distances = GridSearch.BreadthFirstSearch(grid, 0, 0, _ => false);

        foreach (var dist in distances)
        {
            Assert.AreEqual(-1, dist);
        }
    }

    [TestMethod]
    public void BreadthFirstSearch_SimpleGrid_ReturnsCorrectDistances()
    {
        var grid = new Grid<int>(3, 3);
        var distances = GridSearch.BreadthFirstSearch(grid, 1, 1, _ => true);

        // 2 1 2
        // 1 0 1
        // 2 1 2
        Assert.AreEqual(2, distances[0, 0]);
        Assert.AreEqual(1, distances[1, 0]);
        Assert.AreEqual(2, distances[2, 0]);
        Assert.AreEqual(1, distances[0, 1]);
        Assert.AreEqual(0, distances[1, 1]);
        Assert.AreEqual(1, distances[2, 1]);
        Assert.AreEqual(2, distances[0, 2]);
        Assert.AreEqual(1, distances[1, 2]);
        Assert.AreEqual(2, distances[2, 2]);
    }

    [TestMethod]
    public void BreadthFirstSearch_WithObstacles_AvoidsObstacles()
    {
        var grid = new Grid<int>(3, 3);
        // Map:
        // 0 1 0 (1 = obstacle)
        // 0 1 0
        // 0 0 0
        grid[1, 0] = 1;
        grid[1, 1] = 1;

        var distances = GridSearch.BreadthFirstSearch(grid, 0, 0, val => val == 0);

        Assert.AreEqual(0, distances[0, 0]);
        Assert.AreEqual(1, distances[0, 1]);
        Assert.AreEqual(2, distances[0, 2]);
        Assert.AreEqual(3, distances[1, 2]);
        Assert.AreEqual(4, distances[2, 2]);
        Assert.AreEqual(5, distances[2, 1]);
        Assert.AreEqual(6, distances[2, 0]);

        Assert.AreEqual(-1, distances[1, 0]);
        Assert.AreEqual(-1, distances[1, 1]);
    }

    [TestMethod]
    public void BreadthFirstSearch_Diagonal_AllowsDiagonalMovement()
    {
        var grid = new Grid<int>(3, 3);
        var distances = GridSearch.BreadthFirstSearch(grid, 1, 1, _ => true, diagonal: true);

        // 1 1 1
        // 1 0 1
        // 1 1 1
        Assert.AreEqual(1, distances[0, 0]);
        Assert.AreEqual(1, distances[2, 2]);
        Assert.AreEqual(1, distances[0, 2]);
        Assert.AreEqual(1, distances[2, 0]);
    }

    [TestMethod]
    public void BreadthFirstSearch_Vector2Start_WorksSameAsInt()
    {
        var grid = new Grid<int>(3, 3);
        var distances = GridSearch.BreadthFirstSearch(grid, new Vector2(1, 1), _ => true);

        Assert.AreEqual(0, distances[1, 1]);
        Assert.AreEqual(1, distances[0, 1]);
    }

    [TestMethod]
    public void FloodFill_NullGrid_ThrowsArgumentNullException()
    {
        Grid<int>? grid = null;
        Assert.ThrowsExactly<ArgumentNullException>(() => GridSearch.FloodFill(grid!, 0, 0, _ => true));
    }

    [TestMethod]
    public void FloodFill_NullPassable_ThrowsArgumentNullException()
    {
        var grid = new Grid<int>(5, 5);
        Assert.ThrowsExactly<ArgumentNullException>(() => GridSearch.FloodFill(grid, 0, 0, null!));
    }

    [TestMethod]
    public void FloodFill_OutOfBoundsStart_ReturnsEmptyList()
    {
        var grid = new Grid<int>(5, 5);
        var filled = GridSearch.FloodFill(grid, -1, -1, _ => true);

        Assert.AreEqual(0, filled.Count);
    }

    [TestMethod]
    public void FloodFill_UnpassableStart_ReturnsEmptyList()
    {
        var grid = new Grid<int>(5, 5);
        var filled = GridSearch.FloodFill(grid, 0, 0, _ => false);

        Assert.AreEqual(0, filled.Count);
    }

    [TestMethod]
    public void FloodFill_SimpleGrid_ReturnsReachableCells()
    {
        var grid = new Grid<int>(2, 2);
        var filled = GridSearch.FloodFill(grid, 0, 0, _ => true);

        Assert.AreEqual(4, filled.Count);
        Assert.IsTrue(filled.Contains((0, 0)));
        Assert.IsTrue(filled.Contains((1, 0)));
        Assert.IsTrue(filled.Contains((0, 1)));
        Assert.IsTrue(filled.Contains((1, 1)));
    }

    [TestMethod]
    public void FloodFill_WithObstacles_StopsAtObstacles()
    {
        var grid = new Grid<int>(3, 3);
        grid[1, 0] = 1;
        grid[1, 1] = 1;
        grid[1, 2] = 1;

        var filled = GridSearch.FloodFill(grid, 0, 0, val => val == 0);

        Assert.AreEqual(3, filled.Count);
        Assert.IsTrue(filled.Contains((0, 0)));
        Assert.IsTrue(filled.Contains((0, 1)));
        Assert.IsTrue(filled.Contains((0, 2)));
    }

    [TestMethod]
    public void FloodFill_Diagonal_FillsDiagonally()
    {
        var grid = new Grid<int>(3, 3);
        grid[1, 0] = 1;
        grid[0, 1] = 1;

        // Map:
        // 0 1 0
        // 1 0 0
        // 0 0 0

        // Without diagonal movement, we can't get past (0,0) because (1,0) and (0,1) are blocked
        var filledNoDiag = GridSearch.FloodFill(grid, 0, 0, val => val == 0, diagonal: false);
        Assert.AreEqual(1, filledNoDiag.Count);
        Assert.IsTrue(filledNoDiag.Contains((0, 0)));

        // With diagonal movement, we can move from (0,0) to (1,1) diagonally
        var filledDiag = GridSearch.FloodFill(grid, 0, 0, val => val == 0, diagonal: true);
        Assert.AreEqual(7, filledDiag.Count);
        Assert.IsTrue(filledDiag.Contains((1, 1)));
    }

    [TestMethod]
    public void FloodFill_Vector2Start_WorksSameAsInt()
    {
        var grid = new Grid<int>(2, 2);
        var filled = GridSearch.FloodFill(grid, new Vector2(0, 0), _ => true);

        Assert.AreEqual(4, filled.Count);
        Assert.IsTrue(filled.Contains((0, 0)));
    }
}
