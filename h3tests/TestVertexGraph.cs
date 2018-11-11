using h3net.API;
using NUnit.Framework;
using NUnit.Framework.Internal.Commands;

namespace h3tests
{
    [TestFixture]
    public class TestVertexGraph
    {
        static GeoCoord center = new GeoCoord();
        static GeoCoord vertex1 = new GeoCoord();
        static GeoCoord vertex2 = new GeoCoord();
        static GeoCoord vertex3 = new GeoCoord();
        static GeoCoord vertex4 = new GeoCoord();
        static GeoCoord vertex5 = new GeoCoord();
        static GeoCoord vertex6 = new GeoCoord();

        
        public void resetGeoCoords()
        {
            GeoCoord.setGeoDegs(ref center, 37.77362016769341, -122.41673772517154);
            GeoCoord.setGeoDegs(ref vertex1, 87.372002166, 166.160981117);
            GeoCoord.setGeoDegs(ref vertex2, 87.370101364, 166.160184306);
            GeoCoord.setGeoDegs(ref vertex3, 87.369088356, 166.196239997);
            GeoCoord.setGeoDegs(ref vertex4, 87.369975080, 166.233115768);
            GeoCoord.setGeoDegs(ref vertex5, 0, 0);
            GeoCoord.setGeoDegs(ref vertex6, -10, -10);
        }

        [Test]
        public void makeVertexGraph()
        {
            VertexGraph graph = new VertexGraph(10, 9);
            Assert.True(graph.numBuckets == 10, "numBuckets set");
            Assert.True(graph.size == 0, "size set");
        }

        [Test]
        public void vertexHash()
        {
            GeoBoundary outline= new GeoBoundary();
            uint hash1;
            uint hash2;
            int numBuckets = 1000;

            for (int res = 0; res < 11; res++) {
                var centerIndex = H3Index.geoToH3(ref center, res);
                H3Index.h3ToGeoBoundary(centerIndex, ref outline);
                for (int i = 0; i < outline.numVerts; i++)
                {
                    hash1 = VertexGraph._hashVertex(outline.verts[i], res, numBuckets);
                    hash2 = VertexGraph._hashVertex(outline.verts[(i + 1) % outline.numVerts],
                                                           res, numBuckets);
                    Assert.True(hash1 != hash2, 
                                $"Hashes must not be equal res:{res}\ni:  {i}\nh1: {hash1}\nh2: {hash2}");
                }
            }
        }

        [Test]
        public void vertexHashNegative()
        {
            int numBuckets = 10;
            Assert.True
                (
                 VertexGraph._hashVertex(vertex5, 5, numBuckets) < numBuckets,
                 "zero vertex hashes correctly"
                );
            Assert.True
                (
                 VertexGraph._hashVertex(vertex6, 5, numBuckets) < numBuckets,
                 "negative coordinates vertex hashes correctly"
                );
        }

        [Test]
        public void addVertexNode()
        {
            resetGeoCoords();
            VertexGraph graph = new VertexGraph(10,9);
            VertexGraph.VertexNode node = new VertexGraph.VertexNode();
            VertexGraph.VertexNode addedNode = new VertexGraph.VertexNode();

            // Basic add
            addedNode = VertexGraph.addVertexNode(ref graph, vertex1, vertex2);
            node = VertexGraph.findNodeForEdge(ref graph, vertex1, vertex2);
            Assert.NotNull(node, "Node found");
            Assert.True(node == addedNode, "Right node found v1-v2");
            Assert.True(graph.size == 1, "Graph size incremented");

            // Collision add
            addedNode = VertexGraph.addVertexNode(ref graph, vertex1, vertex3);
            node = VertexGraph.findNodeForEdge(ref graph, vertex1, vertex3);
            Assert.NotNull(node , "Node found after hash collision");
            Assert.True(node == addedNode, "Right node found v1-v3");
            Assert.True(graph.size == 2, "Graph size incremented");

            // Collision add #2
            addedNode = VertexGraph.addVertexNode(ref graph, vertex1, vertex4);
            node = VertexGraph.findNodeForEdge(ref graph, vertex1, vertex4);
            Assert.NotNull( node , "Node found after 2nd hash collision");
            Assert.True(node == addedNode, "Right node found v1-v4");
            Assert.True(graph.size == 3, "Graph size incremented");

            // Exact match no-op
            node = VertexGraph.findNodeForEdge(ref graph, vertex1, vertex2);
            addedNode = VertexGraph.addVertexNode(ref graph, vertex1, vertex2);
            Assert.True(node == VertexGraph.findNodeForEdge(ref graph, vertex1, vertex2),
                     "Exact match did not change existing node");
            Assert.True(node == addedNode, "Old node returned");
            Assert.True(graph.size == 3, "Graph size was not changed");
        }

