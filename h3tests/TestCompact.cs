using System.Collections.Generic;
using System.Linq;
using H3Net.Code;
using NUnit.Framework;

namespace h3tests
{
    [TestFixture]
    public class TestCompact
    {

        internal static H3Index sunnyvale = 0x89283470c27ffff;

        internal static H3Index[] uncompactableHexes = 
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
            List<H3Index> sunnyvaleExpanded =
                new ulong[hexCount].Select(cell => new H3Index(cell)).ToList();
                
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
            int err2 = H3Index.uncompact(ref compressed, count, ref decompressed, hexCount, 9);
            Assert.True(err2 == 0);

            int count2 = 0;
            for (int i = 0; i < hexCount; i++) {
                if (decompressed[i] != 0) {
                    count2++;
                }
            }
            Assert.True(count2 == hexCount);
        }

        [Test]
        public void res0()
        {
            int hexCount = Constants.NUM_BASE_CELLS;

            List<H3Index> res0hexes = new List<H3Index>();
            for (int i = 0; i < hexCount; i++)
            {
                H3Index hex = new H3Index(0);
                H3Index.setH3Index(ref hex, 0, i, 0);
                res0hexes.Add(hex);
            }

            List<H3Index> compressed = new List<H3Index>();
            int err = H3Index.compact(ref res0hexes, ref compressed, hexCount);
            Assert.True(err == 0);

            for (int i = 0; i < hexCount; i++)
            {
                // At resolution 0, it will be an exact copy.
                // The test is further optimizing that it will be in order (which
                // isn't guaranteed.)
                Assert.True(compressed[i] == res0hexes[i]);
            }

            List<H3Index> decompressed = new List<H3Index>();
            int err2 = H3Index.uncompact(ref compressed, hexCount, ref decompressed, hexCount, 0);
            Assert.True(err2 == 0);

            int count2 = 0;
            for (int i = 0; i < hexCount; i++)
            {
                if (decompressed[i] != 0)
                {
                    count2++;
                }
            }
            Assert.True(count2 == hexCount);
        }

        [Test]
        public void uncompactable()
        {
            int hexCount = 3;
            int expectedCompactCount = 3;

            List<H3Index> uncompactableData = uncompactableHexes.ToList();
            List<H3Index> compressed = new List<H3Index>();
		
        
            int err = H3Index.compact(ref uncompactableData, ref compressed, hexCount);
            Assert.True(err == 0);

            int count = 0;
            for (int i = 0; i < hexCount; i++) {
                if (compressed[i] != 0) {
                    count++;
                }
            }
            Assert.True(count == expectedCompactCount);

            List<H3Index> decompressed = new List<H3Index>();

            int err2 = H3Index.uncompact(ref compressed, count, ref decompressed, hexCount, 9);
            Assert.True(err2 == 0);

            int count2 = 0;
            for (int i = 0; i < hexCount; i++)
            {
                if (decompressed[i] != 0)
                {
                    count2++;
                }
            }
            Assert.True(count2 == hexCount);
        }

        [Test]
        public void compact_duplicates()
        {
            int numHex = 3;
            List<H3Index> someHexagons = new List<H3Index> {0, 0, 0, 0, 0, 0, 0, 0, 0, 0}.ToList();

            for (int i = 0; i < numHex; i++)
            {
                H3Index hp = someHexagons[i];
                H3Index.setH3Index(ref hp, 5, 0, (Direction) 2);
                someHexagons[i] = hp;
            }

            List<H3Index> compressed = new List<H3Index>();
            int err = H3Index.compact(ref someHexagons, ref compressed, numHex);
            Assert.True(err != 0);


        }

        [Test]
        public void uncompact_wrongRes()
        {
            int numHex = 3;
            List<H3Index> someHexagons = new List<H3Index> {0, 0, 0}.ToList();

            for (int i = 0; i < numHex; i++)
            {
                H3Index hp = someHexagons[i];
                H3Index.setH3Index(ref hp, 5, i, 0);
                someHexagons[i] = hp;
            }

            int sizeResult = H3Index.maxUncompactSize(ref someHexagons, numHex, 4);
            // maxUncompactSize fails when given illogical resolutions
            Assert.True(sizeResult < 0);
            sizeResult = H3Index.maxUncompactSize(ref someHexagons, numHex, -1);
            // maxUncompactSize fails when given illegal resolutions
            Assert.True(sizeResult < 0);


            List<H3Index> uncompressed = new List<H3Index> {0, 0, 0}.ToList();
            int uncompactResult =H3Index.uncompact(ref someHexagons, numHex, ref uncompressed, numHex, 0);
            // uncompact fails when given illogical resolutions
            Assert.True(uncompactResult != 0);

            //  Since I'm using List<>, these aren't relevant.  If our values are reaching the point of
            //  causing an issue with List<> unable to allocate memory, we've got other problems.

            //uncompactResult = H3Index.uncompact(ref someHexagons, numHex, ref uncompressed, numHex, 6);
            // uncompact fails when given too little buffer
            //Assert.True(uncompactResult != 0);
            //uncompactResult = H3Index.uncompact(ref someHexagons, numHex, ref uncompressed, numHex - 1, 5);
            // uncompact fails when given too little buffer (same resolution)
            //Assert.True(uncompactResult != 0);
        }

        [Test]
        public void hexagon()
        {
            H3Index origin = new H3Index(0);
            
            H3Index.setH3Index(ref origin, 1, 5, 0);

            List<H3Index> origin_list = new List<H3Index>{origin};
            int childrenSz = H3Index.maxUncompactSize(ref origin_list, 1, 2);
            List<H3Index> children = new List<H3Index>(childrenSz);

            int uncompactResult = H3Index.uncompact(ref origin_list, 1, ref children, childrenSz, 2);
            // uncompact origin succeeds
            Assert.True(uncompactResult == 0);

            List<H3Index> result = new List<H3Index>();
            int compactResult = H3Index.compact(ref children, ref result, childrenSz);
            // compact origin succeeds"
            Assert.True(compactResult == 0);

            int found = 0;
            for (int i = 0; i < result.Count; i++)
            {
                if (result[i] != 0)
                {
                    found++;
                    // compacted to correct origin
                    Assert.True((ulong)result[i] == (ulong)origin);
                }
            }
            //compacted to a single hexagon
            Assert.True(found == 1);
        }

        [Test]
        public void pentagon()
        {
            H3Index pentagon = new H3Index(0);
            H3Index.setH3Index(ref pentagon, 1, 4, 0);

            List<H3Index> penta = new List<H3Index>{pentagon};
            int childrenSz = H3Index.maxUncompactSize(ref penta, 1, 2);
            List<H3Index> children = new List<H3Index>();
            int uncompactResult =H3Index.uncompact(ref penta, 1, ref children, childrenSz, 2);
            Assert.True(uncompactResult == 0, "uncompact pentagon succeeds");

            List<H3Index> result = new List<H3Index>();
            int compactResult = H3Index.compact(ref children, ref result, childrenSz);
            Assert.True(compactResult == 0, "compact pentagon succeeds");

            int found = 0;
            for (int i = 0; i < result.Count; i++)
            {
                if (result[i] != 0)
                {
                    found++;
                    // compacted to correct pentagon
                    Assert.True((ulong)result[i] == (ulong)pentagon);
                }
            }
            // compacted to a single pentagon
            Assert.True(found == 1);
        }
    }
}
