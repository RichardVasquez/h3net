using System;
using System.Collections.Generic;
using System.Linq;
using H3Net.Code;
using NUnit.Framework;

namespace h3tests
{
    [TestFixture]
    public class TestH3Distance
    {
        private static readonly int[] MAX_DISTANCES = {1, 2, 5, 12, 19, 26};

        private static void h3Distance_identity_assertions(H3Index h3)
        {
            Assert.True(LocalIJ.h3Distance(h3, h3) == 0, "distance to self is 0");
        }

        readonly Action<H3Index> h3DistanceIdentityAssertions = h3Distance_identity_assertions;
        readonly Action<H3Index> h3DistanceKRingassertions = h3Distance_kRing_assertions;

        private static void h3Distance_kRing_assertions(H3Index h3)
        {
            int r = H3Index.H3_GET_RESOLUTION(h3);
            Assert.True(r <= 5, "resolution supported by test function (kRing)");
            int maxK = MAX_DISTANCES[r];

            int sz = Algos.maxKringSize(maxK);
            var neighbors = new H3Index[sz].ToList();
            var distances = new int[sz].ToList();

            Algos.kRingDistances(h3, maxK,ref  neighbors,ref distances);

            for (int i = 0; i < sz; i++) {
                if (neighbors[i] == 0) {
                    continue;
                }

                int calculatedDistance = LocalIJ.h3Distance(h3, neighbors[i]);

                // Don't consider indexes where h3Distance reports failure to
                // generate
                Assert.True(calculatedDistance == distances[i] || calculatedDistance == -1,
                         "kRingDistances matches h3Distance");
            }
        }

        private H3Index bc1 = H3Index.H3_INIT;
        private H3Index bc2 = H3Index.H3_INIT;
        private H3Index bc3 = H3Index.H3_INIT;
        private H3Index pent1 = H3Index.H3_INIT;

        [SetUp]
        public void setCells()
        {
            H3Index.setH3Index(ref bc1, 0, 15, 0);
            H3Index.setH3Index(ref bc2, 0, 8, 0);
            H3Index.setH3Index(ref bc3, 0, 31, 0);
            H3Index.setH3Index(ref pent1, 0, 4, 0);
        }

        [Test]
        public void testIndexDistance()
        {
            H3Index bc = 0;
            H3Index.setH3Index(ref bc, 1, 17, (Direction) 0);
            H3Index p = 0;
            H3Index.setH3Index(ref p, 1, 14, (Direction) 0);
            H3Index p2 = 0;
            H3Index.setH3Index(ref p2, 1, 14, (Direction) 2);
            H3Index p3 = 0;
            H3Index.setH3Index(ref p3, 1, 14, (Direction) 3);
            H3Index p4 = 0;
            H3Index.setH3Index(ref p4, 1, 14, (Direction) 4);
            H3Index p5 = 0;
            H3Index.setH3Index(ref p5, 1, 14, (Direction) 5);
            H3Index p6 = 0;
            H3Index.setH3Index(ref p6, 1, 14, (Direction) 6);

            Assert.True(LocalIJ.h3Distance(bc, p) == 3, "distance onto pentagon");
            Assert.True(LocalIJ.h3Distance(bc, p2) == 2, "distance onto p2");
            Assert.True(LocalIJ.h3Distance(bc, p3) == 3, "distance onto p3");
            // TODO works correctly but is rejected due to possible pentagon
            // distortion.
            //    t_assert(H3_EXPORT(h3Distance)(bc, p4) == 3, "distance onto p4");
            //    t_assert(H3_EXPORT(h3Distance)(bc, p5) == 4, "distance onto p5");
            Assert.True(LocalIJ.h3Distance(bc, p6) == 2, "distance onto p6");
        }

        [Test]
        public void testIndexDistance2()
        {
            H3Index origin = 0x820c4ffffffffffL;
            // Destination is on the other side of the pentagon
            H3Index destination = 0x821ce7fffffffffL;

            // TODO doesn't work because of pentagon distortion. Both should be 5.
            Assert.True(LocalIJ.h3Distance(destination, origin) == -1,
                     "distance in res 2 across pentagon");
            Assert.True(LocalIJ.h3Distance(origin, destination) == -1,
                     "distance in res 2 across pentagon (reversed)");
        }

        [Test]
        public void H3Distance_identity()
        {

            iterateAllIndexesAtRes(0, h3DistanceIdentityAssertions);
            iterateAllIndexesAtRes(1, h3DistanceIdentityAssertions);
            iterateAllIndexesAtRes(2, h3DistanceIdentityAssertions);
        }

