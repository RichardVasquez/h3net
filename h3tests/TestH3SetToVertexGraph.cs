using System.Collections.Generic;
using h3net.API;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace h3tests
{
    [TestFixture]
    public class TestH3SetToVertexGraph
    {
        private List<H3Index> makeSet(List<string> hexes, int numHexes)
        {
            List<H3Index> set = new List<H3Index>();
            for (int i = 0; i < numHexes; i++)
            {
                set.Add(H3Index.stringToH3(hexes[i]));
            }

            return set;
        }

        [Test]
        public void empty()
        {
            VertexGraph graph = new VertexGraph(0,0);
            int numHexes = 0;
            var set = makeSet(null, numHexes);

            Algos.h3SetToVertexGraph(ref set, numHexes, ref graph);

            Assert.True(graph.size == 0, "No edges added to graph");

            VertexGraph.destroyVertexGraph(ref graph);
        }
     
        [Test]
        public void singleHex()
        {
            VertexGraph graph = new VertexGraph(0,0);
            List<string> hexes = new List<string> {"890dab6220bffff"};

            int numHexes = hexes.Count;
            List<H3Index> set = makeSet(hexes, numHexes);

            Algos.h3SetToVertexGraph(ref set, numHexes, ref graph);
            Assert.True(graph.size == 6, "All edges of one hex added to graph");

            VertexGraph.destroyVertexGraph(ref graph);
        }


        [Test]
        public void nonContiguous2()
        {
            VertexGraph graph = new VertexGraph(0,0);
            List<string> hexes = new List<string> {"8928308291bffff", "89283082943ffff"};
            int numHexes = hexes.Count;
            var set = makeSet(hexes, numHexes);

            Algos.h3SetToVertexGraph(ref set, numHexes, ref graph);
            Assert.True(graph.size == 12,
                     "All edges of two non-contiguous hexes added to graph");

            VertexGraph.destroyVertexGraph(ref graph);

        }

        [Test]
        public void contiguous2()
        {
            VertexGraph graph = new VertexGraph(0,0);
            var hexes = new List<string>{"8928308291bffff", "89283082957ffff"};
            int numHexes = hexes.Count;
            var set = makeSet(hexes, numHexes);

            Algos.h3SetToVertexGraph(ref set, numHexes, ref graph);
            Assert.True(graph.size == 10, "All edges except 2 shared added to graph");

            VertexGraph.destroyVertexGraph(ref graph);
        }

        [Test]
        public void contiguous2distorted()
        {
            VertexGraph graph = new VertexGraph(0,0);
            var hexes = new List<string>{"894cc5365afffff", "894cc536537ffff"};
            int numHexes = hexes.Count;
            var set = makeSet(hexes, numHexes);

            Algos.h3SetToVertexGraph(ref set, numHexes, ref graph);
            Assert.True(graph.size == 12, "All edges except 2 shared added to graph");

            VertexGraph.destroyVertexGraph(ref graph);
        }

        [Test]
        public void contiguous3()
        {
            VertexGraph graph = new VertexGraph(0,0);
            var hexes = new List<string>{"8928308288bffff", "892830828d7ffff", "8928308289bffff"};
            int numHexes = hexes.Count;
            var set = makeSet(hexes, numHexes);

            Algos.h3SetToVertexGraph(ref set, numHexes, ref graph);
            Assert.True(graph.size == 3 * 4, "All edges except 6 shared added to graph");

            VertexGraph.destroyVertexGraph(ref graph);
        }

        [Test]
        public void hole()
        {
            VertexGraph graph =new VertexGraph(0,0);
            var hexes = new List<string>{"892830828c7ffff", "892830828d7ffff", "8928308289bffff",
                "89283082813ffff", "8928308288fffff", "89283082883ffff"};
            int numHexes = hexes.Count;
            var set = makeSet(hexes, numHexes);

            Algos.h3SetToVertexGraph(ref set, numHexes, ref graph);
            Assert.True( graph.size == (6 * 3) + 6,
                     "All outer edges and inner hole edges added to graph");

            VertexGraph.destroyVertexGraph(ref graph);
        }

    }
}