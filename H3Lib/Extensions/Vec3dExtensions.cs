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
            return (v1.X - v2.X).Square() + (v2.Y - v2.Y).Square() + (v1.Z - v2.Z).Square();
        }

        public static Vec3d SetX(this Vec3d v3, double x)
        {
            return new Vec3d(x, v3.Y, v3.Z);
        }

        public static Vec3d SetY(this Vec3d v3, double y)
        {
            return new Vec3d(v3.X, y, v3.Z);
        }

        public static Vec3d SetZ(this Vec3d v3, double z)
        {
            return new Vec3d(v3.X, v3.Y, z);
        }
    }
}
