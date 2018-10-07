using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;

namespace h3net.API
{
    [DebuggerDisplay("{value}")]
    public class H3Index
    {
        /** The number of bits in an H3 index. */
        public static int H3_NUM_BITS = 64;
        /** The bit offset of the max resolution digit in an H3 index. */
        public static int H3_MAX_OFFSET = 63;
        /** The bit offset of the mode in an H3 index. */
        public static int H3_MODE_OFFSET = 59;
        /** The bit offset of the base cell in an H3 index. */
        public static int H3_BC_OFFSET = 45;
        /** The bit offset of the resolution in an H3 index. */
        public static int H3_RES_OFFSET = 52;
        /** The bit offset of the reserved bits in an H3 index. */
        public static int H3_RESERVED_OFFSET = 56;
        /** The number of bits in a single H3 resolution digit. */
        public static int H3_PER_DIGIT_OFFSET = 3;
        /** 1's in the 4 mode bits, 0's everywhere else. */
        public static ulong H3_MODE_MASK = (ulong)15 << H3_MODE_OFFSET;
        /** 0's in the 4 mode bits, 1's everywhere else. */
        public static ulong H3_MODE_MASK_NEGATIVE = ~H3_MODE_MASK;
        /** 1's in the 7 base cell bits, 0's everywhere else. */
        public static ulong H3_BC_MASK = (ulong) 127 << H3_BC_OFFSET;
        /** 0's in the 7 base cell bits, 1's everywhere else. */
        public static ulong H3_BC_MASK_NEGATIVE = ~H3_BC_MASK;
        /** 1's in the 4 resolution bits, 0's everywhere else. */
        public static ulong H3_RES_MASK = (ulong) 15 << H3_RES_OFFSET;
        /** 0's in the 4 resolution bits, 1's everywhere else. */
        public static ulong H3_RES_MASK_NEGATIVE = ~H3_RES_MASK;
        /** 1's in the 3 reserved bits, 0's everywhere else. */
        public static ulong H3_RESERVED_MASK = (ulong) 7 << H3_RESERVED_OFFSET;

        /** 0's in the 3 reserved bits, 1's everywhere else. */
        public static ulong H3_RESERVED_MASK_NEGATIVE = ~H3_RESERVED_MASK;
        /** 1's in the 3 bits of res 15 digit bits, 0's everywhere else. */
        public static ulong H3_DIGIT_MASK = 7;
        /** 0's in the 7 base cell bits, 1's everywhere else. */
        public static ulong H3_DIGIT_MASK_NEGATIVE = ~H3_DIGIT_MASK;
        /** H3 index with mode 0, res 0, base cell 0, and 7 for all index digits. */
        public static ulong H3_INIT = 35184372088831;
        /**
         * Invalid index used to indicate an error from geoToH3 and related functions.
         */
        public static ulong H3_INVALID_INDEX = 0;

        /** Where the actual index is stored. */
        private ulong value;

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

        /**
         * Gets the integer mode of h3.
         */
        public static int H3_GET_MODE(ref H3Index h3)
        {
            return (int) ((h3 & H3_MODE_MASK) >> H3_MODE_OFFSET);
        } 

        /**
         * Sets the integer mode of h3 to v.
         */
        public static void H3_SET_MODE(ref H3Index h3, ulong v)
        {
            h3 = h3 & H3_MODE_MASK_NEGATIVE | (v << H3_MODE_OFFSET);
        }

        /**
         * Gets the integer base cell of h3.
         */
        public static int H3_GET_BASE_CELL(H3Index h3)
        {
            return (int)((h3 & H3_BC_MASK) >> H3_BC_OFFSET);
        }

        /**
         * Sets the integer base cell of h3 to bc.
         */
        public static void H3_SET_BASE_CELL(ref H3Index h3, int bc)
        {
            h3 = (h3 & H3_BC_MASK_NEGATIVE) | ((ulong)bc << H3_BC_OFFSET);
        }

