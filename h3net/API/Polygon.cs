using System;
using System.Collections.Generic;

namespace h3net.API
{
    public class Polygon
    {

        private static double NORMALIZE_LON(double lon, bool isTransmeridian)
        {
            return isTransmeridian && lon < 0
                ? lon + Constants.M_2PI
                : lon;
        }

        /**
         * Create a bounding box from a simple polygon loop.
         * Known limitations:
         * - Does not support polygons with two adjacent points > 180 degrees of
         *   longitude apart. These will be interpreted as crossing the antimeridian.
         * - Does not currently support polygons containing a pole.
         * @param loop     Loop of coordinates
         * @param bbox     Output bbox
         */
        public static void bboxFromGeofence(ref Geofence loop, ref BBox bbox)
        {
            // Early exit if there are no vertices
            if (loop.numVerts == 0) {
                bbox = new BBox();
                return;
            }

            bbox.south = Double.MaxValue;
            bbox.west = Double.MaxValue;
            bbox.north = -Double.MaxValue;
            bbox.east = -Double.MaxValue;
            double minPosLon = Double.MaxValue;
            double maxNegLon = -Double.MaxValue;
            bool isTransmeridian = false;

            double lat;
            double lon;
            GeoCoord coord;
            GeoCoord next;

            int loopIndex = -1;

            while (true) {
                
                if (++loopIndex >= loop.numVerts)
                {
                    break;
                }
                coord = loop.verts[loopIndex];
                next = loop.verts[(loopIndex + 1) % loop.numVerts];

                lat = coord.lat;
                lon = coord.lon;
                if (lat < bbox.south) {bbox.south = lat;}
                if (lon < bbox.west) {bbox.west = lon;}
                if (lat > bbox.north) {bbox.north = lat;}
                if (lon > bbox.east) {bbox.east = lon;}
                // Save the min positive and max negative longitude for
                // use in the transmeridian case
                if (lon > 0 && lon < minPosLon) minPosLon = lon;
                if (lon < 0 && lon > maxNegLon) maxNegLon = lon;
                // check for arcs > 180 degrees longitude, flagging as transmeridian
                if (Math.Abs( lon - next.lon) > Constants .M_PI) {
                    isTransmeridian = true;
                }
            }
            // Swap east and west if transmeridian
            if (isTransmeridian) {
                bbox.east = maxNegLon;
                bbox.west = minPosLon;
            }
        }

        /**
         * pointInside is the core loop of the point-in-poly algorithm
         * @param loop  The loop to check
         * @param bbox  The bbox for the loop being tested
         * @param coord The coordinate to check
         * @return      Whether the point is contained
         */
        public static bool pointInsideGeofence(ref Geofence loop, ref BBox bbox, ref GeoCoord coord)
        {
            // fail fast if we're outside the bounding box
            if (!BBox .bboxContains(bbox, coord)) {
                return false;
            }
            bool isTransmeridian = BBox.bboxIsTransmeridian(bbox);
            bool contains = false;

            double lat = coord.lat;
            double lng = NORMALIZE_LON(coord.lon, isTransmeridian);

            GeoCoord a;
            GeoCoord b;

            int loopIndex = -1;

            while (true) {

                if (++loopIndex >= loop.numVerts){ break;}
                a = loop.verts[loopIndex];            
                b = loop.verts[(loopIndex + 1) % loop.numVerts];

                // Ray casting algo requires the second point to always be higher
                // than the first, so swap if needed
                if (a.lat > b.lat) {
                    GeoCoord tmp = a;
                    a = b;
                    b = tmp;
                }

                // If we're totally above or below the latitude ranges, the test
                // ray cannot intersect the line segment, so let's move on
                if (lat < a.lat || lat > b.lat) {
                    continue;
                }

                double aLng = NORMALIZE_LON(a.lon, isTransmeridian);
                double bLng = NORMALIZE_LON(b.lon, isTransmeridian);

                // Rays are cast in the longitudinal direction, in case a point
                // exactly matches, to decide tiebreakers, bias westerly
                if (aLng == lng || bLng == lng)
                {
                    lng -= Constants.DBL_EPSILON;
                }

                // For the latitude of the point, compute the longitude of the
                // point that lies on the line segment defined by a and b
                // This is done by computing the percent above a the lat is,
                // and traversing the same percent in the longitudinal direction
                // of a to b
                double ratio = (lat - a.lat) / (b.lat - a.lat);
                double testLng =
                    NORMALIZE_LON(aLng + (bLng - aLng) * ratio, isTransmeridian);

                // Intersection of the ray
                if (testLng > lng) {
                    contains = !contains;
                }
            }

            return contains;
        }


