using H3Lib;
using H3Lib.Extensions;
using NUnit.Framework;

namespace TestSuite
{
    [TestFixture]
    public class TestH3ToCenterChild
    {
        private H3Index baseHex;
        GeoCoord baseCentroid;
            
        [SetUp]    
        public void Init()
        {
            baseHex = new H3Index(8, 4, 2);
            baseCentroid = baseHex.ToGeoCoord();
        }

        [Test]
        public void PropertyTests()
        {
            for (int res = 0; res <= Constants.H3.MaxH3Resolution - 1; res++)
            {
                for (int childRes = res + 1; childRes <= Constants.H3.MaxH3Resolution; childRes++)
                {
                    var h3 = baseCentroid.ToH3Index(res);
                    var centroid = h3.ToGeoCoord();

                    var geoChild = centroid.ToH3Index(childRes);
                    var centerChild = h3.ToCenterChild(childRes);

                    Assert.AreEqual(centerChild,geoChild);

                    Assert.AreEqual(childRes, centerChild.Resolution);

                    Assert.AreEqual(h3, centerChild.ToParent(res));
                }
            }
        }

        [Test]
        public void SameRes()
        {
            int res = baseHex.Resolution;
            Assert.AreEqual(baseHex, baseHex.ToCenterChild(res));
        }

        [Test]
        public void InvalidInputs()
        {
            int res = baseHex.Resolution;
            Assert.AreEqual(0, baseHex.ToCenterChild(res - 1).Value);
            Assert.AreEqual(0, baseHex.ToCenterChild(-1).Value);
            Assert.AreEqual(0, baseHex.ToCenterChild(Constants.H3.MaxH3Resolution + 1).Value);
        }

    }
}
