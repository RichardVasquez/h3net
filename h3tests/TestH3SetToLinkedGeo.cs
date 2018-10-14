using h3net.API;
using NUnit.Framework;
using System.Collections.Generic;


namespace h3tests
{
    [TestFixture]
    public class TestH3SetToLinkedGeo
    {
        [Test]
        public void empty()
        {
            LinkedGeo.LinkedGeoPolygon polygon = new LinkedGeo.LinkedGeoPolygon();
            List<H3Index> nullset = new List<H3Index>();
            Algos.h3SetToLinkedGeo(ref nullset, 0, ref polygon);

            Assert.True(LinkedGeo.countLinkedLoops(ref polygon) == 0, "No loops added to polygon");

            LinkedGeo.destroyLinkedPolygon(ref polygon);
        }

        [Test]
        public void singleHex()
        {
            LinkedGeo.LinkedGeoPolygon polygon = new LinkedGeo.LinkedGeoPolygon();
            List<H3Index> set = new List<H3Index>{0x890dab6220bffff};
            int numHexes = set.Count;

            Algos.h3SetToLinkedGeo(ref set, numHexes, ref polygon);

            Assert.True(LinkedGeo.countLinkedLoops(ref polygon) == 1, "1 loop added to polygon");
            Assert.True(LinkedGeo.countLinkedCoords(ref polygon.first) == 6,
                     "6 coords added to loop");

            LinkedGeo.destroyLinkedPolygon(ref polygon);
        }

        [Test]
        public void contiguous2()
        {
            LinkedGeo.LinkedGeoPolygon polygon = new LinkedGeo.LinkedGeoPolygon();
            List<H3Index> set = new List<H3Index>{0x8928308291bffff, 0x89283082957ffff};
            int numHexes = set.Count;

            Algos.h3SetToLinkedGeo(ref set, numHexes, ref polygon);

            Assert.True(LinkedGeo.countLinkedLoops(ref polygon) == 1, "1 loop added to polygon");
            Assert.True(LinkedGeo.countLinkedCoords(ref polygon.first) == 10,
                     "All coords added to loop except 2 shared");

            LinkedGeo.destroyLinkedPolygon(ref polygon);
        }

        // TODO: This test asserts incorrect behavior - we should be creating
        // multiple polygons, each with their own single loop. Update when the
        // algorithm is corrected.
        [Test]
        public void nonContiguous2()
        {
            LinkedGeo.LinkedGeoPolygon polygon = new LinkedGeo.LinkedGeoPolygon();
            List<H3Index> set = new List<H3Index>{0x8928308291bffff, 0x89283082943ffff};
            int numHexes = set.Count;

            Algos.h3SetToLinkedGeo(ref set, numHexes, ref polygon);

            Assert.True(LinkedGeo.countLinkedPolygons(ref polygon) == 2, "2 polygons added");
            Assert.True(LinkedGeo.countLinkedLoops(ref polygon) == 1, "1 loop on the first polygon");
            Assert.True(LinkedGeo.countLinkedCoords(ref polygon.first) == 6,
                     "All coords for one hex added to first loop");
            Assert.True(LinkedGeo.countLinkedLoops(ref polygon.next) == 1,
                     "Loop count on second polygon correct");
            Assert.True(LinkedGeo.countLinkedCoords(ref polygon.next.first) == 6,
                     "All coords for one hex added to second polygon");

            LinkedGeo.destroyLinkedPolygon(ref polygon);
        }

        [Test]
        public void contiguous3()
        {
            LinkedGeo.LinkedGeoPolygon polygon = new LinkedGeo.LinkedGeoPolygon();
            List<H3Index> set = new List<H3Index>{0x8928308288bffff, 0x892830828d7ffff,
                0x8928308289bffff};
            int numHexes = set.Count;

            Algos.h3SetToLinkedGeo(ref set, numHexes, ref polygon);

            Assert.True(LinkedGeo.countLinkedLoops(ref polygon) == 1, "1 loop added to polygon");
            Assert.True(LinkedGeo.countLinkedCoords(ref polygon.first) == 12,
                     "All coords added to loop except 6 shared");

            LinkedGeo.destroyLinkedPolygon(ref polygon);
        }

        [Test]
        public void hole()
        {
            LinkedGeo.LinkedGeoPolygon polygon = new LinkedGeo.LinkedGeoPolygon();
            List<H3Index> set = new List<H3Index> {0x892830828c7ffff, 0x892830828d7ffff,
                0x8928308289bffff, 0x89283082813ffff,
                0x8928308288fffff, 0x89283082883ffff};
            int numHexes = set.Count;

            Algos.h3SetToLinkedGeo(ref set, numHexes, ref polygon);

            Assert.True(LinkedGeo.countLinkedLoops(ref polygon) == 2, "2 loops added to polygon");
            Assert.True(LinkedGeo.countLinkedCoords(ref polygon.first) == 6 * 3,
                     "All outer coords added to first loop");
            Assert.True(LinkedGeo.countLinkedCoords(ref polygon.first.next) == 6,
                     "All inner coords added to second loop");

            LinkedGeo.destroyLinkedPolygon(ref polygon);
        }

