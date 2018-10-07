using System;
using System.Text;
using System.Collections.Generic;
using h3net.API;
using NUnit.Framework;


public delegate double DecreaseFunction(int i);

namespace h3tests
{
    /// <summary>
    /// Summary description for TestGeoCoord
    /// </summary>
    [TestFixture]
    public class TestGeoCoord
    {


        void testDecreasingFunction(Func<int, double>function, string message)
        {
            double last = 0;
            for (int i = Constants.MAX_H3_RES; i >= 0; i--)
            {
                var next = function(i);
                Assert.True(next > last, message);
                last = next;
            }
        }
        [Test]
        public void radsToDegs()
        {
            double originalRads = 1.0;
            double degs = GeoCoord.radsToDegs(originalRads);
            double rads = GeoCoord.degsToRads(degs);
            //radsToDegs/degsToRads invertible
            Assert.True(Math.Abs(rads - originalRads) < Constants.EPSILON_RAD);
        }

        [Test]
        public void _geoDistRads()
        {
            GeoCoord p1 = new GeoCoord();
            GeoCoord.setGeoDegs(ref p1, 10, 10);
            GeoCoord p2 = new GeoCoord();
            GeoCoord.setGeoDegs(ref p2, 0, 10);

            // 0 distance as expected
            Assert.True(GeoCoord._geoDistRads(p1, p1) < Constants.EPSILON_RAD * 1000);
            // distance along longitude as expected
            Assert.True
                (Math.Abs(GeoCoord._geoDistRads(p1, p2) - GeoCoord.degsToRads(10)) < Constants.EPSILON_RAD * 1000);
        }

        [Test]
        public void constrainLatLng()
        {
            double TOLERANCE = Constants.EPSILON;
            // lat 0
            Assert.True(Math.Abs(GeoCoord.constrainLat(0)) < TOLERANCE);
            // lat 1
            Assert.True(Math.Abs(GeoCoord.constrainLat(1) - 1) < TOLERANCE);
            // lat pi / 2
            Assert.True(Math.Abs(GeoCoord.constrainLat(Constants.M_PI_2) - Constants.M_PI_2) < TOLERANCE);
            // lat pi
            Assert.True(Math.Abs(GeoCoord.constrainLat(Constants.M_PI)) < TOLERANCE);
            // lat pi + 1
            Assert.True(Math.Abs(GeoCoord.constrainLat(Constants.M_PI + 1) - 1) < TOLERANCE);
            // lat 2 pi + 1
            Assert.True(Math.Abs(GeoCoord.constrainLat(2 * Constants.M_PI + 1) - 1) < TOLERANCE);

            // lng 0
            Assert.True(Math.Abs(GeoCoord.constrainLng(0)) < TOLERANCE);
            // lng 1
            Assert.True(Math.Abs(GeoCoord.constrainLng(1) - 1) < TOLERANCE);
            // lng pi
            Assert.True(Math.Abs(GeoCoord.constrainLng(Constants.M_PI) - Constants.M_PI) < TOLERANCE);
            // lng 2 pi
            Assert.True(Math.Abs(GeoCoord.constrainLng(2 * Constants.M_PI)) < TOLERANCE);
            // lng 3 pi
            Assert.True(Math.Abs(GeoCoord.constrainLng(3 * Constants.M_PI) - Constants.M_PI) < TOLERANCE);
            // lng 4 pi
            Assert.True(Math.Abs(GeoCoord.constrainLng(4 * Constants.M_PI)) < TOLERANCE);
        }

        [Test]
        public void _geoAzDistanceRads_noop()
        {
            GeoCoord start = new GeoCoord(15, 10);
            GeoCoord out_gc = new GeoCoord();
            GeoCoord expected = new GeoCoord(15, 10);

            GeoCoord._geoAzDistanceRads(ref start, 0, 0, ref out_gc);
            // 0 distance produces same point
            Assert.True(GeoCoord.geoAlmostEqual(expected, out_gc));
        }

