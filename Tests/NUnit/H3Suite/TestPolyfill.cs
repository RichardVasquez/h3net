using System.Collections.Generic;
using System.Linq;
using H3Lib;
using H3Lib.Extensions;
using NUnit.Framework;
using TestSuite.Lib;

namespace TestSuite
{
    [TestFixture]
    public class TestPolyfill
    {
        public static GeoCoord[] sfVerts =
        {
            new GeoCoord(0.659966917655, -2.1364398519396),
            new GeoCoord(0.6595011102219, -2.1359434279405),
            new GeoCoord(0.6583348114025, -2.1354884206045),
            new GeoCoord(0.6581220034068, -2.1382437718946),
            new GeoCoord(0.6594479998527, -2.1384597563896),
            new GeoCoord(0.6599990002976, -2.1376771158464)
        };

        public static GeoFence sfGeofence =
            new GeoFence {NumVerts = 6, Verts = sfVerts};

        public static GeoPolygon sfGeoPolygon = new GeoPolygon();

        public static GeoCoord[] holeVerts =
        {
            new GeoCoord(0.6595072188743, -2.1371053983433),
            new GeoCoord(0.6591482046471, -2.1373141048153),
            new GeoCoord(0.6592295020837, -2.1365222838402)
        };

        public static GeoFence holeGeofence =
            new GeoFence {NumVerts = 3, Verts = holeVerts};

        public static GeoPolygon holeGeoPolygon = new GeoPolygon();

        public static GeoCoord[] emptyVerts =
        {
            new GeoCoord(0.659966917655, -2.1364398519394),
            new GeoCoord(0.659966917655, -2.1364398519395),
            new GeoCoord(0.659966917655, -2.1364398519396)
        };

        public static GeoFence emptyGeofence =
            new GeoFence {NumVerts = 3, Verts = emptyVerts};

        public static GeoPolygon emptyGeoPolygon = new GeoPolygon();
        
        /// <summary>
        /// Return true if the cell crosses the meridian.
        /// </summary>
        private static bool IsTransmeridianCell(H3Index h)
        {
            GeoBoundary boundary = h.ToGeoBoundary();

            double minLon = Constants.H3.M_PI, maxLon = -Constants.H3.M_PI;
            for (int i = 0; i < boundary.NumVerts; i++)
            {
                if (boundary.Verts[i].Longitude < minLon)
                {
                    minLon = boundary.Verts[i].Longitude;
                }

                if (boundary.Verts[i].Longitude > maxLon)
                {
                    maxLon = boundary.Verts[i].Longitude;
                }
            }

            return maxLon - minLon > Constants.H3.M_PI - (Constants.H3.M_PI / 4);
        }
        
        private static void fillIndex_assertions(H3Index h)
        {
            if(IsTransmeridianCell(h))
            {
                // TODO: these do not work correctly
                return;
            }

            int currentRes = h.Resolution;
            // TODO: Not testing more than one depth because the assertions fail.
            for (int nextRes = currentRes; nextRes <= currentRes + 1; nextRes++)
            {
                GeoBoundary bndry = h.ToGeoBoundary();

                GeoPolygon polygon = new GeoPolygon()
                                     {
                                         GeoFence =
                                             new GeoFence
                                             {
                                                 NumVerts = bndry.NumVerts,
                                                 Verts = bndry.Verts.ToArray()
                                             },
                                         NumHoles = 0,
                                         Holes = new List<GeoFence>()
                                     };

                var polyfillOut = polygon.Polyfill(nextRes);
                int polyfillCount = Utility.CountActualHexagons(polyfillOut);

                var children = h.ToChildren(nextRes);
                int h3ToChildrenCount = Utility.CountActualHexagons(children);

                polyfillOut = polyfillOut.Where(p => p != H3Lib.Constants.H3Index.H3_NULL).ToList();
                polyfillOut.Sort();

                children = children.Where(p => p != H3Lib.Constants.H3Index.H3_NULL).ToList();
                children.Sort();

                Assert.AreEqual(h3ToChildrenCount, polyfillCount);

                bool match = true;
                for (int i = 0; i < children.Count; i++)
                {
                    if (children[i] != polyfillOut[i])
                    {
                        match = false;
                    }
                }
                
                Assert.IsTrue(match);
            }
        }

