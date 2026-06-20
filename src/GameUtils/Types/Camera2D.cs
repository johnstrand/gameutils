using System.Numerics;

namespace GameUtils.Types;

/// <summary>
/// A simple 2D camera with position, zoom, and rotation transforms.
/// Provides conversions between world space and screen (viewport) space.
/// </summary>
public class Camera2D
{
    /// <summary>
    /// The camera's position in world space (the point the camera looks at).
    /// </summary>
    public Vector2 Position { get; set; }

    /// <summary>
    /// Zoom factor. Values &gt; 1 zoom in; values between 0 and 1 zoom out. Must be positive.
    /// </summary>
    public float Zoom
    {
        get => _zoom;
        set
        {
            if (value <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(value), "Zoom must be positive.");
            }

            _zoom = value;
        }
    }

    /// <summary>
    /// Camera rotation in radians.
    /// </summary>
    public float Rotation { get; set; }

    /// <summary>
    /// The size of the viewport in pixels.
    /// </summary>
    public Vector2 ViewSize { get; set; }

    private float _zoom = 1f;

    /// <summary>
    /// Creates a camera at the origin with the given viewport size.
    /// </summary>
    public Camera2D(Vector2 viewSize)
    {
        ViewSize = viewSize;
    }

    /// <summary>
    /// Creates a camera with the given position, viewport size, zoom, and rotation.
    /// </summary>
    public Camera2D(Vector2 position, Vector2 viewSize, float zoom = 1f, float rotation = 0f)
    {
        Position = position;
        ViewSize = viewSize;
        Zoom = zoom;
        Rotation = rotation;
    }

    /// <summary>
    /// Converts a point in world space to screen (viewport pixel) coordinates.
    /// </summary>
    public Vector2 WorldToScreen(Vector2 worldPos)
    {
        var translated = worldPos - Position;
        var (sin, cos) = MathF.SinCos(-Rotation);
        var rotated = new Vector2(
            (translated.X * cos) - (translated.Y * sin),
            (translated.X * sin) + (translated.Y * cos));
        return (rotated * Zoom) + (ViewSize / 2f);
    }

    /// <summary>
    /// Converts a screen (viewport pixel) coordinate to world space.
    /// </summary>
    public Vector2 ScreenToWorld(Vector2 screenPos)
    {
        var centered = (screenPos - (ViewSize / 2f)) / Zoom;
        var (sin, cos) = MathF.SinCos(Rotation);
        var rotated = new Vector2(
            (centered.X * cos) - (centered.Y * sin),
            (centered.X * sin) + (centered.Y * cos));
        return rotated + Position;
    }

    /// <summary>
    /// Returns an <see cref="GameUtils.Types.Geometry.AABB"/> representing the visible world-space region.
    /// </summary>
    public Geometry.AABB GetVisibleBounds()
    {
        var halfSize = ViewSize / (2f * Zoom);
        return new Geometry.AABB(Position - halfSize, Position + halfSize);
    }
}
