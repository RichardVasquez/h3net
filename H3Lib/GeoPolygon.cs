using System.Collections.Generic;

namespace H3Lib
{
    /// <summary>
    /// Simplified core of GeoJSON Polygon coordinates definition
    /// </summary>
    public class GeoPolygon
    {
        /// <summary>
        /// exterior boundary of the polygon
        /// </summary>
        public GeoFence GeoFence;
        /// <summary>
        /// Number of elements in the array pointed to by the holes
        /// </summary>
        public int NumHoles;
        /// <summary>
        ///  interior boundaries (holes) in the polygon
        /// </summary>
        public List<GeoFence> Holes;
    }

}
