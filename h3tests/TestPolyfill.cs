using System.Collections.Generic;
using System.Linq;
using h3net.API;
using NUnit.Framework;

namespace h3tests
{
    [TestFixture]
    public class TestPolyfill
    {
        private static List<GeoCoord> sfVerts =
            new List<GeoCoord>
            {
                new GeoCoord(0.659966917655, -2.1364398519396),
                new GeoCoord(0.6595011102219, -2.1359434279405),
                new GeoCoord(0.6583348114025, -2.1354884206045),
                new GeoCoord(0.6581220034068, -2.1382437718946),
                new GeoCoord(0.6594479998527, -2.1384597563896),
                new GeoCoord(0.6599990002976, -2.1376771158464)
            };

        private static Geofence sfGeofence =
            new Geofence {numVerts = 6, verts = sfVerts.ToArray()};
        private static GeoPolygon sfGeoPolygon = new GeoPolygon();

        private static List<GeoCoord> holeVerts =
            new List<GeoCoord>
            {
                new GeoCoord(0.6595072188743, -2.1371053983433),
                new GeoCoord(0.6591482046471, -2.1373141048153),
                new GeoCoord(0.6592295020837, -2.1365222838402)

            };
        private static Geofence holeGeofence =
            new Geofence{numVerts = 3, verts = holeVerts.ToArray()};
        private static GeoPolygon holeGeoPolygon = new GeoPolygon();

        private static List<GeoCoord> emptyVerts =
            new List<GeoCoord>
            {
                new GeoCoord(0.659966917655, -2.1364398519394),
                new GeoCoord(0.659966917655, -2.1364398519395),
                new GeoCoord(0.659966917655, -2.1364398519396)
            };
        private static Geofence emptyGeofence =
            new Geofence{numVerts = 3, verts = emptyVerts.ToArray() };
        static GeoPolygon emptyGeoPolygon = new GeoPolygon();

        private int countActualHexagons(List<H3Index> hexagons, int numHexagons)
        {
            int actualNumHexagons = 0;
            for (int i = 0; i < numHexagons; i++) {
                if (hexagons[i] != 0) {
                    actualNumHexagons++;
                }
            }
            return actualNumHexagons;
        }

        [Test]
        public void maxPolyfillSize()
        {
            sfGeoPolygon.Geofence = sfGeofence;
            sfGeoPolygon.numHoles = 0;

            holeGeoPolygon.Geofence = sfGeofence;
            holeGeoPolygon.numHoles = 1;
            holeGeoPolygon.holes = new List<Geofence> {holeGeofence};

            emptyGeoPolygon.Geofence = emptyGeofence;
            emptyGeoPolygon.numHoles = 0;

            int numHexagons = Algos.maxPolyfillSize(ref sfGeoPolygon, 9);
            Assert.True(numHexagons == 3169, "got expected max polyfill size");

            numHexagons = Algos.maxPolyfillSize(ref holeGeoPolygon, 9);
            Assert.True(numHexagons == 3169, "got expected max polyfill size (hole)");

            numHexagons = Algos.maxPolyfillSize(ref emptyGeoPolygon, 9);
            Assert.True(numHexagons == 1, "got expected max polyfill size (empty)");
        }

        [Test]
        public void polyfill()
        {
            sfGeoPolygon.Geofence = sfGeofence;
            sfGeoPolygon.numHoles = 0;

            holeGeoPolygon.Geofence = sfGeofence;
            holeGeoPolygon.numHoles = 1;
            holeGeoPolygon.holes = new List<Geofence> {holeGeofence};

            emptyGeoPolygon.Geofence = emptyGeofence;
            emptyGeoPolygon.numHoles = 0;

            int numHexagons = Algos.maxPolyfillSize(ref sfGeoPolygon, 9);
            var hexagons = new ulong[numHexagons].Select(cell => new H3Index(cell)).ToList();

            Algos.polyfill(sfGeoPolygon, 9, hexagons);
            int actualNumHexagons = countActualHexagons(hexagons, numHexagons);

            Assert.True(actualNumHexagons == 1253, "got expected polyfill size");
        }

