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
using System.Diagnostics;
using System.Linq;

namespace h3net.API
{
    /// <summary>
    /// Functions for working with lat/lon coordinates.
    /// </summary>
    /// <!-- Based off 3.1.1 -->
    [DebuggerDisplay("Lat: {lat} Lon: {lon}")]
    public class GeoCoord
    {
        public double lat;
        public double lon;

        public GeoCoord(double _lat, double _lon)
        {
            lat = _lat;
            lon = _lon;
        }

        public GeoCoord()
        {
        }

        /// <summary>
        /// Normalizes radians to a value between 0.0 and two PI.
        /// </summary>
        /// <param name="rads">The input radans value</param>
        /// <returns>The normalized radians value</returns>
        /// <!-- Based off 3.1.1 -->
        public static double _posAngleRads(double rads)
        {
            var tmp = rads < 0.0
                ? rads + Constants.M_2PI
                : rads;
            if (rads >= Constants.M_2PI)
            {
                tmp -= Constants.M_2PI;
            }

            return tmp;
        }

        /// <summary>
        /// Determines if the components of two spherical coordinates are within some
        /// threshold distance of each other.
        /// </summary>
        /// <param name="p1">The first spherical coordinates</param>
        /// <param name="p2">The second spherical coordinates</param>
        /// <param name="threshold">The threshold distance</param>
        /// <returns>
        /// Whether or not the two coordinates are within the threshold distance
        /// of each other
        /// </returns>
        /// <!-- Based off 3.1.1 -->
        public static bool geoAlmostEqualThreshold(GeoCoord p1, GeoCoord p2, double threshold)
        {
            return Math.Abs(p1.lat - p2.lat) < threshold &&
                   Math.Abs(p1.lon - p2.lon) < threshold;
        }

        /// <summary>
        /// Determines if the components of two spherical coordinates are within our
        /// standard epsilon distance of each other.
        /// </summary>
        /// <param name="p1">The first spherical coordinates.</param>
        /// <param name="p2">The second spherical coordinates.</param>
        /// <returns>
        ///  Whether or not the two coordinates are within the epsilon distance
        /// of each other.
        /// </returns>
        /// <!-- Based off 3.1.1 -->
        public static bool geoAlmostEqual(GeoCoord v1, GeoCoord v2)
        {
            return geoAlmostEqualThreshold(v1, v2, Constants.EPSILON_RAD);
        }

        /// <summary>
        /// Set the components of spherical coordinates in decimal degrees.
        /// </summary>
        /// <param name="p">The spherical coordinates</param>
        /// <param name="latDegs">The desired latitude in decimal degrees</param>
        /// <param name="lonDegs">The desired longitude in decimal degrees</param>
        /// <!-- Based off 3.1.1 -->
        public static void setGeoDegs(ref GeoCoord p, double latDegs, double lonDegs)
        {
            _setGeoRads(ref p, degsToRads(latDegs), degsToRads(lonDegs));

        }

        /// <summary>
        /// Set the components of spherical coordinates in radians.
        /// </summary>
        /// <param name="p">The spherical coordinates</param>
        /// <param name="latDegs">The desired latitude in decimal radians</param>
        /// <param name="lonDegs">The desired longitude in decimal radians</param>
        /// <!-- Based off 3.1.1 -->
        public static void _setGeoRads(ref GeoCoord p, double latRads, double lonRads)
        {
            p.lat = latRads;
            p.lon = lonRads;
        }

        /// <summary>
        /// Convert from decimal degrees to radians.
        /// </summary>
        /// <param name="degrees">The decimal degrees</param>
        /// <returns>The corresponding radians</returns>
        /// <!-- Based off 3.1.1 -->
        public static double degsToRads(double degrees)
        {
            return degrees * Constants.M_PI_180;
        }

        /// <summary>
        /// Convert fro radians to decimal degrees.
        /// </summary>
        /// <param name="radians">The radians</param>
        /// <returns>The corresponding decimal degrees</returns>
        /// <!-- Based off 3.1.1 -->
        public static double radsToDegs(double radians)
        {
            return radians * Constants.M_180_PI;
        }

        /// <summary>
        /// Makes sure latitudes are in the proper bounds
        /// </summary>
        /// <param name="lat">The original lat value</param>
        /// <returns>The corrected lat value</returns>
        /// <!-- Based off 3.1.1 -->
        public static double constrainLat(double lat)
        {
            while (lat > Constants.M_PI_2)
            {
                lat -= Constants.M_PI;
            }

            return lat;
        }

