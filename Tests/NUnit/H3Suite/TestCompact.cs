using System.Collections.Generic;
using System.Linq;
using H3Lib;
using H3Lib.Extensions;
using NUnit.Framework;

//
//#include "constants.h"
//#include "h3Index.h"
//#include "test.h"
//
namespace TestSuite
{
    [TestFixture]
    public class TestCompact
    {
        public readonly  H3Index Sunnyvale = 0x89283470c27ffff;

        public readonly H3Index[] uncompactable =
        {
            0x89283470803ffff,
            0x8928347081bffff,
            0x8928347080bffff
        };

        public readonly H3Index[] uncompactableWithZero =
        {
            0x89283470803ffff,
            0x8928347081bffff,
            0,
            0x8928347080bffff
        };

        [Test]
        public void RoundTrip()
        {
            var k = 9;
            int hexCount = k.MaxKringSize();
            var expectedCompactCount = 73;

            // Generate a set of hexagons to compact
            var sunnyvaleExpanded = Sunnyvale.KRing(k);

            var (status1, compressed) = sunnyvaleExpanded.Compact();

            Assert.AreEqual(0, status1);
            int count1 = compressed.Select(h => h != 0).Count();
            Assert.AreEqual(expectedCompactCount,count1);

            var (status2, uncompacted) = compressed.Uncompact(k);
            Assert.AreEqual(0, status2);
            int count2 = uncompacted.Select(h => h != 0).Count();
            Assert.AreEqual(count2, hexCount);
        }

        [Test]
        public void Res0()
        {
            const int hexCount = Constants.H3.NUM_BASE_CELLS;

            var res0Hexes = Enumerable.Range(0, hexCount).Select
                (s => new H3Index().SetIndex(0, s, Direction.CENTER_DIGIT)).ToList();

            (int status, var compressed) = res0Hexes.Compact();

            Assert.AreEqual(H3Lib.Constants.H3Index.COMPACT_SUCCESS, status);
            for (var i = 0; i < compressed.Count; i++)
            {
                Assert.AreEqual(compressed[i], res0Hexes[i]);
            }

            (int status2, var decompressed) = compressed.Uncompact(0);
            Assert.AreEqual(0, status2);
            Assert.AreEqual(hexCount, decompressed.Select(s => s.Resolution != 0).Count());
        }

        [Test]
        public void Uncompactable()
        {
            var hexCount = 3;
            var expectedCompactCount = 3;

            (int status1, var compressed) = uncompactable.ToList().Compact();

            Assert.AreEqual(0, status1);
            var count1 = compressed.Count(s => s.Resolution != 0);
            Assert.AreEqual(expectedCompactCount, count1);

            (int status2, var decompressed) = compressed.Uncompact(9);
            Assert.AreEqual(0, status2);
            var count2 = decompressed.Count(s => s.Resolution != 0);
            Assert.AreEqual(hexCount, count2);
        }

        [Test]
        public void CompactDuplicate()
        {
            const int numHex = 10;
            var someHexagons = new List<H3Index>();

            for (var i = 0; i < numHex; i++)
            {
                someHexagons.Add(new H3Index().SetIndex(5, 0, Direction.J_AXES_DIGIT));
            }

            (int status, _) = someHexagons.Compact();
            
            Assert.AreNotEqual(0, status);
        }

        [Test]
        public void CompactDuplicateMinimum()
        {
            // Test that the minimum number of duplicate hexagons causes failure
            const int res = 10;
            // Arbitrary index
            var h3 = new H3Index().SetIndex(res, 0, Direction.J_AXES_DIGIT);

            var children = h3.ToChildren(res + 1);
            
            children[^1] = children.First();
            (int result, _) = children.Compact();

            Assert.AreEqual(H3Lib.Constants.H3Index.COMPACT_DUPLICATE, result);
        }

        [Test]
        public void CompactDuplicatePentagonLimit()
        {
            // Test that the minimum number of duplicate hexagons causes failure
            const int res = 10;
            // Arbitrary pentagon parent cell
            var h3 = new H3Index().SetIndex(res, 4, Direction.CENTER_DIGIT);

            var children = h3.ToChildren(res + 1);

            // duplicate one index
            children[^1] = h3.ToCenterChild(res + 1);
            (var result, var compact) = children.Compact();

            Assert.AreEqual(H3Lib.Constants.H3Index.COMPACT_DUPLICATE, result);
        }

