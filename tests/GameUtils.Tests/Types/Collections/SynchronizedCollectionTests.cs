using Microsoft.VisualStudio.TestTools.UnitTesting;
using GameUtils.Types.Collections;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameUtils.Tests.Types.Collections;

[TestClass]
public class SynchronizedCollectionTests
{
    private class TestSynchronizedCollection<T> : SynchronizedCollection<T> where T : notnull
    {
        private readonly List<T> _items = new();

        protected override IEnumerable<T> GetInternal()
        {
            return _items;
        }

        protected override void HandleOperation(Operation<T> operation)
        {
            if (operation.Kind == OperationKind.Add)
            {
                _items.Add(operation.Entity);
            }
            else if (operation.Kind == OperationKind.Remove)
            {
                _items.Remove(operation.Entity);
            }
        }
    }

    [TestMethod]
    public void Add_ReturnsScheduledEntity()
    {
        var collection = new TestSynchronizedCollection<string>();

        var result = collection.Add("item1");

        Assert.AreEqual("item1", result);
    }

    [TestMethod]
    public void Integrate_WhenPendingIsEmpty_DoesNotChangeCollection()
    {
        var collection = new TestSynchronizedCollection<int>();

        collection.Integrate();

        Assert.AreEqual(0, collection.Count());
    }

    [TestMethod]
    public void Integrate_ProcessesPendingAddAndRemoveOperations()
    {
        var collection = new TestSynchronizedCollection<int>();

        collection.Add(1);
        collection.Add(2);
        collection.Add(3);

        collection.Integrate();

        var snapshot1 = collection.Get().ToList();
        CollectionAssert.AreEqual(new List<int> { 1, 2, 3 }, snapshot1);

        collection.Remove(2);
        collection.Integrate();

        var snapshot2 = collection.Get().ToList();
        CollectionAssert.AreEqual(new List<int> { 1, 3 }, snapshot2);
    }

    [TestMethod]
    public void Get_CachesSnapshotUntilIntegrated()
    {
        var collection = new TestSynchronizedCollection<int>();

        collection.Add(10);
        collection.Integrate();

        var snapshotInitial = collection.Get();
        Assert.AreEqual(1, snapshotInitial.Count());

        collection.Add(20);

        // Before Integrate(), Get() returns cached snapshot from previous integrate
        var snapshotUnintegrated = collection.Get();
        Assert.AreEqual(1, snapshotUnintegrated.Count());

        collection.Integrate();

        // After Integrate(), Get() returns updated items
        var snapshotAfterIntegrate = collection.Get();
        Assert.AreEqual(2, snapshotAfterIntegrate.Count());
    }

    [TestMethod]
    public void ClearPending_DiscardsScheduledOperations()
    {
        var collection = new TestSynchronizedCollection<int>();

        collection.Add(100);
        collection.Add(200);

        collection.ClearPending();
        collection.Integrate();

        Assert.AreEqual(0, collection.Get().Count());
    }

    [TestMethod]
    public void WaitForIntegration_SucceedsWhenNoActiveIntegration()
    {
        var collection = new TestSynchronizedCollection<int>();

        collection.Add(5);
        collection.WaitForIntegration();

        // Pending operations are still pending because Integrate wasn't called
        collection.Integrate();
        Assert.AreEqual(1, collection.Get().Count());
    }

    [TestMethod]
    public void GetEnumerator_AllowsIterationAndExplicitIEnumerableCall()
    {
        var collection = new TestSynchronizedCollection<int>();
        collection.Add(1);
        collection.Add(2);
        collection.Integrate();

        var items = new List<int>();
        foreach (var item in collection)
        {
            items.Add(item);
        }

        CollectionAssert.AreEqual(new List<int> { 1, 2 }, items);

        IEnumerable nonGenericCollection = collection;
        var nonGenericItems = new List<object>();
        var enumerator = nonGenericCollection.GetEnumerator();
        while (enumerator.MoveNext())
        {
            nonGenericItems.Add(enumerator.Current!);
        }

        CollectionAssert.AreEqual(new List<object> { 1, 2 }, nonGenericItems);
    }

    [TestMethod]
    public void Concurrency_MultiThreadedAddAndRemove_IntegratesSafely()
    {
        var collection = new TestSynchronizedCollection<int>();
        const int itemOperationsCount = 500;

        Parallel.For(0, itemOperationsCount, i =>
        {
            collection.Add(i);
        });

        collection.Integrate();
        Assert.AreEqual(itemOperationsCount, collection.Get().Count());

        Parallel.For(0, itemOperationsCount / 2, i =>
        {
            collection.Remove(i);
        });

        collection.Integrate();
        Assert.AreEqual(itemOperationsCount - (itemOperationsCount / 2), collection.Get().Count());
    }
}
