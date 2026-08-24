using System;
using System.Numerics;
using GameUtils.Types;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GameUtils.Tests.Types
{
    [TestClass]
    public class Camera2DTests
    {
        private const float Delta = 1e-4f;

        [TestMethod]
        public void Constructor_WithViewSize_SetsDefaultProperties()
        {
            var viewSize = new Vector2(800, 600);
            var camera = new Camera2D(viewSize);

            Assert.AreEqual(Vector2.Zero, camera.Position);
            Assert.AreEqual(viewSize, camera.ViewSize);
            Assert.AreEqual(1f, camera.Zoom);
            Assert.AreEqual(0f, camera.Rotation);
        }

        [TestMethod]
        public void Constructor_WithFullParameters_SetsProperties()
        {
            var pos = new Vector2(100, 200);
            var viewSize = new Vector2(1920, 1080);
            var zoom = 2f;
            var rotation = MathF.PI / 4f;

            var camera = new Camera2D(pos, viewSize, zoom, rotation);

            Assert.AreEqual(pos, camera.Position);
            Assert.AreEqual(viewSize, camera.ViewSize);
            Assert.AreEqual(zoom, camera.Zoom);
            Assert.AreEqual(rotation, camera.Rotation);
        }

        [TestMethod]
        [DataRow(0f)]
        [DataRow(-1f)]
        [DataRow(-0.001f)]
        public void Zoom_InvalidValue_ThrowsArgumentOutOfRangeException(float invalidZoom)
        {
            var camera = new Camera2D(new Vector2(800, 600));
            Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => camera.Zoom = invalidZoom);
        }

        [TestMethod]
        public void Zoom_ValidValue_UpdatesZoom()
        {
            var camera = new Camera2D(new Vector2(800, 600));
            camera.Zoom = 2.5f;
            Assert.AreEqual(2.5f, camera.Zoom);
        }

        [TestMethod]
        public void WorldToScreen_IdentityCamera_TransformsCorrectly()
        {
            var camera = new Camera2D(Vector2.Zero, new Vector2(800, 600), 1f, 0f);
            var world = new Vector2(0, 0);
            var screen = camera.WorldToScreen(world);

            Assert.AreEqual(400f, screen.X, Delta);
            Assert.AreEqual(300f, screen.Y, Delta);
        }

        [TestMethod]
        public void ScreenToWorld_IdentityCamera_TransformsCorrectly()
        {
            var camera = new Camera2D(Vector2.Zero, new Vector2(800, 600), 1f, 0f);
            var screen = new Vector2(400, 300);
            var world = camera.ScreenToWorld(screen);

            Assert.AreEqual(0f, world.X, Delta);
            Assert.AreEqual(0f, world.Y, Delta);
        }

        [TestMethod]
        public void WorldToScreen_And_ScreenToWorld_RoundTrip()
        {
            var camera = new Camera2D(new Vector2(150, -50), new Vector2(1024, 768), 1.5f, MathF.PI / 6f);
            var originalWorld = new Vector2(200, 100);

            var screen = camera.WorldToScreen(originalWorld);
            var worldRoundTrip = camera.ScreenToWorld(screen);

            Assert.AreEqual(originalWorld.X, worldRoundTrip.X, Delta);
            Assert.AreEqual(originalWorld.Y, worldRoundTrip.Y, Delta);
        }

        [TestMethod]
        public void GetVisibleBounds_CalculatesCorrectAABB()
        {
            var camera = new Camera2D(new Vector2(100, 200), new Vector2(800, 600), 2f, 0f);
            var bounds = camera.GetVisibleBounds();

            Assert.AreEqual(-100f, bounds.Min.X, Delta);
            Assert.AreEqual(50f, bounds.Min.Y, Delta);
            Assert.AreEqual(300f, bounds.Max.X, Delta);
            Assert.AreEqual(350f, bounds.Max.Y, Delta);
        }
    }
}