        [Test]
        public void     pentagon()
        {
            LinkedGeo.LinkedGeoPolygon polygon = new LinkedGeo.LinkedGeoPolygon();
            List<H3Index> set = new List<H3Index>{0x851c0003fffffff};
            int numHexes = set.Count;

            Algos.h3SetToLinkedGeo(ref set, numHexes, ref polygon);

            Assert.True(LinkedGeo.countLinkedLoops(ref polygon) == 1, "1 loop added to polygon");
            Assert.True(LinkedGeo.countLinkedCoords(ref polygon.first) == 10,
                     "10 coords (distorted pentagon) added to loop");

            LinkedGeo.destroyLinkedPolygon(ref polygon);
        }

        [Test]
        public void test2Ring() {
            LinkedGeo.LinkedGeoPolygon polygon = new LinkedGeo.LinkedGeoPolygon();
            // 2-ring, in order returned by k-ring algo
            List<H3Index> set = new List<H3Index> {
                0x8930062838bffff, 0x8930062838fffff, 0x89300628383ffff,
                0x8930062839bffff, 0x893006283d7ffff, 0x893006283c7ffff,
                0x89300628313ffff, 0x89300628317ffff, 0x893006283bbffff,
                0x89300628387ffff, 0x89300628397ffff, 0x89300628393ffff,
                0x89300628067ffff, 0x8930062806fffff, 0x893006283d3ffff,
                0x893006283c3ffff, 0x893006283cfffff, 0x8930062831bffff,
                0x89300628303ffff};
            int numHexes = set.Count;

            Algos.h3SetToLinkedGeo(ref set, numHexes, ref polygon);

            Assert.True(LinkedGeo.countLinkedLoops(ref polygon) == 1, "1 loop added to polygon");
            Assert.True(LinkedGeo.countLinkedCoords(ref polygon.first) == (6 * (2 * 2 + 1)),
                     "Expected number of coords added to loop");

            LinkedGeo.destroyLinkedPolygon(ref polygon);
        }

        [Test]
        public void     test2RingUnordered()
        {
            LinkedGeo.LinkedGeoPolygon polygon = new LinkedGeo.LinkedGeoPolygon();
            // 2-ring in random order
            List<H3Index> set = new List<H3Index>
                                {
                                    0x89300628393ffff, 0x89300628383ffff, 0x89300628397ffff,
                                    0x89300628067ffff, 0x89300628387ffff, 0x893006283bbffff,
                                    0x89300628313ffff, 0x893006283cfffff, 0x89300628303ffff,
                                    0x89300628317ffff, 0x8930062839bffff, 0x8930062838bffff,
                                    0x8930062806fffff, 0x8930062838fffff, 0x893006283d3ffff,
                                    0x893006283c3ffff, 0x8930062831bffff, 0x893006283d7ffff,
                                    0x893006283c7ffff
                                };
            int numHexes = set.Count;

            Algos.h3SetToLinkedGeo(ref set, numHexes, ref polygon);

            Assert.True(LinkedGeo.countLinkedLoops(ref polygon) == 1, "1 loop added to polygon");
            Assert.True(LinkedGeo.countLinkedCoords(ref polygon.first) == (6 * (2 * 2 + 1)),
                     "Expected number of coords added to loop");

            LinkedGeo.destroyLinkedPolygon(ref polygon);
        }

        [Test]
        public void nestedDonut()
        {
            LinkedGeo.LinkedGeoPolygon polygon = new LinkedGeo.LinkedGeoPolygon();
            // hollow 1-ring + hollow 3-ring around the same hex
            List<H3Index> set= new List<H3Index>{
                0x89283082813ffff, 0x8928308281bffff, 0x8928308280bffff,
                0x8928308280fffff, 0x89283082807ffff, 0x89283082817ffff,
                0x8928308289bffff, 0x892830828d7ffff, 0x892830828c3ffff,
                0x892830828cbffff, 0x89283082853ffff, 0x89283082843ffff,
                0x8928308284fffff, 0x8928308287bffff, 0x89283082863ffff,
                0x89283082867ffff, 0x8928308282bffff, 0x89283082823ffff,
                0x89283082837ffff, 0x892830828afffff, 0x892830828a3ffff,
                0x892830828b3ffff, 0x89283082887ffff, 0x89283082883ffff};
            int numHexes = set.Count;

            Algos.h3SetToLinkedGeo(ref set, numHexes, ref polygon);

            // Note that the polygon order here is arbitrary, making this test
            // somewhat brittle, but it's difficult to assert correctness otherwise
            Assert.True(LinkedGeo.countLinkedPolygons(ref polygon) == 2, "Polygon count correct");
            Assert.True(LinkedGeo.countLinkedLoops(ref polygon) == 2,
                     "Loop count on first polygon correct");
            Assert.True(LinkedGeo.countLinkedCoords(ref polygon.first) == 42,
                     "Got expected big outer loop");
            Assert.True(LinkedGeo.countLinkedCoords(ref polygon.first.next) == 30,
                     "Got expected big inner loop");
            Assert.True(LinkedGeo.countLinkedLoops(ref polygon.next) == 2,
                     "Loop count on second polygon correct");
            Assert.True(LinkedGeo.countLinkedCoords(ref polygon.next.first) == 18,
                     "Got expected outer loop");
            Assert.True(LinkedGeo.countLinkedCoords(ref polygon.next.first.next) == 6,
                     "Got expected inner loop");

            LinkedGeo.destroyLinkedPolygon(ref polygon);
        }

