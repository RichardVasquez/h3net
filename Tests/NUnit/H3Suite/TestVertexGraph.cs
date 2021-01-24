using System.Xml.Schema;
using H3Lib;
using H3Lib.Extensions;
using NUnit.Framework;

namespace TestSuite
{
    [TestFixture]
    public class TestVertexGraph
    {
        private static GeoCoord center = new GeoCoord().SetDegrees(37.77362016769341, -122.41673772517154);
        private static GeoCoord vertex1 = new GeoCoord().SetDegrees(87.372002166, 166.160981117);
        private static GeoCoord vertex2 = new GeoCoord().SetDegrees(87.370101364, 166.160184306);
        private static GeoCoord vertex3 = new GeoCoord().SetDegrees(87.369088356, 166.196239997);
        private static GeoCoord vertex4 = new GeoCoord().SetDegrees(87.369975080, 166.233115768);
        private static GeoCoord vertex5 = new GeoCoord().SetDegrees(0, 0);
        private static GeoCoord vertex6 = new GeoCoord().SetDegrees(-10, -10);

        // General tests involving basic hash testing removed as they looked at buckets
        // and use H3's hash to place data in buckets, while h3net GeoCoord use .Net
        // GetHash and is stored as a readonly struct to maintain hash integrity along
        // h3net using .Net HashSet<T> to store wrapping readonly structs.
        //
        // Removed tests:
        //      makeVertexGraph
        //      vertexHash
        //      vertexHashNegative
        //
        // Further tests will be modified to ignore buckets
        
        [Test]
        public void AddVertexNode()
        {
            var graph = new VertexGraph(9);

            // Basic add
            var addedNode = graph.AddNode(vertex1, vertex2);
            var node = graph.FindEdge(vertex1, vertex2);
            Assert.IsNotNull(node);
            Assert.AreEqual(addedNode,node.Value);
            Assert.AreEqual(1,graph.Size);

            // Collision add
            addedNode = graph.AddNode(vertex1, vertex3);
            node = graph.FindEdge(vertex1, vertex3);
            Assert.IsNotNull(node);
            Assert.AreEqual(addedNode, node.Value);
            Assert.AreEqual(2,graph.Size);
            
            // Collision add #2
            addedNode = graph.AddNode(vertex1, vertex4);
            node = graph.FindEdge(vertex1, vertex4);
            Assert.IsNotNull(node);
            Assert.AreEqual(addedNode, node.Value);
            Assert.AreEqual(3, graph.Size);

            // Exact match no-op
            node = graph.FindEdge(vertex1, vertex2);
            addedNode = graph.AddNode(vertex1, vertex2);
            Assert.IsNotNull(node);
            Assert.AreEqual(graph.FindEdge(vertex1, vertex2), node.Value);
            Assert.AreEqual(node.Value,addedNode);
            Assert.AreEqual(3,graph.Size);

            graph.Clear();
        }

        [Test]
        public void AddVertexNodeDupe()
        {
            
            VertexGraph graph = new VertexGraph(9);

            // Basic add
            var addedNode = graph.AddNode(vertex1, vertex2);
            var node = graph.FindEdge(vertex1, vertex2);
            Assert.IsNotNull(node);
            Assert.AreEqual(addedNode,node.Value);
            Assert.AreEqual(1,graph.Size);

            // Dupe add
            addedNode = graph.AddNode(vertex1, vertex2);
            Assert.AreEqual(addedNode,node.Value);
            Assert.AreEqual(1,graph.Size);
            
            graph.Clear();
        }

        [Test]
        public void FindNodeForEdge()
        {
            // Basic lookup tested in testAddVertexNode, only test failures here
            VertexGraph graph = new VertexGraph(9);

            // Empty graph
            var node = graph.FindEdge(vertex1, vertex2);
            Assert.IsNull(node);
            graph.AddNode(vertex1, vertex2);

            // Different hash
            node = graph.FindEdge(vertex3, vertex2);
            Assert.IsNull(node);

            // Hash collision
            node = graph.FindEdge(vertex1, vertex3);
            Assert.IsNull(node);

            graph.AddNode(vertex1, vertex4);

            // Hash collision, list iteration
            node = graph.FindEdge(vertex1, vertex3);
            Assert.IsNull(node);
            graph.Clear();
        }

        [Test]
        public void FindNodeForVertex()
        {
            VertexGraph graph = new VertexGraph(9);

            // Empty graph
            var node = graph.FindVertex(vertex1);
            Assert.IsNull(node);

            graph.AddNode(vertex1, vertex2);

            node = graph.FindVertex(vertex1);
            Assert.IsNotNull(node);
            node = graph.FindVertex(vertex3);
            Assert.IsNull(node);

            graph.Clear();
        }

        [Test]
        public void RemoveVertexNode()
        {
            VertexGraph graph = new VertexGraph(9);

            // Straight removal
            var node = graph.AddNode(vertex1, vertex2);
            var success = graph.RemoveNode(node);

            Assert.IsTrue(success);
            Assert.IsNull(graph.FindVertex(vertex1));
            Assert.AreEqual(0, graph.Size);

            // Remove end of list
            graph.AddNode(vertex1, vertex2);
            node = graph.AddNode(vertex1, vertex3);
            success = graph.RemoveNode(node);

            Assert.IsTrue(success);
            Assert.IsNull(graph.FindEdge(vertex1,vertex3));
            Assert.AreEqual(1, graph.Size);

            // This removal is just cleanup
            node = graph.FindVertex(vertex1);
            Assert.IsTrue(graph.RemoveNode(node));

            // Remove beginning of list
            node = graph.AddNode(vertex1, vertex2);
            graph.AddNode(vertex1, vertex3);
            success = graph.RemoveNode(node);

            Assert.IsTrue(success);

            Assert.IsNull(graph.FindEdge(vertex1,vertex2));
            Assert.IsNotNull(graph.FindEdge(vertex1,vertex3));
            Assert.AreEqual(1, graph.Size);

            // This removal is just cleanup
            node = graph.FindVertex(vertex1);
            Assert.IsTrue(graph.RemoveNode(node));

            // Remove middle of list
            graph.AddNode(vertex1, vertex2);
            node = graph.AddNode(vertex1, vertex3);
            graph.AddNode(vertex1, vertex4);
            success = graph.RemoveNode(node);

            Assert.IsTrue(success);
            Assert.IsNull(graph.FindEdge(vertex1,vertex3));
            Assert.IsNotNull(graph.FindEdge(vertex1,vertex4));
            Assert.AreEqual(2, graph.Size);

            // Remove non-existent node
            node = new VertexNode();
            success = graph.RemoveNode(node);
            Assert.IsFalse(success);

            Assert.AreEqual(2, graph.Size);
            graph.Clear();
        }

        [Test]
        public void FirstVertexNode()
        {
            VertexGraph graph = new VertexGraph(9);

            var node = graph.FirstNode();
            Assert.IsNull(node);

            var addedNode = graph.AddNode(vertex1, vertex2);

            node = graph.FirstNode();
            Assert.IsNotNull(node);
            Assert.AreEqual(addedNode, node.Value);

            graph.Clear();
        }

        [Test]
        public void DestroyEmptyVertexGraph()
        {
            VertexGraph graph = new VertexGraph(9);
            graph.Clear();
        }
        
        


        
        
    }
}
