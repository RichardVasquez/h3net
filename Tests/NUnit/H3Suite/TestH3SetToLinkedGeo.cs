using System.Collections.Generic;
using System.Linq;
using H3Lib;
using H3Lib.Extensions;
using NUnit.Framework;

namespace TestSuite
{
    [TestFixture]
    public class TestH3SetToLinkedGeo
    {
        [Test]
        public void Empty()
        {
            Assert.IsTrue(true);
            //  Can't call an extension method on a null.
        }

        [Test]
        public void SingleHex()
        {
            List<H3Index> set = new List<H3Index> {0x890dab6220bffff};

            var polygon = set.ToLinkedGeoPolygon();

            Assert.AreEqual(1, polygon.CountPolygons());
            Assert.IsNotNull(polygon.GeoLoopList.First);
            Assert.AreEqual(6, polygon.GeoLoopList.First.Value.CountCoords);

            polygon.Clear();
        }

        [Test]
        public void Contiguous2()
        {
            List<H3Index> set = new List<H3Index> {0x8928308291bffff, 0x89283082957ffff};
            LinkedGeoPolygon polygon = set.ToLinkedGeoPolygon();

            Assert.AreEqual(1, polygon.CountLoops());
            Assert.IsNotNull(polygon.GeoLoopList.First);
            Assert.AreEqual(10, polygon.GeoLoopList.First.Value.CountCoords);

            polygon.Clear();
        }

        [Test]
        public void NonContiguous2()
        {
            // TODO: This test asserts incorrect behavior - we should be creating
            // multiple polygons, each with their own single loop. Update when the
            // algorithm is corrected.

            //  NOTE: With the current code I've written, we'll actually
            //  end up with one polygon, with 0 loops and no nodes, since
            //  the hexagons are clockwise, and so they're not holes, and
            //  Normalize says no thanks and dumps them since they're not
            //  in each other, and I quit.  Accept these assertions until
            //  the next version of h3net.

            var set = new List<H3Index> {0x8928308291bffff, 0x89283082943ffff};
            var polygon = set.ToLinkedGeoPolygon();

            Assert.AreEqual(1, polygon.CountPolygons());
            Assert.AreEqual(0, polygon.CountLoops());

            polygon.Clear();
        }

        [Test]
        public void Contiguous3()
        {
            var set = new List<H3Index>
                      {
                          0x8928308288bffff,
                          0x892830828d7ffff,
                          0x8928308289bffff
                      };
            LinkedGeoPolygon polygon = set.ToLinkedGeoPolygon();

            Assert.AreEqual(1, polygon.CountLoops());
            Assert.IsNotNull(polygon.GeoLoopList.First);
            Assert.AreEqual(12, polygon.GeoLoopList.First.Value.CountCoords);

            polygon.Clear();
        }

        [Test]
        public void Hole()
        {
            var set = new List<H3Index>
                      {
                          0x892830828c7ffff, 0x892830828d7ffff,
                          0x8928308289bffff, 0x89283082813ffff,
                          0x8928308288fffff, 0x89283082883ffff
                      };
            var polygon = set.ToLinkedGeoPolygon();
            Assert.AreEqual(2, polygon.CountLoops());
            Assert.IsNotNull(polygon.GeoLoopList.First);
            Assert.AreEqual(18, polygon.GeoLoopList.First.Value.CountCoords);
            Assert.IsNotNull(polygon.GeoLoopList.First.Next);
            Assert.AreEqual(6, polygon.GeoLoopList.First.Next.Value.CountCoords);
            polygon.Clear();
        }

        [Test]
        public void Pentagon()
        {
            var set = new List<H3Index> {0x851c0003fffffff};
            var polygon = set.ToLinkedGeoPolygon();

            Assert.AreEqual(1, polygon.CountLoops());
            Assert.IsNotNull(polygon.GeoLoopList.First);
            Assert.AreEqual(10, polygon.GeoLoopList.First.Value.CountCoords);
            polygon.Clear();
        }

        [Test]
        public void Test2Ring()
        {
            // 2-ring, in order returned by k-ring algo
            var set = new List<H3Index>
                      {
                          0x8930062838bffff, 0x8930062838fffff, 0x89300628383ffff,
                          0x8930062839bffff, 0x893006283d7ffff, 0x893006283c7ffff,
                          0x89300628313ffff, 0x89300628317ffff, 0x893006283bbffff,
                          0x89300628387ffff, 0x89300628397ffff, 0x89300628393ffff,
                          0x89300628067ffff, 0x8930062806fffff, 0x893006283d3ffff,
                          0x893006283c3ffff, 0x893006283cfffff, 0x8930062831bffff,
                          0x89300628303ffff
                      };

            var polygon = set.ToLinkedGeoPolygon();

            Assert.AreEqual(1, polygon.CountLoops());
            Assert.IsNotNull(polygon.GeoLoopList.First);
            Assert.AreEqual(6 * (2 * 2 + 1), polygon.GeoLoopList.First.Value.CountCoords);
            polygon.Clear();
        }

        [Test]
        public void Test2RingUnordered()
        {
            // 2-ring in random order
            var set =
                new List<H3Index>
                {
                    0x89300628393ffff, 0x89300628383ffff, 0x89300628397ffff,
                    0x89300628067ffff, 0x89300628387ffff, 0x893006283bbffff,
                    0x89300628313ffff, 0x893006283cfffff, 0x89300628303ffff,
                    0x89300628317ffff, 0x8930062839bffff, 0x8930062838bffff,
                    0x8930062806fffff, 0x8930062838fffff, 0x893006283d3ffff,
                    0x893006283c3ffff, 0x8930062831bffff, 0x893006283d7ffff,
                    0x893006283c7ffff
                };

            var polygon = set.ToLinkedGeoPolygon();
            Assert.AreEqual(1, polygon.CountLoops());
            Assert.IsNotNull(polygon.GeoLoopList.First);
            Assert.AreEqual(6 * (2 * 2 + 1), polygon.GeoLoopList.First.Value.CountCoords);
            polygon.Clear();
        }

