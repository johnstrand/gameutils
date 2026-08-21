using BenchmarkDotNet.Attributes;
using GameUtils.Types.Collections;

namespace GameUtils.Benchmarks;

[MemoryDiagnoser]
public class SynchronizedCollectionBenchmarks
{
    private SynchronizedHashSet<int> _set = null!;

    [GlobalSetup]
    public void Setup()
    {
        _set = new SynchronizedHashSet<int>();
        for (int i = 0; i < 1000; i++)
        {
            _set.Add(i);
        }
        _set.Integrate();
    }

    [Benchmark]
    public void GetSnapshot()
    {
        _set.Add(1001);
        _set.Integrate();
        _ = _set.Get();
    }
}
