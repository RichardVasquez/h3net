using System;

namespace H3Lib
{
    /// <summary>
    /// Header file for CoordIJK functions including conversion from lat/lon
    /// </summary>
    /// <remarks>
    /// References two Vec2d cartesian coordinate systems:
    ///
    /// 1. gnomonic: face-centered polyhedral gnomonic projection space with
    ///    traditional scaling and x-axes aligned with the face Class II
    ///    i-axes
    ///
    /// 2. hex2d: local face-centered coordinate system scaled a specific H3 grid
    ///    resolution unit length and with x-axes aligned with the local i-axes
    /// </remarks>
    public class CoordIjk:IEquatable<CoordIjk>
    {
        public int I;
        public int J;
        public int K;

        /// <summary>
        /// IJK hexagon coordinates
        /// </summary>
        public CoordIjk(int i, int j, int k)
        {
            I = i;
            J = j;
            K = k;
        }

        public CoordIjk()
        {
            I = 0;
            J = 0;
            K = 0;
        }

        /// <summary>
        /// CoordIJK unit vectors corresponding to the 7 H3 digits.
        /// </summary>
        private static readonly CoordIjk[] UNIT_VECS =
        {
            new CoordIjk{I=0, J=0, K=0},  // direction 0
            new CoordIjk{I=0, J=0, K=1},  // direction 1
            new CoordIjk{I=0, J=1, K=0},  // direction 2
            new CoordIjk{I=0, J=1, K=1},  // direction 3
            new CoordIjk{I=1, J=0, K=0},  // direction 4
            new CoordIjk{I=1, J=0, K=1},  // direction 5
            new CoordIjk{I=1, J=1, K=0}   // direction 6
        };

        /// <summary>
        /// Sets an IJK coordinate to the specified component values.
        /// </summary>
        /// <param name="ijk">The IJK coordinate to set.</param>
        /// <param name="i">The desired i component value.</param>
        /// <param name="j">The desired j component value.</param>
        /// <param name="k">The desired k component value.</param>
        public static void _setIJK(ref CoordIjk ijk, int i, int j, int k)
        {
            ijk.I = i;
            ijk.J = j;
            ijk.K = k;
        }

        /// <summary>
        /// Determine the containing hex in ijk+ coordinates for a 2D cartesian
        /// coordinate vector (from <a href="http://www.discreteglobalgrids.org/software/">DGGRID</a>).
        /// </summary>
        /// <param name="v">The 2D cartesian coordinate vector.</param>
        /// <param name="h">The ijk+ coordinates of the containing hex.</param>
        public static void _hex2dToCoordIJK(ref Vec2d v, ref CoordIjk h)
        {
            // quantize into the ij system and then normalize
            h.K = 0;

            double a1 = Math.Abs(v.X);
            double a2 = Math.Abs(v.Y);

            // first do a reverse conversion
            double x2 = a2 / Constants.M_SIN60;
            double x1 = a1 + x2 / 2.0;

            // check if we have the center of a hex
            var m1 = (int)x1;
            var m2 = (int)x2;

            // otherwise round correctly
            double r1 = x1 - m1;
            double r2 = x2 - m2;

            if (r1 < 0.5) {
                if (r1 < 1.0 / 3.0) {
                    if (r2 < (1.0 + r1) / 2.0) {
                        h.I = m1;
                        h.J = m2;
                    } else {
                        h.I = m1;
                        h.J = m2 + 1;
                    }
                } else {
                    if (r2 < (1.0 - r1)) {
                        h.J = m2;
                    } else {
                        h.J = m2 + 1;
                    }

                    if ((1.0 - r1) <= r2 && r2 < (2.0 * r1)) {
                        h.I = m1 + 1;
                    } else {
                        h.I = m1;
                    }
                }
            } else {
                if (r1 < 2.0 / 3.0) {
                    if (r2 < (1.0 - r1)) {
                        h.J = m2;
                    } else {
                        h.J = m2 + 1;
                    }

                    if ((2.0 * r1 - 1.0) < r2 && r2 < (1.0 - r1)) {
                        h.I = m1;
                    } else {
                        h.I = m1 + 1;
                    }
                } else {
                    if (r2 < (r1 / 2.0)) {
                        h.I = m1 + 1;
                        h.J = m2;
                    } else {
                        h.I = m1 + 1;
                        h.J = m2 + 1;
                    }
                }
            }

            // now fold across the axes if necessary
            if (v.X < 0.0) {
                if ((h.J % 2) == 0)  // even
                {
                    long axisI = (long)h.J / 2;
                    long diff = h.I - axisI;
                    h.I =(int)( h.I - 2.0 * diff);
                } else {
                    long axisI = (long)(h.J + 1) / 2;
                    long diff = h.I - axisI;
                    h.I -= (int)(2.0 * diff + 1);
                }
            }

            if (v.Y < 0.0) {
                h.I -= (2 * h.J + 1) / 2;
                h.J = -h.J;
            }

            _ijkNormalize(ref h);
        }