        /// <summary>
        /// Makes sure longitudes are in the proper bounds
        /// </summary>
        /// <param name="lng">The origin lng value</param>
        /// <returns>The corrected lng value</returns>
        /// <!-- Based off 3.1.1 -->
        public static double constrainLng(double lng)
        {
            while (lng > Constants.M_PI)
            {
                lng = lng - (2 * Constants.M_PI);
            }

            while (lng < -Constants.M_PI)
            {
                lng = lng + (2 * Constants.M_PI);
            }

            return lng;
        }

        /// <summary>
        /// Find the great circle distance in radians between two spherical coordinates.
        /// </summary>
        /// <param name="p1">The first spherical coordinates</param>
        /// <param name="p2">The second spherical coordinates</param>
        /// <returns>The great circle distance between p1 and p2</returns>
        /// <!-- Based off 3.1.1 -->
        public static double _geoDistRads(GeoCoord p1, GeoCoord p2)
        {
            // use spherical triangle with p1 at A, p2 at B, and north pole at C
            double bigC = Math.Abs(p2.lon - p1.lon);
            if (bigC > Constants.M_PI) // assume we want the complement
            {
                // note that in this case they can't both be negative
                double lon1 = p1.lon;
                if (lon1 < 0.0) lon1 += 2.0 * Constants.M_PI;
                double lon2 = p2.lon;
                if (lon2 < 0.0) lon2 += 2.0 * Constants.M_PI;

                bigC = Math.Abs(lon2 - lon1);
            }

            double b = Constants.M_PI_2 - p1.lat;
            double a = Constants.M_PI_2 - p2.lat;

            // use law of cosines to find c
            double cosc = Math.Cos(a) * Math.Cos(b) + Math.Sin(a) * Math.Sin(b) * Math.Cos(bigC);
            if (cosc > 1.0)
            {
                cosc = 1.0;
            }

            if (cosc < -1.0)
            {
                cosc = -1.0;
            }

            return Math.Acos(cosc);
        }

        /// <summary>
        /// Find the great circle distance in kilometers between two spherical
        /// coordinates
        /// </summary>
        /// <param name="p1">The first spherical coordinates</param>
        /// <param name="p2">The distance in kilometers between p1 and p2</param>
        /// <!-- Based off 3.1.1 -->
        public static  double _geoDistKm(GeoCoord p1, IEnumerable<GeoCoord> p2)
        {
            return Constants.EARTH_RADIUS_KM * _geoDistRads(p1, p2.First( ));
        }

        /// <summary>
        /// Determines the azimuth to p2 from p1 in radians
        /// </summary>
        /// <param name="p1">The first spherical coordinates</param>
        /// <param name="p2">The second spherical coordinates</param>
        /// <returns>The azimuth in radians from p1 to p2</returns>
        /// <!-- Based off 3.1.1 -->
        public static double _geoAzimuthRads(GeoCoord p1, GeoCoord p2)
        {
            return
                Math.Atan2
                    (
                     Math.Cos(p2.lat) * Math.Sin(p2.lon - p1.lon),
                     Math.Cos(p1.lat) * Math.Sin(p2.lat) -
                     Math.Sin(p1.lat) * Math.Cos(p2.lat) * Math.Cos(p2.lon - p1.lon)
                    );
        }

