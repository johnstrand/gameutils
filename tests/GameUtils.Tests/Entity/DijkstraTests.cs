using System.Collections.Generic;
using GameUtils.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GameUtils.Tests.Entity;

[TestClass]
public class DijkstraTests
{
    [TestMethod]
    public void Solve_WithValidPath_ReturnsTrueAndPath()
    {
        // Arrange
        var dijkstra = new Dijkstra<string>();
        dijkstra.AddEdge(new Edge<string>("A", "B", 1f));
        dijkstra.AddEdge(new Edge<string>("B", "C", 1f));
        dijkstra.AddEdge(new Edge<string>("C", "D", 1f));

        // Act
        var result = dijkstra.Solve("A", "D", out var path);

        // Assert
        Assert.IsTrue(result);
        CollectionAssert.AreEqual(new[] { "A", "B", "C", "D" }, path);
    }

    [TestMethod]
    public void Solve_WithWeights_FavorsLighterPath()
    {
        // Arrange
        var dijkstra = new Dijkstra<string>();
        // Path 1: A -> B -> C (Cost = 10)
        dijkstra.AddEdge(new Edge<string>("A", "B", 5f));
        dijkstra.AddEdge(new Edge<string>("B", "C", 5f));

        // Path 2: A -> D -> E -> C (Cost = 3)
        dijkstra.AddEdge(new Edge<string>("A", "D", 1f));
        dijkstra.AddEdge(new Edge<string>("D", "E", 1f));
        dijkstra.AddEdge(new Edge<string>("E", "C", 1f));

        // Act
        var result = dijkstra.Solve("A", "C", out var path);

        // Assert
        Assert.IsTrue(result);
        CollectionAssert.AreEqual(new[] { "A", "D", "E", "C" }, path);
    }

    [TestMethod]
    public void Solve_WhenNoPath_ReturnsFalse()
    {
        // Arrange
        var dijkstra = new Dijkstra<string>();
        dijkstra.AddEdge(new Edge<string>("A", "B", 1f));
        dijkstra.AddEdge(new Edge<string>("C", "D", 1f)); // Disconnected from A/B

        // Act
        var result = dijkstra.Solve("A", "D", out var path);

        // Assert
        Assert.IsFalse(result);
        Assert.AreEqual(0, path.Count);
    }

    [TestMethod]
    public void Solve_WithMissingNodes_ReturnsFalse()
    {
        // Arrange
        var dijkstra = new Dijkstra<string>();
        dijkstra.AddEdge(new Edge<string>("A", "B", 1f));

        // Act
        var resultStartMissing = dijkstra.Solve("X", "B", out var path1);
        var resultEndMissing = dijkstra.Solve("A", "Y", out var path2);

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
        var dijkstra = new Dijkstra<string>();
        dijkstra.AddEdge(new Edge<string>("A", "B", 1f, IsDirected: true));

        // Act
        var resultForward = dijkstra.Solve("A", "B", out var pathForward);
        var resultBackward = dijkstra.Solve("B", "A", out var pathBackward);

        // Assert
        Assert.IsTrue(resultForward);
        CollectionAssert.AreEqual(new[] { "A", "B" }, pathForward);

        Assert.IsFalse(resultBackward);
        Assert.AreEqual(0, pathBackward.Count);
    }

    [TestMethod]
    public void Solve_StartEqualsEnd_ReturnsPathWithOneNode()
    {
        // Arrange
        var dijkstra = new Dijkstra<string>();
        dijkstra.AddNode("A");

        // Act
        var result = dijkstra.Solve("A", "A", out var path);

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
        var dijkstra = new Dijkstra<string>(nodes, edges);
        var result = dijkstra.Solve("A", "C", out var path);

        // Assert
        Assert.IsTrue(result);
        CollectionAssert.AreEqual(new[] { "A", "B", "C" }, path);
    }

    [TestMethod]
    public void AddNodes_AddEdges_AddsMultipleElements()
    {
        // Arrange
        var dijkstra = new Dijkstra<string>();
        var nodes = new[] { "A", "B", "C" };
        var edges = new[]
        {
            new Edge<string>("A", "B", 1f),
            new Edge<string>("B", "C", 2f)
        };

        // Act
        dijkstra.AddNodes(nodes);
        dijkstra.AddEdges(edges);
        var result = dijkstra.Solve("A", "C", out var path);

        // Assert
        Assert.IsTrue(result);
        CollectionAssert.AreEqual(new[] { "A", "B", "C" }, path);
    }

    [TestMethod]
    public void RemoveEdge_RemovesConnection_ChangesPath()
    {
        // Arrange
        var dijkstra = new Dijkstra<string>();
        dijkstra.AddEdge(new Edge<string>("A", "B", 1f));
        var edgeToRemove = new Edge<string>("A", "C", 2f);
        dijkstra.AddEdge(edgeToRemove);
        dijkstra.AddEdge(new Edge<string>("B", "C", 5f));

        // Verify initial path goes through C
        var resultInitial = dijkstra.Solve("A", "C", out var pathInitial);
        Assert.IsTrue(resultInitial);
        CollectionAssert.AreEqual(new[] { "A", "C" }, pathInitial);

        // Act
        dijkstra.RemoveEdge(edgeToRemove);
        var resultAfter = dijkstra.Solve("A", "C", out var pathAfter);

        // Assert
        Assert.IsTrue(resultAfter);
        // It must now go through B
        CollectionAssert.AreEqual(new[] { "A", "B", "C" }, pathAfter);
    }
}
