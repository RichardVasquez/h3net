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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using h3net.API;

namespace h3net.API
{
    /// <summary>
    /// Local IJ coordinate space functions
    ///
    /// These functions try to provide a useful
    /// coordinate space in the vicinity of
    /// an origin index.
    /// </summary>
    /// <!-- Based off 3.2.0 -->
    public class LocalIJ
    {
        /// <summary>
        /// IJ Hexagon coordinates.
        ///
        /// Each axis is spaced 120 degrees apart
        /// </summary>
        public class CoordIJ
        {
            public int i;
            public int j;
        }
        /// <summary>
        /// Origin leading digit -&gt; index leading digit -&gt; rotations 60 cw
        /// Either being 1 (K axis) is invalid.
        /// No good default at 0.
        /// </summary>
        /// <!-- Based off 3.2.0 -->
        internal static readonly int[,] PENTAGON_ROTATIONS =
        {
            { 0, -1,  0,  0,  0,  0,  0}, // 0
            {-1, -1, -1, -1, -1, -1, -1}, // 1
            { 0, -1,  0,  0,  0,  1,  0}, // 2
            { 0, -1,  0,  0,  1,  1,  0}, // 3
            { 0, -1,  0,  5,  0,  0,  0}, // 4
            { 0, -1,  5,  5,  0,  0,  0}, // 5
            { 0, -1,  0,  0,  0,  0,  0}  // 6
        };

        /// <summary>
        /// Reverse base cell direction -&gt; leading index digit -&gt; rotations 60 ccw.
        /// For reversing the rotation introduced in PENTAGON_ROTATIONS when the index is
        /// on a pentagon and the origin is not.
        /// </summary>
        /// <!-- Based off 3.2.0 -->
        internal  static readonly int[,] PENTAGON_ROTATIONS_REVERSE =
        {
            {0, 0, 0, 0, 0, 0, 0}, // 0
            {-1, -1, -1, -1, -1, -1, -1}, // 1
            {0, 1, 0, 0, 0, 0, 0}, // 2
            {0, 1, 0, 0, 0, 1, 0}, // 3
            {0, 5, 0, 0, 0, 0, 0}, // 4
            {0, 5, 0, 5, 0, 0, 0}, // 5
            {0, 0, 0, 0, 0, 0, 0}  // 6
        };

        /// <summary>
        /// Reverse base cell direction -&gt; leading index digit -&gt; rotations 60 ccw.
        /// For reversing the rotation introduced in PENTAGON_ROTATIONS when the index is
        /// on a pentagon and the origin is not.
        /// <!-- Based off 3.2.0 -->
        internal static readonly int[,] PENTAGON_ROTATIONS_REVERSE_NONPOLAR =
        {
            {0, 0, 0, 0, 0, 0, 0},         // 0
            {-1, -1, -1, -1, -1, -1, -1},  // 1
            {0, 1, 0, 0, 0, 0, 0},         // 2
            {0, 1, 0, 0, 0, 1, 0},         // 3
            {0, 5, 0, 0, 0, 0, 0},         // 4
            {0, 1, 0, 5, 1, 1, 0},         // 5
            {0, 0, 0, 0, 0, 0, 0},         // 6
        };

        /// <summary>
        /// Reverse base cell direction -&gt; leading index digit -&gt; rotations 60 ccw.
        /// For reversing the rotation introduced in PENTAGON_ROTATIONS when the index is
        /// on a polar pentagon and the origin is not.
        /// </summary>
        /// <!-- Based off 3.2.0 -->
        internal static readonly int[,] PENTAGON_ROTATIONS_REVERSE_POLAR =
        {
            {0, 0, 0, 0, 0, 0, 0},         // 0
            {-1, -1, -1, -1, -1, -1, -1},  // 1
            {0, 1, 1, 1, 1, 1, 1},         // 2
            {0, 1, 0, 0, 0, 1, 0},         // 3
            {0, 1, 0, 0, 1, 1, 1},         // 4
            {0, 1, 0, 5, 1, 1, 0},         // 5
            {0, 1, 1, 0, 1, 1, 1}          // 6
        };


