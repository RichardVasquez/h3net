using System;
using System.Collections.Generic;

namespace H3Lib
{
    public class Polygon
    {
        /// <summary>
        /// Normalize longitude, dealing with transmeridian arcs
        /// </summary>
        /// <param name="lon"></param>
        /// <param name="isTransmeridian"></param>
        /// <returns>Normalized longitude</returns>
        /// <!-- Based off 3.1.1 -->
        private static double NORMALIZE_LON(double lon, bool isTransmeridian)
        {
            return isTransmeridian && lon < 0
                ? lon + Constants.M_2PI
                : lon;
        }



        /// <summary>
        /// Create a bounding box from a simple polygon loop.
        /// Known limitations:
        /// - Does not support polygons with two adjacent points > 180 degrees of
        ///   longitude apart. These will be interpreted as crossing the antimeridian.
        /// - Does not currently support polygons containing a pole.
        /// </summary>
        /// <param name="loop">Loop of coordinates</param>
        /// <param name="bbox">Output bbox</param>
        /// <!-- Based off 3.1.1 -->
        public static void bboxFromGeofence(ref GeoFence loop, ref BBox bbox)
        {
            // Early exit if there are no vertices
            if (loop.NumVerts == 0) {
                bbox = new BBox();
                return;
            }

            bbox.South = Double.MaxValue;
            bbox.West = Double.MaxValue;
            bbox.North = -Double.MaxValue;
            bbox.East = -Double.MaxValue;
            double minPosLon = Double.MaxValue;
            double maxNegLon = -Double.MaxValue;
            bool isTransmeridian = false;

            double lat;
            double lon;
            GeoCoord coord;
            GeoCoord next;

            int loopIndex = -1;

            while (true) {
                
                if (++loopIndex >= loop.NumVerts)
                {
                    break;
                }

                coord = new GeoCoord(loop.Verts[loopIndex].Latitude, loop.Verts[loopIndex].Longitude);
                next = new GeoCoord
                    (
                     loop.Verts[(loopIndex + 1) % loop.NumVerts].Latitude,
                     loop.Verts[(loopIndex + 1) % loop.NumVerts].Longitude
                    );



                lat = coord.Latitude;
                lon = coord.Longitude;
                if (lat < bbox.South) {bbox.South = lat;}
                if (lon < bbox.West) {bbox.West = lon;}
                if (lat > bbox.North) {bbox.North = lat;}
                if (lon > bbox.East) {bbox.East = lon;}
                // Save the min positive and max negative longitude for
                // use in the transmeridian case
                if (lon > 0 && lon < minPosLon) minPosLon = lon;
                if (lon < 0 && lon > maxNegLon) maxNegLon = lon;
                // check for arcs > 180 degrees longitude, flagging as transmeridian
                if (Math.Abs( lon - next.Longitude) > Constants .M_PI) {
                    isTransmeridian = true;
                }
            }
            // Swap east and west if transmeridian
            if (isTransmeridian) {
                bbox.East = maxNegLon;
                bbox.West = minPosLon;
            }
        }

        /// <summary>
        /// pointInside is the core loop of the point-in-poly algorithm
        /// </summary>
        /// <param name="loop">The loop to check</param>
        /// <param name="bbox">The bbox for the loop being tested</param>
        /// <param name="coord">The coordinate to check</param>
        /// <returns>Whether the point is contained</returns>
        /// <!-- Based off 3.1.1 -->
        public static bool pointInsideGeofence(ref GeoFence loop, ref BBox bbox, ref GeoCoord coord)
        {
            // fail fast if we're outside the bounding box
            if (!BBox .bboxContains(bbox, coord)) {
                return false;
            }
            bool isTransmeridian = BBox.bboxIsTransmeridian(bbox);
            bool contains = false;

            double lat = coord.Latitude;
            double lng = NORMALIZE_LON(coord.Longitude, isTransmeridian);

            GeoCoord a;
            GeoCoord b;

            int loopIndex = -1;

            while (true) {

                if (++loopIndex >= loop.NumVerts){ break;}
                a = new GeoCoord(loop.Verts[loopIndex].Latitude, loop.Verts[loopIndex].Longitude);
                b = new GeoCoord
                    (
                     loop.Verts[(loopIndex + 1) % loop.NumVerts].Latitude,
                     loop.Verts[(loopIndex + 1) % loop.NumVerts].Longitude
                    );


                //b = loop.verts[(loopIndex + 1) % loop.numVerts];

                // Ray casting algo requires the second point to always be higher
                // than the first, so swap if needed
                if (a.Latitude > b.Latitude) {
                    GeoCoord tmp = a;
                    a = b;
                    b = tmp;
                }

                // If we're totally above or below the latitude ranges, the test
                // ray cannot intersect the line segment, so let's move on
                if (lat < a.Latitude || lat > b.Latitude) {
                    continue;
                }

                double aLng = NORMALIZE_LON(a.Longitude, isTransmeridian);
                double bLng = NORMALIZE_LON(b.Longitude, isTransmeridian);

                // Rays are cast in the longitudinal direction, in case a point
                // exactly matches, to decide tiebreakers, bias westerly
                if (Math.Abs(aLng - lng) < Constants.DBL_EPSILON || Math.Abs(bLng - lng) < Constants.DBL_EPSILON)
                {
                    lng -= Constants.DBL_EPSILON;
                }

                // For the latitude of the point, compute the longitude of the
                // point that lies on the line segment defined by a and b
                // This is done by computing the percent above a the lat is,
                // and traversing the same percent in the longitudinal direction
                // of a to b
                double ratio = (lat - a.Latitude) / (b.Latitude - a.Latitude);
                double testLng =
                    NORMALIZE_LON(aLng + (bLng - aLng) * ratio, isTransmeridian);

                // Intersection of the ray
                if (testLng > lng) {
                    contains = !contains;
                }
            }

            return contains;
        }

