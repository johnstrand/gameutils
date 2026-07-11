namespace GameUtils.Math;

#pragma warning disable S1244 // t == 0f / t == 1f are intentional boundary guards, not precision comparisons

/// <summary>
/// Standard easing functions for animation and interpolation.
/// All functions accept a normalized time <c>t</c> in [0, 1] and return a value in approximately [0, 1].
/// </summary>
public static class Easing
{
    // ── Sine ─────────────────────────────────────────────────────────────────

    /// <summary>Ease-in using a sine curve.</summary>
    public static float SineIn(float t) => 1f - MathF.Cos(t * MathF.PI / 2f);

    /// <summary>Ease-out using a sine curve.</summary>
    public static float SineOut(float t) => MathF.Sin(t * MathF.PI / 2f);

    /// <summary>Ease-in-out using a sine curve.</summary>
    public static float SineInOut(float t) => -(MathF.Cos(MathF.PI * t) - 1f) / 2f;

    // ── Quadratic ────────────────────────────────────────────────────────────

    /// <summary>Ease-in using a quadratic (t²) curve.</summary>
    public static float QuadIn(float t) => t * t;

    /// <summary>Ease-out using a quadratic curve.</summary>
    public static float QuadOut(float t) => 1f - (1f - t) * (1f - t);

    /// <summary>Ease-in-out using a quadratic curve.</summary>
    public static float QuadInOut(float t) => t < 0.5f ? 2f * t * t : 1f - ((-2f * t + 2f) * (-2f * t + 2f) / 2f);

    // ── Cubic ────────────────────────────────────────────────────────────────

    /// <summary>Ease-in using a cubic (t³) curve.</summary>
    public static float CubicIn(float t) => t * t * t;

    /// <summary>Ease-out using a cubic curve.</summary>
    public static float CubicOut(float t) { var u = 1f - t; return 1f - u * u * u; }

    /// <summary>Ease-in-out using a cubic curve.</summary>
    public static float CubicInOut(float t) => t < 0.5f ? 4f * t * t * t : 1f - ((-2f * t + 2f) * (-2f * t + 2f) * (-2f * t + 2f) / 2f);

    // ── Quartic ──────────────────────────────────────────────────────────────

    /// <summary>Ease-in using a quartic (t⁴) curve.</summary>
    public static float QuartIn(float t) => t * t * t * t;

    /// <summary>Ease-out using a quartic curve.</summary>
    public static float QuartOut(float t) { var u = 1f - t; return 1f - u * u * u * u; }

    /// <summary>Ease-in-out using a quartic curve.</summary>
    public static float QuartInOut(float t)
    {
        var u = -2f * t + 2f;
        return t < 0.5f ? 8f * t * t * t * t : 1f - (u * u * u * u / 2f);
    }

    // ── Quintic ──────────────────────────────────────────────────────────────

    /// <summary>Ease-in using a quintic (t⁵) curve.</summary>
    public static float QuintIn(float t) => t * t * t * t * t;

    /// <summary>Ease-out using a quintic curve.</summary>
    public static float QuintOut(float t) { var u = 1f - t; return 1f - u * u * u * u * u; }

    /// <summary>Ease-in-out using a quintic curve.</summary>
    public static float QuintInOut(float t)
    {
        var u = -2f * t + 2f;
        return t < 0.5f ? 16f * t * t * t * t * t : 1f - (u * u * u * u * u / 2f);
    }

    // ── Exponential ──────────────────────────────────────────────────────────

    /// <summary>Ease-in using an exponential curve. Returns 0 when <paramref name="t"/> is 0.</summary>
    public static float ExpoIn(float t) => t == 0f ? 0f : MathF.Pow(2f, (10f * t) - 10f);

    /// <summary>Ease-out using an exponential curve. Returns 1 when <paramref name="t"/> is 1.</summary>
    public static float ExpoOut(float t) => t == 1f ? 1f : 1f - MathF.Pow(2f, -10f * t);

    /// <summary>Ease-in-out using an exponential curve.</summary>
    public static float ExpoInOut(float t)
    {
        if (t == 0f) return 0f;
        if (t == 1f) return 1f;
        return t < 0.5f
            ? MathF.Pow(2f, (20f * t) - 10f) / 2f
            : (2f - MathF.Pow(2f, (-20f * t) + 10f)) / 2f;
    }

