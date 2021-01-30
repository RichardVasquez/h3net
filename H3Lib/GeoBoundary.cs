using System.Collections.Generic;
using System.Text;

namespace H3Lib
{
    /// <summary>
    /// cell boundary in latitude/longitude
    /// </summary>
    public class GeoBoundary
    {
        /// <summary>
        /// number of vertices
        /// </summary>
        public int NumVerts;
        
        /// <summary>
        /// vertices in ccw order
        /// </summary>
        public readonly List<GeoCoord> Verts = new List<GeoCoord>();

        /// <summary>
        /// Constructor
        /// </summary>
        public GeoBoundary()
        {
            for (var i = 0; i < Constants.H3.MAX_CELL_BNDRY_VERTS; i++)
            {
                Verts.Add(new GeoCoord());
            }
        }

        /// <summary>
        /// Debug information in string form
        /// </summary>
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
