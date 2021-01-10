namespace H3Lib.StaticData
{
    public static class Algos
    {
        /*
         * Return codes from hexRange and related functions.
        */
        public const int HexRangeSuccess = 0;
        public const int HexRangePentagon = 1;
        public const int HexRangeKSubsequence = 2;
        public const int MaxOneRingSize = 7;
        public const int HexHashOverflow = -1;
        public const int PolyfillBuffer = 12;

        /// <summary>
        ///      _
        ///    _/ \_      Directions used for traversing a        
        ///   / \5/ \     hexagonal ring counterclockwise
        ///   \0/ \4/     around {1, 0, 0}
        ///   / \_/ \
        ///   \1/ \3/
        ///     \2/
        /// </summary>
        /// <!--
        /// algos.c
        /// -->
        public static readonly Direction[] Directions =
        {
            Direction.J_AXES_DIGIT, Direction.JK_AXES_DIGIT,
            Direction.K_AXES_DIGIT, Direction.IK_AXES_DIGIT,
            Direction.I_AXES_DIGIT, Direction.IJ_AXES_DIGIT
        };

        /// <summary>
        /// Direction used for traversing to the next outward hexagonal ring.
        /// </summary>
        /// <!--
        /// Algos.c
        /// NEXT_RING_DIRECTION
        /// -->
        public const Direction NextRingDirection = Direction.I_AXES_DIGIT;

        /// <summary>
        /// New digit when traversing along class II grids.
        ///
        /// Current digit -> direction -> new digit.
        /// </summary>
        /// <!--
        /// Algos.c
        /// NEW_DIGIT_II
        /// -->
        public static readonly Direction[,] NewDigitIi =
        {
            {
                Direction.CENTER_DIGIT, Direction.K_AXES_DIGIT, Direction.J_AXES_DIGIT,
                Direction.JK_AXES_DIGIT, Direction.I_AXES_DIGIT, Direction.IK_AXES_DIGIT,
                Direction.IJ_AXES_DIGIT
            },
            {
                Direction.K_AXES_DIGIT, Direction.I_AXES_DIGIT, Direction.JK_AXES_DIGIT,
                Direction.IJ_AXES_DIGIT, Direction.IK_AXES_DIGIT, Direction.J_AXES_DIGIT,
                Direction.CENTER_DIGIT
            },
            {
                Direction.J_AXES_DIGIT, Direction.JK_AXES_DIGIT, Direction.K_AXES_DIGIT,
                Direction.I_AXES_DIGIT, Direction.IJ_AXES_DIGIT, Direction.CENTER_DIGIT,
                Direction.IK_AXES_DIGIT
            },
            {
                Direction.JK_AXES_DIGIT, Direction.IJ_AXES_DIGIT, Direction.I_AXES_DIGIT,
                Direction.IK_AXES_DIGIT, Direction.CENTER_DIGIT, Direction.K_AXES_DIGIT,
                Direction.J_AXES_DIGIT
            },
            {
                Direction.I_AXES_DIGIT, Direction.IK_AXES_DIGIT, Direction.IJ_AXES_DIGIT,
                Direction.CENTER_DIGIT, Direction.J_AXES_DIGIT, Direction.JK_AXES_DIGIT,
                Direction.K_AXES_DIGIT
            },
            {
                Direction.IK_AXES_DIGIT, Direction.J_AXES_DIGIT, Direction.CENTER_DIGIT,
                Direction.K_AXES_DIGIT, Direction.JK_AXES_DIGIT, Direction.IJ_AXES_DIGIT,
                Direction.I_AXES_DIGIT
            },
            {
                Direction.IJ_AXES_DIGIT, Direction.CENTER_DIGIT, Direction.IK_AXES_DIGIT,
                Direction.J_AXES_DIGIT, Direction.K_AXES_DIGIT, Direction.I_AXES_DIGIT,
                Direction.JK_AXES_DIGIT
            }
        };

        /// <summary>
        /// New traversal direction when traversing along class II grids.
        ///
        /// Current digit -> direction -> new ap7 move (at coarser level).
        /// </summary>
        /// <!--
        /// Algos.c
        /// NEW_ADJUSTMENT_II
        /// -->
        public static readonly Direction[,] NewAdjustmentIi =
        {
            {
                Direction.CENTER_DIGIT, Direction.CENTER_DIGIT, Direction.CENTER_DIGIT,
                Direction.CENTER_DIGIT, Direction.CENTER_DIGIT, Direction.CENTER_DIGIT,
                Direction.CENTER_DIGIT
            },
            {
                Direction.CENTER_DIGIT, Direction.K_AXES_DIGIT, Direction.CENTER_DIGIT,
                Direction.K_AXES_DIGIT, Direction.CENTER_DIGIT, Direction.IK_AXES_DIGIT,
                Direction.CENTER_DIGIT
            },
            {
                Direction.CENTER_DIGIT, Direction.CENTER_DIGIT, Direction.J_AXES_DIGIT,
                Direction.JK_AXES_DIGIT, Direction.CENTER_DIGIT, Direction.CENTER_DIGIT,
                Direction.J_AXES_DIGIT
            },
            {
                Direction.CENTER_DIGIT, Direction.K_AXES_DIGIT, Direction.JK_AXES_DIGIT,
                Direction.JK_AXES_DIGIT, Direction.CENTER_DIGIT, Direction.CENTER_DIGIT,
                Direction.CENTER_DIGIT
            },
            {
                Direction.CENTER_DIGIT, Direction.CENTER_DIGIT, Direction.CENTER_DIGIT,
                Direction.CENTER_DIGIT, Direction.I_AXES_DIGIT, Direction.I_AXES_DIGIT,
                Direction.IJ_AXES_DIGIT
            },
            {
                Direction.CENTER_DIGIT, Direction.IK_AXES_DIGIT, Direction.CENTER_DIGIT,
                Direction.CENTER_DIGIT, Direction.I_AXES_DIGIT, Direction.IK_AXES_DIGIT,
                Direction.CENTER_DIGIT
            },
            {
                Direction.CENTER_DIGIT, Direction.CENTER_DIGIT, Direction.J_AXES_DIGIT,
                Direction.CENTER_DIGIT, Direction.IJ_AXES_DIGIT, Direction.CENTER_DIGIT,
                Direction.IJ_AXES_DIGIT
            }
        };

        /// <summary>
        /// New traversal direction when traversing along class III grids.
        ///
        /// Current digit -&gt; direction -&gt; new ap7 move (at coarser level).
        /// </summary>
        /// <!--
        /// Algos.c
        /// NEW_DIGIT_III
        /// -->
        public static readonly Direction[,] NewDigitIii =
        {
            {
                Direction.CENTER_DIGIT, Direction.K_AXES_DIGIT, Direction.J_AXES_DIGIT,
                Direction.JK_AXES_DIGIT, Direction.I_AXES_DIGIT, Direction.IK_AXES_DIGIT,
                Direction.IJ_AXES_DIGIT
            },
            {
                Direction.K_AXES_DIGIT, Direction.J_AXES_DIGIT, Direction.JK_AXES_DIGIT,
                Direction.I_AXES_DIGIT, Direction.IK_AXES_DIGIT, Direction.IJ_AXES_DIGIT,
                Direction.CENTER_DIGIT
            },
            {
                Direction.J_AXES_DIGIT, Direction.JK_AXES_DIGIT, Direction.I_AXES_DIGIT,
                Direction.IK_AXES_DIGIT, Direction.IJ_AXES_DIGIT, Direction.CENTER_DIGIT,
                Direction.K_AXES_DIGIT
            },
            {
                Direction.JK_AXES_DIGIT, Direction.I_AXES_DIGIT, Direction.IK_AXES_DIGIT,
                Direction.IJ_AXES_DIGIT, Direction.CENTER_DIGIT, Direction.K_AXES_DIGIT,
                Direction.J_AXES_DIGIT
            },
            {
                Direction.I_AXES_DIGIT, Direction.IK_AXES_DIGIT, Direction.IJ_AXES_DIGIT,
                Direction.CENTER_DIGIT, Direction.K_AXES_DIGIT, Direction.J_AXES_DIGIT,
                Direction.JK_AXES_DIGIT
            },
            {
                Direction.IK_AXES_DIGIT, Direction.IJ_AXES_DIGIT, Direction.CENTER_DIGIT,
                Direction.K_AXES_DIGIT, Direction.J_AXES_DIGIT, Direction.JK_AXES_DIGIT,
                Direction.I_AXES_DIGIT
            },
            {
                Direction.IJ_AXES_DIGIT, Direction.CENTER_DIGIT, Direction.K_AXES_DIGIT,
                Direction.J_AXES_DIGIT, Direction.JK_AXES_DIGIT, Direction.I_AXES_DIGIT,
                Direction.IK_AXES_DIGIT
            }
        };

        /// <summary>
        /// New traversal direction when traversing along class III grids.
        ///
        /// Current digit -gt; direction -gt; new ap7 move (at coarser level).
        /// </summary>
        /// <!--
        /// algos.c
        /// NEW_ADJUSTMENT_III
        /// -->
        public static readonly Direction[,] NewAdjustmentIii =
        {
            {
                Direction.CENTER_DIGIT, Direction.CENTER_DIGIT, Direction.CENTER_DIGIT,
                Direction.CENTER_DIGIT, Direction.CENTER_DIGIT, Direction.CENTER_DIGIT,
                Direction.CENTER_DIGIT
            },
            {
                Direction.CENTER_DIGIT, Direction.K_AXES_DIGIT, Direction.CENTER_DIGIT,
                Direction.JK_AXES_DIGIT, Direction.CENTER_DIGIT, Direction.K_AXES_DIGIT,
                Direction.CENTER_DIGIT
            },
            {
                Direction.CENTER_DIGIT, Direction.CENTER_DIGIT, Direction.J_AXES_DIGIT,
                Direction.J_AXES_DIGIT, Direction.CENTER_DIGIT, Direction.CENTER_DIGIT,
                Direction.IJ_AXES_DIGIT
            },
            {
                Direction.CENTER_DIGIT, Direction.JK_AXES_DIGIT, Direction.J_AXES_DIGIT,
                Direction.JK_AXES_DIGIT, Direction.CENTER_DIGIT, Direction.CENTER_DIGIT,
                Direction.CENTER_DIGIT
            },
            {
                Direction.CENTER_DIGIT, Direction.CENTER_DIGIT, Direction.CENTER_DIGIT,
                Direction.CENTER_DIGIT, Direction.I_AXES_DIGIT, Direction.IK_AXES_DIGIT,
                Direction.I_AXES_DIGIT
            },
            {
                Direction.CENTER_DIGIT, Direction.K_AXES_DIGIT, Direction.CENTER_DIGIT,
                Direction.CENTER_DIGIT, Direction.IK_AXES_DIGIT, Direction.IK_AXES_DIGIT,
                Direction.CENTER_DIGIT
            },
            {
                Direction.CENTER_DIGIT, Direction.CENTER_DIGIT, Direction.IJ_AXES_DIGIT,
                Direction.CENTER_DIGIT, Direction.I_AXES_DIGIT, Direction.CENTER_DIGIT,
                Direction.IJ_AXES_DIGIT
            }
        };

    }
}
