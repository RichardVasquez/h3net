using H3Lib.Extensions;
using NUnit.Framework;

namespace TestSuite
{
    [TestFixture]
    public class TestBaseCells
    {
        [Test]
        public void GetRes0Indexes()
        {
            var indexes = BaseCellsExtensions.GetRes0Indexes();
            Assert.AreEqual(indexes[0].Value,   0x8001fffffffffffUL);
            Assert.AreEqual(indexes[121].Value, 0x80f3fffffffffffUL);
        }

        [Test]
        public void BaseCellToCcwRot60()
        {
            // a few random spot-checks
            Assert.AreEqual(16.ToCounterClockwiseRotate60(0), 0);
            Assert.AreEqual(32.ToCounterClockwiseRotate60(0), 3);
            Assert.AreEqual(7.ToCounterClockwiseRotate60(3), 1);
        }

        [Test]
        public void BaseCellToCcwRot60Invalid()
        {
            Assert.AreEqual(16.ToCounterClockwiseRotate60(42), H3Lib.Constants.BaseCells.InvalidRotations);
            Assert.AreEqual(16.ToCounterClockwiseRotate60(-1), H3Lib.Constants.BaseCells.InvalidRotations);
            Assert.AreEqual(11.ToCounterClockwiseRotate60(0), H3Lib.Constants.BaseCells.InvalidRotations);
        }
    }
}
