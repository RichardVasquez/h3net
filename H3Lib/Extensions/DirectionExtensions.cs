namespace H3Lib.Extensions
{
    /// <summary>
    /// Operations for Direction enum type
    /// </summary>
    public static class DirectionExtensions
    {
        /// <summary>
        /// Rotates indexing digit 60 degrees counter-clockwise. Returns result.
        /// </summary>
        /// <param name="digit">Indexing digit (between 1 and 6 inclusive)</param>
        /// <!--
        /// coordijk.c
        /// Direction _rotate60ccw
        /// -->
        internal static Direction Rotate60CounterClockwise(this Direction digit)
        {
            switch (digit)
            {
                case Direction.K_AXES_DIGIT: return Direction.IK_AXES_DIGIT;
                case Direction.IK_AXES_DIGIT: return Direction.I_AXES_DIGIT;
                case Direction.I_AXES_DIGIT: return Direction.IJ_AXES_DIGIT;
                case Direction.IJ_AXES_DIGIT: return Direction.J_AXES_DIGIT;
                case Direction.J_AXES_DIGIT: return Direction.JK_AXES_DIGIT;
                case Direction.JK_AXES_DIGIT: return Direction.K_AXES_DIGIT;
                default: return digit;
            };
        }

        /// <summary>
        /// Rotates indexing digit 60 degrees clockwise. Returns result.
        /// </summary>
        /// <param name="digit">Indexing digit (between 1 and 6 inclusive)</param>
        /// <!--
        /// coordijk.c
        /// Direction _rotate60cw
        /// -->
        internal static Direction Rotate60Clockwise(this Direction digit)
        {
            switch(digit)
            {
                case Direction.K_AXES_DIGIT: return Direction.JK_AXES_DIGIT;
                case Direction.JK_AXES_DIGIT: return Direction.J_AXES_DIGIT;
                case Direction.J_AXES_DIGIT: return Direction.IJ_AXES_DIGIT;
                case Direction.IJ_AXES_DIGIT: return Direction.I_AXES_DIGIT;
                case Direction.I_AXES_DIGIT: return Direction.IK_AXES_DIGIT;
                case Direction.IK_AXES_DIGIT: return Direction.K_AXES_DIGIT;
                default: return digit;
            };
        }
    }
}
