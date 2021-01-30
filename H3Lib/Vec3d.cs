using System;
using System.Runtime.CompilerServices;

namespace H3Lib
{
    /// <summary>
    /// 3D floating point structure
    /// </summary>
    public readonly struct Vec3d:IEquatable<Vec3d>
    {
        /// <summary>
        /// X Coordinate
        /// </summary>
        public readonly double X;
        /// <summary>
        /// Y Coordinate
        /// </summary>
        public readonly double Y;
        /// <summary>
        /// Z Coordinate
        /// </summary>
        public readonly double Z;

        /// <summary>
        /// Constructor
        /// </summary>
        public Vec3d(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        /// <summary>
        /// Debug info in string format
        /// </summary>
        public override string ToString()
        {
            return $"Vec3d (X,Y,Z) {X:F6}, {Y:F6}, {Z:F6}";
        }

        /// <summary>
        /// Equality test
        /// </summary>
        public bool Equals(Vec3d other)
        {
            return X.Equals(other.X) &&
                   Y.Equals(other.Y) &&
                   Z.Equals(other.Z);
        }

        /// <summary>
        /// Equality test
        /// </summary>
        public override bool Equals(object obj)
        {
            return obj is Vec3d other &&
                   Equals(other);
        }

        /// <summary>
        /// Hashcode for identity
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y, Z);
        }

        /// <summary>
        /// Equality operator
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(Vec3d left, Vec3d right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// inequality operator
        /// </summary>
        public static bool operator !=(Vec3d left, Vec3d right)
        {
            return !left.Equals(right);
        }
    }
}
