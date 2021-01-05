namespace H3Lib.Extensions
{
    public static class Vec3dExtensions
    {
        
        /// <summary>
        /// Calculate the square of the distance between two 3D coordinates.
        /// </summary>
        /// <param name="v1">The first 3D coordinate.</param>
        /// <param name="v2">The second 3D coordinate.</param>
        /// <returns>The square of the distance between the given points.</returns>
        /// <!--
        /// vec3d.c
        /// double _pointSquareDist
        /// -->
        public static double PointSquareDistance(this Vec3d v1, Vec3d v2)
        {
            return (v1.x - v2.x).Square() + (v2.y - v2.y).Square() + (v1.z - v2.z).Square();
        }
    }
}