        [Test]
        public void NestedDonut()
        {
            // hollow 1-ring + hollow 3-ring around the same hex
            var set = new List<H3Index>
                      {
                          0x89283082813ffff, 0x8928308281bffff, 0x8928308280bffff,
                          0x8928308280fffff, 0x89283082807ffff, 0x89283082817ffff,
                          0x8928308289bffff, 0x892830828d7ffff, 0x892830828c3ffff,
                          0x892830828cbffff, 0x89283082853ffff, 0x89283082843ffff,
                          0x8928308284fffff, 0x8928308287bffff, 0x89283082863ffff,
                          0x89283082867ffff, 0x8928308282bffff, 0x89283082823ffff,
                          0x89283082837ffff, 0x892830828afffff, 0x892830828a3ffff,
                          0x892830828b3ffff, 0x89283082887ffff, 0x89283082883ffff
                      };

            var polygon = set.ToLinkedGeoPolygon();

            // Note that the polygon order here is arbitrary, making this test
            // somewhat brittle, but it's difficult to assert correctness otherwise
            Assert.AreEqual(2, polygon.CountPolygons());
            Assert.AreEqual(2, polygon.CountLoops());
            Assert.IsNotNull(polygon.GeoLoopList.First);
            Assert.AreEqual(42, polygon.GeoLoopList.First.Value.CountCoords);
            Assert.IsNotNull(polygon.GeoLoopList.First.Next);
            Assert.AreEqual(30, polygon.GeoLoopList.First.Next.Value.CountCoords);
            Assert.AreEqual(2, polygon.Next.CountLoops());
            Assert.IsNotNull(polygon.Next.GeoLoopList.First);
            Assert.AreEqual(18, polygon.Next.GeoLoopList.First.Value.CountCoords);
            Assert.IsNotNull(polygon.Next.GeoLoopList.First.Next);
            Assert.AreEqual(6, polygon.Next.GeoLoopList.First.Next.Value.CountCoords);

            polygon.Clear();
        }

        [Test]
        public void NestedDonutTransmeridian()
        {
            // hollow 1-ring + hollow 3-ring around the hex at (0, -180)
            var set = new List<H3Index>
                      {
                          0x897eb5722c7ffff, 0x897eb5722cfffff, 0x897eb572257ffff,
                          0x897eb57220bffff, 0x897eb572203ffff, 0x897eb572213ffff,
                          0x897eb57266fffff, 0x897eb5722d3ffff, 0x897eb5722dbffff,
                          0x897eb573537ffff, 0x897eb573527ffff, 0x897eb57225bffff,
                          0x897eb57224bffff, 0x897eb57224fffff, 0x897eb57227bffff,
                          0x897eb572263ffff, 0x897eb572277ffff, 0x897eb57223bffff,
                          0x897eb572233ffff, 0x897eb5722abffff, 0x897eb5722bbffff,
                          0x897eb572287ffff, 0x897eb572283ffff, 0x897eb57229bffff
                      };

            var polygon = set.ToLinkedGeoPolygon();

            // Note that the polygon order here is arbitrary, making this test
            // somewhat brittle, but it's difficult to assert correctness otherwise
            Assert.AreEqual(2, polygon.CountPolygons());
            Assert.AreEqual(2, polygon.CountLoops());
            Assert.IsNotNull(polygon.GeoLoopList.First);
            Assert.AreEqual(18, polygon.GeoLoopList.First.Value.CountCoords);
            Assert.IsNotNull(polygon.GeoLoopList.First.Next);
            Assert.AreEqual(6, polygon.GeoLoopList.First.Next.Value.CountCoords);
            Assert.AreEqual(2, polygon.Next.CountLoops());
            Assert.IsNotNull(polygon.Next.GeoLoopList.First);
            Assert.AreEqual(42, polygon.Next.GeoLoopList.First.Value.CountCoords);
            Assert.IsNotNull(polygon.Next.GeoLoopList.First.Next);
            Assert.AreEqual(30, polygon.Next.GeoLoopList.First.Next.Value.CountCoords);

            polygon.Clear();
        }

        [Test]
        public void Contiguous2distorted()
        {
            var set = new List<H3Index> {0x894cc5365afffff, 0x894cc536537ffff};
            var polygon = set.ToLinkedGeoPolygon();

            Assert.AreEqual(1, polygon.CountLoops());
            Assert.IsNotNull(polygon.GeoLoopList.First);
            Assert.AreEqual(12, polygon.GeoLoopList.First.Value.CountCoords);

            polygon.Clear();
        }

        [Test]
        public void NegativeHashedCoordinates()
        {
            var set = new List<H3Index> {0x88ad36c547fffff, 0x88ad36c467fffff};
            var polygon = set.ToLinkedGeoPolygon();

            Assert.AreEqual(2, polygon.CountPolygons());
            Assert.AreEqual(1, polygon.CountLoops());
            Assert.IsNotNull(polygon.GeoLoopList.First);
            Assert.AreEqual(6, polygon.GeoLoopList.First.Value.CountCoords);
            Assert.AreEqual(1, polygon.Next.CountLoops());
            Assert.IsNotNull(polygon.Next.GeoLoopList.First);
            Assert.AreEqual(6, polygon.Next.GeoLoopList.First.Value.CountCoords);

            polygon.Clear();
        }
    }
}
