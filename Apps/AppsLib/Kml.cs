using System.Collections.Generic;
using System.Linq;
using System.Text;
using H3Lib;
using H3Lib.Extensions;

namespace AppsLib
{
    public static class Kml
    {
        public static string PtsHeader(string name, string desc)
        {
            var sb = new StringBuilder();
            sb
               .AppendLine(@"<?xml version=""1.0"" encoding=""UTF-8""?>")
               .Append(@"<kml xmlns=""http://www.opengis.net/kml/2.2"" ")
               .Append(@"xmlns:gx=""http://www.google.com/kml/ext/2.2"" ")
               .Append(@"xmlns:kml=""http://www.opengis.net/kml/2.2"" ")
               .AppendLine(@"xmlns:atom=""http://www.w3.org/2005/Atom"">")
               .AppendLine("<Document>")
               .AppendLine($"        <name>{name}</name>")
               .AppendLine($"        <description>{desc}</description>")
               .AppendLine(@"        <Style id=""s_circle_hl"">")
               .AppendLine("                <IconStyle>")
               .AppendLine("                        <scale>1.3</scale>")
               .AppendLine("                        <Icon>")
               .AppendLine("                                <href>http://maps.google.com/mapfiles/kml/shapes/placemark_circle.png</href>")
               .AppendLine("                        </Icon>")
               .AppendLine(@"                        <hotSpot x=""20"" y=""2"" xunits=""pixels"" yunits=""pixels""/>")
               .AppendLine("                </IconStyle>")
               .AppendLine("                <LabelStyle>")
               .AppendLine("                        <color>ff0000ff</color>")
               .AppendLine("                        <scale>2</scale>")
               .AppendLine("                </LabelStyle>")
               .AppendLine("        </Style>")
               .AppendLine(@"        <StyleMap id=""m_ylw-pushpin"">")
               .AppendLine("                <Pair>")
               .AppendLine("                        <key>normal</key>")
               .AppendLine("                        <styleUrl>#s_circle</styleUrl>")
               .AppendLine("                </Pair>")
               .AppendLine("                <Pair>")
               .AppendLine("                        <key>highlight</key>")
               .AppendLine("                        <styleUrl>#s_circle_hl</styleUrl>")
               .AppendLine("                </Pair>")
               .AppendLine("        </StyleMap>")
               .AppendLine(@"        <Style id=""s_circle"">")
               .AppendLine("                <IconStyle>")
               .AppendLine("                        <scale>1.1</scale>")
               .AppendLine("                        <Icon>")
               .AppendLine("                                <href>http://maps.google.com/mapfiles/kml/shapes/placemark_circle.png</href>")
               .AppendLine("                        </Icon>")
               .AppendLine(@"                        <hotSpot x=""20"" y=""2"" xunits=""pixels"" yunits=""pixels""/>")
               .AppendLine("                </IconStyle>")
               .AppendLine("                <LabelStyle>")
               .AppendLine("                        <color>ff000fff</color>")
               .AppendLine("                        <scale>2</scale>")
               .AppendLine("                </LabelStyle>")
               .AppendLine("        </Style>");

            return sb.ToString();
        }

        public static string BoundaryHeader(string name, string desc)
        {
            var sb = new StringBuilder();
            sb
               .AppendLine(@"<?xml version=""1.0"" encoding=""UTF-8""?>")
               .AppendLine(@"<kml xmlns=""http://earth.google.com/kml/2.1"">")
               .AppendLine("<Folder>")
               .AppendLine($"   <name>{name}</name>")
               .AppendLine($"   <description>{desc}</description>")
               .AppendLine(@"   <Style id=""lineStyle1"">")
               .AppendLine(@"      <LineStyle id=""lineStyle2"">")
               .AppendLine("         <color>ff000fff</color>")
               .AppendLine("         <width>2</width>")
               .AppendLine("      </LineStyle>")
               .AppendLine("   </Style>");

            return sb.ToString();
        }

        public static string PtsFooter()
        {
            var sb = new StringBuilder();
            sb.AppendLine("</Document>")
              .AppendLine("</kml>");

            return sb.ToString();
        }

        public static string BoundaryFooter()
        {
            var sb = new StringBuilder();
            sb.AppendLine("</Folder>")
              .AppendLine("</kml>");

            return sb.ToString();
        }
        
        public static string OutputLatLongKml(GeoCoord g)
        {
            var sb = new StringBuilder();
            sb.AppendLine
                ($"            {g.Longitude.RadiansToDegrees(),8:F6},{g.Latitude.RadiansToDegrees(),8:F6},5.0");
            return sb.ToString();
        }

        public static string OutputPointKml(GeoCoord g, string name)
        {
            var sb = new StringBuilder();

            sb.AppendLine("<Placemark>")
              .AppendLine($"   <name>{name}</name>")
              .AppendLine("   <styleUrl>#m_ylw-pushpin</styleUrl>")
              .AppendLine("   <Point>")
              .AppendLine("      <altitudeMode>relativeToGround</altitudeMode>")
              .AppendLine("      <coordinates>")
              .Append(OutputLatLongKml(g))
              .AppendLine("      </coordinates>")
              .AppendLine("   </Point>")
              .AppendLine("</Placemark>");

            return sb.ToString();
        }
        
        public static string OutputTriKML(GeoCoord v1, GeoCoord v2, GeoCoord v3, string name)
        {
            var sb = new StringBuilder();
            sb.AppendLine("<Placemark>")
              .AppendLine($"<name>{name}</name>")
              .AppendLine("      <styleUrl>#lineStyle1</styleUrl>")
              .AppendLine("      <LineString>")
              .AppendLine("         <tessellate>1</tessellate>")
              .AppendLine("         <coordinates>")
              .Append(OutputLatLongKml(v1))
              .Append(OutputLatLongKml(v2))
              .Append(OutputLatLongKml(v3))
              .Append(OutputLatLongKml(v1))
              .AppendLine("         </coordinates>")
              .AppendLine("      </LineString>")
              .AppendLine("</Placemark>");

            return sb.ToString();
        }

        public static string OutputBoundaryKML(GeoBoundary b, string name)
        {
            return OutputPolyKml(b.Verts.Take(b.NumVerts).ToList(), name);
        }
        
        public static string OutputPolyKml(IList<GeoCoord> geoVerts, string name)
        {
            var sb = new StringBuilder();
            
            sb.AppendLine("<Placemark>")
            .AppendLine($"<name>{name}</name>")
              .AppendLine("      <styleUrl>#lineStyle1</styleUrl>")
              .AppendLine("      <LineString>")
              .AppendLine("         <tessellate>1</tessellate>")
              .AppendLine("         <coordinates>");

            foreach (var vert in geoVerts)
            {
                sb.Append(OutputLatLongKml(vert));
            }
            sb.Append(OutputLatLongKml(geoVerts[0]));

            sb.AppendLine("         </coordinates>")
              .AppendLine("      </LineString>")
              .AppendLine("</Placemark>");

            return sb.ToString();
        }
        



    }
}
