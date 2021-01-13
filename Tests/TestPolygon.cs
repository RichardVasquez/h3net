using System.Collections.Generic;
using H3Lib;
using H3Lib.Extensions;
using NUnit.Framework;
using GeoCoord=H3Lib.GeoCoord;

namespace Tests
{
    [TestFixture]
    public class TestPolygon
    {
        private static readonly GeoCoord[] SfVerts =
        {
            new GeoCoord(0.659966917655, -2.1364398519396), new GeoCoord(0.6595011102219, -2.1359434279405),
            new GeoCoord(0.6583348114025, -2.1354884206045), new GeoCoord(0.6581220034068, -2.1382437718946),
            new GeoCoord(0.6594479998527, -2.1384597563896), new GeoCoord(0.6599990002976, -2.1376771158464)
        };

        private static GeoCoord[] MakeGeoCoordArray(double[,] coords)
        {
            var results = new List<GeoCoord>();

            for (var gc = 0; gc < coords.GetLength(0); gc++)
            {
                results.Add(new GeoCoord(coords[gc,0], coords[gc,1]));
            }

            return results.ToArray();
        }

        private static BBox MakeBox(IReadOnlyList<double> directions)
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

            var inside = new GeoCoord(0.659, -2.136);
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
                            new GeoCoord(0.01, -Constants.M_PI + 0.01),
                            new GeoCoord(0.01, Constants.M_PI - 0.01),
                            new GeoCoord(-0.01, Constants.M_PI - 0.01),
                            new GeoCoord(-0.01, -Constants.M_PI + 0.01)
                        };

            var transMeridianGeofence = new GeoFence{NumVerts = 4, Verts = verts};

