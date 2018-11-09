using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using h3net.API;
using NUnit.Framework;

namespace h3tests
{
    [TestFixture]
    public class TestPolygon
    {
        public static List<GeoCoord> sfVerts =
            new List<GeoCoord>
            {
                new GeoCoord(0.659966917655, -2.1364398519396),
                new GeoCoord(0.6595011102219, -2.1359434279405),
                new GeoCoord(0.6583348114025, -2.1354884206045),
                new GeoCoord(0.6581220034068, -2.1382437718946),
                new GeoCoord(0.6594479998527, -2.1384597563896),
                new GeoCoord(0.6599990002976, -2.1376771158464)
            };

        public static void createLinkedLoop(
                ref LinkedGeo.LinkedGeoLoop loop, List<GeoCoord> verts, int numVerts)
        {
            loop = new LinkedGeo.LinkedGeoLoop();
            for (int i = 0; i < numVerts; i++)
            {
                var vi = verts[i];
                LinkedGeo.addLinkedCoord(ref loop, ref vi);
            }
        }

        [Test]
        public void pointInsideGeofence()
        {
            Geofence geofence = new Geofence {numVerts = 6, verts = sfVerts.ToArray()};

            GeoCoord inside = new GeoCoord(0.659, -2.136);
            GeoCoord somewhere = new GeoCoord(1, 2);

            BBox bbox = new BBox();
            BBox.bboxFromGeofence(geofence, ref bbox);

            var v0 = sfVerts[0];
            Assert.True(!Polygon.pointInsideGeofence(ref geofence, ref bbox, ref v0),
                     "contains exact");
            var v4 = sfVerts[4];
            Assert.True(Polygon.pointInsideGeofence(ref geofence, ref bbox, ref v4),
                     "contains exact 4");
            Assert.True(Polygon.pointInsideGeofence(ref geofence, ref bbox, ref inside),
                     "contains point inside");
            Assert.True(!Polygon.pointInsideGeofence(ref geofence, ref bbox, ref somewhere),
                     "contains somewhere else");
        }

