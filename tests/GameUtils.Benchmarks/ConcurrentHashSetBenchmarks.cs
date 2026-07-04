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
            // Create an IEnumerable that is NOT an ICollection
            _enumerable = Enumerable.Range(0, 500).Select(x => x);
        }

        [Benchmark]
        public bool IsProperSupersetOf()
        {
            return _set.IsProperSupersetOf(_enumerable);
        }
    }
}
