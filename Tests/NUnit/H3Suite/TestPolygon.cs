using System.Collections.Generic;
using System.Linq;
using H3Lib;
using H3Lib.Extensions;
using NUnit.Framework;
using GeoCoord=H3Lib.GeoCoord;

namespace TestSuite
{
    [TestFixture]
    public class TestPolygon
    {
        private static readonly GeoCoord[] SfVerts =
        {
            new GeoCoord(0.659966917655m, -2.1364398519396m), new GeoCoord(0.6595011102219m, -2.1359434279405m),
            new GeoCoord(0.6583348114025m, -2.1354884206045m), new GeoCoord(0.6581220034068m, -2.1382437718946m),
            new GeoCoord(0.6594479998527m, -2.1384597563896m), new GeoCoord(0.6599990002976m, -2.1376771158464m)
        };

        private static GeoCoord[] MakeGeoCoordArray(decimal[,] coords)
        {
            var results = new List<GeoCoord>();

            for (var gc = 0; gc < coords.GetLength(0); gc++)
            {
                results.Add(new GeoCoord(coords[gc,0], coords[gc,1]));
            }

            return results.ToArray();
        }

        private static BBox MakeBox(IReadOnlyList<decimal> directions)
        {
            return new BBox(directions[0], directions[1], directions[2], directions[3]);
        }
        
        private static LinkedGeoLoop CreateLinkedLoop(IEnumerable<GeoCoord> verts)
        {
            var loop = new LinkedGeoLoop();
            foreach (var geoCoord in verts)
            {
                loop.AddLinkedCoord(geoCoord);
            }

            return loop;
        }

        [Test]
        public void PointInsideGeofence()
        {
            var geofence = new GeoFence {NumVerts = 6, Verts = SfVerts};

            var inside = new GeoCoord(0.659m, -2.136m);
            var somewhere= new GeoCoord(1, 2);

            BBox bbox = geofence.ToBBox();

            Assert.IsFalse(geofence.PointInside(bbox, SfVerts[0]));
            Assert.IsTrue(geofence.PointInside(bbox, SfVerts[4]));
            Assert.IsTrue(geofence.PointInside(bbox, inside));
            Assert.IsFalse(geofence.PointInside(bbox, somewhere));
        }

        [Test]
        public void PointInsideGeofenceTransmeridian()
        {
            var verts = new[]
                        {
                            new GeoCoord(0.01m, -Constants.H3.M_PI + 0.01m),
                            new GeoCoord(0.01m, Constants.H3.M_PI - 0.01m),
                            new GeoCoord(-0.01m, Constants.H3.M_PI - 0.01m),
                            new GeoCoord(-0.01m, -Constants.H3.M_PI + 0.01m)
                        };

            var transMeridianGeofence = new GeoFence{NumVerts = 4, Verts = verts};

            var eastPoint = new GeoCoord(0.001m, -Constants.H3.M_PI + 0.001m);
            var eastPointOutside = new GeoCoord(0.001m, -Constants.H3.M_PI + 0.1m);
            var westPoint = new GeoCoord(0.001m, Constants.H3.M_PI - 0.001m);
            var westPointOutside =new GeoCoord(0.001m, Constants.H3.M_PI - 0.1m);

            var bbox = transMeridianGeofence.ToBBox();

            Assert.IsTrue(transMeridianGeofence.PointInside(bbox,westPoint));
            Assert.IsTrue(transMeridianGeofence.PointInside(bbox,eastPoint));
            Assert.IsFalse(transMeridianGeofence.PointInside(bbox,westPointOutside));
            Assert.IsFalse(transMeridianGeofence.PointInside(bbox,eastPointOutside));
        }

        [Test]
        public void PointInsideLinkedGeoLoop()
        {
            var somewhere = new GeoCoord(1, 2);
            var inside = new GeoCoord(0.659m, -2.136m);

            var loop = CreateLinkedLoop(SfVerts);

            var box = loop.ToBBox();

            Assert.IsTrue(loop.PointInside(box, inside));
            Assert.IsFalse(loop.PointInside(box, somewhere));

            loop.Clear();
        }

