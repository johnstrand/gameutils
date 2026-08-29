using System.IO;
using System.Numerics;
using BenchmarkDotNet.Attributes;
using GameUtils.Types;

namespace GameUtils.Benchmarks
{
    [MemoryDiagnoser]
    public class ImageDataBenchmarks
    {
        private ImageData _imageData = null!;
        private byte[] _serializedBytes = null!;

        [GlobalSetup]
        public void Setup()
        {
            int width = 512;
            int height = 512;
            var data = new Vector4[width * height];
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = new Vector4(i, i * 1.5f, i * 2.5f, i * 3.5f);
            }
            _imageData = new ImageData(width, height, data);

            using var ms = new MemoryStream();
            _imageData.Write(ms);
            _serializedBytes = ms.ToArray();
        }

        [Benchmark]
        public byte[] WriteToStream()
        {
            using var ms = new MemoryStream();
            _imageData.Write(ms);
            return ms.ToArray();
        }

        [Benchmark]
        public ImageData ReadFromStream()
        {
            using var ms = new MemoryStream(_serializedBytes);
            return ImageData.Read(ms);
        }
    }
}
