using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;

namespace h3net.API
{
    /*
     * Copyright 2018, Richard Vasquez
     *
     * Licensed under the Apache License, Version 2.0 (the "License");
     * you may not use this file except in compliance with the License.
     * You may obtain a copy of the License at
     *
     *         http://www.apache.org/licenses/LICENSE-2.0
     *
     * Unless required by applicable law or agreed to in writing, software
     * distributed under the License is distributed on an "AS IS" BASIS,
     * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
     * See the License for the specific language governing permissions and
     * limitations under the License.
     *
     * Original version written in C, Copyright 2016-2017 Uber Technologies, Inc.
     * C version licensed under the Apache License, Version 2.0 (the "License");
     * C Source code available at: https://github.com/uber/h3
     *
     */
    /** @file VertexGraph.cs
     * @brief   Data structure for storing a graph of vertices
     */
    public class VertexGraph
    {
        public class VertexNode
        {
            public GeoCoord from;
            public GeoCoord to;
            public  VertexNode next;
        }

        public List<List<VertexNode>> buckets;
        public int numBuckets;
        public int size;
        public int res;

        /**
         * Initialize a new VertexGraph
         * @param graph       Graph to initialize
         * @param  numBuckets Number of buckets to include in the graph
         * @param  res        Resolution of the hexagons whose vertices we're storing
         */
        public VertexGraph(int num, int re)
        {
            //  We're gonna do it a little different here in .NET land,
            //  using a List<> of List<> since we get enumerators, counts,
            //  etc, for "free".  I really have no interest in chasing down
            //  double pointers.
            numBuckets = num;
            buckets = new List<List<VertexNode>>(num);
            for (var i = 0; i < buckets.Count; i++)
            {
                buckets[i] = new List<VertexNode>();
            }
            size = 0;
            res = re;
        }

        /**
         * Destroy a VertexGraph's sub-objects, freeing their memory. The caller is
         * responsible for freeing memory allocated to the VertexGraph struct itself.
         * @param graph Graph to destroy
         */
        public  static void destroyVertexGraph(ref VertexGraph graph)
        {
            foreach (var bucket in graph.buckets)
            {
                bucket.Clear();
            }
            graph.buckets.Clear();
        }

        /**
         * Get an integer hash for a lat/lon point, at a precision determined
         * by the current hexagon resolution.
         * TODO: Light testing suggests this might not be sufficient at resolutions
         * finer than 10. Design a better hash function if performance and collisions
         * seem to be an issue here.
         * @param  vertex     Lat/lon vertex to hash
         * @param  res        Resolution of the hexagon the vertex belongs to
         * @param  numBuckets Number of buckets in the graph
         * @return            Integer hash
         */
        private static int _hashVertex(GeoCoord vertex, int res, int numBuckets)
        {
            // Simple hash: Take the sum of the lat and lon with a precision level
            // determined by the resolution, converted to int, modulo bucket count.
            return (int) 
                (
                    Math.Abs(
                        (vertex.lat + vertex.lon) * Math.Pow(10, 15 - res)
                    )
                    %  numBuckets
                );
        }

        private static VertexNode _initVertexNode(GeoCoord fromVtx, GeoCoord toVtx)
        {
            var node = new VertexNode {from = fromVtx, to = toVtx, next = null};
            return node;
        }

        /**
         * Add a edge to the graph
         * @param graph   Graph to add node to
         * @param fromVtx Start vertex
         * @param toVtx   End vertex
         * @return        Pointer to the new node
         */
        public static VertexNode addVertexNode(ref VertexGraph graph, GeoCoord fromVtx,
        GeoCoord toVtx)
        {
            // Make the new node
            VertexNode node = _initVertexNode(fromVtx, toVtx);
            // Determine location
            var index = _hashVertex(fromVtx, graph.res, graph.numBuckets);
            // Check whether there's an existing node in that spot
            List<VertexNode> currentNode = graph.buckets[index];
            if (currentNode.Count == 0) {
                // Set bucket to the new node
                graph.buckets[index].Add(node);
            } else {
                //  Go through the list to make sure the
                //  edge doesn't vertexnode doesn't already exist
                //
                //  NOTE: Later, use a Hashset
                foreach (var vertexNode in graph.buckets[index])
                {
                    if (GeoCoord.geoAlmostEqual(vertexNode.from, fromVtx) &&
                        GeoCoord.geoAlmostEqual(vertexNode.to, toVtx))
                    {
                        //  already exists, bail.
                        return node;
                    }
                }
                // Add the new node to the end of the list
                graph.buckets[index].Add(node);
                graph.size++;
            }
            return node;
        }

        /**
         * Remove a node from the graph. The input node will be freed, and should
         * not be used after removal.
         * @param graph Graph to mutate
         * @param node  Node to remove
         * @return      0 on success, 1 on failure (node not found)
         */
        public static int removeVertexNode(ref VertexGraph graph, ref VertexNode node)
        {
            // Determine location
            int index = _hashVertex(node.from, graph.res, graph.numBuckets);
            var currentBucket = graph.buckets[index];

            var tnode = node;
            var nodeIndex = currentBucket.FindIndex(t => t.from == tnode.from && t.to == tnode.to);
            // Failed to find the node
            if (nodeIndex < 0)
            {
                return 1;
            }
            currentBucket.RemoveAt(nodeIndex);
            return 0;
        }

        /**
         * Find the Vertex node for a given edge, if it exists
         * @param  graph   Graph to look in
         * @param  fromVtx Start vertex
         * @param  toVtx   End vertex, or NULL if we don't care
         * @return         Pointer to the vertex node, if found
         */
        public static VertexNode findNodeForEdge(
            ref VertexGraph graph,
            GeoCoord fromVtx,
            GeoCoord toVtx)
        {
            int index = _hashVertex(fromVtx, graph.res, graph.numBuckets);
            var currentBucket = graph.buckets[index];

            var nodeIndex = currentBucket.FindIndex(
                t => GeoCoord.geoAlmostEqual(t.from, fromVtx) &&
                     (toVtx == null || GeoCoord.geoAlmostEqual(t.to, toVtx))
            );

            return nodeIndex < 0
                ? null
                : currentBucket[nodeIndex];
        }

        /**
         * Find a Vertex node starting at the given vertex
         * @param  graph   Graph to look in
         * @param  fromVtx Start vertex
         * @return         Pointer to the vertex node, if found
         */
        public static VertexNode findNodeForVertex(
            ref VertexGraph graph,
            ref GeoCoord fromVtx)
        {
            return findNodeForEdge(ref graph, fromVtx, null);
        }

        /**
         * Get the next vertex node in the graph.
         * @param  graph Graph to iterate
         * @return       Vertex node, or NULL if at the end
         */
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