        [Test]
        public void BboxFromGeofence()
        {
            var verts = new[]
            {
                new GeoCoord(0.8m, 0.3m), new GeoCoord(0.7m, 0.6m),
                new GeoCoord(1.1m, 0.7m), new GeoCoord(1.0m, 0.2m)
            };
            
            var geofence = new GeoFence{NumVerts = 4, Verts = verts};

            var expected = new BBox(1.1m, 0.7m, 0.7m, 0.2m);

            BBox result = geofence.ToBBox();
            Assert.AreEqual(result, expected);
        }

        [Test]
        public void BboxFromGeofenceTransmeridian()
        {
            var verts =
                new[]
                {
                    new GeoCoord(0.1m, -Constants.H3.M_PI + 0.1m), new GeoCoord(0.1m, Constants.H3.M_PI - 0.1m),
                    new GeoCoord(0.05m, Constants.H3.M_PI - 0.2m), new GeoCoord(-0.1m, Constants.H3.M_PI - 0.1m),
                    new GeoCoord(-0.1m, -Constants.H3.M_PI + 0.1m), new GeoCoord(-0.05m, -Constants.H3.M_PI + 0.2m)
                };

            var geofence = new GeoFence {NumVerts = 6, Verts = verts};

            var expected = new BBox( 0.1m, -0.1m, -Constants.H3.M_PI + 0.2m, Constants.H3.M_PI - 0.2m);

            var result = geofence.ToBBox();
            Assert.AreEqual(result, expected);
        }

        [Test]
        public void BboxFromGeofenceNoVertices()
        {
            var geofence = new GeoFence();

            var expected = new BBox(0.0m, 0.0m, 0.0m, 0.0m);

            var result = geofence.ToBBox();
            Assert.AreEqual(result, expected);
        }

        [Test]
        public void BboxesFromGeoPolygon()
        {
            var verts = new[]
                        {
                            new GeoCoord(0.8m, 0.3m), new GeoCoord(0.7m, 0.6m),
                            new GeoCoord(1.1m, 0.7m), new GeoCoord(1.0m, 0.2m)
                        };

            var geofence = new GeoFence{NumVerts = 4, Verts = verts};
            var  polygon = new GeoPolygon {GeoFence = geofence, NumHoles = 0};

            var expected = new BBox(1.1m, 0.7m, 0.7m, 0.2m);

            var result = polygon.ToBBoxes();
            Assert.AreEqual(result[0], expected);
        }

        [Test]
        public void BboxesFromGeoPolygonHole()
        {
            var verts = new[]
                        {
                            new GeoCoord(0.8m, 0.3m), new GeoCoord(0.7m, 0.6m),
                            new GeoCoord(1.1m, 0.7m), new GeoCoord(1.0m, 0.2m)
                        };

            var geofence = new GeoFence {NumVerts = 4, Verts = verts};

            // not a real hole, but doesn't matter for the test
            var holeVerts = new[]
                            {
                                new GeoCoord(0.9m, 0.3m), new GeoCoord(0.9m, 0.5m),
                                new GeoCoord(1.0m, 0.7m), new GeoCoord(0.9m, 0.3m)
                            };

            var holeGeofence = new GeoFence {NumVerts = 4, Verts = holeVerts};

            var polygon =
                new GeoPolygon
                {
                    GeoFence = geofence,
                    NumHoles = 1,
                    Holes = new List<GeoFence> {holeGeofence}
                };

            var expected = new BBox(1.1m, 0.7m, 0.7m, 0.2m);
            var expectedHole = new BBox(1.0m, 0.9m, 0.7m, 0.3m);

            var result = polygon.ToBBoxes();

            Assert.AreEqual(result[0], expected);
            Assert.AreEqual(result[1],expectedHole);
        }

