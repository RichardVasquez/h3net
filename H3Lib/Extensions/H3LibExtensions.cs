using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace H3Lib.Extensions
{
    /// <summary>
    /// Extension methods that work on numbers that are then converted to some
    /// parameter of H3Index space
    /// </summary>
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
        ///
        /// 3.7.1
        /// geoCoord.c
        /// double _posAngleRads
        /// </remarks>
        internal static decimal NormalizeRadians(this decimal rads, decimal limit = Constants.H3.M_2PI)
        {
            decimal tmp = rads < 0.0m
                             ? rads + Constants.H3.M_2PI
                             : rads;
            if (rads >= Constants.H3.M_2PI)
            {
                tmp -= Constants.H3.M_2PI;
            }

            return tmp;
        }

        /// <summary>
        /// Makes sure latitudes are in the proper bounds
        /// </summary>
        /// <param name="latitude">The original lat value</param>
        /// <returns>The corrected lat value</returns>
        /// <remarks>
        /// 3.7.1
        /// geoCoord.c
        /// double constrainLat
        /// </remarks>
        public static decimal ConstrainLatitude(this decimal latitude)
        {
            while (latitude > Constants.H3.M_PI_2)
            {
                latitude -= Constants.H3.M_PI;
            }

            return latitude;
        }


        /// <summary>
        /// Constrain Latitude to +/- PI/2
        /// </summary>
        public static decimal ConstrainLatitude(this int latitude)
        {
            var newLatitude = (decimal) latitude;
            while (newLatitude > Constants.H3.M_PI_2)
            {
                newLatitude -= Constants.H3.M_PI;
            }

            return newLatitude;
        }

        /// <summary>
        /// Constants only covers PI to a certain value.  Who am I to improve on that?
        /// </summary>
        internal static decimal ConstrainToPiAccuracy(this decimal number)
        {
            number *= 100000000000000000000m;
            number = decimal.Truncate(number);
            number /= 100000000000000000000m;
            return number;
        }
        
        /// <summary>
        /// Makes sure longitudes are in the proper bounds
        /// </summary>
        /// <param name="longitude">The origin lng value</param>
        /// <returns>The corrected lng value</returns>
        /// <remarks>
        /// 3.7.1
        /// geoCoord.c
        /// double constrainLng
        /// </remarks>
        public static decimal ConstrainLongitude(this decimal longitude)
        {
            longitude = longitude.ConstrainToPiAccuracy();
            while (longitude > Constants.H3.M_PI)
            {
                longitude -= 2 * Constants.H3.M_PI;
            }

            while (longitude < -Constants.H3.M_PI)
            {
                longitude += 2 * Constants.H3.M_PI;
            }

            return longitude;
        }

        /// <summary>
        /// Constrain Longitude to +/- PI
        /// </summary>
        /// <param name="longitude"></param>
        /// <returns></returns>
        public static decimal ConstrainLongitude(this int longitude)
        {
            var newLongitude = (decimal) longitude;
            while (newLongitude > Constants.H3.M_PI)
            {
                newLongitude -= 2m * Constants.H3.M_PI;
            }

            while (newLongitude < -Constants.H3.M_PI)
            {
                newLongitude += 2m * Constants.H3.M_PI;
            }

            return newLongitude;
        }

        /// <summary>
        /// Convert from decimal degrees to radians.
        /// </summary>
        /// <param name="degrees">The decimal degrees</param>
        /// <returns>The corresponding radians</returns>
        /// <remarks>
        /// 3.7.1
        /// geoCoord.c
        /// double H3_EXPORT(degsToRads)
        /// </remarks>
        public static decimal DegreesToRadians(this decimal degrees)
        {
            return degrees * Constants.H3.M_PI_180;
        }

        /// <summary>
        /// Convert decimal degrees to radians
        /// </summary>
        /// <param name="degrees"></param>
        public static decimal DegreesToRadians(this int degrees)
        {
            return degrees * Constants.H3.M_PI_180;
        }

        /// <summary>
        /// Convert from radians to decimal degrees.
        /// </summary>
        /// <param name="radians">The radians</param>
        /// <returns>The corresponding decimal degrees</returns>
        /// <remarks>
        /// 3.7.1
        /// geoCoord.c
        /// double H3_EXPORT(radsToDegs)
        /// </remarks>
        public static decimal RadiansToDegrees(this decimal radians)
        {
            return radians * Constants.H3.M_180_PI;
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
        /// <remarks>
        /// 3.7.1
        /// mathExtensions.c
        /// int64_t _ipow
        /// </remarks>
        internal static long Power(this long baseValue, long power)
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
        /// <remarks>
        /// 3.7.1
        /// geoCoord.c
        /// int64_t H3_EXPORT(numHexagons)
        /// </remarks>
        public static long NumHexagons(this int res)
        {
            return 2 + 120 * 7L.Power(res);
        }
        
        /// <summary>
        /// Returns whether or not a resolution is a Class III grid. Note that odd
        ///  resolutions are Class III and even resolutions are Class II.
        /// </summary>
        /// <param name="res">The H3 resolution</param>
        /// <returns>Returns true if the resolution is class III grid, otherwise false.</returns>
        /// <remarks>
        /// 3.7.1
        /// h3Index.c
        /// int H3_EXPORT(h3IsResClassIII)
        /// </remarks>
        public static bool IsResClassIii(this int res) => res % 2 == 1;


        /// <summary>
        /// Square of a number
        /// </summary>
        /// <param name="x">The input number</param>
        /// <returns>The square of the input number</returns>
        /// <remarks>
        /// vec3d.c
        /// double _square
        /// </remarks>
        internal static decimal Square(this decimal x)
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
        /// <remarks>
        /// 3.7.1
        /// h3Index.c
        /// H3Index H3_EXPORT(stringToH3)
        /// </remarks>
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
        /// <remarks>
        /// 3.7.1
        /// h3Index.c
        /// static bool _isValidChildRes
        /// </remarks>
        internal static bool IsValidChildRes(this int parentRes, int childRes)
        {
            return childRes >= parentRes &&
                   childRes <= Constants.H3.MaxH3Resolution;
        }

        /// Generates all pentagons at the specified resolution
        ///
        /// <param name="res">The resolution to produce pentagons at.</param>
        /// <returns>Output List.</returns>
        /// <remarks>
        /// 3.7.1
        /// h3Index.c
        /// void H3_EXPORT(getPentagonIndexes)
        /// </remarks>
        public static List<H3Index> GetPentagonIndexes(this int res)
        {
            var results = new List<H3Index>();
            for (var bc = 0; bc < Constants.H3.BaseCellsCount; bc++)
            {
                if (!bc.IsBaseCellPentagon())
                {
                    continue;
                }
                var pentagon = new H3Index(res, bc, 0);
                results.Add(pentagon);
            }

            return results;
        }

        /// <summary>
        /// compact takes a set of hexagons all at the same resolution and compresses
        /// them by pruning full child branches to the parent level. This is also done
        /// for all parents recursively to get the minimum number of hex addresses that
        /// perfectly cover the defined space.
        /// </summary>
        /// <param name="h3Set"> Set of hexagons</param>
        /// <returns>status code and compacted hexes</returns>
        /// <remarks>
        /// Gonna do this a bit differently, allowing for varying
        /// resolutions in input data.  Also, this is a front for <see cref="FlexiCompact"/>
        /// that tries to maintain the same restrictions the original H3 compact enforces.
        ///
        /// 3.7.1
        /// h3index.c
        /// int H3_EXPORT(compact)
        /// </remarks>
        public static (int, List<H3Index>) Compact(this List<H3Index> h3Set)
        {
            if (h3Set == null || h3Set.Count == 0)
            {
                return (Constants.H3Index.COMPACT_SUCCESS, new List<H3Index>());
            }

            if (h3Set.All(h => h.Resolution == 0))
            {
                // No compaction possible, just copy the set to output
                return (Constants.H3Index.COMPACT_SUCCESS, h3Set);
            }

            //  Compact assumes that all cells are the same resolution and uses first cell
            int testResolution = h3Set[0].Resolution;
            if (h3Set.Any(h => h.Resolution != testResolution))
            {
                return (Constants.H3Index.COMPACT_BAD_DATA, h3Set);
            }

            return h3Set.Distinct().Count() != h3Set.Count
                       ? (Constants.H3Index.COMPACT_DUPLICATE, h3Set)
                       : h3Set.FlexiCompact();
        }

        /// A slightly different approach to the problem of compacting with some
        /// flexibility.  All resolutions are handled, duplicates are avoided,
        /// and we shouldn't have overlapping children in case the parent was
        /// provided in the original data.
        public static (int, List<H3Index>) FlexiCompact(this List<H3Index> h3Set)
        {
            if (h3Set == null || h3Set.Count == 0)
            {
                return (Constants.H3Index.COMPACT_SUCCESS, new List<H3Index>());
            }

            if (h3Set.All(h => h.Resolution == 0))
            {
                // No compaction possible, just copy the set to output
                return (Constants.H3Index.COMPACT_SUCCESS, h3Set);
            }

            var finalPool = new HashSet<H3Index>();
            var testPool = new HashSet<H3Index>();

            foreach (var index in h3Set)
            {
                var fakePool = index.Resolution == 0
                               ? finalPool
                               : testPool;
                fakePool.Add(index);
            }

            var maxResolution = testPool.Select(h => h.Resolution).Max();
            while (testPool.Count > 0)
            {
                //  Grab the cells of a resolution from the testpool, and remove them as we're going
                //  to process them.
                var currentCells = testPool.Where(h => h.Resolution == maxResolution).ToList();
                foreach (var cell in currentCells)
                {
                    testPool.Remove(cell);
                }

                var tally = new Dictionary<H3Index, List<H3Index>>();

                //  Get the parent of each cell, and use that as a key pointing to siblings
                foreach (var cell in currentCells)
                {
                    var parent = cell.ToParent(maxResolution - 1);
                    if (!tally.ContainsKey(parent))
                    {
                        tally[parent] = new List<H3Index>();
                    }

                    tally[parent].Add(cell);
                }

                foreach (var key in tally.Keys)
                {
                    if (!key.IsValid())
                    {
                        return (Constants.H3Index.COMPACT_BAD_DATA, new List<H3Index>());
                    }

                    var neededChildren = key.IsPentagon
                                             ? 6
                                             : 7;

                    if (tally[key].Count == neededChildren)
                    {
                        //  We've got all the children of a cell. Place parent in testpool
                        //  for possible further compression.
                        testPool.Add(key);
                    }
                    else
                    {
                        //  Don't have all the children. What we have, we place in final,
                        //  so long as the parent didn't sneak in earlier (duplicate data
                        //  or resolution 0 that got yanked out at the beginning)
                        if (testPool.Contains(key))
                        {
                            continue;
                        }
                        foreach (var index in tally[key])
                        {
                            finalPool.Add(index);
                        }
                    }
                }

                maxResolution--;
            }

            return (Constants.H3Index.COMPACT_SUCCESS, finalPool.ToList());
        }

        /// <summary>
        /// Maximum number of cells that result from the kRing algorithm with the given
        /// k. Formula source and proof: https://oeis.org/A003215
        /// </summary>
        /// <param name="k">k value, k &gt;= 0.</param>
        /// <remarks>
        /// 3.7.1
        /// algos.c
        /// int H3_EXPORT(maxKringSize)
        /// </remarks>
        public static int MaxKringSize(this int k)
        {
            return 3 * k * (k + 1) + 1;
        }

        /// <summary>
        /// Normalize longitude, dealing with transmeridian arcs
        /// </summary>
        /// <remarks>
        /// 3.7.1
        /// polygonAlgos.h
        /// #define NORMALIZE_LON
        /// </remarks>
        public static decimal NormalizeLongitude(this decimal longitude, bool isTransmeridian)
        {
            return isTransmeridian && longitude < 0
                       ? longitude + Constants.H3.M_2PI
                       : longitude;
        }

    }
}
