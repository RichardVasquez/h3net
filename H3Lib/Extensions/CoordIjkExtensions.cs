using System;
using System.Linq;

namespace H3Lib.Extensions
{
    public static class CoordIjkExtensions
    {
        public static CoordIjk SetI(this CoordIjk ijk, int i)
        {
            return new CoordIjk(i, ijk.J, ijk.K);
        }
        public static CoordIjk SetJ(this CoordIjk ijk, int j)
        {
            return new CoordIjk(ijk.I, j, ijk.K);
        }
        public static CoordIjk SetK(this CoordIjk ijk, int k)
        {
            return new CoordIjk(ijk.I, ijk.J, k);
        }

        public static CoordIjk SetIJ(this CoordIjk ijk, int i, int j)
        {
            return new CoordIjk(i, j, ijk.K);
        }

        public static CoordIjk SetIK(this CoordIjk ijk, int i, int k)
        {
            return new CoordIjk(i, ijk.J, k);
        }

        public static CoordIjk SetJK(this CoordIjk ijk, int j, int k)
        {
            return new CoordIjk(ijk.I, j, k);
        }

        public static CoordIjk SetIJK(this CoordIjk ijk, int i, int j, int k)
        {
            return new CoordIjk(i, j, k);
        }
        
        /// <summary>
        /// Find the center point in 2D cartesian coordinates of a hex.
        /// </summary>
        /// <param name="h">The ijk coordinates of the hex.</param>
        public static Vec2d ToHex2d(this CoordIjk h)
        {
            int i = h.I - h.K;
            int j = h.J - h.K;

            return new Vec2d(i - 0.5 * j, j * Constants.M_SQRT3_2);
        }

        /// <summary>
        /// Normalizes ijk coordinates by setting the components to the smallest possible
        /// values. Works in place.
        /// </summary>
        /// <param name="coord">The ijk coordinates to normalize.</param>
        public static CoordIjk Normalized(this CoordIjk coord)
        {
            (int i, int j, int k) = (coord.I, coord.J, coord.K);
            
            // remove any negative values
            if (i < 0) {
                j -= i;
                k -= i;
                i = 0;
            }

            if (j < 0) {
                i -= j;
                k -= j;
                j = 0;
            }

            if (k < 0) {
                i -= k;
                j -= k;
                k = 0;
            }

            // remove the min value if needed
            int min = new[]{i,j,k}.Min();
            
            return min <= 0
                       ? new CoordIjk(i, j, k)
                       : new CoordIjk(i - min, j - min, k - min);
        }

        /// <summary>
        /// Determines the H3 digit corresponding to a unit vector in ijk coordinates.
        /// </summary>
        /// <param name="ijk">The ijk coordinates; must be a unit vector.</param>
        /// <returns>The H3 digit (0-6) corresponding to the ijk unit vector, or <see cref="Direction.INVALID_DIGIT"/> INVALID_DIGIT on failure</returns>
        public static Direction ToDirection(this CoordIjk ijk)
        {
            var c = ijk.Normalized();
            var test = CoordIjk.UnitVectors.Where(pair => pair.Value == c).ToList();

            return test.Any()
                       ? test.First().Key
                       : Direction.INVALID_DIGIT;
        }

        /// <summary>
        /// Find the normalized ijk coordinates of the indexing parent of a cell in a
        /// counter-clockwise aperture 7 grid. Works in place.
        /// </summary>
        /// <param name="ijk">The ijk coordinates</param>
        public static CoordIjk UpAp7(this CoordIjk ijk)
        {
            // convert to CoordIJ
            int i = ijk.I - ijk.K;
            int j = ijk.J - ijk.K;

            var newI =(int) Math.Round((3 * i - j) / 7.0, MidpointRounding.AwayFromZero);
            var newJ = (int) Math.Round((i + 2 * j) / 7.0, MidpointRounding.AwayFromZero);

            return new CoordIjk(newI, newJ, 0).Normalized();
        }

        /// <summary>
        /// Find the normalized ijk coordinates of the indexing parent of a cell in a
        /// clockwise aperture 7 grid. Works in place.
        /// </summary>
        /// <param name="ijk">The ijk coordinates</param>
        public static CoordIjk UpAp7R(this CoordIjk ijk)
        {
            // convert to CoordIJ
            int i = ijk.I - ijk.K;
            int j = ijk.J - ijk.K;

            var newI = (int) Math.Round(((2 * i + j) / 7.0d), MidpointRounding.AwayFromZero);
            var newJ = (int)Math.Round(((3 * j - i) / 7.0d), MidpointRounding.AwayFromZero);

            return new CoordIjk(newI, newJ, 0).Normalized();
        }

        /// <summary>
        /// Find the normalized ijk coordinates of the hex centered on the indicated
        /// hex at the next finer aperture 7 counter-clockwise resolution. Works in
        /// place.
        /// </summary>
        /// <param name="ijk">The ijk coordinates</param>
        public static CoordIjk DownAp7(this CoordIjk ijk)
        {
            var iVec = new CoordIjk(3, 0, 1) * ijk.I;
            var jVec = new CoordIjk(1, 3, 0) * ijk.J;
            var kVec = new CoordIjk(0, 1, 3) * ijk.K;

            return (iVec + jVec + kVec).Normalized();
        }

