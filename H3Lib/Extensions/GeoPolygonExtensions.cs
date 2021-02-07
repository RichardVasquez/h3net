using System.Collections.Generic;
using System.Linq;

namespace H3Lib.Extensions
{
    /// <summary>
    /// Operations on GeoPolygon type
    /// </summary>
    public static class GeoPolygonExtensions
    {
        /// <summary>
        /// pointInsidePolygon takes a given GeoPolygon data structure and
        /// checks if it contains a given geo coordinate.
        /// </summary>
        /// <param name="polygon">The geofence and holes defining the relevant area</param>
        /// <param name="boxes">The bboxes for the main geofence and each of its holes</param>
        /// <param name="coord">The coordinate to check</param>
        /// <returns>Whether the point is contained</returns>
        /// <!--
        /// polygon.c
        /// bool pointInsidePolygon
        /// -->
        public static bool PointInside(this GeoPolygon polygon, List<BBox> boxes, GeoCoord coord)
        {
            // Start with contains state of primary geofence
            bool contains =
                polygon.GeoFence.PointInside(boxes[0], coord);
            
            // If the point is contained in the primary geofence, but there are holes in
            // the geofence iterate through all holes and return false if the point is
            // contained in any hole
            if (!contains || polygon.NumHoles <= 0)
            {
                return contains;
            }
            for (var i = 0; i < polygon.NumHoles; i++)
            {
                if (polygon.Holes[i].PointInside(boxes[i + 1], coord))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Create a set of bounding boxes from a GeoPolygon
        /// </summary>
        /// <param name="polygon">Input GeoPolygon</param>
        /// <returns>
        /// Output bboxes, one for the outer loop and one for each hole
        /// </returns>
        /// <!--
        /// polygon.c
        /// void bboxesFromGeoPolygon
        /// -->
        public static List<BBox> ToBBoxes(this GeoPolygon polygon)
        {
            var results = new List<BBox> {polygon.GeoFence.ToBBox()};

            for (var i = 0; i < polygon.NumHoles; i++)
            {
                results.Add(polygon.Holes[i].ToBBox());
            }

            return results;
        }

        /// <summary>
        /// polyfill takes a given GeoJSON-like data structure and preallocated,
        /// zeroed memory, and fills it with the hexagons that are contained by
        /// the GeoJSON-like data structure.
        ///
        /// This implementation traces the GeoJSON geofence(s) in cartesian space with
        /// hexagons, tests them and their neighbors to be contained by the geofence(s),
        /// and then any newly found hexagons are used to test again until no new
        /// hexagons are found.
        /// </summary>
        /// <param name="polygon">The geofence and holes defining the relevant area</param>
        /// <param name="res">The Hexagon resolution (0-15)</param>
        /// <returns>List of H3Index that compose the polyfill</returns>
        /// <!--
        /// algos.c
        /// void H3_EXPORT(polyfill)
        /// -->
        public static List<H3Index> Polyfill(this GeoPolygon polygon, int res)
        {
            // TODO: Eliminate this wrapper with the H3 4.0.0 release
            (int failure, var result) = polygon.PolyFillInternal(res);
            if (failure == 0)
            {
                return result.Where(r => r != 0).ToList();
            }
            // The polyfill algorithm can theoretically fail if the allocated memory is
            // not large enough for the polygon, but this should be impossible given the
            // conservative overestimation of the number of hexagons possible.
            int numHexagons = polygon.MaxPolyFillSize(res);
            return new H3Index[numHexagons].ToList();
        }

        /// <summary>
        /// maxPolyfillSize returns the number of hexagons to allocate space for when
        /// performing a polyfill on the given GeoJSON-like data structure.
        ///
        /// The size is the maximum of either the number of points in the geofence or the
        /// number of hexagons in the bounding box of the geofence.
        /// </summary>
        /// <param name="geoPolygon">A GeoJSON-like data structure indicating the poly to fill</param>
        /// <param name="res">Hexagon resolution (0-15)</param>
        /// <returns>number of hexagons to allocate for</returns>
        /// <!--
        /// algos.c
        /// int H3_EXPORT(maxPolyfillSize)
        /// -->
        public static int MaxPolyFillSize(this GeoPolygon geoPolygon, int res)
        {
            // Get the bounding box for the GeoJSON-like struct
            var bbox = geoPolygon.GeoFence.ToBBox();
            int numHexagons = bbox.HexEstimate(res);
            // This algorithm assumes that the number of vertices is usually less than
            // the number of hexagons, but when it's wrong, this will keep it from
            // failing
            int totalVerts = geoPolygon.GeoFence.NumVerts;
            for (var i = 0; i < geoPolygon.NumHoles; i++)
            {
                totalVerts += geoPolygon.Holes[i].NumVerts;
            }

            if (numHexagons < totalVerts)
            {
                numHexagons = totalVerts;
            }
            // When the polygon is very small, near an icosahedron edge and is an odd
            // resolution, the line tracing needs an extra buffer than the estimator
            // function provides (but beefing that up to cover causes most situations to
            // over allocate memory)
            numHexagons += Constants.Algos.PolyfillBuffer;
            return numHexagons;
        }

        /// <summary>
        /// _polyfillInternal traces the provided geoPolygon data structure with hexagons
        /// and then iteratively searches through these hexagons and their immediate
        /// neighbors to see if they are contained within the polygon or not. Those that
        /// are found are added to the out array as well as the found array. Once all
        /// hexagons to search are checked, the found hexagons become the new search
        /// array and the found array is wiped and the process repeats until no new
        /// hexagons can be found.
        /// </summary>
        /// <remarks>
        /// This comes at it a little differently using C#'s internal HashSet rather
        /// than a collision bucket.  I've tweaked it to where it matches the speed
        /// of the original in benchmark tests, but it liekly could be made faster.
        /// </remarks>
        /// <param name="polygon">The geofence and holes defining the relevant area</param>
        /// <param name="res">The Hexagon resolution (0-15)</param>
        /// <returns>
        /// Tuple
        /// Item1 - Status code
        /// Item2 - List of H3Index values
        /// </returns>
        /// <!--
        /// algos.c
        /// int _polyfillInternal
        /// -->
        public static (int, List<H3Index>) PolyFillInternal(this GeoPolygon polygon, int res)
        {
            //  Get bounding boxes
            var bboxes = polygon.ToBBoxes();

            // Get the traced hexagons around the outer polygon;
            var geofence = polygon.GeoFence;

            var preSearch = new List<H3Index>();
            var search = geofence.GetEdgeHexagons(res);
            preSearch.AddRange(search);

            int numHexagons = polygon.MaxPolyFillSize(res);

            //  Check inner holes
            for (var i = 0; i < polygon.NumHoles; i++)
            {
                var hole = polygon.Holes[i];
                var innerHex = hole.GetEdgeHexagons(res);
                preSearch.AddRange(innerHex);
            }

            search = new HashSet<H3Index>(numHexagons);
            search.UnionWith(preSearch);
            
            var found = new HashSet<H3Index>();
            var results = new HashSet<H3Index>(numHexagons);
            int numSearchHexes = search.Count;
            var numFoundHexes = 0;

            while (numSearchHexes > 0)
            {
                var currentSearchNum = 0;
                while (currentSearchNum < numSearchHexes)
                {
                    foreach (var ring in search
                       .Select
                            (
                             index => index.KRing(1)
                                           .Where(h => h != Constants.H3Index.H3_NULL)
                            ))
                    {
                        foreach (
                            var hex in ring
                               .Where
                                    (
                                     hex => !results.Contains(hex) &&
                                            !found.Contains(hex) &&
                                            polygon.PointInside(bboxes, hex.ToGeoCoord())
                                    ))
                        {
                            found.Add(hex);
                            numFoundHexes++;
                        }

                        currentSearchNum++;
                    }
                }

                search = new HashSet<H3Index>(found);
                numSearchHexes = numFoundHexes;
                numFoundHexes = 0;
                results.UnionWith(found);
                found.Clear();
            }

            return (0, results.ToList());
        }
    }
}
