using System;
using H3Net.Code;
using NUnit.Framework;

namespace h3tests
{
    public class TestVec3d
    {
        [Test]
        public void _pointSquareDist()
        {
            Vec3d v1 = new Vec3d( 0, 0, 0);
            Vec3d v2 = new Vec3d( 1, 0, 0);
            Vec3d v3 = new Vec3d( 0, 1, 1);
            Vec3d v4 = new Vec3d( 1, 1, 1);
            Vec3d v5 = new Vec3d( 1, 1, 2);

            Assert.True(Math.Abs(Vec3d._pointSquareDist(v1, v1)) < Constants.DBL_EPSILON,
                     "distance to self is 0");
            Assert.True(Math.Abs(Vec3d._pointSquareDist(v1, v2) - 1) < Constants.DBL_EPSILON,
                     "distance to <1,0,0> is 1");
            Assert.True(Math.Abs(Vec3d._pointSquareDist(v1, v3) - 2) < Constants.DBL_EPSILON,
                     "distance to <0,1,1> is 2");
            Assert.True(Math.Abs(Vec3d._pointSquareDist(v1, v4) - 3) < Constants.DBL_EPSILON,
                     "distance to <1,1,1> is 3");
            Assert.True(Math.Abs(Vec3d._pointSquareDist(v1, v5) - 6) < Constants.DBL_EPSILON,
                     "distance to <1,1,2> is 6");
        }

        [Test]
        public void _geoToVec3d()
        {
            Vec3d origin = new Vec3d();

            GeoCoord c1 = new GeoCoord(0, 0);
            Vec3d p1 = new Vec3d();
            Vec3d._geoToVec3d(c1, ref p1);
            Assert.True(Math.Abs(Vec3d._pointSquareDist(origin, p1) - 1) < Constants.EPSILON_RAD,
                     "Geo point is on the unit sphere");

            GeoCoord c2 = new GeoCoord(Constants.M_PI_2, 0);
            Vec3d p2 = new Vec3d();
            Vec3d._geoToVec3d(c2, ref p2);
            Assert.True(Math.Abs(Vec3d._pointSquareDist(p1, p2) - 2) < Constants.EPSILON_RAD,
                     "Geo point is on another axis");

            GeoCoord c3 = new GeoCoord(Constants.M_PI, 0);
            Vec3d p3 = new Vec3d();
            Vec3d._geoToVec3d(c3, ref p3);
            Assert.True(Math.Abs(Vec3d._pointSquareDist(p1, p3) - 4) < Constants.EPSILON_RAD,
                     "Geo point is the other side of the sphere");
        }
    }
}
