using System.Linq;
using H3Lib;
using H3Lib.Extensions;
using NUnit.Framework;
using TestSuite.Lib;

namespace TestSuite
{
    [TestFixture]
    public class TestH3GetFaces
    {
        private static int CountFaces(H3Index h3)
        {
            var faces = h3.GetFaces();
            return faces.Count(face => face >= 0 && face <= 19);
        }
        
        private static void AssertSingleHexFace(H3Index h3)
        {
            int validCount = CountFaces(h3);
            Assert.AreEqual(1,validCount);
        }
        
        private void AssertMultipleHexFaces(H3Index h3)
        {
            int validCount = CountFaces(h3);
            Assert.AreEqual(2, validCount);
        }
        
        private void AssertPentagonFaces(H3Index h3)
        {
            Assert.IsTrue(h3.IsPentagon);
            int validCount = CountFaces(h3);
            Assert.AreEqual(5, validCount);
        }
        
        [Test]
        public void SingleFaceHexes()
        {
            // base cell 16 is at the center of an icosahedron face,
            // so all children should have the same face
            Utility.IterateBaseCellIndexesAtRes(2, AssertSingleHexFace, 16);
            Utility.IterateBaseCellIndexesAtRes(3, AssertSingleHexFace, 16);
        }
        
        [Test]
        public void HexagonWithEdgeVertices()
        {
            // Class II pentagon neighbor - one face, two adjacent vertices on edge
            H3Index h3 = 0x821c37fffffffff;
            AssertSingleHexFace(h3);
        }
        
        [Test]
        public void HexagonWithDistortion()
        {
            // Class III pentagon neighbor, distortion across faces
            H3Index h3 = 0x831c06fffffffff;
            AssertMultipleHexFaces(h3);
        }
        
        [Test]
        public void HexagonCrossingFaces()
        {
            // Class II hex with two vertices on edge
            H3Index h3 = 0x821ce7fffffffff;
            AssertMultipleHexFaces(h3);
        }
        
        [Test]
        public void ClassIiiPentagon()
        {
            H3Index pentagon = new H3Index(1, 4, 0);
            AssertPentagonFaces(pentagon);
        }
        
        [Test]
        public void ClassIiPentagon()
        {
            H3Index pentagon = new H3Index(2, 4, 0);
            AssertPentagonFaces(pentagon);
        }
        
        [Test]
        public void Res15Pentagon()
        {
            H3Index pentagon = new H3Index(15, 4, 0);
            AssertPentagonFaces(pentagon);
        }
        
        [Test]
        public void BaseCellHexagons()
        {
            int singleCount = 0;
            int multipleCount = 0;
            for (int i = 0; i < Constants.H3.BaseCellsCount; i++)
            {
                if (!i.IsBaseCellPentagon())
                {
                    // Make the base cell index
                    H3Index baseCell = new H3Index(0, i, 0);
                    int validCount = CountFaces(baseCell);
                    Assert.Greater(validCount, 0);
                    if (validCount == 1)
                    {
                        singleCount++;
                    }
                    else
                    {
                        multipleCount++;
                    }
                }
            }
            Assert.AreEqual(80,singleCount);
            Assert.AreEqual(30,multipleCount);
        }
        
        [Test]
        public void BaseCellPentagons()
        {
            for (var i = 0; i < Constants.H3.BaseCellsCount; i++)
            {
                if (!i.IsBaseCellPentagon())
                {
                    continue;
                }
                // Make the base cell index
                var baseCell = new H3Index(0, i, 0);
                AssertPentagonFaces(baseCell);
            }
        }
    }
}
