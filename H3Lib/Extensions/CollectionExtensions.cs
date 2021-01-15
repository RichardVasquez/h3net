using System.Collections.Generic;
using System.Linq;

namespace H3Lib.Extensions
{
    /// <summary>
    /// Static methods that work on collections.
    /// 
    /// Currently List<T>, but will likely be switched to
    /// IEnumerable<T> in future
    /// </summary>
    public static class CollectionExtensions
    {
       
        /// <summary>
        /// uncompact takes a compressed set of hexagons and expands back to the
        /// original set of hexagons.
        /// </summary>
        /// <param name="compactedSet"> Set of hexagons</param>
        /// <param name="res"> The hexagon resolution to decompress to</param>
        /// <returns>
        /// A status code and the uncompacted hexagons.
        /// </returns>
        /// <!--
        /// h3index.c
        /// int H3_EXPORT(uncompact)
        /// -->
        public static (int, List<H3Index>) Uncompact(this List<H3Index> compactedSet, int res)
        {
            //  Let's deal with the resolution issue first
            if (compactedSet.Any(h => h.Resolution > res))
            {
                return (-2, new List<H3Index>());
            }
            
            if (compactedSet.Any(cs => !cs.Resolution.IsValidChildRes(res)))
            {
                return (-2, compactedSet);
            }

            // setup the grind

            var testPool = new HashSet<H3Index>();

            var validCheck = compactedSet.Where(h => h.IsValid());

            foreach (var index in validCheck)
            {
                if (index.Resolution == res)
                {
                    testPool.Add(index);
                    continue;
                }

                var children = index.ToChildren(res);
                foreach (var child in children)
                {
                    testPool.Add(child);
                }
            }
            return (0, testPool.ToList());
        }

        /// <summary>
        /// maxUncompactSize takes a compacted set of hexagons are provides an
        /// upper-bound estimate of the size of the uncompacted set of hexagons.
        /// </summary>
        /// <param name="compactedSet"> Set of hexagons</param>
        /// <param name="res"> The hexagon resolution to decompress to</param>
        /// <returns>
        /// The number of hexagons to allocate memory for, or a negative
        /// number if an error occurs.
        /// </returns>
        /// <!--
        /// h3Index.c
        /// int H3_EXPORT(maxUncompactSize)
        /// -->
        public static long MaxUncompactSize(this List<H3Index> compactedSet, int res)
        {
            long maxCount = 0;
            foreach (var hex in compactedSet)
            {
                if (hex == 0)
                {
                    continue;
                }

                int currentRes = hex.Resolution;
                if (!currentRes.IsValidChildRes(res))
                {
                    // Nonsensical. Abort.
                    return -1;
                }

                if (currentRes == res)
                {
                    maxCount++;
                }
                else
                {
                    // Bigger hexagon to reduce in size
                    maxCount += hex.MaxChildrenSize(res);
                }
            }

            return maxCount;
        }

        /// <summary>
        /// hexRanges takes an array of input hex IDs and a max k-ring and returns an
        /// array of hexagon IDs sorted first by the original hex IDs and then by the
        /// k-ring (0 to max), with no guaranteed sorting within each k-ring group.
        /// </summary>
        /// <param name="h3Set">a list of H3Indexes</param>
        /// <param name="k">k The number of rings to generate</param>
        /// <returns>
        /// Tuple
        ///     Item1 - 0 if no pentagon is encountered. Cannot trust output otherwise
        ///     Item2 - List of H3Index cells
        /// </returns>
        public static (int, List<H3Index>) HexRanges(this List<H3Index> h3Set, int k)
        {
            var results = new List<H3Index>();
            foreach (var h3Cell in h3Set)
            {
                (int success, List<H3Index> temp) = h3Cell.HexRange(k);
                if (success != 0)
                {
                    return (success, results);
                }
                results.AddRange(temp);
            }

            return (0, results);
        }

