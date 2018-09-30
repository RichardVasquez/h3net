using System;
using System.Collections.Generic;
using h3net.API;
using NUnit.Framework;

namespace h3tests
{
    [TestFixture]
    public class TestCompact
    {

        internal static H3Index sunnyvale = 0x89283470c27ffff;

        internal static H3Index[] uncompactable = 
            {
            0x89283470803ffff,
            0x8928347081bffff,
            0x8928347080bffff};

        [Test]
        public void roundtrip()
        {
            int k = 9;
            int hexCount = Algos.maxKringSize(k);
            int expectedCompactCount = 73;

            // Generate a set of hexagons to compact
            List<H3Index> sunnyvaleExpanded = new List<H3Index>(hexCount);
            Algos.kRing(sunnyvale, k, ref sunnyvaleExpanded);

            List<H3Index> compressed = new List<H3Index>(hexCount);
            int err = H3Index.compact(ref sunnyvaleExpanded, ref compressed, hexCount);
            Assert.True(err == 0);

            int count = 0;
            for (int i = 0; i < compressed.Count; i++) {
                if (compressed[i] != 0)
                {
                    count++;
                }
            }

            Assert.True(count == expectedCompactCount);
            int size = H3Index.maxUncompactSize(ref compressed, count, 9);
            List<H3Index> decompressed = new List<H3Index>(size);
            for (int kk = 0; kk < size; kk++)
            {
                decompressed.Add(0);
            }
                
            int err2 =
                H3Index.uncompact(ref compressed, count, ref decompressed, hexCount, 9);
            Assert.True(err2 == 0);

            int count2 = 0;
            for (int i = 0; i < hexCount; i++) {
                if (decompressed[i] != 0) {
                    count2++;
                }
            }
            Assert.True(count2 == hexCount);
        }
    }
}
