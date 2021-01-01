using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using H3Lib.Extensions;

namespace H3Lib
{
    /// <summary>
    /// Functions for working with lat/lon coordinates.
    /// </summary>
    [DebuggerDisplay("Lat: {Latitude} Lon: {Longitude}")]
    public class GeoCoord
    {
        public double Latitude;
        public double Longitude;

        public GeoCoord(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        public GeoCoord()
        {
            Latitude = 0;
            Longitude = 0;
        }

        /// <summary>
        /// Normalizes radians to a value between 0.0 and wo PI
        /// </summary>
        /// <param name="rads">Input radians value</param>
        /// <returns>normalized radians value</returns>
        private double _posAngleRads(double rads)
        {
            double tmp = rads < 0.0
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
        public static bool geoAlmostEqualThreshold(GeoCoord p1, GeoCoord p2, double threshold)
        {
            return Math.Abs(p1.Latitude - p2.Latitude) < threshold &&
                   Math.Abs(p1.Longitude - p2.Longitude) < threshold;
        }

        /// <summary>
        /// Determines if the components of two spherical coordinates are within our
        /// standard epsilon distance of each other.
        /// </summary>
        /// <param name="v1">The first spherical coordinates.</param>
        /// <param name="v1">The second spherical coordinates.</param>
        /// <returns>
        ///  Whether or not the two coordinates are within the epsilon distance
        /// of each other.
        /// </returns>
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
        public static void setGeoDegs(ref GeoCoord p, double latDegs, double lonDegs)
        {
            _setGeoRads(ref p, DegreesToRadians(latDegs), DegreesToRadians(lonDegs));

        }

        /// <summary>
        /// Set the components of spherical coordinates in radians.
        /// </summary>
        /// <param name="p">The spherical coordinates</param>
        /// <param name="latDegs">The desired latitude in decimal radians</param>
        /// <param name="lonDegs">The desired longitude in decimal radians</param>
        public static void _setGeoRads(ref GeoCoord p, double latRads, double lonRads)
        {
            p.Latitude = latRads;
            p.Longitude = lonRads;
        }

        /// <summary>
        /// Convert from decimal degrees to radians.
        /// </summary>
        /// <param name="degrees">The decimal degrees</param>
        /// <returns>The corresponding radians</returns>
        public static double DegreesToRadians(double degrees)
        {
            return degrees * Constants.M_PI_180;
        }

        /// <summary>
        /// Convert fro radians to decimal degrees.
        /// </summary>
        /// <param name="radians">The radians</param>
        /// <returns>The corresponding decimal degrees</returns>
        public static double RadiansToDegrees(double radians)
        {
            return radians * Constants.M_180_PI;
        }

        /// <summary>
        /// Makes sure latitudes are in the proper bounds
        /// </summary>
        /// <param name="latitude">The original lat value</param>
        /// <returns>The corrected lat value</returns>
        public static double ConstrainLatitude(double latitude)
        {
            while (latitude > Constants.M_PI_2)
            {
                latitude -= Constants.M_PI;
            }

            return latitude;
        }

        /// <summary>
        /// Makes sure longitudes are in the proper bounds
        /// </summary>
        /// <param name="longitude">The origin lng value</param>
        /// <returns>The corrected lng value</returns>
        public static double ConstrainLongitude(double longitude)
        {
            while (longitude > Constants.M_PI)
            {
                longitude -= (2 * Constants.M_PI);
            }

            while (longitude < -Constants.M_PI)
            {
                longitude += (2 * Constants.M_PI);
            }

            return longitude;
        }

        /// <summary>
        /// The great circle distance in radians between two spherical coordinates.
        /// This function uses the Haversine formula.
        /// For math details, see:
        ///     https://en.wikipedia.org/wiki/Haversine_formula
        ///     https://www.movable-type.co.uk/scripts/latlong.html
        /// </summary>
        /// <param name="a">the first lat/lng pair (in radians)</param>
        /// <param name="b">the second lat/lng pair (in radians)</param>
        /// <returns>
        /// the great circle distance in radians between a and b
        /// </returns>
        public static double PointDistRads(GeoCoord a, GeoCoord b)
        {
            double sinLat = Math.Sin((b.Latitude - a.Latitude) / 2.0);
            double sinLng = Math.Sin((b.Longitude - a.Longitude) / 2.0);
            double p = sinLat * sinLat + Math.Cos(a.Latitude) * Math.Cos(b.Latitude) * sinLng * sinLng;

            return 2 * Math.Atan2(Math.Sqrt(p), Math.Sqrt(1 - p));
        }

        /// <summary>
        /// The great circle distance in kilometers between two spherical coordinates
        /// </summary>
        /// <param name="a">the first lat/lng pair (in radians)</param>
        /// <param name="b">the second lat/lng pair (in radians)</param>
        public static double PointDistKm(GeoCoord a, GeoCoord b)
        {
            return PointDistRads(a, b) * Constants.EARTH_RADIUS_KM;
        }

        /// <summary>
        /// The great circle distance in meters between two spherical coordinates
        /// </summary>
        /// <param name="a">the first lat/lng pair (in radians)</param>
        /// <param name="b">the second lat/lng pair (in radians)</param>
        public static double PointDistM(GeoCoord a, GeoCoord b)
        {
            return PointDistKm(a, b) * 1000;
        }

        /// <summary>
        /// Find the great circle distance in radians between two spherical coordinates.
        /// </summary>
        /// <param name="p1">The first spherical coordinates</param>
        /// <param name="p2">The second spherical coordinates</param>
        /// <returns>The great circle distance between p1 and p2</returns>
        public static double _geoDistRads(GeoCoord p1, GeoCoord p2)
        {
            // use spherical triangle with p1 at A, p2 at B, and north pole at C
            double bigC = Math.Abs(p2.Longitude - p1.Longitude);
            if (bigC > Constants.M_PI) // assume we want the complement
            {
                // note that in this case they can't both be negative
                double lon1 = p1.Longitude;
                if (lon1 < 0.0) lon1 += 2.0 * Constants.M_PI;
                double lon2 = p2.Longitude;
                if (lon2 < 0.0) lon2 += 2.0 * Constants.M_PI;

                bigC = Math.Abs(lon2 - lon1);
            }

            double b = Constants.M_PI_2 - p1.Latitude;
            double a = Constants.M_PI_2 - p2.Latitude;

            // use law of cosines to find c
            double cosC = Math.Cos(a) * Math.Cos(b) + Math.Sin(a) * Math.Sin(b) * Math.Cos(bigC);
            if (cosC > 1.0)
            {
                cosC = 1.0;
            }

            if (cosC < -1.0)
            {
                cosC = -1.0;
            }

            return Math.Acos(cosC);
        }

        /// <summary>
        /// Find the great circle distance in kilometers between two spherical
        /// coordinates
        /// </summary>
        /// <param name="p1">The first spherical coordinates</param>
        /// <param name="p2">The distance in kilometers between p1 and p2</param>
        /// <remarks>TODO remove?</remarks>
        public static double _geoDistKm(GeoCoord p1, IEnumerable<GeoCoord> p2)
        {
            return Constants.EARTH_RADIUS_KM * _geoDistRads(p1, p2.First());
        }

        /// <summary>
        /// Determines the azimuth to p2 from p1 in radians
        /// </summary>
        /// <param name="p1">The first spherical coordinates</param>
        /// <param name="p2">The second spherical coordinates</param>
        /// <returns>The azimuth in radians from p1 to p2</returns>
        public static double _geoAzimuthRads(GeoCoord p1, GeoCoord p2)
        {
            return
                Math.Atan2
                    (
                     Math.Cos(p2.Latitude) * Math.Sin(p2.Longitude - p1.Longitude),
                     Math.Cos(p1.Latitude) * Math.Sin(p2.Latitude) -
                     Math.Sin(p1.Latitude) * Math.Cos(p2.Latitude) * Math.Cos(p2.Longitude - p1.Longitude)
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
        public static void _geoAzDistanceRads(ref GeoCoord p1, double az, double distance, ref GeoCoord p2)
        {
            if (distance < Constants.EPSILON)
            {
                p2 = p1;
                return;
            }

            double sinlat, sinlon, coslon;

            az = az.NormalizeRadians();// _posAngleRads(az);

            // check for due north/south azimuth
            if (az < Constants.EPSILON || Math.Abs(az - Constants.M_PI) < Constants.EPSILON)
            {
                if (az < Constants.EPSILON) // due north
                {
                    p2.Latitude = p1.Latitude + distance;
                }
                else // due south
                {
                    p2.Latitude = p1.Latitude - distance;
                }

                if (Math.Abs(p2.Latitude - Constants.M_PI_2) < Constants.EPSILON) // north pole
                {
                    p2.Latitude = Constants.M_PI_2;
                    p2.Longitude = 0.0;
                }
                else if (Math.Abs(p2.Latitude + Constants.M_PI_2) < Constants.EPSILON) // south pole
                {
                    p2.Latitude = -Constants.M_PI_2;
                    p2.Longitude = 0.0;
                }
                else
                {
                    p2.Longitude = ConstrainLongitude(p1.Longitude);
                }
            }
            else // not due north or south
            {
                sinlat = Math.Sin(p1.Latitude) * Math.Cos(distance) +
                         Math.Cos(p1.Latitude) * Math.Sin(distance) * Math.Cos(az);
                if (sinlat > 1.0)
                {
                    sinlat = 1.0;
                }

                if (sinlat < -1.0)
                {
                    sinlat = -1.0;
                }
                p2.Latitude = Math.Asin(sinlat);
                if (Math.Abs(p2.Latitude - Constants.M_PI_2) < Constants.EPSILON) // north pole
                {
                    p2.Latitude = Constants.M_PI_2;
                    p2.Longitude = 0.0;
                }
                else if (Math.Abs(p2.Latitude + Constants.M_PI_2) < Constants.EPSILON) // south pole
                {
                    p2.Latitude = -Constants.M_PI_2;
                    p2.Longitude = 0.0;
                }
                else
                {
                    sinlon = Math.Sin(az) * Math.Sin(distance) / Math.Cos(p2.Latitude);
                    coslon = (Math.Cos(distance) - Math.Sin(p1.Latitude) * Math.Sin(p2.Latitude)) /
                             Math.Cos(p1.Latitude) / Math.Cos(p2.Latitude);
                    if (sinlon > 1.0)
                    {
                        sinlon = 1.0;
                    }

                    if (sinlon < -1.0)
                    {
                        sinlon = -1.0;
                    }

                    if (coslon > 1.0)
                    {
                        sinlon = 1.0;
                    }

                    if (coslon < -1.0)
                    {
                        sinlon = -1.0;
                    }

                    p2.Longitude = ConstrainLongitude(p1.Longitude + Math.Atan2(sinlon, coslon));
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
            double[] areas =
            {
                4250546.848, 607220.9782, 86745.85403, 12392.26486,
                1770.323552, 252.9033645, 36.1290521, 5.1612932,
                0.7373276, 0.1053325, 0.0150475, 0.0021496,
                0.0003071, 0.0000439, 0.0000063, 0.0000009
            };
            return areas[res];
        }

        public static double hexAreaM2(int res)
        {
            double[] areas =
            {
                4.25055E+12, 6.07221E+11, 86745854035, 12392264862,
                1770323552, 252903364.5, 36129052.1, 5161293.2,
                737327.6, 105332.5, 15047.5, 2149.6,
                307.1, 43.9, 6.3, 0.9
            };
            return areas[res];
        }

        public static double edgeLengthKm(int res)
        {
            double[] lens =
            {
                1107.712591, 418.6760055, 158.2446558, 59.81085794,
                22.6063794, 8.544408276, 3.229482772, 1.220629759,
                0.461354684, 0.174375668, 0.065907807, 0.024910561,
                0.009415526, 0.003559893, 0.001348575, 0.000509713

            };
            return lens[res];
        }

        public static double edgeLengthM(int res)
        {
            double[] lens =
            {
                1107712.591, 418676.0055, 158244.6558, 59810.85794,
                22606.3794, 8544.408276, 3229.482772, 1220.629759,
                461.3546837, 174.3756681, 65.90780749, 24.9105614,
                9.415526211, 3.559893033, 1.348574562, 0.509713273
            };
            return lens[res];
        }

        /// <summary>
        /// Number of unique valid H3Indexes at given resolution.
        /// </summary>
        /// <param name="res">Resolution to get count of cells</param>
        public static long NumHexagons(int res)
        {
            return 2 + 120 * 7L.Power(res);
        }


        /// <summary>
        /// Surface area in radians^2 of spherical triangle on unit sphere.
        ///
        /// For the math, see:
        /// https://en.wikipedia.org/wiki/Spherical_trigonometry#Area_and_spherical_excess
        /// </summary>
        /// <param name="a">length of triangle side A in radians</param>
        /// <param name="b">length of triangle side B in radians</param>
        /// <param name="c">length of triangle side C in radians</param>
        /// <returns>area in radians^2 of triangle on unit sphere</returns>
        public double TriangleEdgeLengthToArea(double a, double b, double c)
        {
            double s = (a + b + c) / 2;

            a = (s - a) / 2;
            b = (s - b) / 2;
            c = (s - c) / 2;
            s /= 2;

            return 4 * Math.Atan
                       (Math.Sqrt(Math.Tan(s) *
                                  Math.Tan(a) * 
                                  Math.Tan(b) *
                                  Math.Tan(c)));
        }

        /// <summary>
        /// Compute area in radians^2 of a spherical triangle, given its vertices.
        /// </summary>
        /// <param name="a">vertex lat/lng in radians</param>
        /// <param name="b">vertex lat/lng in radians</param>
        /// <param name="c">vertex lat/lng in radians</param>
        /// <returns>area of triangle on unit sphere, in radians^2</returns>
        public double TriangleArea(GeoCoord a, GeoCoord b, GeoCoord c)
        {
            return TriangleEdgeLengthToArea(
                                            PointDistRads(a, b),
                                            PointDistRads(b, c),
                                            PointDistRads(c, a));
            
        }

        /// <summary>
        /// Area of H3 cell in radians^2.
        ///
        /// The area is calculated by breaking the cell into spherical triangles and
        /// summing up their areas. Note that some H3 cells (hexagons and pentagons)
        /// are irregular, and have more than 6 or 5 sides.
        ///
        /// todo: optimize the computation by re-using the edges shared between triangles
        /// </summary>
        /// <param name="cell">H3 cell</param>
        /// <returns>cell area in radians^2</returns>
        public double CellAreaRads2(H3Index cell)
        {
            var c = new GeoCoord();
            var gb = new GeoBoundary();
            H3Index.h3ToGeo(cell, ref c);
            H3Index.h3ToGeoBoundary(cell, ref gb);

            var area = 0.0;
            for (var i = 0; i < gb.numVerts; i++)
            {
                int j = (i + 1) % gb.numVerts;
                area += TriangleArea(gb.verts[i], gb.verts[j], c);
            }

            return area;
        }

        /// <summary>
        /// Area of H3 cell in kilometers^2.
        /// </summary>
        /// <param name="h">h3 cell</param>
        public double CellAreaKm2(H3Index h)
        {
            return CellAreaRads2(h) * Constants.EARTH_RADIUS_KM * Constants.EARTH_RADIUS_KM;
        }

        /// <summary>
        /// Area of H3 cell in meters^2
        /// </summary>
        /// <param name="h">h3 cell</param>
        public double CellAreaM2(H3Index h)
        {
            return CellAreaKm2(h) * 1000 * 1000;
        }

        /// <summary>
        /// Length of a unidirectional edge in radians.
        /// </summary>
        /// <param name="edge">H3 unidirectional edge</param>
        /// <returns>length in radians</returns>
        public double ExactEdgeLengthRads(H3Index edge)
        {
            var gb = new GeoBoundary();
            H3UniEdge.getH3UnidirectionalEdgeBoundary(edge, ref gb);

            var length = 0.0;
            for (var i = 0; i < gb.numVerts - 1; i++)
            {
                length += PointDistRads(gb.verts[i], gb.verts[i + 1]); 
            }
            return length;
        }

        /// <summary>
        /// Length of a unidirectional edge in kilometers.
        /// </summary>
        /// <param name="edge">H3 unidirectional edge</param>
        public double ExactEdgeLengthKm(H3Index edge)
        {
            return ExactEdgeLengthRads(edge) * Constants.EARTH_RADIUS_KM;
        }

        /// <summary>
        /// Length of a unidirectional edge in meters.
        /// </summary>
        /// <param name="edge">H3 unidirectional edge</param>
        public double ExactEdgeLengthM(H3Index edge)
        {
            return ExactEdgeLengthKm(edge) * 1000;
        }
    }

}
