using System.Collections.Generic;

namespace H3Lib
{
    /// <summary>
    /// Simplified core of GeoJSON MultiPolygon coordinates definition
    /// </summary>
    public class GeoMultiPolygon
    {
        public int NumPolygons;
        public List<GeoPolygon> Polygons;
    }
}
