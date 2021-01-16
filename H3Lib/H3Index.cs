using System;
using System.Diagnostics;
using System.Linq;

namespace H3Lib
{
    /// <summary>
    /// H3Index utility functions
    /// </summary>
    [DebuggerDisplay("Value: {Value} => 0x{ToString()}")]
    public readonly struct H3Index:IEquatable<H3Index>,IEquatable<ulong>,IComparable<H3Index>
    {
    #region base value and constructors
        /// <summary>
        /// Where the actual index is stored.
        /// </summary>
        public readonly ulong Value;

        public H3Index(ulong val) 
        {
            Value = val;
        }
        #endregion

        /// <summary>
        /// Integer resolution of an H3 index.  
        /// </summary>
        /// <!--
        /// h3index.c
        /// int H3_EXPORT(h3GetResolution)(H3Index h)
        /// -->
        public int Resolution =>
            (int) ((Value & StaticData.H3Index.H3_RES_MASK) >>
                   StaticData.H3Index.H3_RES_OFFSET);

        /// <summary>
        /// Integer base cell of H3
        /// </summary>
        /// <!--
        /// h3index.c
        /// int H3_EXPORT(h3GetBaseCell)
        /// -->
        public int BaseCell =>
            (int) ((Value & StaticData.H3Index.H3_BC_MASK) >>
                   StaticData.H3Index.H3_BC_OFFSET);

        /// <summary>
        /// Returns the highest resolution non-zero digit in an H3Index.
        /// </summary>
        /// <!--
        /// h3index.c
        /// Direction _h3LeadingNonZeroDigit
        /// -->
        public Direction LeadingNonZeroDigit
        {
            get
            {
                for (var r = 1; r < Resolution; r++)
                {
                    if (GetIndexDigit(r) > 0)
                    {
                        return GetIndexDigit(r);
                    }
                }
                
                return Direction.CENTER_DIGIT;
            }
        }

        /// <summary>
        /// Integer mode of H3
        /// </summary>
        public H3Mode Mode =>
            (H3Mode) ((Value & StaticData.H3Index.H3_MODE_MASK) >>
                      StaticData.H3Index.H3_MODE_OFFSET);

        /// <summary>
        /// High bit of H3
        /// </summary>
        public int HighBit =>
            (int) ((Value & StaticData.H3Index.H3_HIGH_BIT_MASK) >>
                   StaticData.H3Index.H3_MAX_OFFSET);

        public int ReservedBits =>
            (int) ((Value & StaticData.H3Index.H3_RESERVED_MASK) >>
                   StaticData.H3Index.H3_RESERVED_OFFSET);

        /// <summary>
        /// Gets the resolution res integer digit (0-7) of h3.
        /// </summary>
        public Direction GetIndexDigit(int res)
        {
            return (Direction)
                ((Value >>
                  ((Constants.MAX_H3_RES - res) * StaticData.H3Index.H3_PER_DIGIT_OFFSET)) &
                 StaticData.H3Index.H3_DIGIT_MASK);
        }


        /// <summary>
        /// returns the number of pentagons (same at any resolution)
        /// </summary>
        public int PentagonIndexCount => Constants.NUM_PENTAGONS;

        /// <summary>
        /// IsResClassIII takes a hexagon ID and determines if it is in a
        /// Class III resolution (rotated versus the icosahedron and subject
        /// to shape distortion adding extra points on icosahedron edges, making
        /// them not true hexagons).
        /// </summary>
        /// <!--
        /// h3index.c
        /// int H3_EXPORT(h3IsResClassIII)
        /// -->
        public bool IsResClassIii => Resolution % 2 == 1;

        /// <summary>
        /// Converts an H3 index into a string representation.
        /// </summary>
        /// <returns>The string representation of the H3 index as a hexadecimal number</returns>
        public override string ToString()
        {
            string s = Value.ToString("X").PadLeft(16, '0');
            return s.Substring(s.Length - 16, 16).ToLower();
        }

        public static implicit operator H3Index(ulong u) => new H3Index(u);
        public static implicit operator ulong(H3Index h3) => h3.Value;

        public bool Equals(H3Index other)
        {
            return Value == other.Value;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public static bool operator ==(H3Index left, H3Index right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(H3Index left, H3Index right)
        {
            return !left.Equals(right);
        }

        public int CompareTo(H3Index other)
        {
            return Value.CompareTo(other.Value);
        }

        public bool Equals(ulong other)
        {
            return Value == other;
        }

        public override bool Equals(object obj)
        {
            return obj is H3Index other && Equals(other);
        }
    }
}
