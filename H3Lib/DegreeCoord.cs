using H3Lib.Extensions;

namespace H3Lib
{
    /// <summary>
    /// A simple class to process degree based measurements.
    /// </summary>
    public readonly struct DegreeCoord
    {
        public readonly decimal Latitude;
        public readonly decimal Longitude;

        public DegreeCoord(decimal latitude, decimal longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        /// <summary>
        /// Converts the values for this struct into a new GeoCoord.
        /// </summary>
        public GeoCoord ToGeoCoord()
        {
            return new GeoCoord(Latitude.DegreesToRadians(), Longitude.DegreesToRadians());
        }
    }
}
