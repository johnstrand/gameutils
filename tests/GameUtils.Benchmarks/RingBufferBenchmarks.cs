using BenchmarkDotNet.Attributes;
using GameUtils.Types.Collections;

namespace GameUtils.Benchmarks
{
    [MemoryDiagnoser]
    public class RingBufferBenchmarks
    {
        private RingBuffer<int> _buffer = null!;

        [GlobalSetup]
        public void Setup()
        {
            _buffer = new RingBuffer<int>(1000);
            for (int i = 0; i < 1000; i++)
            {
                _buffer.Write(i);
            }
        }

        [Benchmark]
        public void Write()
        {
            _buffer.Write(1);
        }

        [Benchmark]
        public int Enumerate()
        {
            int sum = 0;
            foreach (var item in _buffer)
            {
                sum += item;
            }
            return sum;
        }
    }
}
