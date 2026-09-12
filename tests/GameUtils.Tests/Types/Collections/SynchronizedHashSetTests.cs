using Microsoft.VisualStudio.TestTools.UnitTesting;
using GameUtils.Types.Collections;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameUtils.Tests.Types.Collections;

[TestClass]
public class SynchronizedHashSetTests
{
    [TestMethod]
    public void Integrate_AddsUniqueElementsOnly()
    {
        var set = new SynchronizedHashSet<int>();

        set.Add(1);
        set.Add(2);
        set.Add(1);

        set.Integrate();

        var items = set.Get().ToList();
        Assert.AreEqual(2, items.Count);
        CollectionAssert.AreEquivalent(new List<int> { 1, 2 }, items);
    }

    [TestMethod]
    public void Integrate_RemovesElementFromHashSet()
    {
        var set = new SynchronizedHashSet<string>();

        set.Add("alpha");
        set.Add("beta");
        set.Integrate();

        Assert.AreEqual(2, set.Get().Count());

        set.Remove("alpha");
        set.Integrate();

        var items = set.Get().ToList();
        Assert.AreEqual(1, items.Count);
        Assert.AreEqual("beta", items[0]);
    }

    [TestMethod]
    public void Integrate_RemoveNonExistentElement_DoesNotThrowOrModifySet()
    {
        var set = new SynchronizedHashSet<int>();

        set.Add(10);
        set.Integrate();

        set.Remove(99);
        set.Integrate();

        var items = set.Get().ToList();
        Assert.AreEqual(1, items.Count);
        Assert.AreEqual(10, items[0]);
    }

    [TestMethod]
    public void Get_CachesSnapshotUntilIntegrateIsCalled()
    {
        var set = new SynchronizedHashSet<int>();

        set.Add(100);
        set.Integrate();

        var snapshot1 = set.Get();
        Assert.AreEqual(1, snapshot1.Count());

        set.Add(200);

        var snapshot2 = set.Get();
        Assert.AreEqual(1, snapshot2.Count());

        set.Integrate();

        var snapshot3 = set.Get();
        Assert.AreEqual(2, snapshot3.Count());
    }

    [TestMethod]
    public void GetEnumerator_EnumeratesSetElements()
    {
        var set = new SynchronizedHashSet<int>();

        set.Add(1);
        set.Add(2);
        set.Add(3);
        set.Integrate();

        var items = new List<int>();
        foreach (var item in set)
        {
            items.Add(item);
        }

        Assert.AreEqual(3, items.Count);
        CollectionAssert.AreEquivalent(new List<int> { 1, 2, 3 }, items);

        IEnumerable nonGenericEnumerable = set;
        var nonGenericItems = new List<object>();
        foreach (var item in nonGenericEnumerable)
        {
            nonGenericItems.Add(item!);
        }

        Assert.AreEqual(3, nonGenericItems.Count);
    }

    [TestMethod]
    public void Concurrency_MultiThreadedAddAndRemove_MaintainsSetUniqueness()
    {
        var set = new SynchronizedHashSet<int>();
        const int itemOperationsCount = 500;

        Parallel.For(0, itemOperationsCount, i =>
        {
            set.Add(i % 50);
        });

        set.Integrate();
        Assert.AreEqual(50, set.Get().Count());

        Parallel.For(0, 25, i =>
        {
            set.Remove(i);
        });

        set.Integrate();
        Assert.AreEqual(25, set.Get().Count());
    }
}
