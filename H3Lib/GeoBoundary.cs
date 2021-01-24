using System.Collections.Generic;
using System.Text;

namespace H3Lib
{
    public class GeoBoundary
    {
        public int NumVerts;
        public readonly List<GeoCoord> Verts = new List<GeoCoord>();

        public GeoBoundary()
        {
            for (var i = 0; i < Constants.H3.MAX_CELL_BNDRY_VERTS; i++)
            {
                Verts.Add(new GeoCoord());
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append($"GeoBoundary: {NumVerts} Vertices ");
            for (var i = 0; i < NumVerts; i++)
            {
                sb.Append($"- {i} {Verts[i]}");
            }

            return sb.ToString();
        }
    }
}
