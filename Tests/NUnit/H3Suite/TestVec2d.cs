using System;
using H3Lib;
using NUnit.Framework;

namespace TestSuite
{
    [TestFixture]
    public class TestVec2d
    {
        [Test]
        public void V2DMagnitude()
        {
            var v = new Vec2d(3.0m, 4.0m);
            const  decimal expected = 5.0m;
            decimal mag = v.Magnitude;
            Assert.IsTrue(Math.Abs(mag-expected) < Constants.H3.DBL_EPSILON);
        }

        [Test]
        public void V2DIntersect()
        {
            var p0 = new Vec2d(2.0m, 2.0m);
            var p1 = new Vec2d(6.0m, 6.0m);
            var p2 = new Vec2d(0.0m, 4.0m);
            var p3 = new Vec2d(10.0m, 4.0m);

            var intersection = Vec2d.FindIntersection(p0, p1, p2, p3);

            const  decimal expectedX = 4.0m;
            const  decimal expectedY = 4.0m;

            Assert.IsTrue(Math.Abs(intersection.X - expectedX) < Constants.H3.DBL_EPSILON);
            Assert.IsTrue(Math.Abs(intersection.Y - expectedY) < Constants.H3.DBL_EPSILON);
        }

        [Test]
        public void V2DEquals()
        {
            Vec2d v1 = new Vec2d(3.0m, 4.0m);
            Vec2d v2 = new Vec2d(3.0m, 4.0m);
            Vec2d v3 = new Vec2d(3.5m, 4.0m);
            Vec2d v4 = new Vec2d(3.0m, 4.5m);

            Assert.AreEqual(v1, v2);
            Assert.AreNotEqual(v1, v3);
            Assert.AreNotEqual(v1, v4);
        }
    }
}
