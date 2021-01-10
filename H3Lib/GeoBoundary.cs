using System.Collections.Generic;

namespace H3Lib
{
    public class GeoBoundary
    {
        public int NumVerts;
        public readonly List<GeoCoord> Verts = new List<GeoCoord>();

        public GeoBoundary()
        {
            for (var i = 0; i < Constants.MAX_CELL_BNDRY_VERTS; i++)
            {
                Verts.Add(new GeoCoord());
            }
        }

    }
}
