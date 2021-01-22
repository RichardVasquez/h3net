using H3Lib;
using H3Lib.Extensions;
using NUnit.Framework;

namespace TestSuite
{
    [TestFixture]
    public class TestH3Line
    {
        [Test]
        public void H3LineAcrossMultipleFaces()
        {
            H3Index start = 0x85285aa7fffffff;
            H3Index end = 0x851d9b1bfffffff;

            int lineSz = start.LineSize(end);
            Assert.Less(lineSz, 0);
        }
    }
}
