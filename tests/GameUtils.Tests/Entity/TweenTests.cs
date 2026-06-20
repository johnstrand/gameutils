using Microsoft.VisualStudio.TestTools.UnitTesting;
using GameUtils.Entity;
using System;
using System.Numerics;

namespace GameUtils.Tests.Entity
{
    [TestClass]
    public class TweenTests
    {
        [TestMethod]
        public void Update_LinearFloatTween_ShouldInterpolateCorrectly()
        {
            var tween = Tween.Float(0f, 100f, 1f);

            Assert.AreEqual(0f, tween.Value);

            var value = tween.Update(0.5f);
            Assert.AreEqual(50f, value);
            Assert.AreEqual(50f, tween.Value);
            Assert.IsFalse(tween.IsComplete);

            tween.Update(0.5f);
            Assert.AreEqual(100f, tween.Value);
            Assert.IsTrue(tween.IsComplete);
        }

        [TestMethod]
        public void Update_BeyondDuration_ShouldClampToFinalValueAndSetComplete()
        {
            var tween = Tween.Float(0f, 100f, 1f);

            var value = tween.Update(1.5f);

            Assert.AreEqual(100f, value);
            Assert.IsTrue(tween.IsComplete);
        }

        [TestMethod]
        public void Update_WhenComplete_ShouldReturnFinalValueAndNotChangeCompleteState()
        {
            var tween = Tween.Float(0f, 10f, 1f);
            tween.Update(1f); // Now complete

            var value = tween.Update(1f);

            Assert.AreEqual(10f, value);
            Assert.IsTrue(tween.IsComplete);
        }

        [TestMethod]
        public void Reset_AfterPartialUpdate_ShouldReturnToInitialState()
        {
            var tween = Tween.Float(0f, 10f, 1f);
            tween.Update(0.5f);

            tween.Reset();

            Assert.AreEqual(0f, tween.Value);
            Assert.IsFalse(tween.IsComplete);

            // Updating should now proceed from the beginning
            tween.Update(0.5f);
            Assert.AreEqual(5f, tween.Value);
        }

        [TestMethod]
        public void Reverse_WhileRunning_ShouldUpdateBackwards()
        {
            var tween = Tween.Float(0f, 10f, 1f);
            tween.Update(0.5f); // Value is 5

            tween.Reverse();
            tween.Update(0.25f); // Should move back towards 0 (t = 0.5 + 0.25 = 0.75 elapsed, but reversed so effective t = 0.25)

            Assert.AreEqual(2.5f, tween.Value);
            Assert.IsFalse(tween.IsComplete);

            tween.Update(0.5f); // Will complete going backwards
            Assert.AreEqual(0f, tween.Value);
            Assert.IsTrue(tween.IsComplete);
        }

        [TestMethod]
        public void Reverse_WhenComplete_ShouldRestartBackwards()
        {
            var tween = Tween.Float(0f, 10f, 1f);
            tween.Update(1f); // Complete, value is 10

            tween.Reverse(); // Now restarts towards 0

            Assert.IsFalse(tween.IsComplete);
            tween.Update(0.5f);
            Assert.AreEqual(5f, tween.Value);

            tween.Update(0.5f);
            Assert.AreEqual(0f, tween.Value);
            Assert.IsTrue(tween.IsComplete);
        }

        [TestMethod]
        public void Constructor_ZeroDuration_ShouldThrowArgumentOutOfRangeException()
        {
            Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => Tween.Float(0f, 10f, 0f));
        }

        [TestMethod]
        public void Constructor_NegativeDuration_ShouldThrowArgumentOutOfRangeException()
        {
            Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => Tween.Float(0f, 10f, -1f));
        }

        [TestMethod]
        public void Constructor_NullLerpFunction_ShouldThrowArgumentNullException()
        {
            Assert.ThrowsExactly<ArgumentNullException>(() => new Tween<float>(0f, 10f, 1f, null!));
        }

        [TestMethod]
        public void Constructor_CustomEasing_ShouldApplyEasing()
        {
            // Simple ease in quad easing: t => t * t
            var tween = Tween.Float(0f, 100f, 1f, t => t * t);

            tween.Update(0.5f); // Normal float lerp would be 50. Ease quad is 0.5 * 0.5 = 0.25. 100 * 0.25 = 25.
            Assert.AreEqual(25f, tween.Value);
        }

        [TestMethod]
        public void Vec2Tween_ShouldInterpolateCorrectly()
        {
            var from = new Vector2(0, 0);
            var to = new Vector2(10, 20);
            var tween = Tween.Vec2(from, to, 1f);

            tween.Update(0.5f);

            Assert.AreEqual(new Vector2(5, 10), tween.Value);
        }

        [TestMethod]
        public void Vec3Tween_ShouldInterpolateCorrectly()
        {
            var from = new Vector3(0, 0, 0);
            var to = new Vector3(10, 20, 30);
            var tween = Tween.Vec3(from, to, 1f);

            tween.Update(0.5f);

            Assert.AreEqual(new Vector3(5, 10, 15), tween.Value);
        }

        [TestMethod]
        public void ColorTween_ShouldInterpolateCorrectly()
        {
            var from = new GameUtils.Color(0, 0, 0, 255); // Black
            var to = new GameUtils.Color(255, 255, 255, 255); // White
            var tween = Tween.Color(from, to, 1f);

            tween.Update(0.5f);

            var expected = GameUtils.Color.Lerp(from, to, 0.5f);
            Assert.AreEqual(expected.R, tween.Value.R);
            Assert.AreEqual(expected.G, tween.Value.G);
            Assert.AreEqual(expected.B, tween.Value.B);
            Assert.AreEqual(expected.A, tween.Value.A);
        }
    }
}