        //  Simply prohibit many pentagon distortion cases rather than handling them.
        internal static readonly bool[,] FAILED_DIRECTIONS_II =
        {
            {false, false, false, false, false, false, false}, // 0
            {false, false, false, false, false, false, false}, // 1
            {false, false, false, false, true, false, false}, // 2
            {false, false, false, false, false, false, true}, // 3
            {false, false, false, true, false, false, false}, // 4
            {false, false, true, false, false, false, false}, // 5
            {false, false, false, false, false, true, false}  // 6
        };

        internal static readonly bool[,] FAILED_DIRECTIONS_III =
        {
            {false, false, false, false, false, false, false}, // 0
            {false, false, false, false, false, false, false}, // 1
            {false, false, false, false, false, true, false}, // 2
            {false, false, false, false, true, false, false}, // 3
            {false, false, true, false, false, false, false}, // 4
            {false, false, false, false, false, false, true}, // 5
            {false, false, false, true, false, false, false} // 6
        };

        /// <summary>
        /// Produces ijk+ coordinates for an index anchored by an origin.
        ///
        /// The coordinate space used by this function may have deleted
        /// regions or warping due to pentagonal distortion.
        ///
        /// Coordinates are only comparable if they come from the same
        /// origin index.
        ///
        /// Failure may occur if the index is too far away from the origin
        /// or if the index is on the other side of a pentagon.
        /// </summary>
        /// <param name="origin">An anchoring index for the ijk+ coordinate system</param>
        /// <param name="h3">Index to find the coordinates of</param>
        /// <param name="out_coord">ijk+ coordinates of the index will be placed here on success</param>
        /// <returns>0 on success, or another value on failure.</returns>
        /// <!-- Based off 3.2.0 -->
        static int h3ToLocalIjk(H3Index origin, H3Index h3, ref CoordIJK out_coord)
        {
            int res = H3Index.H3_GET_RESOLUTION(origin);

            if (res != H3Index.H3_GET_RESOLUTION(h3))
            {
                return 1;
            }

            int originBaseCell = H3Index.H3_GET_BASE_CELL(origin);
            int baseCell = H3Index.H3_GET_BASE_CELL(h3);

            // Direction from origin base cell to index base cell
            Direction dir = 0;
            Direction revDir = 0;
            if (originBaseCell != baseCell)
            {
                dir = BaseCells._getBaseCellDirection(originBaseCell, baseCell);
                if (dir == Direction.INVALID_DIGIT)
                {
                    // Base cells are not neighbors, can't unfold.
                    return 2;
                }

                revDir = BaseCells._getBaseCellDirection(baseCell, originBaseCell);
                if (revDir == Direction.INVALID_DIGIT)
                {
                    throw new Exception("assert(revDir != INVALID_DIGIT)");
                }
            }

            int originOnPent = (BaseCells._isBaseCellPentagon(originBaseCell)
                                    ? 1
                                    : 0);
            int indexOnPent = (BaseCells._isBaseCellPentagon(baseCell)
                                   ? 1
                                   : 0);

            FaceIJK indexFijk = new FaceIJK();
            if (dir != Direction.CENTER_DIGIT)
            {
                // Rotate index into the orientation of the origin base cell.
                // cw because we are undoing the rotation into that base cell.
                int baseCellRotations = BaseCells.baseCellNeighbor60CCWRots[originBaseCell, (int) dir];
                if (indexOnPent != 0)
                {
                    for (int i = 0; i < baseCellRotations; i++)
                    {
                        h3 = H3Index._h3RotatePent60cw(h3);

                        revDir = CoordIJK._rotate60cw(revDir);
                        if (revDir == Direction.K_AXES_DIGIT)
                        {
                            revDir = CoordIJK._rotate60cw(revDir);
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < baseCellRotations; i++)
                    {
                        h3 = H3Index._h3Rotate60cw(ref h3);

                        revDir = CoordIJK._rotate60cw(revDir);
                    }
                }
            }

            // Face is unused. This produces coordinates in base cell coordinate space.
            H3Index._h3ToFaceIjkWithInitializedFijk(h3, ref indexFijk);

            if (dir != Direction.CENTER_DIGIT)
            {
                if (baseCell == originBaseCell)
                {
                    throw new Exception("assert(baseCell != originBaseCell);");
                }

                if ((originOnPent != 0) && (indexOnPent != 0))
                {
                    throw new Exception("assert(!(originOnPent && indexOnPent));");
                }

                int pentagonRotations = 0;
                int directionRotations = 0;

                if (originOnPent != 0)
                {
                    int originLeadingDigit = (int) H3Index._h3LeadingNonZeroDigit(origin);

                    if ((H3Index.isResClassIII(res) &&
                         FAILED_DIRECTIONS_III[originLeadingDigit, (int) dir]) ||
                        (!H3Index.isResClassIII(res) &&
                         FAILED_DIRECTIONS_II[originLeadingDigit, (int) dir]))
                    {
                        // TODO this part of the pentagon might not be unfolded
                        // correctly.
                        return 3;
                    }

                    directionRotations = PENTAGON_ROTATIONS[originLeadingDigit, (int) dir];
                    pentagonRotations = directionRotations;
                }
                else if (indexOnPent != 0)
                {
                    int indexLeadingDigit = (int) H3Index._h3LeadingNonZeroDigit(h3);

                    if ((H3Index.isResClassIII(res) &&
                         FAILED_DIRECTIONS_III[indexLeadingDigit, (int) revDir]) ||
                        (!H3Index.isResClassIII(res) &&
                         FAILED_DIRECTIONS_II[indexLeadingDigit, (int) revDir]))
                    {
                        // TODO this part of the pentagon might not be unfolded
                        // correctly.
                        return 4;
                    }

                    pentagonRotations = PENTAGON_ROTATIONS[(int) revDir, indexLeadingDigit];
                }

                if (pentagonRotations < 0)
                {
                    throw new Exception("assert(pentagonRotations >= 0);");
                }

                if (directionRotations < 0)
                {
                    throw new Exception("assert(directionRotations >= 0);");
                }



                for (int i = 0; i < pentagonRotations; i++)
                {
                    CoordIJK._ijkRotate60cw(ref indexFijk.coord);
                }

                CoordIJK offset = new CoordIJK();
                CoordIJK._neighbor(ref offset, dir);
                // Scale offset based on resolution
                for (int r = res - 1; r >= 0; r--)
                {
                    if (H3Index.isResClassIII(r + 1))
                    {
                        // rotate ccw
                        CoordIJK._downAp7(ref offset);
                    }
                    else
                    {
                        // rotate cw
                        CoordIJK._downAp7r(ref offset);
                    }
                }

                for (int i = 0; i < directionRotations; i++)
                {
                    CoordIJK._ijkRotate60cw(ref offset);
                }

                // Perform necessary translation
                CoordIJK._ijkAdd(indexFijk.coord, offset, ref indexFijk.coord);
                CoordIJK._ijkNormalize(ref indexFijk.coord);
            }
            else if (originOnPent != 0 && indexOnPent != 0)
            {
                // If the origin and index are on pentagon, and we checked that the base
                // cells are the same or neighboring, then they must be the same base
                // cell.
                if (baseCell != originBaseCell)
                {
                    throw new Exception("assert(baseCell == originBaseCell);");
                }


                int originLeadingDigit = (int) H3Index._h3LeadingNonZeroDigit(origin);
                int indexLeadingDigit = (int) H3Index._h3LeadingNonZeroDigit(h3);

                if (FAILED_DIRECTIONS_III[originLeadingDigit, indexLeadingDigit] ||
                    FAILED_DIRECTIONS_II[originLeadingDigit, indexLeadingDigit])
                {
                    // TODO this part of the pentagon might not be unfolded
                    // correctly.
                    return 5;
                }

                int withinPentagonRotations =
                    PENTAGON_ROTATIONS[originLeadingDigit, indexLeadingDigit];

                for (int i = 0; i < withinPentagonRotations; i++)
                {
                    CoordIJK._ijkRotate60cw(ref indexFijk.coord);
                }
            }

            out_coord = indexFijk.coord;
            return 0;
        }

        /// <summary>
        /// Produces an index for ijk+ coordinates anchored by an origin.
        ///
        /// The coordinate space used by this function may have deleted
        /// regions or warping due to pentagonal distortion.
        ///
        /// Failure may occur if the coordinates are too far away from the origin
        /// or if the index is on the other side of a pentagon.
        /// </summary>
        /// <param name="origin">An anchoring index for the ijk+ coordinate system.</param>
        /// <param name="ijk">IJK+ Coordinates to find the index of</param>
        /// <param name="out_h3">The index will be placed here on success</param>
        /// <returns>0 on success, or another value on failure</returns>
        /// <!-- Based off 3.2.0 -->
        internal static int localIjkToH3(H3Index origin, CoordIJK ijk, ref H3Index out_h3)
        {
            int res = H3Index.H3_GET_RESOLUTION(origin);
            int originBaseCell = H3Index.H3_GET_BASE_CELL(origin);
            int originOnPent = BaseCells._isBaseCellPentagon(originBaseCell)
                                   ? 1
                                   : 0;

            // This logic is very similar to faceIjkToH3
            // initialize the index
            out_h3 = H3Index.H3_INIT;
            H3Index.H3_SET_MODE(ref out_h3, Constants.H3_HEXAGON_MODE);
            H3Index.H3_SET_RESOLUTION(ref out_h3, res);
            Direction dir;
            // check for res 0/base cell
            if (res == 0)
            {
                if (ijk.i > 1 || ijk.j > 1 || ijk.k > 1)
                {
                    // out of range input
                    return 1;
                }

                dir = CoordIJK._unitIjkToDigit(ref ijk);
                int newBaseCell = BaseCells._getBaseCellNeighbor(originBaseCell, dir);
                if (newBaseCell == BaseCells.INVALID_BASE_CELL)
                {
                    // Moving in an invalid direction off a pentagon.
                    return 1;
                }

                H3Index.H3_SET_BASE_CELL(ref out_h3, newBaseCell);
                return 0;
            }

            // we need to find the correct base cell offset (if any) for this H3 index;
            // start with the passed in base cell and resolution res ijk coordinates
            // in that base cell's coordinate system
            CoordIJK ijkCopy = new CoordIJK(ijk.i, ijk.j, ijk.k);

            // build the H3Index from finest res up
            // adjust r for the fact that the res 0 base cell offsets the indexing
            // digits
            for (int r = res - 1; r >= 0; r--)
            {
                CoordIJK lastIJK = ijkCopy;
                CoordIJK lastCenter;
                if (H3Index.isResClassIII(r + 1))
                {
                    // rotate ccw
                    CoordIJK._upAp7(ref ijkCopy);
                    lastCenter = ijkCopy;
                    CoordIJK._downAp7(ref lastCenter);
                }
                else
                {
                    // rotate cw
                    CoordIJK._upAp7r(ref ijkCopy);
                    lastCenter = ijkCopy;
                    CoordIJK._downAp7r(ref lastCenter);
                }

                CoordIJK diff = new CoordIJK();
                CoordIJK._ijkSub(ref lastIJK, ref lastCenter, ref diff);
                CoordIJK._ijkNormalize(ref diff);

                H3Index.H3_SET_INDEX_DIGIT(ref out_h3, r + 1, (ulong) CoordIJK._unitIjkToDigit(ref diff));
            }

            // ijkCopy should now hold the IJK of the base cell in the
            // coordinate system of the current base cell

            if (ijkCopy.i > 1 || ijkCopy.j > 1 || ijkCopy.k > 1)
            {
                // out of range input
                return 2;
            }

            // lookup the correct base cell
            dir = CoordIJK._unitIjkToDigit(ref ijkCopy);
            int baseCell = BaseCells._getBaseCellNeighbor(originBaseCell, dir);
            // If baseCell is invalid, it must be because the origin base cell is a
            // pentagon, and because pentagon base cells do not border each other,
            // baseCell must not be a pentagon.
            int indexOnPent =
                (baseCell == BaseCells.INVALID_BASE_CELL
                     ? 0
                     : BaseCells._isBaseCellPentagon(baseCell)
                         ? 1
                         : 0);

            if (dir != (int) Direction.CENTER_DIGIT)
            {
                // If the index is in a warped direction, we need to unwarp the base
                // cell direction. There may be further need to rotate the index digits.
                int pentagonRotations = 0;
                if (originOnPent != 0)
                {
                    Direction originLeadingDigit = H3Index._h3LeadingNonZeroDigit(origin);
                    pentagonRotations =
                        PENTAGON_ROTATIONS_REVERSE[(int) originLeadingDigit, (int) dir];
                    for (int i = 0; i < pentagonRotations; i++)
                    {
                        dir = CoordIJK._rotate60ccw(dir);
                    }

                    // The pentagon rotations are being chosen so that dir is not the
                    // deleted direction. If it still happens, it means we're moving
                    // into a deleted subsequence, so there is no index here.
                    if (dir == Direction.K_AXES_DIGIT)
                    {
                        return 3;
                    }

                    baseCell = BaseCells._getBaseCellNeighbor(originBaseCell, dir);

                    // indexOnPent does not need to be checked again since no pentagon
                    // base cells border each other.
                    if (baseCell == BaseCells.INVALID_BASE_CELL)
                    {
                        throw new Exception("assert(baseCell != BaseCells.INVALID_BASE_CELL);");
                    }

                    if (BaseCells._isBaseCellPolarPentagon(baseCell))
                    {
                        throw new Exception("assert(!BaseCells._isBaseCellPentagon(baseCell));");
                    }
                }

                // Now we can determine the relation between the origin and target base
                // cell.
                int baseCellRotations =
                    BaseCells.baseCellNeighbor60CCWRots[originBaseCell, (int) dir];
                if (baseCellRotations < 0)
                {
                    throw new Exception("assert(baseCellRotations >= 0);");
                }

                // Adjust for pentagon warping within the base cell. The base cell
                // should be in the right location, so now we need to rotate the index
                // back. We might not need to check for errors since we would just be
                // double mapping.
                if (indexOnPent != 0)
                {
                    Direction revDir =
                        BaseCells._getBaseCellDirection(baseCell, originBaseCell);

                    if (revDir == Direction.INVALID_DIGIT)
                    {
                        throw new Exception("assert(revDir != Direction.INVALID_DIGIT);");
                    }


                    // Adjust for the different coordinate space in the two base cells.
                    // This is done first because we need to do the pentagon rotations
                    // based on the leading digit in the pentagon's coordinate system.
                    for (int i = 0; i < baseCellRotations; i++)
                    {
                        out_h3 = H3Index._h3Rotate60ccw(ref out_h3);
                    }

                    Direction indexLeadingDigit = H3Index._h3LeadingNonZeroDigit(out_h3);
                    if (BaseCells._isBaseCellPolarPentagon(baseCell))
                    {
                        pentagonRotations =
                            PENTAGON_ROTATIONS_REVERSE_POLAR[(int) revDir, (int) indexLeadingDigit];
                    }
                    else
                    {
                        pentagonRotations =
                            PENTAGON_ROTATIONS_REVERSE_NONPOLAR[(int) revDir, (int) indexLeadingDigit];
                    }

                    if (pentagonRotations < 0)
                    {
                        throw new Exception("assert(pentagonRotations >= 0);");
                    }


                    for (int i = 0; i < pentagonRotations; i++)
                    {
                        out_h3 = H3Index._h3RotatePent60ccw(ref out_h3);
                    }
                }
                else
                {
                    if (pentagonRotations < 0)
                    {
                        throw new Exception("assert(pentagonRotations >= 0);");
                    }


                    for (int i = 0; i < pentagonRotations; i++)
                    {
                        out_h3 = H3Index._h3Rotate60ccw(ref out_h3);
                    }

                    // Adjust for the different coordinate space in the two base cells.
                    for (int i = 0; i < baseCellRotations; i++)
                    {
                        out_h3 = H3Index._h3Rotate60ccw(ref out_h3);
                    }
                }
            }
            else if (originOnPent != 0 && indexOnPent != 0)
            {
                int originLeadingDigit = (int) H3Index._h3LeadingNonZeroDigit(origin);
                int indexLeadingDigit = (int) H3Index._h3LeadingNonZeroDigit(out_h3);

                int withinPentagonRotations =
                    PENTAGON_ROTATIONS_REVERSE[originLeadingDigit, indexLeadingDigit];
                if (withinPentagonRotations < 0)
                {
                    throw new Exception("assert(withinPentagonRotations >= 0);");
                }

                for (int i = 0; i < withinPentagonRotations; i++)
                {
                    out_h3 = H3Index._h3Rotate60ccw(ref out_h3);
                }
            }

            if (indexOnPent != 0)
            {
                // TODO: There are cases in h3ToLocalIjk which are failed but not
                // accounted for here - instead just fail if the recovered index is
                // invalid.
                if (H3Index._h3LeadingNonZeroDigit(out_h3) == Direction.K_AXES_DIGIT)
                {
                    return 4;
                }
            }

            H3Index.H3_SET_BASE_CELL(ref out_h3, baseCell);

            return 0;
        }

        /// <summary>
        /// Produces ij coordinates for an index anchored by an origin.
        ///
        /// The coordinate space used by this function may have deleted
        /// regions or warping due to pentagonal distortion.
        ///
        /// Coordinates are only comparable if they come from the same
        /// origin index.
        ///
        /// Failure may occur if the index is too far away from the origin
        /// or if the index is on the other side of a pentagon.
        ///
        /// This function is experimental, and its output is not guaranteed
        /// to be compatible across different versions of H3.
        /// </summary>
        /// <param name="origin">An anchoring index for the ij coordinate system.</param>
        /// <param name="h3">Index to find the coordinates of</param>
        /// <param name="out_coord">ij coordinates of the index will be placed here on success</param>
        /// <returns>0 on success, or another value on failure.</returns>
        /// <!-- Based off 3.2.0 -->
        int experimentalH3ToLocalIj(H3Index origin, H3Index h3, CoordIJ out_coord) {
            // This function is currently experimental. Once ready to be part of the
            // non-experimental API, this function (with the experimental prefix) will
            // be marked as deprecated and to be removed in the next major version. It
            // will be replaced with a non-prefixed function name.
            CoordIJK ijk = new CoordIJK();
            int failed = h3ToLocalIjk(origin, h3, ref ijk);
            if (failed != 0) {
                return failed;
            }

            CoordIJK.ijkToIj(ijk, ref out_coord);

            return 0;
        }

        /// <summary>
        /// Produces an index for ij coordinates anchored by an origin.
        ///
        /// The coordinate space used by this function may have deleted
        /// regions or warping due to pentagonal distortion.
        ///
        /// Failure may occur if the index is too far away from the origin
        /// or if the index is on the other side of a pentagon.
        ///
        /// This function is experimental, and its output is not guaranteed
        /// to be compatible across different versions of H3.
        /// </summary>
        /// <param name="origin">An anchoring index for the ij coordinate system.</param>
        /// <param name="ij">ij coordinates to index.</param>
        /// <param name="out_h3">Index will be placed here on success.</param>
        /// <returns>0 on succedd, or another value on failure</returns>
        /// <!-- Based off 3.2.0 -->
        public static int experimentalLocalIjToH3(H3Index origin,  CoordIJ ij,
                                    H3Index out_h3)
        {
            // This function is currently experimental. Once ready to be part of the
            // non-experimental API, this function (with the experimental prefix) will
            // be marked as deprecated and to be removed in the next major version. It
            // will be replaced with a non-prefixed function name.
            CoordIJK ijk = new CoordIJK();
            CoordIJK.ijToIjk(ij, ref ijk);

            return localIjkToH3(origin, ijk, ref out_h3);
        }

        /// <summary>
        /// Produces the grid distance between the two indexes.
        ///
        /// This function may fail to find the distance between two indexes, for
        /// example if they are very far apart. It may also fail when finding
        /// distances for indexes on opposite sides of a pentagon.
        /// </summary>
        /// <param name="origin">Index to find the distance from</param>
        /// <param name="h3">Index to find the distance to</param>
        /// <returns>
        /// The distance, or a negative number if the library could not compute the distance
        /// </returns>
        /// <!-- Based off 3.2.0 -->
        public static int h3Distance(H3Index origin, H3Index h3) {
            CoordIJK originIjk = new CoordIJK();
            CoordIJK h3Ijk = new CoordIJK();
            if (h3ToLocalIjk(origin, origin, ref originIjk) != 0)
            {
                // Currently there are no tests that would cause getting the coordinates
                // for an index the same as the origin to fail.
                return -1;  // LCOV_EXCL_LINE
            }
            if (h3ToLocalIjk(origin, h3, ref h3Ijk)!=0)
            {
                return -1;
            }

            return CoordIJK.ijkDistance(originIjk, h3Ijk);
        }
    }

}