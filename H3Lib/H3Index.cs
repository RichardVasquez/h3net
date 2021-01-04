using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using H3Lib.Extensions;

namespace H3Lib
{
    /// <summary>
    /// H3Index utility functions
    /// </summary>
    [DebuggerDisplay("{H3Value}")]
    public class H3Index
    {
        public int Resolution
        {
            get => (int) (H3Value & H3_RES_MASK) >> H3_RES_OFFSET;
            set => H3Value = (H3Value & H3_RES_MASK_NEGATIVE) |
                             ((ulong)value << H3_RES_OFFSET);
        }

        public int BaseCell
        {
            get => (int)((H3Value & H3_BC_MASK) >> H3_BC_OFFSET);
            set => H3Value = (H3Value & H3_BC_MASK_NEGATIVE) |
                             ((ulong)value << H3_BC_OFFSET);
        }

        public Direction LeadingNonZeroDigit
        {
            get
            {
                for (var r = 1; r < Resolution; r++)
                {
                    if (H3_GET_INDEX_DIGIT(H3Value, r) > 0)
                    {
                        return H3_GET_INDEX_DIGIT(H3Value, r);
                    }
                }

                return Direction.CENTER_DIGIT;
            }
        }

        public H3Mode Mode
        {
            get => (H3Mode) ((H3Value & H3_MODE_MASK) >> H3_MODE_OFFSET);
            set => H3Value = H3Value & H3_MODE_MASK_NEGATIVE |
                             ((ulong)value << H3_MODE_OFFSET);
        }

        public int HighBit
        {
            get => (int) ((H3Value & H3_HIGH_BIT_MASK) >> H3_MAX_OFFSET);
            set => H3Value = (H3Value & H3_HIGH_BIT_MASK_NEGATIVE) |
                             ((ulong) value << H3_MAX_OFFSET);
        }

        public int ReservedBits
        {
            get => (int) ((H3Value & H3_RESERVED_MASK) >> H3_RESERVED_OFFSET);
            set => H3Value = (H3Value & H3_RESERVED_MASK_NEGATIVE) | ((ulong) value << H3_RESERVED_OFFSET);
        }
        /// <summary>
        /// Sets a value in the reserved space. Setting to non-zero may produce
        /// invalid indexes.
        /// </summary>
        public static void H3_SET_RESERVED_BITS(ref H3Index h3, ulong v)
        {
            h3 = (h3 & H3_RESERVED_MASK_NEGATIVE) | (v << H3_RESERVED_OFFSET);
        }

        /// <summary>
        /// Gets a value in the reserved space. Should always be zero for valid indexes.
        /// </summary>
        public static int H3_GET_RESERVED_BITS(H3Index h3)
        {
            return (int) ((h3 & H3_RESERVED_MASK) >> H3_RESERVED_OFFSET);
        }

#region
       // public H3Mode Mode { get; set; }

        public bool IsResClassIii(int resolution)
        {
            return resolution % 2 == 1;
        }

        /// <summary>
        /// The number of bits in an H3 index.
        /// </summary>
        public static int H3_NUM_BITS = 64;
        /// <summary>
        /// The bit offset of the max resolution digit in an H3 index.
        /// </summary>
        public static int H3_MAX_OFFSET = 63;
        /// <summary>
        /// The bit offset of the mode in an H3 index.
        /// </summary>
        public static int H3_MODE_OFFSET = 59;
        /// <summary>
        /// The bit offset of the base cell in an H3 index.
        /// </summary>
        public static int H3_BC_OFFSET = 45;
        /// <summary>
        /// The bit offset of the resolution in an H3 index.
        /// </summary>
        public static int H3_RES_OFFSET = 52;
        /// <summary>
        /// The bit offset of the reserved bits in an H3 index.
        /// </summary>
        public static int H3_RESERVED_OFFSET = 56;
        /// <summary>
        /// The number of bits in a single H3 resolution digit.
        /// </summary>
        public static int H3_PER_DIGIT_OFFSET = 3;
        /// <summary>
        /// 1 in the highest bit, 0's everywhere else.
        /// </summary>
        public static ulong H3_HIGH_BIT_MASK = (ulong) 1 << H3_MAX_OFFSET;
        /// <summary>
        /// 0 in the highest bit, 1's everywhere else.
        /// </summary>
        public static ulong H3_HIGH_BIT_MASK_NEGATIVE = ~H3_HIGH_BIT_MASK;
        /// <summary>
        /// 1's in the 4 mode bits, 0's everywhere else.
        /// </summary>
        public static ulong H3_MODE_MASK = (ulong)15 << H3_MODE_OFFSET;
        /// <summary>
        /// 0's in the 4 mode bits, 1's everywhere else.
        /// </summary>
        public static ulong H3_MODE_MASK_NEGATIVE = ~H3_MODE_MASK;
        /// <summary>
        /// 1's in the 7 base cell bits, 0's everywhere else.
        /// </summary>
        public static ulong H3_BC_MASK = (ulong) 127 << H3_BC_OFFSET;
        /// <summary>
        /// 0's in the 7 base cell bits, 1's everywhere else.
        /// </summary>
        public static ulong H3_BC_MASK_NEGATIVE = ~H3_BC_MASK;
        /// <summary>
        /// 1's in the 4 resolution bits, 0's everywhere else.
        /// </summary>
        public static ulong H3_RES_MASK = (ulong) 15 << H3_RES_OFFSET;
        /// <summary>
        /// 0's in the 4 resolution bits, 1's everywhere else.
        /// </summary>
        public static ulong H3_RES_MASK_NEGATIVE = ~H3_RES_MASK;
        /// <summary>
        /// 1's in the 3 reserved bits, 0's everywhere else.
        /// </summary>
        public static ulong H3_RESERVED_MASK = (ulong) 7 << H3_RESERVED_OFFSET;

