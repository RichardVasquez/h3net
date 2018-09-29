using System.Collections.Generic;

namespace h3net.API
{
    public class GeoPolygon
    {
        public GeoFence geofence;
        public int numHoles;
        public List<GeoFence> holes;
    }
}
