# GameUtils

A C# library of utility types, math helpers, and game-systems primitives for .NET 10.
Primarily aimed at game development, with a focus on performance and zero unnecessary allocations.

---

## Installation

```
dotnet add package JST.GameUtils
```

---

## Contents

- [Contributing](#notes-for-contributing-to-this-project)
- [Math](#math)
- [Vector extensions](#vector-extensions)
- [Geometry](#geometry)
- [Collections](#collections)
- [Entity / Game systems](#entity--game-systems)
- [Animation](#animation)
- [Procedural generation](#procedural-generation)
- [Types](#types)
- [Extensions](#extensions)
- [Terminal](#terminal)

---

## Notes for contributing to this project:

- Performance is a key goal. Avoid unnecessary allocations, prefer structs for small types, and consider cache locality.
- Add XML documentation comments to all public members for better IntelliSense support.
- Add tests for all public APIs, covering edge cases and typical usage patterns.
- Update the README with usage examples for any new features or types added.

## Math

### `MathFExt` — float helpers

```csharp
MathFExt.Lerp(0f, 10f, 0.5f);              // 5f
MathFExt.InverseLerp(0f, 10f, 5f);         // 0.5f
MathFExt.Smoothstep(0f, 1f, 0.5f);
MathFExt.Smootherstep(0f, 1f, 0.5f);
MathFExt.Remap(value, 0f, 1f, -1f, 1f);
MathFExt.RemapClamped(value, 0f, 1f, -1f, 1f);
MathFExt.Wrap(value, min, max);
MathFExt.Normalize(value, min, max);
MathFExt.PingPong(t, length);
MathFExt.AngleDifference(fromAngle, toAngle);
MathFExt.ToRadians(degrees);
MathFExt.ToDegrees(radians);
```

Constants: `DEGREES_PER_RADIANS`, `RADIANS_PER_DEGREE`, `HALF_PI`.

### `MathExt` — random helpers

```csharp
MathExt.RandomFloat();                      // [0, 1)
MathExt.RandomFloat(min, max);
MathExt.RandomInt(min, max);
MathExt.RandomBool();
MathExt.RandomGaussian(mean, stddev);
MathExt.RandomInCircle(radius);             // random point inside circle
MathExt.RandomOnCircle(radius);             // random point on circle edge
```

### `Easing` — standard easing functions

All functions take `float t` in [0, 1] and return a `float`.
Available families: **Sine**, **Quad**, **Cubic**, **Quart**, **Quint**, **Expo**, **Circ**, **Back**, **Elastic**, **Bounce**.
Each has `In`, `Out`, and `InOut` variants.

```csharp
float t = Easing.CubicInOut(progress);
float t = Easing.BounceOut(progress);
float t = Easing.ElasticIn(progress);
```

### `Bezier` — Bézier curves

```csharp
Vector2 point = Bezier.Quadratic(p0, p1, p2, t);
Vector2 point = Bezier.Cubic(p0, p1, p2, p3, t);
Vector2 tangent = Bezier.CubicTangent(p0, p1, p2, p3, t);

var path = new BezierPath();
path.AddSegment(p0, p1, p2, p3);
Vector2 point = path.GetPoint(t);   // t over entire path [0, 1]
Vector2 tangent = path.GetTangent(t);
```

### `CatmullRom` — Catmull-Rom splines

Unlike Bézier curves, Catmull-Rom splines pass _through_ every control point — ideal for camera paths, patrol routes, and cutscenes.

```csharp
// Single segment
Vector2 point = CatmullRom.Sample(p0, p1, p2, p3, t);
Vector2 tangent = CatmullRom.Tangent(p0, p1, p2, p3, t);

// Multi-point path
var path = new CatmullRomPath()
    .AddPoint(new Vector2(0, 0))
    .AddPoint(new Vector2(5, 3))
    .AddPoint(new Vector2(10, 0))
    .SetLooping(false);

Vector2 pos = path.GetPoint(0.5f);      // midpoint of path
Vector2 dir = path.GetTangent(0.5f);
```

### `ShuffleBag` — weighted random bag

```csharp
var bag = new ShuffleBag<string>();
bag.Add("common", weight: 10);
bag.Add("rare", weight: 1);
string item = bag.Next();
bag.Reset();
```

---

## Vector extensions

### `Vector2Ext`

```csharp
vec.Rotate(radians);
vec.Rotate(radians, origin);
vec.Add(scalar);
vec.Add(x, y);
vec.Floor(); vec.Ceil(); vec.Round();
vec.Perpendicular();
vec.WithX(x); vec.WithY(y);
vec.IsZero();
vec.ToAngle();
vec.AngleTowards(target);
vec.AngleBetween(other);
vec.GetDirection(target);
vec.MoveTowards(target, maxDelta);
vec.ToVector3(z);
vec.Deconstruct(out x, out y);

// On IEnumerable<Vector2>:
vectors.Midpoint();
vectors.Sort(clockwise: true);
vectors.Sort(center, clockwise: true);
```

### `Vector3Ext`

Includes `WithX`, `WithY`, `WithZ`, `ToVector2`, `Deconstruct`, and other component helpers.

---

## Geometry

All geometry types are in `GameUtils.Types.Geometry`.

### `AABB` — axis-aligned bounding box

```csharp
var box = new AABB(min, max);
box.Contains(point);
box.Intersects(other);           // AABB, Line, Circle, Polygon2D, Vector2[]
box.Intersects(line, out intersectionPoints);
```

### `Circle`

```csharp
var circle = new Circle(center, radius);
circle.Contains(point);
circle.Intersects(aabb);         // AABB, Line, Circle, Polygon2D
circle.RadiusSquared;            // cached, avoids sqrt in hot paths
```

### `Line`

```csharp
var line = new Line(start, end);
var line = Line.FromAngle(origin, angle, length);
line.Length; line.Midpoint; line.NormalA; line.NormalB;
line.Intersects(other);          // Line, AABB, Circle, Polygon2D
line.Cast(maxDistance, shapes);  // first intersection along ray
```

### `Ray2D`

```csharp
var ray = new Ray2D(origin, direction);
ray.At(distance);
ray.Intersects(line, out t, out point);
ray.Intersects(circle, out t, out point);
ray.Intersects(aabb, out t, out point);
```

### `Polygon2D`

```csharp
var poly = new Polygon2D(vertices);        // auto-sorts CCW
poly.Vertices; poly.Edges; poly.Normals;
poly.Contains(point);
poly.Intersects(aabb);
poly.TranslateBy(delta);
```

### `Quad`

```csharp
var quad = new Quad(topLeft, topRight, bottomLeft, bottomRight);
quad.Intersects(line, out point);
quad[index]; // corner by index 0–3
```

---

## Collections

### `Grid<T>` — 2D array wrapper

```csharp
var grid = new Grid<int>(width, height);
grid[x, y] = 1;
grid[vector2] = 1;
grid.TryGet(x, y, out value);
grid.Fill(defaultValue);
grid.Fill((x, y) => ComputeValue(x, y));
grid.IsInBounds(x, y);
foreach (var (x, y, value) in grid) { ... }
```

### `QuadTree<T>` — hierarchical spatial partition

Best for non-uniform object distributions (e.g. world objects, static geometry).

```csharp
var tree = new QuadTree<Enemy>(worldBounds, capacity: 8);
tree.Insert(enemy, enemy.Position);
tree.Query(camera.GetVisibleBounds());         // IEnumerable<Enemy>
tree.Query(explosionCenter, blastRadius);
tree.Remove(enemy, enemy.Position);
tree.Clear();
```

### `SpatialHash<T>` — grid-based spatial hash

Best for large numbers of uniformly distributed dynamic objects (bullets, particles, units). O(1) amortized insert and query.

```csharp
var hash = new SpatialHash<Bullet>(cellSize: 64f);
hash.Insert(bullet, bullet.Position);
hash.Query(region);                             // IEnumerable<Bullet>
hash.Query(center, radius);
hash.Remove(bullet, bullet.Position);
hash.Clear();
```

### `RingBuffer<T>` — fixed-size circular FIFO

```csharp
var buffer = new RingBuffer<float>(capacity: 64);
buffer.Write(sample);
float value = buffer.Read();
buffer.TryRead(out value);
buffer.Peek();
buffer.Snapshot();     // copy of all current elements
```

### `ConcurrentHashSet<T>` / `SynchronizedHashSet<T>`

Thread-safe set types for use in multi-threaded game systems.

---

## Entity / Game systems

### `StateMachine<TState>` — finite state machine

```csharp
var fsm = new StateMachine<State>(State.Idle);
fsm.AddTransition(State.Idle, State.Run, () => speed > 0);
fsm.OnEnter(State.Run, () => PlayAnim("run"));
fsm.OnExit(State.Run, () => StopAnim());
fsm.Update();           // evaluates transitions and fires callbacks
fsm.ForceState(State.Dead);
```

### `ObjectPool<T>` — reusable object pool

```csharp
var pool = new ObjectPool<Bullet>(() => new Bullet());
var bullet = pool.Rent();
// ... use bullet ...
pool.Return(bullet);
```

### `EventBus` — type-safe publish/subscribe

Zero-allocation on the dispatch path (copy-on-write snapshot rebuilt only on subscribe/unsubscribe).

```csharp
var bus = new EventBus();
bus.Subscribe<PlayerDiedEvent>(e => HandleDeath(e));
bus.Publish(new PlayerDiedEvent { Position = pos });
bus.Unsubscribe<PlayerDiedEvent>(handler);
bus.Clear<PlayerDiedEvent>();
```

### `Tween<T>` — smooth value transitions over time

```csharp
// Float tween with cubic ease-out
var tween = Tween.Float(0f, 100f, duration: 1.5f, Easing.CubicOut);
float value = tween.Update(deltaTime);
if (tween.IsComplete) { ... }
tween.Reset();
tween.Reverse();

// Vector2 tween
var move = Tween.Vec2(start, end, 2f, Easing.SineInOut);

// Color tween
var fade = Tween.Color(Color.White, Color.Transparent, 0.5f);
```

### `BehaviorTree` — behavior tree framework

```csharp
var tree = new BehaviorTree(
    new Selector(
        new Sequence(new IsEnemyVisible(), new ChaseEnemy()),
        new Patrol()
    )
);
tree.Tick(context);
```

Node types: `Sequence`, `Selector`, `Inverter`, `Leaf` (for custom logic).

### `AStar<T>` / `Dijkstra<T>` — pathfinding

```csharp
var astar = new AStar<Vector2Int>();
astar.AddEdge(new Edge<Vector2Int>(a, b, cost));
astar.Solve(start, goal, (a, b) => Vector2.Distance(a, b), out var path);

var dijkstra = new Dijkstra<string>();
dijkstra.AddEdge(new Edge<string>("A", "B", 1.5f));
dijkstra.Solve("A", "D", out var path);
```

### `GridSearch` — BFS / flood fill on `Grid<T>`

```csharp
GridSearch.BreadthFirstSearch(grid, start, isWalkable, out path);
GridSearch.FloodFill(grid, origin, condition, fillValue);
```

### `Thinker` — timer-driven callback

```csharp
var thinker = new Thinker(interval: 0.5f);
thinker.OnThink = () => UpdateAI();
thinker.Update(deltaTime);
```

### `FixedScheduler` — fixed-rate update loop

```csharp
class PhysicsLoop : FixedScheduler
{
    public PhysicsLoop() : base(tickRate: 60) { }
    protected override void Update() => StepPhysics(1f / 60f);
}
var loop = new PhysicsLoop();
loop.Start();
// ... later ...
loop.Stop();
```

---

## Animation

### `Controller` — frame-based animation

```csharp
var anim = new Controller(frameCount: 10, isLooping: true, framesPerSecond: 12);
anim.OnFrameChanged = frame => currentSprite = sprites[frame];
anim.Play();
anim.Update(deltaTime);
```

### `Tweener` — time-driven float interpolation

```csharp
var tween = new Tweener(from: 0f, to: 100f, duration: 1.5f, easingFunction: Ease.CubicOut);
tween.OnComplete = () => Console.WriteLine("Done!");
tween.Update(deltaTime);
```

### `Ease` / `Offset` — animation helpers

`Ease` contains easing functions like `Ease.ElasticOut`, `Ease.BounceIn`, etc.
`Offset` contains math functions for offsetting animations (`Jagged`, `Sine`, `Pulse`, `Triangle`, `Wobble`).

---

## Procedural generation

### `PerlinNoise` — Perlin noise + fBm

```csharp
var noise = new PerlinNoise();             // standard Ken Perlin table
var noise = new PerlinNoise(seed: 42);    // seeded / reproducible

float v = noise.Sample(x, y);            // 2D, [0, 1]
float v = noise.Sample(x, y, z);         // 3D, [-1, 1]
float v = noise.Fbm(x, y, octaves: 6);   // fractal Brownian motion, [0, 1]

// Or use the shared default instance:
PerlinNoise.Default.Sample(x, y);
```

### `Diamond` — diamond-square terrain generation

```csharp
Grid<int> heightmap = Diamond.Create(
    size: 257,          // must be power-of-two + 1
    min: 0, max: 255,
    range: 128f,
    nextRange: r => r * 0.5f,
    valueFactory: (avg, range) => (int)(avg + Random.Shared.NextSingle() * range),
    seed: 42
);
```

---

## Types

### `Color`

```csharp
var c = new Color(r: 1f, g: 0f, b: 0.5f, a: 1f);
var c = new Color(byte r, byte g, byte b, byte a);
Color.Lerp(a, b, t);
Color.Red; Color.SkyBlue; // 140+ named CSS colors
(Vector3)c; (Vector4)c;   // explicit casts
Color.FromRgba(0xFF8800FF);
```

### `Gradient`

```csharp
var gradient = new Gradient();
gradient.AddStop(0f, Color.Black)
        .AddStop(0.5f, Color.Red)
        .AddStop(1f, Color.White);
Color c = gradient.Evaluate(0.75f);
```

### `Camera2D`

```csharp
var cam = new Camera2D { Position = pos, Zoom = 1.5f, Rotation = 0f };
Vector2 screen = cam.WorldToScreen(worldPos);
Vector2 world = cam.ScreenToWorld(screenPos);
AABB visible = cam.GetVisibleBounds();
```

### `Bitmap` / `ImageData`

Low-level pixel buffers. `Bitmap` supports drawing primitives (`Rectangle`, `Line`, `Circle`) and writing BMP files. `ImageData` is a compact RGBA container.

---

## Extensions

### `CollectionExtensions`

```csharp
list.GetRandom();
list.Shuffle();
list.WeightedRandom(item => item.Weight);
list.ForEach(item => DoSomething(item));
list.SelectWhere(item => item.IsActive, item => item.Value);
list.ToIndex();                          // Dictionary<T, int>
```

### `ObjectExtensions`

```csharp
obj.Mutate(o => o.X = 5);               // mutate and return same object
obj.Out(out var local);                  // assign to local, keep chain
obj.Curry(arg1);                         // partial application
```

### `StringExtensions`

```csharp
str.TryGet(index, out char c);
str.Repeat(3);
```

---

## Terminal

### `Ansi` — ANSI escape sequences

```csharp
Ansi.WriteLine("Hello!", Ansi.Bold + Ansi.Foreground(Color.Cyan));
Ansi.MoveTo(x: 10, y: 5);
Ansi.Clear();
Ansi.ClearLine();
```

### `Progress` — console progress helpers

```csharp
Progress.Bar(current, total, width: 40);
Progress.PercentComplete(current, total);
Progress.Rate(itemsProcessed, elapsed);
Progress.TimeRemaining(current, total, elapsed);
```
