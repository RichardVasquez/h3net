using System;
using System.Collections.Generic;
using System.Linq;
using H3Lib.StaticData;

namespace H3Lib.Extensions
{
    /// <summary>
    /// Operations that act upon a data type of <see cref="H3Index"/> located
    /// in one central location.
    /// </summary>
    public static class H3IndexExtensions
    {
        /// <summary>
        /// Area of H3 cell in radians^2.
        ///
        /// The area is calculated by breaking the cell into spherical triangles and
        /// summing up their areas. Note that some H3 cells (hexagons and pentagons)
        /// are irregular, and have more than 6 or 5 sides.
        ///
        /// todo: optimize the computation by re-using the edges shared between triangles
        /// </summary>
        /// <param name="cell">H3 cell</param>
        /// <returns>cell area in radians^2</returns>
        /// <!--
        /// geoCoord.c
        /// double H3_EXPORT(cellAreaRads2)
        /// -->
        public static double CellAreaRadians2(this H3Index cell)
        {
            var c = cell.ToGeoCoord();
            var gb = cell.ToGeoBoundary();

            var area = 0.0;
            for (var i = 0; i < gb.NumVerts; i++)
            {
                int j = (i + 1) % gb.NumVerts;
                area += GeoCoord.TriangleArea(gb.Verts[i], gb.Verts[j], c);
            }

            return area;
        }

        /// <summary>
        /// Area of H3 cell in kilometers^2.
        /// </summary>
        /// <param name="h">h3 cell</param>
        /// <!--
        /// geoCoord.c
        /// double H3_EXPORT(cellAreaKm2)
        /// -->
        public static double CellAreaKm2(this H3Index h)
        {
            return h.CellAreaRadians2() * 
                   Constants.EARTH_RADIUS_KM *
                   Constants.EARTH_RADIUS_KM;
        }

        /// <summary>
        /// Area of H3 cell in meters^2.
        /// </summary>
        /// <param name="h">h3 cell</param>
        /// <!--
        /// geoCoord.c
        /// double H3_EXPORT(cellAreaM2)
        /// -->
        public static double CellAreaM2(this H3Index h)
        {
            return h.CellAreaKm2() * 1000 * 1000;
        }

        /// <summary>
        /// Length of a unidirectional edge in radians.
        /// </summary>
        /// <param name="edge">H3 unidirectional edge</param>
        /// <returns>length in radians</returns>
        /// <!--
        /// geoCoord.c
        /// double H3_EXPORT(exactEdgeLengthRads)
        /// -->
        public static double ExactEdgeLengthRads(this H3Index edge)
        {
            var gb = edge.UniEdgeToGeoBoundary();

            var length = 0.0;
            for (var i = 0; i < gb.NumVerts - 1; i++)
            {
                length += gb.Verts[i].DistanceToRadians(gb.Verts[i + 1]);
            }
            return length;
        }

        /// <summary>
        /// Length of a unidirectional edge in kilometers.
        /// </summary>
        /// <param name="edge">H3 unidirectional edge</param>
        /// <!--
        /// geoCoord.c
        /// double H3_EXPORT(exactEdgeLengthKm)
        /// -->
        public static double ExactEdgeLengthKm(this H3Index edge)
        {
            return edge.ExactEdgeLengthRads() * Constants.EARTH_RADIUS_KM;
        }

