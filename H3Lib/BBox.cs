using System;
using System.Collections.Generic;
using System.Linq;

namespace H3Lib
{
    /// <summary>
    /// Geographic bounding box with coordinates defined in radians
    /// </summary>
    /// <!-- Based off 3.1.1 -->
    public class BBox
    {
        public double North;
        public double South;
        public double East;
        public double West;

        /// <summary>
        /// Create a bounding box from a simple polygon defined as an array of vertices.
        ///
        /// Known limitations:
        /// - Does not support polygons with two adjacent points &gt; 180 degrees of
        ///   longitude apart. These will be interpreted as crossing the antimeridian.
        /// - Does not currently support polygons containing a pole.
        /// </summary>
        /// <param name="verts">Array of vertices</param>
        /// <param name="numVerts">Number of vertices</param>
        /// <param name="bbox">Output box</param>
        /// <!-- Based off 3.1.1 -->
        static void BboxFromVertices(List<GeoCoord> verts, int numVerts, ref BBox bbox) 
        {
            // Early exit if there are no vertices
            if (numVerts == 0) {
                bbox.North = 0;
                bbox.South = 0;
                bbox.East = 0;
                bbox.West = 0;
                return;
            }
            double lat;
            double lon;

            bbox.South = double.MaxValue;
            bbox.West = double.MaxValue;
            bbox.North = -double.MaxValue;
            bbox.East = -double.MaxValue;
            bool isTransmeridian = false;

            for (int i = 0; i < numVerts; i++) {
                lat = verts[i].Latitude;
                lon = verts[i].Longitude;
                if (lat < bbox.South) bbox.South = lat;
                if (lon < bbox.West) bbox.West = lon;
                if (lat > bbox.North) bbox.North = lat;
                if (lon > bbox.East) bbox.East = lon;
                // check for arcs > 180 degrees longitude, flagging as transmeridian
                if (Math.Abs( lon - verts[(i + 1) % numVerts].Longitude) > Constants.M_PI)
                {
                    isTransmeridian = true;
                }
            }
            // Swap east and west if transmeridian
            if (isTransmeridian)
            {
                double tmp = bbox.East;
                bbox.East = bbox.West;
                bbox.West = tmp;
            }
        }

        /// <summary>
        /// Create a bounding box from a Geofence
        /// </summary>
        /// <param name="Geofence">Input <see cref="Geofence"/></param>
        /// <param name="bbox">Output bbox</param>
        /// <!-- Based off 3.1.1 -->
        public static void bboxFromGeofence(Geofence Geofence, ref BBox bbox) {
            BboxFromVertices(Geofence.verts.ToList() , Geofence.numVerts, ref bbox);
        }

        /// <summary>
        /// Create a bounding box from a GeoPolygon
        /// </summary>
        /// <param name="polygon">Input <see cref="GeoPolygon"/></param>
        /// <param name="bboxes">Output bboxes, one for the outer loop and one for each hole</param>
        /// <!-- Based off 3.1.1 -->
        void bboxesFromGeoPolygon(GeoPolygon polygon, ref List<BBox> bboxes)
        {
            var bb = bboxes[0];
            bboxFromGeofence(polygon.Geofence, ref bb);
            bboxes[0] = bb;
            for (int i = 0; i < polygon.numHoles; i++)
            {
                bb = bboxes[i + 1];
                bboxFromGeofence(polygon.holes[i], ref bb);
                bboxes[i + 1] = bb;
            }
        }

        /// <summary>
        /// Whether the given bounding box cross the antimeridian
        /// </summary>
        /// <param name="bbox">bounding box to inspect</param>
        /// <returns>true if transmeridian</returns>
        /// <!-- Based off 3.1.1 -->
        public static bool bboxIsTransmeridian(BBox bbox)
        {
            return bbox.East < bbox.West;
        }

        /// <summary>
        /// Gets the center of a bounding box
        /// </summary>
        /// <param name="bbox">Input bounding box</param>
        /// <param name="center">Output center coordinate</param>
        /// <!-- Based off 3.1.1 -->
        public static void bboxCenter(BBox bbox, ref GeoCoord center)
        {
            center.Latitude = (bbox.North + bbox.South) / 2.0;
            // If the bbox crosses the antimeridian, shift east 360 degrees
            double east = bboxIsTransmeridian(bbox)
                ? bbox.East + Constants.M_2PI
                : bbox.East;
            center.Longitude = GeoCoord.ConstrainLongitude((east + bbox.West) / 2.0);
        }

