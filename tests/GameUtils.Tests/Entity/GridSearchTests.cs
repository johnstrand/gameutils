using System;
using System.Numerics;
using GameUtils.Entity;
using GameUtils.Types.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GameUtils.Tests.Entity;

[TestClass]
public class GridSearchTests
{
    [TestMethod]
    public void BreadthFirstSearch_ValidGrid_ReturnsDistances()
    {
        // Arrange
        var grid = new Grid<int>(3, 3).Fill(1); // 1 is passable

        // Act
        var distances = GridSearch.BreadthFirstSearch(grid, 0, 0, cell => cell == 1);

        // Assert
        Assert.AreEqual(0, distances[0, 0]);
        Assert.AreEqual(1, distances[1, 0]);
        Assert.AreEqual(1, distances[0, 1]);
        Assert.AreEqual(2, distances[2, 0]);
        Assert.AreEqual(2, distances[1, 1]);
        Assert.AreEqual(2, distances[0, 2]);
        Assert.AreEqual(3, distances[2, 1]);
        Assert.AreEqual(3, distances[1, 2]);
        Assert.AreEqual(4, distances[2, 2]);
    }

    [TestMethod]
    public void BreadthFirstSearch_WithObstacles_AvoidsObstacles()
    {
        // Arrange
        var grid = new Grid<int>(3, 3).Fill(1);
        grid[1, 0] = 0; // obstacle
        grid[1, 1] = 0; // obstacle

        // Act
        var distances = GridSearch.BreadthFirstSearch(grid, 0, 0, cell => cell == 1);

        // Assert
        Assert.AreEqual(0, distances[0, 0]);
        Assert.AreEqual(-1, distances[1, 0]); // unpassable
        Assert.AreEqual(-1, distances[1, 1]); // unpassable
        Assert.AreEqual(1, distances[0, 1]);
        Assert.AreEqual(2, distances[0, 2]);
        Assert.AreEqual(3, distances[1, 2]);
        Assert.AreEqual(4, distances[2, 2]);
        Assert.AreEqual(5, distances[2, 1]);
        Assert.AreEqual(6, distances[2, 0]);
    }

    [TestMethod]
    public void BreadthFirstSearch_Diagonal_AllowsDiagonalMovement()
    {
        // Arrange
        var grid = new Grid<int>(3, 3).Fill(1);

        // Act
        var distances = GridSearch.BreadthFirstSearch(grid, 0, 0, cell => cell == 1, diagonal: true);

        // Assert
        Assert.AreEqual(0, distances[0, 0]);
        Assert.AreEqual(1, distances[1, 0]);
        Assert.AreEqual(1, distances[0, 1]);
        Assert.AreEqual(1, distances[1, 1]); // diagonal
        Assert.AreEqual(2, distances[2, 0]);
        Assert.AreEqual(2, distances[2, 1]);
        Assert.AreEqual(2, distances[1, 2]);
        Assert.AreEqual(2, distances[0, 2]);
        Assert.AreEqual(2, distances[2, 2]); // reachable in 2 steps diagonally
    }

    [TestMethod]
    public void BreadthFirstSearch_UnreachableCell_ReturnsMinusOne()
    {
        // Arrange
        var grid = new Grid<int>(3, 3).Fill(1);
        grid[1, 0] = 0; // block off top right corner
        grid[0, 1] = 0;
        grid[1, 1] = 0;

        // Act
        var distances = GridSearch.BreadthFirstSearch(grid, 0, 0, cell => cell == 1);

        // Assert
        Assert.AreEqual(0, distances[0, 0]);
        Assert.AreEqual(-1, distances[2, 0]); // unreachable
        Assert.AreEqual(-1, distances[2, 2]); // unreachable
    }

    [TestMethod]
    public void BreadthFirstSearch_OutOfBoundsStart_ReturnsMinusOneGrid()
    {
        // Arrange
        var grid = new Grid<int>(3, 3).Fill(1);

        // Act
        var distances = GridSearch.BreadthFirstSearch(grid, -1, 4, cell => cell == 1);

        // Assert
        for (int x = 0; x < distances.Width; x++)
        {
            for (int y = 0; y < distances.Height; y++)
            {
                Assert.AreEqual(-1, distances[x, y]);
            }
        }
    }

    [TestMethod]
    public void BreadthFirstSearch_UnpassableStart_ReturnsMinusOneGrid()
    {
        // Arrange
        var grid = new Grid<int>(3, 3).Fill(0); // 0 is unpassable

        // Act
        var distances = GridSearch.BreadthFirstSearch(grid, 1, 1, cell => cell == 1);

        // Assert
        for (int x = 0; x < distances.Width; x++)
        {
            for (int y = 0; y < distances.Height; y++)
            {
                Assert.AreEqual(-1, distances[x, y]);
            }
        }
    }

    [TestMethod]
    public void BreadthFirstSearch_ThrowsArgumentNullException_OnNullGrid()
    {
        // Arrange
        Grid<int> grid = null!;
        Func<int, bool> passable = cell => cell == 1;

        // Act & Assert
        Assert.ThrowsExactly<ArgumentNullException>(() =>
            GridSearch.BreadthFirstSearch(grid, 0, 0, passable));
    }

    [TestMethod]
    public void BreadthFirstSearch_ThrowsArgumentNullException_OnNullPassable()
    {
        // Arrange
        var grid = new Grid<int>(3, 3).Fill(1);
        Func<int, bool> passable = null!;

        // Act & Assert
        Assert.ThrowsExactly<ArgumentNullException>(() =>
            GridSearch.BreadthFirstSearch(grid, 0, 0, passable));
    }
}
