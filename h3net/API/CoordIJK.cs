/*
 * Copyright 2018, Richard Vasquez
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *         http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 *
 * Original version written in C, Copyright 2016-2017 Uber Technologies, Inc.
 * C version licensed under the Apache License, Version 2.0 (the "License");
 * C Source code available at: https://github.com/uber/h3
 *
 */
using System;

namespace h3net.API
{
    /// <summary>
    /// Hex IJK coordinate systems functions including conversions to/from
    /// lat/lon.
    /// </summary>
    /// <!-- Based off 3.1.1 -->
    public class CoordIJK
    {
        public int i;
        public int j;
        public int k;

        /// <summary>
        /// IJK hexagon coordinates
        /// </summary>
        /// <!-- Based off 3.1.1 -->
        public CoordIJK(int _i, int _j, int _k)
        {
            i = _i;
            j = _j;
            k = _k;
        }

        public CoordIJK()
        {
        }

        /// <summary>
        /// CoordIJK unit vectors corresponding to the 7 H3 digits.
        /// </summary>
        /// <!-- Based off 3.1.1 -->
        private static readonly CoordIJK[] UNIT_VECS =
        {
            new CoordIJK{i=0, j=0, k=0},  // direction 0
            new CoordIJK{i=0, j=0, k=1},  // direction 1
            new CoordIJK{i=0, j=1, k=0},  // direction 2
            new CoordIJK{i=0, j=1, k=1},  // direction 3
            new CoordIJK{i=1, j=0, k=0},  // direction 4
            new CoordIJK{i=1, j=0, k=1},  // direction 5
            new CoordIJK{i=1, j=1, k=0}   // direction 6
        };

        /// <summary>
        /// Sets an IJK coordinate to the specified component values.
        /// </summary>
        /// <param name="ijk">The IJK coordinate to set.</param>
        /// <param name="i">The desired i component value.</param>
        /// <param name="j">The desired j component value.</param>
        /// <param name="k">The desired k component value.</param>
        /// <!-- Based off 3.1.1 -->
        public static void _setIJK(ref CoordIJK ijk, int i, int j, int k)
        {
            ijk.i = i;
            ijk.j = j;
            ijk.k = k;
        }

        /// <summary>
        /// Determine the containing hex in ijk+ coordinates for a 2D cartesian
        /// coordinate vector (from <a href="http://www.discreteglobalgrids.org/software/">DGGRID</a>).
        /// </summary>
        /// <param name="v">The 2D cartesian coordinate vector.</param>
        /// <param name="h">The ijk+ coordinates of the containing hex.</param>
        /// <!-- Based off 3.1.1 -->
        public static void _hex2dToCoordIJK(ref Vec2d v, ref CoordIJK h)
        {
            double a1, a2;
            double x1, x2;
            int m1, m2;
            double r1, r2;

            // quantize into the ij system and then normalize
            h.k = 0;

            a1 = Math.Abs(v.x);
            a2 = Math.Abs(v.y);

            // first do a reverse conversion
            x2 = a2 / Constants.M_SIN60;
            x1 = a1 + x2 / 2.0;

            // check if we have the center of a hex
            m1 = (int)x1;
            m2 = (int)x2;

            // otherwise round correctly
            r1 = x1 - m1;
            r2 = x2 - m2;

            if (r1 < 0.5) {
                if (r1 < 1.0 / 3.0) {
                    if (r2 < (1.0 + r1) / 2.0) {
                        h.i = m1;
                        h.j = m2;
                    } else {
                        h.i = m1;
                        h.j = m2 + 1;
                    }
                } else {
                    if (r2 < (1.0 - r1)) {
                        h.j = m2;
                    } else {
                        h.j = m2 + 1;
                    }

                    if ((1.0 - r1) <= r2 && r2 < (2.0 * r1)) {
                        h.i = m1 + 1;
                    } else {
                        h.i = m1;
                    }
                }
            } else {
                if (r1 < 2.0 / 3.0) {
                    if (r2 < (1.0 - r1)) {
                        h.j = m2;
                    } else {
                        h.j = m2 + 1;
                    }

                    if ((2.0 * r1 - 1.0) < r2 && r2 < (1.0 - r1)) {
                        h.i = m1;
                    } else {
                        h.i = m1 + 1;
                    }
                } else {
                    if (r2 < (r1 / 2.0)) {
                        h.i = m1 + 1;
                        h.j = m2;
                    } else {
                        h.i = m1 + 1;
                        h.j = m2 + 1;
                    }
                }
            }

            // now fold across the axes if necessary
            if (v.x < 0.0) {
                if ((h.j % 2) == 0)  // even
                {
                    ulong axisi = (ulong)h.j / 2;
                    ulong diff = (ulong)h.i - axisi;
                    h.i =(int)( h.i - 2.0 * diff);
                } else {
                    ulong axisi = (ulong)(h.j + 1) / 2;
                    ulong diff = (ulong)h.i - axisi;
                    h.i = h.i - (int)(2.0 * diff + 1);
                }
            }

            if (v.y < 0.0) {
                h.i = h.i - (2 * h.j + 1) / 2;
                h.j = -1 * h.j;
            }

            _ijkNormalize(ref h);
        }

