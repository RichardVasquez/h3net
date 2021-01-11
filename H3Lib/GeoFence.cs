namespace H3Lib
{
    public class GeoFence
    {
        public int NumVerts;
        public GeoCoord[] Verts;

        public bool IsEmpty => NumVerts == 0;

        public GeoFence()
        {
            Verts = new[]
                    {
                        new GeoCoord(0.0,0.0), new GeoCoord(0.0,0.0)
                    };
            NumVerts = 0;

        }
    }
}
