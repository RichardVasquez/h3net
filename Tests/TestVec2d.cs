using System;
using H3Lib;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class TestVec2d
    {
        [Test]
        public void V2DMagnitude()
        {
            var v = new Vec2d(3.0, 4.0);
            const double expected = 5.0;
            double mag = v.Magnitude;
            Assert.IsTrue(Math.Abs(mag-expected) < Constants.DBL_EPSILON);
        }

        [Test]
        public void V2DIntersect()
        {
            var p0 = new Vec2d(2.0, 2.0);
            var p1 = new Vec2d(6.0, 6.0);
            var p2 = new Vec2d(0.0, 4.0);
            var p3 = new Vec2d(10.0, 4.0);

            var intersection = Vec2d.FindIntersection(p0, p1, p2, p3);

            const double expectedX = 4.0;
            const double expectedY = 4.0;

            Assert.IsTrue(Math.Abs(intersection.X - expectedX) < Constants.DBL_EPSILON);
            Assert.IsTrue(Math.Abs(intersection.Y - expectedY) < Constants.DBL_EPSILON);
        }

        [Test]
        public void V2DEquals()
        {
            Vec2d v1 = new Vec2d(3.0, 4.0);
            Vec2d v2 = new Vec2d(3.0, 4.0);
            Vec2d v3 = new Vec2d(3.5, 4.0);
            Vec2d v4 = new Vec2d(3.0, 4.5);

            Assert.AreEqual(v1, v2);
            Assert.AreNotEqual(v1, v3);
            Assert.AreNotEqual(v1, v4);
        }
    }
}
