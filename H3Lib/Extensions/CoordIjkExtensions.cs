using System;
using System.Linq;

namespace H3Lib.Extensions
{
    /// <summary>
    /// Extension methods for CoordIjk type
    /// </summary>
    public static class CoordIjkExtensions
    {
        /// <summary>
        /// Tests if all coordinates are zero
        /// </summary>
        public static bool IsZero(this CoordIjk ijk)
        {
            return ijk.I == 0 && ijk.J == 0 && ijk.K == 0;
        }
        
        /// <summary>
        /// Returns sum of all coordinates
        /// </summary>
        public static int Sum(this CoordIjk ijk)
        {
            return ijk.I + ijk.J + ijk.K;
        }
        
        /// <summary>
        /// Change I coordinate value
        /// </summary>
        public static CoordIjk SetI(this CoordIjk ijk, int i)
        {
            return new CoordIjk(i, ijk.J, ijk.K);
        }
        /// <summary>
        /// Change J coordinate value
        /// </summary>
        public static CoordIjk SetJ(this CoordIjk ijk, int j)
        {
            return new CoordIjk(ijk.I, j, ijk.K);
        }
        
        /// <summary>
        /// Change K coordinate value
        /// </summary>
        public static CoordIjk SetK(this CoordIjk ijk, int k)
        {
            return new CoordIjk(ijk.I, ijk.J, k);
        }

        /// <summary>
        /// Change IJ coordinates value
        /// </summary>
        public static CoordIjk SetIJ(this CoordIjk ijk, int i, int j)
        {
            return new CoordIjk(i, j, ijk.K);
        }

        /// <summary>
        /// Change JK coordinates value
        /// </summary>
        public static CoordIjk SetIK(this CoordIjk ijk, int i, int k)
        {
            return new CoordIjk(i, ijk.J, k);
        }

        /// <summary>
        /// Change JK coordinates value
        /// </summary>
        public static CoordIjk SetJK(this CoordIjk ijk, int j, int k)
        {
            return new CoordIjk(ijk.I, j, k);
        }
        
        /// <summary>
        /// Find the center point in 2D cartesian coordinates of a hex.
        /// </summary>
        /// <param name="h">The ijk coordinates of the hex.</param>
        /// <!--
        /// coordijk,c
        /// void _ijkToHex2d
        /// -->
        internal static Vec2d ToHex2d(this CoordIjk h)
        {
            int i = h.I - h.K;
            int j = h.J - h.K;

            return new Vec2d(i - 0.5m * j, j * Constants.H3.M_SQRT3_2);
        }

        /// <summary>
        /// Normalizes ijk coordinates by setting the components to the smallest possible
        /// values. Works in place.
        /// </summary>
        /// <param name="coord">The ijk coordinates to normalize.</param>
        /// <!--
        /// coordijk.c
        /// void _ijkNormalize
        /// -->
        internal static CoordIjk Normalized(this CoordIjk coord)
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
            int min = new[] {i, j, k}.Min();
            
            return min <= 0
                       ? new CoordIjk(i, j, k)
                       : new CoordIjk(i - min, j - min, k - min);
        }

        /// <summary>
        /// Determines the H3 digit corresponding to a unit vector in ijk coordinates.
        /// </summary>
        /// <param name="ijk">The ijk coordinates; must be a unit vector.</param>
        /// <returns>
        /// The H3 digit (0-6) corresponding to the ijk unit vector, or
        /// <see cref="Direction.INVALID_DIGIT"/> INVALID_DIGIT on failure
        /// </returns>
        /// <!--
        /// coordijk.c
        /// Direction _unitIjkToDigit
        /// -->
        internal static Direction ToDirection(this CoordIjk ijk)
        {
            var c = ijk.Normalized();
            var test = Constants.CoordIjk.UnitVectors.Where(pair => pair.Value == c).ToList();

            return test.Any()
                       ? test.First().Key
                       : Direction.INVALID_DIGIT;
        }