        /// <summary>
        /// Find the center point in 2D cartesian coordinates of a hex.
        /// </summary>
        /// <param name="h">The ijk coordinates of the hex.</param>
        /// <param name="v">The 2D cartesian coordinates of the hex center point.</param>
        /// <!-- Based off 3.1.1 -->
        public static void _ijkToHex2d(CoordIJK h, ref Vec2d v) {
            int i = h.i - h.k;
            int j = h.j - h.k;

            v.x = i - 0.5 * j;
            v.y = j * Constants.M_SQRT3_2;
        }

        /// <summary>
        /// Returns whether or not two ijk coordinates contain exactly the same
        /// component values.
        /// </summary>
        /// <param name="c1">The first set of ijk coordinates.</param>
        /// <param name="c2">The second set of ijk coordinates.</param>
        /// <returns>1 if the two address match, 0 if they do not</returns>
        /// <!-- Based off 3.1.1 -->
        public static int _ijkMatches(CoordIJK c1, CoordIJK c2) {
            return (c1.i == c2.i && c1.j == c2.j && c1.k == c2.k) ? 1: 0;
        }

        /// <summary>
        /// Add two ijk coordinates
        /// </summary>
        /// <param name="h1">The first set of ijk coordinates.</param>
        /// <param name="h2">The second set of ijk coordinates.</param>
        /// <param name="sum">The sum of the two sets of ijk coordinates.</param>
        /// <!-- Based off 3.1.1 -->
        public static void _ijkAdd(CoordIJK h1, CoordIJK h2, ref CoordIJK sum)
        {
            sum.i = h1.i + h2.i;
            sum.j = h1.j + h2.j;
            sum.k = h1.k + h2.k;
        }

        /// <summary>
        /// Subtract two ijk coordinates
        /// </summary>
        /// <param name="h1">The first set of ijk coordinates</param>
        /// <param name="h2">The second set of ijk coordinates</param>
        /// <param name="diff">The difference of the two sets of ijk coordinates (h1 - h2)</param>
        /// <!-- Based off 3.1.1 -->
        public static void _ijkSub(ref  CoordIJK h1, ref  CoordIJK h2, ref CoordIJK diff) {
            diff.i = h1.i - h2.i;
            diff.j = h1.j - h2.j;
            diff.k = h1.k - h2.k;
        }

        /// <summary>
        /// Uniformly scale ijk coordinates by a scalar. Works in place.
        /// </summary>
        /// <param name="c">The ijk coordinates to scale.</param>
        /// <param name="factor">The scaling factor.</param>
        /// <!-- Based off 3.1.1 -->
        public static void _ijkScale(ref CoordIJK c, int factor) {
            c.i *= factor;
            c.j *= factor;
            c.k *= factor;
        }