        /// <summary>
        /// Find the center point in 2D cartesian coordinates of a hex.
        /// </summary>
        /// <param name="h">The ijk coordinates of the hex.</param>
        /// <param name="v">The 2D cartesian coordinates of the hex center point.</param>
        public static void _ijkToHex2d(CoordIjk h, ref Vec2d v) {
            int i = h.I - h.K;
            int j = h.J - h.K;

            v.X = i - 0.5 * j;
            v.Y = j * Constants.M_SQRT3_2;
        }

        /// <summary>
        /// Returns whether or not two ijk coordinates contain exactly the same
        /// component values.
        /// </summary>
        /// <param name="c1">The first set of ijk coordinates.</param>
        /// <param name="c2">The second set of ijk coordinates.</param>
        /// <returns>1 if the two address match, 0 if they do not</returns>
        public static int _ijkMatches(CoordIjk c1, CoordIjk c2)
        {
            return c1.Equals(c2)
                       ? 1
                       : 0;
            //return (c1.I == c2.I && c1.J == c2.J && c1.K == c2.K) ? 1: 0;
        }
        
        /// <summary>
        /// Add two ijk coordinates
        /// </summary>
        /// <param name="h1">The first set of ijk coordinates.</param>
        /// <param name="h2">The second set of ijk coordinates.</param>
        /// <param name="sum">The sum of the two sets of ijk coordinates.</param>
        public static void _ijkAdd(CoordIjk h1, CoordIjk h2, ref CoordIjk sum)
        {
            sum.I = h1.I + h2.I;
            sum.J = h1.J + h2.J;
            sum.K = h1.K + h2.K;
        }

        public static CoordIjk _ijkAdd(CoordIjk h1, CoordIjk h2)
        {
            return h1 + h2;
        }

        /// <summary>
        /// Subtract two ijk coordinates
        /// </summary>
        /// <param name="h1">The first set of ijk coordinates</param>
        /// <param name="h2">The second set of ijk coordinates</param>
        /// <param name="diff">The difference of the two sets of ijk coordinates (h1 - h2)</param>
        public static void _ijkSub(ref  CoordIjk h1, ref  CoordIjk h2, ref CoordIjk diff) {
            diff.I = h1.I - h2.I;
            diff.J = h1.J - h2.J;
            diff.K = h1.K - h2.K;
        }

        public static CoordIjk _ijkSub(CoordIjk h1, CoordIjk h2)
        {
            return h1 - h2;
        }

        /// <summary>
        /// Uniformly scale ijk coordinates by a scalar. Works in place.
        /// </summary>
        /// <param name="c">The ijk coordinates to scale.</param>
        /// <param name="factor">The scaling factor.</param>
        public static void _ijkScale(ref CoordIjk c, int factor) {
            c.I *= factor;
            c.J *= factor;
            c.K *= factor;
        }

        public static CoordIjk _ijkScale(CoordIjk c, int factor)
        {
            return c * factor;
        }