            var eastPoint = new GeoCoord(0.001, -Constants.M_PI + 0.001);
            var eastPointOutside = new GeoCoord(0.001, -Constants.M_PI + 0.1);
            var westPoint = new GeoCoord(0.001, Constants.M_PI - 0.001);
            var westPointOutside =new GeoCoord(0.001, Constants.M_PI - 0.1);

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
            var inside = new GeoCoord(0.659, -2.136);

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
                new GeoCoord(0.8, 0.3), new GeoCoord(0.7, 0.6),
                new GeoCoord(1.1, 0.7), new GeoCoord(1.0, 0.2)
            };
            
            var geofence = new GeoFence{NumVerts = 4, Verts = verts};

            var expected = new BBox(1.1, 0.7, 0.7, 0.2);

            BBox result = geofence.ToBBox();
            Assert.AreEqual(result, expected);
        }

        [Test]
        public void BboxFromGeofenceTransmeridian()
        {
            var verts =
                new[]
                {
                    new GeoCoord(0.1, -Constants.M_PI + 0.1), new GeoCoord(0.1, Constants.M_PI - 0.1),
                    new GeoCoord(0.05, Constants.M_PI - 0.2), new GeoCoord(-0.1, Constants.M_PI - 0.1),
                    new GeoCoord(-0.1, -Constants.M_PI + 0.1), new GeoCoord(-0.05, -Constants.M_PI + 0.2)
                };

            var geofence = new GeoFence {NumVerts = 6, Verts = verts};

            var expected = new BBox( 0.1, -0.1, -Constants.M_PI + 0.2, Constants.M_PI - 0.2);

            var result = geofence.ToBBox();
            Assert.AreEqual(result, expected);
        }

        [Test]
        public void BboxFromGeofenceNoVertices()
        {
            var geofence = new GeoFence();

            var expected = new BBox(0.0, 0.0, 0.0, 0.0);

            var result = geofence.ToBBox();
            Assert.AreEqual(result, expected);
        }

        [Test]
        public void BboxesFromGeoPolygon()
        {
            var verts = new[]
                        {
                            new GeoCoord(0.8, 0.3), new GeoCoord(0.7, 0.6),
                            new GeoCoord(1.1, 0.7), new GeoCoord(1.0, 0.2)
                        };

            var geofence = new GeoFence{NumVerts = 4, Verts = verts};
            var  polygon = new GeoPolygon {GeoFence = geofence, NumHoles = 0};

            var expected = new BBox(1.1, 0.7, 0.7, 0.2);

            var result = polygon.ToBBoxes();
            Assert.AreEqual(result[0], expected);
        }

        [Test]
        public void BboxesFromGeoPolygonHole()
        {
            var verts = new[]
                        {
                            new GeoCoord(0.8, 0.3), new GeoCoord(0.7, 0.6),
                            new GeoCoord(1.1, 0.7), new GeoCoord(1.0, 0.2)
                        };

            var geofence = new GeoFence {NumVerts = 4, Verts = verts};

            // not a real hole, but doesn't matter for the test
            var holeVerts = new[]
                            {
                                new GeoCoord(0.9, 0.3), new GeoCoord(0.9, 0.5),
                                new GeoCoord(1.0, 0.7), new GeoCoord(0.9, 0.3)
                            };

            var holeGeofence = new GeoFence {NumVerts = 4, Verts = holeVerts};

            var polygon =
                new GeoPolygon
                {
                    GeoFence = geofence,
                    NumHoles = 1,
                    Holes = new List<GeoFence> {holeGeofence}
                };

            var expected = new BBox(1.1, 0.7, 0.7, 0.2);
            var expectedHole = new BBox(1.0, 0.9, 0.7, 0.3);

            //BBox* result = calloc(sizeof(BBox), 2);
            var result = polygon.ToBBoxes();

            Assert.AreEqual(result[0], expected);
            Assert.AreEqual(result[1],expectedHole);
        }

        [Test]
        public void BboxFromLinkedGeoLoop()
        {
            var verts = new[]
                        {
                            new GeoCoord(0.8, 0.3), new GeoCoord(0.7, 0.6),
                            new GeoCoord(1.1, 0.7), new GeoCoord(1.0, 0.2),
                        };

            var loop = CreateLinkedLoop(verts);

            var expected = new BBox(1.1, 0.7, 0.7, 0.2);

            var result = loop.ToBBox();
            Assert.AreEqual(result, expected);

            loop.Clear();
        }

        [Test]
        public void BboxFromLinkedGeoLoopNoVertices()
        {
            var loop = new LinkedGeoLoop();
            var expected = new BBox(0.0, 0.0, 0.0, 0.0);

            var result = loop.ToBBox();
            Assert.AreEqual(result,expected);

            loop.Clear();
        }

        [Test]
        public void IsClockwiseGeofence()
        {
            var verts = new[]
                        {
                            new GeoCoord(0,0), new GeoCoord(0.1,0.1),
                            new GeoCoord(0,0.1)
                        };

            var geofence = new GeoFence {NumVerts = 3, Verts = verts};

            Assert.IsTrue(geofence.IsClockwise());
        }

        [Test]
        public void IsClockwiseLinkedGeoLoop()
        {
            var verts = MakeGeoCoordArray(new[,]{{0.1, 0.1}, {0.2, 0.2}, {0.1, 0.2}});
            var loop = CreateLinkedLoop(verts);
            Assert.IsTrue(loop.IsClockwise());
            loop.Clear();
        }

        [Test]
        public void IsNotClockwiseLinkedGeoLoop()
        {
            var verts = MakeGeoCoordArray(new[,] {{0, 0}, {0, 0.4}, {0.4, 0.4}, {0.4, 0}});
            var loop = CreateLinkedLoop(verts);
            Assert.IsFalse(loop.IsClockwise());
            loop.Clear();
        }

        [Test]
        public void IsClockwiseGeofenceTransmeridian()
        {
            double[,] raw = {
                                {0.4, Constants.M_PI - 0.1},
                                {0.4, -Constants.M_PI + 0.1},
                                {-0.4, -Constants.M_PI + 0.1},
                                {-0.4, Constants.M_PI - 0.1}
                            };
            var verts = MakeGeoCoordArray(raw);
            var geofence = new GeoFence{NumVerts = 4, Verts = verts};

            Assert.IsTrue(geofence.IsClockwise());
        }

        [Test]
        public void IsClockwiseLinkedGeoLoopTransmeridian()
        {
            double[,] raw =
            {
                {0.4, Constants.M_PI - 0.1},
                {0.4, -Constants.M_PI + 0.1},
                {-0.4, -Constants.M_PI + 0.1},
                {-0.4, Constants.M_PI - 0.1}
            };

            var verts = MakeGeoCoordArray(raw);
            var loop = CreateLinkedLoop(verts);

            Assert.IsTrue(loop.IsClockwise());
            loop.Clear();
        }

        [Test]
        public void IsNotClockwiseLinkedGeoLoopTransmeridian()
        {
            double[,] raw =
            {
                {0.4, Constants.M_PI - 0.1},
                {-0.4, Constants.M_PI - 0.1},
                {-0.4, -Constants.M_PI + 0.1},
                {0.4, -Constants.M_PI + 0.1}
            };

            var verts = MakeGeoCoordArray(raw);
            var loop = CreateLinkedLoop(verts);

            Assert.IsFalse(loop.IsClockwise());

            loop.Clear();
        }

        [Test]
        public void NormalizeMultiPolygonSingle()
        {
            var verts = MakeGeoCoordArray(new double[,] {{0, 0}, {0, 1}, {1, 1}});

            var outer = CreateLinkedLoop(verts);

            var polygon = new LinkedGeoPolygon();
            polygon.AddLinkedLoop(outer);

            int result;
            (result, polygon) = polygon.NormalizeMultiPolygon();

            Assert.AreEqual(result, H3Lib.StaticData.LinkedGeo.NormalizationSuccess);
            Assert.AreEqual(polygon.CountPolygons(), 1);
            Assert.AreEqual(polygon.CountLoops(), 1);
            if (polygon.LinkedGeoList.First != null)
            {
                Assert.AreEqual(polygon.LinkedGeoList.First.Value, outer);
            }

            polygon.Clear();
        }

        [Test]
        public void NormalizeMultiPolygonTwoOuterLoops()
        {
            var verts1 = MakeGeoCoordArray(new double[,] {{0, 0}, {0, 1}, {1, 1}});
            var outer1= CreateLinkedLoop(verts1);

            var verts2 = MakeGeoCoordArray(new double[,] {{2, 2}, {2, 3}, {3, 3}});
            var outer2 = CreateLinkedLoop(verts2);

            var polygon = new LinkedGeoPolygon();
            polygon.AddLinkedLoop(outer1);
            polygon.AddLinkedLoop(outer2);

            int result;
            (result, polygon) = polygon.NormalizeMultiPolygon();

            Assert.AreEqual(result, H3Lib.StaticData.LinkedGeo.NormalizationSuccess);
            Assert.AreEqual(polygon.CountPolygons(), 2);
            Assert.AreEqual(polygon.CountLoops(), 1);
            Assert.AreEqual(polygon.Next.CountLoops(), 1);

            polygon.Clear();
        }

        [Test]
        public void NormalizeMultiPolygonOneHole()
        {
            var verts = MakeGeoCoordArray(new double[,] {{0, 0}, {0, 3}, {3, 3}, {3, 0}});
            var outer = CreateLinkedLoop(verts);

            var verts2 = MakeGeoCoordArray(new double[,] {{1, 1}, {2, 2}, {1, 2}});
            var inner = CreateLinkedLoop(verts2);

            var polygon = new LinkedGeoPolygon();
            polygon.AddLinkedLoop(inner);
            polygon.AddLinkedLoop(outer);

            int result;
            (result, polygon) = polygon.NormalizeMultiPolygon();

            Assert.AreEqual(result, H3Lib.StaticData.LinkedGeo.NormalizationSuccess);
            Assert.AreEqual(polygon.CountPolygons(), 1);
            Assert.AreEqual(polygon.CountLoops(), 2);
            
            Assert.IsNotNull(polygon.LinkedGeoList.First);
            Assert.AreEqual(polygon.LinkedGeoList.First.Value, outer);

            Assert.IsNotNull(polygon.LinkedGeoList.First?.Next);
            Assert.AreEqual(polygon.LinkedGeoList.First.Next.Value, inner);

            polygon.Clear();
        }

        [Test]
        public void NormalizeMultiPolygonTwoHoles()
        {
            var verts = MakeGeoCoordArray(new[,] {{0, 0}, {0, 0.4}, {0.4, 0.4}, {0.4, 0}});
            var outer = CreateLinkedLoop(verts);

            var verts2 = MakeGeoCoordArray(new[,] {{0.1, 0.1}, {0.2, 0.2}, {0.1, 0.2}});
            var inner1 = CreateLinkedLoop(verts2);

            var verts3 = MakeGeoCoordArray(new[,] {{0.2, 0.2}, {0.3, 0.3}, {0.2, 0.3}});
            var inner2 = CreateLinkedLoop(verts3);

            var polygon = new LinkedGeoPolygon();
            polygon.AddLinkedLoop(inner2);
            polygon.AddLinkedLoop(outer);
            polygon.AddLinkedLoop(inner1);

            int result;
            (result, polygon) = polygon.NormalizeMultiPolygon();

            Assert.AreEqual(result, H3Lib.StaticData.LinkedGeo.NormalizationSuccess);
            Assert.AreEqual(polygon.CountPolygons(), 1);
            
            Assert.IsNotNull(polygon.LinkedGeoList.First);
            Assert.AreEqual(polygon.LinkedGeoList.First.Value, outer);

            Assert.AreEqual(polygon.CountLoops(), 3);

            polygon.Clear();
        }

        [Test]
        public void NormalizeMultiPolygonTwoDonuts()
        {
            var verts = MakeGeoCoordArray(new double[,] {{0, 0}, {0, 3}, {3, 3}, {3, 0}});
            var outer = CreateLinkedLoop(verts);

            var verts2 = MakeGeoCoordArray(new double[,] {{1, 1}, {2, 2}, {1, 2}});
            var inner = CreateLinkedLoop(verts2);

            var verts3 = MakeGeoCoordArray(new double[,] {{0, 0}, {0, -3}, {-3, -3}, {-3, 0}});
            var outer2 = CreateLinkedLoop(verts3);

            var verts4 = MakeGeoCoordArray(new double[,] {{-1, -1}, {-2, -2}, {-1, -2}});
            var inner2 = CreateLinkedLoop(verts4);

            var polygon = new LinkedGeoPolygon();
            polygon.AddLinkedLoop(inner2);
            polygon.AddLinkedLoop(inner);
            polygon.AddLinkedLoop(outer);
            polygon.AddLinkedLoop(outer2);

            int result;
            (result, polygon) = polygon.NormalizeMultiPolygon();

            Assert.AreEqual(result, H3Lib.StaticData.LinkedGeo.NormalizationSuccess);
            Assert.AreEqual(polygon.CountPolygons(), 2);
            Assert.AreEqual(polygon.CountLoops(), 2);

            Assert.IsNotNull(polygon.LinkedGeoList.First);
            Assert.AreEqual(polygon.LinkedGeoList.First.Value.CountCoords, 4);

            Assert.IsNotNull(polygon.LinkedGeoList.First.Next);
            Assert.AreEqual(polygon.LinkedGeoList.First.Next.Value.CountCoords, 3);

            Assert.AreEqual(polygon.Next.CountLoops(), 2);
            
            Assert.IsNotNull(polygon.Next.LinkedGeoList.First);
            Assert.AreEqual(polygon.Next.LinkedGeoList.First.Value.CountCoords, 4);

            Assert.IsNotNull(polygon.Next.LinkedGeoList.First.Next);
            Assert.AreEqual(polygon.Next.LinkedGeoList.First.Next.Value.GeoCoordList.Count, 3);

            polygon.Clear();
        }

        [Test]
        public void NormalizeMultiPolygonNestedDonuts()
        {
            var verts = MakeGeoCoordArray(new[,] {{0.2, 0.2}, {0.2, -0.2}, {-0.2, -0.2}, {-0.2, 0.2}});
            var outer = CreateLinkedLoop(verts);

            var verts2 = MakeGeoCoordArray(new[,] {{0.1, 0.1}, {-0.1, 0.1}, {-0.1, -0.1}, {0.1, -0.1}});
            var inner = CreateLinkedLoop(verts2);

            var verts3 = MakeGeoCoordArray(new[,] {{0.6, 0.6}, {0.6, -0.6}, {-0.6, -0.6}, {-0.6, 0.6}});
            var outerBig = CreateLinkedLoop(verts3);

            var verts4 = MakeGeoCoordArray(new[,] {{0.5, 0.5}, {-0.5, 0.5}, {-0.5, -0.5}, {0.5, -0.5}});
            var innerBig = CreateLinkedLoop(verts4);

            var polygon = new LinkedGeoPolygon();
            polygon.AddLinkedLoop(inner);
            polygon.AddLinkedLoop(outerBig);
            polygon.AddLinkedLoop(innerBig);
            polygon.AddLinkedLoop(outer);

            int result;
            (result, polygon) = polygon.NormalizeMultiPolygon();

            Assert.AreEqual(result, H3Lib.StaticData.LinkedGeo.NormalizationSuccess);
            Assert.AreEqual(polygon.CountPolygons(), 2);
            Assert.AreEqual(polygon.CountLoops(), 2);

            Assert.IsNotNull(polygon.LinkedGeoList.First);
            Assert.AreEqual(polygon.LinkedGeoList.First.Value, outerBig);

            Assert.IsNotNull(polygon.LinkedGeoList.First.Next);
            Assert.AreEqual(polygon.LinkedGeoList.First.Next.Value, innerBig);

            Assert.AreEqual(polygon.Next.CountLoops(), 2);

            Assert.IsNotNull(polygon.Next.LinkedGeoList.First);
            Assert.AreEqual(polygon.Next.LinkedGeoList.First.Value, outer);

            Assert.IsNotNull(polygon.Next.LinkedGeoList.First.Next);
            Assert.AreEqual(polygon.Next.LinkedGeoList.First.Next.Value, inner);

            polygon.Clear();
        }

        [Test]
        public void NormalizeMultiPolygonNoOuterLoops()
        {
            var verts = MakeGeoCoordArray(new double[,] {{0, 0}, {1, 1}, {0, 1}});
            var outer1 = CreateLinkedLoop(verts);

            var verts2 = MakeGeoCoordArray(new double[,] {{2, 2}, {3, 3}, {2, 3}});
            var outer2 = CreateLinkedLoop(verts2);

            var polygon = new LinkedGeoPolygon();
            polygon.AddLinkedLoop(outer1);
            polygon.AddLinkedLoop(outer2);

            int result;
            (result, polygon) = polygon.NormalizeMultiPolygon();

            Assert.AreEqual(H3Lib.StaticData.LinkedGeo.NormalizationErrUnassignedHoles, result);

            Assert.AreEqual(1, polygon.CountPolygons());
            Assert.AreEqual(0, polygon.CountLoops());

            //polygon.Clear();
        }

        [Test]
        public void NormalizeMultiPolygonAlreadyNormalized()
        {
            var verts1 = MakeGeoCoordArray(new double[,] {{0, 0}, {0, 1}, {1, 1}});
            var outer1 = CreateLinkedLoop(verts1);

            var verts2 = MakeGeoCoordArray(new double[,] {{2, 2}, {2, 3}, {3, 3}});
            var outer2 = CreateLinkedLoop(verts2);

            var polygon = new LinkedGeoPolygon();
            polygon.AddLinkedLoop(outer1);
            var next = polygon.AddNew();
            next.AddLinkedLoop(outer2);

            // Should be a no-op
            int result;
            (result, polygon) = polygon.NormalizeMultiPolygon();

            Assert.AreEqual(H3Lib.StaticData.LinkedGeo.NormalizationErrMultiplePolygons, result);

            Assert.AreEqual(2, polygon.CountPolygons());
            Assert.AreEqual(1, polygon.CountLoops());

            Assert.IsNotNull(polygon.LinkedGeoList.First);
            Assert.AreEqual(outer1, polygon.LinkedGeoList.First.Value);

            Assert.AreEqual(1, polygon.Next.CountLoops());
            Assert.IsNotNull(polygon.Next.LinkedGeoList.First);
            Assert.AreEqual(outer2, polygon.Next.LinkedGeoList.First.Value);

            polygon.Clear();
        }

        [Test]
        public void NormalizeMultiPolygonUnassignedHole()
        {
            var verts = MakeGeoCoordArray(new double[,] {{0, 0}, {0, 1}, {1, 1}, {1, 0}});
            var outer = CreateLinkedLoop(verts);

            var verts2 = MakeGeoCoordArray(new double[,] {{2, 2}, {3, 3}, {2, 3}});
            var inner = CreateLinkedLoop(verts2);

            var polygon = new LinkedGeoPolygon();
            polygon.AddLinkedLoop(inner);
            polygon.AddLinkedLoop(outer);

            int result;
            (result, polygon) = polygon.NormalizeMultiPolygon();

            Assert.AreEqual(H3Lib.StaticData.LinkedGeo.NormalizationErrUnassignedHoles, result);
            polygon.Clear();
        }


    }
}
