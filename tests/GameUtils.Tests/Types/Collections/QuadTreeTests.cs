using System.Linq;
using System.Numerics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GameUtils.Types.Collections;
using GameUtils.Types.Geometry;

namespace GameUtils.Tests.Types.Collections;

[TestClass]
public class QuadTreeTests
{
    [TestMethod]
    public void Insert_OutOfBounds_ReturnsFalse()
    {
        var bounds = new AABB(new Vector2(0, 0), new Vector2(100, 100));
        var tree = new QuadTree<string>(bounds);

        bool result = tree.Insert("Item1", new Vector2(-10, 50));

        Assert.IsFalse(result);
        Assert.AreEqual(0, tree.Query(bounds).Count());
    }

    [TestMethod]
    public void Insert_InBounds_ReturnsTrueAndItemIsQueried()
    {
        var bounds = new AABB(new Vector2(0, 0), new Vector2(100, 100));
        var tree = new QuadTree<string>(bounds);

        bool result = tree.Insert("Item1", new Vector2(50, 50));

        Assert.IsTrue(result);
        var queried = tree.Query(bounds).ToList();
        Assert.AreEqual(1, queried.Count);
        Assert.AreEqual("Item1", queried[0]);
    }

    [TestMethod]
    public void Remove_OutOfBoundsPosition_ReturnsFalse()
    {
        var bounds = new AABB(new Vector2(0, 0), new Vector2(100, 100));
        var tree = new QuadTree<string>(bounds);
        tree.Insert("Item1", new Vector2(50, 50));

        bool removed = tree.Remove("Item1", new Vector2(150, 150));

        Assert.IsFalse(removed);
        Assert.AreEqual(1, tree.Query(bounds).Count());
    }

    [TestMethod]
    public void Remove_ItemNotInTree_ReturnsFalse()
    {
        var bounds = new AABB(new Vector2(0, 0), new Vector2(100, 100));
        var tree = new QuadTree<string>(bounds);
        tree.Insert("Item1", new Vector2(50, 50));

        bool removed = tree.Remove("NonExistent", new Vector2(50, 50));

        Assert.IsFalse(removed);
        Assert.AreEqual(1, tree.Query(bounds).Count());
    }

    [TestMethod]
    public void Remove_ItemInUnsubdividedTree_RemovesItemAndReturnsTrue()
    {
        var bounds = new AABB(new Vector2(0, 0), new Vector2(100, 100));
        var tree = new QuadTree<string>(bounds);
        var pos = new Vector2(25, 25);
        tree.Insert("Item1", pos);

        var beforeQuery = tree.Query(bounds).ToList();
        Assert.AreEqual(1, beforeQuery.Count);
        Assert.AreEqual("Item1", beforeQuery[0]);

        bool removed = tree.Remove("Item1", pos);

        Assert.IsTrue(removed);
        var afterQuery = tree.Query(bounds).ToList();
        Assert.AreEqual(0, afterQuery.Count);
    }

    [TestMethod]
    public void Remove_WrongItemAtPosition_ReturnsFalse()
    {
        var bounds = new AABB(new Vector2(0, 0), new Vector2(100, 100));
        var tree = new QuadTree<string>(bounds);
        var pos = new Vector2(30, 30);
        tree.Insert("ActualItem", pos);

        bool removed = tree.Remove("WrongItem", pos);

        Assert.IsFalse(removed);
        var items = tree.Query(bounds).ToList();
        Assert.AreEqual(1, items.Count);
        Assert.AreEqual("ActualItem", items[0]);
    }

    [TestMethod]
    public void Remove_WrongPositionForItem_ReturnsFalse()
    {
        var bounds = new AABB(new Vector2(0, 0), new Vector2(100, 100));
        var tree = new QuadTree<string>(bounds);
        tree.Insert("Item1", new Vector2(30, 30));

        bool removed = tree.Remove("Item1", new Vector2(40, 40));

        Assert.IsFalse(removed);
        var items = tree.Query(bounds).ToList();
        Assert.AreEqual(1, items.Count);
        Assert.AreEqual("Item1", items[0]);
    }

