using System;
using System.Collections.Generic;
using System.Linq;

namespace H3Lib
{
    
    /// <summary>
    /// Simplified core of GeoJSON MultiPolygon coordinates definition
    /// </summary>
    public class GeoMultiPolygon
    {
        public int NumPolygons;
        public List<GeoPolygon> Polygons;
    }

    /// <summary>
    /// Linked data structure for geo data
    /// </summary>
    // ReSharper disable once ClassNeverInstantiated.Global
    public class LinkedGeo
    {


        public const int NormalizationSuccess = 0;
        public const int NormalizationErrMultiplePolygons = 1;
        public const int NormalizationErrUnassignedHoles = 2;

        public LinkedGeo()
        {
        }

        private static double  NORMALIZE_LON(double lon, bool isTransmeridian)
        {
            return isTransmeridian && lon < 0 ? lon + Constants.M_2PI : lon;
        }

        /// <summary>
        /// Add a linked polygon to the current polygon
        /// </summary>
        /// <param name="polygon">Polygon to add link to</param>
        /// <returns>Pointer to new polygon</returns>
        public static LinkedGeoPolygon AddNewLinkedPolygon(ref LinkedGeoPolygon polygon)
        {
            if (polygon.Next != null)
            {
                throw new Exception("assert(polygon->next == NULL);");
            }
            
            var next = new LinkedGeoPolygon();
            polygon.Next = next;
            return next;
        }

        /// <summary>
        /// Add a new linked loop to the current polygon
        /// </summary>
        /// <param name="polygon">Polygon to add loop to</param>
        /// <returns>Pointer to loop</returns>
        public static LinkedGeoLoop AddNewLinkedLoop(ref LinkedGeoPolygon polygon)
        {
            LinkedGeoLoop loop = new LinkedGeoLoop();
            if (loop == null)
            {
                throw new Exception("FAIL: assert(loop != NULL)");
            }

            return AddLinkedLoop(ref polygon,ref  loop);
        }

        /// <summary>
        /// Add an existing linked loop to the current polygon
        /// </summary>
        /// <param name="polygon">Polygon to add loop to</param>
        /// <param name="loop">Loop being added to polygon</param>
        /// <returns>Pointer to loop</returns>
        public static  LinkedGeoLoop AddLinkedLoop(ref LinkedGeoPolygon polygon, ref LinkedGeoLoop loop)
        {
            LinkedGeoLoop last = polygon.Last;
            if (last == null) {
                if (polygon.First != null)
                {
                    throw new Exception("FAIL: assert(polygon->first == NULL)");
                }
                polygon.First = loop;
            } else {
                last.Next = loop;
            }
            polygon.Last = loop;
            return loop;
        }

        /// <summary>
        /// Add a new linked coordinate to the current loop
        /// </summary>
        /// <param name="loop">Loop to add coordinate to</param>
        /// <param name="vertex">Coordinate to add</param>
        /// <returns>Pointer to the coordinate</returns>
        public static LinkedGeoCoord AddLinkedCoord(ref LinkedGeoLoop loop, ref GeoCoord vertex)
        {
            var coord = new LinkedGeoCoord
                        {
                            Vertex = new GeoCoord(vertex.Latitude, vertex.Longitude), Next = null
                        };

            var last = loop.Last;
            
            if (last == null) {
                if (loop.First != null)
                {
                    throw new Exception("assert(loop->first == NULL);");
                }
                loop.First = coord;
            } else {
                last.Next = coord;
            }
            loop.Last = coord;
            return coord;
        }

        /// <summary>
        /// Free all allocated memory for a linked geo loop. The caller is
        /// responsible for freeing memory allocated to input loop struct.
        /// </summary>
        /// <param name="loop">Loop to free</param>
        public static void DestroyLinkedGeoLoop(ref LinkedGeoLoop loop)
        {
            LinkedGeoCoord nextCoord;
            for (LinkedGeoCoord currentCoord = loop.First; currentCoord != null;
                 currentCoord = nextCoord)
            {
                nextCoord = currentCoord.Next;
                // ReSharper disable once RedundantAssignment
                currentCoord = null;
            }
        }

