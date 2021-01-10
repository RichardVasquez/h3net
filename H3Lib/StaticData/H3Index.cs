namespace H3Lib.StaticData
{
    public static class H3Index
    {
        /// <summary>
        /// Invalid index used to indicate an error from geoToH3 and related functions.
        /// </summary>
        public static readonly ulong H3_INVALID_INDEX = 0;

        /// <summary>
        /// Invalid index used to indicate an error from geoToH3 and related functions
        /// or missing data in arrays of h3 indices. Analogous to NaN in floating point.
        /// </summary>
        public static readonly ulong H3_NULL = 0;

        /// <summary>
        /// The number of bits in an H3 index.
        /// </summary>
        public static readonly int H3_NUM_BITS = 64;

        /// <summary>
        /// The bit offset of the max resolution digit in an H3 index.
        /// </summary>
        public static readonly int H3_MAX_OFFSET = 63;

        /// <summary>
        /// The bit offset of the mode in an H3 index.
        /// </summary>
        public static readonly int H3_MODE_OFFSET = 59;

        /// <summary>
        /// The bit offset of the base cell in an H3 index.
        /// </summary>
        public static readonly int H3_BC_OFFSET = 45;

        /// <summary>
        /// The bit offset of the resolution in an H3 index.
        /// </summary>
        public static readonly int H3_RES_OFFSET = 52;

        /// <summary>
        /// The bit offset of the reserved bits in an H3 index.
        /// </summary>
        public static readonly int H3_RESERVED_OFFSET = 56;

        /// <summary>
        /// The number of bits in a single H3 resolution digit.
        /// </summary>
        public static readonly int H3_PER_DIGIT_OFFSET = 3;

        /// <summary>
        /// 1 in the highest bit, 0's everywhere else.
        /// </summary>
        public static readonly ulong H3_HIGH_BIT_MASK = (ulong) 1 << H3_MAX_OFFSET;

        /// <summary>
        /// 0 in the highest bit, 1's everywhere else.
        /// </summary>
        public static readonly ulong H3_HIGH_BIT_MASK_NEGATIVE = ~H3_HIGH_BIT_MASK;

        /// <summary>
        /// 1's in the 4 mode bits, 0's everywhere else.
        /// </summary>
        public static readonly ulong H3_MODE_MASK = (ulong)15 << H3_MODE_OFFSET;

        /// <summary>
        /// 0's in the 4 mode bits, 1's everywhere else.
        /// </summary>
        public static readonly ulong H3_MODE_MASK_NEGATIVE = ~H3_MODE_MASK;

        /// <summary>
        /// 1's in the 7 base cell bits, 0's everywhere else.
        /// </summary>
        public static readonly ulong H3_BC_MASK = (ulong) 127 << H3_BC_OFFSET;

        /// <summary>
        /// 0's in the 7 base cell bits, 1's everywhere else.
        /// </summary>
        public static readonly ulong H3_BC_MASK_NEGATIVE = ~H3_BC_MASK;

        /// <summary>
        /// 1's in the 4 resolution bits, 0's everywhere else.
        /// </summary>
        public static readonly ulong H3_RES_MASK = (ulong) 15 << H3_RES_OFFSET;

        /// <summary>
        /// 0's in the 4 resolution bits, 1's everywhere else.
        /// </summary>
        public static readonly ulong H3_RES_MASK_NEGATIVE = ~H3_RES_MASK;

        /// <summary>
        /// 1's in the 3 reserved bits, 0's everywhere else.
        /// </summary>
        public static readonly ulong H3_RESERVED_MASK = (ulong) 7 << H3_RESERVED_OFFSET;

        /// <summary>
        /// 0's in the 3 reserved bits, 1's everywhere else.
        /// </summary>
        public static readonly ulong H3_RESERVED_MASK_NEGATIVE = ~H3_RESERVED_MASK;

        /// <summary>
        /// 1's in the 3 bits of res 15 digit bits, 0's everywhere else.
        /// </summary>
        public static readonly ulong H3_DIGIT_MASK = 7;

        /// <summary>
        /// 0's in the 7 base cell bits, 1's everywhere else.
        /// </summary>
        public static readonly ulong H3_DIGIT_MASK_NEGATIVE = ~H3_DIGIT_MASK;

        /// <summary>
        /// H3 index with mode 0, res 0, base cell 0, and 7 for all index digits.
        /// </summary>
        public static readonly ulong H3_INIT = 35184372088831;

        /*
         * Return codes for compact
         */
        public const int COMPACT_SUCCESS = 0;
        public const int COMPACT_LOOP_EXCEEDED = -1;
        public const int COMPACT_DUPLICATE = -2;
        public const int COMPACT_ALLOC_FAILED = -3;
        public const int COMPACT_BAD_DATA = -10;
    }
}