        /// <summary>
        /// Normalizes ijk coordinates by setting the components to the smallest possible
        /// values. Works in place.
        /// </summary>
        /// <param name="c">The ijk coordinates to normalize.</param>
        /// <!-- Based off 3.1.1 -->
        public static void _ijkNormalize(ref CoordIjk c) {
            // remove any negative values
            if (c.I < 0) {
                c.J -= c.I;
                c.K -= c.I;
                c.I = 0;
            }

            if (c.J < 0) {
                c.I -= c.J;
                c.K -= c.J;
                c.J = 0;
            }

            if (c.K < 0) {
                c.I -= c.K;
                c.J -= c.K;
                c.K = 0;
            }

            // remove the min value if needed
            int min = c.I;
            if (c.J < min) {min = c.J;}
            if (c.K < min) {min = c.K;}

            if (min <= 0)
            {
                return;
            }
            c.I -= min;
            c.J -= min;
            c.K -= min;
        }

        /// <summary>
        /// Determines the H3 digit corresponding to a unit vector in ijk coordinates.
        /// </summary>
        /// <param name="ijk">The ijk coordinates; must be a unit vector.</param>
        /// <returns>The H3 digit (0-6) corresponding to the ijk unit vector, or <see cref="Direction.INVALID_DIGIT"/> INVALID_DIGIT on failure</returns>
        public static Direction _unitIjkToDigit(ref  CoordIjk ijk)
        {
            var c = new CoordIjk(ijk.I, ijk.J, ijk.K);
            _ijkNormalize(ref c);

            var digit = Direction.INVALID_DIGIT;
            for (var i = Direction.CENTER_DIGIT; i < Direction.NUM_DIGITS; i++)
            {
                if (_ijkMatches(c, UNIT_VECS[(int) i]) != 1)
                {
                    continue;
                }
                digit = i;
                break;
            }
            return digit;
        }

        /// <summary>
        /// Find the normalized ijk coordinates of the indexing parent of a cell in a
        /// counter-clockwise aperture 7 grid. Works in place.
        /// </summary>
        /// <param name="ijk">The ijk coordinates</param>
        public static void _upAp7(ref CoordIjk ijk) {
            // convert to CoordIJ
            int i = ijk.I - ijk.K;
            int j = ijk.J - ijk.K;

            ijk.I = (int) Math.Round((3 * i - j) / 7.0, MidpointRounding.AwayFromZero);
            ijk.J = (int) Math.Round((i + 2 * j) / 7.0, MidpointRounding.AwayFromZero);
            ijk.K = 0;
            _ijkNormalize(ref ijk);
        }

        /// <summary>
        /// Find the normalized ijk coordinates of the indexing parent of a cell in a
        /// clockwise aperture 7 grid. Works in place.
        /// </summary>
        /// <param name="ijk">The ijk coordinates</param>
        public static void _upAp7r(ref CoordIjk ijk) {
            // convert to CoordIJ
            int i = ijk.I - ijk.K;
            int j = ijk.J - ijk.K;

            ijk.I = (int) Math.Round(((2 * i + j) / 7.0d), MidpointRounding.AwayFromZero);
            ijk.J = (int)Math.Round(((3 * j - i) / 7.0d), MidpointRounding.AwayFromZero);
            ijk.K = 0;
            _ijkNormalize(ref ijk);
        }

        /// <summary>
        /// Find the normalized ijk coordinates of the hex centered on the indicated
        /// hex at the next finer aperture 7 counter-clockwise resolution. Works in
        /// place.
        /// </summary>
        /// <param name="ijk">The ijk coordinates</param>
        public static void _downAp7(ref CoordIjk ijk) {
            // res r unit vectors in res r+1
            var iVec = new CoordIjk{I=3, J=0, K=1};
            var jVec = new CoordIjk{I=1, J=3, K=0};
            var kVec = new CoordIjk{I=0, J=1, K=3};

            _ijkScale(ref iVec, ijk.I);
            _ijkScale(ref jVec, ijk.J);
            _ijkScale(ref kVec, ijk.K);

            _ijkAdd(iVec, jVec, ref ijk);
            _ijkAdd(ijk, kVec, ref ijk);

            _ijkNormalize(ref ijk);
        }

