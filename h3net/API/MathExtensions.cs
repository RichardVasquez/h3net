namespace h3net.API
{
    public class MathExtensions
    {
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
