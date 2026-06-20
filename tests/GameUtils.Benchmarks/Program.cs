using BenchmarkDotNet.Running;
using BenchmarkDotNet.Attributes;
using GameUtils.Types.Geometry;
using System.Numerics;
using System.Linq;

namespace GameUtils.Benchmarks
{
    [MemoryDiagnoser]
    public class AABBIntersectsBenchmark
    {
        private AABB _aabb;
        private Polygon2D _polyInside;
        private Polygon2D _polyIntersecting;
        private Polygon2D _polyOutside;
        private Polygon2D _polyContainsAABB;

        [GlobalSetup]
        public void Setup()
        {
            _aabb = new AABB(new Vector2(10, 10), new Vector2(20, 20));

            // Polygon completely inside AABB
            _polyInside = new Polygon2D(new[] {
                new Vector2(12, 12), new Vector2(18, 12), new Vector2(15, 18)
            });

            // Polygon intersecting AABB
            _polyIntersecting = new Polygon2D(new[] {
                new Vector2(5, 15), new Vector2(15, 15), new Vector2(10, 25)
            });

            // Polygon completely outside AABB
            _polyOutside = new Polygon2D(new[] {
                new Vector2(30, 30), new Vector2(40, 30), new Vector2(35, 40)
            });

            // Polygon containing the entire AABB
            _polyContainsAABB = new Polygon2D(new[] {
                new Vector2(0, 0), new Vector2(30, 0), new Vector2(30, 30), new Vector2(0, 30)
            });
        }

        [Benchmark]
        public bool IntersectsInside() => _aabb.Intersects(_polyInside);

        [Benchmark]
        public bool IntersectsIntersecting() => _aabb.Intersects(_polyIntersecting);

        [Benchmark]
        public bool IntersectsOutside() => _aabb.Intersects(_polyOutside);

        [Benchmark]
        public bool IntersectsContainsAABB() => _aabb.Intersects(_polyContainsAABB);
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<AABBIntersectsBenchmark>();
        }
    }
}
