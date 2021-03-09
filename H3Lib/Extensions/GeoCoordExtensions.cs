using System;
using DecimalMath;

namespace H3Lib.Extensions
{
    /// <summary>
    /// Operations for GeoCoord type
    /// </summary>
    public static class GeoCoordExtensions
    {
        /// <summary>
        /// Set the components of spherical coordinates in decimal degrees.
        /// </summary>
        /// <param name="gc">The spherical coordinates</param>
        /// <param name="latitude">The desired latitude in decimal degrees</param>
        /// <param name="longitude">The desired longitude in decimal degrees</param>
        /// <remarks>
        /// 3.7.1
        /// geoCoord.c
        /// void setGeoDegs
        /// </remarks>
        public static GeoCoord SetDegrees(this GeoCoord gc, decimal latitude, decimal longitude)
        {
            return gc.SetGeoRads(latitude.DegreesToRadians(), longitude.DegreesToRadians());
        }

        /// <summary>
        /// Set the components of spherical coordinates in radians.
        /// </summary>
        /// <param name="gc">The spherical coordinates</param>
        /// <param name="latitude">The desired latitude in decimal radians</param>
        /// <param name="longitude">The desired longitude in decimal radians</param>
        public static GeoCoord SetRadians(this GeoCoord gc, decimal latitude, decimal longitude)
        {
            return gc.SetGeoRads(latitude, longitude);
        }
        
        /// <summary>
        /// Set the components of spherical coordinates in radians.
        /// </summary>
        /// <param name="gc">The spherical coordinates</param>
        /// <param name="latitudeRadians">The desired latitude in decimal radians</param>
        /// <param name="longitudeRadians">The desired longitude in decimal radians</param>
        /// <remarks>
        /// 3.7.1
        /// geoCoord.c
        /// void _setGeoRads
        /// </remarks>
        private static GeoCoord SetGeoRads(this GeoCoord gc, decimal latitudeRadians, decimal longitudeRadians)
        {
            gc =  new GeoCoord(latitudeRadians, longitudeRadians);
            return gc;
        }

        /// <summary>
        /// Quick replacement for Latitude
        /// </summary>
        public static GeoCoord SetLatitude(this GeoCoord gc, decimal latitude)
        {
            return new GeoCoord(latitude, gc.Longitude);
        }

