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
    public void Insert_OutsideBounds_ReturnsFalse()
    {
        var bounds = new AABB(new Vector2(0, 0), new Vector2(10, 10));
        var quadTree = new QuadTree<string>(bounds);

        bool result = quadTree.Insert("item1", new Vector2(15, 15));

        Assert.IsFalse(result);
        Assert.AreEqual(0, quadTree.Query(bounds).Count());
    }

    [TestMethod]
    public void Insert_InsideBounds_ReturnsTrueAndQueryable()
    {
        var bounds = new AABB(new Vector2(0, 0), new Vector2(10, 10));
        var quadTree = new QuadTree<string>(bounds);

        bool result = quadTree.Insert("item1", new Vector2(5, 5));

        Assert.IsTrue(result);
        var items = quadTree.Query(bounds).ToList();
        Assert.AreEqual(1, items.Count);
        Assert.AreEqual("item1", items[0]);
    }

    [TestMethod]
    public void Insert_AtBoundaries_ReturnsTrue()
    {
        var bounds = new AABB(new Vector2(0, 0), new Vector2(10, 10));
        var quadTree = new QuadTree<string>(bounds);

        Assert.IsTrue(quadTree.Insert("min", new Vector2(0, 0)));
        Assert.IsTrue(quadTree.Insert("max", new Vector2(10, 10)));
        Assert.AreEqual(2, quadTree.Query(bounds).Count());
    }

    [TestMethod]
    public void Insert_ExceedingCapacity_SubdividesAndDistributesItems()
    {
        var bounds = new AABB(new Vector2(0, 0), new Vector2(100, 100));
        var quadTree = new QuadTree<string>(bounds, capacity: 4);

        quadTree.Insert("TL", new Vector2(25, 25));
        quadTree.Insert("TR", new Vector2(75, 25));
        quadTree.Insert("BL", new Vector2(25, 75));
        quadTree.Insert("BR", new Vector2(75, 75));

        quadTree.Insert("TL2", new Vector2(10, 10));

        var allItems = quadTree.Query(bounds).ToList();
        Assert.AreEqual(5, allItems.Count);

        var tlRegion = new AABB(new Vector2(0, 0), new Vector2(50, 50));
        var tlItems = quadTree.Query(tlRegion).ToList();
        Assert.IsTrue(tlItems.Contains("TL"));
        Assert.IsTrue(tlItems.Contains("TL2"));
        Assert.AreEqual(2, tlItems.Count);
    }

    [TestMethod]
    public void Insert_ExceedingMaxDepth_StopsSubdividingAndStoresInLeaf()
    {
        var bounds = new AABB(new Vector2(0, 0), new Vector2(10, 10));
        var quadTree = new QuadTree<string>(bounds, capacity: 1);

        for (int i = 0; i < 20; i++)
        {
            Assert.IsTrue(quadTree.Insert($"item{i}", new Vector2(5, 5)));
        }

        Assert.AreEqual(20, quadTree.Query(bounds).Count());
    }

    [TestMethod]
    public void Insert_MultipleItemsAtSamePosition_ReturnsTrue()
    {
        var bounds = new AABB(new Vector2(0, 0), new Vector2(10, 10));
        var quadTree = new QuadTree<string>(bounds);

        Assert.IsTrue(quadTree.Insert("item1", new Vector2(5, 5)));
        Assert.IsTrue(quadTree.Insert("item2", new Vector2(5, 5)));
        Assert.AreEqual(2, quadTree.Query(bounds).Count());
    }

    [TestMethod]
    public void Remove_InsertedItem_ReturnsTrueAndRemovesItem()
    {
        var bounds = new AABB(new Vector2(0, 0), new Vector2(10, 10));
        var quadTree = new QuadTree<string>(bounds);

        quadTree.Insert("item1", new Vector2(5, 5));
        bool removed = quadTree.Remove("item1", new Vector2(5, 5));

        Assert.IsTrue(removed);
        Assert.AreEqual(0, quadTree.Query(bounds).Count());
    }

    [TestMethod]
    public void Remove_SubdividedTreeItem_ReturnsTrueAndRemovesItem()
    {
        var bounds = new AABB(new Vector2(0, 0), new Vector2(100, 100));
        var quadTree = new QuadTree<string>(bounds, capacity: 2);

        quadTree.Insert("A", new Vector2(10, 10));
        quadTree.Insert("B", new Vector2(20, 20));
        quadTree.Insert("C", new Vector2(80, 80));

        Assert.IsTrue(quadTree.Remove("C", new Vector2(80, 80)));
        Assert.AreEqual(2, quadTree.Query(bounds).Count());
    }

    [TestMethod]
    public void Clear_RemovesAllItems()
    {
        var bounds = new AABB(new Vector2(0, 0), new Vector2(100, 100));
        var quadTree = new QuadTree<string>(bounds, capacity: 2);

        quadTree.Insert("A", new Vector2(10, 10));
        quadTree.Insert("B", new Vector2(20, 20));
        quadTree.Insert("C", new Vector2(80, 80));

        quadTree.Clear();

        Assert.AreEqual(0, quadTree.Query(bounds).Count());
    }

    [TestMethod]
    public void Query_Radius_ReturnsItemsWithinRadius()
    {
        var bounds = new AABB(new Vector2(0, 0), new Vector2(100, 100));
        var quadTree = new QuadTree<string>(bounds);

        quadTree.Insert("center", new Vector2(50, 50));
        quadTree.Insert("near", new Vector2(52, 50));
        quadTree.Insert("far", new Vector2(90, 90));

        var results = quadTree.Query(new Vector2(50, 50), 5f).ToList();

        Assert.AreEqual(2, results.Count);
        Assert.IsTrue(results.Contains("center"));
        Assert.IsTrue(results.Contains("near"));
        Assert.IsFalse(results.Contains("far"));
    }
}
