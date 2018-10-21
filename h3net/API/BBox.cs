using System;
using System.Collections.Generic;
using System.Linq;

namespace h3net.API
{
    public class BBox
    {
        public double north;
        public double south;
        public double east;
        public double west;

        /**
         * Create a bounding box from a simple polygon defined as an array of vertices.
         * Known limitations:
         * - Does not support polygons with two adjacent points > 180 degrees of
         *   longitude apart. These will be interpreted as crossing the antimeridian.
         * - Does not currently support polygons containing a pole.
         * @param verts    Array of vertices
         * @param numVerts Number of vertices
         * @param bbox     Output bbox
         */
        void bboxFromVertices(List<GeoCoord> verts, int numVerts, ref BBox bbox) 
        {
            // Early exit if there are no vertices
            if (numVerts == 0) {
                bbox.north = 0;
                bbox.south = 0;
                bbox.east = 0;
                bbox.west = 0;
                return;
            }
            double lat;
            double lon;

            bbox.south = double.MaxValue;
            bbox.west = double.MaxValue;
            bbox.north = -double.MaxValue;
            bbox.east = -double.MaxValue;
            bool isTransmeridian = false;

            for (int i = 0; i < numVerts; i++) {
                lat = verts[i].lat;
                lon = verts[i].lon;
                if (lat < bbox.south) bbox.south = lat;
                if (lon < bbox.west) bbox.west = lon;
                if (lat > bbox.north) bbox.north = lat;
                if (lon > bbox.east) bbox.east = lon;
                // check for arcs > 180 degrees longitude, flagging as transmeridian
                if (Math.Abs( lon - verts[(i + 1) % numVerts].lon) > Constants.M_PI)
                {
                    isTransmeridian = true;
                }
            }
            // Swap east and west if transmeridian
            if (isTransmeridian)
            {
                double tmp = bbox.east;
                bbox.east = bbox.west;
                bbox.west = tmp;
            }
        }

        /**
         * Create a bounding box from a Geofence
         * @param Geofence Input Geofence
         * @param bbox     Output bbox
         */
        void bboxFromGeofence(Geofence Geofence, ref BBox bbox) {
            bboxFromVertices(Geofence.verts.ToList() , Geofence.numVerts, ref bbox);
        }

        /**
         * Create a bounding box from a GeoPolygon
         * @param polygon Input GeoPolygon
         * @param bboxes  Output bboxes, one for the outer loop and one for each hole
         */
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

        /**
        * Whether the given bounding box crosses the antimeridian
        * @param  bbox Bounding box to inspect
        * @return      is transmeridian
        */
        public static bool bboxIsTransmeridian(BBox bbox)
        {
            return bbox.east < bbox.west;
        }

        /**
         * Get the center of a bounding box
         * @param bbox   Input bounding box
         * @param center Output center coordinate
         */
        public static void bboxCenter(BBox bbox, ref GeoCoord center)
        {
            center.lat = (bbox.north + bbox.south) / 2.0;
            // If the bbox crosses the antimeridian, shift east 360 degrees
            double east = bboxIsTransmeridian(bbox)
                ? bbox.east + Constants.M_2PI
                : bbox.east;
            center.lon = GeoCoord.constrainLng((east + bbox.west) / 2.0);
        }

        /**
         * Whether the bounding box contains a given point
         * @param  bbox  Bounding box
         * @param  point Point to test
         * @return       Whether the point is contained
         */
        public static bool bboxContains(BBox bbox, GeoCoord point)
        {
            return 
                point.lat >= bbox.south && 
                point.lat <= bbox.north &&
                (
                    bboxIsTransmeridian(bbox)
                    // transmeridian case
                    ? point.lon >= bbox.west || point.lon <= bbox.east
                    // standard case
                    : point.lon >= bbox.west && point.lon <= bbox.east
                );
        }

        /**
         * Whether two bounding boxes are strictly equal
         * @param  b1 Bounding box 1
         * @param  b2 Bounding box 2
         * @return    Whether the boxes are equal
         */
        public static bool bboxEquals(BBox b1, BBox b2)
        {
            return Math.Abs(b1.north - b2.north) < Constants.EPSILON &&
                   Math.Abs(b1.south - b2.south) < Constants.EPSILON &&
                   Math.Abs(b1.east - b2.east) < Constants.EPSILON &&
                   Math.Abs(b1.west - b2.west) < Constants.EPSILON;
        }

        /**
         * _hexRadiusKm returns the radius of a given hexagon in Km
         *
         * @param h3Index the index of the hexagon
         * @return the radius of the hexagon in Km
         */
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

        /**
         * Get the radius of the bbox in hexagons - i.e. the radius of a k-ring centered
         * on the bbox center and covering the entire bbox.
         * @param  bbox Bounding box to measure
         * @param  res  Resolution of hexagons to use in measurement
         * @return      Radius in hexagons
         */
        public static int bboxHexRadius(BBox bbox, int res)
        {
            // Determine the center of the bounding box
            GeoCoord center = new GeoCoord();
            bboxCenter(bbox, ref center);

            // Use a vertex on the side closest to the equator, to ensure the longest
            // radius in cases with significant distortion. East/west is arbitrary.
            double lat =
                Math.Abs( bbox.north) > Math.Abs(bbox.south)
                    ? bbox.south
                    : bbox.north;

            GeoCoord vertex = new GeoCoord {lat = lat, lon = bbox.east};

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
