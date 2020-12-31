using System;
using System.Collections.Generic;

namespace H3Lib
{
    /// <summary>
    /// Data structure for storing a graph of vertices
    /// </summary>
    /// <!-- Based off 3.1.1 -->
    public class VertexGraph
    {
        public class VertexNode:IEquatable<VertexNode>
        {
            public GeoCoord from;
            public GeoCoord to;
            public  VertexNode next;

            public bool Equals(VertexNode other)
            {
                if (ReferenceEquals(null, other))
                    return false;
                if (ReferenceEquals(this, other))
                    return true;
                return Equals(@from, other.@from) && Equals(to, other.to) && Equals(next, other.next);
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj))
                    return false;
                if (ReferenceEquals(this, obj))
                    return true;
                if (obj.GetType() != this.GetType())
                    return false;
                return Equals((VertexNode) obj);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    var hashCode = (@from != null
                                        ? @from.GetHashCode()
                                        : 0);
                    hashCode = (hashCode * 433) ^ (to != null
                                                       ? to.GetHashCode()
                                                       : 0);
                    hashCode = (hashCode * 263) ^ (next != null
                                                       ? next.GetHashCode()
                                                       : 0);
                    return hashCode;
                }
            }

            public static bool operator ==(VertexNode left, VertexNode right)
            {
                return Equals(left, right);
            }

            public static bool operator !=(VertexNode left, VertexNode right)
            {
                return !Equals(left, right);
            }
        }

        public List<List<VertexNode>> buckets;
        public int numBuckets;
        public int size;
        public int res;

        /// <summary>
        /// Initialize a new VertexGraph
        /// </summary>
        /// <param name="graph">Graph to initialize</param>
        /// <param name="numBuckets">Number of buckets to include in the graph</param>
        /// <param name="res">Resolution of the hexagons whose vertices we're storing</param>
        /// <!-- Based off 3.1.1 -->
        public VertexGraph(int num, int res)
        {
            //  We're gonna do it a little different here in .NET land,
            //  using a List<> of List<> since we get enumerators, counts,
            //  etc, for "free".  I really have no interest in chasing down
            //  double pointers.
            numBuckets = num;
            buckets = new List<List<VertexNode>>(num);
            for (var i = 0; i < numBuckets; i++)
            {
                buckets.Add(new List<VertexNode>());
            }
            size = 0;
            this.res = res;
        }

        /// <summary>
        /// Destroy a VertexGraph's sub-objects, freeing their memory. The caller is
        /// responsible for freeing memory allocated to the VertexGraph struct itself.
        /// </summary>
        /// <param name="graph">Graph to destroy</param>
        /// <!-- Based off 3.1.1 -->
        public  static void destroyVertexGraph(ref VertexGraph graph)
        {
            foreach (var bucket in graph.buckets)
            {
                bucket.Clear();
            }
            graph.buckets.Clear();
        }

        /// <summary>
        /// Get an integer hash for a lat/lon point, at a precision determined
        /// by the current hexagon resolution.
        /// </summary>
        /// <remarks>
        /// Light testing suggests this might not be sufficient at resolutions
        /// finer than 10. Design a better hash function if performance and
        /// collisions seem to be an issue here. (Modified in code below)
        /// </remarks>
        /// <param name="vertex">Lat/lon vertex to hash</param>
        /// <param name="res">Resolution of the hexagon the vertex belongs to</param>
        /// <param name="numBuckets">Number of buckets in the graph</param>
        /// <returns>Integer hash</returns>
        /// <!-- Based off 3.1.1 -->
        public static uint _hashVertex(GeoCoord vertex, int res, int numBuckets)
        {
            // Simple hash: Take the sum of the lat and lon with a precision level
            // determined by the resolution, converted to int, modulo bucket count.
            //  return (uint)
            //    Math.IEEERemainder
            //        (
            //         Math.Abs((vertex.lat + vertex.lon) * Math.Pow(10, 15 - res)),
            //         numBuckets
            //        );
            //
            // I didn't like that one because it caused TestVertexGraph to
            // fail, so I wrote a new one.

            //  Edge cases for stuff close enough to (0,0) to not matter go straight to bucket 0.
            if (vertex == null)
            {
                return 0;
            }
            double start_lat = Math.Abs(vertex.Latitude);
            double start_lon = Math.Abs(vertex.Longitude);
            if (start_lon < Constants.DBL_EPSILON && start_lat < Constants.DBL_EPSILON)
            {
                return 0;
            }

            const int keepShifting = 1000000000;
            double start = 0;
            while (start < keepShifting)
            {
                start_lat *= 9973;
                start_lon *= 911;
                start += start_lat;
                start += start_lon;
            }

            var fraction = Math.IEEERemainder(start, 1);
            return (uint) (Math.Abs(fraction) * numBuckets);
        }

        /// <summary>
        /// Initializer for a node
        /// </summary>
        /// <param name="fromVtx">Vertex to start from</param>
        /// <param name="toVtx">Vertex to end at</param>
        /// <!-- Based off 3.1.1 -->
        private static VertexNode _initVertexNode(GeoCoord fromVtx, GeoCoord toVtx)
        {
            var node = new VertexNode {from = fromVtx, to = toVtx, next = null};
            return node;
        }

        /// <summary>Add an edge to the graph</summary>
        /// <param name="graph">Graph to add node to</param>
        /// <param name="fromVtx">Start vertex</param>
        /// <param name="toVtx">End vertex</param>
        /// <returns>new node</returns>
        /// <!-- Based off 3.1.1 -->
        public static VertexNode addVertexNode(ref VertexGraph graph, GeoCoord fromVtx,
        GeoCoord toVtx)
        {
            // Make the new node
            VertexNode node = _initVertexNode(fromVtx, toVtx);
            // Determine location
            var index = _hashVertex(fromVtx, graph.res, graph.numBuckets);
            // Check whether there's an existing node in that spot
            List<VertexNode> currentNode = graph.buckets[(int)index];
            if (currentNode.Count == 0) {
                // Set bucket to the new node
                graph.buckets[(int)index].Add(node);
            } else {
                //  Go through the list to make sure the
                //  edge doesn't already exist
                //
                //  NOTE: Later, use a Hashset
                foreach (var vertexNode in graph.buckets[(int)index])
                {
                    if (GeoCoord.geoAlmostEqual(vertexNode.from, fromVtx) &&
                        GeoCoord.geoAlmostEqual(vertexNode.to, toVtx))
                    {
                        //  already exists, bail.
                        return vertexNode;
                    }
                }
                // Add the new node to the end of the list
                graph.buckets[(int)index].Add(node);
            }
            graph.size++;
            return node;
        }

        /// <summary>
        /// Remove a node from the graph.  The input node will be freed, and should
        /// not be used after removal.
        /// </summary>
        /// <param name="graph">Graph to mutate</param>
        /// <param name="node">Node to remove</param>
        /// <returns>0 on success, 1 on failure (node not found)</returns>
        /// <!-- Based off 3.1.1 -->
        public static int removeVertexNode(ref VertexGraph graph, ref VertexNode node)
        {
            // Determine location
            uint index = _hashVertex(node.from, graph.res, graph.numBuckets);
            var currentBucket = graph.buckets[(int)index];

            var tnode = node;
            var nodeIndex = currentBucket.FindIndex(t => t.from == tnode.from && t.to == tnode.to);
            // Failed to find the node
            if (nodeIndex < 0)
            {
                return 1;
            }
            currentBucket.RemoveAt(nodeIndex);
            graph.size--;
            return 0;
        }

        /// <summary>
        /// Find the <see cref="VertexNode"/> for a given edge, if it exists.
        /// </summary>
        /// <param name="graph">Graph to look in</param>
        /// <param name="fromVtx">Start Vertex</param>
        /// <param name="ToVtx">End Vertex, or null if we don't care</param>
        /// <returns>Pointer to the vertex node, if found</returns>
        /// <!-- Based off 3.1.1 -->
        public static VertexNode findNodeForEdge(
            ref VertexGraph graph,
            GeoCoord fromVtx,
            GeoCoord toVtx)
        {
            uint index = _hashVertex(fromVtx, graph.res, graph.numBuckets);
            var currentBucket = graph.buckets[(int)index];

            var nodeIndex = currentBucket.FindIndex(
                t => GeoCoord.geoAlmostEqual(t.from, fromVtx) &&
                     (toVtx == null || GeoCoord.geoAlmostEqual(t.to, toVtx))
            );

            return nodeIndex < 0
                ? null
                : currentBucket[nodeIndex];
        }

        /// <summary>
        /// Find a Vertex node starting at the given vertex
        /// </summary>
        /// <param name="graph">Graph to look in</param>
        /// <param name="fromVtx">Start vertex</param>
        /// <returns>Vertex node, if found</returns>
        /// <!-- Based off 3.1.1 -->
        public static VertexNode findNodeForVertex(
            ref VertexGraph graph,
            ref GeoCoord fromVtx)
        {
            return findNodeForEdge(ref graph, fromVtx, null);
        }

        /// <summary>
        /// Get the next vertex node in the graph.
        /// </summary>
        /// <param name="graph">Graph to iterate</param>
        /// <returns>Vertex node or null if at the the end</returns>
        /// <!-- Based off 3.1.1 -->
        public static VertexNode firstVertexNode(ref VertexGraph graph)
        {
            foreach (var bucket in graph.buckets)
            {
                if (bucket.Count <= 0)
                {
                    continue;
                }
                return bucket[0];
            }

            return null;
        }
    }
}


