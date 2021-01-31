using System.Collections.Generic;
using System.Linq;
using H3Lib;
using H3Lib.Extensions;
using NUnit.Framework;
using TestSuite.Lib;

namespace TestSuite
{
    [TestFixture]
    public class TestPolyfillReported
    {
        //  https://github.com/uber/h3-js/issues/76#issuecomment-561204505
        public void entireWorld()
        {
            
            // TODO: Fails for a single worldwide polygon
            var worldVerts = new List<GeoCoord>
                             {
                                 new GeoCoord(-Constants.H3.M_PI_2, -Constants.H3.M_PI),
                                 new GeoCoord(Constants.H3.M_PI_2, -Constants.H3.M_PI),
                                 new GeoCoord(Constants.H3.M_PI_2, 0),
                                 new GeoCoord(-Constants.H3.M_PI_2, 0),
                             };

            var worldGeofence = new GeoFence {NumVerts = 4, Verts = worldVerts.ToArray()};
            var worldGeoPolygon = new GeoPolygon {GeoFence = worldGeofence, NumHoles = 0};

            var worldVerts2 = new List<GeoCoord>
                              {
                                  new GeoCoord(-Constants.H3.M_PI_2, 0),
                                  new GeoCoord(Constants.H3.M_PI_2, 0),
                                  new GeoCoord(Constants.H3.M_PI_2, -Constants.H3.M_PI),
                                  new GeoCoord(-Constants.H3.M_PI_2, -Constants.H3.M_PI),
                              };

            var worldGeofence2 = new GeoFence {NumVerts = 4, Verts = worldVerts2.ToArray()};
            var worldGeoPolygon2 = new GeoPolygon {GeoFence = worldGeofence2, NumHoles = 0};

            for (int res = 0; res < 3; res++)
            {
                var polyfillOut = worldGeoPolygon.Polyfill(res);
                var actualNumHexagons = Utility.CountActualHexagons(polyfillOut);

                var polyfillOut2 = worldGeoPolygon2.Polyfill(res);
                var actualNumHexagons2 = Utility.CountActualHexagons(polyfillOut2);

                Assert.AreEqual(res.NumHexagons(), actualNumHexagons + actualNumHexagons2);

                
                // Sets should be disjoint
                foreach (var fill1 in polyfillOut)
                {
                    if (fill1 == 0)
                    {
                        continue;
                    }
                    bool found = polyfillOut2.Any(fill2 => fill1 == fill2);
                    Assert.IsFalse(found);
                }

                polyfillOut.Clear();
                polyfillOut2.Clear();
            }
        }
        
        // https://github.com/uber/h3-js/issues/67
        [Test]
        public void h3js_67()
        {
            decimal east = (-56.25m).DegreesToRadians();
            decimal north = (-33.13755119234615m).DegreesToRadians();
            decimal south = (-34.30714385628804m).DegreesToRadians();
            decimal west = (-57.65625m).DegreesToRadians();

            var testVerts = new[]
                            {
                                new GeoCoord(north, east),
                                new GeoCoord(south, east),
                                new GeoCoord(south, west),
                                new GeoCoord(north, west),
                            };

            var testGeoFence = new GeoFence {NumVerts = 4, Verts = testVerts};
            var testPolygon = new GeoPolygon {GeoFence = testGeoFence, NumHoles = 0};

            const int res = 7;
            var hexagons = testPolygon.Polyfill(res);
            int actualNumHexagons = Utility.CountActualHexagons(hexagons);

            Assert.AreEqual(4499, actualNumHexagons);
        }

        // 2nd test case from h3-js#67
        [Test]
        public void h3js_67_2nd()
        {
            decimal east = (-57.65625m).DegreesToRadians();
            decimal north = (-34.30714385628804m).DegreesToRadians();
            decimal south = (-35.4606699514953m).DegreesToRadians();
            decimal west = (-59.0625m).DegreesToRadians();

            var testVerts = new[]
                            {
                                new GeoCoord(north, east),
                                new GeoCoord(south, east),
                                new GeoCoord(south, west),
                                new GeoCoord(north, west),
                            };

            var testGeoFence = new GeoFence {NumVerts = 4, Verts = testVerts};
            var testPolygon = new GeoPolygon {GeoFence = testGeoFence, NumHoles = 0};

            const int res = 7;
            var hexagons = testPolygon.Polyfill(res);
            int actualNumHexagons = Utility.CountActualHexagons(hexagons);

            Assert.AreEqual(4609, actualNumHexagons);
        }

        // https://github.com/uber/h3/issues/136
        [Test]
        public void h3_136()
        {
            var testVerts = new[]
                            {
                                new GeoCoord(0.10068990369902957m, 0.8920772174196191m),
                                new GeoCoord(0.10032914690616246m, 0.8915914753447348m),
                                new GeoCoord(0.10033349237998787m, 0.8915860128746426m),
                                new GeoCoord(0.10069496685903621m, 0.8920742194546231m)
                            };

            var testGeoFence = new GeoFence{NumVerts = 4, Verts = testVerts};
            var testPolygon = new GeoPolygon {GeoFence = testGeoFence, NumHoles = 0};

            const int res = 13;
            var hexagons = testPolygon.Polyfill(res);
            int actualNumHexagons = Utility.CountActualHexagons(hexagons);

            Assert.AreEqual(4355,actualNumHexagons);
        }
    }
}
