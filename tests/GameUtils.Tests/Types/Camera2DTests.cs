using System;
using System.Numerics;
using GameUtils.Types;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GameUtils.Tests.Types
{
    [TestClass]
    public class Camera2DTests
    {
        [TestMethod]
        [DataRow(0f)]
        [DataRow(-1f)]
        [DataRow(-0.5f)]
        [DataRow(float.NegativeInfinity)]
        public void Zoom_SetInvalidValue_ThrowsArgumentOutOfRangeException(float invalidZoom)
        {
            var camera = new Camera2D(new Vector2(800, 600));
            Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => camera.Zoom = invalidZoom);
        }

        [TestMethod]
        [DataRow(0f)]
        [DataRow(-1f)]
        [DataRow(-0.5f)]
        [DataRow(float.NegativeInfinity)]
        public void Constructor_InvalidZoom_ThrowsArgumentOutOfRangeException(float invalidZoom)
        {
            Assert.ThrowsExactly<ArgumentOutOfRangeException>(() =>
                new Camera2D(Vector2.Zero, new Vector2(800, 600), invalidZoom));
        }

        [TestMethod]
        [DataRow(1f)]
        [DataRow(2.5f)]
        [DataRow(0.1f)]
        public void Zoom_SetValidValue_UpdatesZoomProperty(float validZoom)
        {
            var camera = new Camera2D(new Vector2(800, 600))
            {
                Zoom = validZoom
            };
            Assert.AreEqual(validZoom, camera.Zoom);
        }

        [TestMethod]
        [DataRow(1f)]
        [DataRow(2.5f)]
        [DataRow(0.1f)]
        public void Constructor_ValidZoom_SetsZoomProperty(float validZoom)
        {
            var camera = new Camera2D(Vector2.Zero, new Vector2(800, 600), validZoom);
            Assert.AreEqual(validZoom, camera.Zoom);
        }
    }
}
