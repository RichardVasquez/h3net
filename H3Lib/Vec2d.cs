using System;
using System.Diagnostics;
using DecimalMath;

namespace H3Lib
{
    /// <summary>
    /// 2D floating point vector functions.
    /// </summary>
    [DebuggerDisplay("X: {X} Y: {Y}")]
    public readonly struct Vec2d:IEquatable<Vec2d>
    {
        /// <summary>
        /// X coordinate
        /// </summary>
        public readonly decimal X;
        /// <summary>
        /// Y Coordinate
        /// </summary>
        public readonly decimal Y;

        /// <summary>
        /// Constructor
        /// </summary>
        public Vec2d(decimal x, decimal y) 
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
        public decimal Magnitude => DecimalEx.Sqrt(X * X + Y * Y);

        /// <summary>
        /// Finds the intersection between two lines. Assumes that the lines intersect
        /// and that the intersection is not at an endpoint of either line.
        /// </summary>
        /// <param name="p0">The first endpoint of the first line</param>
        /// <param name="p1">The second endpoint of the first line</param>
        /// <param name="p2">The first endpoint of the second line</param>
        /// <param name="p3">The first endpoint of the first line</param>
        /// <returns>The intersection point.</returns>
        /// <remarks>
        /// 3.7.1
        /// vec2d.c
        /// _v2dIntersect
        /// </remarks>
        public static Vec2d FindIntersection(Vec2d p0,  Vec2d p1,  Vec2d p2, Vec2d p3)
        {
            var s1 = new Vec2d(p1.X - p0.X, p1.Y - p0.Y);
            var s2 = new Vec2d(p3.X - p2.X, p3.Y - p2.Y);

            decimal t = (s2.X * (p0.Y - p2.Y) - s2.Y * (p0.X - p2.X)) /
                       (-s2.X * s1.Y + s1.X * s2.Y);

            return new Vec2d
                (
                 p0.X + (t * s1.X),
                 p0.Y + (t * s1.Y)
                );
        }

        /// <summary>
        /// Equality test
        /// </summary>
        public bool Equals(Vec2d other)
        {
            return
                Math.Abs(X - other.X) < Constants.H3.DoubleEpsilon &&
                Math.Abs(Y - other.Y) < Constants.H3.DoubleEpsilon;
            
        }
        
        /// <summary>
        /// Equality test against unboxed object
        /// </summary>
        public override bool Equals(object obj)
        {
            return obj is Vec2d other && Equals(other);
        }

        /// <summary>
        /// Hashcode for identity
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }

        /// <summary>
        /// Equality operator
        /// </summary>
        public static bool operator ==(Vec2d left, Vec2d right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Inequality operator
        /// </summary>
        public static bool operator !=(Vec2d left, Vec2d right)
        {
            return !left.Equals(right);
        }

        /// <summary>
        /// Debug info as string
        /// </summary>
        public override string ToString()
        {
            return $"Vec2d: X: {X} Y: {Y}";
        }
    }
    
}