        /// <summary>
        /// Quick replacement for Longitude
        /// </summary>
        public static GeoCoord SetLongitude(this GeoCoord gc, decimal longitude)
        {
            return new GeoCoord(gc.Latitude, longitude);
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
        /// <remarks>
        /// 3.7.1
        /// geoCoord.c
        /// double H3_EXPORT(pointDistRads)
        /// </remarks>
        public static decimal DistanceToRadians(this GeoCoord a, GeoCoord b)
        {
            decimal sinLat = DecimalEx.Sin((b.Latitude - a.Latitude) / 2.0m);
            decimal sinLng = DecimalEx.Sin((b.Longitude - a.Longitude) / 2.0m);
            decimal p = sinLat * sinLat + DecimalEx.Cos(a.Latitude) *
                        DecimalEx.Cos(b.Latitude) * sinLng * sinLng;

            return 2 * DecimalEx.ATan2(DecimalEx.Sqrt(p), DecimalEx.Sqrt(1 - p));
        }

        /// <summary>
        /// The great circle distance in kilometers between two spherical coordinates
        /// </summary>
        /// <param name="a">the first lat/lng pair (in radians)</param>
        /// <param name="b">the second lat/lng pair (in radians)</param>
        /// <remarks>
        /// 3.7.1
        /// geoCoord.c
        /// double H3_EXPORT(pointDistKm)
        /// </remarks>
        public static decimal DistanceToKm(this GeoCoord a, GeoCoord b)
        {
            return a.DistanceToRadians(b) * Constants.H3.EARTH_RADIUS_KM;
        }
        
        /// <summary>
        /// The great circle distance in meters between two spherical coordinates
        /// </summary>
        /// <param name="a">the first lat/lng pair (in radians)</param>
        /// <param name="b">the second lat/lng pair (in radians)</param>
        /// <remarks>
        /// 3.7.1
        /// geoCoord.c
        /// double H3_EXPORT(pointDistM)
        /// </remarks>
        public static decimal DistanceToM(this GeoCoord a, GeoCoord b)
        {
            return a.DistanceToKm(b) * 1000;
        }

        /// <summary>
        /// Determines the azimuth to p2 from p1 in radians
        /// </summary>
        /// <param name="p1">The first spherical coordinates</param>
        /// <param name="p2">The second spherical coordinates</param>
        /// <returns>The azimuth in radians from p1 to p2</returns>
        /// <remarks>
        /// 3.7.1
        /// geoCoord.c
        /// double _geoAzimuthRads
        /// </remarks>
        private static decimal AzimuthRadiansTo(this GeoCoord p1, GeoCoord p2)
        {
            return
                DecimalEx.ATan2
                    (
                     DecimalEx.Cos(p2.Latitude) * DecimalEx.Sin(p2.Longitude - p1.Longitude),
                     DecimalEx.Cos(p1.Latitude) * DecimalEx.Sin(p2.Latitude) -
                     DecimalEx.Sin(p1.Latitude) * DecimalEx.Cos(p2.Latitude) *
                     DecimalEx.Cos(p2.Longitude - p1.Longitude)
                    );
        }

        /// <summary>
        /// Computes the point on the sphere a specified azimuth and distance from
        /// another point.
        /// </summary>
        /// <param name="p1">The first spherical coordinates.</param>
        /// <param name="azimuth">The desired azimuth from p1.</param>
        /// <param name="distance">The desired distance from p1, must be non-negative.</param>
        /// <returns>The spherical coordinates at the desired azimuth and distance from p1.</returns>
        /// <remarks>
        /// 3.7.1
        /// geoCoord.c
        /// void _geoAzDistanceRads
        /// </remarks>
        internal static GeoCoord GetAzimuthDistancePoint(this GeoCoord p1, decimal azimuth, decimal distance)
        {
            if (distance < Constants.H3.EPSILON)
            {
                return p1;
            }

            azimuth = azimuth.NormalizeRadians().ConstrainToPiAccuracy();
            var p2 = new GeoCoord();

            // check for due north/south azimuth
            if (azimuth < Constants.H3.EPSILON ||
                Math.Abs(azimuth - Constants.H3.M_PI) < Constants.H3.EPSILON)
            {
                if (azimuth < Constants.H3.EPSILON) // due north
                {
                    p2 = p2.SetLatitude(p1.Latitude + distance);
                }
                else // due south
                {
                    p2 = p2.SetLatitude(p1.Latitude - distance);
                }

                if (Math.Abs(p2.Latitude - Constants.H3.M_PI_2) < Constants.H3.EPSILON) // north pole
                {
                    p2 = new GeoCoord(Constants.H3.M_PI_2, 0.0m);
                }
                else if (Math.Abs(p2.Latitude + Constants.H3.M_PI_2) < Constants.H3.EPSILON) // south pole
                {
                    p2 = new GeoCoord(-Constants.H3.M_PI_2, 0.0m);
                }
                else
                {
                    p2 = p2.SetLongitude(p1.Longitude.ConstrainLongitude());
                }
            }
            else // Not due north or south
            {
                decimal sinLatitude = DecimalEx.Sin(p1.Latitude) * DecimalEx.Cos(distance) +
                                        DecimalEx.Cos(p1.Latitude) * DecimalEx.Sin(distance) * DecimalEx.Cos(azimuth);
                sinLatitude = sinLatitude.ConstrainToPiAccuracy();
                if (sinLatitude > 1.0m)
                {
                    sinLatitude = 1.0m;
                }

                if (sinLatitude < -1.0m)
                {
                    sinLatitude = 1.0m;
                }

                p2 = p2.SetLatitude(DecimalEx.ASin(sinLatitude).ConstrainToPiAccuracy());

                if (Math.Abs(p2.Latitude - Constants.H3.M_PI_2) < Constants.H3.EPSILON) // north pole
                {
                    p2 = new GeoCoord(Constants.H3.M_PI_2, 0.0m);
                }
                else if (Math.Abs(p2.Latitude + Constants.H3.M_PI_2) < Constants.H3.EPSILON) // south pole
                {
                    p2 = new GeoCoord(-Constants.H3.M_PI_2, 0.0m);
                }
                else
                {
                    decimal sinLongitude = DecimalEx.Sin(azimuth) * DecimalEx.Sin(distance) / DecimalEx.Cos(p2.Latitude);
                    decimal cosLongitude = (DecimalEx.Cos(distance) - DecimalEx.Sin(p1.Latitude) * DecimalEx.Sin(p2.Latitude)) /
                                          DecimalEx.Cos(p1.Latitude) / DecimalEx.Cos(p2.Latitude);
                    if (sinLongitude > 1.0m)
                    {
                        sinLongitude = 1.0m;
                    }

                    if (sinLongitude < -1.0m)
                    {
                        sinLongitude = -1.0m;
                    }

                    if (cosLongitude > 1.0m)
                    {
                        cosLongitude = 1.0m;
                    }

                    if (cosLongitude < -1.0m)
                    {
                        cosLongitude = -1.0m;
                    }

                    p2 = p2.SetLongitude
                        (
                         (p1.Longitude + DecimalEx.ATan2(sinLongitude, cosLongitude))
                        .ConstrainLongitude().ConstrainToPiAccuracy()
                        );
                }
            }

            return p2;            
        }

        /// <summary>
        /// Encodes a coordinate on the sphere to the FaceIJK address of the containing
        /// cell at the specified resolution.
        /// </summary>
        /// <param name="g">The spherical coordinates to encode.</param>
        /// <param name="res">The desired H3 resolution for the encoding.</param>
        /// <returns>The FaceIJK address of the containing cell at resolution res.</returns>
        /// <remarks>
        /// 3.7.1
        /// faceijk.c
        /// void _geoToFaceIjk
        /// </remarks>
        private static FaceIjk ToFaceIjk(this GeoCoord g, int res)
        {
            // first convert to hex2d
            (int newFace, var v) = g.ToHex2d(res);
            var newCoord = v.ToCoordIjk();
           
            return new FaceIjk(newFace, newCoord);
        }

        /// <summary>
        /// Encodes a coordinate on the sphere to the corresponding icosahedral face and
        /// containing 2D hex coordinates relative to that face center.
        /// </summary>
        /// <param name="g">The spherical coordinates to encode.</param>
        /// <param name="res">The desired H3 resolution for the encoding.</param>
        /// <returns>
        /// Tuple
        /// Item1: The resulting face
        /// Item2: The 2D hex coordinates of the cell containing the point.
        /// </returns>
        /// <remarks>
        /// 3.7.1
        /// faceijk.c
        /// void _geoToHex2d
        /// </remarks>
        private static (int, Vec2d) ToHex2d(this GeoCoord g, int res)
        {
            var v3d = g.ToVec3d();
            
            var newFace = 0;

            // determine the icosahedron face
            decimal sqd = v3d.PointSquareDistance(Constants.FaceIjk.FaceCenterPoint[0]);

            for (var f = 1; f < Constants.H3.IcosahedronFaces; f++)
            {
                decimal sqdT = v3d.PointSquareDistance(Constants.FaceIjk.FaceCenterPoint[f]);
                if (!(sqdT < sqd))
                {
                    continue;
                }
                newFace = f;
                sqd = sqdT;
            }
            
            // cos(r) = 1 - 2 * sin^2(r/2) = 1 - 2 * (sqd / 4) = 1 - sqd/2
            decimal r = DecimalEx.ACos(1 - sqd / 2.0m);

            if (r < Constants.H3.EPSILON)
            {
                return (newFace, new Vec2d());
            }
            // now have face and r, now find CCW theta from CII i-axis
            decimal theta =
                (
                    Constants.FaceIjk.FaceAxesAzRadsCii[newFace, 0] -
                    Constants.FaceIjk.FaceCenterGeo[newFace].AzimuthRadiansTo(g)
                             .NormalizeRadians()
                ).NormalizeRadians();
            
            // adjust theta for Class III (odd resolutions)
            if (res.IsResClassIii())
            {
                theta = (theta - Constants.H3.M_AP7_ROT_RADS).NormalizeRadians();
            }

            // perform gnomonic scaling of r
            r = DecimalEx.Tan(r);

            // scale for current resolution length u
            r /= Constants.H3.RES0_U_GNOMONIC;
            for (var i = 0; i < res; i++)
            {
                r *= Constants.FaceIjk.Sqrt7;
            }
            
            // we now have (r, theta) in hex2d with theta ccw from x-axes
            // convert to local x,y
            return (newFace,
                    new Vec2d
                        (
                         r * DecimalEx.Cos(theta),
                         r * DecimalEx.Sin(theta)
                        ));
        }

        /// <summary>
        /// Calculate the 3D coordinate on unit sphere from the latitude and longitude.
        /// </summary>
        /// <param name="geo">The latitude and longitude of the point</param>
        /// <remarks>
        /// 3.7.1
        /// vec3d.c
        /// void _geoToVec3d
        /// </remarks>
        public static Vec3d ToVec3d(this GeoCoord geo)
        {
            decimal r = DecimalEx.Cos(geo.Latitude);
            return new Vec3d
                (
                 DecimalEx.Cos(geo.Longitude) * r,
                 DecimalEx.Sin(geo.Longitude) * r,
                 DecimalEx.Sin(geo.Latitude)
                );
        }

        /// <summary>
        /// Encodes a coordinate on the sphere to the H3 index of the containing cell at
        /// the specified resolution.
        ///
        /// Returns 0 on invalid input.
        /// </summary>
        /// <param name="g">The spherical coordinates to encode.</param>
        /// <param name="res">The desired H3 resolution for the encoding.</param>
        /// <returns>The encoded H3Index (or H3_NULL on failure).</returns>
        /// <remarks>
        /// 3.7.1
        /// h3Index.c
        /// H3Index H3_EXPORT(geoToH3)
        /// </remarks>
        public static H3Index ToH3Index(this GeoCoord g, int res)
        {
            if (res < 0 || res > Constants.H3.MaxH3Resolution)
            {
                return Constants.H3Index.InvalidIndex;
            }

            // Decimals don't do infinities. Cross our fingers.
            if (!(Math.Abs(g.Latitude) < decimal.MaxValue) || !(Math.Abs(g.Longitude) < decimal.MaxValue))
            {
                return Constants.H3Index.InvalidIndex;
            }
                
            return g.ToFaceIjk(res).ToH3(res);
        }

        /// <summary>
        /// returns an estimated number of hexagons that trace
        /// the cartesian-projected line
        /// </summary>
        /// <param name="origin">the origin coordinates</param>
        /// <param name="destination">the destination coordinates</param>
        /// <param name="res">the resolution of the H3 hexagons to trace the line</param>
        /// <returns>the estimated number of hexagons required to trace the line</returns>
        /// <remarks>
        /// 3.7.1
        /// bbox.c
        /// int lineHexEstimate
        /// </remarks>
        public static int LineHexEstimate(this GeoCoord origin, GeoCoord destination, int res)
        {
            // Get the area of the pentagon as the maximally-distorted area possible
            var pentagons = res.GetPentagonIndexes();
            decimal pentagonRadiusKm = pentagons[0].HexRadiusKm();
            decimal dist = origin.DistanceToKm(destination);

            var estimate = (int) Math.Ceiling(dist / (2 * pentagonRadiusKm));
            if (estimate == 0)
            {
                estimate = 1;
            }
            return estimate;
        }

        /// <summary>
        /// This converts a radian based coordinate to a degree based coordinate for quick
        /// conversion. This has the possibility of losing accuracy if the conversion
        /// round trips, but should be good enough for a one way conversion.
        /// </summary>
        /// <param name="coord">GeoCoord to convert</param>
        /// <returns>A new DegreeCoord struct</returns>
        public static DegreeCoord ToDecimalCoord(this GeoCoord coord)
        {
            return new DegreeCoord(coord.Latitude.RadiansToDegrees(), coord.Longitude.RadiansToDegrees());
        }

    }
}
