using System.Collections.Generic;
using H3Lib.Extensions;

namespace H3Lib
{
    public static class Api
    {
        public static int PentagonIndexCount()
        {
            return Constants.NUM_PENTAGONS;
        }
#region Group GeoToH3
        /// <summary>
        /// find the H3 index of the resolution res cell containing the lat/lng
        /// </summary>
        public static H3Index GeoToH3(GeoCoord g, int res)
        {
            return g.ToH3Index(res);
        }

        /// <summary>
        /// find the lat/lon center point g of the cell h3
        /// </summary>
        public static void H3ToGeo(H3Index h3, out GeoCoord g)
        {
            g = h3.ToGeoCoord();
        }
#endregion
#region Group H3ToGeoBoundary

        /// <summary>
        /// give the cell boundary in lat/lon coordinates for the cell h3
        /// </summary>
        public static void H3ToGeoBoundary(H3Index h3, out GeoBoundary gb)
        {
            gb = h3.ToGeoBoundary();
        }
        
#endregion

#region Group KRing

        /// <summary>
        /// maximum number of hexagons in k-ring
        /// </summary>
        public static int MaxKringSize(int k)
        {
            return k.MaxKringSize();
        }

        /// <summary>
        /// hexagons neighbors in all directions, assuming no pentagons
        /// </summary>
        public static int HexRange(H3Index origin, int k, out List<H3Index> outHex)
        {
            (int status, var tempHex) = origin.HexRange(k);
            outHex = tempHex;
            return status;
        }

        /// <summary>
        /// hexagons neighbors in all directions, assuming no pentagons,
        /// reporting distance from origin
        /// </summary>
        public static int HexRangeDistances(H3Index origin, int k, out List<(H3Index, int)> outList)
        {
            (int status, var values) = origin.HexRangeDistances(k);
            outList = values;
            return status;
        }





#endregion
        
    }
}
