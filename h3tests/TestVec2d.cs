using System;
using H3Net.Code;
using NUnit.Framework;

namespace h3tests
{
    [TestFixture]
    public class TestVec2d
    {
        [Test]
        public void _v2dMag()
        {
            Vec2d v = new Vec2d(3.0, 4.0);
            double expected = 5.0;
            double mag = Vec2d._v2dMag(v);
            Assert.True(Math.Abs(mag - expected) < Constants.DBL_EPSILON, "magnitude as expected");
        }

        [Test]
        public void _v2dIntersect()
        {
            Vec2d p0 = new Vec2d(2.0, 2.0);
            Vec2d p1 = new Vec2d(6.0, 6.0);
            Vec2d p2 = new Vec2d(0.0, 4.0);
            Vec2d p3 = new Vec2d(10.0, 4.0);
            Vec2d intersection = new Vec2d(0.0, 0.0);

            Vec2d._v2dIntersect(p0, p1, p2, p3, ref intersection);

            double expectedX = 4.0;
            double expectedY = 4.0;

            Assert.True(Math.Abs(intersection.x - expectedX) < Constants.DBL_EPSILON,
                     "X coord as expected");
            Assert.True(Math.Abs(intersection.y - expectedY) < Constants.DBL_EPSILON,
                     "Y coord as expected");
        }

        [Test]
        public void _v2dEquals()
        {
            Vec2d v1 = new Vec2d(3.0, 4.0);
            Vec2d v2 = new Vec2d(3.0, 4.0);
            Vec2d v3 = new Vec2d(3.5, 4.0);
            Vec2d v4 = new Vec2d(3.0, 4.5);

            Assert.True(Vec2d._v2dEquals(v1, v2), "true for equal vectors");
            Assert.False(Vec2d._v2dEquals(v1, v3), "false for different x");
            Assert.False(Vec2d._v2dEquals(v1, v4), "false for different y");
        }

    }
}
