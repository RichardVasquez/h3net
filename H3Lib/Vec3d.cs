using System;
using System.Runtime.CompilerServices;

namespace H3Lib
{
    /// <summary>
    /// 3D floating point structure
    /// </summary>
    public readonly struct Vec3d:IEquatable<Vec3d>
    {
        public readonly double X;
        public readonly double Y;
        public readonly double Z;

        public Vec3d(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public override string ToString()
        {
            return $"Vec3d (X,Y,Z) {X:F6}, {Y:F6}, {Z:F6}";
        }

        public bool Equals(Vec3d other)
        {
            return X.Equals(other.X) &&
                   Y.Equals(other.Y) &&
                   Z.Equals(other.Z);
        }

        public override bool Equals(object obj)
        {
            return obj is Vec3d other &&
                   Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y, Z);
        }

        public static bool operator ==(Vec3d left, Vec3d right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Vec3d left, Vec3d right)
        {
            return !left.Equals(right);
        }
    }
}
