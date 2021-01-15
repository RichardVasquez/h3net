using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using H3Lib;
using H3Lib.Extensions;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class TestKRing
    {
        private static void kRing_equals_kRingInternal_assertions(H3Index h3)
        {
            for (int k = 0; k < 3; k++)
            {
                var kSize = k.MaxKringSize();
                var distanceDictionary = h3.KRingDistances(k);
                var internalDictionary = h3.KRingInternal(k);

                var distanceKeys = distanceDictionary.Keys.ToList();
                var internalKeys = internalDictionary.Keys.ToList();
                
                distanceKeys.Sort();
                internalKeys.Sort();
                
                Assert.AreEqual(distanceKeys.Count, internalKeys,
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
    }
}