        /// <summary>
        /// Normalizes ijk coordinates by setting the components to the smallest possible
        /// values. Works in place.
        /// </summary>
        /// <param name="c">The ijk coordinates to normalize.</param>
        /// <!-- Based off 3.1.1 -->
        public static void _ijkNormalize(ref CoordIJK c) {
            // remove any negative values
            if (c.i < 0) {
                c.j -= c.i;
                c.k -= c.i;
                c.i = 0;
            }

            if (c.j < 0) {
                c.i -= c.j;
                c.k -= c.j;
                c.j = 0;
            }

            if (c.k < 0) {
                c.i -= c.k;
                c.j -= c.k;
                c.k = 0;
            }

            // remove the min value if needed
            int min = c.i;
            if (c.j < min) {min = c.j;}
            if (c.k < min) {min = c.k;}
            if (min > 0) {
                c.i -= min;
                c.j -= min;
                c.k -= min;
            }
        }

        /// <summary>
        /// Determines the H3 digit corresponding to a unit vector in ijk coordinates.
        /// </summary>
        /// <param name="ijk">The ijk coordinates; must be a unit vector.</param>
        /// <returns>The H3 digit (0-6) corresponding to the ijk unit vector, or <see cref="Direction.INVALID_DIGIT"/> INVALID_DIGIT on failure</returns>
        /// <!-- Based off 3.1.1 -->
        public static Direction _unitIjkToDigit(ref  CoordIJK ijk)
        {
            CoordIJK c = new CoordIJK(ijk.i, ijk.j, ijk.k);
            _ijkNormalize(ref c);

            Direction digit = Direction.INVALID_DIGIT;
            for (Direction i = Direction.CENTER_DIGIT; i < Direction.NUM_DIGITS; i++) {
                if (_ijkMatches(c,  UNIT_VECS[(int)i]) == 1) {
                    digit = i;
                    break;
                }
            }
            return digit;
        }


        /// <summary>
        /// Find the normalized ijk coordinates of the indexing parent of a cell in a
        /// counter-clockwise aperture 7 grid. Works in place.
        /// </summary>
        /// <param name="ijk">The ijk coordinates</param>
        /// <!-- Based off 3.1.1 -->
        public static void _upAp7(ref CoordIJK ijk) {
            // convert to CoordIJ
            int i = ijk.i - ijk.k;
            int j = ijk.j - ijk.k;

            ijk.i = (int) Math.Round((3 * i - j) / 7.0, MidpointRounding.AwayFromZero);
            ijk.j = (int) Math.Round((i + 2 * j) / 7.0, MidpointRounding.AwayFromZero);
            ijk.k = 0;
            _ijkNormalize(ref ijk);
        }

        /// <summary>
        /// Find the normalized ijk coordinates of the indexing parent of a cell in a
        /// clockwise aperture 7 grid. Works in place.
        /// </summary>
        /// <param name="ijk">The ijk coordinates</param>
        /// <!-- Based off 3.1.1 -->
        public static void _upAp7r(ref CoordIJK ijk) {
            // convert to CoordIJ
            int i = ijk.i - ijk.k;
            int j = ijk.j - ijk.k;

            ijk.i = (int) Math.Round(((2 * i + j) / 7.0d), MidpointRounding.AwayFromZero);
            ijk.j = (int)Math.Round(((3 * j - i) / 7.0d), MidpointRounding.AwayFromZero);
            ijk.k = 0;
            _ijkNormalize(ref ijk);
        }

        /// <summary>
        /// Find the normalized ijk coordinates of the hex centered on the indicated
        /// hex at the next finer aperture 7 counter-clockwise resolution. Works in
        /// place.
        /// </summary>
        /// <param name="ijk">The ijk coordinates</param>
        /// <!-- Based off 3.1.1 -->
        public static void _downAp7(ref CoordIJK ijk) {
            // res r unit vectors in res r+1
            CoordIJK iVec = new CoordIJK{i=3, j=0, k=1};
            CoordIJK jVec = new CoordIJK{i=1, j=3, k=0};
            CoordIJK kVec = new CoordIJK{i=0, j=1, k=3};

            _ijkScale(ref iVec, ijk.i);
            _ijkScale(ref jVec, ijk.j);
            _ijkScale(ref kVec, ijk.k);

            _ijkAdd(iVec, jVec, ref ijk);
            _ijkAdd(ijk, kVec, ref ijk);

            _ijkNormalize(ref ijk);
        }