    // ── Circular ─────────────────────────────────────────────────────────────

    /// <summary>Ease-in using a circular curve.</summary>
    public static float CircIn(float t) => 1f - MathF.Sqrt(1f - (t * t));

    /// <summary>Ease-out using a circular curve.</summary>
    public static float CircOut(float t) => MathF.Sqrt(1f - ((t - 1f) * (t - 1f)));

    /// <summary>Ease-in-out using a circular curve.</summary>
    public static float CircInOut(float t)
    {
        return t < 0.5f
            ? (1f - MathF.Sqrt(1f - (2f * t) * (2f * t))) / 2f
            : (MathF.Sqrt(1f - (-2f * t + 2f) * (-2f * t + 2f)) + 1f) / 2f;
    }

    // ── Back ─────────────────────────────────────────────────────────────────

    private const float BackC1 = 1.70158f;
    private const float BackC2 = BackC1 * 1.525f;
    private const float BackC3 = BackC1 + 1f;

    /// <summary>Ease-in with a slight overshoot backwards before moving forward.</summary>
    public static float BackIn(float t) => BackC3 * t * t * t - BackC1 * t * t;

    /// <summary>Ease-out with a slight overshoot past the target before settling.</summary>
    public static float BackOut(float t) { var u = t - 1f; return 1f + BackC3 * u * u * u + BackC1 * u * u; }

    /// <summary>Ease-in-out with overshoot on both ends.</summary>
    public static float BackInOut(float t)
    {
        return t < 0.5f
            ? ((2f * t) * (2f * t) * ((BackC2 + 1f) * 2f * t - BackC2)) / 2f
            : ((2f * t - 2f) * (2f * t - 2f) * ((BackC2 + 1f) * (2f * t - 2f) + BackC2) + 2f) / 2f;
    }

    // ── Elastic ──────────────────────────────────────────────────────────────

    private const float ElasticC4 = MathF.Tau / 3f;
    private const float ElasticC5 = MathF.Tau / 4.5f;

    /// <summary>Ease-in with an elastic (spring) effect.</summary>
    public static float ElasticIn(float t)
    {
        if (t == 0f) return 0f;
        if (t == 1f) return 1f;
        return -MathF.Pow(2f, (10f * t) - 10f) * MathF.Sin(((10f * t) - 10.75f) * ElasticC4);
    }

    /// <summary>Ease-out with an elastic (spring) effect.</summary>
    public static float ElasticOut(float t)
    {
        if (t == 0f) return 0f;
        if (t == 1f) return 1f;
        return MathF.Pow(2f, -10f * t) * MathF.Sin(((10f * t) - 0.75f) * ElasticC4) + 1f;
    }

    /// <summary>Ease-in-out with an elastic (spring) effect.</summary>
    public static float ElasticInOut(float t)
    {
        if (t == 0f) return 0f;
        if (t == 1f) return 1f;
        return t < 0.5f
            ? -(MathF.Pow(2f, (20f * t) - 10f) * MathF.Sin(((20f * t) - 11.125f) * ElasticC5)) / 2f
            : (MathF.Pow(2f, (-20f * t) + 10f) * MathF.Sin(((20f * t) - 11.125f) * ElasticC5)) / 2f + 1f;
    }

    // ── Bounce ───────────────────────────────────────────────────────────────

    /// <summary>Ease-out with a bouncing effect.</summary>
    public static float BounceOut(float t)
    {
        const float n1 = 7.5625f;
        const float d1 = 2.75f;
        if (t < 1f / d1) return n1 * t * t;
        if (t < 2f / d1) { t -= 1.5f / d1; return n1 * t * t + 0.75f; }
        if (t < 2.5f / d1) { t -= 2.25f / d1; return n1 * t * t + 0.9375f; }
        t -= 2.625f / d1;
        return n1 * t * t + 0.984375f;
    }

    /// <summary>Ease-in with a bouncing effect.</summary>
    public static float BounceIn(float t) => 1f - BounceOut(1f - t);

    /// <summary>Ease-in-out with a bouncing effect.</summary>
    public static float BounceInOut(float t) =>
        t < 0.5f ? (1f - BounceOut(1f - 2f * t)) / 2f : (1f + BounceOut(2f * t - 1f)) / 2f;
}

#pragma warning restore S1244
