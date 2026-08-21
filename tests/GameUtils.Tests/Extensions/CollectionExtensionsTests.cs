using System;
using System.Collections.Generic;
using System.Linq;
using GameUtils.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GameUtils.Tests.Extensions;

[TestClass]
public class CollectionExtensionsTests
{
    [TestMethod]
    public void WeightedRandom_IEnumerable_ReturnsElement()
    {
        var elements = Enumerable.Range(1, 10).Select(x => x);
        var result = elements.WeightedRandom(x => x);
        Assert.IsTrue(elements.Contains(result));
    }

    [TestMethod]
    public void WeightedRandom_IEnumerable_Empty_Throws()
    {
        var elements = Enumerable.Empty<int>();
        Assert.ThrowsExactly<InvalidOperationException>(() => elements.WeightedRandom(x => x));
    }

    [TestMethod]
    public void WeightedRandom_IEnumerable_ZeroWeight_Throws()
    {
        var elements = Enumerable.Range(1, 10).Select(x => x);
        Assert.ThrowsExactly<InvalidOperationException>(() => elements.WeightedRandom(x => 0f));
    }

    [TestMethod]
    public void WeightedRandom_IReadOnlyList_ReturnsElement()
    {
        IReadOnlyList<int> elements = new List<int> { 1, 2, 3, 4, 5 };
        var result = elements.WeightedRandom(x => x);
        Assert.IsTrue(elements.Contains(result));
    }

    [TestMethod]
    public void WeightedRandom_IList_ReturnsElement()
    {
        IList<int> elements = new List<int> { 1, 2, 3, 4, 5 };
        var result = elements.WeightedRandom(x => x);
        Assert.IsTrue(elements.Contains(result));
    }

    [TestMethod]
    public void Shuffle_Array_ReturnsShuffledElements()
    {
        var source = new[] { 1, 2, 3, 4, 5 };
        var result = GameUtils.Extensions.CollectionExtensions.Shuffle(source).ToList();
        Assert.AreEqual(5, result.Count);
        CollectionAssert.AreEquivalent(source, result);
    }

    [TestMethod]
    public void Shuffle_IEnumerableNonEnumerated_ReturnsShuffledElements()
    {
        static IEnumerable<int> YieldItems()
        {
            for (int i = 1; i <= 10; i++) yield return i;
        }
        var result = GameUtils.Extensions.CollectionExtensions.Shuffle(YieldItems()).ToList();
        Assert.AreEqual(10, result.Count);
        CollectionAssert.AreEquivalent(Enumerable.Range(1, 10).ToList(), result);
    }

    [TestMethod]
    public void Shuffle_EmptyIEnumerable_ReturnsEmpty()
    {
        static IEnumerable<int> YieldEmpty()
        {
            yield break;
        }
        var result = GameUtils.Extensions.CollectionExtensions.Shuffle(YieldEmpty()).ToList();
        Assert.AreEqual(0, result.Count);
    }

    [TestMethod]
    public void Shuffle_NullSource_ThrowsArgumentNullException()
    {
        IEnumerable<int> source = null!;
        Assert.ThrowsExactly<ArgumentNullException>(() => GameUtils.Extensions.CollectionExtensions.Shuffle(source));
    }
}
