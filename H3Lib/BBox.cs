using System;

namespace H3Lib
{
    /// <summary>
    /// Geographic bounding box with coordinates defined in radians
    /// </summary>
    public readonly struct BBox:IEquatable<BBox>
    {
        public readonly double North;
        public readonly double South;
        public readonly double East;
        public readonly double West;

        /// <summary>
        /// Whether the given bounding box crosses the antimeridian
        /// </summary>
        /// <!--
        /// bbox.c
        /// bboxIsTransmeridian
        /// -->
        public bool IsTransmeridian => East < West;

        public BBox(double n, double s, double e, double w)
        {
            North = n;
            South = s;
            East = e;
            West = w;
        }

        public bool Equals(BBox other)
        {
            return
                Math.Abs(North - other.North) < Constants.H3.EPSILON_RAD &&
                Math.Abs(South - other.South) < Constants.H3.EPSILON_RAD &&
                Math.Abs(East - other.East) < Constants.H3.EPSILON_RAD &&
                Math.Abs(West - other.West) < Constants.H3.EPSILON_RAD;
        }

        public override bool Equals(object obj)
        {
            return obj is BBox other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(North, South, East, West);
        }

        public static bool operator ==(BBox left, BBox right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(BBox left, BBox right)
        {
            return !left.Equals(right);
        }
    }
}
