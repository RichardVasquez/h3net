using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using h3net.API;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace h3tests
{
    [TestFixture]
    class TestH3ToChildren
    {
        private const int PADDED_COUNT = 10;
        static GeoCoord sf = new GeoCoord(0.659966917655, 2 * 3.14159 - 2.1364398519396);
        H3Index sfHex8 = H3Index.geoToH3(ref sf, 8);

        private void verifyCountAndUniqueness(List<H3Index> children, int paddedCount,
                                      int expectedCount)
        {
            int numFound = 0;
            for (int i = 0; i < children.Count; i++)
            {
                H3Index currIndex = children[i];
                if (currIndex == 0)
                {
                    continue;
                }
                numFound++;
                // verify uniqueness
                int indexSeen = 0;
                for (int j = i + 1; j < children.Count; j++)
                {
                    if (children[j] == currIndex) {
                        indexSeen++;
                    }
                }
                Assert.True(indexSeen == 0, "index was seen only once");
            }
            Assert.True(numFound == expectedCount, "got expected number of children");
        }

        [Test]
        public void oneResStep()
        {
            const int expectedCount = 7;

            var sfHex9s = new List<H3Index>();
            for (var i = 0; i < PADDED_COUNT; i++)
            {
                sfHex9s.Add(0L);
            }

            H3Index.h3ToChildren(sfHex8, 9,ref sfHex9s);

            GeoCoord center = new GeoCoord();
            H3Index.h3ToGeo(sfHex8, ref center);
            H3Index sfHex9_0 = H3Index.geoToH3(ref center, 9);

            int numFound = 0;

            // Find the center
            for (int i = 0; i < sfHex9s.Count; i++)
            {
                if (sfHex9_0 == sfHex9s[i])
                {
                    numFound++;
                }
            }
            Assert.True(numFound == 1, "found the center hex");

            // Get the neighbor hexagons by averaging the center point and outer points
            // then query for those independently
            GeoBoundary outside = new GeoBoundary();
            H3Index.h3ToGeoBoundary(sfHex8, ref outside);
            for (int i = 0; i < outside.numVerts; i++)
            {
                GeoCoord avg = new GeoCoord(0,0);
                avg.lat = (outside.verts[i].lat + center.lat) / 2;
                avg.lon = (outside.verts[i].lon + center.lon) / 2;
                H3Index avgHex9 = H3Index.geoToH3(ref avg, 9);
                for (int j = 0; j < expectedCount; j++)
                {
                    if (avgHex9 == sfHex9s[j])
                    {
                        numFound++;
                    }
                }
            }

            Assert.True(numFound == expectedCount, "found all expected children");
        }


        [Test]
        public void multipleResSteps() 
        {
            // Lots of children. Will just confirm number and uniqueness
            const int expectedCount = 49;
            const int paddedCount = 60;

            var children = new ulong[paddedCount].Select(child => new H3Index(child)).ToList();

            H3Index.h3ToChildren(sfHex8, 10, ref children);

            verifyCountAndUniqueness(children, paddedCount, expectedCount);
        }

        [Test]
        public void sameRes()
        {
            const int expectedCount = 1;
            const int paddedCount = 7;

            var children = new ulong[paddedCount].Select(child => new H3Index(child)).ToList();
            H3Index.h3ToChildren(sfHex8, 8, ref children);

            verifyCountAndUniqueness(children, paddedCount, expectedCount);
        }

        [Test]
        public void childResTooHigh()
        {
            const int expectedCount = 0;
            const int paddedCount = 7;
            var children = new ulong[paddedCount].Select(child => new H3Index(child)).ToList();

            H3Index.h3ToChildren(sfHex8, 7, ref children);

            verifyCountAndUniqueness(children, paddedCount, expectedCount);
        }

        [Test]
        public void pentagonChildren()
        {
            H3Index pentagon = 0;
            H3Index.setH3Index(ref pentagon, 1, 4, 0);

            const int expectedCount = (5 * 7) + 6;
            int paddedCount = H3Index.maxH3ToChildrenSize(pentagon, 3);
            var children = new ulong[paddedCount].Select(child => new H3Index(child)).ToList();

            H3Index.h3ToChildren(sfHex8, 10, ref children);
            H3Index.h3ToChildren(pentagon, 3, ref children);

            verifyCountAndUniqueness(children, paddedCount, expectedCount);
        }


    }


}

