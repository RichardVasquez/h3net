using System.Text;
using H3Lib;
using H3Lib.Extensions;

namespace AppsLib
{
    public static class Utility
    {
        public static string GeoToStringDegsNoFmt(GeoCoord gc)
        {
            var sb = new StringBuilder();
            //,9:66
            sb.Append($"({gc.Latitude.RadiansToDegrees(),9:F6},{gc.Longitude.RadiansToDegrees(),9:F6})");
            return sb.ToString();
        }
        
        public static string GeoBoundaryPrintLines(GeoBoundary gb)
        {
            var sb = new StringBuilder();

            sb.AppendLine("{");
            for (int i = 0;i < gb.NumVerts; i++)
            {
                sb.Append("   ");
                sb.AppendLine(GeoToStringDegsNoFmt(gb.Verts[i]));
            }

            sb.AppendLine("}");
            return sb.ToString();

        }
        
    }
}
