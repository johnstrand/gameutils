using GameUtils.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace GameUtils.Tests.Entity;

[TestClass]
public class EventBusTests
{
    private class TestEventA { }
    private class TestEventB { }

    [TestMethod]
    public void Subscribe_ValidHandler_AddsSubscriber()
    {
        var bus = new EventBus();
        int counter = 0;
        bus.Subscribe<TestEventA>(e => counter++);

        bus.Publish(new TestEventA());

        Assert.AreEqual(1, counter);
    }

    [TestMethod]
    public void Subscribe_NullHandler_ThrowsArgumentNullException()
    {
        var bus = new EventBus();
        Assert.ThrowsExactly<ArgumentNullException>(() => bus.Subscribe<TestEventA>(null!));
    }

    [TestMethod]
    public void Unsubscribe_ValidHandler_RemovesSubscriber()
    {
        var bus = new EventBus();
        int counter = 0;
        Action<TestEventA> handler = e => counter++;

        bus.Subscribe(handler);
        bus.Unsubscribe(handler);

        bus.Publish(new TestEventA());

        Assert.AreEqual(0, counter);
    }

    [TestMethod]
    public void Unsubscribe_NullHandler_ThrowsArgumentNullException()
    {
        var bus = new EventBus();
        Assert.ThrowsExactly<ArgumentNullException>(() => bus.Unsubscribe<TestEventA>(null!));
    }

    [TestMethod]
    public void Publish_NoSubscribers_DoesNothing()
    {
        var bus = new EventBus();
        // Should not throw
        bus.Publish(new TestEventA());
    }

    [TestMethod]
    public void Publish_MultipleSubscribers_InvokesAllInOrder()
    {
        var bus = new EventBus();
        var list = new List<int>();

        bus.Subscribe<TestEventA>(e => list.Add(1));
        bus.Subscribe<TestEventA>(e => list.Add(2));

        bus.Publish(new TestEventA());

        Assert.AreEqual(2, list.Count);
        Assert.AreEqual(1, list[0]);
        Assert.AreEqual(2, list[1]);
    }

    [TestMethod]
    public void Publish_SubscribeInsideHandler_IsSafe()
    {
        var bus = new EventBus();
        int counter1 = 0;
        int counter2 = 0;

        bus.Subscribe<TestEventA>(e =>
        {
            counter1++;
            bus.Subscribe<TestEventA>(e2 => counter2++);
        });

        bus.Publish(new TestEventA());

        // counter1 should be incremented once, counter2 should be 0 because it was subscribed during the enumeration
        Assert.AreEqual(1, counter1);
        Assert.AreEqual(0, counter2);

        // A second publish would invoke both
        bus.Publish(new TestEventA());
        Assert.AreEqual(2, counter1);
        Assert.AreEqual(1, counter2);
    }

    [TestMethod]
    public void Publish_UnsubscribeInsideHandler_IsSafe()
    {
        var bus = new EventBus();
        int counter = 0;
        Action<TestEventA>? handler = null;

        handler = e =>
        {
            counter++;
            bus.Unsubscribe(handler!);
        };

        bus.Subscribe(handler);

        bus.Publish(new TestEventA());
        Assert.AreEqual(1, counter);

        // Second publish shouldn't invoke it because it was unsubscribed
        bus.Publish(new TestEventA());
        Assert.AreEqual(1, counter);
    }

    [TestMethod]
    public void Clear_RemovesAllSubscribers()
    {
        var bus = new EventBus();
        int counterA = 0;
        int counterB = 0;

        bus.Subscribe<TestEventA>(e => counterA++);
        bus.Subscribe<TestEventB>(e => counterB++);

        bus.Clear();

        bus.Publish(new TestEventA());
        bus.Publish(new TestEventB());

        Assert.AreEqual(0, counterA);
        Assert.AreEqual(0, counterB);
    }

    [TestMethod]
    public void ClearTEvent_RemovesSpecificSubscribers()
    {
        var bus = new EventBus();
        int counterA = 0;
        int counterB = 0;

        bus.Subscribe<TestEventA>(e => counterA++);
        bus.Subscribe<TestEventB>(e => counterB++);

        bus.Clear<TestEventA>();

        bus.Publish(new TestEventA());
        bus.Publish(new TestEventB());

        Assert.AreEqual(0, counterA);
        Assert.AreEqual(1, counterB); // B is not cleared
    }
}