        /// <summary>
        /// Find the normalized ijk coordinates of the hex centered on the indicated
        /// hex at the next finer aperture 7 clockwise resolution. Works in place.
        /// </summary>
        /// <param name="ijk">The ijk coordinates.</param>
        public static CoordIjk DownAp7R(this CoordIjk ijk)
        {
            var iVec = new CoordIjk(3, 1, 0) * ijk.I;
            var jVec = new CoordIjk(0, 3, 1) * ijk.J;
            var kVec = new CoordIjk(1, 0, 3) * ijk.K;

            return (iVec + jVec + kVec).Normalized();
        }

        /// <summary>
        /// Find the normalized ijk coordinates of the hex in the specified digit
        /// direction from the specified ijk coordinates. Works in place.
        /// </summary>
        /// <param name="ijk">The ijk coordinates.</param>
        /// <param name="digit">The digit direction from the original ijk coordinates.</param>
        public static CoordIjk Neighbor(this CoordIjk ijk, Direction digit)
        {
            if (digit <= Direction.CENTER_DIGIT || digit >= Direction.NUM_DIGITS)
            {
                return ijk;
            }

            return (ijk + CoordIjk.UnitVectors[digit]).Normalized();
        }

        /// <summary>
        /// Rotates ijk coordinates 60 degrees counter-clockwise. Works in place.
        /// </summary>
        /// <param name="ijk">The ijk coordinates.</param>
        public static CoordIjk Rotate60CounterClockwise(this CoordIjk ijk)
        {
            // unit vector rotations
            var iVec = new CoordIjk(1, 1, 0) * ijk.I;
            var jVec = new CoordIjk(0, 1, 1) * ijk.J;
            var kVec = new CoordIjk(1, 0, 1) * ijk.K;

            return (iVec + jVec + kVec).Normalized();
        }

        /// <summary>
        /// Rotates ijk coordinates 60 degrees clockwise. Works in place.
        /// </summary>
        /// <param name="ijk">The ijk coordinates.</param>
        public static CoordIjk Rotate60Clockwise(this CoordIjk ijk)
        {
            // unit vector rotations
            var iVec = new CoordIjk(1, 0, 1) * ijk.I;
            var jVec = new CoordIjk(1, 1, 0) * ijk.J;
            var kVec = new CoordIjk(0, 1, 1) * ijk.K;

            return (iVec + jVec + kVec).Normalized();
        }

                /// <summary>
        /// Find the normalized ijk coordinates of the hex centered on the indicated
        /// hex at the next finer aperture 3 counter-clockwise resolution. Works in
        /// place.
        /// </summary>
        /// <param name="ijk">The ijk coordinates.</param>
        public static CoordIjk DownAp3(this CoordIjk ijk)
        {
            // res r unit vectors in res r+1
            var iVec = new CoordIjk(2, 0, 1) * ijk.I;
            var jVec = new CoordIjk(1, 2, 0) * ijk.J;
            var kVec = new CoordIjk(0, 1, 2) * ijk.K;

            return (iVec + jVec + kVec).Normalized();
        }

        /// <summary>
        /// Find the normalized ijk coordinates of the hex centered on the indicated
        /// hex at the next finer aperture 3 clockwise resolution. Works in place.
        /// </summary>
        /// <param name="ijk">The ijk coordinates.</param>
        public static CoordIjk DownAp3R(this CoordIjk ijk)
        {
            // res r unit vectors in res r+1
            var iVec = new CoordIjk(2, 1, 0) * ijk.I;
            var jVec = new CoordIjk(0, 2, 1) * ijk.J;
            var kVec = new CoordIjk(1, 0, 2) * ijk.K;

            return (iVec + jVec + kVec).Normalized();
        }

        /// <summary>
        /// Finds the distance between the two coordinates. Returns result.
        /// </summary>
        /// <param name="start">The first set of ijk coordinates.</param>
        /// <param name="end">The second set of ijk coordinates.</param>
        public static int DistanceTo(this CoordIjk start, CoordIjk end)
        {
            var diff = (start - end).Normalized();
            var absDiff = new CoordIjk(Math.Abs(diff.I), Math.Abs(diff.J), Math.Abs(diff.K));
            return new[] {absDiff.I, absDiff.J, absDiff.K}.Max();
        }

        /// <summary>
        /// Transforms coordinates from the IJK+ coordinate system to the IJ coordinate system
        /// </summary>
        /// <param name="ijk">The input IJK+ coordinates</param>
        public static LocalIJ.CoordIJ ToIj(this CoordIjk ijk)
        {
            return new LocalIJ.CoordIJ(ijk.I - ijk.K, ijk.J - ijk.K);
        }

        /// <summary>
        /// Convert IJK coordinates to cube coordinates, in place
        /// </summary>
        /// <param name="ijk">Coordinate to convert</param>
        public static CoordIjk ToCube(this CoordIjk ijk)
        {
            (int i, int j, int k) = (ijk.I, ijk.J, ijk.K);
            i  =-i + k;
            j -= k;
            k = -i - j;

            return new CoordIjk(i, j, k);
        }

        /// <summary>
        /// Convert cube coordinates to IJK coordinates, in place
        /// </summary>
        /// <param name="ijk">Coordinate to convert</param>
        public static CoordIjk FromCube(this CoordIjk ijk)
        {
            return new CoordIjk(-ijk.I, ijk.J, 0).Normalized();
        }
    }
}
