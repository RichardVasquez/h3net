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

            while (graph.Count>0)
            {
                var edge = graph.FirstNode();
                var loop = new LinkedGeoLoop();
                while (edge.HasValue)
                {
                    loop.AddLinkedCoord(edge.Value.From);
                    //loop.GeoCoordList.AddLast(edge.Value.From);
                    var nextVertex = edge.Value.To;
                    graph.RemoveNode(edge.Value);
                    edge = graph.FindVertex(nextVertex);
                }

                // if (loop.CountCoords > 0)
                // {
                //     result.GeoLoopList.AddLast(loop);
                // }
                if (loop.Count > 0)
                {
                    result.AddLinkedLoop(loop);
                }
            }
            
            return result;
        }
    }
}