        /// <summary>
        /// 0's in the 3 reserved bits, 1's everywhere else.
        /// </summary>
        public static ulong H3_RESERVED_MASK_NEGATIVE = ~H3_RESERVED_MASK;
        /// <summary>
        /// 1's in the 3 bits of res 15 digit bits, 0's everywhere else.
        /// </summary>
        public static ulong H3_DIGIT_MASK = 7;
        /// <summary>
        /// 0's in the 7 base cell bits, 1's everywhere else.
        /// </summary>
        public static ulong H3_DIGIT_MASK_NEGATIVE = ~H3_DIGIT_MASK;
        /// <summary>
        /// H3 index with mode 0, res 0, base cell 0, and 7 for all index digits.
        /// </summary>
        public static ulong H3_INIT = 35184372088831;
        /// <summary>
        /// Invalid index used to indicate an error from geoToH3 and related functions.
        /// </summary>
        public static ulong H3_INVALID_INDEX = 0;
        /// <summary>
        /// Where the actual index is stored.
        /// </summary>
        public ulong H3Value;

        /// <summary>
        /// Invalid index used to indicate an error from geoToH3 and related functions
        /// or missing data in arrays of h3 indices. Analogous to NaN in floating point.
        /// </summary>
        public static ulong H3_NULL = 0;
        
        

        public H3Index(ulong val) 
        {
            H3Value = val;
        }

        public H3Index()
        {
            H3Value = 0;
        }

        protected bool Equals(H3Index other)
        {
            return H3Value == other.H3Value;
        }
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != this.GetType())
            {
                return false;
            }
            return Equals((H3Index) obj);
        }

        public override int GetHashCode()
        {
            return H3Value.GetHashCode();
        }

        public static bool operator ==(H3Index h1, int i2)
        {
            if (ReferenceEquals(h1, null))
            {
                return false;
            }
            return h1.H3Value == (ulong)i2;
        }

        public static bool operator !=(H3Index h1, int i2)
        {
            return !(h1==i2);
        }

        public static bool operator ==(int i1, H3Index h2)
        {
            if (ReferenceEquals(h2, null))
            {
                return false;
            }
            return h2.H3Value == (ulong)i1;
        }

        public static bool operator !=(int i1, H3Index h2)
        {
            return !(i1==h2);
        }


        public static bool operator ==(H3Index h1, H3Index h2)
        {
            if (ReferenceEquals(h1, null) || ReferenceEquals(h2,null))
            {
                return false;
            }

            return h1.H3Value == h2.H3Value;
        }

        public static bool operator !=(H3Index h1, H3Index h2)
        {
            return !(h1 == h2);
        }

        public static bool operator ==(H3Index h1, ulong u2)
        {
            if (ReferenceEquals(h1, null))
            {
                return false;
            }

            return h1.H3Value == u2;
        }

        public static bool operator !=(H3Index  u1, ulong u2)
        {
            return !(u1 == u2);
        }

        public static bool operator ==(ulong u1, H3Index h2)
        {
            if (ReferenceEquals(h2, null))
            {
                return false;
            }

            return h2.H3Value == u1;
        }

        public static bool operator !=(ulong  u1, H3Index h2)
        {
            return !(u1 == h2);
        }

        public static implicit operator H3Index(ulong u)
        {
            H3Index h3 = new H3Index(u);
            return h3;
        }

        public static implicit operator H3Index(ushort u)
        {
            ulong u2 = u;
            H3Index h3 = new H3Index(u2);
            return h3;
        }

        public static implicit operator H3Index(uint u)
        {
            ulong u2 = u;
            H3Index h3 = new H3Index(u2);
            return h3;
        }

        public static implicit operator H3Index(int i)
        {
            if (i < 0)
            {
                throw new ArgumentOutOfRangeException();
            }

            ulong u2 = (ulong) i;
            H3Index h3 = new H3Index(u2);
            return h3;
        }

        public static implicit operator ulong(H3Index h3)
        {
            return h3.H3Value;
        }

        /// <summary>
        /// Gets the integer mode of h3.
        /// </summary>
        public static int H3_GET_MODE(ref H3Index h3)
        {
            return (int) ((h3 & H3_MODE_MASK) >> H3_MODE_OFFSET);
        } 

        /// <summary>
        /// Sets the integer mode of h3 to v.
        /// </summary>
        public static void H3_SET_MODE(ref H3Index h3, ulong v)
        {
            h3 = h3 & H3_MODE_MASK_NEGATIVE | (v << H3_MODE_OFFSET);
        }

        /// <summary>
        /// Gets the integer base cell of h3.
        /// </summary>
        public static int H3_GET_BASE_CELL(H3Index h3)
        {
            return (int)((h3 & H3_BC_MASK) >> H3_BC_OFFSET);
        }

        /// <summary>
        /// Sets the integer base cell of h3 to bc.
        /// </summary>
        public static void H3_SET_BASE_CELL(ref H3Index h3, int bc)
        {
            h3 = (h3 & H3_BC_MASK_NEGATIVE) | ((ulong)bc << H3_BC_OFFSET);
        }

        /// <summary>
        /// Gets the integer resolution of h3.
        /// </summary>
        public static int H3_GET_RESOLUTION(H3Index h3)
        {
            return (int) ((h3 & H3_RES_MASK) >> H3_RES_OFFSET);
        }

        /// <summary>
        /// Sets the integer resolution of h3.
        /// </summary>
        public static void H3_SET_RESOLUTION(ref H3Index h3, H3Index res)
        {
            h3 = (h3 & H3_RES_MASK_NEGATIVE) | (res << H3_RES_OFFSET);
        }
