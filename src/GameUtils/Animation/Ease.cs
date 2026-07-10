namespace GameUtils.Animation;
#pragma warning disable S3358 // Ternary operators should not be nested
#pragma warning disable S1121 // Assignments should not be made from within sub-expressions

/// <summary>
/// Easing functions for interpolation. Based on <see href="https://easings.net/"/>, check them out for visual samples of most of the functions.
/// In each function, as x goes from 0 to 1, the return value goes from 0 to 1.
/// The out functions are flipped versions of the in functions are generally in the form of 1 - f(1 - x). The inOut functions are a combination of the in and out functions.
/// </summary>
public static class Ease
{
    /// <summary>
    /// Wraps any easing function to clamp the input to [0, 1] before evaluating.
    /// </summary>
    public static Func<float, float> Clamp(Func<float, float> easingFunction)
    {
        ArgumentNullException.ThrowIfNull(easingFunction);
        return x => easingFunction(System.Math.Clamp(x, 0f, 1f));
    }

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
        return GameUtils.Math.Easing.SineIn(x);
    }

    /// <summary>
    /// Sine easing out
    /// </summary>
    public static float SineOut(float x)
    {
        return GameUtils.Math.Easing.SineOut(x);
    }

    /// <summary>
    /// Sine easing in/out
    /// </summary>
    public static float SineInOut(float x)
    {
        return GameUtils.Math.Easing.SineInOut(x);
    }
    #endregion

    #region Cubic
    /// <summary>
    /// Cubic easing in
    /// </summary>
    public static float CubicIn(float x)
    {
        return GameUtils.Math.Easing.CubicIn(x);
    }

    /// <summary>
    /// Cubic easing out
    /// </summary>
    public static float CubicOut(float x)
    {
        return GameUtils.Math.Easing.CubicOut(x);
    }

    /// <summary>
    /// Cubic easing in/out
    /// </summary>
    public static float CubicInOut(float x)
    {
        return GameUtils.Math.Easing.CubicInOut(x);
    }
    #endregion

    #region Quint
    /// <summary>
    /// Quintic easing in
    /// </summary>
    public static float QuinticIn(float x)
    {
        return GameUtils.Math.Easing.QuintIn(x);
    }

    /// <summary>
    /// Quintic easing out
    /// </summary>
    public static float QuinticOut(float x)
    {
        return GameUtils.Math.Easing.QuintOut(x);
    }

    /// <summary>
    /// Quintic easing in/out
    /// </summary>
    public static float QuinticInOut(float x)
    {
        return GameUtils.Math.Easing.QuintInOut(x);
    }
    #endregion

    #region Circ
    /// <summary>
    /// Circular easing in
    /// </summary>
    public static float CircularIn(float x)
    {
        return GameUtils.Math.Easing.CircIn(x);
    }

    /// <summary>
    /// Circular easing out
    /// </summary>
    public static float CircularOut(float x)
    {
        return GameUtils.Math.Easing.CircOut(x);
    }

    /// <summary>
    /// Circular easing in/out
    /// </summary>
    public static float CircularInOut(float x)
    {
        return GameUtils.Math.Easing.CircInOut(x);
    }
    #endregion

    #region Elastic
    /// <summary>
    /// Elastic easing in
    /// </summary>
    public static float ElasticIn(float x)
    {
        return GameUtils.Math.Easing.ElasticIn(x);
    }

    /// <summary>
    /// Elastic easing out
    /// </summary>
    public static float ElasticOut(float x)
    {
        return GameUtils.Math.Easing.ElasticOut(x);
    }

    /// <summary>
    /// Elastic easing in/out
    /// </summary>
    public static float ElasticInOut(float x)
    {
        return GameUtils.Math.Easing.ElasticInOut(x);
    }
    #endregion

    #region Quad
    /// <summary>
    /// Quadratic easing in
    /// </summary>
    public static float QuadraticIn(float x)
    {
        return GameUtils.Math.Easing.QuadIn(x);
    }

    /// <summary>
    /// Quadratic easing out
    /// </summary>
    public static float QuadraticOut(float x)
    {
        return GameUtils.Math.Easing.QuadOut(x);
    }

    /// <summary>
    /// Quadratic easing in/out
    /// </summary>
    public static float InOutQuad(float x)
    {
        return GameUtils.Math.Easing.QuadInOut(x);
    }
    #endregion

    #region Quart
    /// <summary>
    /// Quartic easing in
    /// </summary>
    public static float QuarticIn(float x)
    {
        return GameUtils.Math.Easing.QuartIn(x);
    }

    /// <summary>
    /// Quartic easing out
    /// </summary>
    public static float QuarticOut(float x)
    {
        return GameUtils.Math.Easing.QuartOut(x);
    }

    /// <summary>
    /// Quartic easing in/out
    /// </summary>
    public static float QuarticInOut(float x)
    {
        return GameUtils.Math.Easing.QuartInOut(x);
    }
    #endregion

    #region Expo
    /// <summary>
    /// Exponential easing in
    /// </summary>
    public static float ExponentialIn(float x)
    {
        return GameUtils.Math.Easing.ExpoIn(x);
    }

    /// <summary>
    /// Exponential easing out
    /// </summary>
    public static float ExponentialOut(float x)
    {
        return GameUtils.Math.Easing.ExpoOut(x);
    }

    /// <summary>
    /// Exponential easing in/out
    /// </summary>
    public static float ExponentialInOut(float x)
    {
        return GameUtils.Math.Easing.ExpoInOut(x);
    }
    #endregion

    #region Back
    /// <summary>
    /// Easing in back - slightly overshoots, then reverses to reach the target
    /// </summary>
    public static float BackIn(float x)
    {
        return GameUtils.Math.Easing.BackIn(x);
    }

    /// <summary>
    /// Easing out back - slightly overshoots, then reverses to reach the target
    /// </summary>
    public static float BackOut(float x)
    {
        return GameUtils.Math.Easing.BackOut(x);
    }

    /// <summary>
    /// Easing in/out back - slightly overshoots (twice), then reverses to reach the target
    /// </summary>
    public static float BackInOut(float x)
    {
        return GameUtils.Math.Easing.BackInOut(x);
    }
    #endregion

    #region Bounce
    /// <summary>
    /// Easing in bounce - increasing arcs until the target is reached
    /// </summary>
    public static float BounceIn(float x)
    {
        return GameUtils.Math.Easing.BounceIn(x);
    }

    /// <summary>
    /// Easing out bounce - decreasing arcs until the target is reached
    /// </summary>
    public static float BounceOut(float x)
    {
        return GameUtils.Math.Easing.BounceOut(x);
    }

    /// <summary>
    /// Ease in/out bounce - increasing arcs until halfway, then decreasing arcs until the target is reached
    /// </summary>
    public static float BounceInOut(float x)
    {
        return GameUtils.Math.Easing.BounceInOut(x);
    }
    #endregion
}
#pragma warning restore S1121 // Assignments should not be made from within sub-expressions
#pragma warning restore S3358 // Ternary operators should not be nested