        [Test]
        public void BboxFromLinkedGeoLoop()
        {
            var verts = new[]
                        {
                            new GeoCoord(0.8m, 0.3m), new GeoCoord(0.7m, 0.6m),
                            new GeoCoord(1.1m, 0.7m), new GeoCoord(1.0m, 0.2m),
                        };

            var loop = CreateLinkedLoop(verts);

            var expected = new BBox(1.1m, 0.7m, 0.7m, 0.2m);

            var result = loop.ToBBox();
            Assert.AreEqual(result, expected);

            loop.Clear();
        }

        [Test]
        public void BboxFromLinkedGeoLoopNoVertices()
        {
            var loop = new LinkedGeoLoop();
            var expected = new BBox(0.0m, 0.0m, 0.0m, 0.0m);

            var result = loop.ToBBox();
            Assert.AreEqual(result,expected);

            loop.Clear();
        }

        [Test]
        public void IsClockwiseGeofence()
        {
            var verts = new[]
                        {
                            new GeoCoord(0m,0m), new GeoCoord(0.1m,0.1m),
                            new GeoCoord(0m,0.1m)
                        };

            var geofence = new GeoFence {NumVerts = 3, Verts = verts};

            Assert.IsTrue(geofence.IsClockwise());
        }

        [Test]
        public void IsClockwiseLinkedGeoLoop()
        {
            var verts = MakeGeoCoordArray(new[,]{{0.1m, 0.1m}, {0.2m, 0.2m}, {0.1m, 0.2m}});
            var loop = CreateLinkedLoop(verts);
            Assert.IsTrue(loop.IsClockwise());
            loop.Clear();
        }

        [Test]
        public void IsNotClockwiseLinkedGeoLoop()
        {
            var verts = MakeGeoCoordArray(new[,] {{0m, 0m}, {0m, 0.4m}, {0.4m, 0.4m}, {0.4m, 0m}});
            var loop = CreateLinkedLoop(verts);
            Assert.IsFalse(loop.IsClockwise());
            loop.Clear();
        }

        [Test]
        public void IsClockwiseGeofenceTransmeridian()
        {
            decimal[,] raw = {
                                {0.4m, Constants.H3.M_PI - 0.1m},
                                {0.4m, -Constants.H3.M_PI + 0.1m},
                                {-0.4m, -Constants.H3.M_PI + 0.1m},
                                {-0.4m, Constants.H3.M_PI - 0.1m}
                            };
            var verts = MakeGeoCoordArray(raw);
            var geofence = new GeoFence{NumVerts = 4, Verts = verts};

            Assert.IsTrue(geofence.IsClockwise());
        }

        [Test]
        public void IsClockwiseLinkedGeoLoopTransmeridian()
        {
            decimal[,] raw =
            {
                {0.4m, Constants.H3.M_PI - 0.1m},
                {0.4m, -Constants.H3.M_PI + 0.1m},
                {-0.4m, -Constants.H3.M_PI + 0.1m},
                {-0.4m, Constants.H3.M_PI - 0.1m}
            };

            var verts = MakeGeoCoordArray(raw);
            var loop = CreateLinkedLoop(verts);

            Assert.IsTrue(loop.IsClockwise());
            loop.Clear();
        }

        [Test]
        public void IsNotClockwiseLinkedGeoLoopTransmeridian()
        {
            decimal[,] raw =
            {
                {0.4m, Constants.H3.M_PI - 0.1m},
                {-0.4m, Constants.H3.M_PI - 0.1m},
                {-0.4m, -Constants.H3.M_PI + 0.1m},
                {0.4m, -Constants.H3.M_PI + 0.1m}
            };

            var verts = MakeGeoCoordArray(raw);
            var loop = CreateLinkedLoop(verts);

            Assert.IsFalse(loop.IsClockwise());

            loop.Clear();
        }

