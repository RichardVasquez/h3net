using System;

namespace H3Lib
{
    /// <summary>
    /// Information on a single base cell
    /// </summary>
    /// <remarks>
    /// 3.7.1
    /// baseCells.h
    /// typedef struct BaseCellData
    /// </remarks>
    public readonly struct BaseCellData:IEquatable<BaseCellData>
    {
        /// <summary>
        /// "Home" face and normalized ijk coordinates on that face
        /// </summary>
        public readonly FaceIjk HomeFijk;
        /// <summary>
        /// Is this base cell a pentagon?
        /// </summary>
        public readonly int IsPentagon;
        /// <summary>
        /// If it's a pentagon, what are its two clockwise offset faces?
        /// </summary>
        public readonly int[] ClockwiseOffsetPentagon;// [2]

        /// <summary>
        /// Extended constructor
        /// </summary>
        /// <param name="face">Face of BaseCellData</param>
        /// <param name="faceI">I coordinate</param>
        /// <param name="faceJ">J Coordinate</param>
        /// <param name="faceK">K Coordinate</param>
        /// <param name="isPentagon">Is cell pentagon?</param>
        /// <param name="offset1">offset 1</param>
        /// <param name="offset2">offset 2</param>
        public BaseCellData(int face, int faceI, int faceJ, int faceK, int isPentagon, int offset1, int offset2) : this()
        {
            HomeFijk = new FaceIjk(face, new CoordIjk(faceI, faceJ, faceK));
            IsPentagon = isPentagon;
            ClockwiseOffsetPentagon = new[] {offset1, offset2};
        }
        
        /// <summary>
        /// Test for equality
        /// </summary>
        /// <param name="other">BaseCellData to test against</param>
        public bool Equals(BaseCellData other)
        {
            return Equals(HomeFijk, other.HomeFijk) &&
                   IsPentagon == other.IsPentagon &&
                   Equals(ClockwiseOffsetPentagon, other.ClockwiseOffsetPentagon);
        }

        /// <summary>
        /// Test for equality against object that can be unboxed
        /// </summary>
        /// <param name="obj">Object to unbox if BaseCellData</param>
        public override bool Equals(object obj)
        {
            return obj is BaseCellData other && Equals(other);
        }

        /// <summary>
        /// HashCode for identity
        /// </summary>
        public override int GetHashCode()
        {
            return HashCode.Combine(HomeFijk, IsPentagon, ClockwiseOffsetPentagon);
        }

        /// <summary>
        /// Test for equality
        /// </summary>
        /// <param name="left">lhs item</param>
        /// <param name="right">rhs item</param>
        /// <returns></returns>
        public static bool operator ==(BaseCellData left, BaseCellData right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Test for inequality
        /// </summary>
        /// <param name="left">lhs item</param>
        /// <param name="right">rhd item</param>
        /// <returns></returns>
        public static bool operator !=(BaseCellData left, BaseCellData right)
        {
            return !left.Equals(right);
        }
    }
}