        /// <summary>
        /// Find the normalized ijk coordinates of the indexing parent of a cell in a
        /// counter-clockwise aperture 7 grid. Works in place.
        /// </summary>
        /// <param name="ijk">The ijk coordinates</param>
        /// <!--
        /// coordijk.c
        /// void _upAp7
        /// -->
        internal static CoordIjk UpAp7(this CoordIjk ijk)
        {
            // convert to CoordIJ
            int i = ijk.I - ijk.K;
            int j = ijk.J - ijk.K;
            
            var dividendI = (decimal)(3 * i - j);
            var newI = (int) Math.Round(dividendI / 7.0m, MidpointRounding.AwayFromZero);
            
            var dividendJ = (decimal)(i + 2 * j);
            var newJ = (int) Math.Round(dividendJ / 7.0m, MidpointRounding.AwayFromZero);

            return new CoordIjk(newI, newJ, 0).Normalized();
        }

        /// <summary>
        /// Find the normalized ijk coordinates of the indexing parent of a cell in a
        /// clockwise aperture 7 grid. Works in place.
        /// </summary>
        /// <param name="ijk">The ijk coordinates</param>
        /// <!--
        /// coordijk.c
        /// void _upAp7r
        /// -->
        internal static CoordIjk UpAp7R(this CoordIjk ijk)
        {
            // convert to CoordIJ
            int i = ijk.I - ijk.K;
            int j = ijk.J - ijk.K;

            var newI = (int) Math.Round((2 * i + j) / 7.0, MidpointRounding.AwayFromZero);
            var newJ = (int) Math.Round((3 * j - i) / 7.0, MidpointRounding.AwayFromZero);

            return new CoordIjk(newI, newJ, 0).Normalized();
        }

        /// <summary>
        /// Find the normalized ijk coordinates of the hex centered on the indicated
        /// hex at the next finer aperture 7 counter-clockwise resolution. Works in
        /// place.
        /// </summary>
        /// <param name="ijk">The ijk coordinates</param>
        /// <!--
        /// coordijk.c
        /// void _downAp7
        /// -->
        internal static CoordIjk DownAp7(this CoordIjk ijk)
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
        /// <!--
        /// coordijk.c
        /// void _downAp7r
        /// -->
        internal static CoordIjk DownAp7R(this CoordIjk ijk)
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
        /// <!--
        /// coordijk.c
        /// void _neighbor
        /// -->
        internal static CoordIjk Neighbor(this CoordIjk ijk, Direction digit)
        {
            if (digit <= Direction.CENTER_DIGIT || digit >= Direction.NUM_DIGITS)
            {
                return ijk;
            }

            return (ijk + Constants.CoordIjk.UnitVectors[digit]).Normalized();
        }

        /// <summary>
        /// Rotates ijk coordinates 60 degrees counter-clockwise. Works in place.
        /// </summary>
        /// <param name="ijk">The ijk coordinates.</param>
        /// <!--
        /// coordijk.c
        /// void _ijkRotate60ccw
        /// -->
        internal static CoordIjk Rotate60CounterClockwise(this CoordIjk ijk)
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
        /// <!--
        /// coordijk.c
        /// void _ijkRotate60cw
        /// -->
        internal static CoordIjk Rotate60Clockwise(this CoordIjk ijk)
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
        /// <!--
        /// coordijk.c
        /// void _downAp3
        /// -->
        internal static CoordIjk DownAp3(this CoordIjk ijk)
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
        /// <!--
        /// coordijk.c
        /// void _downAp3r
        /// -->
        internal static CoordIjk DownAp3R(this CoordIjk ijk)
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
        /// <!--
        /// coordijk.c
        /// int ijkDistance
        /// -->
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
        /// <!--
        /// coordijk.c
        /// void ijkToIj
        /// -->
        public static CoordIj ToIj(this CoordIjk ijk)
        {
            return new CoordIj(ijk.I - ijk.K, ijk.J - ijk.K);
        }

        /// <summary>
        /// Convert IJK coordinates to cube coordinates, in place
        /// </summary>
        /// <param name="ijk">Coordinate to convert</param>
        /// <!--
        /// coordijk.c
        /// void ijkToCube
        /// -->
        public static CoordIjk ToCube(this CoordIjk ijk)
        {
            (int i, int j, int k) = (ijk.I, ijk.J, ijk.K);
            i = -i + k;
            j -= k;
            k = -i - j;

            return new CoordIjk(i, j, k);
        }

