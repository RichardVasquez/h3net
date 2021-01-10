using System;
using System.Collections.Generic;
using System.Linq;
using H3Lib;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class TestPolygon
    {
        private static readonly List<GeoCoord> SfVerts =
            new List<GeoCoord>
            {
                new GeoCoord(0.659966917655, -2.1364398519396),
                new GeoCoord(0.6595011102219, -2.1359434279405),
                new GeoCoord(0.6583348114025, -2.1354884206045),
                new GeoCoord(0.6581220034068, -2.1382437718946),
                new GeoCoord(0.6594479998527, -2.1384597563896),
                new GeoCoord(0.6599990002976, -2.1376771158464)
            };

        private static void CreateLinkedLoop(
                ref LinkedGeo.LinkedGeoLoop loop, List<GeoCoord> verts, int numVerts)
        {
            loop ??= new LinkedGeo.LinkedGeoLoop();
            for (int i = 0; i < numVerts; i++)
            {
                var vi = verts[i];
                LinkedGeo.AddLinkedCoord(ref loop, ref vi);
            }
        }

        [Test]
        public void PointInsideGeofence()
        {
            Geofence geofence = new Geofence {NumVerts = 6, Verts = SfVerts.ToArray()};

            GeoCoord inside = new GeoCoord(0.659, -2.136);
            GeoCoord somewhere = new GeoCoord(1, 2);

            BBox bbox = new BBox();
            BBox.bboxFromGeofence(geofence, ref bbox);

            var v0 = SfVerts[0];
            Assert.True(!Polygon.pointInsideGeofence(ref geofence, ref bbox, ref v0),
                     "contains exact");
            var v4 = SfVerts[4];
            Assert.True(Polygon.pointInsideGeofence(ref geofence, ref bbox, ref v4),
                     "contains exact 4");
            Assert.True(Polygon.pointInsideGeofence(ref geofence, ref bbox, ref inside),
                     "contains point inside");
            Assert.True(!Polygon.pointInsideGeofence(ref geofence, ref bbox, ref somewhere),
                     "contains somewhere else");
        }

        [Test]
        public void PointInsideGeofenceTransmeridian()
        {
            List<GeoCoord> verts =
                new List<GeoCoord>
                {
                    new GeoCoord(0.01, -Constants.M_PI + 0.01),
                    new GeoCoord(0.01, Constants.M_PI - 0.01),
                    new GeoCoord(-0.01, Constants.M_PI - 0.01),
                    new GeoCoord(-0.01, -Constants.M_PI + 0.01)
                };
            Geofence transMeridianGeofence = new Geofence {NumVerts = 4, Verts = verts.ToArray()};

            GeoCoord eastPoint = new GeoCoord(0.001, -Constants.M_PI + 0.001);
            GeoCoord eastPointOutside = new GeoCoord(.001, -Constants.M_PI + 0.1);
            GeoCoord westPoint = new GeoCoord(.001, Constants.M_PI - 0.001);
            GeoCoord westPointOutside = new GeoCoord(.001, Constants.M_PI - 0.1);

            BBox bbox = new BBox();
            BBox.bboxFromGeofence(transMeridianGeofence, ref bbox);

            Assert.True
                (
                 Polygon.pointInsideGeofence(ref transMeridianGeofence, ref bbox, ref westPoint),
                 "contains point to the west of the antimeridian"
                );
            Assert.True
                (
                 Polygon.pointInsideGeofence(ref transMeridianGeofence, ref bbox, ref eastPoint),
                 "contains point to the east of the antimeridian"
                );
            Assert.True
                (
                 !Polygon.pointInsideGeofence
                     (
                      ref transMeridianGeofence, ref bbox,
                      ref westPointOutside
                     ),
                 "does not contain outside point to the west of the antimeridian"
                );
            Assert.True
                (
                 !Polygon.pointInsideGeofence
                     (
                      ref transMeridianGeofence, ref bbox,
                      ref eastPointOutside
                     ),
                 "does not contain outside point to the east of the antimeridian"
                );
        }

        [Test]
        public void PointInsideLinkedGeoLoop() 
        {
            GeoCoord somewhere = new GeoCoord(1, 2);
            GeoCoord inside = new GeoCoord(0.659, -2.136);

            LinkedGeo.LinkedGeoLoop loop = new LinkedGeo.LinkedGeoLoop();
            CreateLinkedLoop(ref loop, SfVerts, 6);

            BBox bbox = new BBox();
            LinkedGeo.BboxFromLinkedGeoLoop(ref loop, ref bbox);

            Assert.True(LinkedGeo.PointInsideLinkedGeoLoop(ref loop, ref bbox, ref inside),
                     "contains exact4");
            Assert.True(!LinkedGeo.PointInsideLinkedGeoLoop(ref loop, ref bbox, ref somewhere),
                     "contains somewhere else");

            LinkedGeo.DestroyLinkedGeoLoop(ref loop);
        }

        [Test]
        public void BboxFromGeofence()
        {
            List<GeoCoord> verts =
                new List<GeoCoord>
                {
                    new GeoCoord(0.8, 0.3),
                    new GeoCoord(0.7, 0.6),
                    new GeoCoord(1.1, 0.7),
                    new GeoCoord(1.0, 0.2)

                };
            Geofence geofence = new Geofence {NumVerts = 4, Verts = verts.ToArray()};

            BBox expected = new BBox {North = 1.1, South = 0.7, East = 0.7, West = 0.2};

            BBox result = new BBox();
            Polygon.bboxFromGeofence(ref geofence, ref result);
            Assert.True(BBox.bboxEquals(result, expected), "Got expected bbox");
        }

        [Test]
        public void BboxFromGeofenceTransmeridian()
        {
            List<GeoCoord> verts =
                new List<GeoCoord>
                {
                    new GeoCoord(0.1, -Constants.M_PI + 0.1),
                    new GeoCoord(0.1, Constants.M_PI - 0.1),
                    new GeoCoord(0.05, Constants.M_PI - 0.2),
                    new GeoCoord(-0.1, Constants.M_PI - 0.1),
                    new GeoCoord(-0.1, -Constants.M_PI + 0.1),
                    new GeoCoord(-0.05, -Constants.M_PI + 0.2)
                };
            Geofence geofence = new Geofence {NumVerts = 6, Verts = verts.ToArray()};

            BBox expected = new BBox
                            {
                                North = 0.1, South = -0.1, East = -Constants.M_PI + 0.2,
                                West = Constants.M_PI - 0.2
                            };

            BBox result = new BBox();
            Polygon.bboxFromGeofence(ref geofence, ref result);
            Assert.True
                (
                 BBox.bboxEquals(result, expected),
                 "Got expected transmeridian bbox"
                );
        }

        [Test]
        public void BboxFromGeofenceNoVertices()
        {
            Geofence geofence = new Geofence {NumVerts = 0, Verts = null};

            BBox expected = new BBox{North = 0.0, South = 0.0, East = 0.0, West = 0.0};

            BBox result = new BBox();
            Polygon.bboxFromGeofence(ref geofence, ref result);

            Assert.True(BBox.bboxEquals(result, expected), "Got expected bbox");
        }
        
        
        [Test]
        public void BboxesFromGeoPolygon()
        {
            List<GeoCoord> verts =
                new List<GeoCoord>
                {
                    new GeoCoord(0.8, 0.3),
                    new GeoCoord(0.7, 0.6),
                    new GeoCoord(1.1, 0.7),
                    new GeoCoord(1.0, 0.2)
                };
            Geofence geofence = new Geofence {NumVerts = 4, Verts = verts.ToArray()};
            GeoPolygon polygon = new GeoPolygon {Geofence = geofence, NumHoles = 0};

            BBox expected = new BBox {North = 1.1, South = 0.7, East = 0.7, West = 0.2};

            List<BBox> result = new List<BBox> {new BBox()};
            Polygon.bboxesFromGeoPolygon(polygon, ref result);
            Assert.True(BBox.bboxEquals(result[0], expected), "Got expected bbox");
        }

        [Test]
        public void BboxesFromGeoPolygonHole()
        {
            List<GeoCoord> verts =
                new List<GeoCoord>
                {
                    new GeoCoord(0.8, 0.3),
                    new GeoCoord(0.7, 0.6),
                    new GeoCoord(1.1, 0.7),
                    new GeoCoord(1.0, 0.2)
                };
            Geofence geofence = new Geofence {NumVerts = 4, Verts = verts.ToArray()};

            // not a real hole, but doesn't matter for the test
            List<GeoCoord> holeVerts =
                new List<GeoCoord>
                {
                    new GeoCoord(0.9, 0.3),
                    new GeoCoord(0.9, 0.5),
                    new GeoCoord(1.0, 0.7),
                    new GeoCoord(0.9, 0.3)
                };
            List<Geofence> holeGeofence =
                new List<Geofence>
                {
                    new Geofence {NumVerts = 4, Verts = holeVerts.ToArray()}
                };

            GeoPolygon polygon = new GeoPolygon {Geofence = geofence, NumHoles = 1, Holes = holeGeofence};

            BBox expected = new BBox {North = 1.1, South = 0.7, East = 0.7, West = 0.2};
            BBox expectedHole = new BBox {North = 1.0, South = 0.9, East = 0.7, West = 0.3};

            List<BBox> result = new List<BBox> {new BBox(), new BBox()};

            Polygon.bboxesFromGeoPolygon(polygon, ref result);
            Assert.True(BBox.bboxEquals(result[0], expected), "Got expected bbox");
            Assert.True(BBox.bboxEquals(result[1], expectedHole), "Got expected hole bbox");
        }

        [Test]
        public void BboxFromLinkedGeoLoop()
        {
            List<GeoCoord> verts =
                new List<GeoCoord>
                {
                    new GeoCoord(0.8, 0.3),
                    new GeoCoord(0.7, 0.6),
                    new GeoCoord(1.1, 0.7),
                    new GeoCoord( 1.0, 0.2)
                };

            LinkedGeo.LinkedGeoLoop loop = new LinkedGeo.LinkedGeoLoop();
            CreateLinkedLoop(ref loop, verts, 4);

            BBox expected = new BBox {North = 1.1, South = 0.7, East = 0.7, West = 0.2};

            BBox result = new BBox();
            LinkedGeo.BboxFromLinkedGeoLoop(ref loop, ref result);
            Assert.True(BBox.bboxEquals(result, expected), "Got expected bbox");
        }

        [Test]
        public void BboxFromLinkedGeoLoopNoVertices()
        {
            LinkedGeo.LinkedGeoLoop loop = new LinkedGeo.LinkedGeoLoop();

            BBox expected = new BBox {North = 0.0, South = 0.0, East = 0.0, West = 0.0};

            BBox result = new BBox();
            LinkedGeo.BboxFromLinkedGeoLoop(ref loop, ref result);

            Assert.True(BBox.bboxEquals(result, expected), "Got expected bbox");
        }

        [Test]
        public void IsClockwiseGeofence()
        {
            List<GeoCoord> verts =
                new List<GeoCoord>
                {
                    new GeoCoord(0, 0), new GeoCoord(0.1, 0.1), new GeoCoord(0, 0.1)
                };
            Geofence geofence = new Geofence {NumVerts = 3, Verts = verts.ToArray()};

            Assert.True
                (
                 Polygon.isClockwiseGeofence(geofence),
                 "Got true for clockwise geofence"
                );
        }

        [Test]
        public void IsClockwiseLinkedGeoLoop()
        {
            List<GeoCoord> verts =
                new List<GeoCoord>
                {
                    new GeoCoord(0.1, 0.1),
                    new GeoCoord(0.2, 0.2),
                    new GeoCoord(0.1, 0.2)
                };
            LinkedGeo.LinkedGeoLoop loop = new LinkedGeo.LinkedGeoLoop();
            CreateLinkedLoop(ref loop, verts, 3);

            Assert.True
                (
                 LinkedGeo.IsClockwiseLinkedGeoLoop(loop),
                 "Got true for clockwise loop"
                );
        }

        [Test]
        public void IsNotClockwiseLinkedGeoLoop()
        {
            List<GeoCoord> verts =
                new List<GeoCoord>
                {
                    new GeoCoord(0, 0), new GeoCoord(0, 0.4), new GeoCoord(0.4, 0.4), new GeoCoord(0.4, 0)
                };
            LinkedGeo.LinkedGeoLoop loop = new LinkedGeo.LinkedGeoLoop();
            CreateLinkedLoop(ref loop, verts, 4);

            Assert.False(LinkedGeo.IsClockwiseLinkedGeoLoop(loop),
                     "Got false for counter-clockwise loop");
        }

        [Test]
        public void IsClockwiseGeofenceTransmeridian()
        {
            List<GeoCoord> verts =
                new List<GeoCoord>
                {
                    new GeoCoord(0.4, Constants.M_PI - 0.1),
                    new GeoCoord(0.4, -Constants.M_PI + 0.1),
                    new GeoCoord(-0.4, -Constants.M_PI + 0.1),
                    new GeoCoord(0.4, Constants.M_PI - 0.1)
                };
            Geofence geofence = new Geofence {NumVerts = 4, Verts = verts.ToArray()};

            Assert.True
                (
                 Polygon.isClockwiseGeofence(geofence),
                 "Got true for clockwise geofence"
                );
        }

        [Test]
        public void IsClockwiseLinkedGeoLoopTransmeridian()
        {
            List<GeoCoord> verts =
                new List<GeoCoord>
                {
                    new GeoCoord(0.4, Constants.M_PI - 0.1),
                    new GeoCoord(0.4, -Constants.M_PI + 0.1),
                    new GeoCoord(-0.4, -Constants.M_PI + 0.1),
                    new GeoCoord(-0.4, Constants.M_PI - 0.1)
                };
            LinkedGeo.LinkedGeoLoop loop = new LinkedGeo.LinkedGeoLoop();
            CreateLinkedLoop(ref loop, verts, 4);

            Assert.True(LinkedGeo.IsClockwiseLinkedGeoLoop(loop),
                     "Got true for clockwise transmeridian loop");
        }

        [Test]
        public void IsNotClockwiseLinkedGeoLoopTransmeridian()
        {
            List<GeoCoord> verts =
                new List<GeoCoord>
                {
                    new GeoCoord(0.4, Constants.M_PI - 0.1),
                    new GeoCoord(-0.4, Constants.M_PI - 0.1),
                    new GeoCoord(-0.4, -Constants.M_PI + 0.1),
                    new GeoCoord(0.4, -Constants.M_PI + 0.1)
                };
            LinkedGeo.LinkedGeoLoop loop = new LinkedGeo.LinkedGeoLoop();
            CreateLinkedLoop(ref loop, verts, 4);

            Assert.False(LinkedGeo.IsClockwiseLinkedGeoLoop(loop),
                     "Got false for counter-clockwise transmeridian loop");
        }

        [Test]
        public void NormalizeMultiPolygonSingle()
        {
            List<GeoCoord> verts =
                new List<GeoCoord>
                {
                    new GeoCoord(0, 0),
                    new GeoCoord(0, 1),
                    new GeoCoord(1, 1)
                };

            LinkedGeo.LinkedGeoLoop outer = new LinkedGeo.LinkedGeoLoop();
            Assert.True(outer != null);
            CreateLinkedLoop(ref outer, verts, 3);

            LinkedGeo.LinkedGeoPolygon polygon = new LinkedGeo.LinkedGeoPolygon();
            LinkedGeo.AddLinkedLoop(ref polygon, ref outer);

            int result = LinkedGeo.NormalizeMultiPolygon(ref polygon);

            Assert.True(result == LinkedGeo.NormalizationSuccess, "No error code returned");

            Assert.True(LinkedGeo.CountLinkedPolygons(ref polygon) == 1, "Polygon count correct");
            Assert.True(LinkedGeo.CountLinkedLoops(ref polygon) == 1, "Loop count correct");
            Assert.True(polygon.First == outer, "Got expected loop");
        }

        [Test]
        public void NormalizeMultiPolygonTwoOuterLoops()
        {
            List<GeoCoord> verts1 =
                new List<GeoCoord>
                {
                    new GeoCoord(0, 0), new GeoCoord(0, 1), new GeoCoord(1, 1)
                };

            LinkedGeo.LinkedGeoLoop outer1 = new LinkedGeo.LinkedGeoLoop();
            Assert.NotNull(outer1);
            CreateLinkedLoop(ref outer1, verts1, 3);

            List<GeoCoord> verts2 =
                new List<GeoCoord>
                {
                    new GeoCoord(2, 2), new GeoCoord(2, 3), new GeoCoord(3, 3)
                };

            LinkedGeo.LinkedGeoLoop outer2 = new LinkedGeo.LinkedGeoLoop();
            Assert.NotNull(outer2);
            CreateLinkedLoop(ref outer2, verts2, 3);

            LinkedGeo.LinkedGeoPolygon polygon = new LinkedGeo.LinkedGeoPolygon();
            LinkedGeo.AddLinkedLoop(ref polygon, ref outer1);
            LinkedGeo.AddLinkedLoop(ref polygon, ref outer2);

            int result = LinkedGeo.NormalizeMultiPolygon(ref polygon);

            Assert.True(result == LinkedGeo.NormalizationSuccess, "No error code returned");

            Assert.True(LinkedGeo.CountLinkedPolygons(ref polygon) == 2, "Polygon count correct");
            Assert.True
                (
                 LinkedGeo.CountLinkedLoops(ref polygon) == 1,
                 "Loop count on first polygon correct"
                );
            Assert.True
                (
                 LinkedGeo.CountLinkedLoops(ref polygon.Next) == 1,
                 "Loop count on second polygon correct"
                );
        }

        [Test]
        public void normalizeMultiPolygonOneHole()
        {
            List<GeoCoord> verts =
                new List<GeoCoord>
                {
                    new GeoCoord(0, 0), new GeoCoord(0, 3),
                    new GeoCoord(3, 3), new GeoCoord(3, 0)
                };

            LinkedGeo.LinkedGeoLoop outer = new LinkedGeo.LinkedGeoLoop();
            Assert.NotNull(outer);
            CreateLinkedLoop(ref outer, verts, 4);

            List<GeoCoord> verts2 =
                new List<GeoCoord>
                {
                    new GeoCoord(1, 1), new GeoCoord(2, 2), new GeoCoord(1, 2)
                };

            LinkedGeo.LinkedGeoLoop inner = new LinkedGeo.LinkedGeoLoop();
            Assert.NotNull(inner);
            CreateLinkedLoop(ref inner, verts2, 3);

            LinkedGeo.LinkedGeoPolygon polygon = new LinkedGeo.LinkedGeoPolygon();
            LinkedGeo.AddLinkedLoop(ref polygon, ref inner);
            LinkedGeo.AddLinkedLoop(ref polygon, ref outer);

            int result = LinkedGeo.NormalizeMultiPolygon(ref polygon);

            Assert.True(result == LinkedGeo.NormalizationSuccess, "No error code returned");

            Assert.True(LinkedGeo.CountLinkedPolygons(ref polygon) == 1, "Polygon count correct");
            Assert.True(LinkedGeo.CountLinkedLoops(ref polygon) == 2,
                     "Loop count on first polygon correct");
            Assert.True(polygon.First == outer, "Got expected outer loop");
            Assert.True(polygon.First.Next == inner, "Got expected inner loop");
        }

        [Test]
        public void NormalizeMultiPolygonTwoHoles()
        {
            List<GeoCoord> verts =
                new List<GeoCoord>
                {
                    new GeoCoord(0, 0),
                    new GeoCoord(0, 0.4),
                    new GeoCoord(0.4, 0.4),
                    new GeoCoord(0.4, 0)
                };

            LinkedGeo.LinkedGeoLoop outer = new LinkedGeo.LinkedGeoLoop();
            Assert.NotNull(outer);
            CreateLinkedLoop(ref outer, verts, 4);

            List<GeoCoord> verts2 =
                new List<GeoCoord>
                {
                    new GeoCoord(0.1, 0.1),
                    new GeoCoord(0.2, 0.2),
                    new GeoCoord(0.1, 0.2)
                };

            LinkedGeo.LinkedGeoLoop inner1 = new LinkedGeo.LinkedGeoLoop();
            Assert.NotNull(inner1);
            CreateLinkedLoop(ref inner1, verts2, 3);

            List<GeoCoord> verts3 =
                new List<GeoCoord>
                {
                    new GeoCoord(0.2, 0.2),
                    new GeoCoord(0.3, 0.3),
                    new GeoCoord(0.2, 0.3)
                };

            LinkedGeo.LinkedGeoLoop inner2 = new LinkedGeo.LinkedGeoLoop();
            Assert.NotNull(inner2);
            CreateLinkedLoop(ref inner2, verts3, 3);

            LinkedGeo.LinkedGeoPolygon polygon = new LinkedGeo.LinkedGeoPolygon();
            LinkedGeo.AddLinkedLoop(ref polygon, ref inner2);
            LinkedGeo.AddLinkedLoop(ref polygon, ref outer);
            LinkedGeo.AddLinkedLoop(ref polygon, ref inner1);

            int result = LinkedGeo.NormalizeMultiPolygon(ref polygon);

            Assert.True(result == LinkedGeo.NormalizationSuccess, "No error code returned");

            Assert.True
                (
                 LinkedGeo.CountLinkedPolygons(ref polygon) == 1,
                 "Polygon count correct for 2 holes"
                );
            Assert.True(polygon.First == outer, "Got expected outer loop");
            Assert.True
                (
                 LinkedGeo.CountLinkedLoops(ref polygon) == 3,
                 "Loop count on first polygon correct"
                );
        }

        [Test]
        public void NormalizeMultiPolygonTwoDonuts()
        {
            GeoCoord[] verts =
            {
                new GeoCoord(0, 0),
                new GeoCoord(0, 3),
                new GeoCoord(3, 3),
                new GeoCoord(3, 0)
            };
            LinkedGeo.LinkedGeoLoop outer = new LinkedGeo.LinkedGeoLoop();
            Assert.NotNull(outer);
            CreateLinkedLoop(ref outer, verts.ToList(), 4);

            GeoCoord[] verts2 =
            {
                new GeoCoord(1, 1),
                new GeoCoord(2, 2),
                new GeoCoord(1, 2)
            };
            LinkedGeo.LinkedGeoLoop inner = new LinkedGeo.LinkedGeoLoop();
            Assert.NotNull(inner);
            CreateLinkedLoop(ref inner, verts2.ToList(), 3);

            GeoCoord[] verts3 =
            {
                new GeoCoord(0, 0),
                new GeoCoord(0, -3),
                new GeoCoord(-3, -3),
                new GeoCoord(-3, 0)
            };
            LinkedGeo.LinkedGeoLoop outer2 = new LinkedGeo.LinkedGeoLoop();
            Assert.NotNull(outer2);
            CreateLinkedLoop(ref outer2, verts3.ToList(), 4);

            GeoCoord[] verts4 =
            {
                new GeoCoord(-1, -1),
                new GeoCoord(-2, -2),
                new GeoCoord(-1, -2)
            };
            LinkedGeo.LinkedGeoLoop inner2 = new LinkedGeo.LinkedGeoLoop();
            Assert.NotNull(inner2);
            CreateLinkedLoop(ref inner2, verts4.ToList(), 3);

            LinkedGeo.LinkedGeoPolygon polygon = new LinkedGeo.LinkedGeoPolygon();
            LinkedGeo.AddLinkedLoop(ref polygon, ref inner2);
            LinkedGeo.AddLinkedLoop(ref polygon, ref inner);
            LinkedGeo.AddLinkedLoop(ref polygon, ref outer);
            LinkedGeo.AddLinkedLoop(ref polygon, ref outer2);

            int result = LinkedGeo.NormalizeMultiPolygon(ref polygon);

            Assert.True(result == LinkedGeo.NormalizationSuccess, "No error code returned");

            Assert.True(LinkedGeo.CountLinkedPolygons(ref polygon) == 2, "Polygon count correct");
            Assert.True
                (
                 LinkedGeo.CountLinkedLoops(ref polygon) == 2,
                 "Loop count on first polygon correct"
                );
            Assert.True
                (
                 LinkedGeo.CountLinkedCoords(ref polygon.First) == 4,
                 "Got expected outer loop"
                );
            Assert.True
                (
                 LinkedGeo.CountLinkedCoords(ref polygon.First.Next) == 3,
                 "Got expected inner loop"
                );
            Assert.True
                (
                 LinkedGeo.CountLinkedLoops(ref polygon.Next) == 2,
                 "Loop count on second polygon correct"
                );
            Assert.True
                (
                 LinkedGeo.CountLinkedCoords(ref polygon.Next.First) == 4,
                 "Got expected outer loop"
                );
            Assert.True
                (
                 LinkedGeo.CountLinkedCoords(ref polygon.Next.First.Next) == 3,
                 "Got expected inner loop"
                );
        }

        [Test]
        public void NormalizeMultiPolygonNestedDonuts()
        {
            GeoCoord[] verts =
            {
                new GeoCoord(0.2, 0.2),
                new GeoCoord(0.2, -0.2),
                new GeoCoord(-0.2, -0.2),
                new GeoCoord(-0.2, 0.2)
            };
            LinkedGeo.LinkedGeoLoop outer =new LinkedGeo.LinkedGeoLoop();
            Assert.NotNull(outer);
            CreateLinkedLoop(ref outer, verts.ToList(), 4);

            GeoCoord[] verts2 =
            {
                new GeoCoord(0.1, 0.1),
                new GeoCoord(-0.1, 0.1),
                new GeoCoord(-0.1, -0.1),
                new GeoCoord(0.1, -0.1)
            };
            LinkedGeo.LinkedGeoLoop inner =new LinkedGeo.LinkedGeoLoop();
            Assert.NotNull(inner);
            CreateLinkedLoop(ref inner, verts2.ToList(), 4);

            GeoCoord[] verts3 =
            {
                new GeoCoord(0.6, 0.6),
                new GeoCoord(0.6, -0.6),
                new GeoCoord(-0.6, -0.6),
                new GeoCoord(-0.6, 0.6)
            };
            LinkedGeo.LinkedGeoLoop outerBig =new LinkedGeo.LinkedGeoLoop();
            Assert.NotNull(outerBig);
            CreateLinkedLoop(ref outerBig, verts3.ToList(), 4);

            GeoCoord[] verts4 =
            {
                new GeoCoord(0.5, 0.5),
                new GeoCoord(-0.5, 0.5),
                new GeoCoord(-0.5, -0.5),
                new GeoCoord(0.5, -0.5)
            };
            LinkedGeo.LinkedGeoLoop innerBig =new LinkedGeo.LinkedGeoLoop();
            Assert.NotNull(innerBig);
            CreateLinkedLoop(ref innerBig, verts4.ToList(), 4);

            LinkedGeo.LinkedGeoPolygon polygon = new LinkedGeo.LinkedGeoPolygon();
            LinkedGeo.AddLinkedLoop(ref polygon, ref inner);
            LinkedGeo.AddLinkedLoop(ref polygon, ref outerBig);
            LinkedGeo.AddLinkedLoop(ref polygon, ref innerBig);
            LinkedGeo.AddLinkedLoop(ref polygon, ref outer);

            int result = LinkedGeo.NormalizeMultiPolygon(ref polygon);

            Assert.True(result == LinkedGeo.NormalizationSuccess, "No error code returned");

            Assert.True(LinkedGeo.CountLinkedPolygons(ref polygon) == 2, "Polygon count correct");
            Assert.True(LinkedGeo.CountLinkedLoops(ref polygon) == 2,
                     "Loop count on first polygon correct");
            Assert.True(polygon.First == outerBig, "Got expected outer loop");
            Assert.True(polygon.First.Next == innerBig, "Got expected inner loop");
            Assert.True(LinkedGeo.CountLinkedLoops(ref polygon.Next) == 2,
                     "Loop count on second polygon correct");
            Assert.True(polygon.Next.First == outer, "Got expected outer loop");
            Assert.True(polygon.Next.First.Next == inner, "Got expected inner loop");
        }

        [Test]
        public void NormalizeMultiPolygonNoOuterLoops()
        {
            GeoCoord[] verts1 =
            {
                new GeoCoord(0, 0),
                new GeoCoord(1, 1),
                new GeoCoord(0, 1)
            };

            LinkedGeo.LinkedGeoLoop outer1 = new LinkedGeo.LinkedGeoLoop();
            Assert.NotNull(outer1);
            CreateLinkedLoop(ref outer1, verts1.ToList(), 3);

            GeoCoord[] verts2 =
            {
                new GeoCoord(2, 2),
                new GeoCoord(3, 3),
                new GeoCoord(2, 3)
            };

            LinkedGeo.LinkedGeoLoop outer2 = new LinkedGeo.LinkedGeoLoop();
            Assert.NotNull(outer2);
            CreateLinkedLoop(ref outer2, verts2.ToList(), 3);

            LinkedGeo.LinkedGeoPolygon polygon = new LinkedGeo.LinkedGeoPolygon();
            LinkedGeo.AddLinkedLoop(ref polygon, ref outer1);
            LinkedGeo.AddLinkedLoop(ref polygon, ref outer2);

            int result = LinkedGeo.NormalizeMultiPolygon(ref polygon);

            Assert.True
                (
                 result == LinkedGeo.NormalizationErrUnassignedHoles,
                 "Expected error code returned"
                );

            Assert.True(LinkedGeo.CountLinkedPolygons(ref polygon) == 1,
                        "Polygon count correct");
            Assert.True
                (
                 LinkedGeo.CountLinkedLoops(ref polygon) == 0,
                 "Loop count as expected with invalid input"
                );
        }

        [Test]
        public void NormalizeMultiPolygonAlreadyNormalized()
        {
            GeoCoord[] verts1 =
            {
                new GeoCoord(0, 0),
                new GeoCoord(0, 1),
                new GeoCoord(1, 1)
            };

            LinkedGeo.LinkedGeoLoop outer1 = new LinkedGeo.LinkedGeoLoop();
            Assert.NotNull(outer1);
            CreateLinkedLoop(ref outer1, verts1.ToList(), 3);

            GeoCoord[] verts2 =
            {
                new GeoCoord(2, 2),
                new GeoCoord(2, 3),
                new GeoCoord(3, 3)
            };

            LinkedGeo.LinkedGeoLoop outer2 =new LinkedGeo.LinkedGeoLoop();
            Assert.NotNull(outer2);
            CreateLinkedLoop(ref outer2, verts2.ToList(), 3);

            LinkedGeo.LinkedGeoPolygon polygon = new LinkedGeo.LinkedGeoPolygon();
            LinkedGeo.AddLinkedLoop(ref polygon, ref outer1);
            LinkedGeo.LinkedGeoPolygon next = LinkedGeo.AddNewLinkedPolygon(ref polygon);
            LinkedGeo.AddLinkedLoop(ref next, ref outer2);

            // Should be a no-op
            int result = LinkedGeo.NormalizeMultiPolygon(ref polygon);

            Assert.True(result ==LinkedGeo. NormalizationErrMultiplePolygons,
                     "Expected error code returned");

            Assert.True(LinkedGeo.CountLinkedPolygons(ref polygon) == 2, "Polygon count correct");
            Assert.True(LinkedGeo.CountLinkedLoops(ref polygon) == 1,
                     "Loop count on first polygon correct");
            Assert.True(polygon.First == outer1, "Got expected outer loop");
            Assert.True(LinkedGeo.CountLinkedLoops(ref polygon.Next) == 1,
                     "Loop count on second polygon correct");
            Assert.True(polygon.Next.First == outer2, "Got expected outer loop");
        }

        [Test]
        public void NormalizeMultiPolygonUnassignedHole()
        {
            GeoCoord[] verts =
            {
                new GeoCoord(0, 0),
                new GeoCoord(0, 1),
                new GeoCoord(1, 1),
                new GeoCoord(1, 0)
            };
            
            LinkedGeo.LinkedGeoLoop outer = new LinkedGeo.LinkedGeoLoop();
            Assert.NotNull(outer);
            CreateLinkedLoop(ref outer, verts.ToList(), 4);

            GeoCoord[] verts2 =
            {
                new GeoCoord(2, 2),
                new GeoCoord(3, 3),
                new GeoCoord(2, 3)
            };

            LinkedGeo.LinkedGeoLoop inner = new LinkedGeo.LinkedGeoLoop();
            Assert.NotNull(inner);
            CreateLinkedLoop(ref inner, verts2.ToList(), 3);

            LinkedGeo.LinkedGeoPolygon polygon = new LinkedGeo.LinkedGeoPolygon();
            LinkedGeo.AddLinkedLoop(ref polygon, ref inner);
            LinkedGeo.AddLinkedLoop(ref polygon, ref outer);

            int result = LinkedGeo.NormalizeMultiPolygon(ref polygon);
            
            // Expected error code returned
            Assert.AreEqual(result, LinkedGeo.NormalizationErrUnassignedHoles);
        }
    }
}