        /// <summary>
        /// Find the normalized ijk coordinates of the hex centered on the indicated
        /// hex at the next finer aperture 7 clockwise resolution. Works in place.
        /// </summary>
        /// <param name="ijk">The ijk coordinates.</param>
        public static void _downAp7r(ref CoordIjk ijk)
        {
            // res r unit vectors in res r+1
            var iVec = new CoordIjk{I=3, J=1, K=0};
            var jVec = new CoordIjk{I=0, J=3, K=1};
            var kVec = new CoordIjk{I=1, J=0, K=3};

            _ijkScale(ref iVec, ijk.I);
            _ijkScale(ref jVec, ijk.J);
            _ijkScale(ref kVec, ijk.K);

            _ijkAdd(iVec, jVec, ref ijk);
            _ijkAdd(ijk, kVec, ref ijk);

            _ijkNormalize(ref ijk);
        }

        /// <summary>
        /// Find the normalized ijk coordinates of the hex in the specified digit
        /// direction from the specified ijk coordinates. Works in place.
        /// </summary>
        /// <param name="ijk">The ijk coordinates.</param>
        /// <param name="digit">The digit direction from the original ijk coordinates.</param>
        public static void _neighbor(ref CoordIjk ijk, Direction digit)
        {
            if (digit <= Direction.CENTER_DIGIT || digit >= Direction.NUM_DIGITS)
            {
                return;
            }
            _ijkAdd(ijk, UNIT_VECS[(int)digit], ref ijk);
            _ijkNormalize(ref ijk);
        }

        /// <summary>
        /// Rotates ijk coordinates 60 degrees counter-clockwise. Works in place.
        /// </summary>
        /// <param name="ijk">The ijk coordinates.</param>
        public static void _ijkRotate60ccw(ref CoordIjk ijk)
        {
            // unit vector rotations
            var iVec = new CoordIjk(1, 1, 0);
            var jVec = new CoordIjk(0, 1, 1);
            var kVec = new CoordIjk(1, 0, 1);

            _ijkScale(ref iVec, ijk.I);
            _ijkScale(ref jVec, ijk.J);
            _ijkScale(ref kVec, ijk.K);

            _ijkAdd(iVec, jVec, ref ijk);
            _ijkAdd(ijk, kVec, ref ijk);

            _ijkNormalize(ref ijk);
        }

        /// <summary>
        /// Rotates ijk coordinates 60 degrees clockwise. Works in place.
        /// </summary>
        /// <param name="ijk">The ijk coordinates.</param>
        public static void _ijkRotate60cw(ref CoordIjk ijk) {
            // unit vector rotations
            var iVec = new CoordIjk(1, 0, 1);
            var jVec = new CoordIjk(1, 1, 0);
            var kVec = new CoordIjk(0, 1, 1);

            _ijkScale(ref iVec, ijk.I);
            _ijkScale(ref jVec, ijk.J);
            _ijkScale(ref kVec, ijk.K);

            _ijkAdd(iVec, jVec, ref ijk);
            _ijkAdd(ijk, kVec, ref ijk);

            _ijkNormalize(ref ijk);
        }

        /// <summary>
        /// Rotates indexing digit 60 degrees counter-clockwise. Returns result.
        /// </summary>
        /// <param name="digit">Indexing digit (between 1 and 6 inclusive)</param>
        public static Direction _rotate60ccw(Direction digit)
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
        public static Direction _rotate60cw(Direction digit)
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

        /// <summary>
        /// Find the normalized ijk coordinates of the hex centered on the indicated
        /// hex at the next finer aperture 3 counter-clockwise resolution. Works in
        /// place.
        /// </summary>
        /// <param name="ijk">The ijk coordinates.</param>
        public static void _downAp3(ref CoordIjk ijk)
        {
            // res r unit vectors in res r+1
            var iVec = new CoordIjk( 2, 0, 1);
            var jVec = new CoordIjk( 1, 2, 0);
            var kVec = new CoordIjk(0, 1, 2);

            _ijkScale(ref iVec, ijk.I);
            _ijkScale(ref jVec, ijk.J);
            _ijkScale(ref kVec, ijk.K);

            _ijkAdd(iVec, jVec, ref ijk);
            _ijkAdd(ijk, kVec, ref ijk);

            _ijkNormalize(ref ijk);
        }

