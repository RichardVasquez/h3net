using System.Collections.Generic;
using System.Linq;
using H3Lib;
using H3Lib.Extensions;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class TestKRing
    {
        private static void KRingEqualsKRingInternalAssertions(H3Index h3)
        {
            for (int k = 0; k < 3; k++)
            {
                var distanceDictionary = h3.KRingDistances(k);
                var internalDictionary = h3.KRingInternal(k);

                var distanceKeys = distanceDictionary.Keys.ToList();
                var internalKeys = internalDictionary.Keys.ToList();
                
                distanceKeys.Sort();
                internalKeys.Sort();
                
                Assert.AreEqual(distanceKeys.Count, internalKeys.Count,
                                "Resulting dictionaries don't have matching size");
                for (int i = 0; i < distanceKeys.Count; i++)
                {
                    Assert.AreEqual(distanceKeys[i], internalKeys[i], "Keys (h3index value) don't match");
                    Assert.AreEqual(distanceDictionary[distanceKeys[i]], internalDictionary[internalKeys[i]],
                                    "Distance values don't match");
                }
            }
        }

        [Test]
        public void KRing0()
        {
            var sf = new GeoCoord(0.659966917655, 2 * 3.14159 - 2.1364398519396);
            var sfHex0 = sf.ToH3Index(0);

            var expectedK1 = new List<H3Index>
                             {
                                 0x8029fffffffffff, 0x801dfffffffffff,
                                 0x8013fffffffffff, 0x8027fffffffffff,
                                 0x8049fffffffffff, 0x8051fffffffffff,
                                 0x8037fffffffffff
                             };

            var k1Dist = sfHex0.KRingDistances(1);

            Assert.AreEqual(7, k1Dist.Count);

            foreach (var pair in k1Dist)
            {
                if (pair.Key == sfHex0)
                {
                    Assert.AreEqual(0, pair.Value);
                }
                else
                {
                    Assert.AreEqual(1, pair.Value);
                    Assert.IsTrue(expectedK1.Contains(pair.Key));
                }
            }
        }

        [Test]
        public void KRingPolarPentagon()
        {
            H3Index polar = new H3Index().SetIndex(0, 4, Direction.CENTER_DIGIT);

            H3Index[] expectedK2 =
            {
                0x8009fffffffffff,
                0x8007fffffffffff,
                0x8001fffffffffff,
                0x8011fffffffffff,
                0x801ffffffffffff,
                0x8019fffffffffff
            };

            // dictionary<h3, int>
            var k2PlusDistance = polar.KRingDistances(1);

            foreach ((var key, int value) in k2PlusDistance)
            {
                Assert.IsTrue(expectedK2.Contains(key));
                int checkValue = key == polar
                                      ? 0
                                      : 1;
                Assert.AreEqual(checkValue, value);
            }

            Assert.AreEqual(6, k2PlusDistance.Count);
        }

        [Test]
        public void KRing1PolarPentagon()
        {
            H3Index polar = new H3Index().SetIndex(1, 4, 0);

            H3Index[] expectedK2 =
            {
                0x81083ffffffffff,
                0x81093ffffffffff,
                0x81097ffffffffff,
                0x8108fffffffffff,
                0x8108bffffffffff,
                0x8109bffffffffff
            };

            var k2PlusDist = polar.KRingDistances(1);
            foreach ((var key, int value) in k2PlusDist)
            {
                Assert.IsTrue(expectedK2.Contains(key));
                int checkValue = key == polar
                                     ? 0
                                     : 1;
                Assert.AreEqual(checkValue, value);
            }

            Assert.AreEqual(6, k2PlusDist.Count);
        }

        [Test]
        public void KRing1PolarPentagonK3()
        {
            var polar = new H3Index().SetIndex(1, 4, 0);

            var expectedK2 = new List<H3Index>
                 {
                     0x81013ffffffffff, 0x811fbffffffffff, 0x81193ffffffffff, 0x81097ffffffffff, 0x81003ffffffffff,
                     0x81183ffffffffff, 0x8111bffffffffff, 0x81077ffffffffff, 0x811f7ffffffffff, 0x81067ffffffffff,
                     0x81093ffffffffff, 0x811e7ffffffffff, 0x81083ffffffffff, 0x81117ffffffffff, 0x8101bffffffffff,
                     0x81107ffffffffff, 0x81073ffffffffff, 0x811f3ffffffffff, 0x81063ffffffffff, 0x8108fffffffffff,
                     0x811e3ffffffffff, 0x8119bffffffffff, 0x81113ffffffffff, 0x81017ffffffffff, 0x81103ffffffffff,
                     0x8109bffffffffff, 0x81197ffffffffff, 0x81007ffffffffff, 0x8108bffffffffff, 0x81187ffffffffff,
                     0x8107bffffffffff
                 };
            
            var expectedK2Dist = new List<int>
                                 {
                                     2, 3, 2, 1, 3, 3, 3, 2, 2, 3, 1, 3, 0, 2, 3, 3, 2, 2, 3, 1,
                                     3, 3, 2, 2, 3, 1, 2, 3, 1, 3, 3
                                 };

            var polarDictionary = polar.KRingDistances(3);

            foreach (var pair in polarDictionary)
            {
                Assert.IsTrue(expectedK2.Contains(pair.Key));
                var index = expectedK2.FindIndex(v => v == pair.Key);
                Assert.AreEqual(expectedK2Dist[index], pair.Value);
            }
            
            Assert.AreEqual(31,polarDictionary.Count);
        }

        [Test]
        public void KRing1PentagonK4()
        {
            H3Index pent = new H3Index(1, 14, 0);

            var expectedK2 =
                new List<H3Index>
                {
                    0x811d7ffffffffff, 0x810c7ffffffffff, 0x81227ffffffffff, 0x81293ffffffffff, 0x81133ffffffffff,
                    0x8136bffffffffff, 0x81167ffffffffff, 0x811d3ffffffffff, 0x810c3ffffffffff, 0x81223ffffffffff,
                    0x81477ffffffffff, 0x8128fffffffffff, 0x81367ffffffffff, 0x8112fffffffffff, 0x811cfffffffffff,
                    0x8123bffffffffff, 0x810dbffffffffff, 0x8112bffffffffff, 0x81473ffffffffff, 0x8128bffffffffff,
                    0x81363ffffffffff, 0x811cbffffffffff, 0x81237ffffffffff, 0x810d7ffffffffff, 0x81127ffffffffff,
                    0x8137bffffffffff, 0x81287ffffffffff, 0x8126bffffffffff, 0x81177ffffffffff, 0x810d3ffffffffff,
                    0x81233ffffffffff, 0x8150fffffffffff, 0x81123ffffffffff, 0x81377ffffffffff, 0x81283ffffffffff,
                    0x8102fffffffffff, 0x811c3ffffffffff, 0x810cfffffffffff, 0x8122fffffffffff, 0x8113bffffffffff,
                    0x81373ffffffffff, 0x8129bffffffffff, 0x8102bffffffffff, 0x811dbffffffffff, 0x810cbffffffffff,
                    0x8122bffffffffff, 0x81297ffffffffff, 0x81507ffffffffff, 0x8136fffffffffff, 0x8127bffffffffff,
                    0x81137ffffffffff
                };

            var polarDictionary = pent.KRingDistances(4);

            foreach (var pair in polarDictionary)
            {
                Assert.IsTrue(expectedK2.Contains(pair.Key));
            }

            Assert.AreEqual(expectedK2.Count, polarDictionary.Count);
        }

        [Test]
        public void KRingEqualsKRingInternal()
        {
            // Check that kRingDistances output matches _kRingInternal,
            // since kRingDistances will sometimes use a different implementation.

            for (var res = 0; res < 2; res++)
            {
                Utility.IterateAllIndexesAtRes(2,KRingEqualsKRingInternalAssertions);
            }
        }

        [Test]
        public void H3NeighborRotationsIdentity()
        {
            // This is undefined behavior, but it's helpful for it to make sense.
            H3Index origin = 0x811d7ffffffffffL;
            int rotations = 0;

            var (result, _) = origin.NeighborRotations(Direction.CENTER_DIGIT, rotations);
            Assert.AreEqual(result, origin);
        }

        [Test]
        public void ClockwiseOffsetPent()
        {
            // Try to find a case where h3NeighborRotations would not pass the
            // cwOffsetPent check, and would hit a line marked as unreachable.

            // To do this, we need to find a case that would move from one
            // non-pentagon base cell into the deleted k-subsequence of a pentagon
            // base cell, and neither of the cwOffsetPent values are the original
            // base cell's face.

            for (int pentagon = 0; pentagon < Constants.NUM_BASE_CELLS; pentagon++)
            {
                if (!pentagon.IsBaseCellPentagon())
                {
                    continue;
                }

                for (int neighbor = 0; neighbor < Constants.NUM_BASE_CELLS; neighbor++)
                {
                    var homeFaceIjk = neighbor.ToFaceIjk();
                    int neighborFace = homeFaceIjk.Face;

                    // Only direction 2 needs to be checked, because that is the
                    // only direction where we can move from digit 2 to digit 1, and
                    // into the deleted k subsequence.
                    Assert.IsTrue(
                                  neighbor.GetNeighbor(Direction.J_AXES_DIGIT) != pentagon ||
                                  pentagon.IsClockwiseOffset(neighborFace)
                                  );
                }
            }

        }
        
    }
}
