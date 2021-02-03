using System;
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
            return Enumerable.Range(1, numHexagons)
                             .Select(r => (H3Index) Constants.H3Index.H3_NULL)
                             .ToList();
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
            for (int i = 0; i < geoPolygon.NumHoles; i++)
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
            // overallocate memory)
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
        /// <param name="geoPolygon">The geofence and holes defining the relevant area</param>
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
        internal static (int, List<H3Index>) PolyFillInternal(this GeoPolygon geoPolygon, int res)
        {
            // One of the goals of the polyfill algorithm is that two adjacent polygons
            // with zero overlap have zero overlapping hexagons. That the hexagons are
            // uniquely assigned. There are a few approaches to take here, such as
            // deciding based on which polygon has the greatest overlapping area of the
            // hexagon, or the most number of contained points on the hexagon (using the
            // center point as a tiebreaker).
            //
            // But if the polygons are convex, both of these more complex algorithms can
            // be reduced down to checking whether or not the center of the hexagon is
            // contained in the polygon, and so this is the approach that this polyfill
            // algorithm will follow, as it's simpler, faster, and the error for concave
            // polygons is still minimal (only affecting concave shapes on the order of
            // magnitude of the hexagon size or smaller, not impacting larger concave
            // shapes)
            //
            // This first part is identical to the maxPolyfillSize above.

            // Get the bounding boxes for the polygon and any holes
            var bboxes = geoPolygon.ToBBoxes();

            // Get the estimated number of hexagons and allocate some temporary memory
            // for the hexagons
            int numHexagons = geoPolygon.MaxPolyFillSize(res);
            var search = new H3Index[numHexagons].ToList();
            var found = new H3Index[numHexagons].ToList();
            var results = new H3Index[numHexagons].ToList();

            // Some metadata for tracking the state of the search and found memory
            // blocks
            var numSearchHexes = 0;
            var numFoundHexes = 0;

            // 1. Trace the hexagons along the polygon defining the outer geofence and
            // add them to the search hash. The hexagon containing the geofence point
            // may or may not be contained by the geofence (as the hexagon's center
            // point may be outside of the boundary.)
            var geofence = geoPolygon.GeoFence;

            //  TODO: Get rid of these refs if at all possible
            int failure = geofence
               .GetEdgeHexagons(numHexagons, res, ref numSearchHexes,  ref search, ref found);

            // If this branch is reached, we have exceeded the maximum number of
            // hexagons possible and need to clean up the allocated memory.
            if (failure > 0)
            {
                search.Clear();
                found.Clear();
                bboxes.Clear();
                results.Clear();
                return (failure, null);
            }

            // 2. Iterate over all holes, trace the polygons defining the holes with
            // hexagons and add to only the search hash. We're going to temporarily use
            // the `found` hash to use for dedupe purposes and then re-zero it once
            // we're done here, otherwise we'd have to scan the whole set on each insert
            // to make sure there's no duplicates, which is very inefficient.
            for (int i = 0; i < geoPolygon.NumHoles; i++)
            {
                var hole = geoPolygon.Holes[i];
                //  TODO: Get rid of these refs is possible.
                failure = hole
                   .GetEdgeHexagons( numHexagons, res, ref numSearchHexes,  ref search, ref found);

                // If this branch is reached, we have exceeded the maximum number of
                // hexagons possible and need to clean up the allocated memory.
                if (failure > 0)
                {
                    search.Clear();
                    found.Clear();
                    bboxes.Clear();
                    results.Clear();
                    return (failure, null);
                }
            }

            // 3. Re-zero the found hash so it can be used in the main loop below
            for (int i = 0; i < numHexagons; i++)
            {
                found[i] = 0;
            }

            // 4. Begin main loop. While the search hash is not empty do the following
            while (numSearchHexes > 0)
            {
                // Iterate through all hexagons in the current search hash, then loop
                // through all neighbors and test Point-in-Poly, if point-in-poly
                // succeeds, add to out and found hashes if not already there.
                int currentSearchNum = 0;
                int i = 0;
                while (currentSearchNum < numSearchHexes)
                {
                    H3Index searchHex = search[i];
                    var ring = searchHex.KRing(1);
                    for (var kr = ring.Count; kr < Constants.Algos.MaxOneRingSize; kr++)
                    {
                        ring.Add(0);
                    }

                    for (int j = 0; j < Constants.Algos.MaxOneRingSize; j++)
                    {
                        if (ring[j] == Constants.H3Index.H3_NULL)
                        {
                            continue;  // Skip if this was a pentagon and only had 5
                                       // neighbors
                        }

                        H3Index hex = ring[j];

                        // A simple hash to store the hexagon, or move to another place
                        // if needed. This MUST be done before the point-in-poly check
                        // since that's far more expensive
                        int loc = (int) (hex % (ulong) numHexagons);
                        int loopCount = 0;
                        while (results[loc] != 0)
                        {
                            // If this branch is reached, we have exceeded the maximum
                            // number of hexagons possible and need to clean up the
                            // allocated memory.
                            // LCOV_EXCL_START
                            if (loopCount > numHexagons)
                            {
                                search.Clear();
                                found.Clear();
                                bboxes.Clear();
                                results.Clear();
                                return (-1, null);
                            }
                            // LCOV_EXCL_STOP
                            if (results[loc] == hex)
                            {
                                break;  // Skip duplicates found
                            }
                            loc = (loc + 1) % numHexagons;
                            loopCount++;
                        }
                        if (results[loc] == hex)
                        {
                            continue;  // Skip this hex, already exists in the out hash
                        }

                        // Check if the hexagon is in the polygon or not
                        GeoCoord hexCenter = hex.ToGeoCoord();

                        // If not, skip
                        if (!geoPolygon.PointInside(bboxes,hexCenter))
                        {
                            continue;
                        }

                        // Otherwise set it in the output array
                        results[loc] = hex;

                        // Set the hexagon in the found hash
                        found[numFoundHexes] = hex;
                        numFoundHexes++;
                    }
                    currentSearchNum++;
                    i++;
                }

                // Swap the search and found pointers, copy the found hex count to the
                // search hex count, and zero everything related to the found memory.
                var temp = search;
                search = found;
                found = temp;

                for (int j = 0; j < numSearchHexes; j++)
                {
                    found[j] = 0;
                }
                numSearchHexes = numFoundHexes;
                numFoundHexes = 0;
                // Repeat until no new hexagons are found
            }
            // The out memory structure should be complete, end it here
            bboxes.Clear();
            search.Clear();
            found.Clear();
            return (0, results);
        }
    }
}