        [SetUp]
        public void Setup()
        {
            sfGeoPolygon.GeoFence = TestPolyfill.sfGeofence;
            sfGeoPolygon.NumHoles = 0;

            holeGeoPolygon.GeoFence = sfGeofence;
            holeGeoPolygon.NumHoles = 1;
            holeGeoPolygon.Holes = new List<GeoFence> {holeGeofence};

            emptyGeoPolygon.GeoFence = emptyGeofence;
            emptyGeoPolygon.NumHoles = 0;
        }

        [Test]
        public void MaxPolyfillSize()
        {
            int numHexagons = sfGeoPolygon.MaxPolyFillSize(9);
            Assert.AreEqual(5613, numHexagons);

            numHexagons = holeGeoPolygon.MaxPolyFillSize(9);
            Assert.AreEqual(5613, numHexagons);

            numHexagons = emptyGeoPolygon.MaxPolyFillSize(9);
            Assert.AreEqual(15,numHexagons);
        }

        [Test]
        public void Polyfill()
        {
            var hexagons = sfGeoPolygon.Polyfill(9);
            int actualNumHexagons = Utility.CountActualHexagons(hexagons);
            Assert.AreEqual(1253, actualNumHexagons);
        }

        [Test]
        public void PolyfillHole()
        {
            var hexagons = holeGeoPolygon.Polyfill(9);
            int actualNumHexagons = Utility.CountActualHexagons(hexagons);
            Assert.AreEqual(1214,actualNumHexagons);
        }

        [Test]
        public void PolyfillEmpty()
        {
            var hexagons = emptyGeoPolygon.Polyfill(9);
            var actualNumHexagons = Utility.CountActualHexagons(hexagons);
            Assert.AreEqual(0, actualNumHexagons);
        }

        [Test]
        public void PolyfillExact()
        {
            var somewhere = new GeoCoord(1, 2);
            var origin = somewhere.ToH3Index(9);
            var boundary = origin.ToGeoBoundary();

            var verts = new List<GeoCoord>();
            verts.AddRange(boundary.Verts.Take(boundary.NumVerts));
            verts.Add(boundary.Verts[0]);

            var someGeofence = new GeoFence {NumVerts = boundary.NumVerts + 1, Verts = verts.ToArray()};

            var someHexagon = new GeoPolygon {GeoFence = someGeofence, NumHoles = 0};

            var hexagons = someHexagon.Polyfill(9);
            var actualNumHexagons = Utility.CountActualHexagons(hexagons);
            Assert.AreEqual(1, actualNumHexagons);            
        }

        [Test]
        public void PolyfillTransmeridian()
        {
            var primeMeridianVerts = new[]
                                     {
                                         new GeoCoord(0.01, 0.01),
                                         new GeoCoord(0.01, -0.01),
                                         new GeoCoord(-0.01, -0.01),
                                         new GeoCoord(-0.01, 0.01)
                                     };
            var primeMeridianGeofence = new GeoFence {NumVerts = 4, Verts = primeMeridianVerts};
            var primeMeridianGeoPolygon = new GeoPolygon {GeoFence = primeMeridianGeofence, NumHoles = 0};

            var transMeridianVerts = new[]
                                     {
                                         new GeoCoord(0.01, -Constants.H3.M_PI + 0.01),
                                         new GeoCoord(0.01, Constants.H3.M_PI - 0.01),
                                         new GeoCoord(-0.01, Constants.H3.M_PI - 0.01),
                                         new GeoCoord(-0.01, -Constants.H3.M_PI + 0.01),
                                     };

            var transMeridianGeofence = new GeoFence {NumVerts = 4, Verts = transMeridianVerts};
            var transMeridianGeoPolygon = new GeoPolygon {GeoFence = transMeridianGeofence, NumHoles = 0};

            var transMeridianHoleVerts = new[]
                                         {
                                             new GeoCoord(0.005, -Constants.H3.M_PI + 0.005),
                                             new GeoCoord(0.005, Constants.H3.M_PI - 0.005),
                                             new GeoCoord(-0.005, Constants.H3.M_PI - 0.005),
                                             new GeoCoord(-0.005, -Constants.H3.M_PI + 0.005),
                                         };
            var transMeridianHoleGeofence = new GeoFence {NumVerts = 4, Verts = transMeridianHoleVerts};
            var transMeridianHoleGeoPolygon = new GeoPolygon
                                              {
                                                  GeoFence = transMeridianGeofence,
                                                  NumHoles = 1,
                                                  Holes = new List<GeoFence> {transMeridianHoleGeofence}
                                              };
            var transMeridianFilledHoleGeoPolygon = new GeoPolygon
                                                    {
                                                        GeoFence = transMeridianHoleGeofence,
                                                        NumHoles = 0
                                                    };

            int expectedSize = 4228;
            var hexagons = primeMeridianGeoPolygon.Polyfill(7);
            int actualNumHexagons = Utility.CountActualHexagons(hexagons);

            Assert.AreEqual(expectedSize, actualNumHexagons);
            
            // Transmeridian case
            // This doesn't exactly match the prime meridian count because of slight
            // differences in hex size and grid offset between the two cases

            expectedSize = 4238;

            var hexagonsTM = transMeridianGeoPolygon.Polyfill(7);
            actualNumHexagons = Utility.CountActualHexagons(hexagonsTM);
            Assert.AreEqual(expectedSize, actualNumHexagons);
            
            // Transmeridian filled hole case -- only needed for calculating hole size
            var hexagonsTMFH = transMeridianFilledHoleGeoPolygon.Polyfill(7);
            var actualNumHoleHexagons = Utility.CountActualHexagons(hexagonsTMFH);

            // Transmeridian hole case
            var hexagonsTMH = transMeridianHoleGeoPolygon.Polyfill(7);
            actualNumHexagons = Utility.CountActualHexagons(hexagonsTMH);

            Assert.AreEqual(expectedSize-actualNumHoleHexagons, actualNumHexagons);
        }

