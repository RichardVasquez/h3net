using System.Collections.Generic;
using System.Linq;
using H3Lib;
using H3Lib.Extensions;
using NUnit.Framework;

namespace TestSuite
{
    [TestFixture]
    public class TestHexRing
    {
        private static GeoCoord sf = new GeoCoord(0.659966917655m, 2 * 3.14159m - 2.1364398519396m);
        private static H3Index sfHex = sf.ToH3Index(9);

        [Test]
        public void IdentityKRing()
        {
            var status = sfHex.HexRing(0);
            Assert.AreEqual(0, status.Item1);
            Assert.AreEqual(sfHex, status.Item2[0]);
        }

        [Test]
        public void Ring1()
        {
            var expectedK1 = new List<H3Index>
                             {
                                 0x89283080ddbffff, 0x89283080c37ffff,
                                 0x89283080c27ffff, 0x89283080d53ffff,
                                 0x89283080dcfffff, 0x89283080dc3ffff
                             };

            (var err, var k1) = sfHex.HexRing(1);
            Assert.AreEqual(0, err);

            foreach (var index in expectedK1)
            {
                Assert.IsTrue(k1.Contains(index));
            }
        }

        [Test]
        public void Ring2()
        {
            var expectedK2 =
                new List<H3Index>
                {
                    0x89283080ca7ffff, 0x89283080cafffff, 0x89283080c33ffff,
                    0x89283080c23ffff, 0x89283080c2fffff, 0x89283080d5bffff,
                    0x89283080d43ffff, 0x89283080d57ffff, 0x89283080d1bffff,
                    0x89283080dc7ffff, 0x89283080dd7ffff, 0x89283080dd3ffff
                };

            (var err, var k2) = sfHex.HexRing(2);
            Assert.AreEqual(0, err);

            foreach (var index in expectedK2)
            {
                Assert.IsTrue(k2.Contains(index));
            }
        }

        [Test]
        public void NearPentagonRing1()
        {
            H3Index nearPentagon = 0x837405fffffffff;
            (int err, _) = nearPentagon.HexRing(1);
            Assert.AreNotEqual(0, err);
        }

        [Test]
        public void NearPentagonRing2()
        {
            H3Index nearPentagon = 0x837405fffffffff;
            (int err, _) = nearPentagon.HexRing(2);
            Assert.AreNotEqual(0, err);
        }

        [Test]
        public void OnPentagon()
        {
            var nearPentagon = new H3Index(0, 4, 0);
            (int err, _) = nearPentagon.HexRing(2);
            Assert.AreNotEqual(0, err);
        }

        [Test]
        public void hexRing_matches_kRingInternal()
        {
            for (int res = 0; res < 2; res++)
            {
                for (int i = 0; i < Constants.H3.BaseCellsCount; i++)
                {
                    H3Index bc = new H3Index(0, i, 0);
                    (int stat, List<H3Index> children) = bc.Uncompact(res);

                    for (int j = 0; j < children.Count; j++)
                    {
                        if (children[j] == 0)
                        {
                            continue;
                        }

                        for (int k = 0; k < 3; k++)
                        {
                            int ringSz = k != 0
                                             ? 6 * k
                                             : 1;
                            (var failed, var ring) = children[j].HexRing(k); 
                            
                            if (failed == 0)
                            {
                                var lookup = children[j].KRingInternal(k);
                                var internalNeighbors = lookup.Keys.ToList();
                                var internalDistances = lookup.Values.ToList();

                                int found = 0;
                                int internalFound = 0;
                                for (int iRing = 0; iRing < ringSz; iRing++)
                                {
                                    if (ring[iRing] != 0)
                                    {
                                        found++;

                                        for (int iInternal = 0;
                                             iInternal < lookup.Count;
                                             iInternal++)
                                        {
                                            if (internalNeighbors[iInternal] ==
                                                ring[iRing])
                                            {
                                                internalFound++;

                                                Assert.AreEqual(internalDistances[iInternal], k);

                                                break;
                                            }
                                        }

                                        Assert.AreEqual(found,internalFound);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
