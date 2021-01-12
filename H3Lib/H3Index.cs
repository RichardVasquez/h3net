using System;
using System.Diagnostics;

namespace H3Lib
{
    /// <summary>
    /// H3Index utility functions
    /// </summary>
    [DebuggerDisplay("Value: {H3Value} => {ToString()}")]
    public class H3Index:IEquatable<H3Index>,IEquatable<ulong>
    {
    #region base value and constructors
        /// <summary>
        /// Where the actual index is stored.
        /// </summary>
        public ulong H3Value;

        public H3Index(ulong val) 
        {
            H3Value = val;
        }

        public H3Index()
        {
            H3Value = 0;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Integer resolution of an H3 index.  
        /// </summary>
        /// <!--
        /// h3index.c
        /// int H3_EXPORT(h3GetResolution)(H3Index h)
        /// -->
        public int Resolution
        {
            get => (int) ((H3Value & StaticData.H3Index.H3_RES_MASK) >> StaticData.H3Index.H3_RES_OFFSET);
            set => H3Value = (H3Value & StaticData.H3Index.H3_RES_MASK_NEGATIVE) |
                             ((ulong)value << StaticData.H3Index.H3_RES_OFFSET);
        }

        /// <summary>
        /// Integer base cell of H3
        /// </summary>
        /// <!--
        /// h3index.c
        /// int H3_EXPORT(h3GetBaseCell)
        /// -->
        public int BaseCell
        {
            get => (int)((H3Value & StaticData.H3Index.H3_BC_MASK) >> StaticData.H3Index.H3_BC_OFFSET);
            set => H3Value = (H3Value & StaticData.H3Index.H3_BC_MASK_NEGATIVE) |
                             ((ulong)value << StaticData.H3Index.H3_BC_OFFSET);
        }

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
        public H3Mode Mode
        {
            get => (H3Mode) ((H3Value & StaticData.H3Index.H3_MODE_MASK) >> StaticData.H3Index.H3_MODE_OFFSET);
            set => H3Value = H3Value & StaticData.H3Index.H3_MODE_MASK_NEGATIVE |
                             ((ulong)value << StaticData.H3Index.H3_MODE_OFFSET);
        }

        /// <summary>
        /// High bit of H3
        /// </summary>
        public int HighBit
        {
            get => (int) ((H3Value & StaticData.H3Index.H3_HIGH_BIT_MASK) >> StaticData.H3Index.H3_MAX_OFFSET);
            set => H3Value = (H3Value & StaticData.H3Index.H3_HIGH_BIT_MASK_NEGATIVE) |
                             ((ulong) value << StaticData.H3Index.H3_MAX_OFFSET);
        }

        public int ReservedBits
        {
            get => (int) ((H3Value & StaticData.H3Index.H3_RESERVED_MASK) >> StaticData.H3Index.H3_RESERVED_OFFSET);
            set => H3Value = (H3Value & StaticData.H3Index.H3_RESERVED_MASK_NEGATIVE) | ((ulong) value << StaticData.H3Index.H3_RESERVED_OFFSET);
        }

        /// <summary>
        /// Gets the resolution res integer digit (0-7) of h3.
        /// </summary>
        public Direction GetIndexDigit(int res)
        {
            return (Direction) ((H3Value >> ((Constants.MAX_H3_RES - res) * StaticData.H3Index.H3_PER_DIGIT_OFFSET)) & StaticData.H3Index.H3_DIGIT_MASK);
        }

        public void SetIndexDigit(int res, ulong digit)
        {
            H3Value = (H3Value & ~(StaticData.H3Index.H3_DIGIT_MASK << ((Constants.MAX_H3_RES - res) * StaticData.H3Index.H3_PER_DIGIT_OFFSET))) |
                      (digit << (Constants.MAX_H3_RES - res) * StaticData.H3Index.H3_PER_DIGIT_OFFSET);
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
            
        #endregion


        /// <summary>
        /// Converts an H3 index into a string representation.
        /// </summary>
        /// <returns>The string representation of the H3 index as a hexadecimal number</returns>
        public override string ToString()
        {
            string s = H3Value.ToString("X").PadLeft(16, '0');
            return s.Substring(s.Length - 16, 16);
        }

#region

        
        public static implicit operator H3Index(ulong u) => new H3Index(u);
        public static implicit operator ulong(H3Index h3) => h3.H3Value;

#endregion

    public override int GetHashCode()
    {
        return H3Value.GetHashCode();
    }

    public static bool operator ==(H3Index left, H3Index right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(H3Index left, H3Index right)
    {
        return !Equals(left, right);
    }

    public bool Equals(H3Index other)
    {
        if (ReferenceEquals(null, other))
            return false;
        if (ReferenceEquals(this, other))
            return true;
        return H3Value == other.H3Value;
    }

    public bool Equals(ulong other)
    {
        return H3Value == other;
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj))
            return false;
        if (ReferenceEquals(this, obj))
            return true;
        return Equals((H3Index) obj);
    }
    }
}