        /// <summary>
        /// Find the normalized ijk coordinates of the hex centered on the indicated
        /// hex at the next finer aperture 3 clockwise resolution. Works in place.
        /// </summary>
        /// <param name="ijk">The ijk coordinates.</param>
        public static void _downAp3r(ref CoordIjk ijk)
        {
            // res r unit vectors in res r+1
            var iVec =  new CoordIjk(2, 1, 0);
            var jVec =  new CoordIjk(0, 2, 1);
            var kVec =  new CoordIjk(1, 0, 2);

            _ijkScale(ref iVec, ijk.I);
            _ijkScale(ref jVec, ijk.J);
            _ijkScale(ref kVec, ijk.K);

            _ijkAdd(iVec, jVec, ref ijk);
            _ijkAdd(ijk, kVec, ref ijk);

            _ijkNormalize(ref ijk);
        }

        /// <summary>
        /// Finds the distance between the two coordinates. Returns result.
        /// </summary>
        /// <param name="c1">The first set of ijk coordinates.</param>
        /// <param name="c2">The second set of ijk coordinates.</param>
        public static int ijkDistance( CoordIjk c1,  CoordIjk c2)
        {
            var diff = new CoordIjk();
            _ijkSub(ref c1, ref c2, ref diff);
            _ijkNormalize(ref diff);
            var absDiff = new CoordIjk(Math.Abs(diff.I), Math.Abs(diff.J), Math.Abs(diff.K));

            return Math.Max(absDiff.I, Math.Max(absDiff.J, absDiff.K));
        }

        /// <summary>
        /// Transforms coordinates from the IJK+ coordinate system to the IJ coordinate system
        /// </summary>
        /// <param name="ijk">The input IJK+ coordinates</param>
        /// <param name="ij">The output IJ coordinates</param>
        public static void ijkToIj(CoordIjk ijk, ref LocalIJ.CoordIJ ij)
        {
            ij.i = ijk.I - ijk.K;
            ij.j = ijk.J - ijk.K;
        }

        /// <summary>
        /// Transforms coordinates from the IJ coordinate system to the IJK+ coordinate system
        /// </summary>
        /// <param name="ij">The input IJ coordinates</param>
        /// <param name="ijk">The output IJK+ coordinates</param>
        public static void ijToIjk(LocalIJ.CoordIJ ij, ref CoordIjk ijk) {
            ijk.I = ij.i;
            ijk.J = ij.j;
            ijk.K = 0;

            _ijkNormalize(ref ijk);
        }

        /// <summary>
        /// Convert IJK coordinates to cube coordinates, in place
        /// </summary>
        /// <param name="ijk">Coordinate to convert</param>
        public static void ijkToCube(ref CoordIjk ijk)
        {
            ijk.I = -ijk.I + ijk.K;
            ijk.J -= ijk.K;
            ijk.K = -ijk.I - ijk.J;
        }

        /// <summary>
        /// Convert cube coordinates to IJK coordinates, in place
        /// </summary>
        /// <param name="ijk">Coordinate to convert</param>
        public static void cubeToIjk(ref CoordIjk ijk)
        {
            ijk.I = -ijk.I;
            ijk.K = 0;
            _ijkNormalize(ref ijk);
        }

        public bool Equals(CoordIjk other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }
            return I == other.I && J == other.J && K == other.K;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            return obj.GetType() == typeof(CoordIjk) && Equals((CoordIjk) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(I, J, K);
        }

        public static bool operator ==(CoordIjk left, CoordIjk right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(CoordIjk left, CoordIjk right)
        {
            return !Equals(left, right);
        }
        
        public static CoordIjk operator+(CoordIjk c1,CoordIjk c2)
        {
            return new CoordIjk(c1.I + c2.I, c1.J + c2.J, c1.K + c2.K);
        }

        public static CoordIjk operator-(CoordIjk c1,CoordIjk c2)
        {
            return new CoordIjk(c1.I - c2.I, c1.J - c2.J, c1.K - c2.K);
        }

        public static CoordIjk operator *(CoordIjk c, int scalar)
        {
            return new CoordIjk(c.I * scalar, c.J * scalar, c.K * scalar);
        }
    }
}
