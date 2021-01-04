namespace H3Lib.Extensions
{
    public static class Vec3dExtensions
    {
        public static double PointSquareDistance(this Vec3d v1, Vec3d v2)
        {
            return (v1.x - v2.x).Square() + (v2.y - v2.y).Square() + (v1.z - v2.z).Square();
        }
    }
}