        [Test]
        public void NormalizeMultiPolygonSingle()
        {
            var verts = MakeGeoCoordArray(new decimal[,] {{0, 0}, {0, 1}, {1, 1}});

            var outer = CreateLinkedLoop(verts);

            var polygon = new LinkedGeoPolygon();
            polygon.AddLinkedLoop(outer);

            int result;
            (result, polygon) = polygon.NormalizeMultiPolygon();

            Assert.AreEqual(result, Constants.LinkedGeo.NormalizationSuccess);
            Assert.AreEqual(polygon.CountPolygons, 1);
            Assert.AreEqual(polygon.CountLoops, 1);
            if (polygon.Loops.First() != null)
            {
                Assert.AreEqual(polygon.Loops.First(), outer);
            }

            polygon.Clear();
        }

        [Test]
        public void NormalizeMultiPolygonTwoOuterLoops()
        {
            var verts1 = MakeGeoCoordArray(new decimal[,] {{0, 0}, {0, 1}, {1, 1}});
            var outer1= CreateLinkedLoop(verts1);

            var verts2 = MakeGeoCoordArray(new decimal[,] {{2, 2}, {2, 3}, {3, 3}});
            var outer2 = CreateLinkedLoop(verts2);

            var polygon = new LinkedGeoPolygon();
            polygon.AddLinkedLoop(outer1);
            polygon.AddLinkedLoop(outer2);

            int result;
            (result, polygon) = polygon.NormalizeMultiPolygon();

            Assert.AreEqual(result, Constants.LinkedGeo.NormalizationSuccess);
            Assert.AreEqual(polygon.CountPolygons, 2);
            Assert.AreEqual(polygon.CountLoops, 1);
            Assert.AreEqual(polygon.Next.CountLoops, 1);

            polygon.Clear();
        }

        [Test]
        public void NormalizeMultiPolygonOneHole()
        {
            var verts = MakeGeoCoordArray(new decimal[,] {{0, 0}, {0, 3}, {3, 3}, {3, 0}});
            var outer = CreateLinkedLoop(verts);

            var verts2 = MakeGeoCoordArray(new decimal[,] {{1, 1}, {2, 2}, {1, 2}});
            var inner = CreateLinkedLoop(verts2);

            var polygon = new LinkedGeoPolygon();
            polygon.AddLinkedLoop(inner);
            polygon.AddLinkedLoop(outer);

            int result;
            (result, polygon) = polygon.NormalizeMultiPolygon();

            Assert.AreEqual(result, Constants.LinkedGeo.NormalizationSuccess);
            Assert.AreEqual(polygon.CountPolygons, 1);
            Assert.AreEqual(polygon.CountLoops, 2);
            
            Assert.IsNotNull(polygon.Loops.First());
            Assert.AreEqual(polygon.Loops.First(), outer);

            Assert.IsNotNull(polygon.Loops[1]);
            Assert.AreEqual(polygon.Loops[1], inner);

            polygon.Clear();
        }

        [Test]
        public void NormalizeMultiPolygonTwoHoles()
        {
            var verts = MakeGeoCoordArray(new decimal[,] {{0, 0}, {0, 0.4m}, {0.4m, 0.4m}, {0.4m, 0}});
            var outer = CreateLinkedLoop(verts);

            var verts2 = MakeGeoCoordArray(new decimal[,] {{0.1m, 0.1m}, {0.2m, 0.2m}, {0.1m, 0.2m}});
            var inner1 = CreateLinkedLoop(verts2);

            var verts3 = MakeGeoCoordArray(new decimal[,] {{0.2m, 0.2m}, {0.3m, 0.3m}, {0.2m, 0.3m}});
            var inner2 = CreateLinkedLoop(verts3);

            var polygon = new LinkedGeoPolygon();
            polygon.AddLinkedLoop(inner2);
            polygon.AddLinkedLoop(outer);
            polygon.AddLinkedLoop(inner1);

            int result;
            (result, polygon) = polygon.NormalizeMultiPolygon();

            Assert.AreEqual(result, Constants.LinkedGeo.NormalizationSuccess);
            Assert.AreEqual(polygon.CountPolygons, 1);
            
            Assert.IsNotNull(polygon.Loops.First());
            Assert.AreEqual(polygon.Loops.First(), outer);

            Assert.AreEqual(polygon.CountLoops, 3);

            polygon.Clear();
        }

