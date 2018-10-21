using System.Collections.Generic;

namespace h3net.API
{
    public class GeoPolygon
    {
        public Geofence Geofence;
        public int numHoles;
        public List<Geofence> holes;
    }
}
