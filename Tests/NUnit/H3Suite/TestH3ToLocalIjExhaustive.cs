using System.Threading.Tasks;
using NUnit.Framework;
using H3Lib;
using H3Lib.Extensions;
using TestSuite.Lib;

namespace TestSuite
{
    [TestFixture]
    public class TestH3ToLocalIjExhaustive
    {
        private readonly int[] _maxDistances = {1, 2, 5, 12, 19, 26};

        //  The same traversal constants from algos.c (for hexRange) here
        //  reused as local IJ vectors.
        private readonly CoordIj[] _directions =
        {
            new CoordIj(0, 1),
            new CoordIj(-1, 0),
            new CoordIj(-1, -1),
            new CoordIj(0, -1),
            new CoordIj(1, 0),
            new CoordIj(1, 1)
        };

        private readonly CoordIj _nextRingDirection = new CoordIj(1, 0);

        /// <summary>
        /// Test that the local coordinates for an index map to itself.
        /// </summary>
        private void LocalIjToH3IdentityAssertions(H3Index h3)
        {
            int status;
            CoordIj ij;
            (status, ij) = h3.ToLocalIjExperimental(h3);
            Assert.AreEqual(0, status);

            H3Index retrieved;
            (status, retrieved) = ij.ToH3Experimental(h3);
            Assert.AreEqual(0, status);
            Assert.AreEqual(h3, retrieved);
        }

        /// <summary>
        /// Test that coordinates for an index match some simple rules about index
        /// digits, when using the index as its own origin. That is, that the IJ
        /// coordinates are in the coordinate space of the origin's base cell.
        /// </summary>
        private void H3ToLocalIjCoordinatesAssertions(H3Index h3)
        {
            int r = h3.Resolution;

            CoordIj ij;
            int status;
            (status, ij) = h3.ToLocalIjExperimental(h3);
            Assert.AreEqual(0, status);
            var ijk = ij.ToIjk();

            switch (r)
            {
                case 0:
                    Assert.AreEqual(ijk, H3Lib.Constants.CoordIjk.UnitVecs[0]);
                    break;
                case 1:
                    Assert.AreEqual(ijk, H3Lib.Constants.CoordIjk.UnitVecs[(int) h3.GetIndexDigit(1)]);
                    break;
                case 2:
                {
                    var expected = H3Lib.Constants.CoordIjk.UnitVecs[(int) h3.GetIndexDigit(1)];
                    expected = expected.DownAp7R().Neighbor(h3.GetIndexDigit(2));
                    Assert.AreEqual(ijk, expected);
                    break;
                }
                default:
                    Assert.IsTrue(true);
                    break;
            }
        }

        /// <summary>
        /// Test the the immediate neighbors of an index are at the expected locations in
        /// the local IJ coordinate space.
        /// </summary>
        private void H3ToLocalIjNeighborsAssertions(H3Index h3)
        {
            (int status, var origin) = h3.ToLocalIjExperimental(h3);
            Assert.AreEqual(0, status);

            var originIjk = origin.ToIjk();

            for (var d = Direction.KAxesDigit; d < Direction.InvalidDigit; d++)
            {
                if (d == Direction.KAxesDigit && h3.IsPentagon)
                {
                    continue;
                }

                const int rotations = 0;
                H3Index offset;
                (offset, _) = h3.NeighborRotations(d, rotations);

                (int result, var ij) = h3.ToLocalIjExperimental(offset);
                Assert.AreEqual(0, result);

                var ijk = ij.ToIjk();
                var invertedIjk = new CoordIjk().Neighbor(d);

                for (int i = 0; i < 3; i++)
                {
                    invertedIjk = invertedIjk.Rotate60CounterClockwise();
                }

                ijk += invertedIjk;
                ijk = ijk.Normalized();

                Assert.AreEqual(originIjk, ijk);
            }
        }

        /// <summary>
        /// Test that the neighbors (k-ring), if they can be found in the local IJ
        /// coordinate space, can be converted back to indexes.
        /// </summary>
        private void LocalIjToH3KRingAssertions(H3Index h3)
        {
            int r = h3.Resolution;
            Assert.LessOrEqual(r, 5);

            int maxK = _maxDistances[r];

            int sz = maxK.MaxKringSize();
            var lookup = h3.KRingDistances(maxK);
            // <H3Index, int>

            var index = 0;
            foreach ((var key, int value) in lookup)
            {
                index++;
                if (key == 0)
                {
                    continue;
                }

                (int status, var ij) = h3.ToLocalIjExperimental(key);
                if (status != 0)
                {
                    continue;
                }

                (int check, H3Index retrieved) = ij.ToH3Experimental(h3);
                Assert.AreEqual(0, check);
                Assert.AreEqual(key, retrieved);
            }
        }

