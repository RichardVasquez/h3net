using H3Lib;
using H3Lib.Extensions;
using NUnit.Framework;

namespace TestSuite
{
    [TestFixture]
    public class TestPentagonIndexes
    {

        private const int PADDED_COUNT = 16;

        [Test]
        public void propertyTests()
        {
            int expectedCount = H3Index.PentagonIndexCount;

            for (int res = 0; res <= 15; res++)
            {

                //var h3Indexes = new H3Index[PADDED_COUNT];
                var h3Indexes = res.GetPentagonIndexes();

                int numFound = 0;

                for (int i = 0; i < h3Indexes.Count; i++)
                {
                    H3Index h3Index = h3Indexes[i];
                    if (h3Index != 0)
                    {
                        numFound++;
                        Assert.IsTrue(h3Index.IsValid());
                        Assert.IsTrue(h3Index.IsPentagon());
                        Assert.AreEqual(res, h3Index.Resolution);


                        // verify uniqueness
                        for (int j = i + 1; j < h3Indexes.Count; j++)
                        {
                            Assert.AreNotEqual(h3Index, h3Indexes[j]);
                        }
                    }
                }
                Assert.AreEqual(expectedCount, numFound);
            }
        }
    }
}
