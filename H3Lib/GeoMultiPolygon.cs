using System.Collections.Generic;

namespace H3Lib
{
    /// <summary>
    /// Simplified core of GeoJSON MultiPolygon coordinates definition
    /// </summary>
    public class GeoMultiPolygon
    {
        /// <summary>
        /// Number of elements in the array pointed to by the holes
        /// </summary>
        public int NumPolygons;
        /// <summary>
        ///  interior boundaries (holes) in the polygon
        /// </summary>
        public List<GeoPolygon> Polygons;
    }
}
