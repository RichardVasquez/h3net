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

        public static void ToFaceIjk(this GeoCoord g, int res, FaceIjk h)
        {
            //var v = g.to
        }

        
        /// <summary>
        /// Set the components of spherical coordinates in radians.
        /// </summary>
        /// <param name="gc">The spherical coordinates</param>
        /// <param name="latitudeRadians">The desired latitude in decimal radians</param>
        /// <param name="longitudeRadians">The desired longitude in decimal radians</param>
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
        /// </returns>
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
        public static double DistanceToKm(this GeoCoord a, GeoCoord b)
        {
            return a.DistanceToRadians(b) * Constants.EARTH_RADIUS_KM;
        }
        
        /// <summary>
        /// The great circle distance in meters between two spherical coordinates
        /// </summary>
        /// <param name="a">the first lat/lng pair (in radians)</param>
        /// <param name="b">the second lat/lng pair (in radians)</param>
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
    }
}
