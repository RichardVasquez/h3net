namespace h3net.API
{
    public class Geofence
    {
        public int numVerts;
        public GeoCoord[] verts;

        public Geofence()
        {
            verts = new[]
            {
                new GeoCoord(0.0,0.0), new GeoCoord(0.0,0.0)
            };
            numVerts = 0;

        }

    }
}

