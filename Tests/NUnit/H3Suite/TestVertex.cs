using H3Lib;
using H3Lib.Extensions;
using NUnit.Framework;

namespace TestSuite
{
    [TestFixture]
    public class TestVertex
    {
        [Test]
        public void VertexNumForDirectionHex()
        {
            H3Index origin = 0x823d6ffffffffff;
            var vertexNums = new int[Constants.H3.HexVertices];

            for (var dir = Direction.KAxesDigit; dir < Direction.NumDigits; dir++)
            {
                int vertexNum = origin.VertexNumForDirection(dir);
                Assert.IsTrue(vertexNum>=0 && vertexNum < Constants.H3.HexVertices);
                Assert.AreEqual(0, vertexNums[vertexNum]);
                vertexNums[vertexNum] = 1;
            }
        }

        [Test]
        public void VertexNumForDirectionPent()
        {
            H3Index pentagon = 0x823007fffffffff;
            var vertexNums = new int[Constants.H3.PentagonVertices];
            
            for (var dir = Direction .JAxesDigit; dir < Direction.NumDigits ; dir++)
            {
                int vertexNum = pentagon.VertexNumForDirection(dir);
                Assert.IsTrue(vertexNum>=0 && vertexNum<Constants.H3.PentagonVertices);
                Assert.AreEqual(0, vertexNums[vertexNum]);
                vertexNums[vertexNum] = 1;
            }
        }

        [Test]
        public void VertexNumForDirectionBadDirections()
        {
            H3Index origin = 0x823007fffffffff;

            Assert.AreEqual
                (origin.VertexNumForDirection(Direction.CenterDigit), H3Lib.Constants.Vertex.INVALID_VERTEX_NUM);
            Assert.AreEqual
                (origin.VertexNumForDirection(Direction.InvalidDigit), H3Lib.Constants.Vertex.INVALID_VERTEX_NUM);

            H3Index pentagon = 0x823007fffffffff;
            Assert.AreEqual
                (pentagon.VertexNumForDirection(Direction.KAxesDigit), H3Lib.Constants.Vertex.INVALID_VERTEX_NUM);
        }
    }
}
