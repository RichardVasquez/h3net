using System;
using System.Collections.Generic;
using System.Linq;
using h3net.API;
using NUnit.Framework;

namespace h3tests
{
    [TestFixture]
    public class TestKRing
    {
        private void kRing_equals_kRingInternal_assertions(H3Index h3)
        {
            for (int k = 0; k < 3; k++) {
                int kSz = Algos.maxKringSize(k);

                var neighbors = new H3Index[kSz].Select(c => (H3Index) 0).ToList();
                var distances = new int[kSz].Select(c => 0).ToList();

                Algos.kRingDistances(h3, k, ref neighbors,ref  distances);

                var internalNeighbors = new H3Index[kSz].Select(c => (H3Index) 0).ToList();
                var internalDistances = new int[kSz].Select(c => 0).ToList();

                Algos._kRingInternal(h3, k, ref internalNeighbors, ref internalDistances, kSz, 0);

                int found = 0;
                int internalFound = 0;
                for (int iNeighbor = 0; iNeighbor < kSz; iNeighbor++) {
                    if (neighbors[iNeighbor] != 0) {
                        found++;

                        for (int iInternal = 0; iInternal < kSz; iInternal++) {
                            if (internalNeighbors[iInternal] == neighbors[iNeighbor]) {
                                internalFound++;

                                Assert.True(distances[iNeighbor] ==
                                         internalDistances[iInternal],
                                         "External and internal agree on distance");

                                break;
                            }
                        }

                        Assert.True(found == internalFound,
                                 "External and internal implementations produce same output");
                    }
                }
            }
        }


        [Test]
        public void kRing0()
        {
            GeoCoord sf = new GeoCoord(0.659966917655, 2 * 3.14159 - 2.1364398519396);
            H3Index sfHex0 = H3Index.geoToH3(ref sf, 0);

            var k1 = new List<H3Index>{0, 0, 0, 0, 0, 0, 0};
            var k1Dist = new List<int>{0, 0, 0, 0, 0, 0, 0};
            H3Index[] expectedK1 = {0x8029fffffffffff, 0x801dfffffffffff,
                0x8013fffffffffff, 0x8027fffffffffff,
                0x8049fffffffffff, 0x8051fffffffffff,
                0x8037fffffffffff};
            Algos.kRingDistances(sfHex0, 1, ref k1, ref k1Dist);

            for (int i = 0; i < 7; i++)
            {
                Assert.True(k1[i] != 0, "index is populated");
                int inList = 0;
                for (int j = 0; j < 7; j++) 
                {
                    if (k1[i] != expectedK1[j])
                    {
                        continue;
                    }
                    Assert.True(k1Dist[i] == (k1[i] == sfHex0 ? 0 : 1),
                                "distance is as expected");
                    inList++;
                }
                Assert.True(inList == 1, "index found in expected set");
            }
        }

        [Test]
        public void kRing0_polarPentagon()
        {
            H3Index polar = 0;
            H3Index.setH3Index(ref polar, 0, 4, 0);
            var k2 = new List<H3Index>{0, 0, 0, 0, 0, 0, 0};
            var k2Dist = new List<int> {0, 0, 0, 0, 0, 0, 0};
            var expectedK2 = new List<H3Index> {0x8009fffffffffff,
                0x8007fffffffffff,
                0x8001fffffffffff,
                0x8011fffffffffff,
                0x801ffffffffffff,
                0x8019fffffffffff,
                0};
            Algos.kRingDistances(polar, 1, ref k2, ref k2Dist);

            int k2present = 0;
            for (int i = 0; i < 7; i++) {
                if (k2[i] != 0) {
                    k2present++;
                    int inList = 0;
                    for (int j = 0; j < 7; j++) {
                        if (k2[i] == expectedK2[j]) {
                            Assert.True(k2Dist[i] == (k2[i] == polar ? 0 : 1),
                                     "distance is as expected");
                            inList++;
                        }
                    }
                    Assert.True(inList == 1, "index found in expected set");
                }
            }
            Assert.True(k2present == 6, "pentagon has 5 neighbors");
        }

