namespace H3Lib.Extensions
{
    public static    class GeoCoordExtensions
    {
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
