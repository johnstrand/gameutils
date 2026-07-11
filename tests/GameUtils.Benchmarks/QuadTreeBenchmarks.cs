using BenchmarkDotNet.Attributes;
using GameUtils.Types.Collections;
using GameUtils.Types.Geometry;
using System;
using System.Linq;
using System.Numerics;

namespace GameUtils.Benchmarks
{
    [MemoryDiagnoser]
    public class QuadTreeBenchmarks
    {
        private QuadTree<int> _tree = null!;
        private AABB _queryAABB;
        private Vector2 _queryCircleCenter;
        private float _queryCircleRadius;

        [GlobalSetup]
        public void Setup()
        {
            _tree = new QuadTree<int>(new AABB(Vector2.Zero, new Vector2(1000, 1000)), 16);
            var random = new Random(42);
            for (int i = 0; i < 1000; i++)
            {
                var x = (float)random.NextDouble() * 1000f;
                var y = (float)random.NextDouble() * 1000f;
                _tree.Insert(i, new Vector2(x, y));
            }

            _queryAABB = new AABB(new Vector2(500, 500), new Vector2(100, 100));
            _queryCircleCenter = new Vector2(500, 500);
            _queryCircleRadius = 100f;
        }

        [IterationCleanup(Target = nameof(Insert))]
        public void CleanupInsert()
        {
            _tree.Remove(0, new Vector2(500, 500));
        }

        [Benchmark]
        public bool Insert()
        {
            return _tree.Insert(0, new Vector2(500, 500));
        }

        [Benchmark]
        public int QueryAABB()
        {
            return _tree.Query(_queryAABB).Count();
        }

        [Benchmark]
        public int QueryCircle()
        {
            return _tree.Query(_queryCircleCenter, _queryCircleRadius).Count();
        }
    }
}
