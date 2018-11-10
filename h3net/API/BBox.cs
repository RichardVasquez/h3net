/*
 * Copyright 2018, Richard Vasquez
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *         http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 *
 * Original version written in C, Copyright 2016-2017 Uber Technologies, Inc.
 * C version licensed under the Apache License, Version 2.0 (the "License");
 * C Source code available at: https://github.com/uber/h3
 *
 */
using System;
using System.Collections.Generic;
using System.Linq;

namespace h3net.API
{
    /// <summary>
    /// Geographic bounding box with coordinates defined in radians
    /// </summary>
    /// <!-- Based off 3.1.1 -->
    public class BBox
    {
        public double north;
        public double south;
        public double east;
        public double west;

        /// <summary>
        /// Create a bounding box from a simple polygon defined as an array of vertices.
        ///
        /// Known limitations:
        /// - Does not support polygons with two adjacent points &gt; 180 degrees of
        ///   longitude apart. These will be interpreted as crossing the antimeridian.
        /// - Does not currently support polygons containing a pole.
        /// </summary>
        /// <param name="verts">Array of vertices</param>
        /// <param name="numVerts">Number of vertices</param>
        /// <param name="bbox">Output box</param>
        /// <!-- Based off 3.1.1 -->
        static void bboxFromVertices(List<GeoCoord> verts, int numVerts, ref BBox bbox) 
        {
            // Early exit if there are no vertices
            if (numVerts == 0) {
                bbox.north = 0;
                bbox.south = 0;
                bbox.east = 0;
                bbox.west = 0;
                return;
            }
            double lat;
            double lon;

            bbox.south = double.MaxValue;
            bbox.west = double.MaxValue;
            bbox.north = -double.MaxValue;
            bbox.east = -double.MaxValue;
            bool isTransmeridian = false;

            for (int i = 0; i < numVerts; i++) {
                lat = verts[i].lat;
                lon = verts[i].lon;
                if (lat < bbox.south) bbox.south = lat;
                if (lon < bbox.west) bbox.west = lon;
                if (lat > bbox.north) bbox.north = lat;
                if (lon > bbox.east) bbox.east = lon;
                // check for arcs > 180 degrees longitude, flagging as transmeridian
                if (Math.Abs( lon - verts[(i + 1) % numVerts].lon) > Constants.M_PI)
                {
                    isTransmeridian = true;
                }
            }
            // Swap east and west if transmeridian
            if (isTransmeridian)
            {
                double tmp = bbox.east;
                bbox.east = bbox.west;
                bbox.west = tmp;
            }
        }

        /// <summary>
        /// Create a bounding box from a Geofence
        /// </summary>
        /// <param name="Geofence">Input <see cref="Geofence"/></param>
        /// <param name="bbox">Output bbox</param>
        /// <!-- Based off 3.1.1 -->
        public static void bboxFromGeofence(Geofence Geofence, ref BBox bbox) {
            bboxFromVertices(Geofence.verts.ToList() , Geofence.numVerts, ref bbox);
        }

        /// <summary>
        /// Create a bounding box from a GeoPolygon
        /// </summary>
        /// <param name="polygon">Input <see cref="GeoPolygon"/></param>
        /// <param name="bboxes">Output bboxes, one for the outer loop and one for each hole</param>
        /// <!-- Based off 3.1.1 -->
        void bboxesFromGeoPolygon(GeoPolygon polygon, ref List<BBox> bboxes)
        {
            var bb = bboxes[0];
            bboxFromGeofence(polygon.Geofence, ref bb);
            bboxes[0] = bb;
            for (int i = 0; i < polygon.numHoles; i++)
            {
                bb = bboxes[i + 1];
                bboxFromGeofence(polygon.holes[i], ref bb);
                bboxes[i + 1] = bb;
            }
        }

        /// <summary>
        /// Whether the given bounding box cross the antimeridian
        /// </summary>
        /// <param name="bbox">bounding box to inspect</param>
        /// <returns>true if transmeridian</returns>
        /// <!-- Based off 3.1.1 -->
        public static bool bboxIsTransmeridian(BBox bbox)
        {
            return bbox.east < bbox.west;
        }

