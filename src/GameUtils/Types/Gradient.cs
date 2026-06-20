namespace GameUtils.Types;

/// <summary>
/// A color gradient defined by a set of color stops. Evaluating the gradient at a given position
/// returns the linearly interpolated color between the two surrounding stops.
/// </summary>
public class Gradient
{
    private readonly List<(float position, Color color)> _stops = [];

    /// <summary>Creates an empty gradient.</summary>
    public Gradient() { }

    /// <summary>Creates a gradient from an existing collection of stops.</summary>
    public Gradient(IEnumerable<(float position, Color color)> stops)
    {
        foreach (var (pos, color) in stops)
        {
            AddStop(pos, color);
        }
    }

    /// <summary>
    /// Adds a color stop at <paramref name="position"/> (0–1 range recommended).
    /// Stops are automatically kept sorted by position.
    /// </summary>
    public Gradient AddStop(float position, Color color)
    {
        _stops.Add((position, color));
        _stops.Sort((a, b) => a.position.CompareTo(b.position));
        return this;
    }

    /// <summary>
    /// Evaluates the gradient at <paramref name="t"/>. Returns the first stop color when the gradient has
    /// only one stop, and <see cref="Color.Black"/> (transparent) when empty.
    /// </summary>
    public Color Evaluate(float t)
    {
        if (_stops.Count == 0)
        {
            return new Color(0, 0, 0, 0);
        }

        if (_stops.Count == 1 || t <= _stops[0].position)
        {
            return _stops[0].color;
        }

        if (t >= _stops[^1].position)
        {
            return _stops[^1].color;
        }

        for (var i = 0; i < _stops.Count - 1; i++)
        {
            var (posA, colorA) = _stops[i];
            var (posB, colorB) = _stops[i + 1];

            if (t >= posA && t <= posB)
            {
                var localT = (t - posA) / (posB - posA);
                return Color.Lerp(colorA, colorB, localT);
            }
        }

        return _stops[^1].color;
    }
}
