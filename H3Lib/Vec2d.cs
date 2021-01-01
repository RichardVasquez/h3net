using System;
using System.Diagnostics;

namespace H3Lib
{
    /// <summary>
    /// 2D floating point vector functions.
    /// </summary>
    [DebuggerDisplay("X: {X} Y: {Y}")]
    public class Vec2d:IEquatable<Vec2d>
    {
        public double X;
        public double Y;

        public Vec2d(double x, double y) 
        {
            X = x;
            Y = y;
        }

        public Vec2d()
        {
            X = 0;
            Y = 0;
        }

        /**
         * Finds the intersection between two lines. Assumes that the lines intersect
         * and that the intersection is not at an endpoint of either line.
         * @param p0 The first endpoint of the first line.
         * @param p1 The second endpoint of the first line.
         * @param p2 The first endpoint of the second line.
         * @param p3 The second endpoint of the second line.
         * @param inter The intersection point.
         */
        /// <summary>
        /// Finds the intersection between two lines. Assumes that the lines intersect
        /// and that the intersection is not at an endpoint of either line.
        /// </summary>
        /// <param name="p0">The first endpoint of the first line</param>
        /// <param name="p1">The second endpoint of the first line</param>
        /// <param name="p2">The first endpoint of the second line</param>
        /// <param name="p3">The first endpoint of the first line</param>
        /// <param name="inter">The second endpoint of the second line</param>
        public static void _v2dIntersect(Vec2d p0,  Vec2d p1,  Vec2d p2, Vec2d p3, ref Vec2d inter)
        {
            inter = _v2dIntersect(p0, p1, p2, p3);
        }

        public static Vec2d _v2dIntersect(Vec2d p0, Vec2d p1, Vec2d p2, Vec2d p3)
        {
            var s1 = new Vec2d(p1.X - p0.X, p1.Y - p0.Y);
            var s2 = new Vec2d(p3.X - p2.X, p3.Y - p2.Y);

            double t = (s2.X * (p0.Y - p2.Y) - s2.Y * (p0.X - p2.X)) /
                       (-s2.X * s1.Y + s1.X * s2.Y);

            return new Vec2d
                (
                 p0.X + (t * s1.X),
                 p0.Y + (t * s1.Y)
                );
        }

        public static Vec2d Intersection(Vec2d p0, Vec2d p1, Vec2d p2, Vec2d p3)
        {
            return _v2dIntersect(p0, p1, p2, p3);
        }

        /// <summary>
        /// Whether two 2D vectors are equal. Does not consider possible false
        /// negatives due to floating-point errors.
        /// </summary>
        /// <param name="v1">First vector to compare</param>
        /// <param name="v2">Second vector to compare</param>
        /// <returns>Whether the vectors are equal</returns>
        public static bool _v2dEquals( Vec2d v1,  Vec2d v2)
        {
            return v1 == v2;
        }

        /// <summary>Calculates the magnitude of a 2D Cartesian vector</summary>
        /// <param name="v">The 2d Cartesian vector.</param>
        /// <returns>The magnitude of the vector</returns>
        public static double _v2dMag(Vec2d v)
        {
            return Math.Sqrt(v.X * v.X + v.Y * v.Y);
        }

        public bool Equals(Vec2d other)
        {
            if (ReferenceEquals(null, other))
                return false;
            if (ReferenceEquals(this, other))
                return true;
            return
                Math.Abs(X - other.X) < double.Epsilon &&
                Math.Abs(Y - other.Y) < double.Epsilon;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            return obj.GetType() == GetType() && Equals((Vec2d) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (X.GetHashCode() * 397) ^ Y.GetHashCode();
            }
        }

        public static bool operator ==(Vec2d left, Vec2d right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Vec2d left, Vec2d right)
        {
            return !Equals(left, right);
        }
    }
    
}
