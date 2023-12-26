using System.IO.Compression;
using System.Numerics;
using GameUtils.Types;
using GameUtils.Types.Geometry;

namespace GameUtils;

// This file only exists to manually test the library. It is not included in the library itself.
internal static class Program
{
    private static void Main()
    {
        var r = new Random();
        var vertices = new List<Vector2>();
        var image = new Bitmap(90, 90);
        for(var i = 0; i < 90 ;i++)
        {
            image[i, 0] = Vector3.One;
            image[i, 89] = Vector3.One;
            image[0, i] = Vector3.One;
            image[89, i] = Vector3.One;
            image[i, i] = Vector3.UnitX;
        }
        /*
        for (var y = 0; y < 3; y++)
        {
            for (var x = 0; x < 3; x++)
            {
                if (x == 1 && y == 1)
                {
                    continue;
                }
                var yp = y * 30 + r.Next(30);
                var xp = x * 30 + r.Next(30);
                vertices.Add(new Vector2(xp, yp));
                image[xp, yp] = Vector3.One;
            }
        }
        */
        var polygon = new Polygon2D([.. vertices]);
        /*
        for(var i = 0; i < polygon.Edges.Length; i++)
        {
            var edge = polygon.Edges[i];
            var normal = polygon.Normals[i];
            var start = edge.Start;
            var end = edge.End;
            foreach (var point in Steps(start, end, 100))
            {
                image[point] = Vector3.One;
            }
        }
        */

        image.Write("polygon.bmp");
    }

    static IEnumerable<Vector2> Steps(Vector2 start, Vector2 end, int steps)
    {
        var step = (end - start) / steps;
        for (var i = 0; i < steps; i++)
        {
            yield return start + step * i;
        }
    }
}
