using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace H3Lib.Extensions
{
    public static class H3LibExtensions
    {
        /// <summary>
        /// Normalizes radians to a value between 0.0 and two PI.
        /// </summary>
        /// <param name="rads">The input radians value</param>
        /// <param name="limit">Default value of 2pi. _Can_ be changed, probably shouldn't</param>
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
        /// <!--
        /// geoCoord.c
        /// double _posAngleRads
        /// -->
        public static double NormalizeRadians(this double rads, double limit = Constants.M_2PI)
        {
            if (rads < 0.0)
            {
                rads += Math.Ceiling(Math.Abs(rads / limit)) * limit;
            }

            while (rads >= limit)
            {
                rads -= limit;
            }

            return rads;
        }

        /// <summary>
        /// Makes sure latitudes are in the proper bounds
        /// </summary>
        /// <param name="latitude">The original lat value</param>
        /// <returns>The corrected lat value</returns>
        /// <!--
        /// geoCoord.c
        /// double constrainLat
        /// -->
        public static double ConstrainLatitude(this double latitude)
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
        /// <!--
        /// geoCoord.c
        /// double constrainLng
        /// -->
        public static double ConstrainLongitude(this double longitude)
        {
            while (longitude > Constants.M_PI)
            {
                longitude -= 2 * Constants.M_PI;
            }

            while (longitude < -Constants.M_PI)
            {
                longitude += 2 * Constants.M_PI;
            }

            return longitude;
        }


        /// <summary>
        /// Convert from decimal degrees to radians.
        /// </summary>
        /// <param name="degrees">The decimal degrees</param>
        /// <returns>The corresponding radians</returns>
        /// <!--
        /// geoCoord.c
        /// double H3_EXPORT(degsToRads)
        /// -->
        public static double DegreesToRadians(this double degrees)
        {
            return degrees * Constants.M_PI_180;
        }

        /// <summary>
        /// Convert from radians to decimal degrees.
        /// </summary>
        /// <param name="radians">The radians</param>
        /// <returns>The corresponding decimal degrees</returns>
        /// <!--
        /// geoCoord.c
        /// double H3_EXPORT(radsToDegs)
        /// -->
        public static double RadiansToDegrees(this double radians)
        {
            return radians * Constants.M_180_PI;
        }

        /// <summary>
        /// Does integer exponentiation efficiently. Taken from StackOverflow.
        ///
        /// An example of this can be found at:
        /// https://stackoverflow.com/questions/101439/the-most-efficient-way-to-implement-an-integer-based-power-function-powint-int
        /// </summary>
        /// <param name="baseValue">the integer base (can be positive or negative)</param>
        /// <param name="power">the integer exponent (should be nonnegative)</param>
        /// <returns>the exponentiated value</returns>
        /// <!--
        /// mathExtensions.c
        /// int64_t _ipow
        /// -->
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
        /// Number of unique valid H3Indexes at given resolution.
        /// </summary>
        /// <param name="res">Resolution to get count of cells</param>
        /// <!--
        /// geoCoord.c
        /// int64_t H3_EXPORT(numHexagons)
        /// -->
        public static long NumHexagons(int res)
        {
            return 2 + 120 * 7L.Power(res);
        }
        
        /// <summary>
        /// Return whether or not the indicated base cell is a pentagon.
        /// </summary>
        /// <!--
        /// baseCells.c
        /// int _isBaseCellPentagon
        /// -->
        public static bool IsBaseCellPentagon(this int baseCell)
        {
            return BaseCells.baseCellData[baseCell].isPentagon == 1;
        }
        
        /// <summary>
        /// Returns whether or not a resolution is a Class III grid. Note that odd
        ///  resolutions are Class III and even resolutions are Class II.
        /// </summary>
        /// <param name="res">The H3 resolution</param>
        /// <returns>Returns true if the resolution is class III grid, otherwise false.</returns>
        /// <!--
        /// h3Index.c
        /// int isResClassIII
        /// -->
        public static bool IsResClassIii(this int res)
        {
            return res % 2 == 1;
        }

        /// <summary>
        /// Return the direction from the origin base cell to the neighbor.
        /// </summary>
        /// <returns>INVALID_DIGIT if the base cells are not neighbors.</returns>
        /// <!--
        /// baseCells.c
        /// Direction _getBaseCellDirection
        /// -->
        public static Direction GetBaseCellDirection(this int originBaseCell, int neighboringBaseCell)
        {
            for (var dir = Direction.CENTER_DIGIT; dir < Direction.NUM_DIGITS; dir++) {
                int testBaseCell = BaseCells.baseCellNeighbors[originBaseCell, (int)dir];
                if (testBaseCell == neighboringBaseCell)
                {
                    return dir;
                }
            }

            return Direction.INVALID_DIGIT;
        }

        /// <summary>
        /// Square of a number
        /// </summary>
        /// <param name="x">The input number</param>
        /// <returns>The square of the input number</returns>
        /// <!--
        /// vec3d.c
        /// double _square
        /// -->
        public static double Square(this double x)
        {
            return x * x;
        }

        /// <summary>
        /// Converts a string representation of an H3 index into an H3 index.
        /// </summary>
        /// <param name="s"> The string representation of an H3 index.</param>
        /// <returns>
        /// The H3 index corresponding to the string argument, or 0 if invalid.
        /// </returns>
        /// <!--
        /// h3Index.c
        /// H3Index H3_EXPORT(stringToH3)
        /// -->
        public static H3Index ToH3Index(this string s)
        {
            // A small risk, but for the most part, we're dealing with hex
            // numbers, so let's use that as our default.
            if (ulong.TryParse(s, NumberStyles.HexNumber, CultureInfo.CurrentCulture, out ulong ul1))
            {
                return new H3Index(ul1);
            }
            // If failed, try parsing as a decimal based number
            return ulong.TryParse(s, out ulong ul2)
                       ? new H3Index(ul2)
                       : 0;
        }

        /// <summary>
        /// Determines whether one resolution is a valid child resolution of another.
        /// Each resolution is considered a valid child resolution of itself.
        /// </summary>
        /// <param name="parentRes">int resolution of the parent</param>
        /// <param name="childRes">int resolution of the child</param>
        /// <returns>The validity of the child resolution</returns>
        /// <!--
        /// h3Index.c
        /// static bool _isValidChildRes
        /// -->
        public static bool IsValidChildRes(this int parentRes, int childRes)
        {
            return childRes >= parentRes &&
                   childRes <= Constants.MAX_H3_RES;
        }

        /// Generates all pentagons at the specified resolution
        ///
        /// <param name="res">The resolution to produce pentagons at.</param>
        /// <returns>Output List.</returns>
        /// <!--
        /// h3Index.c
        /// void H3_EXPORT(getPentagonIndexes)
        /// -->
        public static List<H3Index> GetPentagonIndexes(this int res)
        {
            var cells = Enumerable
                       .Range(0, Constants.NUM_BASE_CELLS)
                       .Where(bc => bc.IsBaseCellPentagon());
            return cells
                  .Select(cell => new H3Index().SetIndex(res, cell, Direction.CENTER_DIGIT))
                  .ToList();
        }
       
    }
}