        [Test]
        public void CompactDuplicateIgnored()
        {
            // Test that duplicated cells are not rejected by compact.
            // This is not necessarily desired - just asserting the
            // existing behavior.
            int res = 10;
            // Arbitrary index
            H3Index h3=new H3Index().SetIndex(res, 0, Direction.J_AXES_DIGIT);

            var children = h3.ToChildren(res + 1);

            // duplicate one index
            children[^1] = children[0];

            //  FlexiCompact takes duplicates. Compact() is more rigid
            var (status, _) = children.FlexiCompact();
            Assert.AreEqual(Constants.H3Index.COMPACT_SUCCESS, status);
        }

        [Test]
        public void CompactEmpty()
        {
            //  Not quite the same, but close enough.
            var temp = new List<H3Index>();
            var (status, _) = temp.Compact();
            Assert.AreEqual(0,status);
        }

        [Test]
        public void CompactDisparate()
        {
            // Exercises a case where compaction needs to be tested but none is
            // possible
            const int numHex = 7;
            var disparate = new List<H3Index>();
            for (int i = 0; i<numHex;i++)
            {
                disparate.Add(new H3Index().SetIndex(1, i, Direction.CENTER_DIGIT));
            }

            var (status, attempt) = disparate.Compact();
            Assert.AreEqual(0, status);

            //  Ensuring that the in and out match, but not going to ensure the ordering is the same.
            disparate.Sort();
            attempt.Sort();
            Assert.AreEqual(disparate,attempt);
        }

        [Test]
        public void UncompactWrongRes()
        {
            const int numHex = 3;
            var someHexagons = new List<H3Index>();
            for (var i = 0; i < numHex; i++)
            {
                someHexagons.Add(new H3Index().SetIndex(5, i, Direction.CENTER_DIGIT));
            }

            long sizeResult = someHexagons.MaxUncompactSize(4);
            Assert.IsTrue(sizeResult<0);

            sizeResult = someHexagons.MaxUncompactSize(-1);
            Assert.IsTrue(sizeResult < 0);

            sizeResult = someHexagons.MaxUncompactSize(Constants.H3.MAX_H3_RES + 1);
            Assert.IsTrue(sizeResult < 0);

            (int status, _) = someHexagons.Uncompact(0);
            Assert.AreNotEqual(0, status);

            //  Skipping buffer size checks            

            someHexagons.Clear();
            for(var i =0; i < numHex;i++)
            {
                someHexagons.Add(new H3Index().SetIndex(Constants.H3.MAX_H3_RES, i, 0));
            }

            (status, _) = someHexagons.Uncompact(Constants.H3.MAX_H3_RES + 1);
            Assert.AreNotEqual(0,status);
        }

        [Test]
        public void SomeHexagon()
        {
            H3Index origin = new H3Index().SetIndex(1, 5, Direction.CENTER_DIGIT);

            var children = origin.ToChildren(2);
            Assert.AreNotEqual(0, children.Count);
            
            (var result2, var compacted) = children.Compact();
            Assert.AreEqual(0,result2);
            
            Assert.AreEqual(1,compacted.Count);
            Assert.AreEqual(compacted[0], origin);
        }
        
        //  Since I'm hanging as much of the H3 API on static extensions at this point,
        //  it's not really possible to test uncompact with a null value, so no
        //  UncompactEmpty analog

        [Test]
        public void UncompactOnlyZero()
        {
            // maxUncompactSize and uncompact both permit 0 indexes
            // in the input array, and skip them. When only a zero is
            // given, it's a no-op.

            var origin = new List<H3Index> {new H3Index(0)};

            (int status, _) = origin.Uncompact(2);
            Assert.AreEqual(0, status);
        }

        [Test]
        public void UncompactWithZero()
        {
            // maxUncompactSize and uncompact both permit 0 indexes
            // in the input array, and skip them.

            var childrenSz = uncompactableWithZero.ToList().MaxUncompactSize(10);

            var (status, result) = uncompactable.ToList().Uncompact(10);
            Assert.AreEqual(0, status);

            Assert.AreEqual(childrenSz,result.Count);
        }

        [Test]
        public void Pentagon()
        {
            var pentagon = new H3Index().SetIndex(1, 4, Direction.CENTER_DIGIT);

            (int status1, var result1) = pentagon.Uncompact(2);
            Assert.AreEqual(0, status1);

            (int status2, List<H3Index> result2) = result1.Compact();
            Assert.AreEqual(0, status2);

            Assert.AreEqual(1, result2.Count);
            Assert.AreEqual(result2[0], pentagon);
        }
    }
}
