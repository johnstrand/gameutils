using BenchmarkDotNet.Attributes;
using GameUtils.Entity;
using System;

namespace GameUtils.Benchmarks
{
    [MemoryDiagnoser]
    public class EventBusBenchmarks
    {
        private class TestEvent1 { }
        private class TestEvent2 { }

        private Action<TestEvent1> _handler1 = null!;
        private Action<TestEvent2> _handler2 = null!;

        [GlobalSetup]
        public void Setup()
        {
            _handler1 = e => { };
            _handler2 = e => { };
        }

        [Benchmark]
        public void SubscribeNewEvent()
        {
            var bus = new EventBus();
            bus.Subscribe(_handler1);
            bus.Subscribe(_handler2);
        }

        [Benchmark]
        public void SubscribeExistingEvent()
        {
            var bus = new EventBus();
            bus.Subscribe(_handler1);
            for (int i = 0; i < 100; i++)
            {
                bus.Subscribe(_handler1);
            }
        }
    }
}
