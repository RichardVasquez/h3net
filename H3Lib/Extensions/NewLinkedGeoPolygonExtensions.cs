using System.Collections.Generic;
using System.Linq;

namespace H3Lib.Extensions
{
    public static class NewLinkedGeoPolygonExtensions
    {
        public static (int, NewLinkedGeoPolygon) NormalizeMultiPolygon(this NewLinkedGeoPolygon root)
        {
            // We assume that the input is a single polygon with loops;
            // if it has multiple polygons, don't touch it
            if (root.Next != null)
            {
                return (Constants.LinkedGeo.NormalizationErrMultiplePolygons, root);
            }

            
            // Count loops, exiting early if there's only one
            int loopCount = root.CountLoops;
            if (loopCount <= 1)
            {
                return (Constants.LinkedGeo.NormalizationSuccess, root);
            }

            int resultCode = Constants.LinkedGeo.NormalizationSuccess;
            NewLinkedGeoPolygon polygon = null;
            int innerCount = 0;

            // Create an array to hold all of the inner loops. Note that
            // this array will never be full, as there will always be fewer
            // inner loops than outer loops.
            var innerLoops = new List<NewLinkedGeoLoop>();
            //Enumerable.Range(1, loopCount).Select(s => new NewLinkedGeoLoop()).ToList();
            // Create an array to hold the bounding boxes for the outer loops
            var bboxes = new List<BBox>();
            //Enumerable.Range(1, loopCount).Select(s => new BBox()).ToList();

            
            //  Get the first loop and unlink it from root
            //  In C, this feels hinky, as we're zeroing out the root.
            //  I hope we rebuild it.
            var loops = root.Loops;
            root = new NewLinkedGeoPolygon();

            foreach (var loop in loops)
            {
                if (loop.IsClockwise())
                {
                    innerLoops.Add(loop);
                    innerCount++;
                } else
                {
                    polygon = polygon == null
                                  ? root
                                  : polygon.AddNewLinkedGeoPolygon();

                    polygon.AddLinkedLoop(loop);
                    bboxes.Add(loop.ToBBox());
                }
            }

            // Find polygon for each inner loop and assign the hole to it
            for (var i = 0; i < innerCount; i++)
            {
                polygon = innerLoops[i].FindPolygonForHole(in root, bboxes);

                if (polygon!=null)
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

            foreach (var loop in innerLoops)
            {
                loop.Clear();
            }

            return (resultCode, polygon);
        }
    }
}
