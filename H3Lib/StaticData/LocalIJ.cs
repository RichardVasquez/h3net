namespace H3Lib.StaticData
{
    public static class LocalIJ
    {
        /// <summary>
                /// Origin leading digit -&gt; index leading digit -&gt; rotations 60 cw
                /// Either being 1 (K axis) is invalid.
                /// No good default at 0.
                /// </summary>
                internal static readonly int[,] PENTAGON_ROTATIONS =
                {
                    { 0, -1,  0,  0,  0,  0,  0}, // 0
                    {-1, -1, -1, -1, -1, -1, -1}, // 1
                    { 0, -1,  0,  0,  0,  1,  0}, // 2
                    { 0, -1,  0,  0,  1,  1,  0}, // 3
                    { 0, -1,  0,  5,  0,  0,  0}, // 4
                    { 0, -1,  5,  5,  0,  0,  0}, // 5
                    { 0, -1,  0,  0,  0,  0,  0}  // 6
                };

        /// <summary>
                /// Reverse base cell direction -&gt; leading index digit -&gt; rotations 60 ccw.
                /// For reversing the rotation introduced in PENTAGON_ROTATIONS when the index is
                /// on a pentagon and the origin is not.
                /// </summary>
                internal  static readonly int[,] PENTAGON_ROTATIONS_REVERSE =
                {
                    {0, 0, 0, 0, 0, 0, 0}, // 0
                    {-1, -1, -1, -1, -1, -1, -1}, // 1
                    {0, 1, 0, 0, 0, 0, 0}, // 2
                    {0, 1, 0, 0, 0, 1, 0}, // 3
                    {0, 5, 0, 0, 0, 0, 0}, // 4
                    {0, 5, 0, 5, 0, 0, 0}, // 5
                    {0, 0, 0, 0, 0, 0, 0}  // 6
                };

        /// <summary>
                /// Reverse base cell direction -&gt; leading index digit -&gt; rotations 60 ccw.
                /// For reversing the rotation introduced in PENTAGON_ROTATIONS when the index is
                /// on a pentagon and the origin is not.
                /// </summary>
                internal static readonly int[,] PENTAGON_ROTATIONS_REVERSE_NONPOLAR =
                {
                    {0, 0, 0, 0, 0, 0, 0},         // 0
                    {-1, -1, -1, -1, -1, -1, -1},  // 1
                    {0, 1, 0, 0, 0, 0, 0},         // 2
                    {0, 1, 0, 0, 0, 1, 0},         // 3
                    {0, 5, 0, 0, 0, 0, 0},         // 4
                    {0, 1, 0, 5, 1, 1, 0},         // 5
                    {0, 0, 0, 0, 0, 0, 0},         // 6
                };

        /// <summary>
                /// Reverse base cell direction -&gt; leading index digit -&gt; rotations 60 ccw.
                /// For reversing the rotation introduced in PENTAGON_ROTATIONS when the index is
                /// on a polar pentagon and the origin is not.
                /// </summary>
                internal static readonly int[,] PENTAGON_ROTATIONS_REVERSE_POLAR =
                {
                    {0, 0, 0, 0, 0, 0, 0},         // 0
                    {-1, -1, -1, -1, -1, -1, -1},  // 1
                    {0, 1, 1, 1, 1, 1, 1},         // 2
                    {0, 1, 0, 0, 0, 1, 0},         // 3
                    {0, 1, 0, 0, 1, 1, 1},         // 4
                    {0, 1, 0, 5, 1, 1, 0},         // 5
                    {0, 1, 1, 0, 1, 1, 1}          // 6
                };

        /**
         * Prohibited directions when unfolding a pentagon.
         *
         * Indexes by two directions, both relative to the pentagon base cell. The first
         * is the direction of the origin index and the second is the direction of the
         * index to unfold. Direction refers to the direction from base cell to base
         * cell if the indexes are on different base cells, or the leading digit if
         * within the pentagon base cell.
         *
         * This previously included a Class II/Class III check but these were removed
         * due to failure cases. It's possible this could be restricted to a narrower
         * set of a failure cases. Currently, the logic is any unfolding across more
         * than one icosahedron face is not permitted.
         */
                internal static readonly bool[,] FAILED_DIRECTIONS =
                {
                    {false, false, false, false, false, false, false}, // 0
                    {false, false, false, false, false, false, false}, // 1
                    {false, false, false, false, true, true, false}, // 2
                    {false, false, false, false, true, false, true}, // 3
                    {false, false, true, true, false, false, false}, // 4
                    {false, false, true, false, false, false, true}, // 5
                    {false, false, false, true, false, true, false}, // 6
                };
    }
}
