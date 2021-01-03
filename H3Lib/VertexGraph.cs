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

        /// <summary>
        /// Initialize a new VertexGraph
        /// </summary>
        public VertexGraph()
        {
            _pool = new HashSet<VertexNode>();
        }

        /// <summary>
        /// Destroy a VertexGraph's sub-objects, freeing their memory. The caller is
        /// responsible for freeing memory allocated to the VertexGraph struct itself.
        /// </summary>
        public void Clear()
        {
            _pool.Clear();
        }

        private static VertexNode InitNode(GeoCoord fromNode, GeoCoord toNode)
        {
            return new VertexNode(toNode, fromNode);
        }

        public VertexNode AddNode(GeoCoord fromNode, GeoCoord toNode)
        {
            var edge = InitNode(fromNode, toNode);
            if (!_pool.Contains(edge))
            {
                _pool.Add(edge);
            }

            return edge;
        }
        
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

        public VertexNode? FindEdge(GeoCoord fromNode, GeoCoord toNode)
        {
            int lookFor = _pool.Select(p => p.From == fromNode && p.To == toNode).Count();
            if (lookFor == 0)
            {
                return null;
            }

            return _pool.First(p => p.From == fromNode && p.To == toNode);
        }
    }
}