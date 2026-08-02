using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using GameUtils.Entity;
using System;
using System.Runtime.InteropServices;

namespace EventBusBenchmark
{
    [MemoryDiagnoser]
    public class EventBusPerf
    {
        private EventBus _bus = new();
        private Action<TestEvent> _handler = e => { };

        [IterationSetup(Target = nameof(Subscribe))]
        public void IterationSetup()
        {
            _bus.Clear();
        }

        [Benchmark]
        public void Subscribe()
        {
            for (int i = 0; i < 1000; i++)
            {
                _bus.Subscribe(_handler);
            }
        }

        [Benchmark]
        public void Unsubscribe()
        {
            for (int i = 0; i < 1000; i++)
            {
                _bus.Unsubscribe(_handler);
            }
        }

        [IterationSetup(Target = nameof(Unsubscribe))]
        public void UnsubSetup()
        {
            _bus.Clear();
            for (int i = 0; i < 1000; i++)
            {
                _bus.Subscribe(_handler);
            }
        }
    }

    public class TestEvent { }

    public class Program
    {
        public static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<EventBusPerf>(null, args);
        }
    }
}
