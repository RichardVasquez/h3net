using System;

namespace H3Lib
{
    /// <summary>
    /// Information to transform into an adjacent face IJK system
    /// </summary>
    public readonly struct FaceOrientIjk:IEquatable<FaceOrientIjk>
    {
        /// <summary>
        /// face number
        /// </summary>
        public readonly int Face;
        
        /// <summary>
        /// res 0 translation relative to primary face
        /// </summary>
        public readonly CoordIjk Translate;
        
        /// <summary>
        /// number of 60 degree ccw rotations relative to primary
        /// </summary>
        public readonly int Ccw60Rotations;

        /// <summary>
        /// Constructor
        /// </summary>
        public FaceOrientIjk(int f, int i, int j, int k, int c)
        {
            Face = f;
            Translate = new CoordIjk(i, j, k);
            Ccw60Rotations = c;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public FaceOrientIjk(int f, CoordIjk translate, int c)
        {
            Face = f;
            Translate = translate;
            Ccw60Rotations = c;
        }

        /// <summary>
        /// Equality test
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(FaceOrientIjk other)
        {
            return Face == other.Face &&
                   Translate.Equals(other.Translate) &&
                   Ccw60Rotations == other.Ccw60Rotations;
        }

        /// <summary>
        /// Equality test against unboxed object
        /// </summary>
        public override bool Equals(object obj)
        {
            return obj is FaceOrientIjk other && Equals(other);
        }

        /// <summary>
        /// Hashcode for identity
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return HashCode.Combine(Face, Translate, Ccw60Rotations);
        }

        /// <summary>
        /// Equality operator
        /// </summary>
        public static bool operator ==(FaceOrientIjk left, FaceOrientIjk right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Inequality operator
        /// </summary>
        public static bool operator !=(FaceOrientIjk left, FaceOrientIjk right)
        {
            return !left.Equals(right);
        }
    }
}
