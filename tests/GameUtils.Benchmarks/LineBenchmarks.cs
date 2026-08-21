using BenchmarkDotNet.Attributes;
using GameUtils.Types.Geometry;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace GameUtils.Benchmarks
{
    [MemoryDiagnoser]
    public class LineBenchmarks
    {
        private Line _line;
        private List<Line> _rays = null!;

        [GlobalSetup]
        public void Setup()
        {
            _line = new Line(new Vector2(0, 0), new Vector2(100, 0));
            _rays = new List<Line>();
            var random = new Random(42);
            for (int i = 0; i < 100; i++)
            {
                float x = (float)random.NextDouble() * 100f;
                _rays.Add(new Line(new Vector2(x, -50), new Vector2(x, 50)));
            }
        }

        [Benchmark]
        public bool IntersectsAny()
        {
            return _line.IntersectsAny(_rays, out _);
        }
    }
}
