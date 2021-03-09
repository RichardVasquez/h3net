using H3Lib;
using H3Lib.Extensions;
using NUnit.Framework;

namespace TestSuite
{
    [TestFixture]
    public class TestCoordIjk
    {
        [Test]
        public void UnitIjkToDigit()
        {
            var zero = new CoordIjk();
            var i = new CoordIjk(1, 0, 0);
            var outOfRange = new CoordIjk(2, 0, 0);
            var unNormalizedZero = new CoordIjk(2, 2, 2);

            Assert.AreEqual(zero.ToDirection(), Direction.CenterDigit);
            Assert.AreEqual(i.ToDirection(), Direction.IAxesDigit);
            Assert.AreEqual(outOfRange.ToDirection(), Direction.InvalidDigit);
            Assert.AreEqual(unNormalizedZero.ToDirection(), Direction.CenterDigit);
        }

        [Test]
        public void Neighbor()
        {
            var ijk = new CoordIjk();
            var zero = new CoordIjk();
            var i = new CoordIjk(1, 0, 0);

            ijk = ijk.Neighbor(Direction.CenterDigit);
            Assert.AreEqual(ijk, zero);

            ijk = ijk.Neighbor(Direction.IAxesDigit);
            Assert.AreEqual(ijk, i);

            ijk = ijk.Neighbor(Direction.InvalidDigit);
            Assert.AreEqual(ijk, i);
        }
    }
}
