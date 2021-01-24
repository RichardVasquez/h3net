using H3Lib;
using H3Lib.Extensions;
using NUnit.Framework;
using TestSuite.Lib;

namespace TestSuite
{
    [TestFixture]
    public class TestH3DistanceExhaustive
    {
        private static readonly int[] MAX_DISTANCES = {1, 2, 5, 12, 19, 26};

        private static void h3Distance_identity_assertions(H3Index h3)
        {
            Assert.AreEqual(0, h3.DistanceTo(h3));
        }

        private static void h3Distance_kRing_assertions(H3Index h3)
        {
            int r = h3.Resolution;
            Assert.LessOrEqual(r, 5);
            int maxK = MAX_DISTANCES[r];

            var lookup = h3.KRingDistances(maxK);

            foreach ((H3Index cell, int distance) in lookup)
            {
                if (lookup[cell] == 0)
                {
                    continue;
                }

                int calcDistance = h3.DistanceTo(cell);
                Assert.IsTrue(calcDistance == distance || calcDistance == -1);
            }
        }

        [Test]
        public void h3Distance_identity()
        {
            Utility.IterateAllIndexesAtRes(0, h3Distance_identity_assertions);
            Utility.IterateAllIndexesAtRes(1, h3Distance_identity_assertions);
            Utility.IterateAllIndexesAtRes(2, h3Distance_identity_assertions);
        }

        [Test]
        public void h3Distance_kRing()
        {
            Utility.IterateAllIndexesAtRes(0, h3Distance_kRing_assertions);
            Utility.IterateAllIndexesAtRes(1, h3Distance_kRing_assertions);
            Utility.IterateAllIndexesAtRes(2, h3Distance_kRing_assertions);
            // Don't iterate all of res 3, to save time
            Utility.IterateAllIndexesAtResPartial(3, h3Distance_kRing_assertions, 27);
            // Further resolutions aren't tested to save time.
        }

        
    }
}
