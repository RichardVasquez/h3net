namespace H3Lib
{
    /// <summary>
    /// similar to GeoBoundary, but requires more alloc work
    /// </summary>
    public class GeoFence
    {
        /// <summary>
        /// number of vertices
        /// </summary>
        public int NumVerts;
        /// <summary>
        /// vertices in ccw order
        /// </summary>
        public GeoCoord[] Verts;

        /// <summary>
        /// Indicates if the geofence has no vertices
        /// </summary>
        public bool IsEmpty => NumVerts == 0;

        /// <summary>
        /// constructor
        /// </summary>
        public GeoFence()
        {
            Verts = new[]
                    {
                        new GeoCoord(0.0m,0.0m), new GeoCoord(0.0m,0.0m)
                    };
            NumVerts = 0;
        }
    }
}
