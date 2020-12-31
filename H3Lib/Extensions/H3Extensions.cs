using System;

namespace H3Lib.Extensions
{
    public static class H3Extensions
    {
        /// <summary>
        /// Normalizes radians to a value between 0.0 and two PI.
        /// </summary>
        /// <param name="rads">The input radians value</param>
        /// <returns>The normalized radians value</returns>
        /// <remarks>
        /// Originally part of geoCoord.c as  double _posAngleRads(double rads)
        ///
        /// However, it's only used once in
        /// void _geoAzDistanceRads(const GeoCoord *p1, double az, double distance, GeoCoord *p2)
        ///
        /// It's used multiple times in faceijk.c, _geoToHex2d and _hex2dToGeo
        ///
        /// For now, let's isolate it and see if it needs to be folded in later.
        /// </remarks>
        public static double NormalizeRadians(this double rads)
        {
            if (rads < 0.0)
            {
                rads += Math.Ceiling(Math.Abs(rads / Constants.M_2PI)) * Constants.M_2PI;
            }

            while (rads>= Constants.M_2PI)
            {
                rads -= Constants.M_2PI;
            }

            return rads;
        }

        /// <summary>
        /// Does integer exponentiation efficiently. Taken from StackOverflow.
        ///
        /// An example of this can be found at:
        /// https://stackoverflow.com/questions/101439/the-most-efficient-way-to-implement-an-integer-based-power-function-powint-int
        /// </summary>
        /// <param name="baseValue">the integer base (can be positive or negative)</param>
        /// <param name="power">the integer exponent (should be nonnegative)</param>
        /// <returns></returns>
        public static long Power(this long baseValue, long power)
        {
            long result = 1;

            while (power > 0)
            {
                if ((power & 1) != 0)
                {
                    result *= baseValue;
                }
                power >>= 1;
                baseValue *= baseValue;
            }

            return result;
        }
        
        /// <summary>
        /// Set the components of spherical coordinates in decimal degrees.
        /// </summary>
        /// <param name="p">The spherical coordinates</param>
        /// <param name="latitude">The desired latitude in decimal degrees</param>
        /// <param name="longitude">The desired longitude in decimal degrees</param>
        /// <!-- Based off 3.1.1 -->
        public static void SetDegrees(this GeoCoord p, double latitude, double longitude)
        {
            GeoCoord._setGeoRads(ref p,
                                 GeoCoord.DegreesToRadians(latitude),
                                 GeoCoord.DegreesToRadians(longitude));
        }
        
        /// <summary>
        /// Set the components of spherical coordinates in radians.
        /// </summary>
        /// <param name="p">The spherical coordinates</param>
        /// <param name="latitude">The desired latitude in decimal radians</param>
        /// <param name="longitude">The desired longitude in decimal radians</param>
        /// <!-- Based off 3.1.1 -->
        public static void SetRadians(this GeoCoord p, double latitude, double longitude)
        {
            GeoCoord._setGeoRads(ref p, latitude, longitude);
        }


    }
}