        /// <summary>
        /// Free all allocated memory for a linked geo structure. The caller is
        /// responsible for freeing memory allocated to input polygon struct.
        /// </summary>
        /// <param name="polygon">Pointer to the first polygon in the structure</param>
        public static void DestroyLinkedPolygon(ref LinkedGeoPolygon polygon)
        {
            // flag to skip the input polygon
            var skip = true;
            LinkedGeoPolygon nextPolygon;
            for (var currentPolygon = polygon; currentPolygon !=null;
                 currentPolygon = nextPolygon)
            {
                LinkedGeoLoop nextLoop;
                for (var currentLoop = currentPolygon.First;
                     currentLoop != null; currentLoop = nextLoop)
                {
                    DestroyLinkedGeoLoop(ref currentLoop);
                    nextLoop = currentLoop.Next;
                }
                nextPolygon = currentPolygon.Next;
                if (skip)
                {
                    // do not free the input polygon
                    skip = false;
                }
            }
        }

        /// <summary>
        /// Count the number of polygons in a linked list
        /// </summary>
        /// <param name="polygon">Starting polygon</param>
        /// <returns>Count</returns>
        public static int CountLinkedPolygons(ref LinkedGeoPolygon polygon)
        {
            var polyIndex = polygon;
            var count = 0;
            while (polyIndex != null)
            {
                count++;
                polyIndex = polyIndex.Next;
            }
            return count;
        }

        /// <summary>
        /// Count the number of linked loops in a polygon
        /// </summary>
        /// <param name="polygon">Polygon to count loops for</param> 
        /// <returns>Count</returns>
        public static int CountLinkedLoops(ref LinkedGeoPolygon polygon)
        {
            var loop = polygon.First;
            var count = 0;
            while (loop != null)
            {
                count++;
                loop = loop.Next;
            }
            return count;
        }

        /// <summary>
        /// Count the number of coordinates in a loop
        /// </summary>
        /// <param name="loop"> Loop to count coordinates for</param>
        /// <returns>Count</returns>
        public static int CountLinkedCoords(ref LinkedGeoLoop loop)
        {
            var coord = loop.First;
            var count = 0;
            while (coord != null)
            {
                count++;
                coord = coord.Next;
            }
            return count;
        }

        /// <summary>
        /// Count the number of polygons containing a given loop.
        /// </summary>
        /// <param name="loop">Loop to count containers for</param>
        /// <param name="polygons">Polygons to test</param>
        /// <param name="bboxes">Bounding boxes for polygons, used in point-in-poly check</param>
        /// <param name="polygonCount">Number of polygons in the test array</param>
        /// <returns>Number of polygons containing the loop</returns>
        private static int CountContainers(
            LinkedGeoLoop loop, IReadOnlyList<LinkedGeoPolygon> polygons,
            IReadOnlyList<BBox> bboxes, int polygonCount)
        {
            var containerCount = 0;
            for (var i = 0; i < polygonCount; i++)
            {
                var bb = bboxes[i];
                if (loop != polygons[i].First &&
                    PointInsideLinkedGeoLoop(ref polygons[i].First, ref bb, ref loop.First.Vertex))
                {
                    containerCount++;
                }
            }
            return containerCount;
        }

