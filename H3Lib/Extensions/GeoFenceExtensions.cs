using System;
using System.Collections.Generic;

namespace H3Lib.Extensions
{
    public static class GeoFenceExtensions
    {
        public static bool PointInside(this GeoFence loop, BBox box, GeoCoord coord)
        {
            // fail fast if we're outside the bounding box
            if (!box.Contains(coord))
            {
                return false;
            }

            bool isTransmeridian = box.IsTransmeridian;
            var contains = false;

            double lat = coord.Latitude;
            double lng = coord.Longitude.NormalizeLongitude(isTransmeridian);

            // TODO: Incorporate equivalent from LinkedGeoLoopExtensions.cs
            
            GeoCoord a;
            GeoCoord b;

            //  INIT_ITERATION;
            int loopIndex = -1;

            while (true)
            {
                //  #define ITERATE_GEOFENCE(geofence, vertexA, vertexB) \
                //      if (++loopIndex >= geofence->numVerts) break;    \
                //      vertexA = geofence->verts[loopIndex];            \
                //      vertexB = geofence->verts[(loopIndex + 1) % geofence->numVerts]
                //               ITERATE(loop, a, b);

                 if (++loopIndex >= loop.NumVerts)
                 {
                     break;
                 }

                 a = loop.Verts[loopIndex];
                 b = loop.Verts[(loopIndex + 1) % loop.NumVerts];
                 
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
                if (Math.Abs(aLng - lng) < double.Epsilon || Math.Abs(bLng - lng) < double.Epsilon)
                {
                    lng -= Constants.DBL_EPSILON;
                }

                // For the latitude of the point, compute the longitude of the
                // point that lies on the line segment defined by a and b
                // This is done by computing the percent above a the lat is,
                // and traversing the same percent in the longitudinal direction
                // of a to b
                double ratio = (lat - a.Latitude) / (b.Latitude - a.Latitude);
                double testLng = (aLng + (bLng - aLng) * ratio).NormalizeLongitude(isTransmeridian);

                // Intersection of the ray
                if (testLng > lng)
                {
                    contains = !contains;
                }
            }

            return contains;
        }
        
        public static BBox ToBBox(this GeoFence loop)
        {
            if (loop.IsEmpty)
            {
                return new BBox(0, 0, 0, 0);
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
            int loopIndex = -1;


            while (true)
            {
                if (++loopIndex >= loop.NumVerts)
                {
                    break;
                }

                coord = loop.Verts[loopIndex];
                next = loop.Verts[(loopIndex + 1) % loop.NumVerts];

                
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
        
        public static bool IsClockwiseNormalized(this GeoFence loop, bool isTransmeridian)
        {
            double sum = 0;
            GeoCoord a;
            GeoCoord b;

            //  INIT_ITERATION;
            int loopIndex = -1;

            while (true) 
            {
                if (++loopIndex >= loop.NumVerts)
                {
                    break;
                }

                a = loop.Verts[loopIndex];
                b = loop.Verts[(loopIndex + 1) % loop.NumVerts];

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

        public static bool IsClockwise(this GeoFence loop)
        {
            return loop.IsClockwiseNormalized(false);
        }

        /// <summary>
        /// _getEdgeHexagons takes a given geofence ring (either the main geofence or
        /// one of the holes) and traces it with hexagons and updates the search and
        /// found memory blocks. This is used for determining the initial hexagon set
        /// for the polyfill algorithm to execute on.
        /// </summary>
        /// <param name="geofence">The geofence (or hole) to be traced</param>
        /// <param name="numHexagons">
        /// The maximum number of hexagons possible for the geofence
        /// (also the bounds of the search and found arrays)
        /// </param>
        /// <param name="res">The hexagon resolution (0-15)</param>
        /// <param name="numSearchHexagons">The number of hexagons found so far to be searched</param>
        /// <param name="search">The block of memory containing the hexagons to search from</param>
        /// <param name="found">The block of memory containing the hexagons found from the search</param>
        /// <returns>
        ///An error code if the hash function cannot insert a found hexagon into the found array.
        /// </returns>
        /// <!--
        /// algos.c
        /// int _getEdgeHexagons
        /// -->
        public static int GetEdgeHexagons(this GeoFence geofence, int numHexagons, int res,
                                                    ref int numSearchHexagons, ref List<H3Index> search,
                                                    ref List<H3Index> found)
        {
            for (int i = 0; i < geofence.NumVerts; i++)
            {
                var origin = geofence.Verts[i];
                var destination = i == geofence.NumVerts - 1
                                      ? geofence.Verts[0]
                                      : geofence.Verts[i + 1];
                int numHexesEstimate = origin.LineHexEstimate(destination, res);

                for (var j = 0; j < numHexesEstimate; j++)
                {
                    var interpolate = 
                        new GeoCoord(
                                     origin.Latitude * (numHexesEstimate - j) / numHexesEstimate +
                                     destination.Latitude * j / numHexesEstimate,
                                     origin.Longitude * (numHexesEstimate - j) / numHexesEstimate +
                                     destination.Longitude * j / numHexesEstimate
                                     );

                    var pointHex = interpolate.ToH3Index(res);
                    
                    // A simple hash to store the hexagon, or move to another place if
                    // needed
                    var loc = (int) (pointHex % (ulong) numHexagons);
                    var loopCount = 0;
                    while (found[loc] != 0)
                    {
                        // If this conditional is reached, the `found` memory block is
                        // too small for the given polygon. This should not happen.
                        if (loopCount > numHexagons)
                        {
                            return StaticData.Algos.HexHashOverflow; // LCOV_EXCL_LINE
                        }
                        if (found[loc] == pointHex)
                        {
                            break;  // At least two points of the geofence index to the same cell
                        }
                        loc = (loc + 1) % numHexagons;
                        loopCount++;
                    }
                    if (found[loc] == pointHex)
                    {
                        continue;  // Skip this hex, already exists in the found hash
                    }
                    // Otherwise, set it in the found hash for now
                    found[loc] = pointHex;

                    search[numSearchHexagons] = pointHex;
                    numSearchHexagons++;
                }
            }

            return 0;
        }
    }
}
