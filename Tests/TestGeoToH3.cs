using H3Lib;
using H3Lib.Extensions;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class TestGeoToH3
    {
        private void AssertExpected(H3Index h1, GeoCoord g1)
        {
            // convert lat/lon to H3 and verify
            int res = h1.Resolution;
            var h2 = g1.ToH3Index(res);
            Assert.AreEqual(h1, h2);
        }
        
        //  OOPS!  This is a file that operates on batches
        //  TODO: Phase 2 of tests.
    }
}