        /// <summary>
        /// Internal: Create a vertex graph from a set of hexagons. It is the
        /// responsibility of the caller to call destroyVertexGraph on the populated
        /// graph, otherwise the memory in the graph nodes will not be freed.
        /// </summary>
        /// <param name="h3Set">Set of hexagons</param>
        /// <returns>Output graph</returns>
        public static VertexGraph ToVertexGraph(this List<H3Index> h3Set)
        {
            if (h3Set.Count<1)
            {
                // We still need to init the graph, or calls to destroyVertexGraph will
                // fail
                return new VertexGraph();
            }

            var graph = new VertexGraph();

            // Iterate through every hexagon
            foreach (var vertices in h3Set.Select(cell => cell.ToGeoBoundary()))
            {
                // iterate through every edge
                for (var j = 0; j < vertices.NumVerts; j++)
                {
                    var fromVtx = vertices.Verts[j];
                    var toVtx = vertices.Verts[(j + 1) % vertices.NumVerts];

                    // If we've seen this edge already, it will be reversed
                    var edge = graph.FindEdge(toVtx, fromVtx);
                    if (edge != null)
                    {
                        // If we've seen it, drop it. No edge is shared by more than 2
                        // hexagons, so we'll never see it again.
                        graph.RemoveNode(edge.Value);
                    } else {
                        // Add a new node for this edge
                        graph.AddNode(fromVtx, toVtx);
                    }
                }
            }

            return graph;
        }

        /// <summary>
        /// Create a LinkedGeoPolygon describing the outline(s) of a set of  hexagons.
        /// Polygon outlines will follow GeoJSON MultiPolygon order: Each polygon will
        /// have one outer loop, which is first in the list, followed by any holes.
        /// 
        /// It is the responsibility of the caller to call destroyLinkedPolygon on the
        /// populated linked geo structure, or the memory for that structure will
        /// not be freed.
        /// 
        /// It is expected that all hexagons in the set have the same resolution and
        /// that the set contains no duplicates. Behavior is undefined if duplicates
        /// or multiple resolutions are present, and the algorithm may produce
        /// unexpected or invalid output.
        /// </summary>
        /// <param name="h3Set">Set of Hexagons</param>
        /// <returns>Output polygon</returns>
        /// <!--
        /// algos.c
        /// void H3_EXPORT(h3SetToLinkedGeo)
        /// -->
        public static LinkedGeoPolygon ToLinkedGeoPolygon(this List<H3Index> h3Set)
        {
            var graph = h3Set.ToVertexGraph();
            var temp = graph.ToLinkedGeoPolygon();

            // TODO: The return value, possibly indicating an error, is discarded here -
            // we should use this when we update the API to return a value
            var (_, result) = temp.NormalizeMultiPolygon();
            graph.Clear();
            return result;
        }

        /// <summary>
        /// Given a list of nested containers, find the one most deeply nested.
        /// </summary>
        /// <param name="polygons">Polygon containers to check</param>
        /// <param name="boxes">Bounding boxes for polygons, used in point-in-poly check</param>
        /// <returns>Deepest container, or null if list is empty</returns>
        /// <!--
        /// linkedGeo.c
        /// static const LinkedGeoPolygon* findDeepestContainer
        /// -->
        public static LinkedGeoPolygon FindDeepestContainer(
                this List<LinkedGeoPolygon> polygons, List<BBox> boxes
            )
        {
            // Set the initial return value to the first candidate
            var parent = polygons.Count > 0
                             ? polygons[0]
                             : null;

            // If we have multiple polygons, they must be nested inside each other.
            // Find the innermost polygon by taking the one with the most containers
            // in the list.
            if (polygons.Count <= 1)
            {
                return parent;
            }
            int max = -1;
            foreach (var poly in polygons)
            {
                if (poly.LinkedGeoList.First == null)
                {
                    continue;
                }
                int count = poly.LinkedGeoList.First.Value.CountContainers(polygons, boxes);
                if (count <= max)
                {
                    continue;
                }
                parent = poly;
                max = count;
            }

            return parent;
            
        }

    }
}