        /// <summary>
        /// Given a list of nested containers, find the one most deeply nested.
        /// </summary>
        /// <param name="polygons">Polygon containers to check</param>
        /// <param name="bboxes">Bounding boxes for polygons, used in point-in-poly check</param>
        /// <param name="polygonCount">Number of polygons in the list</param>
        /// <returns>Deepest container, or null if list is empty</returns>
        private static LinkedGeoPolygon FindDeepestContainer(
            ref List<LinkedGeoPolygon> polygons, ref List<BBox> bboxes,
            int polygonCount)
        {
            // Set the initial return value to the first candidate
            var parent = polygonCount > 0 ? polygons[0] : null;

            // If we have multiple polygons, they must be nested inside each other.
            // Find the innermost polygon by taking the one with the most containers
            // in the list.
            if (polygonCount <= 1)
            {
                return parent;
            }
            int max = -1;
            for (int i = 0; i < polygonCount; i++) 
            {
                int count = CountContainers(polygons[i].First, polygons, bboxes, polygonCount);
                if (count <= max)
                {
                    continue;
                }
                parent = polygons[i];
                max = count;
            }
            return parent;
        }

        /// <summary>
        /// Find the polygon to which a given hole should be allocated. Note that this
        /// function will return null if no parent is found.
        /// </summary>
        /// <param name="loop">Inner loop describing a hole</param>
        /// <param name="polygon">Head of a linked list of polygons to check</param>
        /// <param name="bboxes">Bounding boxes for polygons, used in point-in-poly check</param>
        /// <param name="polygonCount">Number of polygons to check</param>
        /// <returns>Pointer to parent polygon, or null if not found</returns>
        private static LinkedGeoPolygon FindPolygonForHole(
            ref LinkedGeoLoop loop,
            ref LinkedGeoPolygon polygon,
            ref List<BBox> bboxes,
            int polygonCount)
        {
            // Early exit with no polygons
            if (polygonCount == 0) {
                return null;
            }
            // Initialize arrays for candidate loops and their bounding boxes
            var candidates = Enumerable.Range(1, polygonCount)
                                       .Select(i => new LinkedGeoPolygon()).ToList();
            var candidateBBoxes = Enumerable.Range(1, polygonCount)
                                            .Select(i => new BBox()).ToList();
            // Find all polygons that contain the loop
            var candidateCount = 0;
            var index = 0;
            var polygonReference = polygon;
            while (polygonReference != null) 
            {
                // We are guaranteed not to overlap, so just test the first point
                var bb = bboxes[index];
                if (
                    PointInsideLinkedGeoLoop(
                        ref polygonReference.First, ref bb, ref loop.First.Vertex
                    )
                )
                {
                    candidates[candidateCount] = polygonReference;
                    candidateBBoxes[candidateCount] = bboxes[index];
                    candidateCount++;
                }
                polygonReference = polygonReference.Next;
                index++;
            }

            // The most deeply nested container is the immediate parent
            LinkedGeoPolygon parent =
                FindDeepestContainer(ref candidates, ref candidateBBoxes, candidateCount);

            return parent;
        }