        [Test]
        public void pointInsideGeofenceTransmeridian()
        {
            List<GeoCoord> verts =
                new List<GeoCoord>
                {
                    new GeoCoord(0.01, -Constants.M_PI + 0.01),
                    new GeoCoord(0.01, Constants.M_PI - 0.01),
                    new GeoCoord(-0.01, Constants.M_PI - 0.01),
                    new GeoCoord(-0.01, -Constants.M_PI + 0.01)
                };
            Geofence transMeridianGeofence = new Geofence {numVerts = 4, verts = verts.ToArray()};

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
        public void pointInsideLinkedGeoLoop() 
        {
            GeoCoord somewhere = new GeoCoord(1, 2);
            GeoCoord inside = new GeoCoord(0.659, -2.136);

            LinkedGeo.LinkedGeoLoop loop = new LinkedGeo.LinkedGeoLoop();
            createLinkedLoop(ref loop, sfVerts, 6);

            BBox bbox = new BBox();
            LinkedGeo.bboxFromLinkedGeoLoop(ref loop, ref bbox);

            Assert.True(LinkedGeo.pointInsideLinkedGeoLoop(ref loop, ref bbox, ref inside),
                     "contains exact4");
            Assert.True(!LinkedGeo.pointInsideLinkedGeoLoop(ref loop, ref bbox, ref somewhere),
                     "contains somewhere else");

            LinkedGeo.destroyLinkedGeoLoop(ref loop);
        }

        [Test]
        public void bboxFromGeofence()
        {
            List<GeoCoord> verts =
                new List<GeoCoord>
                {
                    new GeoCoord(0.8, 0.3),
                    new GeoCoord(0.7, 0.6),
                    new GeoCoord(1.1, 0.7),
                    new GeoCoord(1.0, 0.2)

                };
            Geofence geofence = new Geofence {numVerts = 4, verts = verts.ToArray()};

            BBox expected = new BBox {north = 1.1, south = 0.7, east = 0.7, west = 0.2};

            BBox result = new BBox();
            Polygon.bboxFromGeofence(ref geofence, ref result);
            Assert.True(BBox.bboxEquals(result, expected), "Got expected bbox");
        }

        [Test]
        public void bboxFromGeofenceTransmeridian()
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
            Geofence geofence = new Geofence {numVerts = 6, verts = verts.ToArray()};

            BBox expected = new BBox
                            {
                                north = 0.1, south = -0.1, east = -Constants.M_PI + 0.2,
                                west = Constants.M_PI - 0.2
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
        public void bboxesFromGeoPolygon()
        {
            List<GeoCoord> verts =
                new List<GeoCoord>
                {
                    new GeoCoord(0.8, 0.3),
                    new GeoCoord(0.7, 0.6),
                    new GeoCoord(1.1, 0.7),
                    new GeoCoord(1.0, 0.2)
                };
            Geofence geofence = new Geofence {numVerts = 4, verts = verts.ToArray()};
            GeoPolygon polygon = new GeoPolygon {Geofence = geofence, numHoles = 0};

            BBox expected = new BBox {north = 1.1, south = 0.7, east = 0.7, west = 0.2};

            List<BBox> result = new List<BBox>();
            result.Add(new BBox());
            Polygon.bboxesFromGeoPolygon(polygon, ref result);
            Assert.True(BBox.bboxEquals(result[0], expected), "Got expected bbox");
        }

        [Test]
        public void bboxesFromGeoPolygonHole()
        {
            List<GeoCoord> verts =
                new List<GeoCoord>
                {
                    new GeoCoord(0.8, 0.3),
                    new GeoCoord(0.7, 0.6),
                    new GeoCoord(1.1, 0.7),
                    new GeoCoord(1.0, 0.2)
                };
            Geofence geofence = new Geofence {numVerts = 4, verts = verts.ToArray()};

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
                    new Geofence {numVerts = 4, verts = holeVerts.ToArray()}
                };

            GeoPolygon polygon = new GeoPolygon {Geofence = geofence, numHoles = 1, holes = holeGeofence};

            BBox expected = new BBox {north = 1.1, south = 0.7, east = 0.7, west = 0.2};
            BBox expectedHole = new BBox {north = 1.0, south = 0.9, east = 0.7, west = 0.3};

            List<BBox> result = new List<BBox> {new BBox(), new BBox()};

            Polygon.bboxesFromGeoPolygon(polygon, ref result);
            Assert.True(BBox.bboxEquals(result[0], expected), "Got expected bbox");
            Assert.True(BBox.bboxEquals(result[1], expectedHole), "Got expected hole bbox");
        }

        [Test]
        public void bboxFromLinkedGeoLoop()
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
            createLinkedLoop(ref loop, verts, 4);

            BBox expected = new BBox {north = 1.1, south = 0.7, east = 0.7, west = 0.2};

            BBox result = new BBox();
            LinkedGeo.bboxFromLinkedGeoLoop(ref loop, ref result);
            Assert.True(BBox.bboxEquals(result, expected), "Got expected bbox");
        }

        [Test]
        public void bboxFromLinkedGeoLoopNoVertices()
        {
            LinkedGeo.LinkedGeoLoop loop = new LinkedGeo.LinkedGeoLoop();

            BBox expected = new BBox {north = 0.0, south = 0.0, east = 0.0, west = 0.0};

            BBox result = new BBox();
            LinkedGeo.bboxFromLinkedGeoLoop(ref loop, ref result);

            Assert.True(BBox.bboxEquals(result, expected), "Got expected bbox");
        }

        [Test]
        public void isClockwiseGeofence()
        {
            List<GeoCoord> verts =
                new List<GeoCoord>
                {
                    new GeoCoord(0, 0), new GeoCoord(0.1, 0.1), new GeoCoord(0, 0.1)
                };
            Geofence geofence = new Geofence {numVerts = 3, verts = verts.ToArray()};

            Assert.True
                (
                 Polygon.isClockwiseGeofence(geofence),
                 "Got true for clockwise geofence"
                );
        }

        [Test]
        public void isClockwiseLinkedGeoLoop()
        {
            List<GeoCoord> verts =
                new List<GeoCoord>
                {
                    new GeoCoord(0.1, 0.1),
                    new GeoCoord(0.2, 0.2),
                    new GeoCoord(0.1, 0.2)
                };
            LinkedGeo.LinkedGeoLoop loop = new LinkedGeo.LinkedGeoLoop();
            createLinkedLoop(ref loop, verts, 3);

            Assert.True
                (
                 LinkedGeo.isClockwiseLinkedGeoLoop(loop),
                 "Got true for clockwise loop"
                );
        }