        [Test]
        public void addVertexNodeDupe()
        {
            resetGeoCoords();
            VertexGraph graph = new VertexGraph(10,9);
            VertexGraph.VertexNode node = new VertexGraph.VertexNode();
            VertexGraph.VertexNode addedNode = new VertexGraph.VertexNode();

            // Basic add
            addedNode = VertexGraph.addVertexNode(ref graph, vertex1, vertex2);
            node = VertexGraph.findNodeForEdge(ref graph, vertex1, vertex2);
            Assert.NotNull(node, "Node found");
            Assert.True(node == addedNode, "Right node found");
            Assert.True(graph.size == 1, "Graph size incremented");

            // Dupe add
            addedNode = VertexGraph.addVertexNode(ref graph, vertex1, vertex2);
            Assert.True(node == addedNode, "addVertexNode returned the original node");
            Assert.True(graph.size == 1, "Graph size not incremented");
        }

        [Test]
        public void findNodeForEdge()
        {
            resetGeoCoords();
            // Basic lookup tested in testAddVertexNode, only test failures here
            VertexGraph graph = new VertexGraph(10, 9);
            VertexGraph.VertexNode node = new VertexGraph.VertexNode();

            // Empty graph
            node = VertexGraph.findNodeForEdge(ref graph, vertex1, vertex2);
            Assert.IsNull(node, "Node lookup failed correctly for empty graph");

            VertexGraph.addVertexNode(ref graph, vertex1, vertex2);

            // Different hash
            node = VertexGraph.findNodeForEdge(ref graph, vertex3, vertex2);
            Assert.IsNull
                (
                 node,
                 "Node lookup failed correctly for different hash"
                );

            // Hash collision
            node = VertexGraph.findNodeForEdge(ref graph, vertex1, vertex3);
            Assert.IsNull
                (
                 node,
                 "Node lookup failed correctly for hash collision"
                );

            VertexGraph.addVertexNode(ref graph, vertex1, vertex4);

            // Hash collision, list iteration
            node = VertexGraph.findNodeForEdge(ref graph, vertex1, vertex3);
            Assert.IsNull
                (
                 node,
                 "Node lookup failed correctly for collision w/iteration"
                );
        }

        [Test]
        public void findNodeForVertex()
        {
            resetGeoCoords();
            VertexGraph graph = new VertexGraph(10,9);
            VertexGraph.VertexNode node = new VertexGraph.VertexNode();

            // Empty graph
            node = VertexGraph.findNodeForVertex(ref graph, ref vertex1);
            Assert.Null(node, "Node lookup failed correctly for empty graph");

            VertexGraph.addVertexNode(ref graph, vertex1, vertex2);

            node = VertexGraph.findNodeForVertex(ref graph, ref vertex1);
            Assert.NotNull(node, "Node lookup succeeded for correct node");

            node = VertexGraph.findNodeForVertex(ref graph, ref vertex3);
            Assert.Null(node,
                     "Node lookup failed correctly for different node");
        }