        /// <summary>
        /// Normalize a LinkedGeoPolygon in-place into a structure following GeoJSON
        /// MultiPolygon rules: Each polygon must have exactly one outer loop, which
        /// must be first in the list, followed by any holes. Holes in this algorithm
        /// are identified by winding order (holes are clockwise), which is guaranteed
        /// by the h3SetToVertexGraph algorithm.
        /// 
        /// Input to this function is assumed to be a single polygon including all
        /// loops to normalize. It's assumed that a valid arrangement is possible.
        /// </summary>
        /// <param name="root">Root polygon including all loops</param>
        /// <returns>0 on success, or an error code > 0 for invalid input</returns>
        public static int NormalizeMultiPolygon(ref LinkedGeoPolygon root)
        {
            // We assume that the input is a single polygon with loops;
            // if it has multiple polygons, don't touch it
            if (root.Next != null)
            {
                return NormalizationErrMultiplePolygons;
            }

            // Count loops, exiting early if there's only one
            int loopCount = CountLinkedLoops(ref root);
            if (loopCount <= 1)
            {
                return NormalizationSuccess;
            }

            int resultCode = NormalizationSuccess;
            LinkedGeoPolygon polygon = null;
            var innerCount = 0;
            var outerCount = 0;

            // Create an array to hold all of the inner loops. Note that
            // this array will never be full, as there will always be fewer
            // inner loops than outer loops.
            var innerLoops = Enumerable.Range(1, loopCount)
                                       .Select(i => new LinkedGeoLoop()).ToList();
            // Create an array to hold the bounding boxes for the outer loops
            var bboxes = Enumerable.Range(1, loopCount)
                                   .Select(i => new BBox()).ToList();

            // Get the first loop and unlink it from root
            var loop = root.First;
            root = new LinkedGeoPolygon();

            // Iterate over all loops, moving inner loops into an array and
            // assigning outer loops to new polygons
            while (loop != null)
            {
                if (IsClockwiseLinkedGeoLoop(loop))
                {
                    innerLoops[innerCount] = loop;
                    innerCount++;
                }
                else
                {
                    polygon = polygon == null ? root : AddNewLinkedPolygon(ref polygon);
                    AddLinkedLoop(ref polygon, ref loop);
                    var bb = bboxes[outerCount];
                    BboxFromLinkedGeoLoop(ref loop, ref bb);
                    bboxes[outerCount] = bb;
                    outerCount++;
                }

                // get the next loop and unlink it from this one
                var next = loop.Next;
                loop.Next = null;
                loop = next;
            }

            // Find polygon for each inner loop and assign the hole to it
            for (int i = 0; i < innerCount; i++)
            {
                var inner1 = innerLoops[i];
                polygon = FindPolygonForHole(ref inner1, ref root, ref bboxes, outerCount);
                if (polygon != null)
                {
                    var inner2 = innerLoops[i];
                    AddLinkedLoop(ref polygon, ref inner2);
                    innerLoops[i] = inner2;
                }
                else
                {
                    // If we can't find a polygon (possible with invalid input), then
                    // we need to release the memory for the hole, because the loop has
                    // been unlinked from the root and the caller will no longer have
                    // a way to destroy it with destroyLinkedPolygon.
                    var inner2 = innerLoops[i];
                    DestroyLinkedGeoLoop(ref inner2);
                    innerLoops[i] = null;
                    resultCode = NormalizationErrUnassignedHoles;
                }
            }
            return resultCode;
        }

