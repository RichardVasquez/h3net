using System;

namespace H3Lib
{
    /// <summary>
    /// IJ Hexagon coordinates.
    ///
    /// Each axis is spaced 120 degrees apart
    /// </summary>
    public readonly struct CoordIj : IEquatable<CoordIj>
    {
        /// <summary>
        /// I Component
        /// </summary>
        public readonly int I;
        /// <summary>
        /// J component
        /// </summary>
        public readonly int J;

        public CoordIj(int i, int j) : this()
        {
            I = i;
            J = j;
        }

        public CoordIj(CoordIj ij)
        {
            I = ij.I;
            J = ij.J;
        }

        public bool Equals(CoordIj other)
        {
            return I == other.I && J == other.J;
        }

        public override bool Equals(object obj)
        {
            return obj is CoordIj other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(I, J);
        }

        public static bool operator ==(CoordIj left, CoordIj right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(CoordIj left, CoordIj right)
        {
            return !left.Equals(right);
        }
    }
}