        /**
         * Gets the integer resolution of h3.
         */
        public static int H3_GET_RESOLUTION(H3Index h3)
        {
            return (int) ((h3 & H3_RES_MASK) >> H3_RES_OFFSET);
        }

        /**
         * Sets the integer resolution of h3.
         */
        public static void H3_SET_RESOLUTION(ref H3Index h3, H3Index res)
        {
            h3 = (h3 & H3_RES_MASK_NEGATIVE) | (res << H3_RES_OFFSET);
        }
        /**
         * Gets the resolution res integer digit (0-7) of h3.
         */
        public static Direction H3_GET_INDEX_DIGIT(H3Index h3, int res)
        {
            return (Direction) ((h3 >> ((Constants.MAX_H3_RES - res) * H3_PER_DIGIT_OFFSET)) & H3_DIGIT_MASK);
        }

        /**
         * Sets a value in the reserved space. Setting to non-zero may produce invalid
         * indexes.
         */
        public static void H3_SET_RESERVED_BITS(ref H3Index h3, ulong v)
        {
            h3 = (h3 & H3_RESERVED_MASK_NEGATIVE) | (v << H3_RESERVED_OFFSET);
        }

        /**
         * Gets a value in the reserved space. Should always be zero for valid indexes.
         */
        public static int H3_GET_RESERVED_BITS(H3Index h3)
        {
            return (int) ((h3 & H3_RESERVED_MASK) >> H3_RESERVED_OFFSET);
        }

        /**
         * Sets the resolution res digit of h3 to the integer digit (0-7)
         */
        public static void H3_SET_INDEX_DIGIT(ref H3Index h3, int res, ulong digit)
        {
            h3 = (h3 & ~(H3_DIGIT_MASK << ((Constants.MAX_H3_RES - res) * H3_PER_DIGIT_OFFSET)))
                 |  ((ulong)digit << ((Constants.MAX_H3_RES - res) * H3_PER_DIGIT_OFFSET));
        }
        
        /**
         * Returns the H3 resolution of an H3 index.
         * @param h The H3 index.
         * @return The resolution of the H3 index argument.
         */
        public int h3GetResolution(H3Index h)
        {
            return H3_GET_RESOLUTION(h);
        }

        /**
         * Returns the H3 base cell number of an H3 index.
         * @param h The H3 index.
         * @return The base cell of the H3 index argument.
         */
        int h3GetBaseCell(H3Index h) { return H3_GET_BASE_CELL(h); }

        /**
         * Converts a string representation of an H3 index into an H3 index.
         * @param str The string representation of an H3 index.
         * @return The H3 index corresponding to the string argument, or 0 if invalid.
         */
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

        /**
         * Converts an H3 index into a string representation.
         * @param h The H3 index to convert.
         * @param str The string representation of the H3 index.
         * @param sz Size of the buffer `str`
         */
        void h3ToString(H3Index h, ref string str, int sz) {
            // An unsigned 64 bit integer will be expressed in at most
            // 16 digits plus 1 for the null terminator.
            if (sz < 17) {
                // Buffer is potentially not large enough.
                return;
            }

            str = h.ToString();
        }


        /**
         * Returns whether or not an H3 index is valid.
         * @param h The H3 index to validate.
         * @return 1 if the H3 index if valid, and 0 if it is not.
         */
        int h3IsValid(H3Index h)
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
            if (res < 0 || res > Constants.MAX_H3_RES)
            {
                return 0;
            }