        [Test]
        public void polyfillHole()
        {
            sfGeoPolygon.Geofence = sfGeofence;
            sfGeoPolygon.numHoles = 0;

            holeGeoPolygon.Geofence = sfGeofence;
            holeGeoPolygon.numHoles = 1;
            holeGeoPolygon.holes = new List<Geofence> {holeGeofence};

            emptyGeoPolygon.Geofence = emptyGeofence;
            emptyGeoPolygon.numHoles = 0;

            int numHexagons = Algos.maxPolyfillSize(ref holeGeoPolygon, 9);
            var hexagons = new ulong[numHexagons].Select(cell => new H3Index(cell)).ToList();

            Algos.polyfill(holeGeoPolygon, 9, hexagons);
            int actualNumHexagons = countActualHexagons(hexagons, numHexagons);

            Assert.True(actualNumHexagons == 1214,
                     "got expected polyfill size (hole)");
        }

        [Test]
        public void polyfillEmpty()
        {
            sfGeoPolygon.Geofence = sfGeofence;
            sfGeoPolygon.numHoles = 0;

            holeGeoPolygon.Geofence = sfGeofence;
            holeGeoPolygon.numHoles = 1;
            holeGeoPolygon.holes = new List<Geofence> {holeGeofence};

            emptyGeoPolygon.Geofence = emptyGeofence;
            emptyGeoPolygon.numHoles = 0;

            int numHexagons = Algos.maxPolyfillSize(ref emptyGeoPolygon, 9);
            var hexagons = new ulong[numHexagons].Select(cell => new H3Index(cell)).ToList();

            Algos.polyfill(emptyGeoPolygon, 9, hexagons);
            int actualNumHexagons = countActualHexagons(hexagons, numHexagons);

            Assert.True(actualNumHexagons == 0, "got expected polyfill size (empty)");
        }

        [Test]
        public void polyfillExact()
        {
            GeoCoord somewhere = new GeoCoord(1, 2);
            H3Index origin = H3Index.geoToH3(ref somewhere, 9);
            GeoBoundary boundary = new GeoBoundary();
            H3Index.h3ToGeoBoundary(origin, ref boundary);

            List<GeoCoord> verts = new GeoCoord [boundary.numVerts + 1].ToList();
            for (int i = 0; i < boundary.numVerts; i++) {
                verts[i] = boundary.verts[i];
            }
            verts[boundary.numVerts] = boundary.verts[0];

            Geofence someGeofence = new Geofence();
            someGeofence.numVerts = boundary.numVerts + 1;
            someGeofence.verts = verts.ToArray();
            GeoPolygon someHexagon = new GeoPolygon();
            someHexagon.Geofence = someGeofence;
            someHexagon.numHoles = 0;

            int numHexagons = Algos.maxPolyfillSize(ref someHexagon, 9);
            var hexagons = new ulong[numHexagons].Select(cell => new H3Index(cell)).ToList();

            Algos.polyfill(someHexagon, 9, hexagons);
            int actualNumHexagons = countActualHexagons(hexagons, numHexagons);

            Assert.True(actualNumHexagons == 1, "got expected polyfill size (1)");
        }