        [Test]
        public void h3Distance_kRing()
        {
            iterateAllIndexesAtRes(0, h3DistanceKRingassertions);
            iterateAllIndexesAtRes(1, h3DistanceKRingassertions);
            iterateAllIndexesAtRes(2, h3DistanceKRingassertions);
            // Don't iterate all of res 3, to save time
            iterateAllIndexesAtResPartial(3, h3DistanceKRingassertions, 27);
            // Further resolutions aren't tested to save time.
        }

        [Test]
        public void h3DistanceBaseCells()
        {
            Assert.True(LocalIJ.h3Distance(bc1, pent1) == 1,
                     "distance to neighbor is 1 (15, 4)");
            Assert.True(LocalIJ.h3Distance(bc1, bc2) == 1,
                     "distance to neighbor is 1 (15, 8)");
            Assert.True(LocalIJ.h3Distance(bc1, bc3) == 1,
                     "distance to neighbor is 1 (15, 31)");
            Assert.True(LocalIJ.h3Distance(pent1, bc3) == -1,
                     "distance to neighbor is invalid");
        }

        [Test]
        public void ijkDistance()
        {
            CoordIJK z = new CoordIJK(0, 0, 0);
            CoordIJK i = new CoordIJK(1, 0, 0);
            CoordIJK ik = new CoordIJK(1, 0, 1);
            CoordIJK ij = new CoordIJK(1, 1, 0);
            CoordIJK j2 = new CoordIJK(0, 2, 0);

            Assert.True(CoordIJK.ijkDistance(z, z) == 0, "identity distance 0,0,0");
            Assert.True(CoordIJK.ijkDistance(i, i) == 0, "identity distance 1,0,0");
            Assert.True(CoordIJK.ijkDistance(ik, ik) == 0, "identity distance 1,0,1");
            Assert.True(CoordIJK.ijkDistance(ij, ij) == 0, "identity distance 1,1,0");
            Assert.True(CoordIJK.ijkDistance(j2, j2) == 0, "identity distance 0,2,0");

            Assert.True(CoordIJK.ijkDistance(z, i) == 1, "0,0,0 to 1,0,0");
            Assert.True(CoordIJK.ijkDistance(z, j2) == 2, "0,0,0 to 0,2,0");
            Assert.True(CoordIJK.ijkDistance(z, ik) == 1, "0,0,0 to 1,0,1");
            Assert.True(CoordIJK.ijkDistance(i, ik) == 1, "1,0,0 to 1,0,1");
            Assert.True(CoordIJK.ijkDistance(ik, j2) == 3, "1,0,1 to 0,2,0");
            Assert.True(CoordIJK.ijkDistance(ij, ik) == 2, "1,0,1 to 1,1,0");
        }

        [Test]
        public void h3DistanceResolutionMismatch()
        {
            Assert.True(
                     LocalIJ.h3Distance(0x832830fffffffffL, 0x822837fffffffffL) == -1,
                     "cannot compare at different resolutions");
        }

        [Test]
        public void h3DistanceEdge()
        {
            H3Index origin = 0x832830fffffffffL;
            H3Index dest = 0x832834fffffffffL;
            H3Index edge = H3UniEdge.getH3UnidirectionalEdge(origin, dest);

            Assert.True(0 != edge, "test edge is valid");
            Assert.True(LocalIJ.h3Distance(edge, origin) == 0,
                     "edge has zero distance to origin");
            Assert.True(LocalIJ.h3Distance(origin, edge) == 0,
                     "origin has zero distance to edge");

            Assert.True(LocalIJ.h3Distance(edge, dest) == 1,
                     "edge has distance to destination");
            Assert.True(LocalIJ.h3Distance(edge, dest) == 1,
                     "destination has distance to edge");

        }
        private void iterateAllIndexesAtRes(int res, Action<H3Index> callback)
        {
            iterateAllIndexesAtResPartial(res, callback, Constants.NUM_BASE_CELLS);
        }

        private void iterateAllIndexesAtResPartial(int res, Action<H3Index> callback, int numBaseCells)
        {
            if (numBaseCells > Constants.NUM_BASE_CELLS)
            {
                throw new Exception("assert(baseCells <= NUM_BASE_CELLS);");
            }

            for (int i = 0; i < numBaseCells; i++)
            {
                H3Index bc = 0;
                H3Index.setH3Index(ref bc, 0, i, 0);
                List<H3Index> bc_list = new List<H3Index> {bc};
                int childrenSz = H3Index.maxUncompactSize(ref bc_list, 1, res);
                var children = new H3Index[childrenSz].Select(c => new H3Index()).ToList();
                H3Index.uncompact(ref bc_list, 1, ref children, childrenSz, res);

                for (int j = 0; j < childrenSz; j++)
                {
                    if (children[j] == 0)
                    {
                        continue;
                    }

                    callback(children[j]);
                }
            }
        }
    }
}
