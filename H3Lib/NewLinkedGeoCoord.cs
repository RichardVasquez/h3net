using System.Diagnostics;

namespace H3Lib
{
    [DebuggerDisplay("Lat: {Latitude} Lon: {Longitude}")]
    public class NewLinkedGeoCoord
    {
        private GeoCoord _gc;

        public double Latitude => _gc.Latitude;
        public double Longitude => _gc.Longitude;

        public GeoCoord Vertex => _gc;

        public NewLinkedGeoCoord()
        {
            _gc = default;
        }

        public NewLinkedGeoCoord(GeoCoord gc)
        {
            _gc = gc;
        }
    }
}
