using System;
using System.Collections.Generic;
using System.Linq;

namespace H3Lib.Extensions
{
    public static class NewLinkedGeoLoopExtensions
    {
        public static bool PointInside(this NewLinkedGeoLoop geoLoop, BBox box, GeoCoord coord)
        {
            // fail fast if we're outside the bounding box
            if(!box.Contains(coord))
            {
                return false;
            }
            
            //  Gonna add another test here for quick fail, as we need a triangle+ to have an inside
            if (geoLoop.Count < 3)
            {
                return false;
            }

            bool isTransmeridian = box.IsTransmeridian;
            var contains = false;

            double targetLatitude = coord.Latitude;
            double targetLongitude = coord.Longitude.NormalizeLongitude(isTransmeridian);

            var nodes = geoLoop.Nodes;
            for (var idx = 0; idx < nodes.Count; idx++)
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
                    targetLongitude -= Constants.H3.DBL_EPSILON;
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

        public static BBox ToBBox(this NewLinkedGeoLoop geoLoop)
        {
            if (geoLoop.IsEmpty)
            {
                return new BBox();
            }

            var box = new BBox(-double.MaxValue, double.MaxValue, -double.MaxValue, double.MaxValue);
            var minPosLon = double.MaxValue;
            double maxNegLon = -double.MaxValue;
            var isTransmeridian = false;

            var nodes = geoLoop.Nodes;
            for (int idx = 0; idx < nodes.Count; idx++)
            {
                var coord = nodes[idx];
                var next = nodes[(idx + 1) % nodes.Count];

                double lat = coord.Latitude;
                double lon = coord.Longitude;
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
                if (Math.Abs(lon - next.Longitude) > Constants.H3.M_PI)
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
        
        public static bool IsClockwiseNormalized(this NewLinkedGeoLoop geoLoop, bool isTransmeridian)
        {
            double sum = 0;

            var nodes = geoLoop.Nodes;
            for (int idx = 0; idx < nodes.Count; idx++)
            {
                var a = nodes[idx];
                var b = nodes[(idx + 1) % nodes.Count];

                // If we identify a transmeridian arc (> 180 degrees longitude),
                // start over with the transmeridian flag set
                if (!isTransmeridian && Math.Abs(a.Longitude - b.Longitude) > Constants.H3.M_PI)
                {
                    return geoLoop.IsClockwiseNormalized(true);
                }
                sum += (b.Longitude.NormalizeLongitude(isTransmeridian) -
                        a.Longitude.NormalizeLongitude( isTransmeridian)) *
                       (b.Latitude + a.Latitude);
            }

            return sum > 0;
        }

        public static bool IsClockwise(this NewLinkedGeoLoop geoLoop)
        {
            return geoLoop.IsClockwiseNormalized(false);
        }

        public static int CountContainers
            (this NewLinkedGeoLoop geoLoop, IList<NewLinkedGeoPolygon> polygons, IList<BBox> bboxes)
        {
            return polygons
                  .Where((t, i) => 
                             geoLoop != t.First &&
                             t.First.PointInside(bboxes[i], geoLoop.First.Vertex))
                  .Count();

            // var containerCount = 0;
            // for (var i = 0; i < polygons.Count; i++)
            // {
            //     if (geoLoop != polygons[i].First &&
            //         polygons[i].First.PointInside(bboxes[i], geoLoop.First.Vertex))
            //     {
            //         containerCount++;
            //     }
            // }
            // return containerCount;
        }

        public static NewLinkedGeoPolygon FindDeepestContainer
            (this NewLinkedGeoLoop geoLoop, IList<NewLinkedGeoPolygon> polygons, IList<BBox> bboxes)
        {
            // Set the initial return value to the first candidate
            var parent = polygons.Count > 0
                             ? polygons[0]
                             : null;
            if (polygons.Count <= 1)
            {
                return parent;
            }

            // If we have multiple polygons, they must be nested inside each other.
            // Find the innermost polygon by taking the one with the most containers
            // in the list.
            int max = -1;
            foreach (var poly in polygons)
            {
                int count = poly.First.CountContainers(polygons, bboxes);

                if (count <= max)
                {
                    continue;
                }
                parent = poly;
                max = count;
            }
            return parent;
        }

        public static NewLinkedGeoPolygon FindPolygonForHole
            (this NewLinkedGeoLoop geoLoop, in NewLinkedGeoPolygon polygon, IList<BBox> bboxes)
        {
            if (polygon == null || polygon.CountPolygons == 0)
            {
                return null;
            }

            // Initialize arrays for candidate loops and their bounding boxes
            var candidates = new List<NewLinkedGeoPolygon>();
            var candidateBBoxes = new List<BBox>();

            // Find all polygons that contain the loop
            var polyList = polygon.LinkedPolygons;
            var index = 0;
            foreach (var geoPolygon in polyList)
            {
                if (geoPolygon.First.PointInside(bboxes[index], geoLoop.First.Vertex))
                {
                    candidates.Add(geoPolygon);
                    candidateBBoxes.Add(bboxes[index]);
                }

                index++;
            }
            // The most deeply nested container is the immediate parent
            var parent = geoLoop.FindDeepestContainer(candidates, candidateBBoxes);

            return parent;
        }
        

    }
    
    
}
