using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using h3net.API;
using NUnit.Framework;

namespace h3tests
{
    [TestFixture]
    class TestHexRing
    {
        private static GeoCoord sf = new GeoCoord(0.659966917655, 2 * 3.14159 - 2.1364398519396);
        private static H3Index sfHex = H3Index.geoToH3(ref sf, 9);

        [Test]
        public void identityKRing()
        {
            List<H3Index> k0 = new List<H3Index> {0};
            var err = Algos.hexRing(sfHex, 0, ref k0);

            Assert.True(err == 0, "No error on hexRing");
            Assert.True(k0[0] == sfHex, "generated identity k-ring");
        }

        [Test]
        public void ring1()
        {
            int err;

            List<H3Index> k1 = new List<H3Index> {0, 0, 0, 0, 0, 0};
            List<H3Index> expectedK1 = new List<H3Index>
                                       {
                                           0x89283080ddbffff, 0x89283080c37ffff,
                                           0x89283080c27ffff, 0x89283080d53ffff,
                                           0x89283080dcfffff, 0x89283080dc3ffff
                                       };
            err = Algos.hexRing(sfHex, 1, ref k1);

            Assert.True(err == 0, "No error on hexRing");

            for (int i = 0; i < 6; i++)
            {
                Assert.True(k1[i] != 0, "index is populated");
                int inList = 0;
                for (int j = 0; j < 6; j++)
                {
                    if (k1[i] == expectedK1[j])
                    {
                        inList++;
                    }
                }

                Assert.True(inList == 1, "index found in expected set");
            }
        }

        [Test]
        public void ring2()
        {
            int err;

            List<H3Index> k2 = new List<H3Index> {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};

            List<H3Index> expectedK2 =
                new List<H3Index>
                {
                    0x89283080ca7ffff, 0x89283080cafffff, 0x89283080c33ffff,
                    0x89283080c23ffff, 0x89283080c2fffff, 0x89283080d5bffff,
                    0x89283080d43ffff, 0x89283080d57ffff, 0x89283080d1bffff,
                    0x89283080dc7ffff, 0x89283080dd7ffff, 0x89283080dd3ffff
                };
            err = Algos.hexRing(sfHex, 2, ref k2);

            Assert.True(err == 0, "No error on hexRing");

            for (int i = 0; i < 12; i++)
            {
                Assert.True(k2[i] != 0, "index is populated");
                int inList = 0;
                for (int j = 0; j < 12; j++)
                {
                    if (k2[i] == expectedK2[j])
                    {
                        inList++;
                    }
                }

                Assert.True(inList == 1, "index found in expected set");
            }
        }

        [Test]
        public void nearPentagonRing1()
        {
            int err;

            H3Index nearPentagon = 0x837405fffffffff;
            List<H3Index> kp1 = new List<H3Index> {0, 0, 0, 0, 0, 0};
            err = Algos.hexRing(nearPentagon, 1, ref kp1);

            Assert.True(err != 0, "Should return an error when hitting a pentagon");
        }

        [Test]
        public void nearPentagonRing2()
        {
            int err;

            H3Index nearPentagon = 0x837405fffffffff;
            List<H3Index> kp2 = new List<H3Index> {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};
            err = Algos.hexRing(nearPentagon, 2, ref kp2);

            Assert.True(err != 0, "Should return an error when hitting a pentagon");
        }

        [Test]
        public void onPentagon()
        {
            int err;

            H3Index nearPentagon = 0;
            H3Index.setH3Index(ref nearPentagon, 0, 4, 0);
            List<H3Index> kp2 = new List<H3Index> {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};
            err = Algos.hexRing(nearPentagon, 2, ref kp2);

            Assert.True(err != 0, "Should return an error when starting at a pentagon");
        }

        [Test]
        public void hexRing_matches_kRingInternal()
        {
            for (int res = 0; res < 2; res++)
            {
                for (int i = 0; i < Constants.NUM_BASE_CELLS; i++)
                {
                    H3Index bc = 0;
                    H3Index.setH3Index(ref bc, 0, i, 0);
                    List<H3Index> bc_list = new List<H3Index>{bc};
                    int childrenSz = H3Index.maxUncompactSize(ref bc_list, 1, res);
                    List<H3Index> children =new ulong[childrenSz].Select(cell => new H3Index(cell)).ToList();
                    H3Index.uncompact(ref bc_list, 1, ref children, childrenSz, res);

                    for (int j = 0; j < childrenSz; j++)
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
                            int kSz = Algos.maxKringSize(k);

                            
                            List<H3Index> ring =new ulong[childrenSz].Select(cell => new H3Index(cell)).ToList();
                            int failed = Algos.hexRing(children[j], k, ref ring);

                            if (failed != 0)
                            {
                                List<H3Index> internalNeighbors =new ulong[kSz].Select(cell => new H3Index(cell)).ToList();
                                List<int> internalDistances = new int[kSz].Select(id => 0).ToList();
                                Algos._kRingInternal
                                    (
                                     children[j], k, ref internalNeighbors,
                                     ref internalDistances, kSz, 0
                                    );

                                int found = 0;
                                int internalFound = 0;
                                for (int iRing = 0; iRing < ringSz; iRing++)
                                {
                                    if (ring[iRing] != 0)
                                    {
                                        found++;

                                        for (int iInternal = 0;
                                             iInternal < kSz;
                                             iInternal++)
                                        {
                                            if (internalNeighbors[iInternal] ==
                                                ring[iRing])
                                            {
                                                internalFound++;

                                                Assert.True
                                                    (
                                                     internalDistances[iInternal] == k,
                                                     "Ring and internal agree on distance");

                                                break;
                                            }
                                        }
                                        Assert.True
                                            (found == internalFound,
                                             "Ring and internal implementations produce same output");
                                    }
                                }
                                internalNeighbors.Clear();
                                internalDistances.Clear();
                            }

                            ring.Clear();
                        }
                    }
                    children.Clear();
                }
            }


        }
    }
}