        [Test]
        public void isNotClockwiseLinkedGeoLoop()
        {
            List<GeoCoord> verts =
                new List<GeoCoord>
                {
                    new GeoCoord(0, 0), new GeoCoord(0, 0.4), new GeoCoord(0.4, 0.4), new GeoCoord(0.4, 0)
                };
            LinkedGeo.LinkedGeoLoop loop = new LinkedGeo.LinkedGeoLoop();
            createLinkedLoop(ref loop, verts, 4);

            Assert.False(LinkedGeo.isClockwiseLinkedGeoLoop(loop),
                     "Got false for counter-clockwise loop");
        }

        [Test]
        public void isClockwiseGeofenceTransmeridian()
        {
            List<GeoCoord> verts =
                new List<GeoCoord>
                {
                    new GeoCoord(0.4, Constants.M_PI - 0.1),
                    new GeoCoord(0.4, -Constants.M_PI + 0.1),
                    new GeoCoord(-0.4, -Constants.M_PI + 0.1),
                    new GeoCoord(0.4, Constants.M_PI - 0.1)
                };
            Geofence geofence = new Geofence {numVerts = 4, verts = verts.ToArray()};

            Assert.True
                (
                 Polygon.isClockwiseGeofence(geofence),
                 "Got true for clockwise geofence"
                );
        }

        [Test]
        public void     isClockwiseLinkedGeoLoopTransmeridian()
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
            createLinkedLoop(ref loop, verts, 4);

            Assert.True(LinkedGeo.isClockwiseLinkedGeoLoop(loop),
                     "Got true for clockwise transmeridian loop");
        }

        [Test]
        public void isNotClockwiseLinkedGeoLoopTransmeridian()
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
            createLinkedLoop(ref loop, verts, 4);

