using System.Diagnostics;

namespace H3Lib
{
    [DebuggerDisplay("Lat: {Latitude} Lon: {Longitude}")]
    public class LinkedGeoCoord
    {
        private readonly GeoCoord _gc;

        public double Latitude => _gc.Latitude;
        public double Longitude => _gc.Longitude;

        public GeoCoord Vertex => _gc;

        public LinkedGeoCoord()
        {
            _gc = default;
        }

        public LinkedGeoCoord(GeoCoord gc)
        {
            _gc = gc;
        }

        public LinkedGeoCoord Replacement(GeoCoord gc)
        {
            return new LinkedGeoCoord(gc);
        }
    }
}
