using H3Lib;
using H3Lib.Extensions;
using NUnit.Framework;

namespace TestSuite
{
    [TestFixture]
    public class TestH3Distance
    {
        // Some indexes that represent base cells. Base cells
        // are hexagons except for `pent1`.
        private readonly H3Index bc1 = new H3Index(0, 15, 0);
        private readonly H3Index bc2 = new H3Index(0, 8, 0);
        private readonly H3Index bc3 = new H3Index(0, 31, 0);
        private readonly H3Index pent1 = new H3Index(0, 4, 0);

        [Test]
        public void TestIndexDistance()
        {
            var bc = new H3Index(1, 17, 0);
            var p = new H3Index(1, 14, 0);
            var p2 = new H3Index(1, 14, 2);
            var p3 = new H3Index(1, 14, 3);
            var p4 = new H3Index(1, 14, 4);
            var p5 = new H3Index(1, 14, 5);
            var p6 = new H3Index(1, 14, 6);

            Assert.AreEqual(3, bc.DistanceTo(p));
            Assert.AreEqual(2, bc.DistanceTo(p2));
            Assert.AreEqual(3, bc.DistanceTo(p3));
            // TODO works correctly but is rejected due to possible pentagon
            // distortion.
            //    t_assert(H3_EXPORT(h3Distance)(bc, p4) == 3, "distance onto p4");
            //    t_assert(H3_EXPORT(h3Distance)(bc, p5) == 4, "distance onto p5");
            Assert.AreEqual(2, bc.DistanceTo(p6));
        }

        [Test]
        public void TestIndexDistance2()
        {
            H3Index origin = 0x820c4ffffffffffL;
            // Destination is on the other side of the pentagon
            H3Index destination = 0x821ce7fffffffffL;

            // TODO doesn't work because of pentagon distortion. Both should be 5.
            Assert.AreEqual(-1, destination.DistanceTo(origin));
            Assert.AreEqual(-1, origin.DistanceTo(destination));
        }

        [Test]
        public void H3DistanceBaseCells()
        {
            Assert.AreEqual(1, bc1.DistanceTo(pent1));
            Assert.AreEqual(1, bc1.DistanceTo(bc2));
            Assert.AreEqual(1, bc1.DistanceTo(bc3));
            Assert.AreEqual(-1,pent1.DistanceTo(bc3));
        }
        
        [Test]
        public void IjkDistance()
        {
            var z = new CoordIjk(0, 0, 0);
            var i = new CoordIjk(1, 0, 0);
            var ik = new CoordIjk(1, 0, 1);
            var ij = new CoordIjk(1, 1, 0);
            var j2 = new CoordIjk(0, 2, 0);

            Assert.AreEqual(0, z.DistanceTo(z));
            Assert.AreEqual(0, i.DistanceTo(i));
            Assert.AreEqual(0, ik.DistanceTo(ik));
            Assert.AreEqual(0, ij.DistanceTo(ij));
            Assert.AreEqual(0, j2.DistanceTo(j2));

            Assert.AreEqual(1, z.DistanceTo(i));
            Assert.AreEqual(2, z.DistanceTo(j2));
            Assert.AreEqual(1, z.DistanceTo(ik));
            Assert.AreEqual(1, i.DistanceTo(ik));
            Assert.AreEqual(3, ik.DistanceTo(j2));
            Assert.AreEqual(2, ij.DistanceTo(ik));
        }

        [Test]
        public void H3DistanceResolutionMismatch()
        {
            var h1 = new H3Index(0x832830fffffffffL);
            var h2 = new H3Index(0x822837fffffffffL);
            Assert.AreEqual(-1, h1.DistanceTo(h2));
        }
        
        [Test]
        public void h3DistanceEdge()
        {
            H3Index origin = 0x832830fffffffffL;
            H3Index dest = 0x832834fffffffffL;
            var edge = origin.UniDirectionalEdgeTo(dest);

            Assert.AreNotEqual(0, edge.Value);
            Assert.AreEqual(0,edge.DistanceTo(origin));
            Assert.AreEqual(0,origin.DistanceTo(edge));
            Assert.AreEqual(1,edge.DistanceTo(dest));
            Assert.AreEqual(1,dest.DistanceTo(edge));
        }
    }
}
