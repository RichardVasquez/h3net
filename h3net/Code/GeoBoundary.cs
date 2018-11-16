using System.Collections.Generic;

namespace H3Net.Code
{
    public class GeoBoundary
    {
        public int numVerts;
        public List<GeoCoord> verts = new List<GeoCoord>();

        public GeoBoundary()
        {
            for (int i = 0; i < Constants.MAX_CELL_BNDRY_VERTS; i++)
            {
                verts.Add(new GeoCoord());
            }
        }
    }
}