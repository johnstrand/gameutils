namespace GameUtils.Animation;

/// <summary>
/// Easing functions for interpolation. Based on <see href="https://easings.net/"/>, check them out for visual samples of most of the functions.
/// In each function, as x goes from 0 to 1, the return value goes from 0 to 1.
/// The out functions are flipped versions of the in functions are generally in the form of 1 - f(1 - x). The inOut functions are a combination of the in and out functions.
/// </summary>
public static class Ease
{
    #region Linear
    /// <summary>
    /// Linear interpolation (no easing). This method is included for completeness.
    /// </summary>
    public static float Linear(float x)
    {
        return x;
    }
    #endregion

    #region Quntic bounce
    /// <summary>
    /// Quintic easing in (with bounce)
    /// </summary>
    public static float QuinticBounceIn(float x)
    {
        float t = (1 - x) * (MathF.Cos(x) - 1) * 5;
        float u = x * x * x * x * x;

        return t + u;
    }

    /// <summary>
    /// Quintic easing out (with bounce)
    /// </summary>
    public static float QuinticBounceOut(float x)
    {
        return 1 - QuinticBounceIn(1 - x);
    }

    /// <summary>
    /// Quintic easing in/out (with bounce)
    /// </summary>
    public static float QuinticBounceInOut(float x)
    {
        return (QuinticBounceIn(x) * x * x * x) + (QuinticBounceOut(x) * (1 - (x * x * x)));
    }
    #endregion

    #region Sine
    /// <summary>
    /// Sine easing in
    /// </summary>
    public static float SineIn(float x)
    {
        return 1 - MathF.Cos(x * MathF.PI / 2);
    }

    /// <summary>
    /// Sine easing out
    /// </summary>
    public static float SineOut(float x)
    {
        return 1 - SineIn(1 - x);
    }

    /// <summary>
    /// Sine easing in/out
    /// </summary>
    public static float SineInOut(float x)
    {
        return -(MathF.Cos(MathF.PI * x) - 1) / 2;
    }
    #endregion

    #region Cubic
    /// <summary>
    /// Cubic easing in
    /// </summary>
    public static float CubicIn(float x)
    {
        return x * x * x;
    }

    /// <summary>
    /// Cubic easing out
    /// </summary>
    public static float CubicOut(float x)
    {
        return 1 - CubicIn(1 - x);
    }

    /// <summary>
    /// Cubic easing in/out
    /// </summary>
    public static float CubicInOut(float x)
    {
        return x < .5f ? 4 * x * x * x : 1 - (MathF.Pow((-2 * x) + 2, 3) / 2);
    }
    #endregion

    #region Quint
    /// <summary>
    /// Quintic easing in
    /// </summary>
    public static float QuinticIn(float x)
    {
        return x * x * x * x * x;
    }

    /// <summary>
    /// Quintic easing out
    /// </summary>
    public static float QuinticOut(float x)
    {
        return 1 - QuinticIn(1 - x);
    }

    /// <summary>
    /// Quintic easing in/out
    /// </summary>
    public static float QuinticInOut(float x)
    {
        return x < 0.5f ? 16 * x * x * x * x * x : 1 - (MathF.Pow((-2 * x) + 2, 5) / 2);
    }
    #endregion

    #region Circ
    /// <summary>
    /// Circular easing in
    /// </summary>
    public static float CircularIn(float x)
    {
        return 1 - MathF.Sqrt(1 - MathF.Pow(x, 2));
    }

    /// <summary>
    /// Circular easing out
    /// </summary>
    public static float CircularOut(float x)
    {
        return 1 - CircularIn(1 - x);
    }

    /// <summary>
    /// Circular easing in/out
    /// </summary>
    public static float CircularInOut(float x)
    {
        return x < 0.5f
      ? (1 - MathF.Sqrt(1 - MathF.Pow(2 * x, 2))) / 2
      : (MathF.Sqrt(1 - MathF.Pow((-2 * x) + 2, 2)) + 1) / 2;
    }
    #endregion

    #region Elastic
    /// <summary>
    /// Elastic easing in
    /// </summary>
    public static float ElasticIn(float x)
    {
        const float C_4 = 2 * MathF.PI / 3;

        return x == 0
          ? 0
          : x == 1
          ? 1
          : -MathF.Pow(2, (10 * x) - 10) * MathF.Sin(((x * 10) - 10.75f) * C_4);
    }

    /// <summary>
    /// Elastic easing out
    /// </summary>
    public static float ElasticOut(float x)
    {
        return 1 - ElasticIn(1 - x);
    }

