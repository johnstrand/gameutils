using System.Collections.Generic;
using GameUtils.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GameUtils.Tests.Entity;

[TestClass]
public class AStarTests
{
    private static float MockHeuristic(string a, string b) => 0f; // Acts as Dijkstra when heuristic is 0
    private static float ManhattanHeuristic((int x, int y) a, (int x, int y) b)
        => System.Math.Abs(a.x - b.x) + System.Math.Abs(a.y - b.y);

    [TestMethod]
    public void Solve_WithValidPath_ReturnsTrueAndPath()
    {
        // Arrange
        var astar = new AStar<string>();
        astar.AddEdge(new Edge<string>("A", "B", 1f));
        astar.AddEdge(new Edge<string>("B", "C", 1f));
        astar.AddEdge(new Edge<string>("C", "D", 1f));

        // Act
        var result = astar.Solve("A", "D", MockHeuristic, out var path);

        // Assert
        Assert.IsTrue(result);
        CollectionAssert.AreEqual(new[] { "A", "B", "C", "D" }, path);
    }

    [TestMethod]
    public void Solve_WithWeights_FavorsLighterPath()
    {
        // Arrange
        var astar = new AStar<string>();
        // Path 1: A -> B -> C (Cost = 10)
        astar.AddEdge(new Edge<string>("A", "B", 5f));
        astar.AddEdge(new Edge<string>("B", "C", 5f));

        // Path 2: A -> D -> E -> C (Cost = 3)
        astar.AddEdge(new Edge<string>("A", "D", 1f));
        astar.AddEdge(new Edge<string>("D", "E", 1f));
        astar.AddEdge(new Edge<string>("E", "C", 1f));

        // Act
        var result = astar.Solve("A", "C", MockHeuristic, out var path);

        // Assert
        Assert.IsTrue(result);
        CollectionAssert.AreEqual(new[] { "A", "D", "E", "C" }, path);
    }

    [TestMethod]
    public void Solve_WhenNoPath_ReturnsFalse()
    {
        // Arrange
        var astar = new AStar<string>();
        astar.AddEdge(new Edge<string>("A", "B", 1f));
        astar.AddEdge(new Edge<string>("C", "D", 1f)); // Disconnected from A/B

        // Act
        var result = astar.Solve("A", "D", MockHeuristic, out var path);

        // Assert
        Assert.IsFalse(result);
        Assert.AreEqual(0, path.Count);
    }

    [TestMethod]
    public void Solve_WithMissingNodes_ReturnsFalse()
    {
        // Arrange
        var astar = new AStar<string>();
        astar.AddEdge(new Edge<string>("A", "B", 1f));

        // Act
        var resultStartMissing = astar.Solve("X", "B", MockHeuristic, out var path1);
        var resultEndMissing = astar.Solve("A", "Y", MockHeuristic, out var path2);

        // Assert
        Assert.IsFalse(resultStartMissing);
        Assert.AreEqual(0, path1.Count);
        Assert.IsFalse(resultEndMissing);
        Assert.AreEqual(0, path2.Count);
    }

    [TestMethod]
    public void Solve_WithDirectedEdges_RespectsDirection()
    {
        // Arrange
        var astar = new AStar<string>();
        astar.AddEdge(new Edge<string>("A", "B", 1f, IsDirected: true));

        // Act
        var resultForward = astar.Solve("A", "B", MockHeuristic, out var pathForward);
        var resultBackward = astar.Solve("B", "A", MockHeuristic, out var pathBackward);

        // Assert
        Assert.IsTrue(resultForward);
        CollectionAssert.AreEqual(new[] { "A", "B" }, pathForward);

        Assert.IsFalse(resultBackward);
        Assert.AreEqual(0, pathBackward.Count);
    }

    [TestMethod]
    public void Solve_ThrowsArgumentNullException_OnNullHeuristic()
    {
        // Arrange
        var astar = new AStar<string>();
        astar.AddNode("A");
        astar.AddNode("B");

        // Act & Assert
        try
        {
            astar.Solve("A", "B", null!, out _);
            Assert.Fail("Expected ArgumentNullException");
        }
        catch (System.ArgumentNullException)
        {
            // Expected
        }
    }

    [TestMethod]
    public void Solve_WithComplexGrid_FindsShortestPathUsingHeuristic()
    {
        // Arrange
        var astar = new AStar<(int x, int y)>();

        // Grid:
        // (0,2) -- (1,2) -- (2,2)
        //   |                 |
        // (0,1)    (1,1)    (2,1)
        //   |                 |
        // (0,0) -- (1,0) -- (2,0)
        // (1,1) is unpathable/obstacle

        astar.AddEdge(new Edge<(int x, int y)>((0, 0), (1, 0), 1f));
        astar.AddEdge(new Edge<(int x, int y)>((1, 0), (2, 0), 1f));

        astar.AddEdge(new Edge<(int x, int y)>((0, 0), (0, 1), 1f));
        astar.AddEdge(new Edge<(int x, int y)>((2, 0), (2, 1), 1f));

        astar.AddEdge(new Edge<(int x, int y)>((0, 1), (0, 2), 1f));
        astar.AddEdge(new Edge<(int x, int y)>((2, 1), (2, 2), 1f));

        astar.AddEdge(new Edge<(int x, int y)>((0, 2), (1, 2), 1f));
        astar.AddEdge(new Edge<(int x, int y)>((1, 2), (2, 2), 1f));

        // Act
        var result = astar.Solve((0, 0), (2, 2), ManhattanHeuristic, out var path);

        // Assert
        Assert.IsTrue(result);
        Assert.AreEqual(5, path.Count); // (0,0) -> (0,1) -> (0,2) -> (1,2) -> (2,2) OR (0,0) -> (1,0) -> (2,0) -> (2,1) -> (2,2)
    }

    [TestMethod]
    public void Solve_StartEqualsEnd_ReturnsPathWithOneNode()
    {
        // Arrange
        var astar = new AStar<string>();
        astar.AddNode("A");

        // Act
        var result = astar.Solve("A", "A", MockHeuristic, out var path);

        // Assert
        Assert.IsTrue(result);
        CollectionAssert.AreEqual(new[] { "A" }, path);
    }

    [TestMethod]
    public void Constructor_WithNodesAndEdges_InitializesCorrectly()
    {
        // Arrange
        var nodes = new[] { "A", "B", "C" };
        var edges = new[]
        {
            new Edge<string>("A", "B", 1f),
            new Edge<string>("B", "C", 2f)
        };

        // Act
        var astar = new AStar<string>(nodes, edges);
        var result = astar.Solve("A", "C", MockHeuristic, out var path);

        // Assert
        Assert.IsTrue(result);
        CollectionAssert.AreEqual(new[] { "A", "B", "C" }, path);
    }

    [TestMethod]
    public void AddNodes_AddEdges_AddsMultipleElements()
    {
        // Arrange
        var astar = new AStar<string>();
        var nodes = new[] { "A", "B", "C" };
        var edges = new[]
        {
            new Edge<string>("A", "B", 1f),
            new Edge<string>("B", "C", 2f)
        };

        // Act
        astar.AddNodes(nodes);
        astar.AddEdges(edges);
        var result = astar.Solve("A", "C", MockHeuristic, out var path);

        // Assert
        Assert.IsTrue(result);
        CollectionAssert.AreEqual(new[] { "A", "B", "C" }, path);
    }
}