        /// <summary>
        /// Length of a unidirectional edge in meters.
        /// </summary>
        /// <param name="edge">H3 unidirectional edge</param>
        /// <!--
        /// geoCoord.c
        /// double H3_EXPORT(exactEdgeLengthM)
        /// -->
        public static double ExactEdgeLengthM(this H3Index edge)
        {
            return edge.ExactEdgeLengthKm() * 1000;
        }

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
        ///
        /// </summary>
        /// <param name="origin">An anchoring index for the ijk+ coordinate system.</param>
        /// <param name="h3">Index to find the coordinates of</param>
        /// <returns>
        /// Item1: 0 on success, or another value on failure.
        /// Item2: ijk+ coordinates of the index will be placed here on success
        /// </returns>
        /// <!--
        /// localij.c
        /// int h3ToLocalIjk
        /// -->
        public static (int, CoordIjk) ToLocalIjk(this H3Index origin, H3Index h3)
        {
            int res = origin.Resolution;

            if (res != h3.Resolution)
            {
                return (1, new CoordIjk());
            }

            int originBaseCell = origin.BaseCell;
            int baseCell =  h3.BaseCell;
            
            // Direction from origin base cell to index base cell
            var dir = Direction.CENTER_DIGIT;
            var revDir = Direction.CENTER_DIGIT;
            if (originBaseCell != baseCell)
            {
                dir = originBaseCell.GetBaseCellDirection(baseCell);
                if (dir == Direction.INVALID_DIGIT)
                {
                    // Base cells are not neighbors, can't unfold.
                    return (2, new CoordIjk());
                }
                revDir = baseCell.GetBaseCellDirection(originBaseCell);
                if (revDir == Direction.INVALID_DIGIT)
                {
                    throw new Exception("assert(revDir != INVALID_DIGIT);");
                }
            }

            int originOnPent = originBaseCell.IsBaseCellPentagon()
                                   ? 1
                                   : 0;
            int indexOnPent = baseCell.IsBaseCellPentagon()
                                  ? 1
                                  : 0;

            var indexFijk = new FaceIjk();

            if (dir != Direction.CENTER_DIGIT)
            {
                // Rotate index into the orientation of the origin base cell.
                // cw because we are undoing the rotation into that base cell.
                int baseCellRotations = StaticData.BaseCells.BaseCellNeighbor60CounterClockwiseRotation[originBaseCell,(int)dir];
                if (indexOnPent == 1)
                {
                    for (int i = 0; i < baseCellRotations; i++)
                    {
                        h3 = h3.RotatePent60Clockwise();
                        revDir = revDir.Rotate60Clockwise();

                        if (revDir == Direction.K_AXES_DIGIT)
                        {
                            revDir = revDir.Rotate60Clockwise();
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < baseCellRotations; i++)
                    {
                        h3 = h3.Rotate60Clockwise();
                        revDir = revDir.Rotate60Clockwise();
                    }
                }
            }

            // Face is unused. This produces coordinates in base cell coordinate space.
            (_, indexFijk) = h3.ToFaceIjkWithInitializedFijk(indexFijk);

            if (dir != Direction.CENTER_DIGIT)
            {
                if (baseCell == originBaseCell)
                {
                    throw new Exception("assert(baseCell != originBaseCell);");
                }

                if (originOnPent == 1 && indexOnPent == 1)
                {
                    throw new Exception("assert(!(originOnPent && indexOnPent));");
                }

                var pentagonRotations = 0;
                var directionRotations = 0;

                if (originOnPent == 1)
                {
                    var originLeadingDigit = (int) origin.LeadingNonZeroDigit;
                    if (LocalIJ.FAILED_DIRECTIONS[originLeadingDigit, (int) dir])
                    {
                        // TODO: We may be unfolding the pentagon incorrectly in this
                        // case; return an error code until this is guaranteed to be
                        // correct.
                        return (3, new CoordIjk());
                    }

                    directionRotations = LocalIJ.PENTAGON_ROTATIONS[originLeadingDigit,(int)dir];
                    pentagonRotations = directionRotations;
                }
                else if (indexOnPent == 1)
                {
                    int indexLeadingDigit = (int) h3.LeadingNonZeroDigit;

                    if (LocalIJ.FAILED_DIRECTIONS[indexLeadingDigit,(int)revDir])
                    {
                        // TODO: We may be unfolding the pentagon incorrectly in this
                        // case; return an error code until this is guaranteed to be
                        // correct.
                        return (4, new CoordIjk());
                    }

                    pentagonRotations = LocalIJ.PENTAGON_ROTATIONS[(int)revDir, indexLeadingDigit];
                }

                if (pentagonRotations < 0)
                {
                    throw new Exception("assert(pentagonRotations >= 0)");
                }

                if (directionRotations < 0)
                {
                    throw new Exception("directionRotations >= 0");
                }

                for (int i = 0; i < pentagonRotations; i++)
                {
                    indexFijk = new FaceIjk(indexFijk.Face, indexFijk.Coord.Rotate60Clockwise());
                }

                var offset = new CoordIjk().Neighbor(dir);
                // Scale offset based on resolution
                for (int r = res - 1; r >= 0; r--)
                {
                    offset = (r + 1).IsResClassIii()
                                 ? offset.DownAp7()
                                 : offset.DownAp7R();
                }

                for (var i = 0; i < directionRotations; i++)
                {
                    offset = offset.Rotate60Clockwise();
                }

                // Perform necessary translation
                indexFijk = new FaceIjk(indexFijk.Face,
                                        (indexFijk.Coord + offset).Normalized());
                
            }
            else if (originOnPent==1 && indexOnPent==1)
            {
                // If the origin and index are on pentagon, and we checked that the base
                // cells are the same or neighboring, then they must be the same base
                // cell.
                if (baseCell != originBaseCell)
                {
                    throw new Exception("assert(baseCell == originBaseCell)");
                }

                var originLeadingDigit = (int)origin.LeadingNonZeroDigit;
                var indexLeadingDigit = (int) h3.LeadingNonZeroDigit;

                if (LocalIJ.FAILED_DIRECTIONS[originLeadingDigit, indexLeadingDigit])
                {
                    // TODO: We may be unfolding the pentagon incorrectly in this case;
                    // return an error code until this is guaranteed to be correct.
                    return (5, new CoordIjk());
                }

                int withinPentagonRotations =
                    LocalIJ.PENTAGON_ROTATIONS[originLeadingDigit,indexLeadingDigit];

                for (var i = 0; i < withinPentagonRotations; i++)
                {
                    indexFijk = new FaceIjk(indexFijk.Face, indexFijk.Coord.Rotate60Clockwise());
                }
            }
        
            return (0, indexFijk.Coord);
        }

        /// <summary>
        /// Rotate an H3Index 60 degrees counter-clockwise about a pentagonal center.
        /// </summary>
        /// <param name="h">The H3Index.</param>
        /// <!--
        /// h3index.c
        /// H3Index _h3RotatePent60ccw
        /// -->
        public static H3Index RotatePent60CounterClockwise(this H3Index h)
        {
            // rotate in place; skips any leading 1 digits (k-axis)
            var foundFirstNonZeroDigit = 0;
            for (int r = 1, res = h.Resolution; r <= res; r++)
            {
                // rotate this digit
                h = h.SetIndexDigit(r, (ulong) h.GetIndexDigit(r).Rotate60CounterClockwise());

                // look for the first non-zero digit so we
                // can adjust for deleted k-axes sequence
                // if necessary
                if (foundFirstNonZeroDigit != 0 || h.GetIndexDigit(r) == 0)
                {
                    continue;
                }
                foundFirstNonZeroDigit = 1;

                // adjust for deleted k-axes sequence
                if (h.LeadingNonZeroDigit == Direction.K_AXES_DIGIT)
                {
                    h = h.Rotate60CounterClockwise();
                }
            }
            return h;
        }

        /// <summary>
        /// Rotate an H3Index 60 degrees clockwise about a pentagonal center.
        /// </summary>
        /// <param name="h"> The H3Index.</param>
        /// <!--
        /// h3Index.c
        /// H3Index _h3RotatePent60cw
        /// -->
        public static H3Index RotatePent60Clockwise(this H3Index h)
        {
            // rotate in place; skips any leading 1 digits (k-axis)
            var foundFirstNonZeroDigit = false;
            for (int r = 1, res = h.Resolution; r <= res; r++)
            {
                // rotate this digit
                h=h.SetIndexDigit(r, (ulong) h.GetIndexDigit(r).Rotate60Clockwise());

                // look for the first non-zero digit so we
                // can adjust for deleted k-axes sequence
                // if necessary
                if (foundFirstNonZeroDigit || h.GetIndexDigit(r) == 0)
                {
                    continue;
                }
                foundFirstNonZeroDigit = true;

                // adjust for deleted k-axes sequence
                if (h.LeadingNonZeroDigit == Direction.K_AXES_DIGIT)
                {
                    h = h.Rotate60Clockwise();
                }
            }
            return h;
        }

        /// <summary>
        /// Rotate an H3Index 60 degrees counter-clockwise.
        /// </summary>
        /// <param name="h">The H3Index.</param>
        /// <!--
        /// h3Index.c
        /// H3Index _h3Rotate60ccw(H3Index h)
        /// -->
        public static H3Index Rotate60CounterClockwise(this H3Index h)
        {
            for (int r = 1, res = h.Resolution; r <= res; r++)
            {
                var oldDigit = h.GetIndexDigit(r);
                h = h.SetIndexDigit(r, (ulong) oldDigit.Rotate60CounterClockwise());
            }

            return h;
        }

        /// <summary>
        /// Rotate an H3Index 60 degrees clockwise.
        /// </summary>
        /// <param name="h">The H3Index.</param>
        /// <!--
        /// h3Index.c
        /// H3Index _h3Rotate60cw
        /// --> 
        public static H3Index Rotate60Clockwise(this H3Index h)
        {
            for (int r = 1, res = h.Resolution; r <= res; r++)
            {
                var oldDigit = h.GetIndexDigit(r);
                h = h.SetIndexDigit(r, (ulong) oldDigit.Rotate60Clockwise());
            }
            return h;
        }

        /// <summary>
        /// Convert an H3Index to the FaceIjk address on a specified icosahedral face.
        /// </summary>
        /// <param name="h"> The H3Index.</param>
        /// <param name="fijk">
        /// The FaceIjk address, initialized with the desired face
        /// and normalized base cell coordinates.
        /// </param>
        /// <returns>
        /// Tuple
        /// Item1: Returns 1 if the possibility of overage exists, otherwise 0.
        /// Item2: Modified FaceIjk
        /// </returns>
        /// <!--
        /// h3Index.c
        /// int _h3ToFaceIjkWithInitializedFijk
        /// -->
        public static (int, FaceIjk) ToFaceIjkWithInitializedFijk(this H3Index h, FaceIjk fijk)
        {
            var empty = new CoordIjk();
            var ijk = new CoordIjk(fijk.Coord);
            int res = h.Resolution;

            // center base cell hierarchy is entirely on this face
            var possibleOverage = 1;

            if (!h.BaseCell.IsBaseCellPentagon() &&
                (res == 0 || ijk == empty))
            {
                possibleOverage = 0;
            }

            for (var r = 1; r <= res; r++)
            {
                ijk = r.IsResClassIii()
                          ? ijk.DownAp7()
                          : ijk.DownAp7R();

                ijk = ijk.Neighbor(h.GetIndexDigit(r));
            }

            fijk = new FaceIjk(fijk.Face, ijk);
            return (possibleOverage, fijk);
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
        /// <returns>
        /// Tuple with Item1 indicating success (0) or other
        ///            Item2 contains ij coordinates. 
        /// </returns>
        /// <!--
        /// localij.c
        /// int H3_EXPORT(experimentalH3ToLocalIj)
        /// -->
        public static (int, CoordIj) ToLocalIjExperimental(this H3Index origin, H3Index h3)
        {
            // This function is currently experimental. Once ready to be part of the
            // non-experimental API, this function (with the experimental prefix) will
            // be marked as deprecated and to be removed in the next major version. It
            // will be replaced with a non-prefixed function name.
            (int result, var coordIjk) = origin.ToLocalIjk(h3);
            return result == 0
                       ? (0, coordIjk.ToIj())
                       : (result, new CoordIj());
        }

        /// <summary>
        /// Produces the grid distance between the two indexes.
        /// 
        /// This function may fail to find the distance between two indexes, for
        /// example if they are very far apart. It may also fail when finding
        /// distances for indexes on opposite sides of a pentagon.
        /// </summary>
        /// <param name="origin">Index to find the distance from.</param>
        /// <param name="h3">Index to find the distance to.</param>
        /// <returns>
        /// The distance, or a negative number if the library could not
        /// compute the distance.
        /// </returns>
        /// <!--
        /// localij.c
        /// int H3_EXPORT(h3Distance)
        /// -->
        public static int DistanceTo(this H3Index origin, H3Index h3)
        {
            (int status1, var originIjk) = origin.ToLocalIjk(origin);

            if (status1 != 0)
            {
                // Currently there are no tests that would cause getting the coordinates
                // for an index the same as the origin to fail.
                return -1;  // LCOV_EXCL_LINE
            }

            (int status2, var h3Ijk) = origin.ToLocalIjk(h3);
            
            if (status2 != 0)
            {
                return -1;
            }

            return originIjk.DistanceTo(h3Ijk);
        }

        /// <summary>
        /// Number of indexes in a line from the start index to the end index,
        /// to be used for allocating memory. Returns a negative number if the
        /// line cannot be computed.
        /// </summary>
        /// <param name="start">Start index of the line</param>
        /// <param name="end">End index of the line</param>
        /// <returns>
        /// Size of the line, or a negative number if the line cannot
        /// be computed.
        /// </returns>
        /// <!--
        /// localij.c
        /// int H3_EXPORT(h3LineSize)
        /// -->
        public static int LineSize(this H3Index start, H3Index end)
        {
            int distance = start.DistanceTo(end);
            return distance >= 0
                       ? distance + 1
                       : distance;
        }

        /// <summary>
        /// Given two H3 indexes, return the line of indexes between them (inclusive).
        ///
        /// This function may fail to find the line between two indexes, for
        /// example if they are very far apart. It may also fail when finding
        /// distances for indexes on opposite sides of a pentagon.
        ///
        /// Notes:
        ///  - The specific output of this function should not be considered stable
        ///    across library versions. The only guarantees the library provides are
        ///    that the line length will be `h3Distance(start, end) + 1` and that
        ///    every index in the line will be a neighbor of the preceding index.
        ///  - Lines are drawn in grid space, and may not correspond exactly to either
        ///    Cartesian lines or great arcs.
        /// </summary>
        /// <param name="start">Start index of the line</param>
        /// <param name="end">End index of the line</param>
        /// <returns>
        /// Tuple:
        /// (status, IEnumerable)
        /// status => 0 success, otherwise failure
        /// </returns>
        /// <!--
        /// localij.c
        /// int H3_EXPORT(h3Line)
        /// -->
        public static (int, IEnumerable<H3Index>) LineTo(this H3Index start, H3Index end)
        {
            int distance = start.DistanceTo(end);
            // Early exit if we can't calculate the line
            if (distance < 0)
            {
                return (distance, null);
            }

            // Get IJK coords for the start and end. We've already confirmed
            // that these can be calculated with the distance check above.
            // Convert H3 addresses to IJK coords
            var (_, startIjk) = start.ToLocalIjk(start);
            var (_, endIjk) = end.ToLocalIjk(end);

            // Convert IJK to cube coordinates suitable for linear interpolation
            startIjk = startIjk.ToCube();
            endIjk = endIjk.ToCube();

            double iStep = distance != 0
                               ? (double) (endIjk.I - startIjk.I) / distance
                               : 0;
            double jStep = distance != 0
                               ? (double) (endIjk.J - startIjk.J) / distance
                               : 0;
            double kStep = distance != 0
                               ? (double) (endIjk.K - startIjk.K) / distance
                               : 0;

            List<H3Index> lineOut = new List<H3Index>();
            for (int n = 0; n <= distance; n++)
            {
                var currentIjk = CoordIjk.CubeRound
                    (
                     startIjk.I + iStep * n,
                     startIjk.J + jStep * n,
                     startIjk.K + kStep * n
                    );
                // Convert cube -> ijk -> h3 index
                currentIjk = currentIjk.FromCube();
                var (_, cell) = currentIjk.LocalIjkToH3(start);
                lineOut.Add(cell);
            }

            return (0, lineOut);
        }

        /// <summary>
        /// Returns whether or not an H3 index is a valid cell (hexagon or pentagon).
        /// </summary>
        /// <param name="h">The H3 index to validate.</param>
        /// <returns>true if the H3 index is valid</returns>
        /// <!--
        /// h3Index.c
        /// int H3_EXPORT(h3IsValid)
        /// -->
        public static bool IsValid(this H3Index h)
        {
            if (h.HighBit != 0 || h.Mode != H3Mode.Hexagon || h.ReservedBits != 0)
            {
                return false;
            }

            int baseCell = h.BaseCell;
            if (baseCell < 0 || baseCell >= Constants.NUM_BASE_CELLS)
            {
                return false;
            }

            int res = h.Resolution;
            if (res < 0 || res > Constants.MAX_H3_RES)
            {
                return false;
            }

            var foundFirstNonZeroDigit = false;
            for (var r = 1; r <= res; r++)
            {
                var digit = h.GetIndexDigit(r);

                if (!foundFirstNonZeroDigit && digit != Direction.CENTER_DIGIT) 
                {
                    foundFirstNonZeroDigit = true;
                    if (baseCell.IsBaseCellPentagon() &&
                        digit == Direction.K_AXES_DIGIT)
                    {
                        return false;
                    }
                }

                if (digit < Direction.CENTER_DIGIT || digit >= Direction.NUM_DIGITS)
                {
                    return false;
                }
            }

            for (int r = res + 1; r <= Constants.MAX_H3_RES; r++)
            { 
                var digit = h.GetIndexDigit(r);
                if (digit != Direction.INVALID_DIGIT)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// h3ToParent produces the parent index for a given H3 index
        /// </summary>
        /// <param name="h">H3Index to find parent of</param>
        /// <param name="parentRes">The resolution to switch to (parent, grandparent, etc)</param>
        /// <returns>H3Index of the parent, or H3_NULL if you actually asked for a child</returns>
        /// <!--
        /// h3Index.c
        /// H3Index H3_EXPORT(h3ToParent)
        /// -->
        public static H3Index ToParent(this H3Index h, int parentRes)
        {
            int childRes = h.Resolution;
            if (parentRes > childRes)
            {
                return StaticData.H3Index.H3_NULL;
            }

            if (parentRes == childRes)
            {
                return h;
            }

            if (parentRes < 0 || parentRes > Constants.MAX_H3_RES)
            {
                return StaticData.H3Index.H3_NULL;
            }

            var parentH = new H3Index(h).SetResolution(parentRes);
            for (int i = parentRes + 1; i <= childRes; i++)
            {
                parentH = parentH.SetIndexDigit(i, StaticData.H3Index.H3_DIGIT_MASK);
            }
            return parentH;
        }

        /// <summary>
        /// MaxChildrenSize returns the maximum number of children possible for a
        /// given child level.
        /// </summary>
        /// <param name="h3">H3Index to find the number of children of</param>
        /// <param name="childRes">The resolution of the child level you're interested in</param>
        /// <returns>count of maximum number of children (equal for hexagons, less for pentagons</returns>
        /// <!--
        /// h3Index.c
        /// int64_t H3_EXPORT(maxH3ToChildrenSize)
        /// -->
        public static long MaxChildrenSize(this H3Index h3, int childRes)
        {
            int parentRes = h3.Resolution;
            return !parentRes.IsValidChildRes(childRes)
                       ? 0
                       : 7L.Power(childRes - parentRes);
        }

        /// <summary>
        /// Initializes an H3 index.
        /// </summary>
        /// <param name="hp"> The H3 index to initialize.</param>
        /// <param name="res"> The H3 resolution to initialize the index to.</param>
        /// <param name="baseCell"> The H3 base cell to initialize the index to.</param>
        /// <param name="initDigit"> The H3 digit (0-7) to initialize all of the index digits to.</param>
        /// <!--
        /// h3Index.c
        /// void setH3Index
        /// -->
        public static H3Index SetIndex(this H3Index hp, int res, int baseCell, Direction initDigit)
        {
            H3Index h = new H3Index(StaticData.H3Index.H3_INIT);
                //.H3Index.H3_INIT;
            h = h.SetMode(H3Mode.Hexagon).SetResolution(res).SetBaseCell(baseCell);

            for (var r = 1; r <= res; r++)
            {
                h = h.SetIndexDigit(r, (ulong) initDigit);
            }

            return h;
        }

        /// <summary>
        /// Takes an H3Index and determines if it is actually a pentagon.
        /// </summary>
        /// <param name="h"> The H3Index to check.</param>
        /// <returns>Returns true if it is a pentagon, otherwise false.</returns>
        /// <!--
        /// h3Index.c
        /// int H3_EXPORT(h3IsPentagon)
        /// -->
        public static bool IsPentagon(this H3Index h)
        {
            return h.BaseCell.IsBaseCellPentagon() &&
                   h.LeadingNonZeroDigit == Direction.CENTER_DIGIT;
        }

        /// <summary>
        /// MakeDirectChild takes an index and immediately returns the immediate child
        /// index based on the specified cell number. Bit operations only, could generate
        /// invalid indexes if not careful (deleted cell under a pentagon).
        /// </summary>
        /// <param name="h"> H3Index to find the direct child of</param>
        /// <param name="cellNumber"> int id of the direct child (0-6)</param>
        /// <returns>The new H3Index for the child</returns>
        /// <!--
        /// h3Index.c
        /// H3Index makeDirectChild
        /// -->
        public static H3Index MakeDirectChild(this H3Index h, int cellNumber)
        {
            int childRes = h.Resolution + 1;
            var childH = h;
            childH = childH.SetResolution(childRes).SetIndexDigit(childRes, (ulong) cellNumber);
            return childH;
        }

        /// <summary>
        /// Convert an H3Index to a FaceIJK address.
        /// </summary>
        /// <param name="h">The H3 Index</param>
        /// <returns>The corresponding FaceIJK address.</returns>
        /// <!--
        /// h3Index.cs
        /// void _h3ToFaceIjk
        /// -->
        public static FaceIjk ToFaceIjk(this H3Index h)
        {
            int baseCell = h.BaseCell;
            // adjust for the pentagonal missing sequence; all of sub-sequence 5 needs
            // to be adjusted (and some of sub-sequence 4 below)
            if (baseCell.IsBaseCellPentagon() && 
                h.LeadingNonZeroDigit == Direction.IK_AXES_DIGIT)
            {
                h = h.Rotate60Clockwise();
            }

            // start with the "home" face and ijk+ coordinates for the base cell of c
            var fijk = StaticData.BaseCells.BaseCellData[baseCell].HomeFijk;
            int result;
            (result, fijk) = h.ToFaceIjkWithInitializedFijk(fijk);
            if (result == 0)
            {
                return fijk; // no overage is possible; h lies on this face
            }

            // if we're here we have the potential for an "overage"; i.e., it is
            // possible that c lies on an adjacent face
            var origIJK = fijk.Coord;

            // if we're in Class III, drop into the next finer Class II grid
            int res = h.Resolution;
            if(res.IsResClassIii())
            {
                // Class III
                fijk = fijk.ReplaceCoord(fijk.Coord.DownAp7R());
                res++;
            }

            // adjust for overage if needed
            // a pentagon base cell with a leading 4 digit requires special handling
            int pentLeading4 =
                baseCell.IsBaseCellPentagon() &&
                h.LeadingNonZeroDigit == Direction.I_AXES_DIGIT
                    ? 1
                    : 0;

            Overage overage;
            (overage, fijk) = fijk.AdjustOverageClassIi(res, pentLeading4, 0);
            if (overage != Overage.NO_OVERAGE)
            {
                // if the base cell is a pentagon we have the potential for secondary
                // overages
                if (baseCell.IsBaseCellPentagon())
                {
                    while (overage != Overage.NO_OVERAGE)
                    {
                        (overage, fijk) =
                            fijk.AdjustOverageClassIi(res, pentLeading4, 0);
                    }
                }

                if (res != h.Resolution)
                {
                    fijk = fijk.ReplaceCoord(fijk.Coord.UpAp7R());
                }
            }
            else if (res != h.Resolution)
            {
                fijk = fijk.ReplaceCoord(origIJK);
            }

            return fijk;
        }

        /// <summary>
        /// Find all icosahedron faces intersected by a given H3 index, represented
        /// as integers from 0-19. The array is sparse; since 0 is a valid value,
        /// invalid array values are represented as -1. It is the responsibility of
        /// the caller to filter out invalid values.
        /// </summary>
        /// <param name="h3">The H3 index</param>*
        /// <returns>Output list.</returns>
        /// <!--
        /// h3Index.c
        /// void H3_EXPORT(h3GetFaces)
        /// -->
        public static List<int> GetFaces(this H3Index h3)
        {
            int res = h3.Resolution;
            bool isPentagon = h3.IsPentagon();
            var results = new List<int>();
            
            // We can't use the vertex-based approach here for class II pentagons,
            // because all their vertices are on the icosahedron edges. Their
            // direct child pentagons cross the same faces, so use those instead.
            if (isPentagon && res.IsResClassIii())
            {
                // Note that this would not work for res 15, but this is only run on
                // Class II pentagons, it should never be invoked for a res 15 index.
                var childPentagon = h3.MakeDirectChild(0);
                return childPentagon.GetFaces();
            }

            // convert to FaceIJK
            var fijk = h3.ToFaceIjk();

            // Get all vertices as FaceIJK addresses. For simplicity, always
            // initialize the array with 6 verts, ignoring the last one for pentagons
            var fijkVerts = Enumerable.Range(1, Constants.NUM_HEX_VERTS)
                                      .Select(s => new FaceIjk()).ToList();

            int vertexCount = isPentagon
                                  ? Constants.NUM_PENT_VERTS
                                  : Constants.NUM_HEX_VERTS;

            
            if (isPentagon)
            {
                (_, int newRes, var vertexArray) = fijk.PentToVerts(res, fijkVerts);
                res = newRes;
                fijkVerts = vertexArray.ToList();
            }
            else
            {
                (_, int newRes, var vertexArray) = fijk.ToVerts(res, fijkVerts);
                res = newRes;
                fijkVerts = vertexArray.ToList();
            }

            // We may not use all of the slots in the output array,
            // so fill with invalid values to indicate unused slots
            int faceCount = h3.MaxFaceCount();
            for (var i = 0; i < faceCount; i++)
            {
                results.Add(StaticData.FaceIjk.InvalidFace);
            }

            // add each vertex face, using the output array as a hash set
            for (var i = 0; i < vertexCount; i++)
            {
                var vert = fijkVerts[i];

                // Adjust overage, determining whether this vertex is
                // on another face
                if (isPentagon)
                {
                    (_, vert) = vert.AdjustPentOverage(res);
                } else
                {
                    (_, vert) = vert.AdjustOverageClassIi(res, 0, 1);
                }

                // Save the face to the output array
                // Find the first empty output position, or the first position
                // matching the current face
                int pos = results.IndexOf(StaticData.FaceIjk.InvalidFace);
                results[pos] = vert.Face;
            }

            results.RemoveAll(r => r == StaticData.FaceIjk.InvalidFace);
            return results;
        }

        /// <summary>
        /// Returns the max number of possible icosahedron faces an H3 index
        /// may intersect.
        /// </summary>
        /// <param name="h3"></param>
        /// <returns></returns>
        /// <!--
        /// h3Index.c
        /// int H3_EXPORT(maxFaceCount)
        /// -->
        public static int MaxFaceCount(this H3Index h3)
        {
            // a pentagon always intersects 5 faces, a hexagon never intersects more
            // than 2 (but may only intersect 1)
            return h3.IsPentagon()
                       ? 5
                       : 2;
        }

        /// <summary>
        /// ToChildren takes the given hexagon id and generates all of the children
        /// at the specified resolution storing them into the provided memory pointer.
        /// It's assumed that maxH3ToChildrenSize was used to determine the allocation.
        /// </summary>
        /// <param name="h"> H3Index to find the children of</param>
        /// <param name="childRes"> int the child level to produce</param>
        /// <returns>The list of H3Index children</returns>
        /// <!--
        /// h3index.c
        /// -->
        public static List<H3Index> ToChildren(this H3Index h, int childRes)
        {
            var children = new List<H3Index>();
            int parentRes = h.Resolution;

            if (parentRes > childRes)
            {
                return children;
            }

            if (parentRes == childRes)
            {
                children.Add(h);
                return children;
            }

            int goalRes = childRes;
            int currentRes = parentRes;
            var current = new List<H3Index> {h};
            var realChildren = new List<H3Index>();
            while (currentRes < goalRes)
            {
                realChildren.Clear();
                foreach (var index in current)
                {
                    bool isPentagon = index.IsPentagon();
                    for (int m = 0; m < 7; m++)
                    {
                        if (isPentagon && m == (int) Direction.K_AXES_DIGIT)
                        {
                            realChildren.Add(StaticData.H3Index.H3_INVALID_INDEX);
                        }
                        else
                        {
                            var child = index.MakeDirectChild(m);
                            realChildren.Add(child);
                        }
                    }
                }

                current = new List<H3Index>(realChildren.Where(c => c.Value != StaticData.H3Index.H3_INVALID_INDEX));
                currentRes++;
            }

            return current;
        }

        /// <summary>
        /// ToCenterChild produces the center child index for a given H3 index at
        /// the specified resolution
        /// </summary>
        /// <param name="h">H3Index to find center child of</param>
        /// <param name="childRes">The resolution to switch to</param>
        /// <returns>
        /// H3Index of the center child, or H3_NULL if you actually asked for a parent
        /// </returns>
        public static H3Index ToCenterChild(this H3Index h, int childRes)
        {
            int parentRes = h.Resolution;
            if (!parentRes.IsValidChildRes(childRes))
            {
                return StaticData.H3Index.H3_NULL;
            }

            if (childRes == parentRes)
            {
                return h;
            }

            var child = h;
            child = child.SetResolution(childRes);
            
            for (int i = parentRes + 1; i <= childRes; i++)
            {
                child = child.SetIndexDigit(i, 0);
            }
            return child;
        }

        /// <summary>
        /// Determines the spherical coordinates of the center point of an H3 index.
        /// </summary>
        /// <param name="h3">The H3 index.</param>
        /// <returns>The spherical coordinates of the H3 cell center.</returns>
        /// <!--
        /// h3index.c
        /// void H3_EXPORT(h3ToGeo)
        /// -->
        public static GeoCoord ToGeoCoord(this H3Index h3)
        {
            return h3.ToFaceIjk().ToGeoCoord(h3.Resolution);
        }

        /// <summary>
        /// Determines the cell boundary in spherical coordinates for an H3 index.
        /// </summary>
        /// <param name="h3">The H3 index.</param>
        /// <returns>The boundary of the H3 cell in spherical coordinates.</returns>
        /// <!--
        /// h3index.c
        /// void H3_EXPORT(h3ToGeoBoundary)
        /// -->
        public static GeoBoundary ToGeoBoundary(this H3Index h3)
        {
            var fijk = h3.ToFaceIjk();
            
            var gb = h3.IsPentagon()
                     ? fijk.PentToGeoBoundary(h3.Resolution, 0, Constants.NUM_PENT_VERTS)
                     : fijk.ToGeoBoundary(h3.Resolution, 0, Constants.NUM_HEX_VERTS);

            return gb;
        }

        /// <summary>
        /// Get the number of CCW rotations of the cell's vertex numbers
        /// compared to the directional layout of its neighbors.
        /// </summary>
        /// <returns>Number of CCW rotations for the cell</returns>
        /// <!--
        /// vertex.c
        /// int vertexRotations
        /// -->
        public static int VertexRotations(this H3Index cell)
        {
            // Get the face and other info for the origin
            var fijk = cell.ToFaceIjk();
            int baseCell = cell.BaseCell;
            var cellLeadingDigit = (int) cell.LeadingNonZeroDigit;

            // get the base cell face
            var baseFijk = baseCell.ToFaceIjk();

            int ccwRot60 = baseCell.ToCounterClockwiseRotate60(fijk.Face);

            if (!baseCell.IsBaseCellPentagon())
            {
                return ccwRot60;
            }

            // Find the appropriate direction-to-face mapping
            int[] dirFaces = { };
            if (Vertex.PentagonDirectionFaces.Any(pd => pd.BaseCell == baseCell))
            {
                dirFaces = Vertex.PentagonDirectionFaces
                                 .First(pd => pd.BaseCell == baseCell).Faces;
            }

            // additional CCW rotation for polar neighbors or IK neighbors
            if (fijk.Face != baseFijk.Face &&
                (baseCell.IsBaseCellPentagon() ||
                 fijk.Face == dirFaces[(int)Direction.IK_AXES_DIGIT - Vertex.DIRECTION_INDEX_OFFSET])
                )
            {
                ccwRot60 = (ccwRot60 + 1) % 6;
            }

            ccwRot60 = cellLeadingDigit switch
            {
                // Check whether the cell crosses a deleted pentagon subsequence
                (int) Direction.JK_AXES_DIGIT when
                    fijk.Face == dirFaces[(int) Direction.IK_AXES_DIGIT - Vertex.DIRECTION_INDEX_OFFSET]
                    => (ccwRot60 + 5) % 6,
                (int) Direction.IK_AXES_DIGIT when
                    fijk.Face == dirFaces[(int) Direction.JK_AXES_DIGIT - Vertex.DIRECTION_INDEX_OFFSET]
                    => (ccwRot60 + 1) % 6,
                _ => ccwRot60
            };
            return ccwRot60;
        }

        /// <summary>
        /// Get the first vertex number for a given direction. The neighbor in this
        /// direction is located between this vertex number and the next number in
        /// sequence.
        /// </summary>
        /// <returns>
        /// The number for the first topological vertex, or INVALID_VERTEX_NUM
        /// if the direction is not valid for this cell
        /// </returns>
        /// <!--
        /// vertex.c
        /// int vertexNumForDirection
        /// -->
        public static int VertexNumForDirection(this H3Index origin, Direction direction)
        {
            bool isPentagon = origin.IsPentagon();
            // Check for invalid directions
            if (direction == Direction.CENTER_DIGIT ||
                direction >= Direction.INVALID_DIGIT ||
                (isPentagon && direction == Direction.K_AXES_DIGIT))
            {
                return Vertex.INVALID_VERTEX_NUM;
            }

            // Determine the vertex rotations for this cell
            int rotations = origin.VertexRotations();

            // Find the appropriate vertex, rotating CCW if necessary
            if (isPentagon)
            {
                return (Vertex.DirectionToVertexNumPent[(int) direction] +
                        Constants.NUM_PENT_VERTS - rotations) %
                       Constants.NUM_PENT_VERTS;
            }

            return (Vertex.DirectionToVertexNumHex[(int) direction] +
                    Constants.NUM_HEX_VERTS - rotations) %
                   Constants.NUM_HEX_VERTS;
        }

        /// <summary>
        /// Returns whether or not the provided H3Indexes are neighbors.
        /// 
        /// </summary>
        /// <param name="origin">The origin H3 index.</param>
        /// <param name="destination">The destination H3 index.</param>
        /// <returns>true if the indices are neighbors, false otherwise</returns>
        /// <!--
        /// hwUniEdge.c
        /// int H3_EXPORT(h3IndexesAreNeighbors)
        /// -->
        public static bool IsNeighborTo(this H3Index origin, H3Index destination)
        {
            // Make sure they're hexagon indexes
            if (origin.Mode!= H3Mode.Hexagon || origin.Mode!= H3Mode.Hexagon)
            {
                return false;
            }

            // Hexagons cannot be neighbors with themselves
            if (origin == destination)
            {
                return false;
            }

            // Only hexagons in the same resolution can be neighbors
            if (origin.Resolution != destination.Resolution)
            {
                return false;
            }

            // H3 Indexes that share the same parent are very likely to be neighbors
            // Child 0 is neighbor with all of its parent's 'offspring', the other
            // children are neighbors with 3 of the 7 children. So a simple comparison
            // of origin and destination parents and then a lookup table of the children
            // is a super-cheap way to possibly determine they are neighbors.
            int parentRes = origin.Resolution - 1;
            if (parentRes > 0 && (origin.ToParent(parentRes) == destination.ToParent(parentRes)))
            {
                var originResDigit = origin.GetIndexDigit(parentRes + 1);
                var destinationResDigit = destination.GetIndexDigit(parentRes + 1);

                if (originResDigit == Direction.CENTER_DIGIT ||
                    destinationResDigit == Direction.CENTER_DIGIT)
                {
                    return true;
                }

                // These sets are the relevant neighbors in the clockwise
                // and counter-clockwise
                Direction[] neighborSetClockwise =
                {
                    Direction.CENTER_DIGIT, Direction.JK_AXES_DIGIT, Direction.IJ_AXES_DIGIT,
                    Direction.J_AXES_DIGIT, Direction.IK_AXES_DIGIT, Direction.K_AXES_DIGIT,
                    Direction.I_AXES_DIGIT
                };
                Direction[] neighborSetCounterclockwise =
                {
                    Direction.CENTER_DIGIT, Direction.IK_AXES_DIGIT, Direction.JK_AXES_DIGIT,
                    Direction.K_AXES_DIGIT, Direction.IJ_AXES_DIGIT, Direction.I_AXES_DIGIT,
                    Direction.J_AXES_DIGIT
                };

                if (neighborSetClockwise[(int) originResDigit] == destinationResDigit ||
                    neighborSetCounterclockwise[(int) originResDigit] == destinationResDigit)
                {
                    return true;
                }
            }

            // Otherwise, we have to determine the neighbor relationship the "hard" way.
            var neighborRing = origin.KRing(1);

            // return if any match
            return neighborRing.Any(nr => nr == destination);
        }

        /// <summary>
        /// Returns a unidirectional edge H3 index based on the provided origin and destination
        /// </summary>
        /// <param name="origin">The origin H3 hexagon index</param>
        /// <param name="destination">The destination H3 hexagon index</param>
        /// <returns>The unidirectional edge H3Index, or H3_NULL on failure.</returns>
        /// <!--
        /// h3UniEdge.c
        /// H3Index H3_EXPORT(getH3UnidirectionalEdge)
        /// -->
        public static H3Index UniDirectionalEdgeTo(this H3Index origin, H3Index destination)
        {
            // Short-circuit and return an invalid index value if they are not neighbors
            if (!origin.IsNeighborTo(destination))
            {
                return StaticData.H3Index.H3_NULL;
            }

            // Otherwise, determine the IJK direction from the origin to the destination
            var output = origin.SetMode(H3Mode.UniEdge);

            //bool isPentagon = origin.IsPentagon();

            // Checks each neighbor, in order, to determine which direction the
            // destination neighbor is located. Skips CENTER_DIGIT since that
            // would be this index.
            //            for (var direction = isPentagon ? Direction.J_AXES_DIGIT : Direction.K_AXES_DIGIT;
            //                 direction <  Direction.NUM_DIGITS; direction++)
            for (Direction direction = Direction.K_AXES_DIGIT;
                 direction < Direction.NUM_DIGITS;
                 direction++)
            {
                {
                    // TODO: Circle back after retrofitting Algos.cs
                    int rotations = 0;
                    (H3Index neighbor, _) = origin.NeighborRotations(direction, rotations);
                    //Algos.h3NeighborRotations(origin,direction, ref rotations);
                    if (neighbor != destination)
                    {
                        continue;
                    }

                    return output.SetReservedBits((int) direction);
                }
            }

            // This should be impossible, return H3_NULL in this case;
            return StaticData.H3Index.H3_NULL;   
        }

        /// <summary>
        /// Returns the origin hexagon from the unidirectional edge H3Index
        /// </summary>
        /// <param name="edge">The edge H3 index</param>
        /// <returns>The origin H3 hexagon index, or H3_NULL on failure</returns>
        /// <!--
        /// h3UniEdge.c
        /// H3Index H3_EXPORT(getOriginH3IndexFromUnidirectionalEdge)
        /// -->
        public static H3Index OriginFromUniDirectionalEdge(this H3Index edge)
        {
            return edge.Mode != H3Mode.UniEdge
                       ? (H3Index) StaticData.H3Index.H3_NULL
                       : new H3Index(edge).SetMode(H3Mode.Hexagon).SetReservedBits(0);
        }

        /// <summary>
        /// Returns the destination hexagon from the unidirectional edge H3Index
        /// </summary>
        /// <param name="edge">The edge H3 index</param>
        /// <returns>
        /// The destination H3 hexagon index, or H3_NULL on failure
        /// </returns>
        /// <!--
        /// h3UniEdge.c
        /// H3Index H3_EXPORT(getDestinationH3IndexFromUnidirectionalEdge)
        /// -->
        public static H3Index DestinationFromUniDirectionalEdge(this H3Index edge)
        {
            if (edge.Mode != H3Mode.UniEdge)
            {
                return StaticData.H3Index.H3_NULL;
            }

            Direction direction = (Direction) edge.ReservedBits;
            int rotations = 0;
            (var destination, _) = edge.OriginFromUniDirectionalEdge()
                                       .NeighborRotations(direction, rotations);
            return destination;
        }

        /// <summary>
        /// Determines if the provided H3Index is a valid unidirectional edge index
        /// </summary>
        /// <param name="edge">The unidirectional edge H3Index</param>
        /// <returns>true if it is a unidirectional edge H3Index, otherwise false</returns>
        /// <!--
        /// h3UniEdge.c
        /// int H3_EXPORT(h3UnidirectionalEdgeIsValid)
        /// -->
        public static bool IsValidUniEdge(this H3Index edge)
        {
            if (edge.Mode != H3Mode.UniEdge)
            {
                return false;
            }

            Direction neighborDirection = (Direction) edge.ReservedBits;

            if (neighborDirection <= Direction.CENTER_DIGIT || neighborDirection >= Direction.NUM_DIGITS)
            {
                return false;
            }

            var origin = edge.OriginFromUniDirectionalEdge();
            //return (!origin.IsPentagon() || neighborDirection != Direction.K_AXES_DIGIT) && origin.IsValid();
            
            if(origin.IsPentagon() && neighborDirection == Direction.K_AXES_DIGIT)
            {
                return false;
            }

            return origin.IsValid();
        }

        /// <summary>
        /// Returns the origin, destination pair of hexagon IDs for the given edge ID
        /// </summary>
        /// <param name="edge">The unidirectional edge H3Index</param>
        /// <returns>Tuple containing origin and destination H#Index cells of edge</returns>
        /// <!--
        /// h3UniEdge.c
        /// void H3_EXPORT(getH3IndexesFromUnidirectionalEdge)
        /// -->
        public static (H3Index, H3Index) GetH3IndexesFromUniEdge(this H3Index edge)
        {
            return (edge.OriginFromUniDirectionalEdge(), edge.DestinationFromUniDirectionalEdge());
        }

        /// <summary>
        /// Returns the origin, destination pair of hexagon IDs for the given edge ID
        /// </summary>
        /// <param name="edge">The unidirectional edge H3Index</param>
        /// <returns>Tuple containing origin and destination H#Index cells of edge</returns>
        /// <!--
        /// h3UniEdge.c
        /// void H3_EXPORT(getH3IndexesFromUnidirectionalEdge)
        /// -->
        public static H3Index[] GetH3IndexesArrayFromUniEdge(this H3Index edge)
        {
            return new[] {edge.OriginFromUniDirectionalEdge(), edge.DestinationFromUniDirectionalEdge()};
        }

        /// <summary>
        /// Provides all of the unidirectional edges from the current H3Index.
        /// </summary>
        /// <param name="origin">The origin hexagon H3Index to find edges for.</param>
        /// <returns>List of edges</returns>
        /// <!--
        /// h3UniEdge.c
        /// void H3_EXPORT(getH3UnidirectionalEdgesFromHexagon)
        /// -->
        public static H3Index[] GetUniEdgesFromCell(this H3Index origin)
        {
            var results = new List<H3Index>();
            // Determine if the origin is a pentagon and special treatment needed.
            bool isPentagon = origin.IsPentagon();

            // This is actually quite simple. Just modify the bits of the origin
            // slightly for each direction, except the 'k' direction in pentagons,
            // which is zeroed.
            for (var i = 0; i < 6; i++)
            {
                switch (isPentagon)
                {
                    case true when i == 0:
                        results.Add(StaticData.H3Index.H3_NULL);
                        break;
                    default:
                        results.Add(new H3Index(origin).SetMode(H3Mode.UniEdge).SetReservedBits(1));
                        break;
                }
            }

            return results.ToArray();
        }

        /// <summary>
        /// Provides the coordinates defining the unidirectional edge.
        /// </summary>
        /// <param name="edge">The unidirectional edge H3Index</param>
        /// <returns>The geoboundary object to store the edge coordinates.</returns>
        /// <!--
        /// h3UniEdge.c
        /// void H3_EXPORT(getH3UnidirectionalEdgeBoundary)
        /// -->
        public static GeoBoundary UniEdgeToGeoBoundary(this H3Index edge)
        {
            // Get the origin and neighbor direction from the edge
            var direction = (Direction) edge.ReservedBits;
            var origin = edge.OriginFromUniDirectionalEdge();

            var gb = new GeoBoundary();
            
            // Get the start vertex for the edge
            int startVertex = origin.VertexNumForDirection(direction);
            if (startVertex == Vertex.INVALID_VERTEX_NUM)
            {
                // This is not actually an edge (i.e. no valid direction),
                // so return no vertices.
                gb.NumVerts = 0;
                return gb;
            }

            // Get the geo boundary for the appropriate vertexes of the origin. Note
            // that while there are always 2 topological vertexes per edge, the
            // resulting edge boundary may have an additional distortion vertex if it
            // crosses an edge of the icosahedron.
            var fijk = origin.ToFaceIjk();

            int res = origin.Resolution;
            bool isPentagon = origin.IsPentagon();

            return isPentagon
                     ? fijk.PentToGeoBoundary(res, startVertex, 2)
                     : fijk.ToGeoBoundary(res, startVertex, 2);
        }

        /// <summary>
        /// hexRange produces indexes within k distance of the origin index.
        ///
        /// Output behavior is undefined when one of the indexes returned by this
        /// function is a pentagon or is in the pentagon distortion area.
        ///
        /// k-ring 0 is defined as the origin index, k-ring 1 is defined as k-ring 0 and
        /// all neighboring indexes, and so on.
        ///
        /// Output is placed in the provided array in order of increasing distance from
        /// the origin.
        /// </summary>
        /// <param name="origin">Origin location.</param>
        /// <param name="k">k &gt;= 0</param>
        /// <returns>
        /// Tuple
        ///     Item1 - 0 if no pentagon or pentagonal distortion area was encountered.
        ///     Item2 - List of H3Index cells
        /// </returns>
        /// <!--
        /// algos.c
        /// int H3_EXPORT(hexRange)
        /// -->
        public static (int, List<H3Index>) HexRange(this H3Index origin, int k)
        {
            (int status, List<(H3Index, int)> values) = origin.HexRangeDistances(k);
            return (status, values.Select(v => v.Item1).ToList());
        }

        /// <summary>
        /// Produce cells within grid distance k of the origin cell.
        ///
        /// k-ring 0 is defined as the origin cell, k-ring 1 is defined as k-ring 0 and
        /// all neighboring cells, and so on.
        /// 
        /// Output is placed in the provided array in no particular order. Elements of
        /// the output array may be left zero, as can happen when crossing a pentagon.
        /// </summary>
        /// <param name="origin">origin cell</param>
        /// <param name="k">k >= 0</param>
        /// <returns>H3Index cells within range</returns>
        /// <!--
        /// algos.c
        /// void H3_EXPORT(kRing)
        /// -->
        public static List<H3Index> KRing(this H3Index origin, int k)
        {
            return origin.KRingDistances(k).Keys.ToList();
        }

        /// <summary>
        /// Produce cells and their distances from the given origin cell, up to
        /// distance k
        ///
        /// k-ring 0 is defined as the origin cell, k-ring 1 is defined as k-ring 0 and
        /// all neighboring cells, and so on.
        ///
        /// Output is placed in the provided array in no particular order. Elements of
        /// the output array may be left zero, as can happen when crossing a pentagon.
        /// </summary>
        /// <param name="origin">origin cell</param>
        /// <param name="k">k &gt;= 0</param>
        /// <returns>
        /// A dictionary with keys being the H3Index, and the value being the distance.
        /// </returns>
        /// <!--
        /// algos.c
        /// void H3_EXPORT(kRingDistances)
        /// -->
        public static Dictionary<H3Index, int> KRingDistances(this H3Index origin, int k)
        {
            // Optimistically try the faster hexRange algorithm first
            var testFaster = origin.HexRangeDistances(k);
            if (testFaster.Item1 == 0)
            {
                return testFaster.Item2.ToDictionary(i => i.Item1, i => i.Item2);
            }

            // Fast algo failed, fall back to slower, correct algo
            // and also wipe out array because contents untrustworthy

            var results = new Dictionary<H3Index, int>();
            return origin.KRingInternal(k, 0, results);
        }

        /// <summary>
        /// Internal helper function called recursively for kRingDistances.
        /// 
        /// Adds the origin cell to the output set (treating it as a hash set)
        /// and recurses to its neighbors, if needed.
        /// </summary>
        /// <param name="origin">Origin cell</param>
        /// <param name="k">Maximum distance to move from the origin</param>
        /// <param name="currentK">Current distance from the origin</param>
        /// <param name="outData">Dictionary passing information between recursions</param>
        /// <returns>
        /// Dictionary of cells
        /// Key - element either an H3Index or 0
        /// Value - indicate ijk distance from the origin cell to Item2
        /// </returns>
        /// <remarks>
        /// NOTE: You _should_ be able to just call this with h3.KRingInternal(k).  We'll see.
        /// </remarks>
        /// <!--
        /// algos.c
        /// void _kRingInternal
        /// -->
        public static Dictionary<H3Index, int> KRingInternal(this H3Index origin, int k, int currentK=0, Dictionary<H3Index, int> outData=null)
        {
            if (origin == 0)
            {
                return null;
            }

            outData ??= new Dictionary<H3Index, int>();
            var results = new Dictionary<H3Index, int>(outData);

            // If we already have the origin h3index in the dictionary, and
            // we have a shorter path known, let's get out instead of
            // processing this further.
            if (results.ContainsKey(origin) && results[origin] <= currentK)
            {
                return results;
            }

            results[origin] = currentK;

            // Base case: reached an index k away from the origin.
            if (currentK >= k)
            {
                return results;
            }
            
            var holdOrigin = origin;
            // Recurse to all neighbors in no particular order.
            for (var i = 0; i < 6; i++)
            {
                int rotations = 0;
                var (tempOrigin, _) = origin.NeighborRotations(Algos.Directions[i], rotations);

                var recurseResults = tempOrigin.KRingInternal(k, currentK + 1, results);
                if (recurseResults == null)
                {
                    continue;
                }
                foreach ((var key, int value) in recurseResults)
                {
                    results[key] = value;
                }
            }

            return results;
        }

        /// <summary>
        /// Returns the hexagon index neighboring the origin, in the direction dir.
        ///
        /// Implementation note: The only reachable case where this returns 0 is if the
        /// origin is a pentagon and the translation is in the k direction. Thus,
        /// 0 can only be returned if origin is a pentagon.
        /// </summary>
        /// <param name="origin">Origin index</param>
        /// <param name="dir">Direction to move in</param>
        /// <param name="rotations">
        /// Number of ccw rotations to perform to reorient the
        /// translation vector. Modified version Will be returned in tuple,
        /// so make sure it's reassigned upon return.  Return will be the
        /// new number of rotations to perform (such as when crossing a face edge.)
        /// </param>
        /// <returns>
        /// Tuple
        /// Item1 - see summary above
        /// Item2 - Modified rotation value
        /// </returns>
        /// <!--
        /// algos.c
        /// H3Index h3NeighborRotations
        /// -->
        public static (H3Index, int) NeighborRotations(this H3Index origin, Direction dir, int rotations)
        {
            H3Index outHex = new H3Index(origin);
            int outRotations = rotations;

            for (int i = 0; i < outRotations; i++)
            {
                dir = dir.Rotate60CounterClockwise();
            }

            int newRotations = 0;
            int oldBaseCell = outHex.BaseCell;
            Direction oldLeadingDigit = outHex.LeadingNonZeroDigit;

            // Adjust the indexing digits and, if needed, the base cell.
            int r = outHex.Resolution - 1;
            while (true)
            {
                if (r == -1)
                {
                    outHex =
                        outHex.SetBaseCell(StaticData.BaseCells.BaseCellNeighbors[oldBaseCell, (int) dir]);
                    newRotations =
                        StaticData.BaseCells.BaseCellNeighbor60CounterClockwiseRotation[oldBaseCell, (int) dir];
                    
                    if(outHex.BaseCell == StaticData.BaseCells.InvalidBaseCell)
                    {
                        // Adjust for the deleted k vertex at the base cell level.
                        // This edge actually borders a different neighbor.
                        outHex = outHex.SetBaseCell(
                            StaticData.BaseCells.BaseCellNeighbors[oldBaseCell, (int) Direction.IK_AXES_DIGIT]);

                        newRotations =
                            StaticData.BaseCells.BaseCellNeighbor60CounterClockwiseRotation
                                [oldBaseCell, (int) Direction.IK_AXES_DIGIT];

                        // perform the adjustment for the k-subsequence we're skipping
                        // over.
                        outHex=outHex.Rotate60CounterClockwise();
                        outRotations++;
                    }
                    break;
                }
                else
                {
                    Direction oldDigit = outHex.GetIndexDigit(r + 1);
                    Direction nextDir = Direction.CENTER_DIGIT;
                    
                    if((r+1).IsResClassIii())
                    {
                        outHex = outHex.SetIndexDigit
                            (r + 1, (ulong) Algos.NewDigitIi[(int) oldDigit, (int) dir]);
                        nextDir = Algos.NewAdjustmentIi[(int) oldDigit, (int) dir];
                    }
                    else
                    {
                        outHex = outHex.SetIndexDigit(r + 1, (ulong) Algos.NewDigitIii[(int) oldDigit, (int) dir]);
                        nextDir = Algos.NewAdjustmentIii[(int) oldDigit, (int) dir];
                    }

                    if (nextDir != Direction.CENTER_DIGIT)
                    {
                        dir = nextDir;
                        r--;
                    }
                    else
                    {
                        // No more adjustment to perform
                        break;
                    }
                }
            }

            int newBaseCell = outHex.BaseCell;
            if(newBaseCell.IsBaseCellPentagon())
            {
                var alreadyAdjustedKSubsequence = false;

                // force rotation out of missing k-axes sub-sequence
                if (outHex.LeadingNonZeroDigit == Direction.K_AXES_DIGIT)
                {
                    if (oldBaseCell != newBaseCell)
                    {
                        // in this case, we traversed into the deleted
                        // k subsequence of a pentagon base cell.
                        // We need to rotate out of that case depending
                        // on how we got here.
                        // check for a cw/ccw offset face; default is ccw
                        outHex = newBaseCell.IsClockwiseOffset
                                     (StaticData.BaseCells.BaseCellData[oldBaseCell].HomeFijk.Face)
                                     ? outHex.Rotate60Clockwise()
                                     : outHex.Rotate60CounterClockwise();
                        alreadyAdjustedKSubsequence = true;
                    }
                    else
                    {
                        // In this case, we traversed into the deleted
                        // k subsequence from within the same pentagon
                        // base cell.
                        if (oldLeadingDigit == Direction.CENTER_DIGIT)
                        {
                            // Undefined: the k direction is deleted from here
                            return (StaticData.H3Index.H3_NULL, outRotations);
                        }
                        else
                        if (oldLeadingDigit == Direction.JK_AXES_DIGIT)
                        {
                            // Rotate out of the deleted k subsequence
                            // We also need an additional change to the direction we're
                            // moving in
                            outHex = outHex.Rotate60CounterClockwise();
                            outRotations++;
                        }
                        else if (oldLeadingDigit == Direction.IK_AXES_DIGIT)
                        {
                            // Rotate out of the deleted k subsequence
                            // We also need an additional change to the direction we're
                            // moving in
                            outHex = outHex.Rotate60Clockwise();
                            outRotations += 5;
                        }
                        else
                        {
                            // Should never occur
                            return (StaticData.H3Index.H3_NULL, outRotations);
                        }
                    }
                }

                for (var i = 0; i < newRotations; i++)
                {
                    outHex = outHex.RotatePent60CounterClockwise();  
                } 

                // Account for differing orientation of the base cells (this edge
                // might not follow properties of some other edges.)
                if (oldBaseCell != newBaseCell)
                {
                    // TODO: Place this and related in H3LibExtensions
                    if (newBaseCell.IsBaseCellPolarPentagon())
                    {
                        // 'polar' base cells behave differently because they have all
                        // i neighbors.
                        if (oldBaseCell != 118 && oldBaseCell != 8 &&
                            outHex.LeadingNonZeroDigit!=Direction.JK_AXES_DIGIT)
                        {
                            outRotations++;
                        }
                    }
                    else if (outHex.LeadingNonZeroDigit == Direction.IK_AXES_DIGIT &&
                       !alreadyAdjustedKSubsequence)
                    {
                        // account for distortion introduced to the 5 neighbor by the
                        // deleted k subsequence.
                        outRotations++;
                    }
                }
            }
            else
            {
                for (int i = 0; i < newRotations; i++)
                {
                    outHex = outHex.Rotate60CounterClockwise();
                }
            }

            outRotations = (outRotations + newRotations) % 6;
            return (outHex, outRotations);
        }

        /// <summary>
        /// Produces indexes within k distance of the origin index.
        /// Output behavior is undefined when one of the indexes returned by this
        /// function is a pentagon or is in the pentagon distortion area.
        ///
        /// k-ring 0 is defined as the origin index, k-ring 1 is defined as k-ring 0 and
        /// all neighboring indexes, and so on.
        ///
        /// Output is placed in the provided array in order of increasing distance from
        /// the origin. The distances in hexagons is placed in the distances array at
        /// the same offset.
        /// </summary>
        /// <param name="origin">Origin location.</param>
        /// <param name="k">k &gt;= 0</param>
        /// <returns>
        /// Tuple with list of tuples
        /// Main tuple:
        ///     Item1 : status code
        ///     Item2 : List of tuples
        ///             Item1: H3Index
        ///             Item2: distance
        /// </returns>
        /// <!--
        /// algos.c
        /// int H3_EXPORT(hexRangeDistances)
        /// -->
        public static (int, List<(H3Index, int)>) HexRangeDistances(this H3Index origin, int k)
        {
            // Return codes:
            // 1 Pentagon was encountered
            // 2 Pentagon distortion (deleted k subsequence) was encountered
            // Pentagon being encountered is not itself a problem; really the deleted
            // k-subsequence is the problem, but for compatibility reasons we fail on
            // the pentagon.

            // k must be >= 0, so origin is always needed
            var results = new List<(H3Index, int)> {(origin, 0)};

            if (origin.IsPentagon())
            {
                return (Algos.HexRangePentagon, results);
            }

            // 0 < ring <= k, current ring
            int ring = 1;
            // 0 <= direction < 6, current side of the ring
            int direction = 0;
            // 0 <= i < ring, current position on the side of the ring
            int i = 0;
            // Number of 60 degree ccw rotations to perform on the direction (based on
            // which faces have been crossed.)
            int rotations = 0;

            while (ring <= k)
            {
                if (direction == 0 && i == 0)
                {
                    // Not putting in the output set as it will be done later, at
                    // the end of this ring.
                    (origin, rotations) = origin.NeighborRotations(Algos.NextRingDirection, rotations);

                    if (origin == 0)
                    {
                        // Should not be possible because `origin` would have to be a
                        // pentagon
                        return (Algos.HexRangeKSubsequence, results);
                    }

                    if(origin.IsPentagon())
                    {
                        // Pentagon was encountered; bail out as user doesn't want this.
                        return (Algos.HexRangePentagon, results);
                    }
                }

                (origin, rotations) = origin.NeighborRotations(Algos.Directions[direction], rotations);
                if (origin == 0)
                {
                    // Should not be possible because `origin` would have to be a
                    // pentagon
                    return (Algos.HexRangeKSubsequence, results);
                }

                results.Add((origin, ring));

                i++;
                // Check if end of this side of the k-ring
                if (i == ring)
                {
                    i = 0;
                    direction++;
                    // Check if end of this ring.
                    if (direction == 6)
                    {
                        direction = 0;
                        ring++;
                    }
                }

                if (origin.IsPentagon())
                {
                    // Pentagon was encountered; bail out as user doesn't want this.
                    return (Algos.HexRangePentagon, results);
                }
            }

            return (Algos.HexRangeSuccess, results);
        }

        /// <summary>
        /// Returns the "hollow" ring of hexagons at exactly grid distance k from
        /// the origin hexagon. In particular, k=0 returns just the origin hexagon.
        ///
        /// A nonzero failure code may be returned in some cases, for example,
        /// if a pentagon is encountered.
        ///
        /// Failure cases may be fixed in future versions.
        /// </summary>
        /// <param name="origin">Origin location.</param>
        /// <param name="k">k >= 0</param>
        /// <returns>
        /// Tuple
        ///     Item1 - Status: 0 if successful, other if failure
        ///     Item2 - List of h3index cells if status == 0, otherwise empty list 
        /// </returns>
        /// <!--
        /// algos.c
        /// int H3_EXPORT(hexRing)
        /// -->
        public static (int, List<H3Index>) HexRing(this H3Index origin, int k)
        {
            var results = new List<H3Index>();
            // Short-circuit on 'identity' ring
            if (k == 0)
            {
                return (0, new List<H3Index>{origin});
            }

            // Number of 60 degree ccw rotations to perform on the direction (based on
            // which faces have been crossed.)
            var rotations = 0;
            // Scratch structure for checking for pentagons
            if (origin.IsPentagon())
            {
                // Pentagon was encountered; bail out as user doesn't want this.
                return (Algos.HexRangePentagon, new List<H3Index>());
            }

            for (var ring = 0; ring < k; ring++)
            {
                (origin, rotations) = origin.NeighborRotations(Algos.NextRingDirection, rotations);

                if (origin == 0)
                {
                    // Should not be possible because `origin` would have to be a
                    // pentagon
                    return (Algos.HexRangeKSubsequence, new List<H3Index>());
                }

                if (origin.IsPentagon())
                {
                    return (Algos.HexRangePentagon, new List<H3Index>());
                }
            }

            var lastIndex = origin;
            results.Add(origin);
            
            for (var direction = 0; direction < 6; direction++)
            {
                for (var pos = 0; pos < k; pos++)
                {
                    (origin, rotations) = origin.NeighborRotations(Algos.Directions[direction], rotations);

                    if (origin == 0) 
                    {
                        // Should not be possible because `origin` would have to be a
                        // pentagon
                        return (Algos.HexRangeKSubsequence, new List<H3Index>());
                    }

                    // Skip the very last index, it was already added. We do
                    // however need to traverse to it because of the pentagonal
                    // distortion check, below.
                    if (pos != k - 1 || direction != 5)
                    {
                        results.Add(origin);
                        if (origin.IsPentagon())
                        {
                            return (Algos.HexRangePentagon, new List<H3Index>());
                        }
                    }
                }
            }

            // Check that this matches the expected lastIndex, if it doesn't,
            // it indicates pentagonal distortion occurred and we should report
            // failure.
            return lastIndex != origin
                       ? (Algos.HexRangePentagon, new List<H3Index>())
                       : (Algos.HexRangeSuccess, results);
        }

        /// <summary>
        /// returns the radius of a given hexagon in Km
        /// </summary>
        /// <param name="h3">the index of the hexagon</param>
        /// <returns>the radius of the hexagon in Km</returns>
        /// <!--
        /// bbox.c
        /// double _hexRadiusKm
        /// -->
        public static double HexRadiusKm(this H3Index h3)
        {
            // There is probably a cheaper way to determine the radius of a
            // hexagon, but this way is conceptually simple
            var h3Center = h3.ToGeoCoord();
            var h3Boundary = h3.ToGeoBoundary();
            return h3Center.DistanceToKm(h3Boundary.Verts.First());
        }
        
        /// <summary>
        /// Lets you get the maxUncompactSize from a single cell instead of
        /// requiring wrapping it in a List
        /// </summary>
        /// <param name="singleCell">Cell that will be uncompacted</param>
        /// <param name="res">resolution to uncompact to</param>
        /// <returns>How many hexagons to expect</returns>
        public static long MaxUncompactSize(this H3Index singleCell, int res)
        {
            return new List<H3Index> {singleCell}.MaxUncompactSize(res);
        }

        /// <summary>
        /// Run uncompact on a single cell
        /// </summary>
        /// <param name="singleCell">cell to uncompact</param>
        /// <param name="res">resolution to uncompact to</param>
        /// <returns><see cref="CollectionExtensions.Uncompact"/> for details</returns>
        public static (int, List<H3Index>) Uncompact(this H3Index singleCell, int res)
        {
            return new List<H3Index> {singleCell}.Uncompact(res);
        }

        
        //---------------------------------------------------------------------------
        //  Field "Modifiers" - This is not going to be fun

        public static H3Index SetResolution(this H3Index h3Index, int resolution)
        {
            return (h3Index & StaticData.H3Index.H3_RES_MASK_NEGATIVE) |
                   ((ulong) resolution << StaticData.H3Index.H3_RES_OFFSET);
        }
        
        public static H3Index SetBaseCell(this H3Index cell, int baseCell)
        {
            return (cell & StaticData.H3Index.H3_BC_MASK_NEGATIVE) |
                   ((ulong)baseCell << StaticData.H3Index.H3_BC_OFFSET);
            
        }

        public static H3Index SetMode(this H3Index cell, H3Mode mode)
        {
            return cell  & StaticData.H3Index.H3_MODE_MASK_NEGATIVE |
                   ((ulong)mode << StaticData.H3Index.H3_MODE_OFFSET);
        }

        public static H3Index SetHighBit(this H3Index cell, int value)
        {
            return  (cell & StaticData.H3Index.H3_HIGH_BIT_MASK_NEGATIVE) |
                    ((ulong) value << StaticData.H3Index.H3_MAX_OFFSET);
        }

        public static H3Index SetReservedBits(this H3Index cell, int value)
        {
            return  (cell & StaticData.H3Index.H3_RESERVED_MASK_NEGATIVE) | ((ulong) value << StaticData.H3Index.H3_RESERVED_OFFSET);
        }

        public static H3Index SetIndexDigit(this H3Index cell, int res, ulong digit)
        {
            return  (cell & ~(StaticData.H3Index.H3_DIGIT_MASK << ((Constants.MAX_H3_RES - res) * StaticData.H3Index.H3_PER_DIGIT_OFFSET))) |
                      (digit << (Constants.MAX_H3_RES - res) * StaticData.H3Index.H3_PER_DIGIT_OFFSET);
        }
    }
}
