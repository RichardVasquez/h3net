using h3net.API;
using NUnit.Framework;

namespace h3tests
{
    [TestFixture]
    public class TestCoordIj
    {
        [Test]
        public void ijkToIj_zero()
        {
            CoordIJK ijk = new CoordIJK();
            LocalIJ.CoordIJ ij = new LocalIJ.CoordIJ();

            CoordIJK.ijkToIj(ijk, ref ij);
            Assert.True(ij.i == 0, "ij.i zero");
            Assert.True(ij.j == 0, "ij.j zero");

            CoordIJK.ijToIjk(ij, ref ijk);
            Assert.True(ijk.i == 0, "ijk.i zero");
            Assert.True(ijk.j == 0, "ijk.j zero");
            Assert.True(ijk.k == 0, "ijk.k zero");
        }

        [Test]
        public void ijkToIj_roundtrip()
        {
            for (Direction dir = Direction.CENTER_DIGIT; dir < Direction.NUM_DIGITS; dir++)
            {
                CoordIJK ijk = new CoordIJK();
                CoordIJK._neighbor(ref ijk, dir);

                LocalIJ.CoordIJ ij = new LocalIJ.CoordIJ();
                CoordIJK.ijkToIj(ijk, ref ij);

                CoordIJK recovered = new CoordIJK();
                CoordIJK.ijToIjk(ij, ref recovered);

                Assert.True(CoordIJK._ijkMatches(ijk, recovered) !=0,
                         "got same ijk coordinates back");
            }
        }
    }
}
