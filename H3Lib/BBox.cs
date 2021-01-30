using System;

namespace H3Lib
{
    /// <summary>
    /// Geographic bounding box with coordinates defined in radians
    /// </summary>
    public readonly struct BBox:IEquatable<BBox>
    {
        /// <summary>
        /// North limit
        /// </summary>
        public readonly double North;
        
        /// <summary>
        /// South limit
        /// </summary>
        public readonly double South;
        
        /// <summary>
        /// East limit
        /// </summary>
        public readonly double East;
        
        /// <summary>
        /// West limit
        /// </summary>
        public readonly double West;

        /// <summary>
        /// Whether the given bounding box crosses the antimeridian
        /// </summary>
        /// <!--
        /// bbox.c
        /// bboxIsTransmeridian
        /// -->
        public bool IsTransmeridian => East < West;

        /// <summary>
        /// constructor
        /// </summary>
        public BBox(double n, double s, double e, double w)
        {
            North = n;
            South = s;
            East = e;
            West = w;
        }

        /// <summary>
        /// Test for equality within measure of error against other BBox
        /// </summary>
        public bool Equals(BBox other)
        {
            return
                Math.Abs(North - other.North) < Constants.H3.EPSILON_RAD &&
                Math.Abs(South - other.South) < Constants.H3.EPSILON_RAD &&
                Math.Abs(East - other.East) < Constants.H3.EPSILON_RAD &&
                Math.Abs(West - other.West) < Constants.H3.EPSILON_RAD;
        }

        /// <summary>
        /// Test for object that can be unboxed to BBox
        /// </summary>
        public override bool Equals(object obj)
        {
            return obj is BBox other && Equals(other);
        }

        /// <summary>
        /// Hashcode for identity
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return HashCode.Combine(North, South, East, West);
        }

        /// <summary>
        /// Test for equality
        /// </summary>
        public static bool operator ==(BBox left, BBox right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Test for inequality
        /// </summary>
        public static bool operator !=(BBox left, BBox right)
        {
            return !left.Equals(right);
        }
    }
}