        [Test]
        public void kRing1_PolarPentagon()
        {
            H3Index polar = 0;
            H3Index.setH3Index(ref polar, 1, 4, 0);
            var k2 = new List<H3Index> {0, 0, 0, 0, 0, 0, 0};
            var k2Dist = new List<int> {0, 0, 0, 0, 0, 0, 0};
            var expectedK2 = new List<H3Index> {0x81083ffffffffff,
                0x81093ffffffffff,
                0x81097ffffffffff,
                0x8108fffffffffff,
                0x8108bffffffffff,
                0x8109bffffffffff,
                0};
            Algos.kRingDistances(polar, 1, ref k2, ref k2Dist);

            int k2present = 0;
            for (int i = 0; i < 7; i++) {
                if (k2[i] != 0) {
                    k2present++;
                    int inList = 0;
                    for (int j = 0; j < 7; j++) {
                        if (k2[i] == expectedK2[j]) {
                            Assert.True(k2Dist[i] == (k2[i] == polar ? 0 : 1),
                                     "distance is as expected");
                            inList++;
                        }
                    }
                    Assert.True(inList == 1, "index found in expected set");
                }
            }
            Assert.True(k2present == 6, "pentagon has 5 neighbors");
        }

        [Test]
        public void kRing1_PolarPentagon_k3()
        {
            H3Index polar = 0;
            H3Index.setH3Index(ref polar, 1, 4, 0);
            var k2 = new H3Index[37].Select(c => (H3Index) 0).ToList();
            var k2Dist = new int[37].ToList();
            var expectedK2 =
                new List<H3Index>
                {
                    0x81013ffffffffff, 0x811fbffffffffff, 0x81193ffffffffff,
                    0x81097ffffffffff, 0x81003ffffffffff, 0x81183ffffffffff,
                    0x8111bffffffffff, 0x81077ffffffffff, 0x811f7ffffffffff,
                    0x81067ffffffffff, 0x81093ffffffffff, 0x811e7ffffffffff,
                    0x81083ffffffffff, 0x81117ffffffffff, 0x8101bffffffffff,
                    0x81107ffffffffff, 0x81073ffffffffff, 0x811f3ffffffffff,
                    0x81063ffffffffff, 0x8108fffffffffff, 0x811e3ffffffffff,
                    0x8119bffffffffff, 0x81113ffffffffff, 0x81017ffffffffff,
                    0x81103ffffffffff, 0x8109bffffffffff, 0x81197ffffffffff,
                    0x81007ffffffffff, 0x8108bffffffffff, 0x81187ffffffffff,
                    0x8107bffffffffff, 0, 0, 0, 0, 0, 0
                };
            int[] expectedK2Dist =
            {
                2, 3, 2, 1, 3, 3, 3, 2, 2, 3, 1, 3, 0,
                2, 3, 3, 2, 2, 3, 1, 3, 3, 2, 2, 3, 1,
                2, 3, 1, 3, 3, 0, 0, 0, 0, 0, 0
            };
            Algos.kRingDistances(polar, 3, ref k2, ref k2Dist);

            int k2present = 0;
            for (int i = 0; i < 37; i++)
            {
                if (k2[i] != 0)
                {
                    k2present++;
                    int inList = 0;
                    for (int j = 0; j < 37; j++)
                    {
                        if (k2[i] == expectedK2[j])
                        {
                            Assert.True
                                (
                                 k2Dist[i] == expectedK2Dist[j],
                                 "distance is as expected"
                                );
                            inList++;
                        }
                    }
                    Assert.True(inList == 1, "index found in expected set");
                }
            }
            Assert.True(k2present == 31, "pentagon has 30 neighbors");
        }

