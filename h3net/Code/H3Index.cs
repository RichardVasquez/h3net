/*
 * Copyright 2018, Richard Vasquez
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *         http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 *
 * Original version written in C, Copyright 2016-2017 Uber Technologies, Inc.
 * C version licensed under the Apache License, Version 2.0 (the "License");
 * C Source code available at: https://github.com/uber/h3
 *
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;

namespace H3Net.Code
{
    /// <summary>
    /// H3Index utility functions
    /// </summary>
    /// <!-- Based off 3.1.1 -->
    [DebuggerDisplay("{value}")]
    public class H3Index
    {
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
        public ulong value;

        public H3Index(ulong val) 
        {
            value = val;
        }

        public H3Index()
        {
            value = 0;
        }

        protected bool Equals(H3Index other)
        {
            return value == other.value;
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
            return value.GetHashCode();
        }

        public static bool operator ==(H3Index h1, int i2)
        {
            if (ReferenceEquals(h1, null))
            {
                return false;
            }
            return h1.value == (ulong)i2;
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
            return h2.value == (ulong)i1;
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

            return h1.value == h2.value;
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

            return h1.value == u2;
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

            return h2.value == u1;
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
            return h3.value;
        }

        /// <summary>
        /// Gets the integer mode of h3.
        /// </summary>
        /// <!-- Based off 3.1.1 -->
        public static int H3_GET_MODE(ref H3Index h3)
        {
            return (int) ((h3 & H3_MODE_MASK) >> H3_MODE_OFFSET);
        } 

        /// <summary>
        /// Sets the integer mode of h3 to v.
        /// </summary>
        /// <!-- Based off 3.1.1 -->
        public static void H3_SET_MODE(ref H3Index h3, ulong v)
        {
            h3 = h3 & H3_MODE_MASK_NEGATIVE | (v << H3_MODE_OFFSET);
        }

        /// <summary>
        /// Gets the integer base cell of h3.
        /// </summary>
        /// <!-- Based off 3.1.1 -->
        public static int H3_GET_BASE_CELL(H3Index h3)
        {
            return (int)((h3 & H3_BC_MASK) >> H3_BC_OFFSET);
        }

        /// <summary>
        /// Sets the integer base cell of h3 to bc.
        /// </summary>
        /// <!-- Based off 3.1.1 -->
        public static void H3_SET_BASE_CELL(ref H3Index h3, int bc)
        {
            h3 = (h3 & H3_BC_MASK_NEGATIVE) | ((ulong)bc << H3_BC_OFFSET);
        }

        /// <summary>
        /// Gets the integer resolution of h3.
        /// </summary>
        /// <!-- Based off 3.1.1 -->
        public static int H3_GET_RESOLUTION(H3Index h3)
        {
            return (int) ((h3 & H3_RES_MASK) >> H3_RES_OFFSET);
        }

        /// <summary>
        /// Sets the integer resolution of h3.
        /// </summary>
        /// <!-- Based off 3.1.1 -->
        public static void H3_SET_RESOLUTION(ref H3Index h3, H3Index res)
        {
            h3 = (h3 & H3_RES_MASK_NEGATIVE) | (res << H3_RES_OFFSET);
        }

        /// <summary>
        ///     Gets the resolution res integer digit (0-7) of h3.
        /// </summary>
        /// <!-- Based off 3.1.1 -->
        public static Direction H3_GET_INDEX_DIGIT(H3Index h3, int res)
        {
            return (Direction) ((h3 >> ((Constants.MAX_H3_RES - res) * H3_PER_DIGIT_OFFSET)) & H3_DIGIT_MASK);
        }

        /// <summary>
        /// Sets a value in the reserved space. Setting to non-zero may produce
        /// invalid indexes.
        /// </summary>
        /// <!-- Based off 3.1.1 -->
        public static void H3_SET_RESERVED_BITS(ref H3Index h3, ulong v)
        {
            h3 = (h3 & H3_RESERVED_MASK_NEGATIVE) | (v << H3_RESERVED_OFFSET);
        }

        /// <summary>
        /// Gets a value in the reserved space. Should always be zero for valid indexes.
        /// </summary>
        /// <!-- Based off 3.1.1 -->
        public static int H3_GET_RESERVED_BITS(H3Index h3)
        {
            return (int) ((h3 & H3_RESERVED_MASK) >> H3_RESERVED_OFFSET);
        }

        /// <summary>
        /// Sets the resolution res digit of h3 to the integer digit (0-7)
        /// </summary>
        /// <!-- Based off 3.1.1 -->
        public static void H3_SET_INDEX_DIGIT(ref H3Index h3, int res, ulong digit)
        {
            h3 = (h3 & ~(H3_DIGIT_MASK << ((Constants.MAX_H3_RES - res) * H3_PER_DIGIT_OFFSET)))
                 |  ((ulong)digit << ((Constants.MAX_H3_RES - res) * H3_PER_DIGIT_OFFSET));
        }
        

        /// <summary>
        /// Returns the H3 resolution of an H3 index.
        /// </summary>
        /// <param name="h">The H3 index</param>
        /// <returns>The resolution of the H3 index argument</returns>
        /// <!-- Based off 3.1.1 -->
        public static int h3GetResolution(H3Index h)
        {
            return H3_GET_RESOLUTION(h);
        }

        /// <summary>
        /// Returns the H3 base cell number of an H3 index.
        /// </summary>
        /// <param name="h"> The H3 index.</param>
         /// <returns>The base cell of the H3 index argument.</returns>
        /// <!-- Based off 3.1.1 -->
        public static int h3GetBaseCell(H3Index h) { return H3_GET_BASE_CELL(h); }

        /// <summary>
        /// Converts a string representation of an H3 index into an H3 index.
        /// </summary>
        /// <param name="str"> The string representation of an H3 index.</param>
        /// <returns>
        /// The H3 index corresponding to the string argument, or 0 if invalid.
        /// </returns>
        /// <!-- Based off 3.1.1 -->
        public static H3Index stringToH3(string str) {
            H3Index h = H3_INVALID_INDEX;
            // A small risk, but for the most part, we're dealing with hex numbers, so let's use that
            // as our default.
            if (ulong.TryParse(str, NumberStyles.HexNumber, CultureInfo.CurrentCulture, out ulong ul1))
            {
                return new H3Index(ul1);
            }
            // If failed, h will be unmodified and we should return 0 anyways.
            if (ulong.TryParse(str, out ulong ul2))
            {
                return new H3Index(ul2);
            }

            return 0;
        }

        /// <summary>
        /// Converts an H3 index into a string representation.
        /// </summary>
        /// <param name="h"> The H3 index to convert.</param>
        /// <param name="str"> The string representation of the H3 index.</param>
        /// <param name="sz"> Size of the buffer <see cref="str"/></param>
        /// <!-- Based off 3.1.1 -->
        public static void h3ToString(H3Index h, ref string str, int sz)
        {
            if (sz < 17)
            {
                return;
            }
            //  We don't care about sz.  We have System.String
            str = ((ulong) h).ToString("X").ToLower();
        }


        /// <summary>
        /// Returns whether or not an H3 index is valid.
        /// </summary>
        /// <param name="h">The H3 index to validate.</param> 
         /// <returns>1 if the H3 index if valid, and 0 if it is not.</returns>
        /// <!-- Based off 3.1.1 -->
        public static int h3IsValid(H3Index h)
        {
            if (H3_GET_MODE(ref h) != Constants.H3_HEXAGON_MODE)
            {
                return 0;
            }

            int baseCell = H3_GET_BASE_CELL(h);
            if (baseCell < 0 || baseCell >= Constants.NUM_BASE_CELLS)
            {
                return 0;
            }

            int res = H3_GET_RESOLUTION(h);
            bool foundFirstNonZeroDigit = false;
            if (res < 0 || res > Constants.MAX_H3_RES)
            {
                return 0;
            }

            for (int r = 1; r <= res; r++)
            {
                Direction digit = H3_GET_INDEX_DIGIT(h, r);
                if (!foundFirstNonZeroDigit && digit != Direction.CENTER_DIGIT) {
                    foundFirstNonZeroDigit = true;
                    if (BaseCells._isBaseCellPentagon(baseCell) && digit == Direction.K_AXES_DIGIT)
                    {
                        return 0;
                    }
                }

                if (digit < Direction.CENTER_DIGIT || digit >= Direction.NUM_DIGITS)
                {
                    return 0;
                }
            }

            for (int r = res + 1; r <= Constants.MAX_H3_RES; r++)
            {
                Direction digit = H3_GET_INDEX_DIGIT(h, r);
                if (digit != Direction.INVALID_DIGIT)
                {
                    return 0;
                }
            }

            return 1;
        }

        /// <summary>
        /// Initializes an H3 index.
        /// </summary>
        /// <param name="hp"> The H3 index to initialize.</param>
        /// <param name="res"> The H3 resolution to initialize the index to.</param>
        /// <param name="baseCell"> The H3 base cell to initialize the index to.</param>
        /// <param name="initDigit"> The H3 digit (0-7) to initialize all of the index digits to.</param>
        /// <!-- Based off 3.1.1 -->
        public static void setH3Index(ref H3Index hp, int res, int baseCell, Direction initDigit)
        {
            H3Index h = H3_INIT;
            H3_SET_MODE(ref h, Constants.H3_HEXAGON_MODE);
            H3_SET_RESOLUTION(ref h, res);
            H3_SET_BASE_CELL(ref h, baseCell);
            for (int r = 1; r <= res; r++)
            {
                H3_SET_INDEX_DIGIT(ref h, r, (ulong) initDigit);
            }

            hp = h;
        }

        /// <summary>
        /// h3ToParent produces the parent index for a given H3 index
        /// </summary>
        /// <param name="h">H3Index to find parent of</param> 
        /// <param name="parentRes"> The resolution to switch to (parent, grandparent, etc)</param> 
        /// <returns>H3Index of the parent, or 0 if you actually asked for a child</returns>
        /// <!-- Based off 3.1.1 -->
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
        /// maxH3ToChildrenSize returns the maximum number of children possible for a
        /// given child level.
        /// </summary>
        /// <param name="h"> H3Index to find the number of children of</param>
        /// <param name="childRes"> The resolution of the child level you're interested in</param>
        /// <returns>
        /// int count of maximum number of children (equal for hexagons, less for
        /// pentagons
        /// </returns>
        /// <!-- Based off 3.1.1 -->
        public static int maxH3ToChildrenSize(H3Index h, int childRes)
        {
            int parentRes = H3_GET_RESOLUTION(h);
            if (parentRes > childRes) 
            {
                return 0;
            }
            return MathExtensions._ipow(7, childRes - parentRes);
        }

        /// <summary>
        /// makeDirectChild takes an index and immediately returns the immediate child
        /// index based on the specified cell number. Bit operations only, could generate
        /// invalid indexes if not careful (deleted cell under a pentagon).
        /// </summary>
        /// <param name="h"> H3Index to find the direct child of</param>
        /// <param name="cellNumber"> int id of the direct child (0-6)</param>
        /// <returns>The new H3Index for the child
        /// <!-- Based off 3.1.1 -->
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
        /// <param name="h" H3Index to find the children of</param>
        /// <param name="childRes" int the child level to produce</param>
        /// <param name="children" H3Index* the memory to store the resulting addresses in</param>
        /// <!-- Based off 3.1.1 -->
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
                current = new List<H3Index>(realChildren.Where(c=>c.value!=0));
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
        /// <!-- Based off 3.1.1 -->
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
                    if (!generation.ContainsKey(parent.value))
                    {
                        generation[parent.value] = new List<ulong>();
                    }

                    if (generation[parent].Contains(hex.value))
                    {
                        return -1;  //  We have duplicate hexes that we're trying to compact
                    }
                    generation[parent].Add(hex.value);
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
        /// <!-- Based off 3.1.1 -->
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
        /// <!-- Based off 3.1.1 -->
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
        /// <!-- Based off 3.1.1 -->
        public static int h3IsResClassIII(H3Index h) { return H3_GET_RESOLUTION(h) % 2; }

        /// <summary>
        /// h3IsPentagon takes an H3Index and determines if it is actually a
        /// pentagon.
        /// </summary>
        /// <param name="h"> The H3Index to check.</param>
        /// <returns>Returns 1 if it is a pentagon, otherwise 0.</returns>
        /// <!-- Based off 3.1.1 -->
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
        /// <!-- Based off 3.1.1 -->
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
        /// <!-- Based off 3.1.1 -->
        public static H3Index _h3RotatePent60ccw(ref H3Index h)
        {
            // rotate in place; skips any leading 1 digits (k-axis)
            int foundFirstNonZeroDigit = 0;
            for (int r = 1, res = H3_GET_RESOLUTION(h); r <= res; r++) {
                // rotate this digit
                H3_SET_INDEX_DIGIT(ref h, r, (ulong) CoordIJK._rotate60ccw(H3_GET_INDEX_DIGIT(h, r)));

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
        /// <!-- Based off 3.1.1 -->
        public static H3Index _h3RotatePent60cw(H3Index h)
        {
            // rotate in place; skips any leading 1 digits (k-axis)
            int foundFirstNonZeroDigit = 0;
            for (int r = 1, res = H3_GET_RESOLUTION(h); r <= res; r++) {
                // rotate this digit
                H3_SET_INDEX_DIGIT(ref h, r, (ulong) CoordIJK._rotate60cw(H3_GET_INDEX_DIGIT(h, r)));

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
        /// <!-- Based off 3.1.1 -->
        public static H3Index _h3Rotate60ccw(ref H3Index h)
        {
            for (int r = 1, res = H3_GET_RESOLUTION(h); r <= res; r++) {
                Direction oldDigit = H3_GET_INDEX_DIGIT(h, r);
                H3_SET_INDEX_DIGIT(ref h, r, (ulong) CoordIJK._rotate60ccw(oldDigit));
            }

            return h;
        }
        /// <summary>
        /// Rotate an H3Index 60 degrees clockwise.
        /// </summary>
        /// <param name="h">The H3Index.</param> 
        /// <!-- Based off 3.1.1 -->
        public static H3Index _h3Rotate60cw(ref H3Index h)
        {
            for (int r = 1, res = H3_GET_RESOLUTION(h); r <= res; r++)
            {
                Direction oldDigit = H3_GET_INDEX_DIGIT(h, r);
                H3_SET_INDEX_DIGIT(ref h, r, (ulong) CoordIJK._rotate60cw(oldDigit));
            }
            return h;
        }

        /// <summary>
        /// Convert an FaceIJK address to the corresponding H3Index.
        /// </summary>
        /// <param name="fijk"> The FaceIJK address.</param>
        /// <param name="res">The cell resolution.</param> 
        /// <returns>The encoded H3Index (or 0 on failure).</returns>
        /// <!-- Based off 3.1.1 -->
        public static H3Index _faceIjkToH3(ref FaceIJK fijk, int res)
        {
            // initialize the index
            H3Index h = H3_INIT;
            H3_SET_MODE(ref h, Constants.H3_HEXAGON_MODE);
            H3_SET_RESOLUTION(ref h, res);

            // check for res 0/base cell
            if (res == 0) {
                if (fijk.coord.i > BaseCells.MAX_FACE_COORD ||
                    fijk.coord.j > BaseCells.MAX_FACE_COORD ||
                    fijk.coord.k > BaseCells.MAX_FACE_COORD)
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
            FaceIJK fijkBC = new FaceIJK(fijk.face, new CoordIJK(fijk.coord.i,fijk.coord.j,fijk.coord.k));

            // build the H3Index from finest res up
            // adjust r for the fact that the res 0 base cell offsets the indexing digits
            CoordIJK ijk = fijkBC.coord;
            for (int r = res - 1; r >= 0; r--) {
                //CoordIJK lastIJK = ijk;
                CoordIJK lastIJK = new CoordIJK(ijk.i,ijk.j,ijk.k);
                CoordIJK lastCenter=new CoordIJK();
                if (isResClassIII(r + 1)) {
                    // rotate ccw
                    CoordIJK._upAp7(ref ijk);
                    lastCenter.i = ijk.i;
                    lastCenter.j = ijk.j;
                    lastCenter.k = ijk.k;
                    CoordIJK._downAp7(ref lastCenter);
                } else {
                    // rotate cw
                    CoordIJK ._upAp7r(ref ijk);
                    lastCenter.i = ijk.i;
                    lastCenter.j = ijk.j;
                    lastCenter.k = ijk.k;
                    CoordIJK._downAp7r(ref lastCenter);
                }

                CoordIJK diff = new CoordIJK();
                CoordIJK._ijkSub(ref lastIJK, ref lastCenter, ref diff);
                CoordIJK._ijkNormalize(ref diff);

                H3_SET_INDEX_DIGIT(ref h, r + 1,
                    (ulong)CoordIJK._unitIjkToDigit(ref diff));
            }

            // fijkBC should now hold the IJK of the base cell in the
            // coordinate system of the current face
            if (fijkBC.coord.i > BaseCells.MAX_FACE_COORD ||
                fijkBC.coord.j > BaseCells.MAX_FACE_COORD ||
                fijkBC.coord.k > BaseCells.MAX_FACE_COORD) {
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
                    if (BaseCells._baseCellIsCwOffset(baseCell, fijkBC.face)) {
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
        /// <!-- Based off 3.1.1 -->
        public static H3Index geoToH3(ref GeoCoord g, int res)
        {
            if (res < 0 || res > Constants. MAX_H3_RES)
            {
                return H3_INVALID_INDEX;
            }

            if (Double.IsInfinity(g.lat) || Double.IsNaN(g.lat) ||
                Double.IsInfinity(g.lon) || Double.IsNaN(g.lon))
            {
                return H3_INVALID_INDEX;
            }

            FaceIJK fijk = new FaceIJK();
            FaceIJK._geoToFaceIjk(g, res, ref fijk);
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
        /// <!-- Based off 3.1.1 -->
        internal static int _h3ToFaceIjkWithInitializedFijk(H3Index h, ref FaceIJK fijk)
        {
            CoordIJK ijk = new CoordIJK(fijk.coord.i, fijk.coord.j, fijk.coord.k);
            int res = H3_GET_RESOLUTION(h);

            // center base cell hierarchy is entirely on this face
            int possibleOverage = 1;
            if (!BaseCells._isBaseCellPentagon(H3_GET_BASE_CELL(h)) &&
                (res == 0 ||
                 fijk.coord.i == 0 && fijk.coord.j == 0 && fijk.coord.k == 0))
                possibleOverage = 0;

            for (int r = 1; r <= res; r++) {
                if (isResClassIII(r)) {
                    // Class III == rotate ccw
                    CoordIJK._downAp7(ref ijk);
                } else {
                    // Class II == rotate cw
                    CoordIJK._downAp7r(ref ijk);
                }

                CoordIJK._neighbor(ref ijk, H3_GET_INDEX_DIGIT(h, r));
            }

            fijk.coord.i = ijk.i;
            fijk.coord.j = ijk.j;
            fijk.coord.k = ijk.k;
            return possibleOverage;
        }


        /// <summary>
        /// Convert an H3Index to a FaceIJK address.
        /// </summary>
        /// <param name="h"> The H3Index.</param>
        /// <param name="fijk"> The corresponding FaceIJK address.</param>
        /// <!-- Based off 3.1.1 -->
        public static void _h3ToFaceIjk(H3Index h, ref FaceIJK fijk)
        {
            int baseCell = H3_GET_BASE_CELL(h);
            // adjust for the pentagonal missing sequence; all of sub-sequence 5 needs
            // to be adjusted (and some of sub-sequence 4 below)
            if (BaseCells._isBaseCellPentagon(baseCell) && _h3LeadingNonZeroDigit(h) == Direction.IK_AXES_DIGIT)
            {
                h = _h3Rotate60cw(ref h);
            }

            // start with the "home" face and ijk+ coordinates for the base cell of c
            fijk = new FaceIJK(
                               BaseCells.baseCellData[baseCell].homeFijk.face,
                               new CoordIJK(
                                            BaseCells.baseCellData[baseCell].homeFijk.coord.i,
                                            BaseCells.baseCellData[baseCell].homeFijk.coord.j,
                                            BaseCells.baseCellData[baseCell].homeFijk.coord.k
                                           )
                              );
            //fijk = BaseCells.baseCellData[baseCell].homeFijk;
            if (_h3ToFaceIjkWithInitializedFijk(h, ref fijk) == 0)
            {
                return; // no overage is possible; h lies on this face
            } 

            // if we're here we have the potential for an "overage"; i.e., it is
            // possible that c lies on an adjacent face
            CoordIJK origIJK = new CoordIJK(fijk.coord.i, fijk.coord.j, fijk.coord.k);

            // if we're in Class III, drop into the next finer Class II grid
            int res = H3_GET_RESOLUTION(h);
            if (isResClassIII(res)) {
                // Class III
                CoordIJK ._downAp7r( ref fijk.coord);
                res++;
            }

            // adjust for overage if needed
            // a pentagon base cell with a leading 4 digit requires special handling
            bool pentLeading4 =
                (BaseCells._isBaseCellPentagon(baseCell) && _h3LeadingNonZeroDigit(h) == Direction.I_AXES_DIGIT);
            if (FaceIJK._adjustOverageClassII(ref fijk, res, pentLeading4 ? 1 : 0, 0) > 0)
            {
                // if the base cell is a pentagon we have the potential for secondary
                // overages
                if (BaseCells._isBaseCellPentagon(baseCell))
                {
                    while (true)
                    {
                        if (FaceIJK._adjustOverageClassII(ref fijk, res, 0, 0) == 0)
                        {
                            break;
                        }
                    }
                }

                if (res != H3_GET_RESOLUTION(h))
                {
                    CoordIJK._upAp7r(ref fijk.coord);
                }
            }
            else if (res != H3_GET_RESOLUTION(h))
            {
                fijk.coord = new CoordIJK(origIJK.i, origIJK.j, origIJK.k);
            }
        }

        /// <summary>
        /// Determines the spherical coordinates of the center point of an H3 index.
        /// </summary>
        /// <param name="h3"> The H3 index.</param>
        /// <param name="g"> The spherical coordinates of the H3 cell center.</param>
        /// <!-- Based off 3.1.1 -->
        public static void h3ToGeo(H3Index h3, ref GeoCoord g)
        {
            FaceIJK fijk = new FaceIJK();
            _h3ToFaceIjk(h3, ref fijk);
            FaceIJK. _faceIjkToGeo(fijk, H3_GET_RESOLUTION(h3), ref g);
        }

        /// <summary>
        /// Determines the cell boundary in spherical coordinates for an H3 index.
        /// </summary>
        /// <param name="h3"> The H3 index.</param>
        /// <param name="gb">The boundary of the H3 cell in spherical coordinates.</param>
        /// <!-- Based off 3.1.1 -->
        public static void h3ToGeoBoundary(H3Index h3, ref GeoBoundary gb) {
            FaceIJK fijk = new FaceIJK();
            _h3ToFaceIjk(h3, ref fijk);
            FaceIJK._faceIjkToGeoBoundary(
                ref fijk, H3_GET_RESOLUTION(h3),
                h3IsPentagon(h3), ref  gb
            );
        }

        /// <summary>
        /// Returns whether or not a resolution is a Class III grid. Note that odd
        //  resolutions are Class III and even resolutions are Class II.
        /// </summary>
        /// <param name="res">The H3 resolution</param>
        /// <returns>Returns 1 if the resolution is class III grid, otherwise 0.</returns>
        /// <!-- Based off 3.1.1 -->
        public static bool isResClassIII(int res)
        {
            return res % 2 == 1;
        }


    }
}
