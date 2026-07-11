using BenchmarkDotNet.Attributes;
using GameUtils.Math;
using System.Numerics;

namespace GameUtils.Benchmarks
{
    [MemoryDiagnoser]
    public class CatmullRomBenchmarks
    {
        private Vector2 _p0 = Vector2.Zero;
        private Vector2 _p1 = new Vector2(10, 10);
        private Vector2 _p2 = new Vector2(20, 0);
        private Vector2 _p3 = new Vector2(30, 10);
        private float _t = 0.5f;

        [Benchmark]
        public Vector2 Sample()
        {
            return CatmullRom.Sample(_p0, _p1, _p2, _p3, _t);
        }
    }
}
