using System;

namespace h3net.API
{
    public class BBox
    {
        public double north;
        public double south;
        public double east;
        public double west;
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
