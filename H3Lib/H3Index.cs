using System;
using System.Diagnostics;
using System.Globalization;
using H3Lib.Extensions;

namespace H3Lib
{
    /// <summary>
    /// H3Index utility functions
    /// </summary>
    [DebuggerDisplay("Value: {Value} => {ToString()}")]
    public readonly struct H3Index:IEquatable<H3Index>,IEquatable<ulong>,IComparable<H3Index>
    {
        /// <summary>
        /// Where the actual index is stored.
        /// </summary>
        public readonly ulong Value;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="val"></param>
        public H3Index(ulong val) : this()
        {
            Value = val;
        }

        /// <summary>
        /// Constructor
        /// 
        /// The contained chain of operations are common enough that
        /// it made sense to hoist them into a constructor.
        ///
        /// Dual implementations exist to avoid type casting of
        /// <see cref="Direction"/> to <see cref="int"/> or the reverse.
        /// </summary>
        /// <param name="res">resolution of the H3Index cell</param>
        /// <param name="baseCell">Base cell to be placed on</param>
        /// <param name="initDigit">Initial digit of heIndex cell</param>
        /// <remarks>
        /// 3.7.1
        /// h3index.c
        /// void setH3Index
        /// </remarks>
        public H3Index(int res, int baseCell, Direction initDigit)
        {
            H3Index h = Constants.H3Index.Init;
            h = h.SetMode(H3Mode.Hexagon).SetResolution(res).SetBaseCell(baseCell);

            for (var r = 1; r <= res; r++)
            {
                h = h.SetIndexDigit(r, initDigit);
            }

            Value = h.Value;
        }
        
        /// <summary>
        /// Constructor
        /// 
        /// The contained chain of operations are common enough that
        /// it made sense to hoist them into a constructor.
        ///
        /// Dual implementations exist to avoid type casting of
        /// <see cref="Direction"/> to <see cref="int"/> or the reverse.
        /// </summary>
        /// <param name="res">resolution of the H3Index cell</param>
        /// <param name="baseCell">Base cell to be placed on</param>
        /// <param name="initDigit">Initial digit of h3Index cell</param>
        public H3Index(int res, int baseCell, int initDigit)
        {
            H3Index h = Constants.H3Index.Init;
            h = h.SetMode(H3Mode.Hexagon).SetResolution(res).SetBaseCell(baseCell);

            for (var r = 1; r <= res; r++)
            {
                h = h.SetIndexDigit(r, (Direction) initDigit);
            }

            Value = h.Value;
        }

        /// <summary>
        /// Tries to create an H3Index based on a string value.
        ///
        /// Will try a hex value, starting with "0x" or "x", then
        /// a stock hex value, then fall back to a decimal number.
        ///
        /// Failure results in the value being a <see cref="Constants.H3Index.Null"/>
        /// </summary>
        /// <param name="s">string value to convert</param>
        public H3Index(string s)
        {
            Value = Constants.H3Index.Null;
            //  Convert starting with 0x
            if (s.StartsWith("0x") || s.StartsWith("0X"))
            {
                s = s.Substring(2);
                Value = ulong.TryParse(s, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out ulong hex1)
                            ? hex1
                            : Constants.H3Index.Null;
                return;
            }

            //  Convert starting with x
            if (s.StartsWith("x") || s.StartsWith("X"))
            {
                s = s.Substring(1);
                Value = ulong.TryParse(s, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out ulong hex2)
                            ? hex2
                            : Constants.H3Index.Null;
                return;
            }

            //  Convert hexadecimal based number, then finally a decimal based number
            Value = ulong.TryParse(s, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out ulong hex3)
                        ? hex3
                        : ulong.TryParse(s, out ulong dec)
                            ? dec
                            : Constants.H3Index.Null;
        }

        /// <summary>
        /// Integer resolution of an H3 index.  
        /// </summary>
        /// <remarks>
        /// 3.7.1
        /// h3index.c
        /// int H3_EXPORT(h3GetResolution)(H3Index h)
        /// </remarks>
        public int Resolution =>
            (int) ((Value & Constants.H3Index.ResolutionMask) >>
                   Constants.H3Index.ResolutionOffset);

        /// <summary>
        /// Integer base cell of H3
        /// </summary>
        /// <remarks>
        /// 3.7.1
        /// h3index.c
        /// int H3_EXPORT(h3GetBaseCell)
        /// </remarks>
        public int BaseCell =>
            (int) ((Value & Constants.H3Index.BaseCellMask) >>
                   Constants.H3Index.BaseCellOffset);

        /// <summary>
        /// Returns the highest resolution non-zero digit in an H3Index.
        /// </summary>
        /// <remarks>
        /// 3.7.1
        /// h3index.c
        /// Direction _h3LeadingNonZeroDigit
        /// </remarks>
        public Direction LeadingNonZeroDigit
        {
            get
            {
                for (var r = 1; r <= Resolution; r++)
                {
                    if (GetIndexDigit(r) > 0)
                    {
                        return GetIndexDigit(r);
                    }
                }
                
                return Direction.CenterDigit;
            }
        }

        /// <summary>
        /// Takes an H3Index and determines if it is actually a pentagon.
        /// </summary>
        /// <returns>Returns true if it is a pentagon, otherwise false.</returns>
        /// <remarks>
        /// 3.7.1
        /// h3Index.c
        /// int H3_EXPORT(h3IsPentagon)
        /// </remarks>
        public bool IsPentagon =>
            BaseCell.IsBaseCellPentagon() &&
            LeadingNonZeroDigit == Direction.CenterDigit;

        /// <summary>
        /// Integer mode of H3
        /// </summary>
        public H3Mode Mode =>
            (H3Mode) ((Value & Constants.H3Index.ModeMask) >>
                      Constants.H3Index.ModeOffset);

        /// <summary>
        /// High bit of H3
        /// </summary>
        public int HighBit =>
            (int) ((Value & Constants.H3Index.HighBitMask) >>
                   Constants.H3Index.MaxOffset);

        /// <summary>
        /// Reserved bits of H3Index
        /// </summary>
        public int ReservedBits =>
            (int) ((Value & Constants.H3Index.ReservedMask) >>
                   Constants.H3Index.ReservedOffset);

        /// <summary>
        /// Gets the resolution res integer digit (0-7) of h3.
        /// </summary>
        public Direction GetIndexDigit(int res)
        {
            return (Direction)
                ((Value >>
                  ((Constants.H3.MaxH3Resolution - res) * Constants.H3Index.PerDigitOffset)) &
                 Constants.H3Index.DigitMask);
        }

        /// <summary>
        /// returns the number of pentagons (same at any resolution)
        /// </summary>
        public static int PentagonIndexCount => Constants.H3.PentagonsCount;

        /// <summary>
        /// IsResClassIii takes a hexagon ID and determines if it is in a
        /// Class III resolution (rotated versus the icosahedron and subject
        /// to shape distortion adding extra points on icosahedron edges, making
        /// them not true hexagons).
        /// </summary>
        /// <remarks>
        /// 3.7.1
        /// h3index.c
        /// int H3_EXPORT(h3IsResClassIII)
        /// </remarks>
        public bool IsResClassIii => Resolution % 2 == 1;

        /// <summary>
        /// Converts an H3 index into a string representation.
        /// </summary>
        /// <returns>The string representation of the H3 index as a hexadecimal number</returns>
        public override string ToString()
        {
            //  Fills out a 16 character wide hex number
            string s = Value.ToString("x").PadLeft(16, '0');
            //  Gets rid of leading 0s.
            while (s[0] == '0')
            {
                s = s.Substring(1);
            }
            return s;
        }

        /// <summary>
        /// Implicit conversion
        /// </summary>
        public static implicit operator H3Index(ulong u) => new H3Index(u);
        /// <summary>
        /// Implicit conversion
        /// </summary>
        public static implicit operator ulong(H3Index h3) => h3.Value;

        /// <summary>
        /// Equality test
        /// </summary>
        /// <param name="other"></param>
        public bool Equals(H3Index other)
        {
            return Value == other.Value;
        }

        /// <summary>
        /// Hashcode for identity
        /// </summary>
        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        /// <summary>
        /// Equality operator
        /// </summary>
        public static bool operator ==(H3Index left, H3Index right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Inequality operator
        /// </summary>
        public static bool operator !=(H3Index left, H3Index right)
        {
            return !left.Equals(right);
        }

        /// <summary>
        /// Compare test
        /// </summary>
        public int CompareTo(H3Index other)
        {
            return Value.CompareTo(other.Value);
        }

        /// <summary>
        /// Equal against ulong
        /// </summary>
        public bool Equals(ulong other)
        {
            return Value == other;
        }

        /// <summary>
        /// Equal test against object
        /// </summary>
        public override bool Equals(object obj)
        {
            return obj is H3Index other && Equals(other);
        }
    }
}