        /// <summary>
        /// Convert cube coordinates to IJK coordinates, in place
        /// </summary>
        /// <param name="ijk">Coordinate to convert</param>
        /// <!--
        /// coordijk.c
        /// void cubeToIjk
        /// -->
        public static CoordIjk FromCube(this CoordIjk ijk)
        {
            return new CoordIjk(-ijk.I, ijk.J, 0).Normalized();
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
        /// <returns>0 on success, or another value on failure</returns>
        /// <!--
        /// localij.c
        /// int localIjkToH3
        /// -->
        public static (int, H3Index) LocalIjkToH3(this CoordIjk ijk, H3Index origin)
        {
            int res = origin.Resolution;
            int originBaseCell = origin.BaseCell;
            bool originOnPent = originBaseCell.IsBaseCellPentagon();

            // This logic is very similar to faceIjkToH3
            // initialize the index

            var outH3 = new H3Index(Constants.H3Index.H3_INIT);
            outH3 = outH3.SetMode(H3Mode.Hexagon).SetResolution(res);

            // check for res 0/base cell
            if (res == 0) 
            {
                if (ijk.I > 1 || ijk.J > 1 || ijk.K > 1)
                {
                    return (1, outH3);
                }

                Direction dir = ijk.ToDirection();
                int newBaseCell = originBaseCell.GetNeighbor(dir);
            
                if (newBaseCell == Constants.BaseCells.InvalidBaseCell)
                {
                    // Moving in an invalid direction off a pentagon.
                    return (1, new H3Index());
                }

                return (0, outH3.SetBaseCell(newBaseCell));
            }

            // we need to find the correct base cell offset (if any) for this H3 index;
            // start with the passed in base cell and resolution res ijk coordinates
            // in that base cell's coordinate system
            var ijkCopy = new CoordIjk(ijk);

            // build the H3Index from finest res up
            // adjust r for the fact that the res 0 base cell offsets the indexing
            // digits
            for (int r = res - 1; r >= 0; r--)
            {
                var lastIJK = new CoordIjk(ijkCopy);
                CoordIjk lastCenter;
                if ((r+1).IsResClassIii())
                {
                    // rotate ccw
                    ijkCopy = ijkCopy.UpAp7();
                    lastCenter = new CoordIjk(ijkCopy);
                    lastCenter = lastCenter.DownAp7();
                }
                else
                {
                    // rotate cw
                    ijkCopy = ijkCopy.UpAp7R();
                    lastCenter = new CoordIjk(ijkCopy);
                    lastCenter = lastCenter.DownAp7R();
                }

                var diff = (lastIJK - lastCenter).Normalized();
                outH3 = outH3.SetIndexDigit(r + 1, (ulong) diff.ToDirection());
            }

            // ijkCopy should now hold the IJK of the base cell in the
            // coordinate system of the current base cell
            if (ijkCopy.I > 1 || ijkCopy.J > 1 || ijkCopy.K > 1)
            {
                // out of range input
                return (2, new H3Index());
            }

            // lookup the correct base cell
            var dir2 = ijkCopy.ToDirection();
            int baseCell = originBaseCell.GetNeighbor(dir2);
            // If baseCell is invalid, it must be because the origin base cell is a
            // pentagon, and because pentagon base cells do not border each other,
            // baseCell must not be a pentagon.
            int indexOnPent =
                baseCell == Constants.BaseCells.InvalidBaseCell
                    ? 0
                    : baseCell.IsBaseCellPentagon()
                        ? 1
                        : 0;
            

            if (dir2 != Direction.CENTER_DIGIT)
            {
                // If the index is in a warped direction, we need to un-warp the base
                // cell direction. There may be further need to rotate the index digits.
                var pentagonRotations = 0;
                if (originOnPent)
                {
                    Direction originLeadingDigit = origin.LeadingNonZeroDigit;
                    pentagonRotations =
                        Constants.LocalIJ.PENTAGON_ROTATIONS_REVERSE[(int)originLeadingDigit, (int)dir2];
                    for (var i = 0; i < pentagonRotations; i++)
                    {
                        dir2 = dir2.Rotate60CounterClockwise();
                    }

                    // The pentagon rotations are being chosen so that dir is not the
                    // deleted direction. If it still happens, it means we're moving
                    // into a deleted subsequence, so there is no index here.
                    if (dir2 == Direction.K_AXES_DIGIT)
                    {
                        return(3, new H3Index());
                    }
                    baseCell = Constants.BaseCells.BaseCellNeighbors[originBaseCell, (int)dir2];

                    // indexOnPent does not need to be checked again since no pentagon
                    // base cells border each other.
                    if (baseCell == Constants.BaseCells.InvalidBaseCell)
                    {
                        throw new Exception("baseCell != INVALID_BASE_CELL");
                    }

                    if (baseCell.IsBaseCellPentagon())
                    {
                        throw new Exception("!_isBaseCellPentagon(baseCell)");
                    }
                }
                
                // Now we can determine the relation between the origin and target base
                // cell.
                int baseCellRotations =
                    Constants.BaseCells.BaseCellNeighbor60CounterClockwiseRotation[originBaseCell, (int) dir2];
                if (baseCellRotations < 0)
                {
                    throw new Exception("assert(baseCellRotations >= 0)");
                }

                // Adjust for pentagon warping within the base cell. The base cell
                // should be in the right location, so now we need to rotate the index
                // back. We might not need to check for errors since we would just be
                // double mapping.
                if (indexOnPent !=0)
                {
                    var revDir = baseCell.GetBaseCellDirection(originBaseCell);
                    if (revDir == Direction.INVALID_DIGIT)
                    {
                        throw new Exception("assert(revDir != INVALID_DIGIT)");
                    }

                    // Adjust for the different coordinate space in the two base cells.
                    // This is done first because we need to do the pentagon rotations
                    // based on the leading digit in the pentagon's coordinate system.
                    for (var i = 0; i < baseCellRotations; i++)
                    {
                        outH3 = outH3.Rotate60CounterClockwise();
                    }

                    var indexLeadingDigit = outH3.LeadingNonZeroDigit;
                    pentagonRotations =
                        baseCell.IsBaseCellPolarPentagon()
                            ? Constants.LocalIJ.PENTAGON_ROTATIONS_REVERSE_POLAR[(int) revDir, (int) indexLeadingDigit]
                            : Constants.LocalIJ.PENTAGON_ROTATIONS_REVERSE_NONPOLAR[(int) revDir, (int) indexLeadingDigit];

                    if (pentagonRotations < 0)
                    {
                        throw new Exception("pentagonRotations >= 0");
                    }

                    for (int i = 0; i < pentagonRotations; i++)
                    {
                        outH3 = outH3.RotatePent60CounterClockwise();
                    }
                }
                else
                {
                    if (pentagonRotations < 0)
                    {
                        throw new Exception("pentagonRotations >= 0");
                    }

                    for (var i = 0; i < pentagonRotations; i++)
                    {
                        outH3 = outH3.Rotate60CounterClockwise();
                    }

                    // Adjust for the different coordinate space in the two base cells.
                    for (var i = 0; i < baseCellRotations; i++)
                    {
                        outH3 = outH3.Rotate60CounterClockwise();
                        
                    }
                }
            }
            else if (originOnPent && indexOnPent!=0)
            {
                var originLeadingDigit = (int)origin.LeadingNonZeroDigit;
                var indexLeadingDigit = (int) outH3.LeadingNonZeroDigit;

                int withinPentagonRotations =
                    Constants.LocalIJ.PENTAGON_ROTATIONS_REVERSE[originLeadingDigit, indexLeadingDigit];
                if (withinPentagonRotations < 0)
                {
                    throw new Exception("withinPentagonRotations >= 0");
                }

                for (var i = 0; i < withinPentagonRotations; i++)
                {
                    outH3 = outH3.Rotate60CounterClockwise();
                }
            }

            if (indexOnPent==0)
            {
                return (0, outH3.SetBaseCell(baseCell));
            }

            // TODO: There are cases in h3ToLocalIjk which are failed but not
            // accounted for here - instead just fail if the recovered index is
            // invalid.
            return outH3.LeadingNonZeroDigit == Direction.K_AXES_DIGIT
                       ? (4, new H3Index())
                       : (0, outH3.SetBaseCell(baseCell));
        }
        
    }
}