        /// <summary>
        /// Computes the point on the sphere a specified azimuth and distance from
        /// another point.
        /// </summary>
        /// <param name="p1">The first spherical coordinates.</param>
        /// <param name="az">The desired azimuth from p1.</param>
        /// <param name="distance">The desired distance from p1, must be non-negative.</param>
        /// <param name="p2">
        /// The spherical coordinates at the desired azimuth and distance from p1.
        /// </param>
        /// <!-- Based off 3.1.1 -->
        public static void _geoAzDistanceRads(ref GeoCoord p1, double az, double distance, ref GeoCoord p2)
        {
            if (distance < Constants.EPSILON) {
                p2 = p1;
                return;
            }

            double sinlat, sinlon, coslon;

            az = _posAngleRads(az);

            // check for due north/south azimuth
            if (az < Constants.EPSILON || Math.Abs(az - Constants.M_PI) <Constants. EPSILON) {
                if (az < Constants.EPSILON)  // due north
                    p2.lat = p1.lat + distance;
                else  // due south
                    p2.lat = p1.lat - distance;

                if (Math.Abs(p2.lat - Constants.M_PI_2) < Constants.EPSILON)  // north pole
                {
                    p2.lat = Constants.M_PI_2;
                    p2.lon = 0.0;
                } else if (Math.Abs(p2.lat + Constants.M_PI_2) < Constants.EPSILON)  // south pole
                {
                    p2.lat = -Constants.M_PI_2;
                    p2.lon = 0.0;
                } else
                    p2.lon = constrainLng(p1.lon);
            } else  // not due north or south
            {
                sinlat = Math.Sin(p1.lat) * Math.Cos(distance) +
                         Math.Cos(p1.lat) * Math.Sin(distance) * Math.Cos(az);
                if (sinlat > 1.0) sinlat = 1.0;
                if (sinlat < -1.0) sinlat = -1.0;
                p2.lat =Math.Asin(sinlat);
                if (Math.Abs(p2.lat - Constants.M_PI_2) < Constants.EPSILON)  // north pole
                {
                    p2.lat = Constants.M_PI_2;
                    p2.lon = 0.0;
                } else if (Math.Abs(p2.lat + Constants.M_PI_2) < Constants.EPSILON)  // south pole
                {
                    p2.lat = -Constants.M_PI_2;
                    p2.lon = 0.0;
                } else {
                    sinlon = Math.Sin(az) * Math.Sin(distance) / Math.Cos(p2.lat);
                    coslon = (Math.Cos(distance) - Math.Sin(p1.lat) * Math.Sin(p2.lat)) /
                             Math.Cos(p1.lat) / Math.Cos(p2.lat);
                    if (sinlon > 1.0) {sinlon = 1.0;}
                    if (sinlon < -1.0) {sinlon = -1.0;}
                    if (coslon > 1.0) {sinlon = 1.0;}
                    if (coslon < -1.0) {sinlon = -1.0;}
                    p2.lon = constrainLng(p1.lon + Math.Atan2(sinlon, coslon));
                }
            }
        }

        /*
         * The following functions provide meta information about the H3 hexagons at
         * each zoom level. Since there are only 16 total levels, these are current
         * handled with hardwired static values, but it may be worthwhile to put these
         * static values into another file that can be autogenerated by source code in
         * the future.
         */
        public static double hexAreaKm2(int res)
        {
            double[] areas = {
                4250546.848, 607220.9782, 86745.85403, 12392.26486,
                1770.323552, 252.9033645, 36.1290521,  5.1612932,
                0.7373276,   0.1053325,   0.0150475,   0.0021496,
                0.0003071,   0.0000439,   0.0000063,   0.0000009};
            return areas[res];
        }

        public static double hexAreaM2(int res)
        {
            double[] areas = {
                4.25055E+12, 6.07221E+11, 86745854035, 12392264862,
                1770323552,  252903364.5, 36129052.1,  5161293.2,
                737327.6,    105332.5,    15047.5,     2149.6,
                307.1,       43.9,        6.3,         0.9};
            return areas[res];
        }

        public static double edgeLengthKm(int res)
        {
            double[] lens = {
                1107.712591, 418.6760055, 158.2446558, 59.81085794,
                22.6063794,  8.544408276, 3.229482772, 1.220629759,
                0.461354684, 0.174375668, 0.065907807, 0.024910561,
                0.009415526, 0.003559893, 0.001348575, 0.000509713

            };
            return lens[res];
        }

        public static double edgeLengthM(int res)
        {
            double[] lens = {
                1107712.591, 418676.0055, 158244.6558, 59810.85794,
                22606.3794,  8544.408276, 3229.482772, 1220.629759,
                461.3546837, 174.3756681, 65.90780749, 24.9105614,
                9.415526211, 3.559893033, 1.348574562, 0.509713273
            };
            return lens[res];
        }

        /// <summary>
        /// Number of unique valid H3Indexes at given resolution.
        /// </summary>
        /// <!-- Based off 3.1.1 -->
        public static long numHexagons(int res) {
            long[] nums =
            {
                122L,
                842L,
                5882L,
                41162L,
                288122L,
                2016842L,
                14117882L,
                98825162L,
                691776122L,
                4842432842L,
                33897029882L,
                237279209162L,
                1660954464122L,
                11626681248842L,
                81386768741882L,
                569707381193162L
            };
            return nums[res];
        }
    }
}
