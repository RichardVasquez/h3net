using System;
using System.Runtime.CompilerServices;

namespace H3Lib.Extensions
{
    public static    class GeoCoordExtensions
    {
        /// <summary>
        /// Set the components of spherical coordinates in decimal degrees.
        /// </summary>
        /// <param name="gc">The spherical coordinates</param>
        /// <param name="latitude">The desired latitude in decimal degrees</param>
        /// <param name="longitude">The desired longitude in decimal degrees</param>
        /// <!--
        /// geoCoord.c
        /// void setGeoDegs
        /// -->
        public static GeoCoord SetDegrees(this GeoCoord gc, double latitude, double longitude)
        {
            return gc.SetGeoRads(latitude.DegreesToRadians(), longitude.DegreesToRadians());
        }

        /// <summary>
        /// Set the components of spherical coordinates in radians.
        /// </summary>
        /// <param name="gc">The spherical coordinates</param>
        /// <param name="latitude">The desired latitude in decimal radians</param>
        /// <param name="longitude">The desired longitude in decimal radians</param>
        public static GeoCoord SetRadians(this GeoCoord gc, double latitude, double longitude)
        {
            return gc.SetGeoRads(latitude, longitude);
        }

        
        /// <summary>
        /// Set the components of spherical coordinates in radians.
        /// </summary>
        /// <param name="gc">The spherical coordinates</param>
        /// <param name="latitudeRadians">The desired latitude in decimal radians</param>
        /// <param name="longitudeRadians">The desired longitude in decimal radians</param>
        /// <!--
        /// geoCoord.c
        /// void _setGeoRads
        /// -->
        public static GeoCoord SetGeoRads(this GeoCoord gc, double latitudeRadians, double longitudeRadians)
        {
            gc =  new GeoCoord(latitudeRadians, longitudeRadians);
            return gc;
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
        /// </
        /// <!--
        /// geoCoord.c
        /// double H3_EXPORT(pointDistRads)
        /// -->
        public static double DistanceToRadians(this GeoCoord a, GeoCoord b)
        {
            double sinLat = Math.Sin((b.Latitude - a.Latitude) / 2.0);
            double sinLng = Math.Sin((b.Longitude - a.Longitude) / 2.0);
            double p = sinLat * sinLat + Math.Cos(a.Latitude) *
                       Math.Cos(b.Latitude) * sinLng * sinLng;

            return 2 * Math.Atan2(Math.Sqrt(p), Math.Sqrt(1 - p));
        }

        /// <summary>
        /// The great circle distance in kilometers between two spherical coordinates
        /// </summary>
        /// <param name="a">the first lat/lng pair (in radians)</param>
        /// <param name="b">the second lat/lng pair (in radians)</param>
        /// <!--
        /// geoCoord.c
        /// double H3_EXPORT(pointDistKm)
        /// -->
        public static double DistanceToKm(this GeoCoord a, GeoCoord b)
        {
            return a.DistanceToRadians(b) * Constants.EARTH_RADIUS_KM;
        }
        
        /// <summary>
        /// The great circle distance in meters between two spherical coordinates
        /// </summary>
        /// <param name="a">the first lat/lng pair (in radians)</param>
        /// <param name="b">the second lat/lng pair (in radians)</param>
        /// <!--
        /// geoCoord.c
        /// double H3_EXPORT(pointDistM)
        /// -->
        public static double DistanceToM(this GeoCoord a, GeoCoord b)
        {
            return a.DistanceToKm(b) * 1000;
        }

        /// <summary>
        /// Determines the azimuth to p2 from p1 in radians
        /// </summary>
        /// <param name="p1">The first spherical coordinates</param>
        /// <param name="p2">The second spherical coordinates</param>
        /// <returns>The azimuth in radians from p1 to p2</returns>
        /// <!--
        /// geoCoord.c
        /// double _geoAzimuthRads
        /// -->
        public static double AzimuthRadiansTo(this GeoCoord p1, GeoCoord p2)
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
        /// <param name="azimuth">The desired azimuth from p1.</param>
        /// <param name="distance">The desired distance from p1, must be non-negative.</param>
        /// <returns>The spherical coordinates at the desired azimuth and distance from p1.</returns>
        /// <!--
        /// geoCoord.c
        /// void _geoAzDistanceRads
        /// -->
        public static GeoCoord GetAzimuthDistancePoint(this GeoCoord p1, double azimuth, double distance)
        {
            if (distance < Constants.EPSILON)
            {
                return new GeoCoord(p1);
            }

            double tempLatitude;
            double tempLongitude = 0;
            azimuth = azimuth.NormalizeRadians();// _posAngleRads(az);

            // check for due north/south azimuth
            if (azimuth < Constants.EPSILON || 
                Math.Abs(azimuth - Constants.M_PI) < Constants.EPSILON)
            {
                tempLatitude = azimuth < Constants.EPSILON
                                   ? p1.Latitude + distance     // due north
                                   : p1.Latitude - distance;    // due south

                if (Math.Abs(tempLatitude - Constants.M_PI_2) < Constants.EPSILON) // north pole
                {
                    tempLatitude = Constants.M_PI_2;
                    tempLongitude = 0.0;
                }
                else if (Math.Abs(tempLatitude + Constants.M_PI_2) < Constants.EPSILON) // south pole
                {
                    tempLatitude = -Constants.M_PI_2;
                    tempLongitude = 0.0;
                }
                else
                {
                    tempLongitude = tempLongitude.ConstrainLongitude();
                }
            }
            else // not due north or south
            {
                double sinLatitude = Math.Sin(p1.Latitude) * Math.Cos(distance) +
                                     Math.Cos(p1.Latitude) * Math.Sin(distance) *
                                     Math.Cos(azimuth);
                if (sinLatitude > 1.0)
                {
                    sinLatitude = 1.0;
                }

                if (sinLatitude < -1.0)
                {
                    sinLatitude = -1.0;
                }
                tempLatitude = Math.Asin(sinLatitude);
                if (Math.Abs(tempLatitude - Constants.M_PI_2) < Constants.EPSILON) // north pole
                {
                    tempLatitude = Constants.M_PI_2;
                    tempLongitude = 0.0;
                }
                else if (Math.Abs(tempLatitude + Constants.M_PI_2) < Constants.EPSILON) // south pole
                {
                    tempLatitude = -Constants.M_PI_2;
                    tempLongitude = 0.0;
                }
                else
                {
                    double sinLongitude = Math.Sin(azimuth) * Math.Sin(distance) / Math.Cos(tempLatitude);
                    double cosLongitude = (Math.Cos(distance) - Math.Sin(p1.Latitude) * Math.Sin(tempLatitude)) /
                                          Math.Cos(p1.Latitude) / Math.Cos(tempLatitude);
                    if (sinLongitude > 1.0)
                    {
                        sinLongitude = 1.0;
                    }

                    if (sinLongitude < -1.0)
                    {
                        sinLongitude = -1.0;
                    }

                    if (cosLongitude > 1.0)
                    {
                        sinLongitude = 1.0;
                    }

                    if (cosLongitude < -1.0)
                    {
                        sinLongitude = -1.0;
                    }

                    tempLongitude =
                        (p1.Longitude + Math.Atan2(sinLongitude, cosLongitude))
                       .ConstrainLongitude();
                }
            }   
            return new GeoCoord(tempLatitude, tempLongitude);
        }

        /// <summary>
        /// Encodes a coordinate on the sphere to the FaceIJK address of the containing
        /// cell at the specified resolution.
        /// </summary>
        /// <param name="g">The spherical coordinates to encode.</param>
        /// <param name="res">The desired H3 resolution for the encoding.</param>
        /// <returns>The FaceIJK address of the containing cell at resolution res.</returns>
        /// <!--
        /// faceijk.c
        /// void _geoToFaceIjk
        /// -->
        public static FaceIjk ToFaceIjk(this GeoCoord g, int res)
        {
            // first convert to hex2d
            (int newFace, var v) = g.ToHex2d(res, 0);
            var newCoord = v.ToCoordIjk();

            return new FaceIjk(newFace, newCoord);
        }

        /// <summary>
        /// Encodes a coordinate on the sphere to the corresponding icosahedral face and
        /// containing 2D hex coordinates relative to that face center.
        /// </summary>
        /// <param name="g">The spherical coordinates to encode.</param>
        /// <param name="res">The desired H3 resolution for the encoding.</param>
        /// <param name="face">The icosahedral face containing the spherical coordinates.</param>
        /// <returns>
        /// Tuple
        /// Item1: The resulting face (can later get rid of <see cref="face"/> parameter.
        /// Item2: The 2D hex coordinates of the cell containing the point.
        /// </returns>
        /// <!--
        /// faceijk.c
        /// void _geoToHex2d
        /// -->
        public static (int, Vec2d) ToHex2d(this GeoCoord g, int res, int face)
        {
            Vec3d v3d = g.ToVec3d();
            int newFace = 0;

            // determine the icosahedron face
            double sqd = v3d.PointSquareDistance(FaceIjk.FaceCenterPoint[0]);

            for (int f = 1; f < Constants.NUM_ICOSA_FACES; f++)
            {
                double sqdT = v3d.PointSquareDistance(FaceIjk.FaceCenterPoint[f]);
                if (!(sqdT < sqd))
                {
                    continue;
                }
                newFace = f;
                sqd = sqdT;
            }

            // cos(r) = 1 - 2 * sin^2(r/2) = 1 - 2 * (sqd / 4) = 1 - sqd/2
            double r = Math.Acos(1 - sqd / 2);

            if (r < double.Epsilon)
            {
                return (newFace, new Vec2d());
            }

            // now have face and r, now find CCW theta from CII i-axis
            double theta =
                (
                    FaceIjk.FaceAxesAzRadsCii[newFace, 0] -
                    g.AzimuthRadiansTo(FaceIjk.FaceCenterGeo[newFace]).NormalizeRadians()
                ).NormalizeRadians();
            
            // adjust theta for Class III (odd resolutions)
            if (res.IsResClassIii())
            {
                theta = (theta - Constants.M_AP7_ROT_RADS).NormalizeRadians();
            }

            // perform gnomonic scaling of r
            r = Math.Tan(r);

            // scale for current resolution length u
            r /= Constants.RES0_U_GNOMONIC;
            for (var i = 0; i < res; i++)
            {
                r *= FaceIjk.M_SQRT7;
            }

            // we now have (r, theta) in hex2d with theta ccw from x-axes
            // convert to local x,y
            return (newFace,
                    new Vec2d
                        (
                         r * Math.Cos(theta),
                         r * Math.Sin(theta)
                        ));
        }

        /// <summary>
        /// Calculate the 3D coordinate on unit sphere from the latitude and longitude.
        /// </summary>
        /// <param name="geo">The latitude and longitude of the point</param>
        /// <!--
        /// vec3d.c
        /// void _geoToVec3d
        /// -->
        public static Vec3d ToVec3d(this GeoCoord geo)
        {
            double r = Math.Cos(geo.Latitude);
            return new Vec3d
                (
                 Math.Cos(geo.Longitude) * r,
                 Math.Sin(geo.Longitude) * r,
                 Math.Sin(geo.Latitude)
                );
        }
        
    }
}