        /// <summary>
        /// Whether the bounding box contains a given point
        /// </summary>
        /// <param name="bbox">Bounding box</param>
        /// <param name="point">Point to test</param>
        /// <returns>true is point is contained</returns>
        /// <!-- Based off 3.1.1 -->
        public static bool bboxContains(BBox bbox, GeoCoord point)
        {
            return 
                point.Latitude >= bbox.South && 
                point.Latitude <= bbox.North &&
                (
                    bboxIsTransmeridian(bbox)
                    // transmeridian case
                    ? point.Longitude >= bbox.West || point.Longitude <= bbox.East
                    // standard case
                    : point.Longitude >= bbox.West && point.Longitude <= bbox.East
                );
        }

        /// <summary>
        /// Determines if two bounding boxes are strictly equal
        /// </summary>
        /// <param name="b1">Bounding box 1</param>
        /// <param name="b2">Bounding box 2</param>
        /// <returns>True if the boxes are equal</returns>
        /// <!-- Based off 3.1.1 -->
        public static bool bboxEquals(BBox b1, BBox b2)
        {
            return Math.Abs(b1.North - b2.North) < Constants.EPSILON &&
                   Math.Abs(b1.South - b2.South) < Constants.EPSILON &&
                   Math.Abs(b1.East - b2.East) < Constants.EPSILON &&
                   Math.Abs(b1.West - b2.West) < Constants.EPSILON;
        }

        /// <summary>
        /// Returns the radius of a given hexagon in kilometers
        /// </summary>
        /// <param name="h3Index">Index of the hexagon</param>
        /// <returns>radius of hexagon in kilometers</returns>
        /// <!-- Based off 3.1.1 -->
        static double _hexRadiusKm(H3Index h3Index)
        {
            // There is probably a cheaper way to determine the radius of a
            // hexagon, but this way is conceptually simple
            GeoCoord h3Center = new GeoCoord();
            GeoBoundary h3Boundary = new GeoBoundary();
            H3Index.h3ToGeo(h3Index, ref h3Center);
            H3Index.h3ToGeoBoundary(h3Index, ref h3Boundary);
            return GeoCoord._geoDistKm(h3Center,   h3Boundary.verts);
        }

        /// <summary>
        /// Radius of bounding box in hexagons.  i.e., the radius of a k-ring centered
        /// on the bbox center and covering the entire bbox.
        /// </summary>
        /// <param name="bbox">Bounding box to measure</param>
        /// <param name="res">Resolution of hexagons to use in measurement</param>
        /// <returns>Radius in hexagons</returns>
        /// <!-- Based off 3.1.1 -->
        public static int bboxHexRadius(BBox bbox, int res)
        {
            // Determine the center of the bounding box
            GeoCoord center = new GeoCoord();
            bboxCenter(bbox, ref center);

            // Use a vertex on the side closest to the equator, to ensure the longest
            // radius in cases with significant distortion. East/west is arbitrary.
            double lat =
                Math.Abs( bbox.North) > Math.Abs(bbox.South)
                    ? bbox.South
                    : bbox.North;

            GeoCoord vertex = new GeoCoord {Latitude = lat, Longitude = bbox.East};

            // Determine the length of the bounding box "radius" to then use
            // as a circle on the earth that the k-rings must be greater than
            double bboxRadiusKm = GeoCoord._geoDistKm(center, new []{vertex});

            // Determine the radius of the center hexagon
            double centerHexRadiusKm = _hexRadiusKm(H3Index.geoToH3(ref center, res));

            // The closest point along a hexagon drawn through the center points
            // of a k-ring aggregation is exactly 1.5 radii of the hexagon. For
            // any orientation of the GeoJSON encased in a circle defined by the
            // bounding box radius and center, it is guaranteed to fit in this k-ring
            // Rounded *up* to guarantee containment
            return (int)Math.Ceiling(bboxRadiusKm / (1.5 * centerHexRadiusKm));
        }
    }
}
