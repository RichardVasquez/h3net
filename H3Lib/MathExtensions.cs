namespace H3Lib
{
    /// <summary>
    /// Math functions that should have been in math.h but aren't
    /// </summary>
    /// <!-- Based off 3.1.1 -->
    public class MathExtensions
    {
        /// <summary>
        /// _ipow does integer exponentiation efficiently. Taken from StackOverflow.
        /// </summary>
        /// <param name="nbase">the integer base</param>
        /// <param name="exp">the integer exponent</param>
        /// <!-- Based off 3.1.1 -->
        public static int _ipow(int nbase, int exp)
        {
            int result = 1;
            while (exp > 0) {
                if ((exp & 1) == 1)
                {
                    result *= nbase;
                }
                exp >>= 1;
                nbase *= nbase;
            }

            return result;
        }

    }
}
