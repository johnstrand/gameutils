namespace GameUtils.Animation;

/// <summary>
/// Easing functions for interpolation. Based on https://easings.net/
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
    public static float QuinticBounceIn(float x)
    {
        float t = (1 - x) * (MathF.Cos(x) - 1) * 5;
        float u = x * x * x * x * x;

        return t + u;
    }

    public static float QuinticBounceOut(float x)
    {
        return 1 - QuinticBounceIn(1 - x);
    }

    public static float QuinticBounceInOut(float x)
    {
        return QuinticBounceIn(x) * x * x * x + QuinticBounceOut(x) * (1 - x * x * x);
    }
    #endregion

    #region Sine
    public static float SineIn(float x)
    {
        return 1 - MathF.Cos(x * MathF.PI / 2);
    }

    public static float SineOut(float x)
    {
        return 1 - SineIn(1 - x);
    }

    public static float SineInOut(float x)
    {
        return -(MathF.Cos(MathF.PI * x) - 1) / 2;
    }
    #endregion

    #region Cubic
    public static float CubicIn(float x)
    {
        return x * x * x;
    }

    public static float CubicOut(float x)
    {
        return 1 - CubicIn(1 - x);
    }

    public static float CubicInOut(float x)
    {
        return x < .5f ? 4 * x * x * x : 1 - MathF.Pow(-2 * x + 2, 3) / 2;
    }
    #endregion

    #region Quint
    public static float QuinticIn(float x)
    {
        return x * x * x * x * x;
    }

    public static float QuinticOut(float x)
    {
        return 1 - QuinticIn(1 - x);
    }

    public static float QuinticInOut(float x)
    {
        return x < 0.5f ? 16 * x * x * x * x * x : 1 - MathF.Pow(-2 * x + 2, 5) / 2;
    }
    #endregion

    #region Circ
    public static float CircularIn(float x)
    {
        return 1 - MathF.Sqrt(1 - MathF.Pow(x, 2));
    }

    public static float CircularOut(float x)
    {
        return 1 - CircularIn(1 - x);
    }

    public static float CircularInOut(float x)
    {
        return x < 0.5f
      ? (1 - MathF.Sqrt(1 - MathF.Pow(2 * x, 2))) / 2
      : (MathF.Sqrt(1 - MathF.Pow(-2 * x + 2, 2)) + 1) / 2;
    }
    #endregion

    #region Elastic
    public static float ElasticIn(float x)
    {
        const float c4 = 2 * MathF.PI / 3;

        return x == 0
          ? 0
          : x == 1
          ? 1
          : -MathF.Pow(2, 10 * x - 10) * MathF.Sin((x * 10 - 10.75f) * c4);
    }

    public static float ElasticOut(float x)
    {
        return 1 - ElasticIn(1 - x);
    }

    public static float ElasticInOut(float x)
    {
        const float c5 = MathF.PI * 4f / 9f;

        return x == 0
          ? 0
          : x == 1
          ? 1
          : x < 0.5f
          ? -(MathF.Pow(2, 20 * x - 10) * MathF.Sin((20f * x - 11.125f) * c5)) / 2
          : MathF.Pow(2, -20 * x + 10) * MathF.Sin((20f * x - 11.125f) * c5) / 2 + 1;
    }
    #endregion

    #region Quad
    public static float QuadraticIn(float x)
    {
        return x * x;
    }

    public static float QuadraticOut(float x)
    {
        return 1 - QuadraticIn(1 - x);
    }

    public static float InOutQuad(float x)
    {
        return x < 0.5f ? 2 * x * x : 1 - MathF.Pow(-2 * x + 2, 2) / 2;
    }
    #endregion

    #region Quart
    public static float QuarticIn(float x)
    {
        return x * x * x * x;
    }

    public static float QuarticOut(float x)
    {
        return 1 - QuarticIn(1 - x);
    }

    public static float QuarticInOut(float x)
    {
        return x < 0.5f ? 8 * x * x * x * x : 1 - MathF.Pow(-2 * x + 2, 4) / 2;
    }
    #endregion

    #region Expo
    public static float ExponentialIn(float x)
    {
        return x == 0 ? 0 : MathF.Pow(2, 10 * x - 10);
    }

    public static float ExponentialOut(float x)
    {
        return 1 - ExponentialIn(1 - x);
    }

    public static float ExponentialInOut(float x)
    {
        return x == 0
      ? 0
      : x == 1
      ? 1
      : x < 0.5f ? MathF.Pow(2, 20 * x - 10) / 2
      : (2 - MathF.Pow(2, -20 * x + 10)) / 2;
    }
    #endregion

    #region Back
    public static float BackIn(float x)
    {
        const float c1 = 1.70158f;
        const float c3 = c1 + 1;

        return c3 * x * x * x - c1 * x * x;
    }

    public static float BackOut(float x)
    {
        return 1 - BackIn(1 - x);
    }

    public static float BackInOut(float x)
    {
        const float c1 = 1.70158f;
        const float c2 = c1 * 1.525f;

        return x < 0.5f
          ? MathF.Pow(2 * x, 2) * ((c2 + 1) * 2 * x - c2) / 2
          : (MathF.Pow(2 * x - 2, 2) * ((c2 + 1) * (x * 2 - 2) + c2) + 2) / 2;
    }
    #endregion

    #region Bounce
    public static float BounceIn(float x)
    {
        return 1 - BounceOut(1 - x);
    }

    public static float BounceOut(float x)
    {
        const float n1 = 7.5625f;
        const float d1 = 2.75f;

        if (x < 1 / d1)
        {
            return n1 * x * x;
        }
        else if (x < 2 / d1)
        {
            return n1 * (x -= 1.5f / d1) * x + 0.75f;
        }
        else if (x < 2.5f / d1)
        {
            return n1 * (x -= 2.25f / d1) * x + 0.9375f;
        }
        else
        {
            return n1 * (x -= 2.625f / d1) * x + 0.984375f;
        }
    }

    public static float BounceInOut(float x)
    {
        return x < 0.5f
          ? (1 - BounceOut(1 - 2 * x)) / 2
          : (1 + BounceOut(2 * x - 1)) / 2;
    }
    #endregion
}
