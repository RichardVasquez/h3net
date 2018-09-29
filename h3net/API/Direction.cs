namespace h3net.API
{
    public enum Direction
    {
        CENTER_DIGIT = 0,
        K_AXES_DIGIT = 1,
        J_AXES_DIGIT = 2,
        JK_AXES_DIGIT = J_AXES_DIGIT|K_AXES_DIGIT,
        I_AXES_DIGIT = 4,
        IK_AXES_DIGIT = I_AXES_DIGIT|K_AXES_DIGIT,
        IJ_AXES_DIGIT = I_AXES_DIGIT|J_AXES_DIGIT,
        INVALID_DIGIT = 7,
        NUM_DIGITS = INVALID_DIGIT
    }
}
