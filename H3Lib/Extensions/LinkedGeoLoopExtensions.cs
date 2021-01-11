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

            bool isTransmeridian = box.IsTransmeridian;
            bool contains = false;

            double lat = coord.Latitude;
            double lng = coord.Longitude.NormalizeLongitude(isTransmeridian);

            LinkedListNode<GeoCoord> currentCoord = null;

            // INIT_ITERATION
            //      LinkedGeoCoord* currentCoord = NULL; \
            //      LinkedGeoCoord* nextCoord = NULL
            //
            //      LinkedListNode<GeoCoord> currentCoord = null;
            //      LinkedListNode<GeoCoord> nextCoord = null;

            while (true) {

                
                //GET_NEXT_COORD(loop, coordToCheck) \
                //  coordToCheck == null
                //      ? loop.Loop.First
                //      : currentCoord-.Next;
                //
                //ITERATE(loop, a, b); =>
                //ITERATE_LINKED_LOOP(loop, vertexA, vertexB)
                //
                //      currentCoord = GET_NEXT_COORD(loop, currentCoord);    \
                //      if (currentCoord == NULL) break;                      \
                //      vertexA = currentCoord->vertex;                       \
                //      nextCoord = GET_NEXT_COORD(loop, currentCoord->next); \
                //      vertexB = nextCoord->vertex

                
                
                currentCoord = currentCoord == null
                                   ? loop.Loop.First
                                   : currentCoord.Next;

                if (currentCoord == null)
                {
                    break;
                }

                var a = currentCoord.Value;

                var nextCoord = (currentCoord.Next == null)
                                    ? loop.Loop.First
                                    : currentCoord.Next;

                // ReSharper disable once PossibleNullReferenceException
                var b = nextCoord.Value;

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
                if (lat < a.Latitude || lat > b.Latitude)
                {
                    continue;
                }

                double aLng = a.Longitude.NormalizeLongitude(isTransmeridian);
                double bLng = b.Longitude.NormalizeLongitude(isTransmeridian);

                // Rays are cast in the longitudinal direction, in case a point
                // exactly matches, to decide tiebreakers, bias westerly
                if (Math.Abs(aLng - lng) < double.Epsilon ||
                    Math.Abs(bLng - lng) < double.Epsilon)
                {
                    lng -= Constants.DBL_EPSILON;
                }

                // For the latitude of the point, compute the longitude of the
                // point that lies on the line segment defined by a and b
                // This is done by computing the percent above a the lat is,
                // and traversing the same percent in the longitudinal direction
                // of a to b
                double ratio = (lat - a.Latitude ) / (b.Latitude  - a.Latitude);
                double testLng =
                    (aLng + (bLng - aLng) * ratio).NormalizeLongitude(isTransmeridian);

                // Intersection of the ray
                if (testLng > lng)
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
            GeoCoord coord;
            GeoCoord next;

            //INIT_ITERATION
            LinkedListNode<GeoCoord> currentCoord = null;
            LinkedListNode<GeoCoord> nextCoord = null;

            while (true) {
                //GET_NEXT_COORD(loop, coordToCheck) \
                //  coordToCheck == null
                //      ? loop.Loop.First
                //      : currentCoord.Next;
                //
                //ITERATE_LINKED_LOOP(loop, vertexA, vertexB)
                //
                //      currentCoord = GET_NEXT_COORD(loop, currentCoord);    \
                //      if (currentCoord == NULL) break;                      \
                //      vertexA = currentCoord->vertex;                       \
                //      nextCoord = GET_NEXT_COORD(loop, currentCoord->next); \
                //      vertexB = nextCoord->vertex

                currentCoord = currentCoord == null
                                   ? loop.Loop.First
                                   : currentCoord.Next;
                if (currentCoord == null)
                {
                    break;
                }

                coord = currentCoord.Value;
                nextCoord = currentCoord.Next ?? loop.Loop.First;

                // ReSharper disable once PossibleNullReferenceException
                next = nextCoord.Value;
                
                //GET_NEXT_COORD(loop, coordToCheck) \
                //  coordToCheck == null
                //      ? loop.Loop.First
                //      : currentCoord.Next;
                //
                //ITERATE_LINKED_LOOP(loop, vertexA, vertexB)
                //
                //      currentCoord = GET_NEXT_COORD(loop, currentCoord);    \
                //      if (currentCoord == NULL) break;                      \
                //      vertexA = currentCoord->vertex;                       \
                //      nextCoord = GET_NEXT_COORD(loop, currentCoord->next); \
                //      vertexB = nextCoord->vertex

                /*
                    currentCoord = currentCoord == null
                                   ? loop.Loop.First
                                   : currentCoord.Next;
                    if (currentCoord == null)
                    {
                        break;
                    }

                    coord = currentCoord.Value;
                    nextCoord = currentCoord.Next ?? loop.Loop.First;

                    // ReSharper disable once PossibleNullReferenceException
                    next = nextCoord.Value;
                 */
                
                lat = coord.Latitude;
                lon = coord.Longitude;
                if (lat < box.South)
                {
                    box.South = lat;
                }

                if (lon < box.West)
                {
                    box.West = lon;
                }

                if (lat > box.North)
                {
                    box.North = lat;
                }

                if (lon > box.East)
                {
                    box.East = lon;
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
                box.East = maxNegLon;
                box.West = minPosLon;
            }

            return box;
        }

        public static bool IsClockwiseNormalized(this LinkedGeoLoop loop, bool isTransmeridian)
        {
            double sum = 0;
            GeoCoord a;
            GeoCoord b;

            LinkedListNode<GeoCoord> currentCoord = null;
            LinkedListNode<GeoCoord> nextCoord = null;

            while (true) 
            {
                currentCoord = currentCoord == null
                                   ? loop.Loop.First
                                   : currentCoord.Next;
                if (currentCoord == null)
                {
                    break;
                }

                a = currentCoord.Value;
                nextCoord = currentCoord.Next ?? loop.Loop.First;

                // ReSharper disable once PossibleNullReferenceException
                b = nextCoord.Value;

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
        /// linkedGoe.c
        /// static int countContainers
        /// -->
        public static int CountContainers(this LinkedGeoLoop loop, List<LinkedGeoPolygon> polygons, List<BBox> boxes)
        {
            return polygons
                  .Where(
                         (t, i) => loop != t.First &&
                                   t.First.PointInside(boxes[i], loop.First.Vertex))
                  .Count();
        }
    }
}
