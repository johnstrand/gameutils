using BenchmarkDotNet.Attributes;
using GameUtils.Types.Collections;

namespace GameUtils.Benchmarks
{
    [MemoryDiagnoser]
    public class GridBenchmarks
    {
        private Grid<int> _grid = null!;

        [GlobalSetup]
        public void Setup()
        {
            _grid = new Grid<int>(100, 100);
        }

        [Benchmark]
        public int Get()
        {
            return _grid[50, 50];
        }

        [Benchmark]
        public void Set()
        {
            _grid[50, 50] = 1;
        }

        [Benchmark]
        public Grid<int> Fill()
        {
            return _grid.Fill(1);
        }
    }
}
