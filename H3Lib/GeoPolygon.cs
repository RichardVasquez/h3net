using System.Collections.Generic;

namespace H3Lib
{
    /// <summary>
    /// Simplified core of GeoJSON Polygon coordinates definition
    /// </summary>
    public class GeoPolygon
    {
        public GeoFence GeoFence;
        public int NumHoles;
        public List<GeoFence> Holes;
    }

}
