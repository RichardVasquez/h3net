using System;
using System.Linq;

namespace H3Lib.Extensions
{
    public static class BBoxExtensions
    {
        /// <summary>
        /// Gets the center of a bounding box
        /// </summary>
        /// <param name="box">input bounding box</param>
        /// <returns>output center coordinate</returns>
        /// <!--
        /// bbox.c
        /// void bboxCenter
        /// -->
        public static GeoCoord Center(this BBox box)
        {
            double latitude = (box.North + box.South) / 2.0;
            // If the bbox crosses the antimeridian, shift east 360 degrees
            double east = box.IsTransmeridian
                              ? box.East + Constants.M_2PI
                              : box.East;
            double longitude = ((east + box.West) / 2.0).ConstrainLongitude();
            return new GeoCoord(latitude, longitude);
        }

        /// <summary>
        /// Whether the bounding box contains a given point
        /// </summary>
        /// <param name="box">Bounding box</param>
        /// <param name="point">Point to test</param>
        /// <returns>Whether the point is contained</returns>
        /// <!--
        /// bbox.c
        /// ool bboxContains
        /// -->
        public static bool Contains(this BBox box, GeoCoord point)
        {
            return point.Latitude >= box.South &&
                   point.Latitude <= box.North &&
                   box.IsTransmeridian
                       // transmeridian case
                       ? point.Longitude >= box.West || point.Longitude <= box.East
                       // standard case
                       : point.Longitude >= box.West && point.Longitude <= box.East;
        }

        /// <summary>
        /// returns an estimated number of hexagons that fit
        /// within the cartesian-projected bounding box
        /// </summary>
        /// <param name="box">bounding box to estimate the hexagon fill level</param>
        /// <param name="res">resolution of the H3 hexagons to fill the bounding box</param>
        /// <returns>estimated number of hexagons to fill the bounding box</returns>
        /// <!--
        /// bbox.c
        /// int bboxHexEstimate
        /// -->
        public static int HexEstimate(this BBox box, int res)
        {
            // Get the area of the pentagon as the maximally-distorted area possible
            var pentagons = res.GetPentagonIndexes();
            double pentagonRadiusKm = pentagons[0].HexRadiusKm();

            // Area of a regular hexagon is 3/2*sqrt(3) * r * r
            // The pentagon has the most distortion (smallest edges) and shares its
            // edges with hexagons, so the most-distorted hexagons have this area,
            // shrunk by 20% off chance that the bounding box perfectly bounds a
            // pentagon.
            double pentagonAreaKm2 =
                0.8 * (2.59807621135 * pentagonRadiusKm * pentagonRadiusKm);

            // Then get the area of the bounding box of the geofence in question
            var p1 = new GeoCoord(box.North, box.East);
            var p2 = new GeoCoord(box.South, box.West);
            double d = p1.DistanceToKm(p2);

            // Derived constant based on: https://math.stackexchange.com/a/1921940
            // Clamped to 3 as higher values tend to rapidly drag the estimate to zero.
            double a = d * d /
                       new[]
                           {
                               3.0,
                               Math.Abs((p1.Longitude - p2.Longitude) / (p1.Latitude - p2.Latitude))
                           }
                          .Min();

            // Divide the two to get an estimate of the number of hexagons needed
            var estimate = (int) Math.Ceiling(a / pentagonAreaKm2);
            if (estimate == 0)
            {
                estimate = 1;
            }
            return estimate;
        }
    }
}
