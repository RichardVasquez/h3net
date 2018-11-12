using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using H3Net.Code;
using NUnit.Framework;

namespace h3tests
{
    [TestFixture]
    class TestH3ToParent
    {
        private static GeoCoord sf = new GeoCoord(0.659966917655, 2 * 3.14159 - 2.1364398519396);

        [Test]
        public void ancestorsForEachRes()
        {
            for (int res = 1; res < 15; res++)
            {
                for (int step = 0; step < res; step++)
                {
                    var child = H3Index.geoToH3(ref sf, res);
                    var parent = H3Index.h3ToParent(child, res - step);
                    var comparisonParent = H3Index.geoToH3(ref sf, res - step);

                    Assert.True(parent == comparisonParent, "Got expected parent");
                }
            }
        }

        [Test]
        public void invalidInputs()
        {
            H3Index child = H3Index.geoToH3(ref sf, 5);

            Assert.True(H3Index.h3ToParent(child, 6) == 0, "Higher resolution fails");
            Assert.True(H3Index.h3ToParent(child, -1) == 0, "Invalid resolution fails");
            Assert.True(H3Index.h3ToParent(child, 15) == 0, "Invalid resolution fails");
        }
    }
}
/*
BEGIN_TESTS(h3ToParent);

GeoCoord sf = {0.659966917655, 2 * 3.14159 - 2.1364398519396};

TEST(ancestorsForEachRes) {
    H3Index child;
    H3Index comparisonParent;
    H3Index parent;

    for (int res = 1; res < 15; res++) {
        for (int step = 0; step < res; step++) {
            child = H3_EXPORT(geoToH3)(&sf, res);
            parent = H3_EXPORT(h3ToParent)(child, res - step);
            comparisonParent = H3_EXPORT(geoToH3)(&sf, res - step);

            t_assert(parent == comparisonParent, "Got expected parent");
        }
    }
}

TEST(invalidInputs) {
    H3Index child = H3_EXPORT(geoToH3)(&sf, 5);

    t_assert(H3_EXPORT(h3ToParent)(child, 6) == 0, "Higher resolution fails");
    t_assert(H3_EXPORT(h3ToParent)(child, -1) == 0, "Invalid resolution fails");
    t_assert(H3_EXPORT(h3ToParent)(child, 15) == 0, "Invalid resolution fails");
}


 */