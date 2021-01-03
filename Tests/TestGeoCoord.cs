using System;
using H3Lib;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class TestGeoCoord
    {
        /// <summary>
        /// Test a function for all resolutions, where the value should be
        /// decreasing as resolution increases.
        /// </summary>
        /// <param name="function">Function to process, takes an int, returns a double</param>
        private static void TestDecreasingFunction(Func<int, double> function)
        {
            double last = 0;
            for (int i = Constants.MAX_H3_RES; i >= 0; i--)
            {
                double next = function(i);
                Assert.Greater(next, last);
                last = next;
            }
        }
        [Test]
        // radsToDegs/degsToRads invertible
        public void RadsToDegs()
        {
            const double originalRads = 1.0;
            double degs = GeoCoord.RadiansToDegrees(originalRads);
            double rads = GeoCoord.DegreesToRadians(degs);
            // radsToDegs/degsToRads invertible
            Assert.Less(Math.Abs(rads - originalRads), Constants.EPSILON_RAD);
        }

        [Test]
        public void PointDistRads()
        {
            var p1 = new GeoCoord();
            GeoCoord.setGeoDegs(ref p1, 10, 10);
            var p2 = new GeoCoord();
            GeoCoord.setGeoDegs(ref p2, 0, 10);

            
            // TODO: Epsilon is relatively large
            // 0 distance as expected
            Assert.Less(GeoCoord.PointDistRads(p1, p1), Constants.EPSILON_RAD * 1000);
            // distance along longitude as expected
            Assert.Less(GeoCoord.PointDistRads(p1, p2) - GeoCoord.DegreesToRadians(10), Constants.EPSILON_RAD * 1000);
        }

        [Test]
        public void GeoAlmostEqualThreshold()
        {
            var a = new GeoCoord(15, 10);
            var b = new GeoCoord(15, 10);
            //  same point
            Assert.True(GeoCoord.geoAlmostEqualThreshold(a,b,double.Epsilon));

            b.Latitude = 15.00001;
            b.Longitude = 10.00002;
            //  differences under threshold
            Assert.True(GeoCoord.geoAlmostEqualThreshold(a,b,0.0001));

            b.Latitude = 15.00001;
            b.Longitude = 10;
            // longitude over threshold
            Assert.False(GeoCoord.geoAlmostEqualThreshold(a,b,0.000001));
        }
        
        [Test]
        public void ConstrainLatLng()
        {
            const double tolerance = Constants.EPSILON;
            // lat 0
            Assert.AreEqual(GeoCoord.ConstrainLatitude(0), 0);
            // lat 1
            Assert.AreEqual(GeoCoord.ConstrainLatitude(1), 1);
            // lat pi / 2
            Assert.AreEqual(GeoCoord.ConstrainLatitude(Constants.M_PI_2), Constants.M_PI_2);
            // lat pi
            Assert.AreEqual(GeoCoord.ConstrainLatitude(Constants.M_PI), 0);
            // lat pi + 1
            Assert.AreEqual(GeoCoord.ConstrainLatitude(Constants.M_PI + 1), 1);
            // lat 2 pi + 1
            Assert.AreEqual(GeoCoord.ConstrainLatitude(2 * Constants.M_PI + 1), 1);

            // lng 0
            Assert.AreEqual(GeoCoord.ConstrainLongitude(0), 0);
            // lng 1
            Assert.AreEqual(GeoCoord.ConstrainLongitude(1), 1);
            // lng pi
            Assert.AreEqual(GeoCoord.ConstrainLongitude(Constants.M_PI), Constants.M_PI);
            // lng 2 pi
            Assert.AreEqual(GeoCoord.ConstrainLongitude(2 * Constants.M_PI), 0);
            // lng 3 pi
            Assert.AreEqual(GeoCoord.ConstrainLongitude(3 * Constants.M_PI), Constants.M_PI);
            // lng 4 pi
            Assert.AreEqual(GeoCoord.ConstrainLongitude(4 * Constants.M_PI), 0);
        }

        [Test]
        public void GeoAzDistanceRadsNoop()
        {
            var start = new GeoCoord(15, 10);
            var outGc = new GeoCoord();
            var expected = new GeoCoord(15, 10);

            GeoCoord._geoAzDistanceRads(ref start, 0, 0, ref outGc);
            // 0 distance produces same point
            Assert.True(GeoCoord.geoAlmostEqual(expected, outGc));
        }

        [Test]
        public void GeoAzDistanceRadsDueNorthSouth()
        {
            var start = new GeoCoord();
            var outGc = new GeoCoord();
            var expected = new GeoCoord();

            // Due north to north pole
            GeoCoord.setGeoDegs(ref start, 45, 1);
            GeoCoord.setGeoDegs(ref expected, 90, 0);
            GeoCoord._geoAzDistanceRads(ref start, 0, GeoCoord.DegreesToRadians(45), ref outGc);
            // due north to north pole produces north pole
            Assert.True(GeoCoord.geoAlmostEqual(expected, outGc));

            // Due north to south pole, which doesn't get wrapped correctly
            GeoCoord.setGeoDegs(ref start, 45, 1);
            GeoCoord.setGeoDegs(ref expected, 270, 1);
            GeoCoord._geoAzDistanceRads(ref start, 0, GeoCoord.DegreesToRadians(45 + 180), ref outGc);
            // Due north to south pole produces south pole
            Assert.True(GeoCoord.geoAlmostEqual(expected, outGc));

            // Due south to south pole
            GeoCoord.setGeoDegs(ref start, -45, 2);
            GeoCoord.setGeoDegs(ref expected, -90, 0);
            GeoCoord._geoAzDistanceRads(ref start, GeoCoord.DegreesToRadians(180), GeoCoord.DegreesToRadians(45), ref outGc);
            // Due south to south pole produces south pole
            Assert.True(GeoCoord.geoAlmostEqual(expected, outGc));

            // Due north to non-pole
            GeoCoord.setGeoDegs(ref start, -45, 10);
            GeoCoord.setGeoDegs(ref expected, -10, 10);
            GeoCoord._geoAzDistanceRads(ref start, 0, GeoCoord.DegreesToRadians(35), ref outGc);
            // Due north produces expected result
            Assert.True(GeoCoord.geoAlmostEqual(expected, outGc));
        }

        [Test]
        public void GeoAzDistanceRadsPoleToPole()
        {
            var start = new GeoCoord();
            var outGc = new GeoCoord();
            var expected = new GeoCoord();

            // Azimuth doesn't really matter in this case. Any azimuth from the
            // north pole is south, any azimuth from the south pole is north.
            GeoCoord.setGeoDegs(ref start, 90, 0);
            GeoCoord.setGeoDegs(ref expected, -90, 0);
            GeoCoord._geoAzDistanceRads(ref start, GeoCoord.DegreesToRadians(12), GeoCoord.DegreesToRadians(180), ref outGc);
            // Some direction to south pole produces south pole
            Assert.True(GeoCoord.geoAlmostEqual(expected, outGc));

            GeoCoord.setGeoDegs(ref start, -90, 0);
            GeoCoord.setGeoDegs(ref expected, 90, 0);
            GeoCoord._geoAzDistanceRads(ref start, GeoCoord.DegreesToRadians(34), GeoCoord.DegreesToRadians(180), ref outGc);
            // some direction to north pole produces north pole
            Assert.True(GeoCoord.geoAlmostEqual(expected, outGc));
        }

        [Test]
        public void GeoAzDistanceRadsInvertible()
        {
            var start = new GeoCoord();
            GeoCoord.setGeoDegs(ref start, 15, 10);
            var outGc= new GeoCoord();

            double azimuth = GeoCoord.DegreesToRadians(20);
            double degrees180 = GeoCoord.DegreesToRadians(180);
            double distance = GeoCoord.DegreesToRadians(15);

            GeoCoord._geoAzDistanceRads(ref start, azimuth, distance, ref outGc);
            // Moved distance is as expected
            Assert.Less(Math.Abs(GeoCoord._geoDistRads(start, outGc) - distance), Constants.EPSILON_RAD);

            var start2 = new GeoCoord(outGc.Latitude, outGc.Longitude);
            GeoCoord._geoAzDistanceRads(ref start2, azimuth + degrees180, distance, ref outGc);
            // TODO: Epsilon is relatively large
            // Moved back to origin
            Assert.Less(GeoCoord._geoDistRads(start, outGc), 0.01);
        }

        [Test]
        public void PointDistRadsWrappedLongitude()
        {
            var negativeLongitude = new GeoCoord(0, -(Constants.M_PI + Constants.M_PI_2));
            var zero = new GeoCoord(0, 0);

            // Distance with wrapped longitude
            Assert.Less(Constants.M_PI_2 - GeoCoord.PointDistRads(negativeLongitude, zero), Constants.EPSILON_RAD);
            //Distance with wrapped longitude and swapped arguments
            Assert.Less(Constants.M_PI_2 - GeoCoord.PointDistRads(zero, negativeLongitude), Constants.EPSILON_RAD);
        }

        [Test]
        public void DoubleConstants()
        {
            // Simple checks for ordering of values
            // hexAreaKm2 ordering
            TestDecreasingFunction(GeoCoord.HexAreaKm2);
            // hexAreaM2 ordering
            TestDecreasingFunction(GeoCoord.HexAreaM2);
            // edgeLengthKm ordering
            TestDecreasingFunction(GeoCoord.EdgeLengthKm);
            // edgeLengthM ordering
            TestDecreasingFunction(GeoCoord.EdgeLengthM);
        }

        [Test]
        public void IntConstants()
        {
            // Simple checks for ordering of values
            long last = 0;
            for (var i = 0; i <= Constants.MAX_H3_RES; i++)
            {
                long next = GeoCoord.NumHexagons(i);
                // NumHexagons ordering
                Assert.Greater(next, last);
                last = next;
            }
        }

        [Test]
        public void NumHexagons()
        {
            var expected = new[]
                           {
                               122L,
                               842L,
                               5882L,
                               41162L,
                               288122L,
                               2016842L,
                               14117882L,
                               98825162L,
                               691776122L,
                               4842432842L,
                               33897029882L,
                               237279209162L,
                               1660954464122L,
                               11626681248842L,
                               81386768741882L,
                               569707381193162L
                           };

            for (var r = 0; r <= Constants.MAX_H3_RES; r++)
            {
                long num = GeoCoord.NumHexagons(r);
                Assert.AreEqual(num, expected[r]);
            }
        }
    }
    
}