        [Test]
        public void polyfillTransmeridian()
        {
            List<GeoCoord> primeMeridianVerts =
                new List<GeoCoord>
                {
                    new GeoCoord(0.01, 0.01),
                    new GeoCoord(0.01, -0.01),
                    new GeoCoord(-0.01, -0.01),
                    new GeoCoord(-0.01, 0.01)
                };

            Geofence primeMeridianGeofence =
                new Geofence {numVerts = 4, verts = primeMeridianVerts.ToArray()};
            GeoPolygon primeMeridianGeoPolygon =
                new GeoPolygon {Geofence = primeMeridianGeofence, numHoles = 0};

            List<GeoCoord> transMeridianVerts =
                new List<GeoCoord>
                {
                    new GeoCoord(0.01, -Constants.M_PI + 0.01),
                    new GeoCoord(0.01, Constants.M_PI - 0.01),
                    new GeoCoord(-0.01, Constants.M_PI - 0.01),
                    new GeoCoord(-0.01, -Constants.M_PI + 0.01)

                };
            Geofence transMeridianGeofence =
                new Geofence {numVerts = 4, verts = transMeridianVerts.ToArray()};
            GeoPolygon transMeridianGeoPolygon =
                new GeoPolygon {Geofence = transMeridianGeofence, numHoles = 0};

            List<GeoCoord> transMeridianHoleVerts =
                new List<GeoCoord>
                {
                    new GeoCoord(0.005, -Constants.M_PI + 0.005),
                    new GeoCoord(0.005, Constants.M_PI - 0.005),
                    new GeoCoord(-0.005, Constants.M_PI - 0.005),
                    new GeoCoord(-0.005, -Constants.M_PI + 0.005)
                };
            Geofence transMeridianHoleGeofence =
                new Geofence {numVerts = 4, verts = transMeridianHoleVerts.ToArray()};
            GeoPolygon transMeridianHoleGeoPolygon =
                new GeoPolygon
                {
                    Geofence = transMeridianGeofence, numHoles = 1,
                    holes = new List<Geofence> {transMeridianHoleGeofence}
                };
            GeoPolygon transMeridianFilledHoleGeoPolygon =
                new GeoPolygon { Geofence = transMeridianHoleGeofence, numHoles = 0};

            // Prime meridian case
            var expectedSize = 4228;
            int numHexagons =
                Algos.maxPolyfillSize(ref primeMeridianGeoPolygon, 7);
            var hexagons = new ulong[numHexagons].Select(cell => new H3Index(cell)).ToList();

            Algos.polyfill(primeMeridianGeoPolygon, 7, hexagons);
            int actualNumHexagons = countActualHexagons(hexagons, numHexagons);

            Assert.True
                (
                 actualNumHexagons == expectedSize,
                 "got expected polyfill size (prime meridian)"
                );

            // Transmeridian case
            // This doesn't exactly match the prime meridian count because of slight
            // differences in hex size and grid offset between the two cases
            expectedSize = 4238;
            numHexagons = Algos.maxPolyfillSize(ref transMeridianGeoPolygon, 7);
            List<H3Index> hexagonsTM = new ulong[numHexagons].Select(cell => new H3Index(cell)).ToList();

            Algos.polyfill(transMeridianGeoPolygon, 7, hexagonsTM);
            actualNumHexagons = countActualHexagons(hexagonsTM, numHexagons);

            Assert.True
                (
                 actualNumHexagons == expectedSize,
                 "got expected polyfill size (transmeridian)"
                );

            // Transmeridian filled hole case -- only needed for calculating hole
            // size
            numHexagons =
                Algos.maxPolyfillSize(ref transMeridianFilledHoleGeoPolygon, 7);
            List<H3Index> hexagonsTMFH = new ulong[numHexagons].Select(cell => new H3Index(cell)).ToList();

            Algos.polyfill(transMeridianFilledHoleGeoPolygon, 7, hexagonsTMFH);
            int actualNumHoleHexagons = countActualHexagons(hexagonsTMFH, numHexagons);

            // Transmeridian hole case
            numHexagons =
                Algos.maxPolyfillSize(ref transMeridianHoleGeoPolygon, 7);
            List<H3Index> hexagonsTMH = new ulong[numHexagons].Select(cell => new H3Index(cell)).ToList();

            Algos.polyfill(transMeridianHoleGeoPolygon, 7, hexagonsTMH);
            actualNumHexagons = countActualHexagons(hexagonsTMH, numHexagons);

            Assert.True
                (
                 actualNumHexagons == expectedSize - actualNumHoleHexagons,
                 "got expected polyfill size (transmeridian hole)"
                );
        }