        [Test]
        public void nestedDonutTransmeridian()
        {
            LinkedGeo.LinkedGeoPolygon polygon = new LinkedGeo.LinkedGeoPolygon();
            // hollow 1-ring + hollow 3-ring around the hex at (0, -180)
            List<H3Index> set = new List<H3Index> {
                0x897eb5722c7ffff, 0x897eb5722cfffff, 0x897eb572257ffff,
                0x897eb57220bffff, 0x897eb572203ffff, 0x897eb572213ffff,
                0x897eb57266fffff, 0x897eb5722d3ffff, 0x897eb5722dbffff,
                0x897eb573537ffff, 0x897eb573527ffff, 0x897eb57225bffff,
                0x897eb57224bffff, 0x897eb57224fffff, 0x897eb57227bffff,
                0x897eb572263ffff, 0x897eb572277ffff, 0x897eb57223bffff,
                0x897eb572233ffff, 0x897eb5722abffff, 0x897eb5722bbffff,
                0x897eb572287ffff, 0x897eb572283ffff, 0x897eb57229bffff};
            int numHexes = set.Count;

            Algos.h3SetToLinkedGeo(ref set, numHexes, ref polygon);

            // Note that the polygon order here is arbitrary, making this test
            // somewhat brittle, but it's difficult to assert correctness otherwise
            Assert.True(LinkedGeo.countLinkedPolygons(ref polygon) == 2, "Polygon count correct");
            Assert.True(LinkedGeo.countLinkedLoops(ref polygon) == 2,
                     "Loop count on first polygon correct");
            Assert.True(LinkedGeo.countLinkedCoords(ref polygon.first) == 18,
                     "Got expected outer loop");
            Assert.True(LinkedGeo.countLinkedCoords(ref polygon.first.next) == 6,
                     "Got expected inner loop");
            Assert.True(LinkedGeo.countLinkedLoops(ref polygon.next) == 2,
                     "Loop count on second polygon correct");
            Assert.True(LinkedGeo.countLinkedCoords(ref polygon.next.first) == 42,
                     "Got expected big outer loop");
            Assert.True(LinkedGeo.countLinkedCoords(ref polygon.next.first.next) == 30,
                     "Got expected big inner loop");

            LinkedGeo.destroyLinkedPolygon(ref polygon);
        }

        [Test]
        public void contiguous2distorted()
        {
            LinkedGeo.LinkedGeoPolygon polygon = new LinkedGeo.LinkedGeoPolygon();
            List<H3Index> set = new List<H3Index> {0x894cc5365afffff, 0x894cc536537ffff};
            int numHexes = set.Count;

            Algos.h3SetToLinkedGeo(ref set, numHexes, ref polygon);

            Assert.True(LinkedGeo.countLinkedLoops(ref polygon) == 1, "1 loop added to polygon");
            Assert.True(LinkedGeo.countLinkedCoords(ref polygon.first) == 12,
                     "All coords added to loop except 2 shared");

            LinkedGeo.destroyLinkedPolygon(ref polygon);
        }

        [Test]
        public void negativeHashedCoordinates()
        {
            LinkedGeo.LinkedGeoPolygon polygon = new LinkedGeo.LinkedGeoPolygon();
            List<H3Index> set = new List<H3Index> {0x88ad36c547fffff, 0x88ad36c467fffff};
            int numHexes = set.Count;
            Algos.h3SetToLinkedGeo(ref set, numHexes, ref polygon);

            Assert.True(LinkedGeo.countLinkedPolygons(ref polygon) == 2, "2 polygons added");
            Assert.True(LinkedGeo.countLinkedLoops(ref polygon) == 1,
                     "1 loop on the first polygon");
            Assert.True(LinkedGeo.countLinkedCoords(ref polygon.first) == 6,
                     "All coords for one hex added to first loop");
            Assert.True(LinkedGeo.countLinkedLoops(ref polygon.next) == 1,
                     "Loop count on second polygon correct");
            Assert.True(LinkedGeo.countLinkedCoords(ref polygon.next.first) == 6,
                     "All coords for one hex added to second polygon");

            LinkedGeo.destroyLinkedPolygon(ref polygon);
        }

    }
}
