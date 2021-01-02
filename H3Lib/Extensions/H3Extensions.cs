using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

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
        /// Transforms coordinates from the IJ coordinate system to the IJK+ coordinate system
        /// </summary>
        /// <param name="ij">The input IJ coordinates</param>
        public static CoordIjk ToIjk(this LocalIJ.CoordIJ ij)
        {
            return new CoordIjk(ij.i, ij.j, 0).Normalized();
        }
    }
}
