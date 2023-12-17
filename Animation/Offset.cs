namespace GameUtils;
public static class Offset
{
    public static float Jagged(float x)
    {
        return x < .25f ? -x : x < .75f ? x - .5f : -x + 1;
    }

    public static float Sine(float x)
    {
        return MathF.Sin(x * MathF.Tau);
    }

    public static float Pulse(float x)
    {
        var t = MathF.Sin(x * MathF.Tau * 3);
        var u = (1 - MathF.Cos(x * MathF.Tau)) / 2;

        return t * u * u;
    }

    public static float Triangle(float x)
    {
        return MathF.Abs(((x + .25f) * 4 % 4) - 2) - 1;
    }

    public static float Wobble(float x)
    {
        float q = (1 - MathF.Cos(x * MathF.Tau)) / 2;
        return (MathF.Cos(x * 20) * q) + (MathF.Sin(x * 400) / 14 * q);
    }
}
