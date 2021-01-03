using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace H3Lib.Extensions
{
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
        public static double CellAreaKm2(this H3Index h)
        {
            return h.CellAreaRadians2() * Constants.EARTH_RADIUS_KM * Constants.EARTH_RADIUS_KM;
        }
        
        /// <summary>
        /// Area of H3 cell in meters^2.
        /// </summary>
        /// <param name="h">h3 cell</param>
        public static double CellAreaM2(this H3Index h)
        {
            return h.CellAreaKm2() * 1000 * 1000;
        }

        /// <summary>
        /// Length of a unidirectional edge in radians.
        /// </summary>
        /// <param name="edge">H3 unidirectional edge</param>
        /// <returns>length in radians</returns>
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
        public static double ExactEdgeLengthKm(this H3Index edge)
        {
            return edge.ExactEdgeLengthRads() * Constants.EARTH_RADIUS_KM;
        }

        /// <summary>
        /// Length of a unidirectional edge in meters.
        /// </summary>
        /// <param name="edge">H3 unidirectional edge</param>
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

            int originOnPent = BaseCells._isBaseCellPentagon(originBaseCell)
                                   ? 1
                                   : 0;
            int indexOnPent = BaseCells._isBaseCellPentagon(baseCell)
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
        public static H3Index Rotate60Clockwise(this H3Index h)
        {
            for (int r = 1, res = h.Resolution; r <= res; r++)
            {
                var oldDigit = h.GetIndexDigit(r);
                h.SetIndexDigit(r, (ulong) oldDigit.Rotate60Clockwise());
            }
            return h;
        }

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

    }
}
