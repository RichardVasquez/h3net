using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using H3Net.Code;
using NUnit.Framework;

namespace h3tests
{
    [TestFixture]
    public class TestHexRanges
    {
        private static GeoCoord sf = new GeoCoord(0.659966917655, 2 * 3.14159 - 2.1364398519396);
        private static H3Index sfHex = H3Index.geoToH3(ref sf, 9);
        private static List<H3Index> sfHexPtr = new List<H3Index>{sfHex.value};

        private static H3Index[] k1 =
        {
            0x89283080ddbffff, 0x89283080c37ffff, 0x89283080c27ffff,
            0x89283080d53ffff, 0x89283080dcfffff, 0x89283080dc3ffff
        };

        [Test]
        public void identityKRing()
        {
            int err;

            List<H3Index> k0 = new List<H3Index>{0};
            err = Algos.hexRanges(ref sfHexPtr, 1, 0, k0);

            Assert.True(err == 0, "No error on hexRanges");
            Assert.True(k0[0] == sfHex, "generated identity k-ring");
        }

        [Test]
        public void ring1of1()
        {
            int err;
            List<H3Index> allKrings = new List<H3Index>
                                      {
                                          0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                          0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                          0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
                                      };

            var newk1 = k1.ToList();

            err = Algos.hexRanges(ref newk1, 6, 1, allKrings);

            Assert.True(err == 0, "No error on hexRanges");

            for (int i = 0; i < 42; i++) {
                Assert.True(allKrings[i] != 0, "index is populated");
                if (i % 7 == 0) {
                    int index = i / 7;
                    Assert.True(newk1[index] == allKrings[i],
                             "The beginning of the segment is the correct hexagon");
                }
            }
        }

        [Test]
        public void ring2of1()
        {
            int err;
            var allKrings2 = new ulong[6 * (1 + 6 + 12)].Select(cell => new H3Index(cell)).ToList();

            var newk1 = k1.ToList();
            err = Algos.hexRanges(ref newk1, 6, 2, allKrings2);

            Assert.True(err == 0, "No error on hexRanges");

            for (int i = 0; i < (6 * (1 + 6 + 12)); i++) {
                Assert.True(allKrings2[i] != 0, "index is populated");

                if (i % (1 + 6 + 12) == 0) {
                    int index = i / (1 + 6 + 12);
                    Assert.True(newk1[index] == allKrings2[i],
                             "The beginning of the segment is the correct hexagon");
                }
            }
        }
    }
}