        [Test]
        public void polyfillTransmeridianComplex()
        {
            // This polygon is "complex" in that it has > 4 vertices - this
            // tests for a bug that was taking the max and min longitude as
            // the bounds for transmeridian polygons
            List<GeoCoord> verts =
                new List<GeoCoord>
                {
                    new GeoCoord(0.1, -Constants.M_PI + 0.00001),
                    new GeoCoord(0.1, Constants.M_PI - 0.00001),
                    new GeoCoord(0.05, Constants.M_PI - 0.2),
                    new GeoCoord(-0.1, Constants.M_PI - 0.00001),
                    new GeoCoord(0.1, -Constants.M_PI + 0.00001),
                    new GeoCoord(-0.05, -Constants.M_PI + 0.2)
                };
            Geofence Geofence = new Geofence {numVerts = 6, verts = verts.ToArray() };
            GeoPolygon polygon = new GeoPolygon {Geofence = Geofence, numHoles = 0};

            int numHexagons = Algos.maxPolyfillSize(ref polygon, 4);
            List<H3Index> hexagons = new ulong[numHexagons].Select(cell => new H3Index(cell)).ToList();
            Algos.polyfill(polygon, 4, hexagons);

            int actualNumHexagons = countActualHexagons(hexagons, numHexagons);

            Assert.True(actualNumHexagons == 1204,
                     "got expected polyfill size (complex transmeridian)");

        }

    }
}
/*

    TEST(polyfillTransmeridianComplex) {
        // This polygon is "complex" in that it has > 4 vertices - this
        // tests for a bug that was taking the max and min longitude as
        // the bounds for transmeridian polygons
        GeoCoord verts[] = {{0.1, -M_PI + 0.00001},  {0.1, M_PI - 0.00001},
                            {0.05, M_PI - 0.2},      {-0.1, M_PI - 0.00001},
                            {-0.1, -M_PI + 0.00001}, {-0.05, -M_PI + 0.2}};
        Geofence Geofence = {.numVerts = 6, .verts = verts};
        GeoPolygon polygon = {.Geofence = Geofence, .numHoles = 0};

        int numHexagons = H3_EXPORT(maxPolyfillSize)(&polygon, 4);

        H3Index* hexagons = calloc(numHexagons, sizeof(H3Index));
        H3_EXPORT(polyfill)(&polygon, 4, hexagons);

        int actualNumHexagons = countActualHexagons(hexagons, numHexagons);

        t_assert(actualNumHexagons == 1204,
                 "got expected polyfill size (complex transmeridian)");

        free(hexagons);
    }

    TEST(polyfillPentagon) {
        H3Index pentagon;
        setH3Index(&pentagon, 9, 24, 0);
        GeoCoord coord;
        H3_EXPORT(h3ToGeo)(pentagon, &coord);

        // Length of half an edge of the polygon, in radians
        double edgeLength2 = H3_EXPORT(degsToRads)(0.001);

        GeoCoord boundingTopRigt = coord;
        boundingTopRigt.lat += edgeLength2;
        boundingTopRigt.lon += edgeLength2;

        GeoCoord boundingTopLeft = coord;
        boundingTopLeft.lat += edgeLength2;
        boundingTopLeft.lon -= edgeLength2;

        GeoCoord boundingBottomRight = coord;
        boundingBottomRight.lat -= edgeLength2;
        boundingBottomRight.lon += edgeLength2;

        GeoCoord boundingBottomLeft = coord;
        boundingBottomLeft.lat -= edgeLength2;
        boundingBottomLeft.lon -= edgeLength2;

        GeoCoord verts[] = {boundingBottomLeft, boundingTopLeft,
                            boundingTopRigt, boundingBottomRight};

        Geofence Geofence;
        Geofence.verts = verts;
        Geofence.numVerts = 4;

        GeoPolygon polygon;
        polygon.Geofence = Geofence;
        polygon.numHoles = 0;

        int numHexagons = H3_EXPORT(maxPolyfillSize)(&polygon, 9);
        H3Index* hexagons = calloc(numHexagons, sizeof(H3Index));

        H3_EXPORT(polyfill)(&polygon, 9, hexagons);

        int found = 0;
        int numPentagons = 0;
        for (int i = 0; i < numHexagons; i++) {
            if (hexagons[i] != 0) {
                found++;
            }
            if (H3_EXPORT(h3IsPentagon)(hexagons[i])) {
                numPentagons++;
            }
        }
        t_assert(found == 1, "one index found");
        t_assert(numPentagons == 1, "one pentagon found");
        free(hexagons);
    }



*/