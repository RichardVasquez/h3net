using System;

namespace H3Lib
{
    /// <summary>
    /// base cell at a given ijk and required rotations into its system
    /// </summary>
    /// <!--
    /// baseCells.c
    /// typedef struct BaseCellRotation
    /// -->
    public readonly struct BaseCellRotation:IEquatable<BaseCellRotation>
    {
        /// <summary>
        /// base cell number
        /// </summary>
        public readonly int BaseCell;
        /// <summary>
        /// number of ccw 60 degree rotations relative to current face
        /// </summary>
        public readonly int CounterClockwiseRotate60;
        public BaseCellRotation(int baseCell, int counterClockwiseRotate60)
        {
            BaseCell = baseCell;
            CounterClockwiseRotate60 = counterClockwiseRotate60;
        }

        public bool Equals(BaseCellRotation other)
        {
            return BaseCell == other.BaseCell && CounterClockwiseRotate60 == other.CounterClockwiseRotate60;
        }

        public override bool Equals(object obj)
        {
            return obj is BaseCellRotation other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(BaseCell, CounterClockwiseRotate60);
        }

        public static bool operator ==(BaseCellRotation left, BaseCellRotation right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(BaseCellRotation left, BaseCellRotation right)
        {
            return !left.Equals(right);
        }
    }
}
