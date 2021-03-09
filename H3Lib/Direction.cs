namespace H3Lib
{
    /// <summary>
    /// H3 digit representing ijk+ axes direction.
    /// Values will be within the lowest 3 bits of an integer.
    /// </summary>
    public enum Direction
    {
        /// <summary>
        /// H3 digit in center
        /// </summary>
        CenterDigit = 0,

        /// <summary>
        /// H3 digit in k-axes direction
        /// </summary>
        KAxesDigit = 1,

        /// <summary>
        /// H3 digit in j-axes direction
        /// </summary>
        JAxesDigit = 2,

        /// <summary>
        /// H3 digit in j==k direction
        /// </summary>
        JKAxesDigit = JAxesDigit | KAxesDigit,

        /// <summary>
        /// H3 digit in i-axes direction
        /// </summary>
        // ReSharper disable once InconsistentNaming
        IAxesDigit = 4,

        /// <summary>
        /// H3 digit in i==k direction
        /// </summary>
        IKAxesDigit = IAxesDigit | KAxesDigit,

        /// <summary>
        /// H3 digit in i==j direction
        /// </summary>
        IJAxesDigit = IAxesDigit | JAxesDigit,

        /// <summary>
        /// H3 digit in the invalid direction
        /// </summary>
        InvalidDigit = 7,

        /// <summary>
        /// Valid digits will be less than this value. Same value as <see cref="InvalidDigit"/>
        /// </summary>
        NumDigits = InvalidDigit
    }
}
