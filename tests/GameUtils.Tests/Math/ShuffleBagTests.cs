using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GameUtils.Math;

namespace GameUtils.Tests.Math;

[TestClass]
public class ShuffleBagTests
{
    [TestMethod]
    public void Constructor_NullItems_ThrowsArgumentNullException()
    {
        Assert.ThrowsExactly<ArgumentNullException>(() => new ShuffleBag<int>(null!));
    }

    [TestMethod]
    public void Constructor_WeightLessThanOne_ThrowsArgumentOutOfRangeException()
    {
        var items = new List<(int item, int weight)>
        {
            (1, 1),
            (2, 0),
            (3, 2)
        };

        Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => new ShuffleBag<int>(items));
    }

    [TestMethod]
    public void Constructor_EmptyItems_ThrowsArgumentException()
    {
        var items = new List<(int item, int weight)>();

        Assert.ThrowsExactly<ArgumentException>(() => new ShuffleBag<int>(items));
    }

    [TestMethod]
    public void Constructor_ValidItems_InstantiatesSuccessfully()
    {
        var items = new List<(int item, int weight)>
        {
            (1, 1),
            (2, 2)
        };

        var bag = new ShuffleBag<int>(items);

        Assert.IsNotNull(bag);
    }
}
