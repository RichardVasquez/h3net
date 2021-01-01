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
        CENTER_DIGIT = 0,

        /// <summary>
        /// H3 digit in k-axes direction
        /// </summary>
        K_AXES_DIGIT = 1,

        /// <summary>
        /// H3 digit in j-axes direction
        /// </summary>
        J_AXES_DIGIT = 2,

        /// <summary>
        /// H3 digit in j==k direction
        /// </summary>
        JK_AXES_DIGIT = J_AXES_DIGIT | K_AXES_DIGIT,

        /// <summary>
        /// H3 digit in i-axes direction
        /// </summary>
        I_AXES_DIGIT = 4,

        /// <summary>
        /// H3 digit in i==k direction
        /// </summary>
        IK_AXES_DIGIT = I_AXES_DIGIT | K_AXES_DIGIT,

        /// <summary>
        /// H3 digit in i==j direction
        /// </summary>
        IJ_AXES_DIGIT = I_AXES_DIGIT | J_AXES_DIGIT,

        /// <summary>
        /// H3 digit in the invalid direction
        /// </summary>
        INVALID_DIGIT = 7,

        /// <summary>
        /// Valid digits will be less than this value. Same value as <see cref="INVALID_DIGIT"/>
        /// </summary>
        NUM_DIGITS = INVALID_DIGIT
    }
}
