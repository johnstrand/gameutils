namespace GameUtils;

public static class MathExt
{
    private static readonly Random _random = new();

    public static float RandomFloat()
    {
        return _random.NextSingle();
    }

    public static float RandomFloat(float min, float max)
    {
        return min + ((max - min) * RandomFloat());
    }
}