        [Test]
        public void NormalizeMultiPolygonTwoDonuts()
        {
            var verts = MakeGeoCoordArray(new decimal[,] {{0, 0}, {0, 3}, {3, 3}, {3, 0}});
            var outer = CreateLinkedLoop(verts);

            var verts2 = MakeGeoCoordArray(new decimal[,] {{1, 1}, {2, 2}, {1, 2}});
            var inner = CreateLinkedLoop(verts2);

            var verts3 = MakeGeoCoordArray(new decimal[,] {{0, 0}, {0, -3}, {-3, -3}, {-3, 0}});
            var outer2 = CreateLinkedLoop(verts3);

            var verts4 = MakeGeoCoordArray(new decimal[,] {{-1, -1}, {-2, -2}, {-1, -2}});
            var inner2 = CreateLinkedLoop(verts4);

            var polygon = new LinkedGeoPolygon();
            polygon.AddLinkedLoop(inner2);
            polygon.AddLinkedLoop(inner);
            polygon.AddLinkedLoop(outer);
            polygon.AddLinkedLoop(outer2);

            int result;
            (result, polygon) = polygon.NormalizeMultiPolygon();

            Assert.AreEqual(result, Constants.LinkedGeo.NormalizationSuccess);
            Assert.AreEqual(polygon.CountPolygons, 2);
            Assert.AreEqual(polygon.CountLoops, 2);

            Assert.IsNotNull(polygon.Loops.First());
            Assert.AreEqual(polygon.Loops.First().Count, 4);

            Assert.IsNotNull(polygon.Loops[1]);
            Assert.AreEqual(polygon.Loops[1].Count, 3);

            Assert.AreEqual(polygon.Next.CountLoops, 2);
            
            Assert.IsNotNull(polygon.Next.Loops.First());
            Assert.AreEqual(polygon.Next.Loops.First().Count, 4);

            Assert.IsNotNull(polygon.Next.Loops[1]);
            Assert.AreEqual(polygon.Next.Loops[1].Count, 3);

            polygon.Clear();
        }

        [Test]
        public void NormalizeMultiPolygonNestedDonuts()
        {
            var verts = MakeGeoCoordArray(new[,] {{0.2m, 0.2m}, {0.2m, -0.2m}, {-0.2m, -0.2m}, {-0.2m, 0.2m}});
            var outer = CreateLinkedLoop(verts);

            var verts2 = MakeGeoCoordArray(new[,] {{0.1m, 0.1m}, {-0.1m, 0.1m}, {-0.1m, -0.1m}, {0.1m, -0.1m}});
            var inner = CreateLinkedLoop(verts2);

            var verts3 = MakeGeoCoordArray(new[,] {{0.6m, 0.6m}, {0.6m, -0.6m}, {-0.6m, -0.6m}, {-0.6m, 0.6m}});
            var outerBig = CreateLinkedLoop(verts3);

            var verts4 = MakeGeoCoordArray(new[,] {{0.5m, 0.5m}, {-0.5m, 0.5m}, {-0.5m, -0.5m}, {0.5m, -0.5m}});
            var innerBig = CreateLinkedLoop(verts4);

            var polygon = new LinkedGeoPolygon();
            polygon.AddLinkedLoop(inner);
            polygon.AddLinkedLoop(outerBig);
            polygon.AddLinkedLoop(innerBig);
            polygon.AddLinkedLoop(outer);

            int result;
            (result, polygon) = polygon.NormalizeMultiPolygon();

            Assert.AreEqual(result, Constants.LinkedGeo.NormalizationSuccess);
            Assert.AreEqual(polygon.CountPolygons, 2);
            Assert.AreEqual(polygon.CountLoops, 2);

            Assert.IsNotNull(polygon.Loops.First());
            Assert.AreEqual(polygon.Loops.First(), outerBig);

            Assert.IsNotNull(polygon.Loops[1]);
            Assert.AreEqual(polygon.Loops[1], innerBig);

            Assert.AreEqual(polygon.Next.CountLoops, 2);

            Assert.IsNotNull(polygon.Next.Loops.First());
            Assert.AreEqual(polygon.Next.Loops.First(), outer);

            Assert.IsNotNull(polygon.Next.Loops[1]);
            Assert.AreEqual(polygon.Next.Loops[1], inner);

            polygon.Clear();
        }

