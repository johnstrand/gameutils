using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;

namespace GameUtils.Benchmarks
{
    [MemoryDiagnoser]
    public class CollectionExtensionsBenchmarks
    {
        private int[] _data = null!;

        [Params(10, 100, 1000)]
        public int N;

        [GlobalSetup]
        public void Setup()
        {
            _data = Enumerable.Range(0, N).ToArray();
        }

        private static IEnumerable<T> YieldSequence<T>(IEnumerable<T> source)
        {
            foreach (var item in source)
            {
                yield return item;
            }
        }

        [Benchmark]
        public int Shuffle_UnknownCount()
        {
            int sum = 0;
            foreach (var item in GameUtils.Extensions.CollectionExtensions.Shuffle(YieldSequence(_data)))
            {
                sum += item;
            }
            return sum;
        }

        [Benchmark]
        public int Shuffle_KnownCount()
        {
            int sum = 0;
            foreach (var item in GameUtils.Extensions.CollectionExtensions.Shuffle(_data))
            {
                sum += item;
            }
            return sum;
        }
    }
}
