using System;
using System.Collections.Generic;
using System.Linq;

namespace H3Lib.Extensions
{
    /// <summary>
    /// Operations on LinkedGeoPolygon type
    /// </summary>
    public static class LinkedGeoPolygonExtensions
    {
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
        /// <returns>
        /// Tuple
        /// Item1 - 0 on success, or an error code > 0 for invalid input
        /// Item2 - Normalized LinkedGeoPolygon
        /// </returns>
        /// <remarks>
        /// 3.7.1
        /// linkedGeo.c
        /// int normalizeMultiPolygon
        /// </remarks>
        public static (int, LinkedGeoPolygon) NormalizeMultiPolygon(this LinkedGeoPolygon root)
        {
            // We assume that the input is a single polygon with loops;
            // if it has multiple polygons, don't touch it
            if (root.Next != null)
            {
                //  TODO: Check the constant location and update
                return (Constants.LinkedGeo.NormalizationErrMultiplePolygons, root);
            }

            // Count loops, exiting early if there's only one
            int loopCount = root.CountLoops;
            if (loopCount <= 1)
            {
                return (Constants.LinkedGeo.NormalizationSuccess, root);
            }

            int resultCode = Constants.LinkedGeo.NormalizationSuccess;
            LinkedGeoPolygon polygon = null;
            var innerCount = 0;
            var outerCount = 0;


            // Create an array to hold all of the inner loops. Note that
            // this array will never be full, as there will always be fewer
            // inner loops than outer loops.
            var innerLoops = new List<LinkedGeoLoop>();
            // Create an array to hold the bounding boxes for the outer loops
            var bboxes = new List<BBox>();

            // Get the first loop and unlink it from root
            var testLoops = root.Loops;//.GeoLoopList.First;

            root = new LinkedGeoPolygon();

            foreach (var geoLoop in testLoops)
            {
                if (geoLoop.IsClockwise())
                {
                    innerLoops.Add(geoLoop);
                    innerCount++;
                }
                else
                {
                    polygon = polygon == null
                                  ? root
                                  : polygon.AddNewLinkedGeoPolygon();
                    polygon.AddLinkedLoop(geoLoop);
                    bboxes.Add(geoLoop.ToBBox());
                    outerCount++;
                }
            }

            // Find polygon for each inner loop and assign the hole to it
            for (var i = 0; i < innerCount; i++)
            {
                polygon = innerLoops[i].FindPolygonForHole(root, bboxes, outerCount);
                if (polygon != null)
                {
                    polygon.AddLinkedLoop(innerLoops[i]);
                }
                else
                {
                    // If we can't find a polygon (possible with invalid input), then
                    // we need to release the memory for the hole, because the loop has
                    // been unlinked from the root and the caller will no longer have
                    // a way to destroy it with destroyLinkedPolygon.
                    innerLoops[i].Clear();
                    resultCode = Constants.LinkedGeo.NormalizationErrUnassignedHoles;
                }
            }
            // Free allocated memory
            innerLoops.Clear();
            bboxes.Clear();

            return (resultCode, root);
        }

        /// <summary>
        /// Find the polygon to which a given hole should be allocated. Note that this
        /// function will return null if no parent is found.
        /// </summary>
        /// <param name="loop">Inner loop describing a hole</param>
        /// <param name="polygon">Head of a linked list of polygons to check</param>
        /// <param name="boxes">Bounding boxes for polygons, used in point-in-poly check</param>
        /// <param name="polygonCount">Number of polygons to check</param>
        /// <returns>Pointer to parent polygon, or null if not found</returns>
        /// <remarks>
        /// 3.7.1
        /// linkedGeo.c
        /// static const LinkedGeoPolygon* findPolygonForHole
        /// </remarks>
        private static LinkedGeoPolygon FindPolygonForHole(
                this LinkedGeoLoop loop, LinkedGeoPolygon polygon, List<BBox> boxes,
                int polygonCount
            )
        {
            // Early exit with no polygons
            if (polygonCount == 0)
            {
                return null;
            }
            // Initialize arrays for candidate loops and their bounding boxes
            var candidates = new List<LinkedGeoPolygon>();
            var candidateBoxes = new List<BBox>();
            
            // Find all polygons that contain the loop
            var index = 0;
            while (polygon != null)
            {
                // We are guaranteed not to overlap, so just test the first point
                if(polygon.Loops !=null && loop.Nodes!=null)
                {
                    if (polygon.Loops.First().PointInside(boxes[index], loop.Nodes.First().Vertex))
                    {
                        candidates.Add(polygon);
                        candidateBoxes.Add(boxes[index]);
                    }
                }
                polygon = polygon.Next;
                index++;
            }

            // The most deeply nested container is the immediate parent
            var parent = candidates.FindDeepestContainer(candidateBoxes);

            // Free allocated memory
            candidates.Clear();
            candidateBoxes.Clear();
            return (parent == null || parent.CountLoops == 0)
                       ? null
                       : parent;
        }
    }
}