        /**
         * Whether the winding order of a given loop is clockwise, with normalization
         * for loops crossing the antimeridian.
         * @param loop              The loop to check
         * @param isTransmeridian   Whether the loop crosses the antimeridian
         * @return                  Whether the loop is clockwise
         */
        public static bool isClockwiseNormalizedGeofence(Geofence loop, bool isTransmeridian)
        {
            double sum = 0;
            GeoCoord a;
            GeoCoord b;

            int loopIndex = -1;

            while (true) {
                if (++loopIndex >= loop.numVerts){ break;    }
                a = loop.verts[loopIndex];            
                b = loop.verts[(loopIndex + 1) % loop.numVerts];



                // If we identify a transmeridian arc (> 180 degrees longitude),
                // start over with the transmeridian flag set
                if (!isTransmeridian && Math.Abs(a.lon - b.lon) > Constants.M_PI) {
                    return isClockwiseNormalizedGeofence(loop, true);
                }
                sum += (NORMALIZE_LON(b.lon, isTransmeridian) -
                        NORMALIZE_LON(a.lon, isTransmeridian)) *
                       (b.lat + a.lat);
            }

            return sum > 0;
        }

        /**
         * Whether the winding order of a given loop is clockwise. In GeoJSON,
         * clockwise loops are always inner loops (holes).
         * @param loop  The loop to check
         * @return      Whether the loop is clockwise
         */
        public static bool isClockwiseGeofence(Geofence loop)
        {
            return isClockwiseNormalizedGeofence( loop, false);
        }

        /**
         * Create a bounding box from a GeoPolygon
         * @param polygon Input GeoPolygon
         * @param bboxes  Output bboxes, one for the outer loop and one for each hole
         */
        public static void bboxesFromGeoPolygon(GeoPolygon polygon,ref List<BBox> bboxes)
        {
            var bbox0 = bboxes[0];
            bboxFromGeofence(ref polygon.Geofence, ref bbox0);
            bboxes[0] = bbox0;
            for (int i = 0; i < polygon.numHoles; i++)
            {
                var tempBox = bboxes[i + 1];
                var hole = polygon.holes[i];
                bboxFromGeofence(ref hole, ref tempBox);
                bboxes[i + 1] = tempBox;
                polygon.holes[i] = hole;
            }
        }

        /**
         * pointInsidePolygon takes a given GeoPolygon data structure and
         * checks if it contains a given geo coordinate.
         *
         * @param geoPolygon The Geofence and holes defining the relevant area
         * @param bboxes     The bboxes for the main Geofence and each of its holes
         * @param coord      The coordinate to check
         * @return           Whether the point is contained
         */
        public static bool pointInsidePolygon(GeoPolygon geoPolygon, List<BBox> bboxes, GeoCoord coord)
        {
            // Start with contains state of primary Geofence
            var tempBox = bboxes[0];
            bool contains = pointInsideGeofence(
                ref geoPolygon.Geofence,
                ref tempBox, ref coord);
            bboxes[0] = tempBox;

            // If the point is contained in the primary Geofence, but there are holes in
            // the Geofence iterate through all holes and return false if the point is
            // contained in any hole
            if (contains && geoPolygon.numHoles > 0)
            {
                for (int i = 0; i < geoPolygon.numHoles; i++)
                {
                    var hole = geoPolygon.holes[i];
                    var box = bboxes[i + 1];
                    var isInside = pointInsideGeofence(ref hole, ref box, ref coord);
                    geoPolygon.holes[i] = hole;
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
