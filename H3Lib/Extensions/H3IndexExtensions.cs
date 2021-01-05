using System;
using System.Collections.Generic;
using System.Linq;

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
            var c = new GeoCoord();
            var gb = new GeoBoundary();
            H3Index.h3ToGeo(cell, ref c);
            H3Index.h3ToGeoBoundary(cell, ref gb);

            var area = 0.0;
            for (var i = 0; i < gb.numVerts; i++)
            {
                int j = (i + 1) % gb.numVerts;
                area += GeoCoord.TriangleArea(gb.verts[i], gb.verts[j], c);
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
            return h.CellAreaRadians2() * Constants.EARTH_RADIUS_KM * Constants.EARTH_RADIUS_KM;
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
            var gb = new GeoBoundary();
            H3UniEdge.getH3UnidirectionalEdgeBoundary(edge, ref gb);

            var length = 0.0;
            for (var i = 0; i < gb.numVerts - 1; i++)
            {
                length += gb.verts[i].DistanceToRadians(gb.verts[i + 1]);
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
            Direction dir = Direction.CENTER_DIGIT;
            Direction revDir = Direction.CENTER_DIGIT;
            if (originBaseCell != baseCell)
            {
                dir = BaseCells._getBaseCellDirection(originBaseCell, baseCell);
                if (dir == Direction.INVALID_DIGIT)
                {
                    // Base cells are not neighbors, can't unfold.
                    return (2, new CoordIjk());
                }
                revDir = BaseCells._getBaseCellDirection(baseCell, originBaseCell);
                if (revDir == Direction.INVALID_DIGIT)
                {
                    throw new Exception("assert(revDir != INVALID_DIGIT);");
                }
            }

            int originOnPent = BaseCells.IsBaseCellPentagon(originBaseCell)
                                   ? 1
                                   : 0;
            int indexOnPent = BaseCells.IsBaseCellPentagon(baseCell)
                                  ? 1
                                  : 0;

            var indexFijk = new FaceIjk();

            if (dir != Direction.CENTER_DIGIT)
            {
                // Rotate index into the orientation of the origin base cell.
                // cw because we are undoing the rotation into that base cell.
                int baseCellRotations = BaseCells.baseCellNeighbor60CCWRots[originBaseCell,(int)dir];
                if (indexOnPent == 1)
                {
                    for (int i = 0; i < baseCellRotations; i++)
                    {
                        h3 = h3.RotatePent60Clockwise();
                        revDir = revDir.Rotate60Clockwise();

                        if (revDir == Direction.K_AXES_DIGIT)
                        {
                            revDir = revDir.Rotate60CounterClockwise();
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
                    if (LocalIj.FAILED_DIRECTIONS[originLeadingDigit, (int) dir])
                    {
                        // TODO: We may be unfolding the pentagon incorrectly in this
                        // case; return an error code until this is guaranteed to be
                        // correct.
                        return (3,new CoordIjk());
                    }

                    directionRotations = LocalIj.PENTAGON_ROTATIONS[originLeadingDigit,(int)dir];
                    pentagonRotations = directionRotations;
                }
                else if (indexOnPent == 1)
                {
                    int indexLeadingDigit = (int) h3.LeadingNonZeroDigit;

                    if (LocalIj.FAILED_DIRECTIONS[indexLeadingDigit,(int)revDir])
                    {
                        // TODO: We may be unfolding the pentagon incorrectly in this
                        // case; return an error code until this is guaranteed to be
                        // correct.
                        return (4, new CoordIjk());
                    }

                    pentagonRotations = LocalIj.PENTAGON_ROTATIONS[(int)revDir, indexLeadingDigit];
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

                if (LocalIj.FAILED_DIRECTIONS[originLeadingDigit, indexLeadingDigit])
                {
                    // TODO: We may be unfolding the pentagon incorrectly in this case;
                    // return an error code until this is guaranteed to be correct.
                    return (5, new CoordIjk());
                }

                int withinPentagonRotations =
                    LocalIj.PENTAGON_ROTATIONS[originLeadingDigit,indexLeadingDigit];

                for (var i = 0; i < withinPentagonRotations; i++)
                {
                    indexFijk = new FaceIjk(indexFijk.Face, indexFijk.Coord.Rotate60Clockwise());
                }
            }
        
            return (0, indexFijk.Coord);
        }

        public static H3Index RotatePent60CounterClockwise(this H3Index h)
        {
            // rotate in place; skips any leading 1 digits (k-axis)
            var foundFirstNonZeroDigit = 0;
            for (int r = 1, res = h.Resolution; r <= res; r++)
            {
                // rotate this digit
                h.SetIndexDigit(r, (ulong) h.GetIndexDigit(r).Rotate60CounterClockwise());

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
                h.SetIndexDigit(r, (ulong) h.GetIndexDigit(r).Rotate60Clockwise());

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
                Direction oldDigit = h.GetIndexDigit(r);
                h.SetIndexDigit(r, (ulong) oldDigit.Rotate60CounterClockwise());
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
                h.SetIndexDigit(r, (ulong) oldDigit.Rotate60Clockwise());
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
                ijk = h.IsResClassIii(r)
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
                    if (baseCell.IsBaseCellPentagon() && digit == Direction.K_AXES_DIGIT)
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
                return H3Index.H3_NULL;
            }

            if (parentRes == childRes)
            {
                return h;
            }

            if (parentRes < 0 || parentRes > Constants.MAX_H3_RES)
            {
                return H3Index.H3_NULL;
            }

            var parentH = new H3Index(h) {Resolution = parentRes};
            for (int i = parentRes + 1; i <= childRes; i++)
            {
                parentH.SetIndexDigit(i, H3Index.H3_DIGIT_MASK);
            }
            return parentH;
        }
     
        /// <summary>
        /// maxH3ToChildrenSize returns the maximum number of children possible for a
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
            H3Index h = H3Index.H3_INIT;
            h.Mode = H3Mode.Hexagon;
            h.Resolution = res;
            h.BaseCell = baseCell;

            for (var r = 1; r <= res; r++)
            {
                h.SetIndexDigit(r, (ulong) initDigit);
            }

            hp = h;
            return hp;
        }

        /// <summary>
        /// Takes an H3Index and determines if it is actually a pentagon.
        /// </summary>
        /// <param name="h"> The H3Index to check.</param>
        /// <returns>Returns true if it is a pentagon, otherwise false.</returns>
        /// <!--
        /// h3Index.c
        /// int H3_EXPORT(h3IsPentagon)
        public static bool IsPentagon(this H3Index h)
        {
            return BaseCells.IsBaseCellPentagon(h.BaseCell) &&
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
            childH.Resolution = childRes;
            childH.SetIndexDigit(childRes, (ulong) cellNumber);
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
            if (baseCell.IsBaseCellPentagon() && h.LeadingNonZeroDigit == Direction.IK_AXES_DIGIT)
            {
                h = h.Rotate60Clockwise();
            }

           
            // start with the "home" face and ijk+ coordinates for the base cell of c
            var fijk = new FaceIjk(BaseCells.baseCellData[baseCell].homeFijk);
            (int result, var faceIjk) = h.ToFaceIjkWithInitializedFijk(fijk);
            if (result == 0)
            {
                return faceIjk; // no overage is possible; h lies on this face
            }

            // if we're here we have the potential for an "overage"; i.e., it is
            // possible that c lies on an adjacent face
            var origIJK = new CoordIjk(fijk.Coord);

            // if we're in Class III, drop into the next finer Class II grid
            int res = h.Resolution;
            if(res.IsResClassIii())
            {
                // Class III
                fijk = new FaceIjk(fijk.Face, fijk.Coord.DownAp7R());
                res++;
            }

            // adjust for overage if needed
            // a pentagon base cell with a leading 4 digit requires special handling
            int pentLeading4 =
                (baseCell.IsBaseCellPentagon() && h.LeadingNonZeroDigit == Direction.I_AXES_DIGIT)
                    ? 1
                    : 0;

            var test = fijk.AdjustOverageClassIi(res, pentLeading4, 0);
            
            if (test.Item1 != Overage.NO_OVERAGE)
            {
                // if the base cell is a pentagon we have the potential for secondary
                // overages
                if (baseCell.IsBaseCellPentagon())
                {
                    while (fijk.AdjustOverageClassIi(res,0,0).Item1 != Overage.NO_OVERAGE)
                    {
                        continue;
                    }
                }

                if (res != h.Resolution)
                {
                    var fCoord = new CoordIjk(fijk.Coord).UpAp7R();
                    fijk = new FaceIjk(fijk.Face, fCoord);
                }
            } else if (res != h.Resolution)
            {
                fijk = new FaceIjk(fijk.Face, origIJK);
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
                (var fijkOut, int newRes, var vertexArray) = fijk.PentToVerts(res, fijkVerts);
                fijk = fijkOut;
                res = newRes;
                fijkVerts = vertexArray.ToList();
            }
            else
            {
                (var fijkOut, int newRes, var vertexArray) = fijk.ToVerts(res, fijkVerts);
                fijk = fijkOut;
                res = newRes;
                fijkVerts = vertexArray.ToList();
            }

            // We may not use all of the slots in the output array,
            // so fill with invalid values to indicate unused slots
            int faceCount = h3.MaxFaceCount();
            for (var i = 0; i < faceCount; i++)
            {
                results.Add(FaceIjk.INVALID_FACE);
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
                int face = vert.Face;
                // Find the first empty output position, or the first position
                // matching the current face
                int pos = results.IndexOf(FaceIjk.INVALID_FACE);
                results[pos] = vert.Face;
            }

            results.RemoveAll(r => r == FaceIjk.INVALID_FACE);
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

    }
}