        [Test]
        public void removeVertexNode()
        {
            resetGeoCoords();
            VertexGraph graph = new VertexGraph(10,9);
            VertexGraph.VertexNode node = new VertexGraph.VertexNode();
            int success;

            // Straight removal
            node = VertexGraph.addVertexNode(ref graph, vertex1, vertex2);
            success = VertexGraph.removeVertexNode(ref graph, ref node) == 0
                          ? 1
                          : 0;

            Assert.True(success!=0, "Removal successful");
            Assert.True
                (
                 VertexGraph.findNodeForVertex(ref graph, ref vertex1) == null,
                 "Node lookup cannot find node"
                );
            Assert.True(graph.size == 0, "Graph size decremented");

            // Remove end of list
            VertexGraph.addVertexNode(ref graph, vertex1, vertex2);
            node = VertexGraph.addVertexNode(ref graph, vertex1, vertex3);
            success = VertexGraph.removeVertexNode(ref graph, ref node) == 0
                          ? 1
                          : 0;

            Assert.True(success!=0, "Removal successful");
            Assert.True
                (
                 VertexGraph.findNodeForEdge(ref graph, vertex1, vertex3) == null,
                 "Node lookup cannot find node"
                );
            Assert.True
                (
                 VertexGraph.findNodeForEdge(ref graph, vertex1, vertex2).next == null,
                 "Base bucket node not pointing to node"
                );
            Assert.True(graph.size == 1, "Graph size decremented");

            // This removal is just cleanup
            node = VertexGraph.findNodeForVertex(ref graph, ref vertex1);
            Assert.True(VertexGraph.removeVertexNode(ref graph, ref node) == 0);

            // Remove beginning of list
            node = VertexGraph.addVertexNode(ref graph, vertex1, vertex2);
            VertexGraph.addVertexNode(ref graph, vertex1, vertex3);
            success = VertexGraph.removeVertexNode(ref graph, ref node) == 0
                          ? 1
                          : 0;

            Assert.True(success !=0, "Removal successful");
            Assert.True
                (
                 VertexGraph.findNodeForEdge(ref graph, vertex1, vertex2) == null,
                 "Node lookup cannot find node"
                );
            Assert.True
                (
                 VertexGraph.findNodeForEdge(ref graph, vertex1, vertex3) != null,
                 "Node lookup can find previous end of list"
                );
            Assert.True
                (
                 VertexGraph.findNodeForEdge(ref graph, vertex1, vertex3).next == null,
                 "Base bucket node not pointing to node"
                );
            Assert.True(graph.size == 1, "Graph size decremented");

            // This removal is just cleanup
            node = VertexGraph.findNodeForVertex(ref graph, ref vertex1);
            Assert.True(VertexGraph.removeVertexNode(ref graph, ref node) == 0);

            // Remove middle of list
            VertexGraph.addVertexNode(ref graph, vertex1, vertex2);
            node = VertexGraph.addVertexNode(ref graph, vertex1, vertex3);
            VertexGraph.addVertexNode(ref graph, vertex1, vertex4);
            success = VertexGraph.removeVertexNode(ref graph, ref node) == 0
                          ? 1
                          : 0;

            Assert.True(success!=0, "Removal successful");
            Assert.True
                (
                 VertexGraph.findNodeForEdge(ref graph, vertex1, vertex3) == null,
                 "Node lookup cannot find node"
                );
            Assert.True
                (
                 VertexGraph.findNodeForEdge(ref graph, vertex1, vertex4) != null,
                 "Node lookup can find previous end of list"
                );
            Assert.True(graph.size == 2, "Graph size decremented");

            // Remove non-existent node
            node = new VertexGraph.VertexNode();
            success = VertexGraph.removeVertexNode(ref graph, ref node) == 0
                          ? 1
                          : 0;

            Assert.True(success==0, "Removal of non-existent node fails");
            Assert.True(graph.size == 2, "Graph size unchanged");
        }

        [Test]
        public void firstVertexNode()
        {
            resetGeoCoords();
            VertexGraph graph = new VertexGraph(10, 9);
            VertexGraph.VertexNode node = new VertexGraph.VertexNode();
            VertexGraph.VertexNode addedNode = new VertexGraph.VertexNode();

            node = VertexGraph.firstVertexNode(ref graph);
            Assert.Null(node, "No node found for empty graph");

            addedNode = VertexGraph.addVertexNode(ref graph, vertex1, vertex2);

            node = VertexGraph.firstVertexNode(ref graph);
            Assert.True(node == addedNode, "Node found");
        }

        [Test]
        public void destroyEmptyVertexGraph()
        {
            VertexGraph graph = new VertexGraph(10,9);
            VertexGraph.destroyVertexGraph(ref graph);
        }

        public void singleBucketVertexGraph() 
        {
            VertexGraph graph = new VertexGraph(1,9);
            VertexGraph.VertexNode node = new VertexGraph.VertexNode();

            Assert.True(graph.numBuckets == 1, "1 bucket created");

            node = VertexGraph.firstVertexNode(ref graph);
            Assert.Null(node, "No node found for empty graph");

            node = VertexGraph.addVertexNode(ref graph, vertex1, vertex2);
            Assert.NotNull(node, "Node added");
            Assert.True(VertexGraph.firstVertexNode(ref graph) == node, "First node is node");

            VertexGraph.addVertexNode(ref graph, vertex2, vertex3);
            VertexGraph.addVertexNode(ref graph, vertex3, vertex4);
            Assert.True(VertexGraph.firstVertexNode(ref graph) == node, "First node is still node");
            Assert.True(graph.size == 3, "Graph size updated");

            VertexGraph.destroyVertexGraph(ref graph);
        }
    }
}
/*


    TEST(singleBucketVertexGraph) {
        VertexGraph graph;
        initVertexGraph(&graph, 1, 9);
        VertexNode* node;

        t_assert(graph.numBuckets == 1, "1 bucket created");

        node = firstVertexNode(&graph);
        t_assert(node == NULL, "No node found for empty graph");

        node = addVertexNode(&graph, &vertex1, &vertex2);
        t_assert(node != NULL, "Node added");
        t_assert(firstVertexNode(&graph) == node, "First node is node");

        addVertexNode(&graph, &vertex2, &vertex3);
        addVertexNode(&graph, &vertex3, &vertex4);
        t_assert(firstVertexNode(&graph) == node, "First node is still node");
        t_assert(graph.size == 3, "Graph size updated");

        destroyVertexGraph(&graph);
    }






    */