        [Test]
        public void PolyfillTransmeridianComplex()
        {
            // This polygon is "complex" in that it has > 4 vertices - this
            // tests for a bug that was taking the max and min longitude as
            // the bounds for transmeridian polygons
            var verts = new[]
                        {
                            new GeoCoord(0.1, -Constants.H3.M_PI + 0.00001),
                            new GeoCoord(0.1, Constants.H3.M_PI - 0.00001),
                            new GeoCoord(0.05, Constants.H3.M_PI - 0.2),
                            new GeoCoord(-0.1, Constants.H3.M_PI - 0.00001),
                            new GeoCoord(-0.1, -Constants.H3.M_PI + 0.00001),
                            new GeoCoord(-0.05, -Constants.H3.M_PI + 0.2),
                        };

            var geofence = new GeoFence {NumVerts = 6, Verts = verts};
            var polygon = new GeoPolygon {GeoFence = geofence, NumHoles = 0};

            var hexagons = polygon.Polyfill(4);
            int actualNumHexagons = Utility.CountActualHexagons(hexagons);
            Assert.AreEqual(1204,actualNumHexagons);
        }

        [Test]
        public void PolyfillPentagon()
        {
            H3Index pentagon = new H3Index(9, 24, 0);
            var coord = pentagon.ToGeoCoord();

            // Length of half an edge of the polygon, in radians
            double edgeLength2 = (0.001).DegreesToRadians();

            var boundingTopRight = coord
                                  .SetLatitude(coord.Latitude + edgeLength2)
                                  .SetLongitude(coord.Longitude + edgeLength2);

            var boundingTopLeft = coord
                                 .SetLatitude(coord.Latitude + edgeLength2)
                                 .SetLongitude(coord.Longitude - edgeLength2);

            var boundingBottomRight = coord
                                     .SetLatitude(coord.Latitude - edgeLength2)
                                     .SetLongitude(coord.Longitude + edgeLength2);

            var boundingBottomLeft = coord
                                    .SetLatitude(coord.Latitude - edgeLength2)
                                    .SetLongitude(coord.Longitude - edgeLength2);

            var verts = new[]
                        {
                            boundingBottomLeft, boundingTopLeft,
                            boundingTopRight, boundingBottomRight
                        };

            var geofence = new GeoFence {Verts = verts, NumVerts = 4};
            var polygon = new GeoPolygon {GeoFence = geofence, NumHoles = 0};

            var hexagons = polygon.Polyfill(9);

            Assert.AreEqual(1, hexagons.Count);
            Assert.IsTrue(hexagons.First().IsPentagon());
        }

        [Test]
        public void FillIndex()
        {
            Utility.IterateAllIndexesAtRes(0, fillIndex_assertions);
            Utility.IterateAllIndexesAtRes(1, fillIndex_assertions);
            Utility.IterateAllIndexesAtRes(2, fillIndex_assertions);
        }
    }
}