        [Test]
        public void kRing1_Pentagon_k4()
        {
            H3Index pent = 0;
            H3Index.setH3Index(ref pent, 1, 14, 0);
            var k2 = new H3Index[61].Select(c => (H3Index) 0).ToList();
            var k2Dist = new int[61].ToList();
            var expectedK2 =
                new List<H3Index>
                {
                    0x811d7ffffffffff, 0x810c7ffffffffff, 0x81227ffffffffff,
                    0x81293ffffffffff, 0x81133ffffffffff, 0x8136bffffffffff,
                    0x81167ffffffffff, 0x811d3ffffffffff, 0x810c3ffffffffff,
                    0x81223ffffffffff, 0x81477ffffffffff, 0x8128fffffffffff,
                    0x81367ffffffffff, 0x8112fffffffffff, 0x811cfffffffffff,
                    0x8123bffffffffff, 0x810dbffffffffff, 0x8112bffffffffff,
                    0x81473ffffffffff, 0x8128bffffffffff, 0x81363ffffffffff,
                    0x811cbffffffffff, 0x81237ffffffffff, 0x810d7ffffffffff,
                    0x81127ffffffffff, 0x8137bffffffffff, 0x81287ffffffffff,
                    0x8126bffffffffff, 0x81177ffffffffff, 0x810d3ffffffffff,
                    0x81233ffffffffff, 0x8150fffffffffff, 0x81123ffffffffff,
                    0x81377ffffffffff, 0x81283ffffffffff, 0x8102fffffffffff,
                    0x811c3ffffffffff, 0x810cfffffffffff, 0x8122fffffffffff,
                    0x8113bffffffffff, 0x81373ffffffffff, 0x8129bffffffffff,
                    0x8102bffffffffff, 0x811dbffffffffff, 0x810cbffffffffff,
                    0x8122bffffffffff, 0x81297ffffffffff, 0x81507ffffffffff,
                    0x8136fffffffffff, 0x8127bffffffffff, 0x81137ffffffffff,
                    0, 0, 0, 0, 0, 0, 0, 0, 0, 0
                };
            Algos.kRingDistances(pent, 4, ref k2, ref k2Dist);

            int k2present = 0;
            for (int i = 0; i < 61; i++)
            {
                if (k2[i] != 0)
                {
                    k2present++;
                    int inList = 0;
                    for (int j = 0; j < 61; j++)
                    {
                        if (k2[i] == expectedK2[j])
                        {
                            inList++;
                        }
                    }

                    Assert.True(inList == 1, "index found in expected set");
                }
            }
            Assert.True(k2present == 51, "pentagon has 50 neighbors");
        }

        [Test]
        public void kRing_equals_kRingInternal()
        {
            // Check that kRingDistances output matches _kRingInternal,
            // since kRingDistances will sometimes use a different implementation.

            for (int res = 0; res < 2; res++) {
                iterateAllIndexesAtRes(res, kRing_equals_kRingInternal_assertions);
            }
        }

        [Test]
        public void h3NeighborRotations_identity()
        {
            // This is undefined behavior, but it's helpful for it to make sense.
            H3Index origin = 0x811d7ffffffffffL;
            int rotations = 0;
            Assert.True(
                     Algos.h3NeighborRotations(origin, Direction.CENTER_DIGIT, ref rotations) == origin,
                     "Moving to self goes to self");
        }

        [Test]
        public void cwOffsetPent()
        {
            // Try to find a case where h3NeighborRotations would not pass the
            // cwOffsetPent check, and would hit a line marked as unreachable.

            // To do this, we need to find a case that would move from one
            // non-pentagon base cell into the deleted k-subsequence of a pentagon
            // base cell, and neither of the cwOffsetPent values are the original
            // base cell's face.

            for (int pentagon = 0; pentagon < Constants.NUM_BASE_CELLS; pentagon++)
            {
                if (!BaseCells._isBaseCellPentagon(pentagon))
                {
                    continue;
                }

                for (int neighbor = 0; neighbor < Constants.NUM_BASE_CELLS; neighbor++) {
                    FaceIJK homeFaceIjk = new FaceIJK();
                    BaseCells._baseCellToFaceIjk(neighbor, ref homeFaceIjk);
                    int neighborFace = homeFaceIjk.face;

                    // Only direction 2 needs to be checked, because that is the
                    // only direction where we can move from digit 2 to digit 1, and
                    // into the deleted k subsequence.
                    Assert.True(
                             BaseCells._getBaseCellNeighbor(neighbor, Direction.J_AXES_DIGIT) != pentagon ||
                             BaseCells._baseCellIsCwOffset(pentagon, neighborFace),
                             "cwOffsetPent is reachable");
                }
            }
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
