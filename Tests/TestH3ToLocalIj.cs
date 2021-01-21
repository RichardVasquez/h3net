using System.Security.Cryptography.X509Certificates;
using H3Lib;
using H3Lib.Extensions;
using NUnit.Framework;
using H3Index = H3Lib.H3Index;

namespace Tests
{
    [TestFixture]
    public class TestH3ToLocalIj
    {
        // Some indexes that represent base cells. Base cells
        // are hexagons except for `pent1`.
        private H3Index bc1 = new H3Index(0, 15, 0);
        private H3Index bc2 = new H3Index(0,8,0);
        private H3Index bc3 = new H3Index(0, 31, 0);
        private H3Index pent1 = new H3Index(0, 4, 0);

        [Test]
        public void IjkBaseCells()
        {
            var (status, ijk) = pent1.ToLocalIjk(bc1);
            Assert.AreEqual(0,status);
            Assert.AreEqual(H3Lib.StaticData.CoordIjk.UnitVecs[2], ijk);
        }

        [Test]
        public void IjBaseCells()
        {
            H3Index origin = 0x8029fffffffffff;
            CoordIj ij = new CoordIj(0, 0);
            
            (int status, var retrieved) = ij.LocalIjToH3Experimental(origin);
            Assert.AreEqual(0, status);
            Assert.AreEqual(0x8029fffffffffff, retrieved.Value);

            ij = ij.ReplaceI(1);
            (status, retrieved) = ij.LocalIjToH3Experimental(origin);
            Assert.AreEqual(0, status);
            Assert.AreEqual(0x8051fffffffffff, retrieved.Value);

            ij = ij.ReplaceI(2);
            (status, retrieved) = ij.LocalIjToH3Experimental(origin);
            Assert.AreNotEqual(0, status);

            ij = new CoordIj(0, 2);
            (status, retrieved) = ij.LocalIjToH3Experimental(origin);
            Assert.AreNotEqual(0, status);

            ij = new CoordIj(-2, -2);
            (status, retrieved) = ij.LocalIjToH3Experimental(origin);
            Assert.AreNotEqual(0, status);
        }

        [Test]
        public void IjOutOfRange()
        {
            const int numCoords = 7;
            var coords = new[]
                         {
                             new CoordIj(0, 0),
                             new CoordIj(1, 0),
                             new CoordIj(2, 0),
                             new CoordIj(3, 0),
                             new CoordIj(4, 0),
                             new CoordIj(-4, 0),
                             new CoordIj(0, 4)
                         };

            H3Index[] expected =
            {
                0x81283ffffffffff,
                0x81293ffffffffff,
                0x8150bffffffffff,
                0x8151bffffffffff,
                H3Lib.StaticData.H3Index.H3_NULL,
                H3Lib.StaticData.H3Index.H3_NULL,
                H3Lib.StaticData.H3Index.H3_NULL,
            };

            for (var i = 0; i < numCoords; i++)
            {
                
                (int err, var result) = coords[i].LocalIjToH3Experimental(expected[0]);

                if (expected[i] == H3Lib.StaticData.H3Index.H3_NULL)
                {
                    Assert.AreNotEqual(0, err);
                }
                else
                {
                    Assert.AreEqual(0,err);
                    Assert.AreEqual(expected[i], result, $"{i}");
                }
            }
        }

        [Test]
        public void ExperimentalH3ToLocalIjFailed()
        {
            var (status, ij) = bc1.ToLocalIjExperimental(bc1);
            Assert.AreEqual(0, status);
            Assert.IsTrue(ij.I == 0 && ij.J == 0);

            (status, ij) = bc1.ToLocalIjExperimental(pent1);
            Assert.AreEqual(0, status);
            Assert.IsTrue(ij.I == 1 && ij.J == 0);
            
            (status, ij) = bc1.ToLocalIjExperimental(bc2);
            Assert.AreEqual(0, status);
            Assert.IsTrue(ij.I == 0 && ij.J == -1);
            
            (status, ij) = bc1.ToLocalIjExperimental(bc3);
            Assert.AreEqual(0, status);
            Assert.IsTrue(ij.I == -1 && ij.J == 0);
            
            (status, ij) = pent1.ToLocalIjExperimental(bc3);
            Assert.AreNotEqual(0, status);
        }

        [Test]
        public void OnOffPentagonSame()
        {
            //  Test that coming from the same direction outside the pentagon is handled
            //  the same as coming from the same direction inside the pentagon.
            for (var bc = 0; bc < Constants.NUM_BASE_CELLS; bc++)
            {
                for (var res = 1; res <= Constants.MAX_H3_RES; res++)
                {
                    // K_AXES_DIGIT is the first internal direction, and it's also
                    // invalid for pentagons, so skip to next.
                    var startDir = Direction.K_AXES_DIGIT;
                    if (bc.IsBaseCellPentagon())
                    {
                        startDir++;
                    }

                    for (var dir = startDir; dir < Direction.NUM_DIGITS; dir++)
                    {
                        var internalOrigin = new H3Index(res, bc, dir);
                        var externalOrigin = new H3Index(res, bc.GetNeighbor(dir), Direction.CENTER_DIGIT);

                        for (var testDir = startDir; testDir < Direction.NUM_DIGITS; testDir++)
                        {
                            var testIndex = new H3Index(res, bc, testDir);
                            (int internalIjFailed, var internalIj) = internalOrigin.ToLocalIjExperimental(testIndex);
                            (int externalIjFailed, var externalIj) = externalOrigin.ToLocalIjExperimental(testIndex);

                            Assert.AreEqual(externalIjFailed != 0, internalIjFailed != 0);

                            if (internalIjFailed!=0)
                            {
                                continue;
                            }

                            (int internalIjFailed2, var internalIndex) = internalIj.LocalIjToH3Experimental(internalOrigin);
                            (int externalIjFailed2, var externalIndex) = externalIj.LocalIjToH3Experimental(externalOrigin);

                            Assert.AreEqual(externalIjFailed2 != 0, internalIjFailed2 != 0);

                            if (internalIjFailed2 != 0)
                            {
                                continue;
                            }

                            Assert.AreEqual(externalIndex, internalIndex);
                        }
                    }
                }
            }
        }
    }
}