        private void LocalIjToH3TraverseAssertions(H3Index h3)
        {
            var r = h3.Resolution;
            Assert.LessOrEqual(r, 5);
            int k = _maxDistances[r];

            (int status, var ij) = h3.ToLocalIjExperimental(h3);
            Assert.AreEqual(0, status);

            // This logic is from hexRangeDistances.
            // 0 < ring <= k, current ring
            int ring = 1;
            // 0 <= direction < 6, current side of the ring
            int direction = 0;
            // 0 <= i < ring, current position on the side of the ring
            int i = 0;

            while (ring <= k)
            {
                if (direction == 0 && i == 0)
                {
                    ij += _nextRingDirection;
                }

                ij += _directions[direction];

                (var failed, var testH3) = ij.ToH3Experimental(h3);
                if (failed == 0)
                {
                    Assert.IsTrue(testH3.IsValid());

                    (var reverseFailed, var expectedIj) = h3.ToLocalIjExperimental(testH3);
                    // If it doesn't give a coordinate for this origin,index pair that's
                    // OK.
                    if (reverseFailed == 0)
                    {
                        if (expectedIj != ij)
                        {
                            // Multiple coordinates for the same index can happen due to
                            // pentagon distortion. In that case, the other coordinates
                            // should also belong to the same index.
                            (var testTestStatus, var testTestH3) = expectedIj.ToH3Experimental(h3);
                            Assert.AreEqual(0, testTestStatus);
                            Assert.AreEqual(testTestH3, testH3);
                        }
                    }
                }

                i++;
                // Check if end of this side of the k-ring
                if (i == ring)
                {
                    i = 0;
                    direction++;
                    // Check if end of this ring.
                    if (direction == 6)
                    {
                        direction = 0;
                        ring++;
                    }
                }
            }
        }

        [Test]
        public void LocalIjToH3Identity()
        {
            Utility.IterateAllIndexesAtRes(0, LocalIjToH3IdentityAssertions);
            Utility.IterateAllIndexesAtRes(1, LocalIjToH3IdentityAssertions);
            Utility.IterateAllIndexesAtRes(2, LocalIjToH3IdentityAssertions);
        }

        [Test]
        public void H3ToLocalIjCoordinates()
        {
            Utility.IterateAllIndexesAtRes(0, H3ToLocalIjCoordinatesAssertions);
            Utility.IterateAllIndexesAtRes(1, H3ToLocalIjCoordinatesAssertions);
            Utility.IterateAllIndexesAtRes(2, H3ToLocalIjCoordinatesAssertions);
        }

        [Test]
        public void H3ToLocalIjNeighbors()
        {
            Utility.IterateAllIndexesAtRes(0, H3ToLocalIjNeighborsAssertions);
            Utility.IterateAllIndexesAtRes(1, H3ToLocalIjNeighborsAssertions);
            Utility.IterateAllIndexesAtRes(2, H3ToLocalIjNeighborsAssertions);
        }

        [Test]
        public void LocalIjToH3KRing()
        {
            Utility.IterateAllIndexesAtRes(0, LocalIjToH3KRingAssertions);
            Utility.IterateAllIndexesAtRes(1, LocalIjToH3KRingAssertions);
            Utility.IterateAllIndexesAtRes(2, LocalIjToH3KRingAssertions);
            // Don't iterate all of res 3, to save time
            Utility.IterateAllIndexesAtResPartial(3, LocalIjToH3KRingAssertions, 27);
            // Further resolutions aren't tested to save time.
        }

        [Test]
        public void LocalIjToH3Traverse()
        {
            Utility.IterateAllIndexesAtRes(0, LocalIjToH3TraverseAssertions);
            Utility.IterateAllIndexesAtRes(1, LocalIjToH3TraverseAssertions);
            Utility.IterateAllIndexesAtRes(2, LocalIjToH3TraverseAssertions);
            // Don't iterate all of res 3, to save time
            Utility.IterateAllIndexesAtResPartial(3, LocalIjToH3TraverseAssertions, 27);
            // Further resolutions aren't tested to save time.
        }
    }
}
