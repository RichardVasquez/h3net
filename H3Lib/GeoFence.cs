namespace H3Lib
{
    public class Geofence
    {
        public int NumVerts;
        public GeoCoord[] Verts;

        public Geofence()
        {
            Verts = new[]
                    {
                        new GeoCoord(0.0,0.0), new GeoCoord(0.0,0.0)
                    };
            NumVerts = 0;

        }
    }
}
