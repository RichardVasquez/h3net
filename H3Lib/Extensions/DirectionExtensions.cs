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
        /// <remarks>
        /// 3.7.1
        /// coordijk.c
        /// Direction _rotate60ccw
        /// </remarks>
        internal static Direction Rotate60CounterClockwise(this Direction digit)
        {
            return digit switch
            {
                Direction.KAxesDigit => Direction.IKAxesDigit,
                Direction.IKAxesDigit => Direction.IAxesDigit,
                Direction.IAxesDigit => Direction.IJAxesDigit,
                Direction.IJAxesDigit => Direction.JAxesDigit,
                Direction.JAxesDigit => Direction.JKAxesDigit,
                Direction.JKAxesDigit => Direction.KAxesDigit,
                _ => digit
            };
        }

        /// <summary>
        /// Rotates indexing digit 60 degrees clockwise. Returns result.
        /// </summary>
        /// <param name="digit">Indexing digit (between 1 and 6 inclusive)</param>
        /// <remarks>
        /// coordijk.c
        /// Direction _rotate60cw
        /// </remarks>
        internal static Direction Rotate60Clockwise(this Direction digit)
        {
            return digit switch
            {
                Direction.KAxesDigit => Direction.JKAxesDigit,
                Direction.JKAxesDigit => Direction.JAxesDigit,
                Direction.JAxesDigit => Direction.IJAxesDigit,
                Direction.IJAxesDigit => Direction.IAxesDigit,
                Direction.IAxesDigit => Direction.IKAxesDigit,
                Direction.IKAxesDigit => Direction.KAxesDigit,
                _ => digit
            };
        }
    }
}
