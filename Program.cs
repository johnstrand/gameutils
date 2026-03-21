using GameUtils.Types;
using System.Numerics;

namespace GameUtils;

// This file only exists to manually test the library. It is not included in the library itself.
internal static class Program
{
    private static void Main()
    {
        var image = new Bitmap(90, 90);

        image.Clear(Color.PeachPuff);

        for (var i = 0; i < 90; i++)
        {
            image[i, 0] = Vector3.One;
            image[i, 89] = Vector3.One;
            image[0, i] = Vector3.One;
            image[89, i] = Vector3.One;
        }

        image[0, 0] = Vector3.UnitX;

        image.Write("polygon.bmp");
    }
}