        [Test]
        public void _geoAzDistanceRads_dueNorthSouth()
        {
            GeoCoord start = new GeoCoord();
            GeoCoord out_gc = new GeoCoord();
            GeoCoord expected = new GeoCoord();

            // Due north to north pole
            GeoCoord.setGeoDegs(ref start, 45, 1);
            GeoCoord.setGeoDegs(ref expected, 90, 0);
            GeoCoord._geoAzDistanceRads(ref start, 0, GeoCoord.degsToRads(45), ref out_gc);
            // due north to north pole produces north pole
            Assert.True(GeoCoord.geoAlmostEqual(expected, out_gc));

            // Due north to south pole, which doesn't get wrapped correctly
            GeoCoord.setGeoDegs(ref start, 45, 1);
            GeoCoord.setGeoDegs(ref expected, 270, 1);
            GeoCoord._geoAzDistanceRads(ref start, 0, GeoCoord.degsToRads(45 + 180), ref out_gc);
            Assert.True(
                 GeoCoord.geoAlmostEqual(expected, out_gc),
                 "due north to south pole produces south pole"
                );

            // Due south to south pole
            GeoCoord.setGeoDegs(ref start, -45, 2);
            GeoCoord.setGeoDegs(ref expected, -90, 0);
            GeoCoord._geoAzDistanceRads(ref start, GeoCoord.degsToRads(180), GeoCoord.degsToRads(45), ref out_gc);
            Assert.True(
                 GeoCoord.geoAlmostEqual(expected, out_gc),
                 "due south to south pole produces south pole"
                );

            // Due north to non-pole
            GeoCoord.setGeoDegs(ref start, -45, 10);
            GeoCoord.setGeoDegs(ref expected, -10, 10);
            GeoCoord._geoAzDistanceRads(ref start, 0, GeoCoord.degsToRads(35), ref out_gc);
            Assert.True(
                 GeoCoord.geoAlmostEqual(expected, out_gc),
                 "due north produces expected result"
                );
        }

        [Test]
        public void _geoAzDistanceRads_poleToPole()
        {
            GeoCoord start = new GeoCoord();
            GeoCoord out_gc = new GeoCoord();
            GeoCoord expected = new GeoCoord();

            // Azimuth doesn't really matter in this case. Any azimuth from the
            // north pole is south, any azimuth from the south pole is north.

            GeoCoord.setGeoDegs(ref start, 90, 0);
            GeoCoord.setGeoDegs(ref expected, -90, 0);
            GeoCoord._geoAzDistanceRads(ref start, GeoCoord.degsToRads(12), GeoCoord.degsToRads(180), ref out_gc);
            Assert.True
                (
                 GeoCoord.geoAlmostEqual(expected, out_gc),
                 "some direction to south pole produces south pole"
                );

            GeoCoord.setGeoDegs(ref start, -90, 0);
            GeoCoord.setGeoDegs(ref expected, 90, 0);
            GeoCoord._geoAzDistanceRads(ref start, GeoCoord.degsToRads(34), GeoCoord.degsToRads(180), ref out_gc);
            Assert.True
                (
                 GeoCoord.geoAlmostEqual(expected, out_gc),
                 "some direction to north pole produces north pole"
                );
        }

        [Test]
        public void _geoAzDistanceRads_invertible()
        {
            GeoCoord start = new GeoCoord();
            GeoCoord.setGeoDegs(ref start, 15, 10);
            GeoCoord out_gc= new GeoCoord();

            double azimuth = GeoCoord.degsToRads(20);
            double degrees180 = GeoCoord.degsToRads(180);
            double distance = GeoCoord.degsToRads(15);

            GeoCoord._geoAzDistanceRads(ref start, azimuth, distance, ref out_gc);
            Assert.True
                (
                 Math.Abs(GeoCoord._geoDistRads(start, out_gc) - distance) < Constants.EPSILON_RAD,
                 "moved distance is as expected"
                );

            GeoCoord start2 = new GeoCoord(out_gc.lat, out_gc.lon);
            GeoCoord._geoAzDistanceRads(ref start2, azimuth + degrees180, distance, ref out_gc);
            // TODO: Epsilon is relatively large
            Assert.True(GeoCoord._geoDistRads(start, out_gc) < 0.01, "moved back to origin");
        }

        [Test]
        public void doubleConstants()
        {
            // Simple checks for ordering of values
            testDecreasingFunction(GeoCoord.hexAreaKm2, "hexAreaKm2 ordering");
            testDecreasingFunction(GeoCoord.hexAreaM2, "hexAreaM2 ordering");
            testDecreasingFunction
                (
                 GeoCoord.edgeLengthKm,
                 "edgeLengthKm ordering"
                );
            testDecreasingFunction(GeoCoord.edgeLengthM, "edgeLengthM ordering");
        }

        [Test]
        public void intConstants()
        {
            // Simple checks for ordering of values
            long last = 0;
            long next;
            for (int i = 0; i <= Constants.MAX_H3_RES; i++)
            {
                next = GeoCoord.numHexagons(i);
                Assert.True(next > last, "numHexagons ordering");
                last = next;
            }
        }
    }
}
