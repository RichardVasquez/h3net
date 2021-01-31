using System.Diagnostics;

namespace H3Lib
{
    /// <summary>
    /// A wrapper class for storing GeoCoords within a linked list
    /// </summary>
    [DebuggerDisplay("Lat: {Latitude} Lon: {Longitude}")]
    public class LinkedGeoCoord
    {
        /// <summary>
        /// Vertex being held
        /// </summary>
        private readonly GeoCoord _gc;

        /// <summary>
        /// Latitude of vertex
        /// </summary>
        public decimal Latitude => _gc.Latitude;
        /// <summary>
        /// longitude of vertex
        /// </summary>
        public decimal Longitude => _gc.Longitude;

        /// <summary>
        /// Return the actual vertex, read only
        /// </summary>
        public GeoCoord Vertex => _gc;

        /// <summary>
        /// constructor
        /// </summary>
        public LinkedGeoCoord()
        {
            _gc = default;
        }

        /// <summary>
        /// constructor with vertex
        /// </summary>
        public LinkedGeoCoord(GeoCoord gc)
        {
            _gc = gc;
        }

        /// <summary>
        /// mutator to change vertex
        /// </summary>
        public LinkedGeoCoord Replacement(GeoCoord gc)
        {
            return new LinkedGeoCoord(gc);
        }
    }
}
