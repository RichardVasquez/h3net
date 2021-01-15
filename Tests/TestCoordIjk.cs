using H3Lib;
using H3Lib.Extensions;
using NUnit.Framework;

namespace Tests
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

            Assert.AreEqual(zero.ToDirection(), Direction.CENTER_DIGIT);
            Assert.AreEqual(i.ToDirection(), Direction.I_AXES_DIGIT);
            Assert.AreEqual(outOfRange.ToDirection(), Direction.INVALID_DIGIT);
            Assert.AreEqual(unNormalizedZero.ToDirection(), Direction.CENTER_DIGIT);
        }

        [Test]
        public void Neighbor()
        {
            var ijk = new CoordIjk();
            var zero = new CoordIjk();
            var i = new CoordIjk(1, 0, 0);

            ijk = ijk.Neighbor(Direction.CENTER_DIGIT);
            Assert.AreEqual(ijk, zero);

            ijk = ijk.Neighbor(Direction.I_AXES_DIGIT);
            Assert.AreEqual(ijk, i);

            ijk = ijk.Neighbor(Direction.INVALID_DIGIT);
            Assert.AreEqual(ijk, i);
        }
    }
}
