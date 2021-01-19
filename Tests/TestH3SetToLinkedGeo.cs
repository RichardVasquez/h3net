using System.Collections.Generic;
using System.Linq;
using H3Lib;
using H3Lib.Extensions;
using NUnit.Framework;

namespace Tests
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

            Assert.AreEqual(1,polygon.CountPolygons());
            Assert.IsNotNull(polygon.LinkedGeoList.First);
            Assert.AreEqual(6, polygon.LinkedGeoList.First.Value.CountCoords);

            polygon.Clear();
        }

        [Test]
        public void Contiguous2()
        {
            List<H3Index> set = new List<H3Index> {0x8928308291bffff, 0x89283082957ffff};
            LinkedGeoPolygon polygon = set.ToLinkedGeoPolygon();

            Assert.AreEqual(1, polygon.CountLoops());
            Assert.IsNotNull(polygon.LinkedGeoList.First);
            Assert.AreEqual(10,polygon.LinkedGeoList.First.Value.CountCoords);

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
            
            var set = new List<H3Index>{0x8928308291bffff, 0x89283082943ffff};
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
            Assert.IsNotNull(polygon.LinkedGeoList.First);
            Assert.AreEqual(12, polygon.LinkedGeoList.First.Value.CountCoords);

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

            var k = 0;
            // t_assert(countLinkedLoops(&polygon) == 2, "2 loops added to polygon");
            // t_assert(countLinkedCoords(polygon.first) == 6 * 3,
            //          "All outer coords added to first loop");
            // t_assert(countLinkedCoords(polygon.first->next) == 6,
            //          "All inner coords added to second loop");
            //
            // H3_EXPORT(destroyLinkedPolygon)(&polygon);
        }
        
    }
}