        /// <summary>
        /// Whether the winding order of a given loop is clockwise, with normalization
        /// for loops crossing the antimeridian.
        /// </summary>
        /// <param name="loop">The loop to check</param>
        /// <param name="isTransmeridian">Whether the loop crosses the antimeridian</param>
        /// <returns>Whether the loop is clockwise</returns>
        /// <!-- Based off 3.1.1 -->
        public static bool isClockwiseNormalizedGeofence(GeoFence loop, bool isTransmeridian)
        {
            double sum = 0;
            GeoCoord a;
            GeoCoord b;

            int loopIndex = -1;

            while (true) {
                if (++loopIndex >= loop.NumVerts)
                {
                    break;
                }
                a = new GeoCoord(loop.Verts[loopIndex].Latitude, loop.Verts[loopIndex].Longitude);
                b = new GeoCoord
                    (
                     loop.Verts[(loopIndex + 1) % loop.NumVerts].Latitude,
                     loop.Verts[(loopIndex + 1) % loop.NumVerts].Longitude
                    );


                // If we identify a transmeridian arc (> 180 degrees longitude),
                // start over with the transmeridian flag set
                if (!isTransmeridian && Math.Abs(a.Longitude - b.Longitude) > Constants.M_PI) {
                    return isClockwiseNormalizedGeofence(loop, true);
                }
                sum += (NORMALIZE_LON(b.Longitude, isTransmeridian) -
                        NORMALIZE_LON(a.Longitude, isTransmeridian)) *
                       (b.Latitude + a.Latitude);
            }

            return sum > 0;
        }

        /// <summary>
        /// Whether the winding order of a given loop is clockwise. In GeoJSON,
        /// clockwise loops are always inner loops (holes).
        /// </summary>
        /// <param name="loop">The loop to check</param>
        /// <returns>Whether the loop is clockwise</returns>
        /// <!-- Based off 3.1.1 -->
        public static bool isClockwiseGeofence(GeoFence loop)
        {
            return isClockwiseNormalizedGeofence( loop, false);
        }

        /// <summary>
        /// Create a bounding box from a GeoPolygon
        /// </summary>
        /// <param name="polygon">Input <see cref="GeoPolygon"/></param>
        /// <param name="bboxes">Output bboxes, one for the outer loop and one for each hole</param>
        /// <!-- Based off 3.1.1 -->
        public static void bboxesFromGeoPolygon(GeoPolygon polygon,ref List<BBox> bboxes)
        {
            var bbox0 = bboxes[0];
            bboxFromGeofence(ref polygon.GeoFence, ref bbox0);
            bboxes[0] = bbox0;
            for (int i = 0; i < polygon.NumHoles; i++)
            {
                var tempBox = bboxes[i + 1];
                var hole = polygon.Holes[i];
                bboxFromGeofence(ref hole, ref tempBox);
                bboxes[i + 1] = tempBox;
                polygon.Holes[i] = hole;
            }
        }

        /// <summary>
        /// takes a given GeoPolygon data structure and
        /// checks if it contains a given geo coordinate.
        /// </summary>
        /// <param name="geoPolygon">The Geofence and holes defining the relevant area</param>
        /// <param name="bboxes">The bboxes for the main Geofence and each of its holes</param>
        /// <param name="coord">The coordinate to check</param>
        /// <returns>Whether the point is contained</returns>
        /// <!-- Based off 3.1.1 -->
        public static bool pointInsidePolygon(GeoPolygon geoPolygon, List<BBox> bboxes, GeoCoord coord)
        {
            // Start with contains state of primary Geofence
            var tempBox = bboxes[0];
            bool contains = pointInsideGeofence(
                ref geoPolygon.GeoFence,
                ref tempBox, ref coord);
            bboxes[0] = tempBox;

            // If the point is contained in the primary Geofence, but there are holes in
            // the Geofence iterate through all holes and return false if the point is
            // contained in any hole
            if (contains && geoPolygon.NumHoles > 0)
            {
                for (int i = 0; i < geoPolygon.NumHoles; i++)
                {
                    var hole = geoPolygon.Holes[i];
                    var box = bboxes[i + 1];
                    var isInside = pointInsideGeofence(ref hole, ref box, ref coord);
                    geoPolygon.Holes[i] = hole;
                    bboxes[i + 1] = box;

                    if (isInside)
                    {
                        return false;
                    }
                }
            }
            return contains;
        }
    }
}