            for (int r = 1; r <= res; r++)
            {
                Direction digit = H3_GET_INDEX_DIGIT(h, r);
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

        /**
         * Initializes an H3 index.
         * @param hp The H3 index to initialize.
         * @param res The H3 resolution to initialize the index to.
         * @param baseCell The H3 base cell to initialize the index to.
         * @param initDigit The H3 digit (0-7) to initialize all of the index digits to.
         */
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

        /**
         * h3ToParent produces the parent index for a given H3 index
         *
         * @param h H3Index to find parent of
         * @param parentRes The resolution to switch to (parent, grandparent, etc)
         *
         * @return H3Index of the parent, or 0 if you actually asked for a child
         */
        static H3Index h3ToParent(H3Index h, int parentRes)
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

        /**
         * maxH3ToChildrenSize returns the maximum number of children possible for a
         * given child level.
         *
         * @param h H3Index to find the number of children of
         * @param childRes The resolution of the child level you're interested in
         *
         * @return int count of maximum number of children (equal for hexagons, less for
         * pentagons
         */
        static int maxH3ToChildrenSize(H3Index h, int childRes)
        {
            int parentRes = H3_GET_RESOLUTION(h);
            if (parentRes > childRes) 
            {
                return 0;
            }
            return MathExtensions._ipow(7, childRes - parentRes);
        }

        /**
         * makeDirectChild takes an index and immediately returns the immediate child
         * index based on the specified cell number. Bit operations only, could generate
         * invalid indexes if not careful (deleted cell under a pentagon).
         *
         * @param h H3Index to find the direct child of
         * @param cellNumber int id of the direct child (0-6)
         *
         * @return The new H3Index for the child
         */
        public static H3Index makeDirectChild(H3Index h, int cellNumber)
        {
            int childRes = H3_GET_RESOLUTION(h) + 1;
            H3_SET_RESOLUTION(ref h, childRes);
            H3Index childH = h;
            H3_SET_INDEX_DIGIT(ref childH, childRes, (ulong) cellNumber);
            return childH;
        }

        /**
         * h3ToChildren takes the given hexagon id and generates all of the children
         * at the specified resolution storing them into the provided memory pointer.
         * It's assumed that maxH3ToChildrenSize was used to determine the allocation.
         *
         * @param h H3Index to find the children of
         * @param childRes int the child level to produce
         * @param children H3Index* the memory to store the resulting addresses in
         */
        static void h3ToChildren(H3Index h, int childRes,ref  List<H3Index> children)
        {
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
                            break;
                        }

                        var child = makeDirectChild(index, m);
                        realChildren.Add(child);
                    }
                }
                current = new List<H3Index>(realChildren);
                currentRes++;
            }

            children = new List<H3Index>(current);
        }

        /**
         * compact takes a set of hexagons all at the same resolution and compresses
         * them by pruning full child branches to the parent level. This is also done
         * for all parents recursively to get the minimum number of hex addresses that
         * perfectly cover the defined space.
         * @param h3Set Set of hexagons
         * @param compactedSet The output array of compressed hexagons (preallocated)
         * @param numHexes The size of the input and output arrays (possible that no
         * contiguous regions exist in the set at all and no compression possible)
         * @return an error code on bad input data
         *
         * We're going to modify this a little bit using some LINQ.
         *
         *
         */
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

                var orphans = generation.Where(hex => hex.Value.Count != 7);
                realCompacted.AddRange(orphans.SelectMany(valuePair => valuePair.Value));

                var nextgen = generation.Where(hex => hex.Value.Count == 7);
                remainingHexes = nextgen.Select(valuePair => new H3Index(valuePair.Key)).ToList();
                generation.Clear();
            }

            compactedSet.Capacity = numHexes;
            compactedSet = realCompacted.Select(number => new H3Index(number)).ToList();
            return 0;
        }

        /**
         * uncompact takes a compressed set of hexagons and expands back to the
         * original set of hexagons.
         * @param compactedSet Set of hexagons
         * @param numHexes The number of hexes in the input set
         * @param h3Set Output array of decompressed hexagons (preallocated)
         * @param maxHexes The size of the output array to bound check against
         * @param res The hexagon resolution to decompress to
         * @return An error code if output array is too small or any hexagon is
         * smaller than the output resolution.
         */
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


        /**
         * maxUncompactSize takes a compacted set of hexagons are provides an
         * upper-bound estimate of the size of the uncompacted set of hexagons.
         * @param compactedSet Set of hexagons
         * @param numHexes The number of hexes in the input set
         * @param res The hexagon resolution to decompress to
         * @return The number of hexagons to allocate memory for, or a negative
         * number if an error occurs.
         */
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

        /**
         * h3IsPentagon takes an H3Index and determines if it is actually a
         * pentagon.
         * @param h The H3Index to check.
         * @return Returns 1 if it is a pentagon, otherwise 0.
         */
        public static int h3IsPentagon(H3Index h)
        {
            var test = 
                BaseCells._isBaseCellPentagon(H3_GET_BASE_CELL(h)) &&
                _h3LeadingNonZeroDigit(h) == 0;
            return test ? 1 : 0;
        }

        /**
         * Returns the highest resolution non-zero digit in an H3Index.
         * @param h The H3Index.
         * @return The highest resolution non-zero digit in the H3Index.
         */
        public static Direction _h3LeadingNonZeroDigit(ulong h)
        {

            for (int r = 1; r <= H3_GET_RESOLUTION(h); r++)
                if (H3_GET_INDEX_DIGIT(h, r) > 0)
                {
                    return H3_GET_INDEX_DIGIT(h, r);
                }

            // if we're here it's all 0's
            return Direction.CENTER_DIGIT;
        }

        /**
         * Rotate an H3Index 60 degrees counter-clockwise about a pentagonal center.
         * @param h The H3Index.
         */
        static H3Index _h3RotatePent60ccw(H3Index h) {
            // rotate in place; skips any leading 1 digits (k-axis)

            int foundFirstNonZeroDigit = 0;
            for (int r = 1, res = H3_GET_RESOLUTION(h); r <= res; r++) {
                // rotate this digit
                H3_SET_INDEX_DIGIT(ref h, r, (ulong) CoordIJK._rotate60ccw(H3_GET_INDEX_DIGIT(h, r)));

                // look for the first non-zero digit so we
                // can adjust for deleted k-axes sequence
                // if necessary
                if ((foundFirstNonZeroDigit ==0) && H3_GET_INDEX_DIGIT(h, r) != 0)
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

        /**
         * Rotate an H3Index 60 degrees clockwise about a pentagonal center.
         * @param h The H3Index.
         */
        H3Index _h3RotatePent60cw(H3Index h) {
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

        /**
         * Rotate an H3Index 60 degrees counter-clockwise.
         * @param h The H3Index.
         */

        public static H3Index _h3Rotate60ccw(ref H3Index h)
        {
            for (int r = 1, res = H3_GET_RESOLUTION(h); r <= res; r++) {
                Direction oldDigit = H3_GET_INDEX_DIGIT(h, r);
                H3_SET_INDEX_DIGIT(ref h, r, (ulong) CoordIJK._rotate60ccw(oldDigit));
            }

            return h;
        }
        /**
         * Rotate an H3Index 60 degrees clockwise.
         * @param h The H3Index.
         */
        public static H3Index _h3Rotate60cw(ref H3Index h)
        {
            for (int r = 1, res = H3_GET_RESOLUTION(h); r <= res; r++)
            {
                Direction oldDigit = H3_GET_INDEX_DIGIT(h, r);
                H3_SET_INDEX_DIGIT(ref h, r, (ulong) CoordIJK._rotate60cw(oldDigit));
            }

            return h;
        }

        /**
         * Convert an FaceIJK address to the corresponding H3Index.
         * @param fijk The FaceIJK address.
         * @param res The cell resolution.
         * @return The encoded H3Index (or 0 on failure).
         */
        static H3Index _faceIjkToH3(ref FaceIJK fijk, int res)
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
            FaceIJK fijkBC = new FaceIJK(fijk.face, fijk.coord);

            // build the H3Index from finest res up
            // adjust r for the fact that the res 0 base cell offsets the index array
            CoordIJK ijk = fijkBC.coord;
            for (int r = res - 1; r >= 0; r--) {
                CoordIJK lastIJK = ijk;
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
                    h = _h3RotatePent60ccw(h);
                }
            } else {
                for (int i = 0; i < numRots; i++) {
                    h = _h3Rotate60ccw(ref h);
                }
            }

            return h;
        }

        /**
         * Encodes a coordinate on the sphere to the H3 index of the containing cell at
         * the specified resolution.
         *
         * Returns 0 on invalid input.
         *
         * @param g The spherical coordinates to encode.
         * @param res The desired H3 resolution for the encoding.
         * @return The encoded H3Index (or 0 on failure).
         */
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
            FaceIJK ._geoToFaceIjk(g, res, ref fijk);
            return _faceIjkToH3(ref fijk, res);
        }

        /**
          * Convert an H3Index to the FaceIJK address on a specified icosahedral face.
          * @param h The H3Index.
          * @param fijk The FaceIJK address, initialized with the desired face
          *        and normalized base cell coordinates.
          * @return Returns 1 if the possibility of overage exists, otherwise 0.
          */
        static int _h3ToFaceIjkWithInitializedFijk(H3Index h, ref FaceIJK fijk)
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


        /**
         * Convert an H3Index to a FaceIJK address.
         * @param h The H3Index.
         * @param fijk The corresponding FaceIJK address.
         */
        public static void _h3ToFaceIjk(H3Index h, ref FaceIJK fijk) {
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

        /**
         * Determines the spherical coordinates of the center point of an H3 index.
         *
         * @param h3 The H3 index.
         * @param g The spherical coordinates of the H3 cell center.
         */
        public static void h3ToGeo(H3Index h3, ref GeoCoord g)
        {
            FaceIJK fijk = new FaceIJK();
            _h3ToFaceIjk(h3, ref fijk);
            FaceIJK. _faceIjkToGeo(fijk, H3_GET_RESOLUTION(h3), ref g);
        }

        /**
         * Determines the cell boundary in spherical coordinates for an H3 index.
         *
         * @param h3 The H3 index.
         * @param gb The boundary of the H3 cell in spherical coordinates.
         */
        public static void h3ToGeoBoundary(H3Index h3, ref GeoBoundary gb) {
            FaceIJK fijk = new FaceIJK();
            _h3ToFaceIjk(h3, ref fijk);
            FaceIJK._faceIjkToGeoBoundary(
                ref fijk, H3_GET_RESOLUTION(h3),
                h3IsPentagon(h3), ref  gb
            );
        }

        /**
          * h3IsResClassIII takes a hexagon ID and determines if it is in a
          * Class III resolution (rotated versus the icosahedron and subject
          * to shape distortion adding extra points on icosahedron edges, making
          * them not true hexagons).
          * @param h The H3Index to check.
          * @return Returns 1 if the hexagon is class III, otherwise 0.
          */
        public static bool isResClassIII(int res)
        {
            return res % 2 == 1;
        }

        /**
         * Produces ijk+ coordinates for an index anchored by an origin.
         *
         * The coordinate space used by this function may have deleted
         * regions or warping due to pentagonal distortion.
         *
         * Coordinates are only comparable if they come from the same
         * origin index.
         *
         * @param origin An anchoring index for the ijk+ coordinate system.
         * @param index Index to find the coordinates of
         * @param out ijk+ coordinates of the index will be placed here on success
         * @return 0 on success, or another value on failure.
         */
        int h3ToIjk(H3Index origin, H3Index h3, ref CoordIJK out_coord) {
            if (H3_GET_MODE(ref origin) != Constants.H3_HEXAGON_MODE ||
                H3_GET_MODE(ref h3) != Constants.H3_HEXAGON_MODE) {
                // Only hexagon mode is relevant, since we can't
                // encode directionality in CoordIJK.
                return 1;
            }

            int res = H3_GET_RESOLUTION(origin);

            if (res != H3_GET_RESOLUTION(h3)) {
                return 1;
            }

            int originBaseCell = H3_GET_BASE_CELL(origin);
            int baseCell = H3_GET_BASE_CELL(h3);

            // Direction from origin base cell to index base cell
            Direction dir = 0;
            Direction revDir = 0;
            if (originBaseCell != baseCell) {
                dir = BaseCells._getBaseCellDirection(originBaseCell, baseCell);
                if (dir == Direction.INVALID_DIGIT) {
                    // Base cells are not neighbors, can't unfold.
                    return 2;
                }
                revDir = BaseCells._getBaseCellDirection(baseCell, originBaseCell);
                if (revDir == Direction.CENTER_DIGIT)
                {
                    throw new Exception("assert(revDir != Direction.CENTER_DIGIT);");
                }
            }

            int originOnPent = BaseCells._isBaseCellPentagon(originBaseCell) ? 1 : 0;
            int indexOnPent = BaseCells._isBaseCellPentagon(baseCell) ? 1 : 0;

            FaceIJK indexFijk = new FaceIJK();
            if (dir != Direction.CENTER_DIGIT) {
                // Rotate index into the orientation of the origin base cell.
                // cw because we are undoing the rotation into that base cell.
                int baseCellRotations = BaseCells.baseCellNeighbor60CCWRots[originBaseCell,(int)dir];
                if (indexOnPent == 1) {
                    for (int i = 0; i < baseCellRotations; i++) {
                        h3 = _h3RotatePent60cw(h3);

                        revDir = CoordIJK. _rotate60cw(revDir);
                        if (revDir == Direction.K_AXES_DIGIT)
                        {
                            revDir =CoordIJK. _rotate60cw(revDir);
                        }
                    }
                } else {
                    for (int i = 0; i < baseCellRotations; i++)
                    {
                        h3 = _h3Rotate60cw(ref h3);
                        revDir =CoordIJK. _rotate60cw(revDir);
                    }
                }
            }
            // Face is unused. This produces coordinates in base cell coordinate space.
            _h3ToFaceIjkWithInitializedFijk(h3, ref indexFijk);

            // Origin leading digit -> index leading digit -> rotations 60 cw
            // Either being 1 (K axis) is invalid.
            // No good default at 0.
             int[,] PENTAGON_ROTATIONS = {
                {0, -1, 0, 0, 0, 0, 0},        // 0
                {-1, -1, -1, -1, -1, -1, -1},  // 1
                {0, -1, 0, 0, 0, 1, 0},        // 2
                {0, -1, 0, 0, 1, 1, 0},        // 3
                {0, -1, 0, 5, 0, 0, 0},        // 4
                {0, -1, 5, 5, 0, 0, 0},        // 5
                {0, -1, 0, 0, 0, 0, 0},        // 6
            };
            // Simply prohibit many pentagon distortion cases rather than handling them.
            bool[,] FAILED_DIRECTIONS_II = {
                {false, false, false, false, false, false, false},  // 0
                {false, false, false, false, false, false, false},  // 1
                {false, false, false, false, true, false, false},   // 2
                {false, false, false, false, false, false, true},   // 3
                {false, false, false, true, false, false, false},   // 4
                {false, false, true, false, false, false, false},   // 5
                {false, false, false, false, false, true, false},   // 6
            };
             bool[,] FAILED_DIRECTIONS_III = {
                {false, false, false, false, false, false, false},  // 0
                {false, false, false, false, false, false, false},  // 1
                {false, false, false, false, false, true, false},   // 2
                {false, false, false, false, true, false, false},   // 3
                {false, false, true, false, false, false, false},   // 4
                {false, false, false, false, false, false, true},   // 5
                {false, false, false, true, false, false, false},   // 6
            };

            if (dir != Direction.CENTER_DIGIT ) {
                if (baseCell == originBaseCell)
                {
                    throw new Exception("assert(baseCell != originBaseCell);");
                }

                if ((originOnPent & indexOnPent) == 0)
                {
                    throw new Exception("assert(!(originOnPent && indexOnPent));");
                }
                

                int pentagonRotations = 0;
                int directionRotations = 0;

                if (originOnPent == 1) {
                    int originLeadingDigit =(int) _h3LeadingNonZeroDigit(origin);

                    if (isResClassIII(res) &&
                        FAILED_DIRECTIONS_III[originLeadingDigit,(int)dir] ||
                        !isResClassIII(res) &&
                        FAILED_DIRECTIONS_II[originLeadingDigit,(int)dir]) {
                        // TODO this part of the pentagon might not be unfolded
                        // correctly.
                        return 3;
                    }

                    directionRotations = PENTAGON_ROTATIONS[originLeadingDigit,(int)dir];
                    pentagonRotations = directionRotations;
                } else if (indexOnPent != 0) {
                    int indexLeadingDigit = (int)_h3LeadingNonZeroDigit(h3);

                    if ((isResClassIII(res) &&
                         FAILED_DIRECTIONS_III[indexLeadingDigit,(int)revDir]) ||
                        (!isResClassIII(res) &&
                         FAILED_DIRECTIONS_II[indexLeadingDigit,(int)revDir])) {
                        // TODO this part of the pentagon might not be unfolded
                        // correctly.
                        return 4;
                    }

                    pentagonRotations = PENTAGON_ROTATIONS[(int)revDir,indexLeadingDigit];
                }

                if (pentagonRotations < 0)
                {
                    throw new Exception("assert(pentagonRotations >= 0);");
                }

                if (directionRotations < 0)
                {
                    throw new Exception("assert(directionRotations >= 0);");
                }

                for (int i = 0; i < pentagonRotations; i++) {
                    CoordIJK._ijkRotate60cw(ref indexFijk.coord);
                }

                CoordIJK offset = new CoordIJK();
                CoordIJK._neighbor( ref offset, dir);
                // Scale offset based on resolution
                for (int r = res - 1; r >= 0; r--) {
                    if (isResClassIII(r + 1)) {
                        // rotate ccw
                        CoordIJK._downAp7(ref offset);
                    } else {
                        // rotate cw
                        CoordIJK ._downAp7r(ref offset);
                    }
                }

                for (int i = 0; i < directionRotations; i++) {
                    CoordIJK._ijkRotate60cw(ref offset);
                }

                // Perform necessary translation
                CoordIJK._ijkAdd(indexFijk.coord, offset, ref indexFijk.coord);
                CoordIJK._ijkNormalize(ref indexFijk.coord);
            } else if (originOnPent==1  && indexOnPent==1) {
                // If the origin and index are on pentagon, and we checked that the base
                // cells are the same or neighboring, then they must be the same base
                // cell.
                if (baseCell != originBaseCell)
                {
                    throw new Exception("assert(baseCell == originBaseCell);");
                }
                

                int originLeadingDigit = (int) _h3LeadingNonZeroDigit(origin);
                int indexLeadingDigit = (int) _h3LeadingNonZeroDigit(h3);

                if (FAILED_DIRECTIONS_III[originLeadingDigit,indexLeadingDigit] ||
                    FAILED_DIRECTIONS_II[originLeadingDigit,indexLeadingDigit]) {
                    // TODO this part of the pentagon might not be unfolded
                    // correctly.
                    return 5;
                }

                int withinPentagonRotations =
                    PENTAGON_ROTATIONS[originLeadingDigit,indexLeadingDigit];

                for (int i = 0; i < withinPentagonRotations; i++) {
                    CoordIJK._ijkRotate60cw(ref indexFijk.coord);
                }
            }

            out_coord = indexFijk.coord;
            return 0;
        }
// Internal functions


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





 

    }
}
