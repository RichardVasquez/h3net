using System.Collections.Generic;
using H3Lib;
using H3Lib.Extensions;
using NUnit.Framework;

namespace TestSuite
{
    [TestFixture]
    public class TestH3ToChildren
    {
        private const int PADDED_COUNT = 10;

        private static readonly GeoCoord sf = new GeoCoord(0.659966917655m, 2 * 3.14159m - 2.1364398519396m);
        private static readonly H3Index sfHex8 = sf.ToH3Index(8);

        private void verifyCountAndUniqueness
            (List<H3Index> children, int expectedCount)
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
                Assert.AreEqual(0, indexSeen);
            }
            Assert.AreEqual(expectedCount,numFound);
        }
        
        [Test]
        public void OneResStep()
        {
            const int expectedCount = 7;
            //const int paddedCount = 10;

            var sfHex9s = sfHex8.ToChildren(9);
            var center = sfHex8.ToGeoCoord();
            var sfHex9_0 = center.ToH3Index(9);

            int numFound = 0;
            // Find the center
            for (int i = 0; i < sfHex9s.Count; i++) {
                if (sfHex9_0 == sfHex9s[i]) {
                    numFound++;
                }
            }
            Assert.AreEqual(1,numFound);

            // Get the neighbor hexagons by averaging the center point and outer
            // points then query for those independently

            GeoBoundary outside = sfHex8.ToGeoBoundary();

            for (int i = 0; i < outside.NumVerts; i++)
            {
                GeoCoord avg = new GeoCoord
                    (
                     (outside.Verts[i].Latitude + center.Latitude) / 2,
                     (outside.Verts[i].Longitude + center.Longitude) / 2
                    );
                H3Index avgHex9 = avg.ToH3Index(9);
                for (int j = 0; j < expectedCount; j++) {
                    if (avgHex9 == sfHex9s[j]) {
                        numFound++;
                    }
                }
            }

            Assert.AreEqual(expectedCount,numFound);
        }

        [Test]
        public void multipleResSteps()
        {
            // Lots of children. Will just confirm number and uniqueness
            const int expectedCount = 49;
            var children = sfHex8.ToChildren(10);
            verifyCountAndUniqueness(children, expectedCount);
        }

        [Test]
        public void SameRes()
        {
            const int expectedCount = 1;
            var children = sfHex8.ToChildren(8);
            verifyCountAndUniqueness(children, expectedCount);
        }
        
        [Test]
        public void ChildResTooCoarse()
        {
            const int expectedCount = 0;
            var children = sfHex8.ToChildren(7);
            verifyCountAndUniqueness(children, expectedCount);
        }

        [Test]
        public void ChildResTooFine()
        {
            const int expectedCount = 0;
            var sfHexMax = sf.ToH3Index(Constants.H3.MaxH3Resolution);
            var children = sfHexMax.ToChildren(Constants.H3.MaxH3Resolution + 1);
            verifyCountAndUniqueness(children, expectedCount);
        }
        
        [Test]
        public void PentagonChildren()
        {
            H3Index pentagon = new H3Index(1, 4, 0);

            const int expectedCount = (5 * 7) + 6;
            var children = sfHex8.ToChildren(10);
            children = pentagon.ToChildren(3);

            verifyCountAndUniqueness(children, expectedCount);
        }
        
    }
}
