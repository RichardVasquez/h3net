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
            return digit switch
            {
                Direction.K_AXES_DIGIT => Direction.IK_AXES_DIGIT,
                Direction.IK_AXES_DIGIT => Direction.I_AXES_DIGIT,
                Direction.I_AXES_DIGIT => Direction.IJ_AXES_DIGIT,
                Direction.IJ_AXES_DIGIT => Direction.J_AXES_DIGIT,
                Direction.J_AXES_DIGIT => Direction.JK_AXES_DIGIT,
                Direction.JK_AXES_DIGIT => Direction.K_AXES_DIGIT,
                _ => digit
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
            return digit switch
            {
                Direction.K_AXES_DIGIT => Direction.JK_AXES_DIGIT,
                Direction.JK_AXES_DIGIT => Direction.J_AXES_DIGIT,
                Direction.J_AXES_DIGIT => Direction.IJ_AXES_DIGIT,
                Direction.IJ_AXES_DIGIT => Direction.I_AXES_DIGIT,
                Direction.I_AXES_DIGIT => Direction.IK_AXES_DIGIT,
                Direction.IK_AXES_DIGIT => Direction.K_AXES_DIGIT,
                _ => digit
            };
        }
    }
}