    [TestMethod]
    public void Remove_SubdividedTree_RemovesItemFromChildAndReturnsTrue()
    {
        var bounds = new AABB(new Vector2(0, 0), new Vector2(100, 100));
        var tree = new QuadTree<string>(bounds, capacity: 2);

        var pos1 = new Vector2(10, 10);
        var pos2 = new Vector2(80, 10);
        var pos3 = new Vector2(10, 80);

        tree.Insert("Item1", pos1);
        tree.Insert("Item2", pos2);
        tree.Insert("Item3", pos3);

        var beforeQuery = tree.Query(bounds).ToList();
        Assert.AreEqual(3, beforeQuery.Count);

        bool removed = tree.Remove("Item2", pos2);

        Assert.IsTrue(removed);
        var afterQuery = tree.Query(bounds).ToList();
        Assert.AreEqual(2, afterQuery.Count);
        Assert.IsFalse(afterQuery.Contains("Item2"));
        Assert.IsTrue(afterQuery.Contains("Item1"));
        Assert.IsTrue(afterQuery.Contains("Item3"));
    }

    [TestMethod]
    public void Remove_SubdividedTree_ItemNotFound_ReturnsFalse()
    {
        var bounds = new AABB(new Vector2(0, 0), new Vector2(100, 100));
        var tree = new QuadTree<string>(bounds, capacity: 2);

        tree.Insert("Item1", new Vector2(10, 10));
        tree.Insert("Item2", new Vector2(80, 10));
        tree.Insert("Item3", new Vector2(10, 80));

        bool removed = tree.Remove("MissingItem", new Vector2(80, 10));

        Assert.IsFalse(removed);
        Assert.AreEqual(3, tree.Query(bounds).Count());
    }

    [TestMethod]
    public void Remove_DuplicateItemsAtSamePosition_RemovesOnlyOne()
    {
        var bounds = new AABB(new Vector2(0, 0), new Vector2(100, 100));
        var tree = new QuadTree<string>(bounds);
        var pos = new Vector2(50, 50);

        tree.Insert("Item1", pos);
        tree.Insert("Item1", pos);

        Assert.AreEqual(2, tree.Query(bounds).Count());

        bool removed = tree.Remove("Item1", pos);

        Assert.IsTrue(removed);
        Assert.AreEqual(1, tree.Query(bounds).Count());
    }

    [TestMethod]
    public void Remove_ThenReInsert_SuccessfullyInsertsAndQueries()
    {
        var bounds = new AABB(new Vector2(0, 0), new Vector2(100, 100));
        var tree = new QuadTree<string>(bounds);
        var pos = new Vector2(20, 20);

        tree.Insert("Item1", pos);
        Assert.IsTrue(tree.Remove("Item1", pos));
        Assert.AreEqual(0, tree.Query(bounds).Count());

        Assert.IsTrue(tree.Insert("Item1", pos));
        var queried = tree.Query(bounds).ToList();
        Assert.AreEqual(1, queried.Count);
        Assert.AreEqual("Item1", queried[0]);
    }

    [TestMethod]
    public void Clear_RemovesAllItemsAndResetsTree()
    {
        var bounds = new AABB(new Vector2(0, 0), new Vector2(100, 100));
        var tree = new QuadTree<string>(bounds, capacity: 2);

        tree.Insert("Item1", new Vector2(10, 10));
        tree.Insert("Item2", new Vector2(80, 10));
        tree.Insert("Item3", new Vector2(10, 80));

        tree.Clear();

        Assert.AreEqual(0, tree.Query(bounds).Count());
        Assert.IsFalse(tree.Remove("Item1", new Vector2(10, 10)));
    }

    [TestMethod]
    public void Query_Radius_ReturnsOnlyItemsWithinRadius()
    {
        var bounds = new AABB(new Vector2(0, 0), new Vector2(100, 100));
        var tree = new QuadTree<string>(bounds);

        var center = new Vector2(50, 50);
        tree.Insert("Near", new Vector2(52, 50));
        tree.Insert("Far", new Vector2(90, 90));

        var results = tree.Query(center, 10f).ToList();

        Assert.AreEqual(1, results.Count);
        Assert.AreEqual("Near", results[0]);
    }
}
