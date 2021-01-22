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
            var vertexNums = new int[Constants.NUM_HEX_VERTS];

            for (var dir = Direction.K_AXES_DIGIT; dir < Direction.NUM_DIGITS; dir++)
            {
                int vertexNum = origin.VertexNumForDirection(dir);
                Assert.IsTrue(vertexNum>=0 && vertexNum < Constants.NUM_HEX_VERTS);
                Assert.AreEqual(0, vertexNums[vertexNum]);
                vertexNums[vertexNum] = 1;
            }
        }

        [Test]
        public void VertexNumForDirectionPent()
        {
            H3Index pentagon = 0x823007fffffffff;
            var vertexNums = new int[Constants.NUM_PENT_VERTS];
            
            for (var dir = Direction .J_AXES_DIGIT; dir < Direction.NUM_DIGITS ; dir++)
            {
                int vertexNum = pentagon.VertexNumForDirection(dir);
                Assert.IsTrue(vertexNum>=0 && vertexNum<Constants.NUM_PENT_VERTS);
                Assert.AreEqual(0, vertexNums[vertexNum]);
                vertexNums[vertexNum] = 1;
            }
        }

        [Test]
        public void VertexNumForDirectionBadDirections()
        {
            H3Index origin = 0x823007fffffffff;

            Assert.AreEqual
                (origin.VertexNumForDirection(Direction.CENTER_DIGIT), H3Lib.StaticData.Vertex.INVALID_VERTEX_NUM);
            Assert.AreEqual
                (origin.VertexNumForDirection(Direction.INVALID_DIGIT), H3Lib.StaticData.Vertex.INVALID_VERTEX_NUM);

            H3Index pentagon = 0x823007fffffffff;
            Assert.AreEqual
                (pentagon.VertexNumForDirection(Direction.K_AXES_DIGIT), H3Lib.StaticData.Vertex.INVALID_VERTEX_NUM);
        }
    }
}
