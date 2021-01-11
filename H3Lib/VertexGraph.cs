using System.Collections.Generic;
using System.Linq;

namespace H3Lib
{
    /// <summary>
    /// Data structure for storing a graph of vertices
    /// </summary>
    public class VertexGraph
    {
        private readonly HashSet<VertexNode> _pool;
        public readonly int Resolution;
        
        /// <summary>
        /// Initialize a new VertexGraph
        /// </summary>
        /// <!--
        /// vertexGraph.c
        /// void initVertexGraph
        /// -->
        public VertexGraph()
        {
            _pool = new HashSet<VertexNode>();
            Resolution = 0;
        }

        /// <summary>
        /// Initialize a new VertexGraph
        /// </summary>
        /// <param name="res">Resolution of the hexagons whose vertices we're storing</param>
        /// <!--
        /// vertexGraph.c
        /// void initVertexGraph
        /// -->
        public VertexGraph(int res)
        {
            _pool = new HashSet<VertexNode>();
            Resolution = res;
        }

        /// <summary>
        /// Destroy a VertexGraph's sub-objects, freeing their memory. The caller is
        /// responsible for freeing memory allocated to the VertexGraph struct itself.
        /// </summary>
        /// <!--
        /// vertexGraph.c
        /// void destroyVertexGraph
        /// -->
        public void Clear()
        {
            _pool.Clear();
        }

        /// <summary>
        /// Create a new node based on two GeoCoords
        /// </summary>
        /// <!--
        /// vertexGraph.c
        /// void _initVertexNode
        /// -->
        private static VertexNode InitNode(GeoCoord fromNode, GeoCoord toNode)
        {
            return new VertexNode(toNode, fromNode);
        }

        /// <summary>
        /// Add an edge to the graph
        /// </summary>
        /// <param name="fromNode">Start vertex</param>
        /// <param name="toNode">End vertex</param>
        /// <returns>Reference to the new node</returns>
        /// <!--
        /// vertexGraph.c
        /// VertexNode* addVertexNode
        /// -->
        public VertexNode AddNode(GeoCoord fromNode, GeoCoord toNode)
        {
            var edge = InitNode(fromNode, toNode);
            if (!_pool.Contains(edge))
            {
                _pool.Add(edge);
            }

            return edge;
        }
        
        /// <summary>
        /// Remove a node from the graph. The input node will be freed, and should
        /// not be used after removal.
        /// </summary>
        /// <param name="vn">Node to remove</param>
        /// <returns>true on success, false on faiilure (node not found)</returns>
        /// <!--
        /// vertexGraph.c
        /// int removeVertexNode
        /// -->
        public bool RemoveNode(VertexNode vn)
        {
            int lookFor = _pool.Count(p => p.From == vn.From && p.To == vn.To);
            if (lookFor != 1)
            {
                return false;
            }
            _pool.Remove(vn);
            return true;
        }

        /// <summary>
        /// Find the Vertex node for a given edge, if it exists
        /// </summary>
        /// <param name="fromNode">Start vertex</param>
        /// <param name="toNode">End vertex, or NULL if we don't care</param>
        /// <returns>Reference to the vertex node, if found</returns>
        /// <!--
        /// vertexGraph.c
        /// VertexNode* findNodeForVertex
        /// -->
        public VertexNode? FindEdge(GeoCoord fromNode, GeoCoord? toNode)
        {
            int lookFor = _pool.Select(p => p.From == fromNode && p.To == toNode).Count();
            if (lookFor == 0)
            {
                return null;
            }

            return _pool.First(p => p.From == fromNode && p.To == toNode);
        }

        /// <summary>Find a Vertex node starting at the given vertex</summary>
        /// <param name="vertex">fromVtx Start vertex</param>
        /// <returns>Pointer to the vertex node, if found</returns>
        /// <!--
        /// vertexGraph.c
        /// VertexNode* findNodeForVertex
        /// -->
        public VertexNode? FindVertex(GeoCoord vertex)
        {
            return FindEdge(vertex, null);
        }

        /// <summary>
        /// Picks whatever HashSet says is the first VertexNode
        /// </summary>
        public VertexNode? FirstNode()
        {
            return _pool.First();
        }
    }
}