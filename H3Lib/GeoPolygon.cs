using System.Collections.Generic;

namespace H3Lib
{
    public class GeoPolygon
    {
        public Geofence Geofence;
        public int numHoles;
        public List<Geofence> holes;
    }

}
