using System;
using System.Diagnostics;

namespace H3Lib
{
    /// <summary>
    /// 2D floating point vector functions.
    /// </summary>
    [DebuggerDisplay("X: {X} Y: {Y}")]
    public readonly struct Vec2d:IEquatable<Vec2d>
    {
        public readonly double X;
        public readonly double Y;

        public Vec2d(double x, double y) 
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Calculates the magnitude of a 2D cartesian vector.
        /// </summary>
        /// <!--
        /// vec2d.c
        /// double _v2dMag
        /// -->
        public double Magnitude => Math.Sqrt(X * X + Y * Y);

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
        /// <returns>The intersection point.</returns>
        /// <!--
        /// vec2d.c
        /// _v2dIntersect
        /// -->
        public static Vec2d FindIntersection(Vec2d p0,  Vec2d p1,  Vec2d p2, Vec2d p3)
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

        public bool Equals(Vec2d other)
        {
            return
                Math.Abs(X - other.X) < Constants.H3.DBL_EPSILON &&
                Math.Abs(Y - other.Y) < Constants.H3.DBL_EPSILON;
            
        }
        public override bool Equals(object obj)
        {
            return obj is Vec2d other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }

        public static bool operator ==(Vec2d left, Vec2d right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Vec2d left, Vec2d right)
        {
            return !left.Equals(right);
        }

        public override string ToString()
        {
            return $"Vec2d: X: {X} Y: {Y}";
        }
    }
    
}
