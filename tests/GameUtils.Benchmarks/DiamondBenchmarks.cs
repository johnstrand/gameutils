using BenchmarkDotNet.Attributes;
using GameUtils.Procedural;
using GameUtils.Types.Collections;

namespace GameUtils.Benchmarks
{
    [MemoryDiagnoser]
    public class DiamondBenchmarks
    {
        [Benchmark]
        public Grid<int> Create()
        {
            return Diamond.Create(129, 0, 100, 10f, r => r * 0.5f, (v, max) => (int)v);
        }
    }
}
