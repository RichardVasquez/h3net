using System.Collections.Generic;
using H3Lib;
using H3Lib.Extensions;
using NUnit.Framework;

namespace TestSuite
{
    [TestFixture]
    public class TestH3SetToVertexGraph
    {
        [Test]
        public void Empty()
        {
            var graph = new VertexGraph();
            Assert.AreEqual(0, graph.Count);
            graph.Clear();

        }

        [Test]
        public void SingleHex()
        {
            var set = new List<H3Index> {0x890dab6220bffff};
            VertexGraph graph = set.ToVertexGraph();

            Assert.AreEqual(6, graph.Size);
            graph.Clear();
        }

        [Test]
        public void NonContiguous2()
        {
            var set = new List<H3Index> {0x8928308291bffff, 0x89283082943ffff};
            var graph = set.ToVertexGraph();

            Assert.AreEqual(12, graph.Size);
            graph.Clear();
        }

        [Test]
        public void Contiguous2()
        {
            var set = new List<H3Index> {0x8928308291bffff, 0x89283082957ffff};
            var graph = set.ToVertexGraph();

            Assert.AreEqual(10, graph.Size);
            graph.Clear();
        }

        [Test]
        public void Contiguous2distorted()
        {
            var set = new List<H3Index> {0x894cc5365afffff, 0x894cc536537ffff};
            var graph = set.ToVertexGraph();
            Assert.AreEqual(12, graph.Size);
            graph.Clear();
        }

        [Test]
        public void Contiguous3()
        {
            var set = new List<H3Index>
                      {
                          0x8928308288bffff, 0x892830828d7ffff,
                          0x8928308289bffff
                      };
            var graph = set.ToVertexGraph();

            Assert.AreEqual(12, graph.Size);
            graph.Clear();
        }

        [Test]
        public void Hole()
        {
            var set = new List<H3Index>
                      {
                          0x892830828c7ffff, 0x892830828d7ffff,
                          0x8928308289bffff, 0x89283082813ffff,
                          0x8928308288fffff, 0x89283082883ffff
                      };
            var graph = set.ToVertexGraph();
            Assert.AreEqual((6 * 3) + 6, graph.Size);
            graph.Clear();
        }
    }
}
