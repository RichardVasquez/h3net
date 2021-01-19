using H3Lib;
using H3Lib.Extensions;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class TestMaxH3ToChildrenSize
    {
        private GeoCoord sf = new GeoCoord(0.659966917655, 2 * 3.14159 - 2.1364398519396);

        [Test]
        public void MaxH3ToChildrenSize()
        {
            var parent = sf.ToH3Index(7);

            Assert.AreEqual(0, parent.MaxChildrenSize(3));
            Assert.AreEqual(1, parent.MaxChildrenSize(7));
            Assert.AreEqual(7, parent.MaxChildrenSize(8));
            Assert.AreEqual(49, parent.MaxChildrenSize(9));
        }

        [Test]
        public void maxH3ToChildrenSize_largest()
        {
            // write out the types explicitly, to make sure errors don't go
            // undetected to to type casting.

            H3Index h = 0x806dfffffffffff;       // res 0 *hexagon*
            ulong expected = 4747561509943L;  // 7^15
            long outCount = h.MaxChildrenSize(15);
            Assert.AreEqual(expected, (ulong) outCount);
        }
    }
}
