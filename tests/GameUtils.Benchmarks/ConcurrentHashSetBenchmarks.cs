using BenchmarkDotNet.Attributes;
using GameUtils.Types.Collections;
using System.Collections.Generic;
using System.Linq;

namespace GameUtils.Benchmarks
{
    [MemoryDiagnoser]
    public class ConcurrentHashSetBenchmarks
    {
        private ConcurrentHashSet<int> _set = null!;
        private IEnumerable<int> _enumerable = null!;

        [GlobalSetup]
        public void Setup()
        {
            _set = new ConcurrentHashSet<int>();
            for (int i = 0; i < 1000; i++)
            {
                ((ISet<int>)_set).Add(i);
            }
            _enumerable = Enumerable.Range(0, 500).Select(x => x);
        }

        [Benchmark]
        public bool IsProperSupersetOf()
        {
            return _set.IsProperSupersetOf(_enumerable);
        }

        [Benchmark]
        public bool Overlaps()
        {
            return _set.Overlaps(_enumerable);
        }

        [Benchmark]
        public void IntersectWith()
        {
            var testSet = new ConcurrentHashSet<int>();
            for (int i = 0; i < 1000; i++)
            {
                ((ISet<int>)testSet).Add(i);
            }
            testSet.IntersectWith(_enumerable);
        }
    }
}
