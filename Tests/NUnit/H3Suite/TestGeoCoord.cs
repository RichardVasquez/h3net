using System;
using H3Lib;
using H3Lib.Extensions;
using NUnit.Framework;

namespace TestSuite
{
    [TestFixture]
    public class TestGeoCoord
    {
        private static void TestDecreasingFunction(Func<int,decimal> function)
        {
            decimal last = 0;
            decimal next;
            for (int i = Constants.H3.MAX_H3_RES; i >= 0; i--)
            {
                next = function(i);
                Assert.Greater(next, last);
                last = next;
            }
        }

        [Test]
        public void RadsToDegs()
        {
            decimal originalRads = 1;
            decimal degs = originalRads.RadiansToDegrees();
            decimal rads = degs.DegreesToRadians();
            Assert.Less(rads-originalRads, Constants.H3.EPSILON_RAD);
        }

        [Test]
        public void PointDistRads()
        {
            var p1 = new GeoCoord().SetDegrees(10, 10);
            var p2 = new GeoCoord().SetDegrees(0, 10);

            // TODO: Epsilon is relatively large
            Assert.Less(p1.DistanceToRadians(p1),Constants.H3.EPSILON_RAD * 1000);
            Assert.Less(p1.DistanceToRadians(p2) - (10.0m).DegreesToRadians(), Constants.H3.EPSILON_RAD * 1000);
        }

        [Test]
        public void GeoAlmostEqualThreshold()
        {
            var a = new GeoCoord(15, 10);
            var b = new GeoCoord(15, 10);

            Assert.AreEqual(a, b);

            b = new GeoCoord(15.00001m, 10.00002m);
            Assert.AreNotEqual(a, b);

            b = new GeoCoord(15.00001m, 10);
            Assert.AreNotEqual(a, b);

            b = new GeoCoord(15, 10.00001m);
            Assert.AreNotEqual(a,b);
        }

        [Test]
        public void ConstrainLatLng()
        {
            Assert.AreEqual(0.ConstrainLatitude(), 0);
            Assert.AreEqual(1.ConstrainLatitude(), 1);
            Assert.AreEqual(Constants.H3.M_PI_2.ConstrainLatitude(), Constants.H3.M_PI_2);
            Assert.AreEqual(Constants.H3.M_PI.ConstrainLatitude(), 0);
            Assert.AreEqual((Constants.H3.M_PI + 1).ConstrainLatitude(), 1);
            Assert.AreEqual((2 * Constants.H3.M_PI + 1).ConstrainLatitude(), 1);

            Assert.AreEqual(0.ConstrainLongitude(), 0);
            Assert.AreEqual(1.ConstrainLongitude(), 1);
            Assert.AreEqual(Constants.H3.M_PI.ConstrainLongitude(), Constants.H3.M_PI);
            Assert.AreEqual((2 * Constants.H3.M_PI).ConstrainLongitude(), 0);
            Assert.AreEqual((3 * Constants.H3.M_PI).ConstrainLongitude(), Constants.H3.M_PI);
            Assert.AreEqual((4 * Constants.H3.M_PI).ConstrainLongitude(), 0);
        }

        [Test]
        public void GeoAzDistanceRadsNoop()
        {
            var start = new GeoCoord(15, 10);
            var expected = new GeoCoord(15, 10);

            var outCoord = start.GetAzimuthDistancePoint(0, 0);
            Assert.AreEqual(outCoord, expected);
        }

