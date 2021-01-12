namespace H3Lib.Extensions
{
    public static class VertexGraphExtensions
    {
        /// <summary>
        /// Internal: Create a LinkedGeoPolygon from a vertex graph. It is the
        /// responsibility of the caller to call destroyLinkedPolygon on the populated
        /// linked geo structure, or the memory for that structure will not be freed.
        /// </summary>
        /// <param name="graph">Input graph</param>
        /// <returns>Output polygon</returns>
        /// <!--
        /// algos.c
        /// void _vertexGraphToLinkedGeo
        /// -->
        public static LinkedGeoPolygon ToLinkedGeoPolygon(this VertexGraph graph)
        {
            var result = new LinkedGeoPolygon();
            var loop = new LinkedGeoLoop();

            // Find the next unused entry point

            var edge = graph.FirstNode();
            while (edge.HasValue)
            {
                result.LinkedGeoList.AddLast(loop);
                // Walk the graph to get the outline
                do
                {
                    loop.GeoCoordList.AddLast(edge.Value.From);
                    var nextVertex = edge.Value.To;
                    // Remove frees the node, so we can't use edge after this
                    graph.RemoveNode(edge.Value);
                    edge = graph.FindVertex(nextVertex);
                } while (edge.HasValue);
            }

            return result;
        }
    }
}
