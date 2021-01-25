using System;
using System.Collections.Generic;
using System.Linq;
using H3Lib;
using H3Lib.Extensions;
using Microsoft.VisualBasic;
using NUnit.Framework;
using TestSuite.Lib;

namespace TestSuite
{
    [TestFixture]
    public class TestH3LineExhaustive
    {
        private static readonly int[] MAX_DISTANCES = {1, 2, 5, 12, 19, 26};

        /**
         * Property-based testing of h3Line output
         */
        private static void h3Line_assertions(H3Index start, H3Index end)
        {
            (int err, IEnumerable<H3Index> line) = start.LineTo(end);

            var check = line.ToList();
            Assert.AreEqual(0, err);
//            Console.WriteLine($"start: {start} end: {end}");
            Assert.AreEqual(start, check.First());
            Assert.AreEqual(end, check.Last());

            for (int i = 1; i < check.Count; i++)
            {
                Assert.IsTrue(check[i].IsValid());
                Assert.IsTrue(check[i].IsNeighborTo(check[i - 1]));
                if (i > 1)
                {
                    Assert.IsFalse(check[i].IsNeighborTo(check[i - 2]));
                }
            }
        }

        /**
         * Tests for invalid h3Line input
         */
        private static void h3Line_invalid_assertions(H3Index start, H3Index end)
        {
            (int err, _) = start.LineTo(end);
            Assert.AreNotEqual(0, err);
        }

        /**
         * Test for lines from an index to all neighbors within a kRing
         */
        private static void h3Line_kRing_assertions(H3Index h3)
        {
            int r = h3.Resolution;
            Assert.LessOrEqual(r, 5);
            int maxK = MAX_DISTANCES[r];

            if (h3.IsPentagon())
            {
                return;
            }

            var neighbors = h3.KRing(maxK);

            for (int i = 0; i < neighbors.Count; i++)
            {
                if (neighbors[i] == 0)
                {
                    continue;
                }

                int distance = h3.DistanceTo(neighbors[i]);
                if (distance >= 0)
                {
                    h3Line_assertions(h3, neighbors[i]);
                }
                else
                {
                    h3Line_invalid_assertions(h3, neighbors[i]);
                }
            }
        }

        [Test]
        public void h3Line_kRing()
        {
            Utility.IterateAllIndexesAtRes(0, h3Line_kRing_assertions);
            Utility.IterateAllIndexesAtRes(1, h3Line_kRing_assertions);
            Utility.IterateAllIndexesAtRes(2, h3Line_kRing_assertions);
            // Don't iterate all of res 3, to save time
            Utility.IterateAllIndexesAtResPartial(3, h3Line_kRing_assertions, 6);
            // Further resolutions aren't tested to save time.
        }
    }
}
