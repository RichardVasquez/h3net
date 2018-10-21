using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using h3net.API;
using NUnit.Framework;


namespace h3tests
{
    [TestFixture]
    class TestMaxH3ToChildrenSize
    {
        public static GeoCoord sf = new GeoCoord(0.659966917655, 2 * 3.14159 - 2.1364398519396);

        [Test]
        public void maxH3ToChildrenSize()
        {
            H3Index parent = H3Index.geoToH3(ref sf, 7);

            Assert.True(H3Index.maxH3ToChildrenSize(parent, 3) == 0,
                     "got expected size for coarser res");
            Assert.True(H3Index.maxH3ToChildrenSize(parent, 7) == 1,
                     "got expected size for same res");
            Assert.True(H3Index.maxH3ToChildrenSize(parent, 8) == 7,
                     "got expected size for child res");
            Assert.True(H3Index.maxH3ToChildrenSize(parent, 9) == 7 * 7,
                     "got expected size for grandchild res");
        }
    }
}
