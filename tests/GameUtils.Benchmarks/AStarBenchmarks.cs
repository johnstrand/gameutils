using System.Collections.Generic;
using BenchmarkDotNet.Attributes;
using GameUtils.Entity;

namespace GameUtils.Benchmarks
{
    [MemoryDiagnoser]
    public class AStarBenchmarks
    {
        private AStar<(int x, int y)> _astar = null!;

        [GlobalSetup]
        public void Setup()
        {
            _astar = new AStar<(int x, int y)>();
            int width = 50;
            int height = 50;
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (x + 1 < width)
                    {
                        _astar.AddEdge(new Edge<(int x, int y)>((x, y), (x + 1, y), 1f));
                    }
                    if (y + 1 < height)
                    {
                        _astar.AddEdge(new Edge<(int x, int y)>((x, y), (x, y + 1), 1f));
                    }
                }
            }
        }

        [Benchmark]
        public bool SolveGrid()
        {
            return _astar.Solve((0, 0), (49, 49), (a, b) => System.Math.Abs(a.x - b.x) + System.Math.Abs(a.y - b.y), out _);
        }
    }
}