        /// <summary>
        /// Find the normalized ijk coordinates of the hex centered on the indicated
        /// hex at the next finer aperture 7 clockwise resolution. Works in place.
        /// </summary>
        /// <param name="ijk">The ijk coordinates.</param>
        /// <!-- Based off 3.1.1 -->
        public static void _downAp7r(ref CoordIJK ijk)
        {
            // res r unit vectors in res r+1
            CoordIJK iVec = new CoordIJK{i=3, j=1, k=0};
            CoordIJK jVec = new CoordIJK{i=0, j=3, k=1};
            CoordIJK kVec = new CoordIJK{i=1, j=0, k=3};

            _ijkScale(ref iVec, ijk.i);
            _ijkScale(ref jVec, ijk.j);
            _ijkScale(ref kVec, ijk.k);

            _ijkAdd( iVec, jVec, ref ijk);
            _ijkAdd(ijk, kVec, ref ijk);

            _ijkNormalize(ref ijk);
        }

        /// <summary>
        /// Find the normalized ijk coordinates of the hex in the specified digit
        /// direction from the specified ijk coordinates. Works in place.
        /// </summary>
        /// <param name="ijk">The ijk coordinates.</param>
        /// <param name="digit">The digit direction from the original ijk coordinates.</param>
        /// <!-- Based off 3.1.1 -->
        public static void _neighbor(ref CoordIJK ijk, Direction digit)
        {
            if (digit > Direction.CENTER_DIGIT && digit < Direction.NUM_DIGITS)
            {
                _ijkAdd(ijk, UNIT_VECS[(int)digit], ref ijk);
                _ijkNormalize(ref ijk);
            }
        }

        /// <summary>
        /// Rotates ijk coordinates 60 degrees counter-clockwise. Works in place.
        /// </summary>
        /// <param name="ijk">The ijk coordinates.</param>
        /// <!-- Based off 3.1.1 -->
        public static void _ijkRotate60ccw(ref CoordIJK ijk) {
            // unit vector rotations
            CoordIJK iVec = new CoordIJK(1, 1, 0);
            CoordIJK jVec = new CoordIJK(0, 1, 1);
            CoordIJK kVec = new CoordIJK(1, 0, 1);

            _ijkScale(ref iVec, ijk.i);
            _ijkScale(ref jVec, ijk.j);
            _ijkScale(ref kVec, ijk.k);

            _ijkAdd(iVec, jVec, ref ijk);
            _ijkAdd(ijk, kVec, ref ijk);

            _ijkNormalize(ref ijk);
        }

        /// <summary>
        /// Rotates ijk coordinates 60 degrees clockwise. Works in place.
        /// </summary>
        /// <param name="ijk">The ijk coordinates.</param>
        /// <!-- Based off 3.1.1 -->
        public static void _ijkRotate60cw(ref CoordIJK ijk) {
            // unit vector rotations
            CoordIJK iVec = new CoordIJK(1, 0, 1);
            CoordIJK jVec = new CoordIJK(1, 1, 0);
            CoordIJK kVec = new CoordIJK(0, 1, 1);

            _ijkScale(ref iVec, ijk.i);
            _ijkScale(ref jVec, ijk.j);
            _ijkScale(ref kVec, ijk.k);

            _ijkAdd(iVec, jVec, ref ijk);
            _ijkAdd(ijk, kVec, ref ijk);

            _ijkNormalize(ref ijk);
        }

        /// <summary>
        /// Rotates indexing digit 60 degrees counter-clockwise. Returns result.
        /// </summary>
        /// <param name="digit">Indexing digit (between 1 and 6 inclusive)</param>
        /// <!-- Based off 3.1.1 -->
        public static Direction _rotate60ccw(Direction digit) {
            switch (digit) {
                case Direction.K_AXES_DIGIT:
                    return Direction.IK_AXES_DIGIT;
                case Direction.IK_AXES_DIGIT:
                    return Direction.I_AXES_DIGIT;
                case Direction.I_AXES_DIGIT:
                    return Direction.IJ_AXES_DIGIT;
                case Direction.IJ_AXES_DIGIT:
                    return Direction.J_AXES_DIGIT;
                case Direction.J_AXES_DIGIT:
                    return Direction.JK_AXES_DIGIT;
                case Direction.JK_AXES_DIGIT:
                    return Direction.K_AXES_DIGIT;
                default:
                    return digit;
            }
        }