    /// <summary>
    /// Elastic easing in/out
    /// </summary>
    public static float ElasticInOut(float x)
    {
        const float C_5 = MathF.PI * 4f / 9f;

        return x == 0
          ? 0
          : x == 1
          ? 1
          : x < 0.5f
          ? -(MathF.Pow(2, (20 * x) - 10) * MathF.Sin(((20f * x) - 11.125f) * C_5)) / 2
          : (MathF.Pow(2, (-20 * x) + 10) * MathF.Sin(((20f * x) - 11.125f) * C_5) / 2) + 1;
    }
    #endregion

    #region Quad
    /// <summary>
    /// Quadratic easing in
    /// </summary>
    public static float QuadraticIn(float x)
    {
        return x * x;
    }

    /// <summary>
    /// Quadratic easing out
    /// </summary>
    public static float QuadraticOut(float x)
    {
        return 1 - QuadraticIn(1 - x);
    }

    /// <summary>
    /// Quadratic easing in/out
    /// </summary>
    public static float InOutQuad(float x)
    {
        return x < 0.5f ? 2 * x * x : 1 - (MathF.Pow((-2 * x) + 2, 2) / 2);
    }
    #endregion

    #region Quart
    /// <summary>
    /// Quartic easing in
    /// </summary>
    public static float QuarticIn(float x)
    {
        return x * x * x * x;
    }

    /// <summary>
    /// Quartic easing out
    /// </summary>
    public static float QuarticOut(float x)
    {
        return 1 - QuarticIn(1 - x);
    }

    /// <summary>
    /// Quartic easing in/out
    /// </summary>
    public static float QuarticInOut(float x)
    {
        return x < 0.5f ? 8 * x * x * x * x : 1 - (MathF.Pow((-2 * x) + 2, 4) / 2);
    }
    #endregion

    #region Expo
    /// <summary>
    /// Exponential easing in
    /// </summary>
    public static float ExponentialIn(float x)
    {
        return x == 0 ? 0 : MathF.Pow(2, (10 * x) - 10);
    }

    /// <summary>
    /// Exponential easing out
    /// </summary>
    public static float ExponentialOut(float x)
    {
        return 1 - ExponentialIn(1 - x);
    }

    /// <summary>
    /// Exponential easing in/out
    /// </summary>
    public static float ExponentialInOut(float x)
    {
        return x == 0
      ? 0
      : x == 1
      ? 1
      : x < 0.5f ? MathF.Pow(2, (20 * x) - 10) / 2
      : (2 - MathF.Pow(2, (-20 * x) + 10)) / 2;
    }
    #endregion

    #region Back
    /// <summary>
    /// Easing in back - slightly overshoots, then reverses to reach the target
    /// </summary>
    public static float BackIn(float x)
    {
        const float C_1 = 1.70158f;
        const float C_3 = C_1 + 1;

        return (C_3 * x * x * x) - (C_1 * x * x);
    }

    /// <summary>
    /// Easing out back - slightly overshoots, then reverses to reach the target
    /// </summary>
    public static float BackOut(float x)
    {
        return 1 - BackIn(1 - x);
    }

    /// <summary>
    /// Easing in/out back - slightly overshoots (twice), then reverses to reach the target
    /// </summary>
    public static float BackInOut(float x)
    {
        const float C_1 = 1.70158f;
        const float C_2 = C_1 * 1.525f;

        return x < 0.5f
          ? MathF.Pow(2 * x, 2) * (((C_2 + 1) * 2 * x) - C_2) / 2
          : ((MathF.Pow((2 * x) - 2, 2) * (((C_2 + 1) * ((x * 2) - 2)) + C_2)) + 2) / 2;
    }
    #endregion

    #region Bounce
    /// <summary>
    /// Easing in bounce - increasing arcs until the target is reached
    /// </summary>
    public static float BounceIn(float x)
    {
        return 1 - BounceOut(1 - x);
    }

    /// <summary>
    /// Easing out bounce - decreasing arcs until the target is reached
    /// </summary>
    public static float BounceOut(float x)
    {
        const float N_1 = 7.5625f;
        const float D_1 = 2.75f;

        if (x < 1 / D_1)
        {
            return N_1 * x * x;
        }
        else if (x < 2 / D_1)
        {
            return (N_1 * (x -= 1.5f / D_1) * x) + 0.75f;
        }
        else if (x < 2.5f / D_1)
        {
            return (N_1 * (x -= 2.25f / D_1) * x) + 0.9375f;
        }
        else
        {
            return (N_1 * (x -= 2.625f / D_1) * x) + 0.984375f;
        }
    }

    /// <summary>
    /// Ease in/out bounce - increasing arcs until halfway, then decreasing arcs until the target is reached
    /// </summary>
    public static float BounceInOut(float x)
    {
        return x < 0.5f
          ? (1 - BounceOut(1 - (2 * x))) / 2
          : (1 + BounceOut((2 * x) - 1)) / 2;
    }
    #endregion
}
