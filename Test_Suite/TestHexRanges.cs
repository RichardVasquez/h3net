using System.Collections.Generic;
using System.Linq;
using H3Lib;
using H3Lib.Extensions;
using NUnit.Framework;

namespace TestSuite
{
    [TestFixture]
    public class TestHexRanges
    {
        private static GeoCoord sf = new GeoCoord(0.659966917655, 2 * 3.14159 - 2.1364398519396);
        private static H3Index sfHex = sf.ToH3Index(9);

        private static List<H3Index> k1 = new List<H3Index>
                                   {
                                       0x89283080ddbffff, 0x89283080c37ffff, 0x89283080c27ffff,
                                       0x89283080d53ffff, 0x89283080dcfffff, 0x89283080dc3ffff
                                   };

        private static List<H3Index> withPentagon = new List<H3Index>
                                                    {
                                                        0x8029fffffffffff,
                                                        0x801dfffffffffff
                                                    };


        [Test]
        public void IdentityKRing()
        {
            var (status, result) = sfHex.HexRange(0);
            
            Assert.AreEqual(0, status);
            Assert.AreEqual(sfHex, result[0]);
        }

        [Test]
        public void Ring1of1()
        {
            (int status, var result) = k1.HexRanges(1);

            Assert.AreEqual(0, status);
            Assert.IsTrue(result.All(r => r != 0));

            foreach (var index in k1)
            {
                Assert.IsTrue(result.Contains(index));
            }
        }

        [Test]
        public void Ring2of1()
        {
            (int status, var result) = k1.HexRanges(2);

            Assert.AreEqual(0, status);
            Assert.IsTrue(result.All(r => r != 0));

            foreach (var index in k1)
            {
                Assert.IsTrue(result.Contains(index));
            }
        }

        [Test]
        public void Failed()
        {
            (int status, var result) = withPentagon.HexRanges(1);
            
            Assert.AreNotEqual(0, status);
        }
    }
}
