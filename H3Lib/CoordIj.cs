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

        /// <summary>
        /// Constructor
        /// </summary>
        public CoordIj(int i, int j) : this()
        {
            I = i;
            J = j;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public CoordIj(CoordIj ij)
        {
            I = ij.I;
            J = ij.J;
        }

        /// <summary>
        /// Test for equality
        /// </summary>
        public bool Equals(CoordIj other)
        {
            return I == other.I && J == other.J;
        }

        /// <summary>
        /// Test for equality on object that can be unboxed to CoordIJ
        /// </summary>
        public override bool Equals(object obj)
        {
            return obj is CoordIj other && Equals(other);
        }

        /// <summary>
        /// Hashcode for identity
        /// </summary>
        public override int GetHashCode()
        {
            return HashCode.Combine(I, J);
        }

        /// <summary>
        /// Test for equality
        /// </summary>
        public static bool operator ==(CoordIj left, CoordIj right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Test for inequality
        /// </summary>
        public static bool operator !=(CoordIj left, CoordIj right)
        {
            return !left.Equals(right);
        }
        
        /// <summary>
        /// Addition operator
        /// </summary>
        public static CoordIj operator+(CoordIj c1,CoordIj c2)
        {
            return new CoordIj(c1.I + c2.I, c1.J + c2.J);
        }

        /// <summary>
        /// Subtraction operator
        /// </summary>
        public static CoordIj operator-(CoordIj c1,CoordIj c2)
        {
            return new CoordIj(c1.I - c2.I, c1.J - c2.J);
        }

        /// <summary>
        /// Multiply operator for scaling
        /// </summary>
        public static CoordIj operator *(CoordIj c, int scalar)
        {
            return new CoordIj(c.I * scalar, c.J * scalar);
        }
    }
}
