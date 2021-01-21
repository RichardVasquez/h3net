using System.Collections.Generic;

namespace H3Lib.StaticData
{
    public static class CoordIjk
    {
        /// <summary>
        /// CoordIJK unit vectors corresponding to the 7 H3 digits.
        /// </summary>
        public static readonly H3Lib.CoordIjk[] UnitVecs =
        {
            new H3Lib.CoordIjk(0, 0, 0),  // direction 0
            new H3Lib.CoordIjk(0, 0, 1),  // direction 1
            new H3Lib.CoordIjk(0, 1, 0),  // direction 2
            new H3Lib.CoordIjk(0, 1, 1),  // direction 3
            new H3Lib.CoordIjk(1, 0, 0),  // direction 4
            new H3Lib.CoordIjk(1, 0, 1),  // direction 5
            new H3Lib.CoordIjk(1, 1, 0)   // direction 6
        };

        public static readonly Dictionary<Direction, H3Lib.CoordIjk> UnitVectors =
            new Dictionary<Direction, H3Lib.CoordIjk>
            {
                {Direction.CENTER_DIGIT, new H3Lib.CoordIjk(0, 0, 0)},
                {Direction.K_AXES_DIGIT, new H3Lib.CoordIjk(0, 0, 1)},
                {Direction.J_AXES_DIGIT, new H3Lib.CoordIjk(0, 1, 0)},
                {Direction.JK_AXES_DIGIT, new H3Lib.CoordIjk(0, 1, 1)},
                {Direction.I_AXES_DIGIT, new H3Lib.CoordIjk(1, 0, 0)},
                {Direction.IK_AXES_DIGIT, new H3Lib.CoordIjk(1, 0, 1)},
                {Direction.IJ_AXES_DIGIT, new H3Lib.CoordIjk(1, 1, 0)},
            };

    }
}
