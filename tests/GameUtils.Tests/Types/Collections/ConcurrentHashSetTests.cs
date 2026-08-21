using Microsoft.VisualStudio.TestTools.UnitTesting;
using GameUtils.Types.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GameUtils.Tests.Types.Collections;

[TestClass]
public class ConcurrentHashSetTests
{
    [TestMethod]
    public void IntersectWith_NullOther_ThrowsArgumentNullException()
    {
        var set = new ConcurrentHashSet<int>();
        Assert.ThrowsExactly<ArgumentNullException>(() => set.IntersectWith(null!));
    }

    [TestMethod]
    public void IntersectWith_SameInstance_DoesNotModifySet()
    {
        var set = new ConcurrentHashSet<int>();
        ((ISet<int>)set).Add(1);
        ((ISet<int>)set).Add(2);
        set.IntersectWith(set);
        Assert.AreEqual(2, set.Count);
        Assert.IsTrue(set.Contains(1));
        Assert.IsTrue(set.Contains(2));
    }

    [TestMethod]
    public void IntersectWith_WithICollection_IntersectsCorrectly()
    {
        var set = new ConcurrentHashSet<int>();
        ((ISet<int>)set).Add(1);
        ((ISet<int>)set).Add(2);
        ((ISet<int>)set).Add(3);

        var list = new List<int> { 2, 3, 4 };
        set.IntersectWith(list);

        Assert.AreEqual(2, set.Count);
        Assert.IsTrue(set.Contains(2));
        Assert.IsTrue(set.Contains(3));
        Assert.IsFalse(set.Contains(1));
    }

    [TestMethod]
    public void IntersectWith_WithNonCollectionEnumerable_IntersectsCorrectly()
    {
        var set = new ConcurrentHashSet<int>();
        ((ISet<int>)set).Add(1);
        ((ISet<int>)set).Add(2);
        ((ISet<int>)set).Add(3);

        IEnumerable<int> enumerable = Enumerable.Range(2, 3);
        set.IntersectWith(enumerable);

        Assert.AreEqual(2, set.Count);
        Assert.IsTrue(set.Contains(2));
        Assert.IsTrue(set.Contains(3));
        Assert.IsFalse(set.Contains(1));
    }

    [TestMethod]
    public void IntersectWith_WithEmptyEnumerable_ClearsSet()
    {
        var set = new ConcurrentHashSet<int>();
        ((ISet<int>)set).Add(1);
        ((ISet<int>)set).Add(2);

        IEnumerable<int> enumerable = Enumerable.Empty<int>();
        set.IntersectWith(enumerable);

        Assert.AreEqual(0, set.Count);
    }
}