        /// <summary>
        /// Gets the center of a bounding box
        /// </summary>
        /// <param name="bbox">Input bounding box</param>
        /// <param name="center">Output center coordinate</param>
        /// <!-- Based off 3.1.1 -->
        public static void bboxCenter(BBox bbox, ref GeoCoord center)
        {
            center.lat = (bbox.north + bbox.south) / 2.0;
            // If the bbox crosses the antimeridian, shift east 360 degrees
            double east = bboxIsTransmeridian(bbox)
                ? bbox.east + Constants.M_2PI
                : bbox.east;
            center.lon = GeoCoord.constrainLng((east + bbox.west) / 2.0);
        }

        /// <summary>
        /// Whether the bounding box contains a given point
        /// </summary>
        /// <param name="bbox">Bounding box</param>
        /// <param name="point">Point to test</param>
        /// <returns>true is point is contained</returns>
        /// <!-- Based off 3.1.1 -->
        public static bool bboxContains(BBox bbox, GeoCoord point)
        {
            return 
                point.lat >= bbox.south && 
                point.lat <= bbox.north &&
                (
                    bboxIsTransmeridian(bbox)
                    // transmeridian case
                    ? point.lon >= bbox.west || point.lon <= bbox.east
                    // standard case
                    : point.lon >= bbox.west && point.lon <= bbox.east
                );
        }

        /// <summary>
        /// Determines if two bounding boxes are strictly equal
        /// </summary>
        /// <param name="b1">Bounding box 1</param>
        /// <param name="b2">Bounding box 2</param>
        /// <returns>True if the boxes are equal</returns>
        /// <!-- Based off 3.1.1 -->
        public static bool bboxEquals(BBox b1, BBox b2)
        {
            return Math.Abs(b1.north - b2.north) < Constants.EPSILON &&
                   Math.Abs(b1.south - b2.south) < Constants.EPSILON &&
                   Math.Abs(b1.east - b2.east) < Constants.EPSILON &&
                   Math.Abs(b1.west - b2.west) < Constants.EPSILON;
        }

        /// <summary>
        /// Returns the radius of a given hexagon in kilometers
        /// </summary>
        /// <param name="h3Index">Index of the hexagon</param>
        /// <returns>radius of hexagon in kilometers</returns>
        /// <!-- Based off 3.1.1 -->
        static double _hexRadiusKm(H3Index h3Index)
        {
            // There is probably a cheaper way to determine the radius of a
            // hexagon, but this way is conceptually simple
            GeoCoord h3Center = new GeoCoord();
            GeoBoundary h3Boundary = new GeoBoundary();
            H3Index.h3ToGeo(h3Index, ref h3Center);
            H3Index.h3ToGeoBoundary(h3Index, ref h3Boundary);
            return GeoCoord._geoDistKm(h3Center,   h3Boundary.verts);
        }

        /// <summary>
        /// Radius of bounding box in hexagons.  i.e., the radius of a k-ring centered
        /// on the bbox center and covering the entire bbox.
        /// </summary>
        /// <param name="bbox">Bounding box to measure</param>
        /// <param name="res">Resolution of hexagons to use in measurement</param>
        /// <returns>Radius in hexagons</returns>
        /// <!-- Based off 3.1.1 -->
        public static int bboxHexRadius(BBox bbox, int res)
        {
            // Determine the center of the bounding box
            GeoCoord center = new GeoCoord();
            bboxCenter(bbox, ref center);

            // Use a vertex on the side closest to the equator, to ensure the longest
            // radius in cases with significant distortion. East/west is arbitrary.
            double lat =
                Math.Abs( bbox.north) > Math.Abs(bbox.south)
                    ? bbox.south
                    : bbox.north;

            GeoCoord vertex = new GeoCoord {lat = lat, lon = bbox.east};

            // Determine the length of the bounding box "radius" to then use
            // as a circle on the earth that the k-rings must be greater than
            double bboxRadiusKm = GeoCoord._geoDistKm(center, new []{vertex});

            // Determine the radius of the center hexagon
            double centerHexRadiusKm = _hexRadiusKm(H3Index.geoToH3(ref center, res));

            // The closest point along a hexagon drawn through the center points
            // of a k-ring aggregation is exactly 1.5 radii of the hexagon. For
            // any orientation of the GeoJSON encased in a circle defined by the
            // bounding box radius and center, it is guaranteed to fit in this k-ring
            // Rounded *up* to guarantee containment
            return (int)Math.Ceiling(bboxRadiusKm / (1.5 * centerHexRadiusKm));
        }
    }
}