        /// <summary>
        /// Rotates indexing digit 60 degrees clockwise. Returns result.
        /// </summary>
        /// <param name="digit">Indexing digit (between 1 and 6 inclusive)</param>
        /// <!-- Based off 3.1.1 -->
        public static Direction _rotate60cw(Direction digit) {
            switch (digit) {
                case Direction.K_AXES_DIGIT:
                    return Direction.JK_AXES_DIGIT;
                case Direction.JK_AXES_DIGIT:
                    return Direction.J_AXES_DIGIT;
                case Direction.J_AXES_DIGIT:
                    return Direction.IJ_AXES_DIGIT;
                case Direction.IJ_AXES_DIGIT:
                    return Direction.I_AXES_DIGIT;
                case Direction.I_AXES_DIGIT:
                    return Direction.IK_AXES_DIGIT;
                case Direction.IK_AXES_DIGIT:
                    return Direction.K_AXES_DIGIT;
                default:
                    return digit;
            }
        }

        /// <summary>
        /// Find the normalized ijk coordinates of the hex centered on the indicated
        /// hex at the next finer aperture 3 counter-clockwise resolution. Works in
        /// place.
        /// </summary>
        /// <param name="ijk">The ijk coordinates.</param>
        /// <!-- Based off 3.1.1 -->
        public static void _downAp3(ref CoordIJK ijk)
        {
            // res r unit vectors in res r+1
            CoordIJK iVec = new CoordIJK( 2, 0, 1);
            CoordIJK jVec = new CoordIJK( 1, 2, 0);
            CoordIJK kVec = new CoordIJK(0, 1, 2);

            _ijkScale(ref iVec, ijk.i);
            _ijkScale(ref jVec, ijk.j);
            _ijkScale(ref kVec, ijk.k);

            _ijkAdd(iVec, jVec, ref ijk);
            _ijkAdd(ijk, kVec, ref ijk);

            _ijkNormalize(ref ijk);
        }

        /// <summary>
        /// Find the normalized ijk coordinates of the hex centered on the indicated
        /// hex at the next finer aperture 3 clockwise resolution. Works in place.
        /// </summary>
        /// <param name="ijk">The ijk coordinates.</param>
        /// <!-- Based off 3.1.1 -->
        public static void _downAp3r(ref CoordIJK ijk)
        {
            // res r unit vectors in res r+1
            CoordIJK iVec =  new CoordIJK(2, 1, 0);
            CoordIJK jVec =  new CoordIJK(0, 2, 1);
            CoordIJK kVec =  new CoordIJK(1, 0, 2);

            _ijkScale(ref iVec, ijk.i);
            _ijkScale(ref jVec, ijk.j);
            _ijkScale(ref kVec, ijk.k);

            _ijkAdd(iVec, jVec, ref ijk);
            _ijkAdd(ijk, kVec, ref ijk);

            _ijkNormalize(ref ijk);
        }

        /// <summary>
        /// Finds the distance between the two coordinates. Returns result.
        /// </summary>
        /// <param name="c1">The first set of ijk coordinates.</param>
        /// <param name="c2">The second set of ijk coordinates.</param>
        /// <!-- Based off 3.1.1 -->
        public static int ijkDistance( CoordIJK c1,  CoordIJK c2) {
            CoordIJK diff = new CoordIJK();
            _ijkSub(ref c1, ref c2, ref diff);
            _ijkNormalize(ref diff);
            CoordIJK absDiff = new CoordIJK(Math.Abs(diff.i), Math.Abs(diff.j), Math.Abs(diff.k));

            return Math.Max(absDiff.i, Math.Max(absDiff.j, absDiff.k));
        }
    }
}