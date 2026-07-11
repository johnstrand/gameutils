using BenchmarkDotNet.Attributes;
using GameUtils.Procedural;

namespace GameUtils.Benchmarks
{
    [MemoryDiagnoser]
    public class PerlinNoiseBenchmarks
    {
        [Benchmark]
        public float Sample2D()
        {
            return PerlinNoise.Default.Sample(1.5f, 2.5f);
        }

        [Benchmark]
        public float Sample3D()
        {
            return PerlinNoise.Default.Sample(1.5f, 2.5f, 3.5f);
        }

        [Benchmark]
        public float Fbm()
        {
            return PerlinNoise.Default.Fbm(1.5f, 2.5f, 4);
        }
    }
}