#endregion

#region        
#endregion
        /// <summary>
        ///     Gets the resolution res integer digit (0-7) of h3.
        /// </summary>
        public static Direction H3_GET_INDEX_DIGIT(H3Index h3, int res)
        {
            return (Direction) ((h3 >> ((Constants.MAX_H3_RES - res) * H3_PER_DIGIT_OFFSET)) & H3_DIGIT_MASK);
        }


        /// <summary>
        /// Sets the resolution res digit of h3 to the integer digit (0-7)
        /// </summary>
        public static void H3_SET_INDEX_DIGIT(ref H3Index h3, int res, ulong digit)
        {
            h3 = (h3 & ~(H3_DIGIT_MASK << ((Constants.MAX_H3_RES - res) * H3_PER_DIGIT_OFFSET)))
                 |  ((ulong)digit << ((Constants.MAX_H3_RES - res) * H3_PER_DIGIT_OFFSET));
        }

        /// <summary>
        ///     Gets the resolution res integer digit (0-7) of h3.
        /// </summary>
        public Direction GetIndexDigit(int res)
        {
            return (Direction) ((H3Value >> ((Constants.MAX_H3_RES - res) * H3_PER_DIGIT_OFFSET)) & H3_DIGIT_MASK);
        }

        /// <summary>
        /// Sets the resolution res digit of h3 to the integer digit (0-7)
        /// </summary>
        public void SetIndexDigit(int res, ulong digit)
        {
            H3Value = (H3Value & ~(H3_DIGIT_MASK << ((Constants.MAX_H3_RES - res) * H3_PER_DIGIT_OFFSET)))
              |  (digit << ((Constants.MAX_H3_RES - res) * H3_PER_DIGIT_OFFSET));
            
        }

        /// <summary>
        /// Returns the H3 resolution of an H3 index.
        /// </summary>
        /// <param name="h">The H3 index</param>
        /// <returns>The resolution of the H3 index argument</returns>
        public static int h3GetResolution(H3Index h)
        {
            return H3_GET_RESOLUTION(h);
        }

        /// <summary>
        /// Returns the H3 base cell number of an H3 index.
        /// </summary>
        /// <param name="h"> The H3 index.</param>
         /// <returns>The base cell of the H3 index argument.</returns>
        public static int h3GetBaseCell(H3Index h) { return H3_GET_BASE_CELL(h); }

        /// <summary>
        /// Converts an H3 index into a string representation.
        /// 16 characters long, 0 padded
        /// </summary>
        public override string ToString()
        {
            var str = H3Value.ToString("X");
            int len = str.Length;
            return  str.PadLeft(16 - len, '0');
        }

        /// <summary>
        /// h3ToParent produces the parent index for a given H3 index
        /// </summary>
        /// <param name="h">H3Index to find parent of</param> 
        /// <param name="parentRes"> The resolution to switch to (parent, grandparent, etc)</param> 
        /// <returns>H3Index of the parent, or 0 if you actually asked for a child</returns>
        public static H3Index h3ToParent(H3Index h, int parentRes)
        {
            int childRes = H3_GET_RESOLUTION(h);
            if (parentRes > childRes)
            {
                return H3_INVALID_INDEX;
            }

            if (parentRes == childRes)
            {
                return h;
            }

            if (parentRes < 0 || parentRes > Constants.MAX_H3_RES)
            {
                return H3_INVALID_INDEX;
            }

            H3Index htemp = h;
            H3_SET_RESOLUTION(ref htemp, parentRes);
            H3Index parentH = htemp;

            for (int i = parentRes + 1; i <= childRes; i++) {
                H3_SET_INDEX_DIGIT(ref parentH, i, H3_DIGIT_MASK);
            }
            return parentH;
        }

        /// <summary>
        /// makeDirectChild takes an index and immediately returns the immediate child
        /// index based on the specified cell number. Bit operations only, could generate
        /// invalid indexes if not careful (deleted cell under a pentagon).
        /// </summary>
        /// <param name="h"> H3Index to find the direct child of</param>
        /// <param name="cellNumber"> int id of the direct child (0-6)</param>
        /// <returns>The new H3Index for the child</returns>
        public static H3Index makeDirectChild(H3Index h, int cellNumber)
        {
            int childRes = H3_GET_RESOLUTION(h) + 1;
            H3_SET_RESOLUTION(ref h, childRes);
            H3Index childH = h;
            H3_SET_INDEX_DIGIT(ref childH, childRes, (ulong) cellNumber);
            return childH;
        }

        /// <summary>
        /// h3ToChildren takes the given hexagon id and generates all of the children
        /// at the specified resolution storing them into the provided memory pointer.
        /// It's assumed that maxH3ToChildrenSize was used to determine the allocation.
        /// </summary>
        /// <param name="h"> H3Index to find the children of</param>
        /// <param name="childRes"> int the child level to produce</param>
        /// <param name="children"> H3Index* the memory to store the resulting addresses in</param>
        public static void h3ToChildren(H3Index h, int childRes,ref  List<H3Index> children)
        {
            children = new List<H3Index>();
            int parentRes = H3_GET_RESOLUTION(h);
            if (parentRes > childRes)
            {
                return;
            }

            if (parentRes == childRes)
            {
                children.Add(h);
                return;
            }

            List<H3Index> current = new List<H3Index> {h};
            List<H3Index> realChildren = new List<H3Index>();
            int goalRes = childRes;
            int currentRes = parentRes;

            while (currentRes < goalRes)
            {
                realChildren.Clear();
                foreach (var index in current)
                {
                    int isPentagon = h3IsPentagon(index);
                    for (int m = 0; m < 7; m++)
                    {
                        if (isPentagon > 0 && m == (int) Direction.K_AXES_DIGIT)
                        {
                            realChildren.Add(H3_INVALID_INDEX);
                        }
                        else
                        {
                            var child = makeDirectChild(index, m);
                            realChildren.Add(child);
                        }
                    }
                }
                current = new List<H3Index>(realChildren.Where(c=>c.H3Value!=0));
                currentRes++;
            }

            children = new List<H3Index>(current);
        }

        /// <summary>
        /// compact takes a set of hexagons all at the same resolution and compresses
        /// them by pruning full child branches to the parent level. This is also done
        /// for all parents recursively to get the minimum number of hex addresses that
        /// perfectly cover the defined space.
        /// </summary>
        /// <param name="h3Set"> Set of hexagons</param>
        /// <param name="compactedSet"> The output array of compressed hexagons (pre-allocated)</param>
        /// <param name="numHexes"> The size of the input and output arrays (possible that no
        /// contiguous regions exist in the set at all and no compression possible)</param>
        /// <returns>an error code on bad input data</returns>
        /// <remarks>
        /// We're going to modify this a little bit using some LINQ.
        /// </remarks>
        public static int compact(ref List<H3Index> h3Set, ref List<H3Index> compactedSet, int numHexes)
        {
            //  Maximum resolution.  We're out.
            int res = H3_GET_RESOLUTION(h3Set[0]);
            if (res == 0)
            {
                // No compaction possible, just copy the set to output
                compactedSet = new List<H3Index>(h3Set);
                return 0;
            }

            var realCompacted = new List<ulong>();

            //  Create a scratch list
            List<H3Index> scratchList = new List<H3Index>(numHexes);
            Dictionary<ulong, List<ulong>> generation = new Dictionary<ulong, List<ulong>>();
            //  These are ones we haven't processed.
            List<H3Index> remainingHexes = new List<H3Index>(h3Set.Take(numHexes));

            //  Loop through until we've removed the stragglers at each resolution
            //  and eventually have gotten the biggest sized hexes possible stored away
            while (remainingHexes.Count > 0)
            {
                //  What resolution are we looking at, and what resolution is our parent?
                res = H3_GET_RESOLUTION(remainingHexes[0]);
                int parentRes = res - 1;
                //  Start processing our unprocessed and storing the parent and children in generation
                foreach (var hex in remainingHexes)
                {
                    H3Index parent = h3ToParent(hex, parentRes);
                    if (!generation.ContainsKey(parent.H3Value))
                    {
                        generation[parent.H3Value] = new List<ulong>();
                    }

                    if (generation[parent].Contains(hex.H3Value))
                    {
                        return -1;  //  We have duplicate hexes that we're trying to compact
                    }
                    generation[parent].Add(hex.H3Value);
                }
                // Only possible on duplicate input
                var errorTest = generation.Where(hex => hex.Value.Count > 7);
                if (errorTest.Any())
                {
                    return -2;
                }

                remainingHexes.Clear();
                var pentagons = generation.Where(parent => H3Index.h3IsPentagon(parent.Key) == 1);
                var pentaParents = pentagons.Select(keyValuePair => keyValuePair.Key)
                                            .Select(dummy => (H3Index) dummy).ToList();
                foreach (var pentaParent in pentaParents)
                {
                    if (generation[pentaParent].Count == 6)
                    {
                        remainingHexes.Add(pentaParent);
                    }
                    else
                    {
                        realCompacted.AddRange(generation[pentaParent]);
                    }
                    generation.Remove(pentaParent);
                }

                var orphans = generation.Where(hex => hex.Key!= 0 && hex.Value.Count >0 && hex.Value.Count < 7);
                realCompacted.AddRange(orphans.SelectMany(valuePair => valuePair.Value));

                var nextgen = generation.Where(hex => hex.Value.Count == 7);
                remainingHexes.AddRange(nextgen.Select(valuePair => new H3Index(valuePair.Key)).ToList());
                generation.Clear();
            }

            compactedSet.Capacity = numHexes;
            compactedSet = realCompacted.Select(number => new H3Index(number)).ToList();
            return 0;
        }

        /// <summary>
        /// uncompact takes a compressed set of hexagons and expands back to the
        /// original set of hexagons.
        /// </summary>
        /// <param name="compactedSet"> Set of hexagons</param>
        /// <param name="numHexes"> The number of hexes in the input set</param>
        /// <param name="h3Set Output"> array of decompressed hexagons (preallocated)</param>
        /// <param name="maxHexes"> The size of the output array to bound check against</param>
        /// <param name="res"> The hexagon resolution to decompress to</param>
        /// <returns>
        /// An error code if output array is too small or any hexagon is
        /// smaller than the output resolution.
        /// </returns>
        public static int uncompact(ref List<H3Index> compactedSet, int numHexes,
            ref List<H3Index> h3Set, int maxHexes, int res)
        {
            //  Let's deal with the resolution issue first.
            if (compactedSet.Any(h3 => H3_GET_RESOLUTION(h3) > res))
            {
                return -2;
            }

            foreach (var index in compactedSet)
            {
                int cellres = H3_GET_RESOLUTION(index);
                if (cellres == res)
                {
                    h3Set.Add(index);// Current cell doesn't need decompression.
                    continue;
                }

                List<H3Index> allDescendants = new List<H3Index>(maxH3ToChildrenSize(index, res));
                h3ToChildren(index, res, ref allDescendants);
                h3Set.AddRange(allDescendants);
            }
            return 0;
        }


        /// <summary>
        /// maxUncompactSize takes a compacted set of hexagons are provides an
        /// upper-bound estimate of the size of the uncompacted set of hexagons.
        /// </summary>
        /// <param name="compactedSet"> Set of hexagons</param>
        /// <param name="numHexes"> The number of hexes in the input set</param>
        /// <param name="res"> The hexagon resolution to decompress to</param>
        /// <returns>
        /// The number of hexagons to allocate memory for, or a negative
        /// number if an error occurs.
        /// </returns>
        public static int maxUncompactSize(ref List<H3Index> compactedSet, int numHexes, int res)
        {
            int maxNumHexagons = 0;
            for (int i = 0; i < numHexes; i++)
            {
                if (compactedSet[i] == 0)
                {
                    continue;
                }

                int currentRes = H3_GET_RESOLUTION(compactedSet[i]);
                if (currentRes > res)
                {
                    // Nonsensical. Abort.
                    return -1;
                }

                if (currentRes == res)
                {
                    maxNumHexagons++;
                }
                else
                {
                    // Bigger hexagon to reduce in size
                    int numHexesToGen = maxH3ToChildrenSize(compactedSet[i], res);
                    maxNumHexagons += numHexesToGen;
                }
            }

            return maxNumHexagons;
        }

        /// <summary>
        /// h3IsResClassIII takes a hexagon ID and determines if it is in a
        /// Class III resolution (rotated versus the icosahedron and subject
        /// to shape distortion adding extra points on icosahedron edges, making
        /// them not true hexagons).
        /// </summary>
        /// <param name="h"> The H3Index to check.</param>
        /// <returns>Returns 1 if the hexagon is class III, otherwise 0.</returns>
        public static int h3IsResClassIII(H3Index h) { return H3_GET_RESOLUTION(h) % 2; }

        /// <summary>
        /// h3IsPentagon takes an H3Index and determines if it is actually a
        /// pentagon.
        /// </summary>
        /// <param name="h"> The H3Index to check.</param>
        /// <returns>Returns 1 if it is a pentagon, otherwise 0.</returns>
        public static int h3IsPentagon(H3Index h)
        {
            var test = 
                BaseCells._isBaseCellPentagon(H3_GET_BASE_CELL(h)) &&
                _h3LeadingNonZeroDigit(h) == 0;
            return test ? 1 : 0;
        }

        /// <summary>
        /// Returns the highest resolution non-zero digit in an H3Index.
        /// </summary>
        /// <param name="h"> The H3Index.</param>
        /// <returns>The highest resolution non-zero digit in the H3Index.</returns>
        public static Direction _h3LeadingNonZeroDigit(ulong h)
        {
            for (int r = 1; r <= H3_GET_RESOLUTION(h); r++)
            {
                if (H3_GET_INDEX_DIGIT(h, r) > 0)
                {
                    return H3_GET_INDEX_DIGIT(h, r);
                }

            }

            // if we're here it's all 0's
            return Direction.CENTER_DIGIT;
        }

        /// <summary>
        ///  * Rotate an H3Index 60 degrees counter-clockwise about a pentagonal center.
        /// </summary>
        /// <param name="h">The H3Index.</param>
        public static H3Index _h3RotatePent60ccw(ref H3Index h)
        {
            // rotate in place; skips any leading 1 digits (k-axis)
            int foundFirstNonZeroDigit = 0;
            for (int r = 1, res = H3_GET_RESOLUTION(h); r <= res; r++) {
                // rotate this digit
                H3_SET_INDEX_DIGIT(ref h, r, (ulong) CoordIjk._rotate60ccw(H3_GET_INDEX_DIGIT(h, r)));

                // look for the first non-zero digit so we
                // can adjust for deleted k-axes sequence
                // if necessary
                if (foundFirstNonZeroDigit == 0 && H3_GET_INDEX_DIGIT(h, r) != 0)
                {
                    foundFirstNonZeroDigit = 1;

                    // adjust for deleted k-axes sequence
                    if (_h3LeadingNonZeroDigit(h) == Direction.K_AXES_DIGIT)
                    {
                        h = _h3Rotate60ccw(ref h);
                    }
                }
            }
            return h;
        }

        /// <summary>
        /// Rotate an H3Index 60 degrees clockwise about a pentagonal center.
        /// </summary>
        /// <param name="h"> The H3Index.</param>
        public static H3Index _h3RotatePent60cw(H3Index h)
        {
            // rotate in place; skips any leading 1 digits (k-axis)
            int foundFirstNonZeroDigit = 0;
            for (int r = 1, res = H3_GET_RESOLUTION(h); r <= res; r++) {
                // rotate this digit
                H3_SET_INDEX_DIGIT(ref h, r, (ulong) CoordIjk._rotate60cw(H3_GET_INDEX_DIGIT(h, r)));

                // look for the first non-zero digit so we
                // can adjust for deleted k-axes sequence
                // if necessary
                if ((foundFirstNonZeroDigit==0) && H3_GET_INDEX_DIGIT(h, r) != 0)
                {
                    foundFirstNonZeroDigit = 1;

                    // adjust for deleted k-axes sequence
                    if (_h3LeadingNonZeroDigit(h) == Direction.K_AXES_DIGIT)
                    {
                        h = _h3Rotate60cw(ref h);
                    }
                }
            }
            return h;
        }

        /// <summary>
        /// Rotate an H3Index 60 degrees counter-clockwise.
        /// </summary>
        /// <param name="h">The H3Index.</param> 
        public static H3Index _h3Rotate60ccw(ref H3Index h)
        {
            for (int r = 1, res = H3_GET_RESOLUTION(h); r <= res; r++) {
                Direction oldDigit = H3_GET_INDEX_DIGIT(h, r);
                H3_SET_INDEX_DIGIT(ref h, r, (ulong) CoordIjk._rotate60ccw(oldDigit));
            }

            return h;
        }
        /// <summary>
        /// Rotate an H3Index 60 degrees clockwise.
        /// </summary>
        /// <param name="h">The H3Index.</param> 
        public static H3Index _h3Rotate60cw(ref H3Index h)
        {
            for (int r = 1, res = H3_GET_RESOLUTION(h); r <= res; r++)
            {
                Direction oldDigit = H3_GET_INDEX_DIGIT(h, r);
                H3_SET_INDEX_DIGIT(ref h, r, (ulong) CoordIjk._rotate60cw(oldDigit));
            }
            return h;
        }

        /// <summary>
        /// Convert an FaceIJK address to the corresponding H3Index.
        /// </summary>
        /// <param name="fijk"> The FaceIJK address.</param>
        /// <param name="res">The cell resolution.</param> 
        /// <returns>The encoded H3Index (or 0 on failure).</returns>
        public static H3Index _faceIjkToH3(ref FaceIjk fijk, int res)
        {
            // initialize the index
            H3Index h = H3_INIT;
            H3_SET_MODE(ref h, Constants.H3_HEXAGON_MODE);
            H3_SET_RESOLUTION(ref h, res);

            // check for res 0/base cell
            if (res == 0) {
                if (fijk.Coord.I > BaseCells.MAX_FACE_COORD ||
                    fijk.Coord.J > BaseCells.MAX_FACE_COORD ||
                    fijk.Coord.K > BaseCells.MAX_FACE_COORD)
                {
                    // out of range input
                    return H3_INVALID_INDEX;
                }

                H3_SET_BASE_CELL(ref h, BaseCells._faceIjkToBaseCell(fijk));
                return h;
            }

            // we need to find the correct base cell FaceIJK for this H3 index;
            // start with the passed in face and resolution res ijk coordinates
            // in that face's coordinate system
            //FaceIJK fijkBC = new FaceIJK(fijk.face, fijk.coord);
            FaceIjk fijkBC = new FaceIjk(fijk.Face, new CoordIjk(fijk.Coord.I,fijk.Coord.J,fijk.Coord.K));

            // build the H3Index from finest res up
            // adjust r for the fact that the res 0 base cell offsets the indexing digits
            CoordIjk ijk = fijkBC.Coord;
            for (int r = res - 1; r >= 0; r--) {
                //CoordIJK lastIJK = ijk;
                CoordIjk lastIJK = new CoordIjk(ijk.I,ijk.J,ijk.K);
                CoordIjk lastCenter=new CoordIjk();
                if (isResClassIII(r + 1)) {
                    // rotate ccw
                    CoordIjk._upAp7(ref ijk);
                    lastCenter.I = ijk.I;
                    lastCenter.J = ijk.J;
                    lastCenter.K = ijk.K;
                    CoordIjk._downAp7(ref lastCenter);
                } else {
                    // rotate cw
                    CoordIjk ._upAp7r(ref ijk);
                    lastCenter.I = ijk.I;
                    lastCenter.J = ijk.J;
                    lastCenter.K = ijk.K;
                    CoordIjk._downAp7r(ref lastCenter);
                }

                CoordIjk diff = new CoordIjk();
                CoordIjk._ijkSub(ref lastIJK, ref lastCenter, ref diff);
                CoordIjk._ijkNormalize(ref diff);

                H3_SET_INDEX_DIGIT(ref h, r + 1,
                    (ulong)CoordIjk._unitIjkToDigit(ref diff));
            }

            // fijkBC should now hold the IJK of the base cell in the
            // coordinate system of the current face
            if (fijkBC.Coord.I > BaseCells.MAX_FACE_COORD ||
                fijkBC.Coord.J > BaseCells.MAX_FACE_COORD ||
                fijkBC.Coord.K > BaseCells.MAX_FACE_COORD) {
                // out of range input
                return H3_INVALID_INDEX;
            }

            // lookup the correct base cell
            int baseCell = BaseCells . _faceIjkToBaseCell(fijkBC);
            H3_SET_BASE_CELL(ref h, baseCell);

            // rotate if necessary to get canonical base cell orientation
            // for this base cell
            int numRots = BaseCells._faceIjkToBaseCellCCWrot60(fijkBC);
            if (BaseCells._isBaseCellPentagon(baseCell)) {
                // force rotation out of missing k-axes sub-sequence
                if (_h3LeadingNonZeroDigit(h) == Direction.K_AXES_DIGIT) {
                    // check for a cw/ccw offset face; default is ccw
                    if (BaseCells._baseCellIsCwOffset(baseCell, fijkBC.Face)) {
                        h = _h3Rotate60cw(ref h);
                    } else {
                        h = _h3Rotate60ccw(ref h);
                    }
                }

                for (int i = 0; i < numRots; i++)
                {
                    h = _h3RotatePent60ccw(ref h);
                }
            } else {
                for (int i = 0; i < numRots; i++) {
                    h = _h3Rotate60ccw(ref h);
                }
            }

            return h;
        }

        /// <summary>
        /// Encodes a coordinate on the sphere to the H3 index of the containing cell at
        /// the specified resolution.
        ///
        /// Returns 0 on invalid input.
        /// </summary>
        /// <param name="g">The spherical coordinates to encode.</param>
        /// <param name="res"> The desired H3 resolution for the encoding.</param>
        /// <returns>The encoded H3Index (or 0 on failure).</returns>
        public static H3Index geoToH3(ref GeoCoord g, int res)
        {
            if (res < 0 || res > Constants. MAX_H3_RES)
            {
                return H3_INVALID_INDEX;
            }

            if (Double.IsInfinity(g.Latitude) || Double.IsNaN(g.Latitude) ||
                Double.IsInfinity(g.Longitude) || Double.IsNaN(g.Longitude))
            {
                return H3_INVALID_INDEX;
            }

            FaceIjk fijk = new FaceIjk();
            FaceIjk._geoToFaceIjk(g, res, ref fijk);
            return _faceIjkToH3(ref fijk, res);
        }

        /// <summary>
        /// Convert an H3Index to the FaceIJK address on a specified icosahedral face.
        /// </summary>
        /// <param name="h"> The H3Index.</param>
        /// <param name="fijk">
        /// The FaceIJK address, initialized with the desired face
        /// and normalized base cell coordinates.
        /// </param>
        /// <returns>Returns 1 if the possibility of overage exists, otherwise 0.</returns>
        internal static int _h3ToFaceIjkWithInitializedFijk(H3Index h, ref FaceIjk fijk)
        {
            CoordIjk ijk = new CoordIjk(fijk.Coord.I, fijk.Coord.J, fijk.Coord.K);
            int res = H3_GET_RESOLUTION(h);

            // center base cell hierarchy is entirely on this face
            int possibleOverage = 1;
            if (!BaseCells._isBaseCellPentagon(H3_GET_BASE_CELL(h)) &&
                (res == 0 ||
                 fijk.Coord.I == 0 && fijk.Coord.J == 0 && fijk.Coord.K == 0))
                possibleOverage = 0;

            for (int r = 1; r <= res; r++) {
                if (isResClassIII(r)) {
                    // Class III == rotate ccw
                    CoordIjk._downAp7(ref ijk);
                } else {
                    // Class II == rotate cw
                    CoordIjk._downAp7r(ref ijk);
                }

                CoordIjk._neighbor(ref ijk, H3_GET_INDEX_DIGIT(h, r));
            }

            fijk.Coord.I = ijk.I;
            fijk.Coord.J = ijk.J;
            fijk.Coord.K = ijk.K;
            return possibleOverage;
        }


        /// <summary>
        /// Convert an H3Index to a FaceIJK address.
        /// </summary>
        /// <param name="h"> The H3Index.</param>
        /// <param name="fijk"> The corresponding FaceIJK address.</param>
        public static void _h3ToFaceIjk(H3Index h, ref FaceIjk fijk)
        {
            int baseCell = H3_GET_BASE_CELL(h);
            // adjust for the pentagonal missing sequence; all of sub-sequence 5 needs
            // to be adjusted (and some of sub-sequence 4 below)
            if (BaseCells._isBaseCellPentagon(baseCell) && _h3LeadingNonZeroDigit(h) == Direction.IK_AXES_DIGIT)
            {
                h = _h3Rotate60cw(ref h);
            }

            // start with the "home" face and ijk+ coordinates for the base cell of c
            fijk = new FaceIjk(
                               BaseCells.baseCellData[baseCell].homeFijk.Face,
                               new CoordIjk(
                                            BaseCells.baseCellData[baseCell].homeFijk.Coord.I,
                                            BaseCells.baseCellData[baseCell].homeFijk.Coord.J,
                                            BaseCells.baseCellData[baseCell].homeFijk.Coord.K
                                           )
                              );
            //fijk = BaseCells.baseCellData[baseCell].homeFijk;
            if (_h3ToFaceIjkWithInitializedFijk(h, ref fijk) == 0)
            {
                return; // no overage is possible; h lies on this face
            } 

            // if we're here we have the potential for an "overage"; i.e., it is
            // possible that c lies on an adjacent face
            CoordIjk origIJK = new CoordIjk(fijk.Coord.I, fijk.Coord.J, fijk.Coord.K);

            // if we're in Class III, drop into the next finer Class II grid
            int res = H3_GET_RESOLUTION(h);
            if (isResClassIII(res)) {
                // Class III
                CoordIjk ._downAp7r( ref fijk.Coord);
                res++;
            }

            // adjust for overage if needed
            // a pentagon base cell with a leading 4 digit requires special handling
            bool pentLeading4 =
                (BaseCells._isBaseCellPentagon(baseCell) && _h3LeadingNonZeroDigit(h) == Direction.I_AXES_DIGIT);
            if (FaceIjk._adjustOverageClassII(ref fijk, res, pentLeading4 ? 1 : 0, 0) > 0)
            {
                // if the base cell is a pentagon we have the potential for secondary
                // overages
                if (BaseCells._isBaseCellPentagon(baseCell))
                {
                    while (true)
                    {
                        if (FaceIjk._adjustOverageClassII(ref fijk, res, 0, 0) == 0)
                        {
                            break;
                        }
                    }
                }

                if (res != H3_GET_RESOLUTION(h))
                {
                    CoordIjk._upAp7r(ref fijk.Coord);
                }
            }
            else if (res != H3_GET_RESOLUTION(h))
            {
                fijk.Coord = new CoordIjk(origIJK.I, origIJK.J, origIJK.K);
            }
        }

        /// <summary>
        /// Determines the spherical coordinates of the center point of an H3 index.
        /// </summary>
        /// <param name="h3"> The H3 index.</param>
        /// <param name="g"> The spherical coordinates of the H3 cell center.</param>
        public static void h3ToGeo(H3Index h3, ref GeoCoord g)
        {
            FaceIjk fijk = new FaceIjk();
            _h3ToFaceIjk(h3, ref fijk);
            FaceIjk. _faceIjkToGeo(fijk, H3_GET_RESOLUTION(h3), ref g);
        }

        /// <summary>
        /// Determines the cell boundary in spherical coordinates for an H3 index.
        /// </summary>
        /// <param name="h3"> The H3 index.</param>
        /// <param name="gb">The boundary of the H3 cell in spherical coordinates.</param>
        public static void h3ToGeoBoundary(H3Index h3, ref GeoBoundary gb)
        {
            FaceIjk fijk = new FaceIjk();
            _h3ToFaceIjk(h3, ref fijk);
            if (h3IsPentagon(h3) == 1)
            {
                FaceIjk._faceIjkPentToGeoBoundary(fijk, H3_GET_RESOLUTION(h3), 0, Constants.NUM_PENT_VERTS, ref gb);
            }
            else
            {
                FaceIjk._faceIjkToGeoBoundary(fijk, H3_GET_RESOLUTION(h3), 0, Constants.NUM_HEX_VERTS, ref gb);
            }
        }
    }
}