        [Test]
        public void GeoAzDistanceRadsDueNorthSouth()
        {
            GeoCoord start = default;

            start = start.SetDegrees(45, 1);
            var expected = start.SetDegrees(90,0);
            var outCoord = start.GetAzimuthDistancePoint(0, 45.DegreesToRadians());
            Assert.AreEqual(outCoord,expected);

            // Due north to south pole, which doesn't get wrapped correctly
            start = start.SetDegrees(45, 1);
            expected = expected.SetDegrees(270, 1);
            outCoord = start.GetAzimuthDistancePoint(0, (45 + 180).DegreesToRadians());
            Assert.AreEqual(outCoord, expected);
            
            // Due south to south pole
            start = start.SetDegrees(-45, 2);
            expected = expected.SetDegrees(-90, 0);
            outCoord = start.GetAzimuthDistancePoint(180.DegreesToRadians(), 45.DegreesToRadians());
            Assert.AreEqual(outCoord,expected);

            // Due north to non-pole
            start = start.SetDegrees(-45, 10);
            expected = expected.SetDegrees(-10, 10);
            outCoord = start.GetAzimuthDistancePoint(0, 35.DegreesToRadians());
            Assert.AreEqual(outCoord,expected);
        }

        [Test]
        public void GeoAzDistanceRadsPoleToPole()
        {
            GeoCoord start = default;
            GeoCoord expected = default;

            // Azimuth doesn't really matter in this case. Any azimuth from the
            // north pole is south, any azimuth from the south pole is north.

            start = start.SetDegrees(90, 0);
            expected = expected.SetDegrees(-90, 0);
            var outCoord = start.GetAzimuthDistancePoint(12.DegreesToRadians().ConstrainToPiAccuracy(), 180.DegreesToRadians().ConstrainToPiAccuracy());
            Assert.AreEqual(outCoord, expected);

            start = start.SetDegrees(-90, 0);
            expected = expected.SetDegrees(90, 0);
            outCoord = start.GetAzimuthDistancePoint(34.DegreesToRadians(), 180.DegreesToRadians());
            Assert.AreEqual(outCoord, expected);
        }

        [Test]
        public void GeoAzDistanceRadsInvertible()
        {
            GeoCoord start = default;
            start = start.SetDegrees(15, 10);
            GeoCoord outCoord = default;

            decimal azimuth = 20.DegreesToRadians();
            decimal degrees180 = 180.DegreesToRadians();
            decimal distance = 15.DegreesToRadians();

            outCoord = start.GetAzimuthDistancePoint(azimuth, distance);
            Assert.Less(Math.Abs(start.DistanceToRadians(outCoord) - distance), Constants.H3.EPSILON_RAD);

            var start2 = outCoord;
            outCoord = start2.GetAzimuthDistancePoint(azimuth + degrees180, distance);
            // TODO: Epsilon is relatively large
            Assert.Less(start.DistanceToRadians(outCoord), 0.01);
        }

        [Test]
        public void PointDistRadsWrappedLongitude()
        {
            var negativeLongitude = new GeoCoord(0, -(Constants.H3.M_PI + Constants.H3.M_PI_2));
            GeoCoord zero = default;

            Assert.Less(Math.Abs(Constants.H3.M_PI_2 - negativeLongitude.DistanceToRadians(zero)), Constants.H3.EPSILON_RAD);
            Assert.Less(Math.Abs(Constants.H3.M_PI_2 - zero.DistanceToRadians(negativeLongitude)), Constants.H3.EPSILON_RAD);
        }

        [Test]
        public void DoubleConstants()
        {
            // Simple checks for ordering of values
            TestDecreasingFunction(GeoCoord.HexAreaKm2);
            TestDecreasingFunction(GeoCoord.HexAreaM2);
            TestDecreasingFunction(GeoCoord.EdgeLengthKm);
            TestDecreasingFunction(GeoCoord.EdgeLengthM);
        }

        [Test]
        public void IntConstants()
        {
            // Simple checks for ordering of values
            long last = 0;
            long next;
            for (int i = 0; i <= Constants.H3.MAX_H3_RES; i++)
            {
                next = i.NumHexagons();
                Assert.Greater(next, last);
                last = next;
            }
        }

        [Test]
        public void NumHexagons()
        {
            // Test numHexagon counts of the number of *cells* at each resolution
            long[] expected =
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

            for (var r = 0; r <= Constants.H3.MAX_H3_RES; r++)
            {
                long num = r.NumHexagons();
                Assert.AreEqual(expected[r], num);
            }
        }
    }
}
