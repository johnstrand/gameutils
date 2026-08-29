using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GameUtils.Types.Collections;
using GameUtils.Types.Geometry;

namespace GameUtils.Tests.Types.Collections;

[TestClass]
public class SpatialHashTests
{
    [TestMethod]
    public void Constructor_InvalidCellSize_ThrowsArgumentOutOfRangeException()
    {
        Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => new SpatialHash<string>(0f));
        Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => new SpatialHash<string>(-10f));
    }

    [TestMethod]
    public void Constructor_ValidCellSize_SetsCellSizeProperty()
    {
        var hash = new SpatialHash<string>(10f);
        Assert.AreEqual(10f, hash.CellSize);
        Assert.AreEqual(0, hash.CellCount);
    }

    [TestMethod]
    public void Insert_SingleItem_IncreasesCellCountAndIsQueryable()
    {
        var hash = new SpatialHash<string>(10f);
        hash.Insert("item1", new Vector2(5, 5));

        Assert.AreEqual(1, hash.CellCount);
        var queried = hash.Query(new AABB(new Vector2(0, 0), new Vector2(10, 10))).ToList();
        Assert.AreEqual(1, queried.Count);
        Assert.AreEqual("item1", queried[0]);
    }

    [TestMethod]
    public void Insert_MultipleItemsSameCell_CellCountIsOne()
    {
        var hash = new SpatialHash<string>(10f);
        hash.Insert("item1", new Vector2(2, 2));
        hash.Insert("item2", new Vector2(8, 8));

        Assert.AreEqual(1, hash.CellCount);
        var queried = hash.Query(new AABB(new Vector2(0, 0), new Vector2(10, 10))).ToList();
        Assert.AreEqual(2, queried.Count);
        CollectionAssert.AreEquivalent(new[] { "item1", "item2" }, queried);
    }

    [TestMethod]
    public void Insert_NegativeCoordinates_InsertsIntoCorrectCell()
    {
        var hash = new SpatialHash<string>(10f);
        hash.Insert("neg1", new Vector2(-5, -5));
        hash.Insert("pos1", new Vector2(5, 5));

        Assert.AreEqual(2, hash.CellCount);
        var queriedNeg = hash.Query(new AABB(new Vector2(-10, -10), new Vector2(0, 0))).ToList();
        Assert.AreEqual(1, queriedNeg.Count);
        Assert.AreEqual("neg1", queriedNeg[0]);
    }

    [TestMethod]
    public void Remove_ExistingItem_RemovesItemAndDecreasesCellCountWhenCellEmpty()
    {
        var hash = new SpatialHash<string>(10f);
        var pos = new Vector2(5, 5);
        hash.Insert("item1", pos);

        bool removed = hash.Remove("item1", pos);

        Assert.IsTrue(removed);
        Assert.AreEqual(0, hash.CellCount);
        var queried = hash.Query(new AABB(new Vector2(0, 0), new Vector2(10, 10))).ToList();
        Assert.AreEqual(0, queried.Count);
    }

    [TestMethod]
    public void Remove_NonExistentCellOrItem_ReturnsFalse()
    {
        var hash = new SpatialHash<string>(10f);
        hash.Insert("item1", new Vector2(5, 5));

        Assert.IsFalse(hash.Remove("item1", new Vector2(15, 15)));
        Assert.IsFalse(hash.Remove("item2", new Vector2(5, 5)));
        Assert.AreEqual(1, hash.CellCount);
    }

    [TestMethod]
    public void Remove_OneOfMultipleItemsInSameCell_KeepsCellAlive()
    {
        var hash = new SpatialHash<string>(10f);
        var pos1 = new Vector2(2, 2);
        var pos2 = new Vector2(8, 8);
        hash.Insert("item1", pos1);
        hash.Insert("item2", pos2);

        bool removed = hash.Remove("item1", pos1);

        Assert.IsTrue(removed);
        Assert.AreEqual(1, hash.CellCount);
        var queried = hash.Query(new AABB(new Vector2(0, 0), new Vector2(10, 10))).ToList();
        Assert.AreEqual(1, queried.Count);
        Assert.AreEqual("item2", queried[0]);
    }

    [TestMethod]
    public void QueryAABB_MultipleCells_ReturnsOnlyItemsInsideRegion()
    {
        var hash = new SpatialHash<string>(10f);
        hash.Insert("in1", new Vector2(5, 5));
        hash.Insert("in2", new Vector2(15, 15));
        hash.Insert("out1", new Vector2(35, 35));

        var queryRegion = new AABB(new Vector2(0, 0), new Vector2(20, 20));
        var results = hash.Query(queryRegion).ToList();

        Assert.AreEqual(2, results.Count);
        CollectionAssert.AreEquivalent(new[] { "in1", "in2" }, results);
    }

    [TestMethod]
    public void QueryRadius_ReturnsOnlyItemsWithinRadius()
    {
        var hash = new SpatialHash<string>(10f);
        var center = new Vector2(20, 20);
        hash.Insert("near", new Vector2(22, 20));
        hash.Insert("far", new Vector2(50, 50));

        var results = hash.Query(center, 5f).ToList();

        Assert.AreEqual(1, results.Count);
        Assert.AreEqual("near", results[0]);
    }

    [TestMethod]
    public void Clear_RemovesAllCellsAndItems()
    {
        var hash = new SpatialHash<string>(10f);
        hash.Insert("item1", new Vector2(5, 5));
        hash.Insert("item2", new Vector2(25, 25));

        hash.Clear();

        Assert.AreEqual(0, hash.CellCount);
        var results = hash.Query(new AABB(new Vector2(0, 0), new Vector2(100, 100))).ToList();
        Assert.AreEqual(0, results.Count);
    }
}