        [Test]
        public void NormalizeMultiPolygonNoOuterLoops()
        {
            var verts = MakeGeoCoordArray(new  decimal[,] {{0, 0}, {1, 1}, {0, 1}});
            var outer1 = CreateLinkedLoop(verts);

            var verts2 = MakeGeoCoordArray(new  decimal[,] {{2, 2}, {3, 3}, {2, 3}});
            var outer2 = CreateLinkedLoop(verts2);

            var polygon = new LinkedGeoPolygon();
            polygon.AddLinkedLoop(outer1);
            polygon.AddLinkedLoop(outer2);

            int result;
            (result, polygon) = polygon.NormalizeMultiPolygon();

            Assert.AreEqual(H3Lib.Constants.LinkedGeo.NormalizationErrUnassignedHoles, result);

            Assert.AreEqual(1, polygon.CountPolygons);
            Assert.AreEqual(0, polygon.CountLoops);

            //polygon.Clear();
        }

        [Test]
        public void NormalizeMultiPolygonAlreadyNormalized()
        {
            var verts1 = MakeGeoCoordArray(new  decimal[,] {{0, 0}, {0, 1}, {1, 1}});
            var outer1 = CreateLinkedLoop(verts1);

            var verts2 = MakeGeoCoordArray(new  decimal[,] {{2, 2}, {2, 3}, {3, 3}});
            var outer2 = CreateLinkedLoop(verts2);

            var polygon = new LinkedGeoPolygon();
            polygon.AddLinkedLoop(outer1);
            var next = polygon.AddNewLinkedGeoPolygon();
            next.AddLinkedLoop(outer2);

            // Should be a no-op
            int result;
            (result, polygon) = polygon.NormalizeMultiPolygon();

            Assert.AreEqual(H3Lib.Constants.LinkedGeo.NormalizationErrMultiplePolygons, result);

            Assert.AreEqual(2, polygon.CountPolygons);
            Assert.AreEqual(1, polygon.CountLoops);

            Assert.IsNotNull(polygon.Loops.First());
            Assert.AreEqual(outer1, polygon.Loops.First());

            Assert.AreEqual(1, polygon.Next.CountLoops);
            Assert.IsNotNull(polygon.Next.Loops.First());
            Assert.AreEqual(outer2, polygon.Next.Loops.First());

            polygon.Clear();
        }

        [Test]
        public void NormalizeMultiPolygonUnassignedHole()
        {
            var verts = MakeGeoCoordArray(new  decimal[,] {{0, 0}, {0, 1}, {1, 1}, {1, 0}});
            var outer = CreateLinkedLoop(verts);

            var verts2 = MakeGeoCoordArray(new  decimal[,] {{2, 2}, {3, 3}, {2, 3}});
            var inner = CreateLinkedLoop(verts2);

            var polygon = new LinkedGeoPolygon();
            polygon.AddLinkedLoop(inner);
            polygon.AddLinkedLoop(outer);

            int result;
            (result, polygon) = polygon.NormalizeMultiPolygon();

            Assert.AreEqual(H3Lib.Constants.LinkedGeo.NormalizationErrUnassignedHoles, result);
            polygon.Clear();
        }


    }
}
