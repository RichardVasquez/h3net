namespace h3net.API
{
    public class GeoFence
    {
        public int numVerts;
        public GeoCoord[] verts;

        public GeoFence()
        {
            verts = new[]
            {
                new GeoCoord(0.0,0.0), new GeoCoord(0.0,0.0)
            };
            numVerts = 0;

        }

    }
}

