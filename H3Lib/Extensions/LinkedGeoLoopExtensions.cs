using System;
using System.Collections.Generic;
using System.Linq;

namespace H3Lib.Extensions
{
    public static class LinkedGeoLoopExtensions
    {
        public static bool PointInside(this LinkedGeoLoop loop, BBox box, GeoCoord coord)
        {
            // fail fast if we're outside the bounding box
            if(!box.Contains(coord))
            {
                return false;
            }
            
            //  Gonna add another test here for quick fail, as we need a triangle+ to have an inside
            if (loop.GeoCoordList.Count < 3)
            {
                return false;
            }

            bool isTransmeridian = box.IsTransmeridian;
            bool contains = false;

            double targetLatitude = coord.Latitude;
            double targetLongitude = coord.Longitude.NormalizeLongitude(isTransmeridian);

            //  If I've got this right, I may be able to make this part of an abstract
            var nodes = loop.GeoCoordList.ToList();
            for (int idx = 0; idx < nodes.Count; idx++)
            {
                var a = nodes[idx];
                var b = nodes[(idx + 1) % nodes.Count];
                
                // Ray casting algo requires the second point to always be higher
                // than the first, so swap if needed
                if (a.Latitude > b.Latitude)
                {
                    var tmp = a;
                    a = b;
                    b = tmp;
                }

                // If we're totally above or below the latitude ranges, the test
                // ray cannot intersect the line segment, so let's move on
                if (targetLatitude < a.Latitude || targetLatitude > b.Latitude)
                {
                    continue;
                }

                double aLng = a.Longitude.NormalizeLongitude(isTransmeridian);
                double bLng = b.Longitude.NormalizeLongitude(isTransmeridian);

                // Rays are cast in the longitudinal direction, in case a point
                // exactly matches, to decide tiebreakers, bias westerly
                if (Math.Abs(aLng - targetLongitude) < double.Epsilon ||
                    Math.Abs(bLng - targetLongitude) < double.Epsilon)
                {
                    targetLongitude -= Constants.DBL_EPSILON;
                }

                // For the latitude of the point, compute the longitude of the
                // point that lies on the line segment defined by a and b
                // This is done by computing the percent above a the lat is,
                // and traversing the same percent in the longitudinal direction
                // of a to b
                double ratio = (targetLatitude - a.Latitude ) / (b.Latitude  - a.Latitude);
                double testLng =
                    (aLng + (bLng - aLng) * ratio).NormalizeLongitude(isTransmeridian);

                // Intersection of the ray
                if (testLng > targetLongitude)
                {
                    contains = !contains;
                }
            }

            return contains;
        }

        public static BBox ToBBox(this LinkedGeoLoop loop)
        {
            if (loop.IsEmpty)
            {
                return new BBox();
            }

            var box = new BBox(-double.MaxValue, double.MaxValue, -double.MaxValue, double.MaxValue);
            double minPosLon = double.MaxValue;
            double maxNegLon = -double.MaxValue;
            bool isTransmeridian = false;

            double lat;
            double lon;

            var nodes = loop.GeoCoordList.ToList();
            for (int idx = 0; idx < nodes.Count; idx++)
            {
                var coord = nodes[idx];
                var next = nodes[(idx + 1) % nodes.Count];

                lat = coord.Latitude;
                lon = coord.Longitude;
                if (lat < box.South)
                {
                    box = box.ReplaceSouth(lat);
                }

                if (lon < box.West)
                {
                    box = box.ReplaceWest(lon);
                }

                if (lat > box.North)
                {
                    box = box.ReplaceNorth(lat);
                }

                if (lon > box.East)
                {
                    box = box.ReplaceEast(lon);
                }
                // Save the min positive and max negative longitude for
                // use in the transmeridian case
                if (lon > 0 && lon < minPosLon)
                {
                    minPosLon = lon;
                }

                if (lon < 0 && lon > maxNegLon)
                {
                    maxNegLon = lon;
                }
                // check for arcs > 180 degrees longitude, flagging as transmeridian
                if (Math.Abs(lon - next.Longitude) > Constants.M_PI)
                {
                    isTransmeridian = true;
                }
            }
            // Swap east and west if transmeridian
            if (isTransmeridian)
            {
                box = box.ReplaceEW(maxNegLon, minPosLon);
            }
            return box;
        }

        public static bool IsClockwiseNormalized(this LinkedGeoLoop loop, bool isTransmeridian)
        {
            double sum = 0;

            var nodes = loop.GeoCoordList.ToList();
            for (int idx = 0; idx < nodes.Count; idx++)
            {
                var a = nodes[idx];
                var b = nodes[(idx + 1) % nodes.Count];

                // If we identify a transmeridian arc (> 180 degrees longitude),
                // start over with the transmeridian flag set
                if (!isTransmeridian && Math.Abs(a.Longitude - b.Longitude) > Constants.M_PI)
                {
                    return loop.IsClockwiseNormalized(true);
                }
                sum += (b.Longitude.NormalizeLongitude(isTransmeridian) -
                        a.Longitude.NormalizeLongitude( isTransmeridian)) *
                        (b.Latitude + a.Latitude);
            }

            return sum > 0;
        }

        public static bool IsClockwise(this LinkedGeoLoop loop)
        {
            return loop.IsClockwiseNormalized(false);
        }

        /// <summary>
        /// Count the number of polygons containing a given loop.
        /// </summary>
        /// <param name="loop">Loop to count containers for</param>
        /// <param name="polygons">Polygons to test</param>
        /// <param name="boxes">Bounding boxes for polygons, used in point-in-poly check</param>
        /// <returns>Number of polygons containing the loop</returns>
        /// <!--
        /// linkedGeo.c
        /// static int countContainers
        /// -->
        public static int CountContainers(this LinkedGeoLoop loop, List<LinkedGeoPolygon> polygons, List<BBox> boxes)
        {
            return polygons
                  .Where
                       (
                        (poly, index) =>
                            poly.LinkedGeoList.First != null &&
                            loop.GeoCoordList.First != null &&
                            loop != poly.LinkedGeoList.First.Value &&
                            poly.LinkedGeoList.First.Value.PointInside(boxes[index], loop.GeoCoordList.First.Value)
                       )
                  .Count();

            /*
            int containerCount = 0;
            for (int i = 0; i < polygons.Count; i++)
            {
                if(polygons[i].LinkedGeoList.First!=null && loop.GeoCoordList.First!=null)
                {
                    if (loop != polygons[i].LinkedGeoList.First.Value &&
                        polygons[i].LinkedGeoList.First.Value.PointInside(boxes[i], loop.GeoCoordList.First.Value))
                    {
                        containerCount++;
                    }
                }
            }
            return containerCount;
         
*/
            //            return polygons
            //                  .Where(
            //                         (t, i) => loop != t.First &&
            //                                   t.First.PointInside(boxes[i], loop.First.Vertex))
            //                 .Count();
        }
    }
}
