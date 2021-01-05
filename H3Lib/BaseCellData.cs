using System;

namespace H3Lib
{
    /// <summary>
    /// Information on a single base cell
    /// </summary>
    /// <!--
    /// baseCells.h
    /// typedef struct BaseCellData
    /// -->
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

        public BaseCellData(int face, int faceI, int faceJ, int faceK, int isPentagon, int offset1, int offset2) : this()
        {
            HomeFijk = new FaceIjk(face, new CoordIjk(faceI, faceJ, faceK));
            IsPentagon = isPentagon;
            ClockwiseOffsetPentagon = new[] {offset1, offset2};
        }

        public bool Equals(BaseCellData other)
        {
            return Equals(HomeFijk, other.HomeFijk) &&
                   IsPentagon == other.IsPentagon &&
                   Equals(ClockwiseOffsetPentagon, other.ClockwiseOffsetPentagon);
        }

        public override bool Equals(object obj)
        {
            return obj is BaseCellData other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(HomeFijk, IsPentagon, ClockwiseOffsetPentagon);
        }

        public static bool operator ==(BaseCellData left, BaseCellData right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(BaseCellData left, BaseCellData right)
        {
            return !left.Equals(right);
        }
    }
}