            Assert.False(LinkedGeo.isClockwiseLinkedGeoLoop(loop),
                     "Got false for counter-clockwise transmeridian loop");
        }

        [Test]
        public void normalizeMultiPolygonSingle()
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
            createLinkedLoop(ref outer, verts, 3);

            LinkedGeo.LinkedGeoPolygon polygon = new LinkedGeo.LinkedGeoPolygon();
            LinkedGeo.addLinkedLoop(ref polygon, ref outer);

            int result = LinkedGeo.normalizeMultiPolygon(ref polygon);

            Assert.True(result == LinkedGeo.NORMALIZATION_SUCCESS, "No error code returned");

            Assert.True(LinkedGeo.countLinkedPolygons(ref polygon) == 1, "Polygon count correct");
            Assert.True(LinkedGeo.countLinkedLoops(ref polygon) == 1, "Loop count correct");
            Assert.True(polygon.first == outer, "Got expected loop");
        }

        [Test]
        public void normalizeMultiPolygonTwoOuterLoops()
        {
            List<GeoCoord> verts1 =
                new List<GeoCoord>
                {
                    new GeoCoord(0, 0), new GeoCoord(0, 1), new GeoCoord(1, 1)
                };

            LinkedGeo.LinkedGeoLoop outer1 = new LinkedGeo.LinkedGeoLoop();
            Assert.NotNull(outer1);
            createLinkedLoop(ref outer1, verts1, 3);

            List<GeoCoord> verts2 =
                new List<GeoCoord>
                {
                    new GeoCoord(2, 2), new GeoCoord(2, 3), new GeoCoord(3, 3)
                };

            LinkedGeo.LinkedGeoLoop outer2 = new LinkedGeo.LinkedGeoLoop();
            Assert.NotNull(outer2);
            createLinkedLoop(ref outer2, verts2, 3);

            LinkedGeo.LinkedGeoPolygon polygon = new LinkedGeo.LinkedGeoPolygon();
            LinkedGeo.addLinkedLoop(ref polygon, ref outer1);
            LinkedGeo.addLinkedLoop(ref polygon, ref outer2);

            int result = LinkedGeo.normalizeMultiPolygon(ref polygon);

            Assert.True(result == LinkedGeo.NORMALIZATION_SUCCESS, "No error code returned");

            Assert.True(LinkedGeo.countLinkedPolygons(ref polygon) == 2, "Polygon count correct");
            Assert.True
                (
                 LinkedGeo.countLinkedLoops(ref polygon) == 1,
                 "Loop count on first polygon correct"
                );
            Assert.True
                (
                 LinkedGeo.countLinkedLoops(ref polygon.next) == 1,
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
            createLinkedLoop(ref outer, verts, 4);

            List<GeoCoord> verts2 =
                new List<GeoCoord>
                {
                    new GeoCoord(1, 1), new GeoCoord(2, 2), new GeoCoord(1, 2)
                };

            LinkedGeo.LinkedGeoLoop inner = new LinkedGeo.LinkedGeoLoop();
            Assert.NotNull(inner);
            createLinkedLoop(ref inner, verts2, 3);

            LinkedGeo.LinkedGeoPolygon polygon = new LinkedGeo.LinkedGeoPolygon();
            LinkedGeo.addLinkedLoop(ref polygon, ref inner);
            LinkedGeo.addLinkedLoop(ref polygon, ref outer);

            int result = LinkedGeo.normalizeMultiPolygon(ref polygon);

            Assert.True(result == LinkedGeo.NORMALIZATION_SUCCESS, "No error code returned");

            Assert.True(LinkedGeo.countLinkedPolygons(ref polygon) == 1, "Polygon count correct");
            Assert.True(LinkedGeo.countLinkedLoops(ref polygon) == 2,
                     "Loop count on first polygon correct");
            Assert.True(polygon.first == outer, "Got expected outer loop");
            Assert.True(polygon.first.next == inner, "Got expected inner loop");
        }

        [Test]
        public void normalizeMultiPolygonTwoHoles()
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
            createLinkedLoop(ref outer, verts, 4);

            List<GeoCoord> verts2 =
                new List<GeoCoord>
                {
                    new GeoCoord(0.1, 0.1),
                    new GeoCoord(0.2, 0.2),
                    new GeoCoord(0.1, 0.2)
                };

            LinkedGeo.LinkedGeoLoop inner1 = new LinkedGeo.LinkedGeoLoop();
            Assert.NotNull(inner1);
            createLinkedLoop(ref inner1, verts2, 3);

            List<GeoCoord> verts3 =
                new List<GeoCoord>
                {
                    new GeoCoord(0.2, 0.2),
                    new GeoCoord(0.3, 0.3),
                    new GeoCoord(0.2, 0.3)
                };

            LinkedGeo.LinkedGeoLoop inner2 = new LinkedGeo.LinkedGeoLoop();
            Assert.NotNull(inner2);
            createLinkedLoop(ref inner2, verts3, 3);

            LinkedGeo.LinkedGeoPolygon polygon = new LinkedGeo.LinkedGeoPolygon();
            LinkedGeo.addLinkedLoop(ref polygon, ref inner2);
            LinkedGeo.addLinkedLoop(ref polygon, ref outer);
            LinkedGeo.addLinkedLoop(ref polygon, ref inner1);

            int result = LinkedGeo.normalizeMultiPolygon(ref polygon);

            Assert.True(result == LinkedGeo.NORMALIZATION_SUCCESS, "No error code returned");

            Assert.True
                (
                 LinkedGeo.countLinkedPolygons(ref polygon) == 1,
                 "Polygon count correct for 2 holes"
                );
            Assert.True(polygon.first == outer, "Got expected outer loop");
            Assert.True
                (
                 LinkedGeo.countLinkedLoops(ref polygon) == 3,
                 "Loop count on first polygon correct"
                );
        }

        [Test]
        public void normalizeMultiPolygonTwoDonuts()
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
            createLinkedLoop(ref outer, verts.ToList(), 4);

            GeoCoord[] verts2 =
            {
                new GeoCoord(1, 1),
                new GeoCoord(2, 2),
                new GeoCoord(1, 2)
            };
            LinkedGeo.LinkedGeoLoop inner = new LinkedGeo.LinkedGeoLoop();
            Assert.NotNull(inner);
            createLinkedLoop(ref inner, verts2.ToList(), 3);

            GeoCoord[] verts3 =
            {
                new GeoCoord(0, 0),
                new GeoCoord(0, -3),
                new GeoCoord(-3, -3),
                new GeoCoord(-3, 0)
            };
            LinkedGeo.LinkedGeoLoop outer2 = new LinkedGeo.LinkedGeoLoop();
            Assert.NotNull(outer2);
            createLinkedLoop(ref outer2, verts3.ToList(), 4);

            GeoCoord[] verts4 =
            {
                new GeoCoord(-1, -1),
                new GeoCoord(-2, -2),
                new GeoCoord(-1, -2)
            };
            LinkedGeo.LinkedGeoLoop inner2 = new LinkedGeo.LinkedGeoLoop();
            Assert.NotNull(inner2);
            createLinkedLoop(ref inner2, verts4.ToList(), 3);

            LinkedGeo.LinkedGeoPolygon polygon = new LinkedGeo.LinkedGeoPolygon();
            LinkedGeo.addLinkedLoop(ref polygon, ref inner2);
            LinkedGeo.addLinkedLoop(ref polygon, ref inner);
            LinkedGeo.addLinkedLoop(ref polygon, ref outer);
            LinkedGeo.addLinkedLoop(ref polygon, ref outer2);

            int result = LinkedGeo.normalizeMultiPolygon(ref polygon);

            Assert.True(result == LinkedGeo.NORMALIZATION_SUCCESS, "No error code returned");

            Assert.True(LinkedGeo.countLinkedPolygons(ref polygon) == 2, "Polygon count correct");
            Assert.True
                (
                 LinkedGeo.countLinkedLoops(ref polygon) == 2,
                 "Loop count on first polygon correct"
                );
            Assert.True
                (
                 LinkedGeo.countLinkedCoords(ref polygon.first) == 4,
                 "Got expected outer loop"
                );
            Assert.True
                (
                 LinkedGeo.countLinkedCoords(ref polygon.first.next) == 3,
                 "Got expected inner loop"
                );
            Assert.True
                (
                 LinkedGeo.countLinkedLoops(ref polygon.next) == 2,
                 "Loop count on second polygon correct"
                );
            Assert.True
                (
                 LinkedGeo.countLinkedCoords(ref polygon.next.first) == 4,
                 "Got expected outer loop"
                );
            Assert.True
                (
                 LinkedGeo.countLinkedCoords(ref polygon.next.first.next) == 3,
                 "Got expected inner loop"
                );
        }

        [Test]
        public void normalizeMultiPolygonNestedDonuts()
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
            createLinkedLoop(ref outer, verts.ToList(), 4);

            GeoCoord[] verts2 =
            {
                new GeoCoord(0.1, 0.1),
                new GeoCoord(-0.1, 0.1),
                new GeoCoord(-0.1, -0.1),
                new GeoCoord(0.1, -0.1)
            };
            LinkedGeo.LinkedGeoLoop inner =new LinkedGeo.LinkedGeoLoop();
            Assert.NotNull(inner);
            createLinkedLoop(ref inner, verts2.ToList(), 4);

            GeoCoord[] verts3 =
            {
                new GeoCoord(0.6, 0.6),
                new GeoCoord(0.6, -0.6),
                new GeoCoord(-0.6, -0.6),
                new GeoCoord(-0.6, 0.6)
            };
            LinkedGeo.LinkedGeoLoop outerBig =new LinkedGeo.LinkedGeoLoop();
            Assert.NotNull(outerBig);
            createLinkedLoop(ref outerBig, verts3.ToList(), 4);

            GeoCoord[] verts4 =
            {
                new GeoCoord(0.5, 0.5),
                new GeoCoord(-0.5, 0.5),
                new GeoCoord(-0.5, -0.5),
                new GeoCoord(0.5, -0.5)
            };
            LinkedGeo.LinkedGeoLoop innerBig =new LinkedGeo.LinkedGeoLoop();
            Assert.NotNull(innerBig);
            createLinkedLoop(ref innerBig, verts4.ToList(), 4);

            LinkedGeo.LinkedGeoPolygon polygon = new LinkedGeo.LinkedGeoPolygon();
            LinkedGeo.addLinkedLoop(ref polygon, ref inner);
            LinkedGeo.addLinkedLoop(ref polygon, ref outerBig);
            LinkedGeo.addLinkedLoop(ref polygon, ref innerBig);
            LinkedGeo.addLinkedLoop(ref polygon, ref outer);

            int result = LinkedGeo.normalizeMultiPolygon(ref polygon);

            Assert.True(result == LinkedGeo.NORMALIZATION_SUCCESS, "No error code returned");

            Assert.True(LinkedGeo.countLinkedPolygons(ref polygon) == 2, "Polygon count correct");
            Assert.True(LinkedGeo.countLinkedLoops(ref polygon) == 2,
                     "Loop count on first polygon correct");
            Assert.True(polygon.first == outerBig, "Got expected outer loop");
            Assert.True(polygon.first.next == innerBig, "Got expected inner loop");
            Assert.True(LinkedGeo.countLinkedLoops(ref polygon.next) == 2,
                     "Loop count on second polygon correct");
            Assert.True(polygon.next.first == outer, "Got expected outer loop");
            Assert.True(polygon.next.first.next == inner, "Got expected inner loop");
        }

        [Test]
        public void normalizeMultiPolygonNoOuterLoops()
        {
            GeoCoord[] verts1 =
            {
                new GeoCoord(0, 0),
                new GeoCoord(1, 1),
                new GeoCoord(0, 1)
            };

            LinkedGeo.LinkedGeoLoop outer1 = new LinkedGeo.LinkedGeoLoop();
            Assert.NotNull(outer1);
            createLinkedLoop(ref outer1, verts1.ToList(), 3);

            GeoCoord[] verts2 =
            {
                new GeoCoord(2, 2),
                new GeoCoord(3, 3),
                new GeoCoord(2, 3)
            };

            LinkedGeo.LinkedGeoLoop outer2 = new LinkedGeo.LinkedGeoLoop();
            Assert.NotNull(outer2);
            createLinkedLoop(ref outer2, verts2.ToList(), 3);

            LinkedGeo.LinkedGeoPolygon polygon = new LinkedGeo.LinkedGeoPolygon();
            LinkedGeo.addLinkedLoop(ref polygon, ref outer1);
            LinkedGeo.addLinkedLoop(ref polygon, ref outer2);

            int result = LinkedGeo.normalizeMultiPolygon(ref polygon);

            Assert.True
                (
                 result == LinkedGeo.NORMALIZATION_ERR_UNASSIGNED_HOLES,
                 "Expected error code returned"
                );

            Assert.True(LinkedGeo.countLinkedPolygons(ref polygon) == 1,
                        "Polygon count correct");
            Assert.True
                (
                 LinkedGeo.countLinkedLoops(ref polygon) == 0,
                 "Loop count as expected with invalid input"
                );
        }

        [Test]
        public void normalizeMultiPolygonAlreadyNormalized()
        {
            GeoCoord[] verts1 =
            {
                new GeoCoord(0, 0),
                new GeoCoord(0, 1),
                new GeoCoord(1, 1)
            };

            LinkedGeo.LinkedGeoLoop outer1 = new LinkedGeo.LinkedGeoLoop();
            Assert.NotNull(outer1);
            createLinkedLoop(ref outer1, verts1.ToList(), 3);

            GeoCoord[] verts2 =
            {
                new GeoCoord(2, 2),
                new GeoCoord(2, 3),
                new GeoCoord(3, 3)
            };

            LinkedGeo.LinkedGeoLoop outer2 =new LinkedGeo.LinkedGeoLoop();
            Assert.NotNull(outer2);
            createLinkedLoop(ref outer2, verts2.ToList(), 3);

            LinkedGeo.LinkedGeoPolygon polygon = new LinkedGeo.LinkedGeoPolygon();
            LinkedGeo.addLinkedLoop(ref polygon, ref outer1);
            LinkedGeo.LinkedGeoPolygon next = LinkedGeo.addNewLinkedPolygon(ref polygon);
            LinkedGeo.addLinkedLoop(ref next, ref outer2);

            // Should be a no-op
            int result = LinkedGeo.normalizeMultiPolygon(ref polygon);

            Assert.True(result ==LinkedGeo. NORMALIZATION_ERR_MULTIPLE_POLYGONS,
                     "Expected error code returned");

            Assert.True(LinkedGeo.countLinkedPolygons(ref polygon) == 2, "Polygon count correct");
            Assert.True(LinkedGeo.countLinkedLoops(ref polygon) == 1,
                     "Loop count on first polygon correct");
            Assert.True(polygon.first == outer1, "Got expected outer loop");
            Assert.True(LinkedGeo.countLinkedLoops(ref polygon.next) == 1,
                     "Loop count on second polygon correct");
            Assert.True(polygon.next.first == outer2, "Got expected outer loop");
        }

    }
}
