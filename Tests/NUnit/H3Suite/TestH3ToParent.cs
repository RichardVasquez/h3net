using H3Lib;
using H3Lib.Extensions;
using NUnit.Framework;

namespace TestSuite
{
    [TestFixture]
    public class TestH3ToParent
    {
        public GeoCoord sf = new GeoCoord(0.659966917655m, 2 * 3.14159m - 2.1364398519396m);

        [Test]
        public void AncestorsForEachRes()
        {
            H3Index child;
            H3Index comparisonParent;
            H3Index parent;

            for (int res = 1; res < 15; res++)
            {
                for (int step = 0; step < res; step++)
                {
                    child = sf.ToH3Index(res);
                    parent = child.ToParent(res - step);
                    comparisonParent = sf.ToH3Index(res - step);
                    Assert.AreEqual(parent, comparisonParent);
                }
            }
        }

        [Test]
        public void invalidInputs()
        {
            var child = sf.ToH3Index(5);

            Assert.AreEqual(0, child.ToParent(6).Value);
            Assert.AreEqual(0, child.ToParent(-1).Value);
            Assert.AreEqual(0, child.ToParent(15).Value);
        }

    }
}