        /// <summary>
        /// Take a given LinkedGeoLoop data structure and check if it
        /// contains a given geo coordinate.
        /// </summary>
        /// <param name="loop">The linked loop</param>
        /// <param name="bbox">The bbox for the loop</param>
        /// <param name="coord">The coordinate to check</param>
        /// <returns>Whether the point is contained</returns>
        public static bool PointInsideLinkedGeoLoop(ref LinkedGeoLoop loop, ref  BBox bbox, ref GeoCoord coord) 
        {
            // fail fast if we're outside the bounding box
            if (!BBox.bboxContains(bbox, coord))
            {
                return false;
            }
            bool isTransmeridian =BBox. bboxIsTransmeridian(bbox);
            var contains = false;

            double lat = coord.Latitude;
            double lng = NORMALIZE_LON(coord.Longitude, isTransmeridian);

            LinkedGeoCoord currentCoord = null;

            while (true) {

                currentCoord = currentCoord == null ? loop.First : currentCoord.Next;
                if (currentCoord == null)
                {
                    break;
                }
            
                var a = currentCoord.Vertex;
                var nextCoord = currentCoord.Next ?? loop.First;
                var b = nextCoord.Vertex;

                // Ray casting algo requires the second point to always be higher
                // than the first, so swap if needed
                if (a.Latitude > b.Latitude) {
                    var tmp = new GeoCoord( a.Latitude,a.Longitude);
                    a = new GeoCoord(b.Latitude, b.Longitude);
                    b = new GeoCoord(tmp.Latitude,tmp.Longitude);
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
                if (Math.Abs(aLng - lng) < Constants.EPSILON || Math.Abs(bLng - lng) < Constants.EPSILON) {
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
        /// Create a bounding box from a simple polygon loop.
        /// Known limitations:
        /// - Does not support polygons with two adjacent points > 180 degrees of
        ///   longitude apart. These will be interpreted as crossing the antimeridian.
        /// - Does not currently support polygons containing a pole.
        /// </summary>
        /// <param name="loop">Loop of coordinates</param>
        /// <param name="bbox">bbox</param>
        public static void BboxFromLinkedGeoLoop(ref LinkedGeoLoop loop, ref BBox bbox)
        {
            // Early exit if there are no vertices
            if (loop.First == null)
            {
                bbox = new BBox();
                return;
            }

            bbox.South = double.MaxValue;
            bbox.West = double.MaxValue;
            bbox.North = -double.MaxValue;
            bbox.East = -double.MaxValue;
            var minPosLon = double.MaxValue;
            double maxNegLon = -double.MaxValue;
            var isTransmeridian = false;

            LinkedGeoCoord currentCoord = null;

            while (true) {

                currentCoord = currentCoord == null ? loop.First : currentCoord.Next;
                if (currentCoord == null)
                {
                    break;
                }

                var coord = currentCoord.Vertex;
                var nextCoord = currentCoord.Next ?? loop.First;
                var next = nextCoord.Vertex;

                double lat = coord.Latitude;
                double lon = coord.Longitude;
                if (lat < bbox.South) bbox.South = lat;
                if (lon < bbox.West) bbox.West = lon;
                if (lat > bbox.North) bbox.North = lat;
                if (lon > bbox.East) bbox.East = lon;
                // Save the min positive and max negative longitude for
                // use in the transmeridian case
                if (lon > 0 && lon < minPosLon){ minPosLon = lon;}
                if (lon < 0 && lon > maxNegLon){ maxNegLon = lon;}
                // check for arcs > 180 degrees longitude, flagging as transmeridian
                if (Math.Abs(lon - next.Longitude) >Constants. M_PI)
                {
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
        /// Whether the winding order of a given LinkedGeoLoop is clockwise
        /// </summary>
        /// <param name="loop">The loop to check</param>
        /// <param name="isTransmeridian">Whether the loop crosses the antimeridian</param>
        /// <returns>Whether the loop is clockwise</returns>
        private static bool IsClockwiseNormalizedLinkedGeoLoop(LinkedGeoLoop loop, bool isTransmeridian)
        {
            double sum = 0;

            LinkedGeoCoord currentCoord = null;

            while (true)
            {
                currentCoord = currentCoord == null
                                   ? loop.First
                                   : currentCoord.Next;
                if (currentCoord == null)
                {
                    break;
                }
            
                var a = currentCoord.Vertex;
                var nextCoord = currentCoord.Next ?? loop.First;
                var b = nextCoord.Vertex;
                // If we identify a transmeridian arc (> 180 degrees longitude),
                // start over with the transmeridian flag set
                if (!isTransmeridian && Math.Abs(a.Longitude - b.Longitude) > Constants.M_PI)
                {
                    return IsClockwiseNormalizedLinkedGeoLoop(loop, true);
                }
                sum += ((NORMALIZE_LON(b.Longitude, isTransmeridian) -
                         NORMALIZE_LON(a.Longitude, isTransmeridian)) *
                        (b.Latitude + a.Latitude));
            }

            return sum > 0;
        }

        /// <summary>
        /// Whether the winding order of a given loop is clockwise. In GeoJSON,
        /// clockwise loops are always inner loops (holes).
        /// </summary>
        /// <param name="loop">The loop to check</param>
        /// <returns>Whether the loop is clockwise</returns>
        public static bool IsClockwiseLinkedGeoLoop(LinkedGeoLoop loop)
        {
            return IsClockwiseNormalizedLinkedGeoLoop(loop, false);
        }
    }
}
