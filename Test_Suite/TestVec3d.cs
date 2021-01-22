using System;
using H3Lib;
using H3Lib.Extensions;
using NUnit.Framework;

namespace TestSuite
{
    [TestFixture]
    public class TestVec3d
    {
        [Test]
        public void PointSquareDistance()
        {
            
            Vec3d v1 = new Vec3d(0, 0, 0);
            Vec3d v2 = new Vec3d(1, 0, 0);
            Vec3d v3 = new Vec3d(0, 1, 1);
            Vec3d v4 = new Vec3d(1, 1, 1);
            Vec3d v5 = new Vec3d(1, 1, 2);

            Assert.IsTrue(Math.Abs(v1.PointSquareDistance(v1)) < Constants.DBL_EPSILON);
            Assert.IsTrue(Math.Abs(v1.PointSquareDistance(v2) - 1) < Constants.DBL_EPSILON);
            Assert.IsTrue(Math.Abs(v1.PointSquareDistance(v3) - 2) < Constants.DBL_EPSILON);
            Assert.IsTrue(Math.Abs(v1.PointSquareDistance(v4) - 3) < Constants.DBL_EPSILON);
            Assert.IsTrue(Math.Abs(v1.PointSquareDistance(v5) - 6) < Constants.DBL_EPSILON);
        }

        [Test]
        public void GeoToVec3d()
        {
            var origin = new Vec3d();
            var c1 = new GeoCoord();
            var p1 = c1.ToVec3d();
            Assert.IsTrue(Math.Abs(origin.PointSquareDistance(p1) -1) < Constants.EPSILON_RAD);

            var c2 = new GeoCoord(Constants.M_PI_2, 0);
            var p2 = c2.ToVec3d();
            Assert.IsTrue(Math.Abs(p1.PointSquareDistance(p2) - 2) < Constants.EPSILON_RAD);

            var c3 = new GeoCoord(Constants.M_PI, 0);
            var p3 = c3.ToVec3d();
            Assert.IsTrue(Math.Abs(p1.PointSquareDistance(p3) - 4)<Constants.EPSILON_RAD);
        }
    }
}
