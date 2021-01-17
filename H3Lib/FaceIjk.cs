using System;
using System.Diagnostics;

namespace H3Lib
{
    /// <summary>
    /// Functions for working with icosahedral face-centered hex IJK
    /// coordinate systems.
    /// </summary>
    [DebuggerDisplay("Face: {Face} Coord: {Coord}")]
    public readonly struct FaceIjk:IEquatable<FaceIjk>
    {
        /// <summary>
        /// face number
        /// </summary>
        public readonly int Face;
        
        /// <summary>
        /// ijk coordinates on that face
        /// </summary>
        public readonly CoordIjk Coord;

        public FaceIjk(int f, CoordIjk cijk)
        {
            Face = f;
            Coord = cijk;
        }

        public FaceIjk(FaceIjk fijk)
        {
            Face = fijk.Face;
            Coord = new CoordIjk(fijk.Coord);
        }

        public override string ToString()
        {
            return $"FaceIjk: Face: {Face} Coord: {Coord}";
        }

        public bool Equals(FaceIjk other)
        {
            return Face == other.Face && Coord.Equals(other.Coord);
        }

        public override bool Equals(object obj)
        {
            return obj is FaceIjk other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Face, Coord);
        }

        public static bool operator ==(FaceIjk left, FaceIjk right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(FaceIjk left, FaceIjk right)
        {
            return !left.Equals(right);
        }
    }
}
