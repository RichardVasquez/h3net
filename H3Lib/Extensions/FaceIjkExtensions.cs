using System;
using System.Collections.Generic;

namespace H3Lib.Extensions
{
    /// <summary>
    /// Operations for FaceIjk type
    /// </summary>
    public static class FaceIjkExtensions
    {
        /// <summary>
        /// Quick replacement of Face value
        /// </summary>
        /// <param name="fijk">FaceIjk to replace Face value of</param>
        /// <param name="face">new Face value to slot in</param>
        /// <returns>A new instance with the correct values</returns>
        public static FaceIjk ReplaceFace(this FaceIjk fijk, int face)
        {
            return new FaceIjk(face, fijk.Coord);
        }

        /// <summary>
        /// Quick replacement of Coord value
        /// </summary>
        /// <param name="fijk">FaceIjk to replace Coord value of</param>
        /// <param name="coord">New CoordIjk to slot in</param>
        /// <returns>A new instance with the correct values</returns>
        public static FaceIjk ReplaceCoord(this FaceIjk fijk, CoordIjk coord)
        {
            return new FaceIjk(fijk.Face, coord);
        }

        /// <summary>
        /// Adjusts a FaceIJK address in place so that the resulting cell address is
        /// relative to the correct icosahedral face.
        /// </summary>
        /// <param name="fijk">The FaceIJK address of the cell.</param>
        /// <param name="res">The H3 resolution of the cell.</param>
        /// <param name="pentLeading4">Whether or not the cell is a pentagon with a leading figit 4</param>
        /// <param name="substrate">Whether or not the cell is in a substrate grid.</param>
        /// <returns>
        /// Tuple
        /// Item1: <see cref="Overage"/>
        /// Item2: Adjusted <see cref="FaceIjk"/>
        /// </returns>
        /// <!--
        /// faceijk.c
        /// Overage _adjustOverageClassII
        /// -->
        public static (Overage, FaceIjk) AdjustOverageClassIi(
                this FaceIjk fijk, int res, int pentLeading4, int substrate
            )
        {
            Overage overage = Overage.NO_OVERAGE;

            var ijk = fijk.Coord;

            // get the maximum dimension value; scale if a substrate grid
            int maxDim = Constants.FaceIjk.MaxDimByCiiRes[res];
            if (substrate != 0)
            {
                maxDim *= 3;
            }

            // check for overage
            if (substrate != 0 && ijk.Sum() == maxDim) // on edge
            {
                overage = Overage.FACE_EDGE;
            }
            else if (ijk.Sum() > maxDim)  // overage
            {
                overage = Overage.NEW_FACE;

                FaceOrientIjk fijkOrient;
                if (ijk.K > 0)
                {
                    if (ijk.J > 0) // jk "quadrant"
                    {
                        fijkOrient = Constants.FaceIjk.FaceNeighbors[fijk.Face, Constants.FaceIjk.JK];
                    }
                    else // ik "quadrant"
                    {
                        fijkOrient = Constants.FaceIjk.FaceNeighbors[fijk.Face, Constants.FaceIjk.KI];
                        // adjust for the pentagonal missing sequence
                        if (pentLeading4 != 0)
                        {
                            // translate origin to center of pentagon
                            var origin = new CoordIjk(maxDim, 0, 0);
                            var tmp = ijk - origin;
                            // rotate to adjust for the missing sequence
                            tmp = tmp.Rotate60Clockwise();
                            // translate the origin back to the center of the triangle
                            ijk = tmp + origin;
                        }
                    }
                }
                else // ij "quadrant"
                {
                    fijkOrient = Constants.FaceIjk.FaceNeighbors[fijk.Face, Constants.FaceIjk.IJ];
                }

                fijk = fijk.ReplaceFace(fijkOrient.Face);

                // rotate and translate for adjacent face
                for (int i = 0; i < fijkOrient.Ccw60Rotations; i++)
                {
                    ijk = ijk.Rotate60CounterClockwise();
                }

                var transVec = fijkOrient.Translate;
                int unitScale = Constants.FaceIjk.UnitScaleByCiiRes[res];
                if (substrate != 0)
                {
                    unitScale *= 3;
                }

                transVec *= unitScale;
                ijk += transVec;
                ijk = ijk.Normalized();

                // overage points on pentagon boundaries can end up on edges
                if (substrate != 0 && ijk.Sum() == maxDim) // on edge
                {
                    overage = Overage.FACE_EDGE;
                }
            }

            fijk = fijk.ReplaceCoord(ijk);
            return (overage, fijk);
        }

        /// <summary>
        /// Adjusts a FaceIJK address for a pentagon vertex in a substrate grid in
        /// place so that the resulting cell address is relative to the correct
        /// icosahedral face.
        /// </summary>
        /// <param name="fijk">The FaceIJK address of the cell.</param>
        /// <param name="res">The H3 resolution of the cell.</param>
        /// <!--
        /// faceIjk.c
        /// Overage _adjustPentVertOverage
        /// -->
        public static (Overage, FaceIjk) AdjustPentOverage(this FaceIjk fijk, int res)
        {
            const int pentLeading4 = 0;
            Overage overage;
            do
            {
                (overage, fijk) = fijk.AdjustOverageClassIi(res, pentLeading4, 1);

            } while (overage == Overage.NEW_FACE);

            return (overage, fijk);
        }

        /// <summary>
        /// Get the vertices of a pentagon cell as substrate FaceIJK addresses
        /// </summary>
        /// <param name="fijk">The FaceIJK address of the cell.</param>
        /// <param name="res">
        /// The H3 resolution of the cell. This may be adjusted if
        /// necessary for the substrate grid resolution.
        /// </param>
        /// <param name="fijkVerts">array for the vertices</param>
        /// <returns>
        /// Tuple
        /// Item1 Possibly modified fijk
        /// Item2 Possibly modified res
        /// Item3 Array for vertices
        /// </returns>
        /// <!--
        /// faceijk.c
        /// void _faceIjkPentToVerts
        /// -->
        public static (FaceIjk, int, IList<FaceIjk>) PentToVerts(this FaceIjk fijk, int res, IList<FaceIjk> fijkVerts)
        {
            // the vertexes of an origin-centered pentagon in a Class II resolution on a
            // substrate grid with aperture sequence 33r. The aperture 3 gets us the
            // vertices, and the 3r gets us back to Class II.
            // vertices listed ccw from the i-axes
            CoordIjk[] vertsCii =
            {
                new CoordIjk(2, 1, 0), // 0
                new CoordIjk(1, 2, 0), // 1
                new CoordIjk(0, 2, 1), // 2
                new CoordIjk(0, 1, 2), // 3
                new CoordIjk(1, 0, 2), // 4
            };

            // the vertexes of an origin-centered pentagon in a Class III resolution on
            // a substrate grid with aperture sequence 33r7r. The aperture 3 gets us the
            // vertices, and the 3r7r gets us to Class II. vertices listed ccw from the
            // i-axes
            CoordIjk[] vertsCiii =
            {
                new CoordIjk(5, 4, 0), // 0
                new CoordIjk(1, 5, 0), // 1
                new CoordIjk(0, 5, 4), // 2
                new CoordIjk(0, 1, 5), // 3
                new CoordIjk(4, 0, 5), // 4
            };

            // get the correct set of substrate vertices for this resolution
            var verts = res.IsResClassIii()
                            ? vertsCiii
                            : vertsCii;

            // adjust the center point to be in an aperture 33r substrate grid
            // these should be composed for speed
            var tempCoord = fijk.Coord.DownAp3().DownAp3R();
            //fijk = fijk.ReplaceCoord(fijk.Coord.DownAp3().DownAp3R());

            // if res is Class III we need to add a cw aperture 7 to get to
            // icosahedral Class II
            if (res.IsResClassIii())
            {
                tempCoord = tempCoord.DownAp7R();
                res++;
            }

            fijk = fijk.ReplaceCoord(tempCoord);

            // The center point is now in the same substrate grid as the origin
            // cell vertices. Add the center point substrate coordinates
            // to each vertex to translate the vertices to that cell.
            for (var v = 0; v < Constants.H3.NUM_PENT_VERTS; v++)
            {
                int newFace = fijk.Face;
                var newCoord = (fijk.Coord + verts[v]).Normalized();
                fijkVerts[v] = new FaceIjk(newFace, newCoord);
            }

            return (fijk, res, fijkVerts);
        }

        /// <summary>
        /// Get the vertices of a cell as substrate FaceIJK addresses
        /// </summary>
        /// <param name="fijk">The FaceIJK address of the cell.</param>
        /// <param name="res">
        /// The H3 resolution of the cell. This may be adjusted if
        /// necessary for the substrate grid resolution.
        /// </param>
        /// <param name="fijkVerts">array for the vertices</param>
        /// <returns>
        /// Tuple
        /// Item1 Possibly modified fijk
        /// Item2 Possibly modified res
        /// Item3 Array for vertices
        /// </returns>
        /// <!--
        /// faceijk.c
        /// void _faceIjkToVerts
        /// -->
        public static (FaceIjk, int, IList<FaceIjk>) ToVerts(this FaceIjk fijk, int res, IList<FaceIjk> fijkVerts)
        {
            // the vertexes of an origin-centered cell in a Class II resolution on a
            // substrate grid with aperture sequence 33r. The aperture 3 gets us the
            // vertices, and the 3r gets us back to Class II.
            // vertices listed ccw from the i-axes
            CoordIjk[] vertsCii =
            {
                new CoordIjk(2, 1, 0), // 0
                new CoordIjk(1, 2, 0), // 1
                new CoordIjk(0, 2, 1), // 2
                new CoordIjk(0, 1, 2), // 3
                new CoordIjk(1, 0, 2), // 4
                new CoordIjk(2, 0, 1)  // 5
            };

            // the vertexes of an origin-centered cell in a Class III resolution on a
            // substrate grid with aperture sequence 33r7r. The aperture 3 gets us the
            // vertices, and the 3r7r gets us to Class II.
            // vertices listed ccw from the i-axes
            CoordIjk[] vertsCiii =
            {
                new CoordIjk(5, 4, 0), // 0
                new CoordIjk(1, 5, 0), // 1
                new CoordIjk(0, 5, 4), // 2
                new CoordIjk(0, 1, 5), // 3
                new CoordIjk(4, 0, 5), // 4
                new CoordIjk(5, 0, 1)  // 5
            };

            // get the correct set of substrate vertices for this resolution
            var verts = res.IsResClassIii()
                            ? vertsCiii
                            : vertsCii;

            // adjust the center point to be in an aperture 33r substrate grid
            // these should be composed for speed
            fijk = fijk.ReplaceCoord(fijk.Coord.DownAp3().DownAp3R());

            // if res is Class III we need to add a cw aperture 7 to get to
            // icosahedral Class II
            if (res.IsResClassIii())
            {
                fijk = fijk.ReplaceCoord(fijk.Coord.DownAp7R());
                res++;
            }

            // The center point is now in the same substrate grid as the origin
            // cell vertices. Add the center point substrate coordinates
            // to each vertex to translate the vertices to that cell.
            for (int v = 0; v < Constants.H3.NUM_HEX_VERTS; v++)
            {
                fijkVerts[v] = fijkVerts[v]
                              .ReplaceFace(fijk.Face)
                              .ReplaceCoord((fijk.Coord + verts[v]).Normalized());
            }

            return (fijk, res, fijkVerts);
        }

        /// <summary>
        /// Convert an FaceIJK address to the corresponding H3Index.
        /// </summary>
        /// <param name="fijk">The FaceIJK address.</param>
        /// <param name="res">The cell resolution.</param>
        /// <returns>The encoded H3Index (or H3_NULL on failure).</returns>
        /// <!--
        /// h3index.c
        /// H3Index _faceIjkToH3
        /// -->
        public static H3Index ToH3(this FaceIjk fijk, int res)
        {
            // initialize the index
            H3Index h = Constants.H3Index.H3_INIT;
            h = h.SetMode(H3Mode.Hexagon).SetResolution(res);

            // check for res 0/base cell
            if (res == 0)
            {
                if (fijk.Coord.I > Constants.BaseCells.MaxFaceCoord ||
                    fijk.Coord.J > Constants.BaseCells.MaxFaceCoord ||
                    fijk.Coord.K > Constants.BaseCells.MaxFaceCoord)
                {
                    // out of range input
                    return Constants.H3Index.H3_INVALID_INDEX;
                }

                return h.SetBaseCell(fijk.ToBaseCell());
            }

            // we need to find the correct base cell FaceIJK for this H3 index;
            // start with the passed in face and resolution res ijk coordinates
            // in that face's coordinate system
            var fijkBc = new FaceIjk(fijk);

            // build the H3Index from finest res up
            // adjust r for the fact that the res 0 base cell offsets the indexing
            // digits
            var ijk = new CoordIjk(fijkBc.Coord);
            for (int r = res - 1; r >= 0; r--)
            {
                var lastIjk = new CoordIjk(ijk);
                CoordIjk lastCenter;
                if ((r + 1).IsResClassIii())
                {
                    // rotate ccw
                    ijk = ijk.UpAp7();
                    lastCenter = new CoordIjk(ijk).DownAp7();
                }
                else
                {
                    // rotate cw
                    ijk = ijk.UpAp7R();
                    lastCenter = new CoordIjk(ijk).DownAp7R();
                }

                var diff = (lastIjk - lastCenter).Normalized();
                h = h.SetIndexDigit(r + 1, (ulong) diff.ToDirection());
            }

            fijkBc = fijkBc.ReplaceCoord(ijk);

            // fijkBC should now hold the IJK of the base cell in the
            // coordinate system of the current face
            if (fijkBc.Coord.I > Constants.BaseCells.MaxFaceCoord ||
                fijkBc.Coord.J > Constants.BaseCells.MaxFaceCoord ||
                fijkBc.Coord.K > Constants.BaseCells.MaxFaceCoord)
            {
                // out of range input
                return Constants.H3Index.H3_INVALID_INDEX;
            }

            // lookup the correct base cell
            int baseCell = fijkBc.ToBaseCell();
            h = h.SetBaseCell(baseCell);

            // rotate if necessary to get canonical base cell orientation
            // for this base cell
            int numRots = fijkBc.ToBaseCellCounterClockwiseRotate60();
            if (baseCell.IsBaseCellPentagon())
            {
                // force rotation out of missing k-axes sub-sequence
                if (h.LeadingNonZeroDigit == Direction.K_AXES_DIGIT)
                {
                    // check for a cw/ccw offset face; default is ccw
                    h = baseCell.IsClockwiseOffset(fijkBc.Face)
                            ? h.Rotate60Clockwise()
                            : h.Rotate60CounterClockwise();
                }

                for (var i = 0; i < numRots; i++)
                {
                    h = h.RotatePent60CounterClockwise();
                }
            }
            else
            {
                for (int i = 0; i < numRots; i++)
                {
                    h = h.Rotate60CounterClockwise();
                }
            }

            return h;
        }

        /// <summary>
        /// Find base cell given FaceIJK.
        ///
        /// Given the face number and a resolution 0 ijk+ coordinate in that face's
        /// face-centered ijk coordinate system, return the base cell located at that
        /// coordinate.
        ///
        /// Valid ijk+ lookup coordinates are from (0, 0, 0) to (2, 2, 2).
        /// </summary>
        /// <!--
        /// baseCells.c
        /// int _faceIjkToBaseCell
        /// -->
        public static int ToBaseCell(this FaceIjk h)
        {
            return Constants.BaseCells
                            .FaceIjkBaseCells[h.Face, h.Coord.I, h.Coord.J, h.Coord.K]
                            .BaseCell;
        }

        /// <summary>
        /// Find base cell given FaceIJK.
        ///
        /// Given the face number and a resolution 0 ijk+ coordinate in that face's
        /// face-centered ijk coordinate system, return the number of 60' ccw rotations
        /// to rotate into the coordinate system of the base cell at that coordinates.
        ///
        /// Valid ijk+ lookup coordinates are from (0, 0, 0) to (2, 2, 2).
        /// </summary>
        /// <!--
        /// baseCells.c
        /// int _faceIjkToBaseCellCCWrot60
        /// -->
        public static int ToBaseCellCounterClockwiseRotate60(this FaceIjk h)
        {
            return Constants.BaseCells
                            .FaceIjkBaseCells[h.Face, h.Coord.I, h.Coord.J, h.Coord.K]
                            .CounterClockwiseRotate60;
        }

        /// <summary>
        /// Determines the center point in spherical coordinates of a cell given by
        /// a FaceIJK address at a specified resolution.
        /// </summary>
        /// <param name="h">The FaceIJK address of the cell.</param>
        /// <param name="res">The H3 resolution of the cell.</param>
        /// <!--
        /// faceijk.c
        /// void _faceIjkToGeo
        /// -->
        public static GeoCoord ToGeoCoord(this FaceIjk h, int res)
        {
            //var v = new Vec2d();
            return h.Coord
                    .ToHex2d()
                    .ToGeoCoord(h.Face, res, 0);
        }

        /// <summary>
        /// Generates the cell boundary in spherical coordinates for a cell given by a
        /// FaceIJK address at a specified resolution.
        /// </summary>
        /// <param name="h">The FaceIJK address of the cell</param>
        /// <param name="res">The H3 resolution of the cell</param>
        /// <param name="start">The first topological vertex to return</param>
        /// <param name="length">The number of topological vertexes to return</param>
        /// <returns>The spherical coordinates of the cell boundary</returns>
        /// <!--
        /// faceijk.c
        /// void _faceIjkToGeoBoundary
        /// -->
        public static GeoBoundary ToGeoBoundary(this FaceIjk h, int res, int start, int length)
        {
            int adjRes = res;
            var centerIjk = h;
            IList<FaceIjk> fijkVerts = new FaceIjk[Constants.H3.NUM_HEX_VERTS];

            (centerIjk, adjRes, fijkVerts) = centerIjk.ToVerts(adjRes, fijkVerts);

            // If we're returning the entire loop, we need one more iteration in case
            // of a distortion vertex on the last edge
            int additionalIteration = length == Constants.H3.NUM_HEX_VERTS
                                          ? 1
                                          : 0;

            var g = new GeoBoundary();
            Overage overage = default;
            // convert each vertex to lat/lon
            // adjust the face of each vertex as appropriate and introduce
            // edge-crossing vertices as needed
            g.NumVerts = 0;
            int lastFace = -1;
            Overage lastOverage = Overage.NO_OVERAGE;

            for (int vert = start; vert < start + length + additionalIteration; vert++)
            {
                int v = vert % Constants.H3.NUM_HEX_VERTS;
                var fijk = fijkVerts[v];
                var pentLeading4 = 0;

                (overage, fijk) = fijk.AdjustOverageClassIi(adjRes, pentLeading4, 1);
                
                /*
                Check for edge-crossing. Each face of the underlying icosahedron is a
                different projection plane. So if an edge of the hexagon crosses an
                icosahedron edge, an additional vertex must be introduced at that
                intersection point. Then each half of the cell edge can be projected
                to geographic coordinates using the appropriate icosahedron face
                projection. Note that Class II cell edges have vertices on the face
                edge, with no edge line intersections.
                */

                if (res.IsResClassIii() && vert > start && fijk.Face != lastFace && lastOverage != Overage.FACE_EDGE)
                {
                    // find hex2d of the two vertexes on original face
                    int lastV = (v + 5) % Constants.H3.NUM_HEX_VERTS;
                    var orig2d0 = fijkVerts[lastV].Coord.ToHex2d();
                    var orig2d1 = fijkVerts[v].Coord.ToHex2d();

                    // find the appropriate icosahedron face edge vertexes
                    int maxDim = Constants.FaceIjk.MaxDimByCiiRes[adjRes];
                    var v0 = new Vec2d(3.0 * maxDim, 0.0);
                    var v1 = new Vec2d(-1.5 * maxDim, 3.0 * Constants.H3.M_SQRT3_2 * maxDim);
                    var v2 = new Vec2d(-1.5 * maxDim, -3.0 * Constants.H3.M_SQRT3_2 * maxDim);

                    int face2 = lastFace == centerIjk.Face
                                    ? fijk.Face
                                    : lastFace;
                    Vec2d edge0;
                    Vec2d edge1;
                    switch (Constants.FaceIjk.AdjacentFaceDir[centerIjk.Face, face2])
                    {
                        case Constants.FaceIjk.IJ:
                            edge0 = v0;
                            edge1 = v1;
                            break;
                        case Constants.FaceIjk.JK:
                            edge0 = v1;
                            edge1 = v2;
                            break;
                        case Constants.FaceIjk.KI:
                            edge0 = v2;
                            edge1 = v0;
                            break;
                        default:
                            throw new Exception
                                (
                                 $"(adjacentFaceDir[centerIJK.face][face2] == KI) idx0: {centerIjk.Face} idx1: {face2}"
                                );
                    }

                    // find the intersection and add the lat/lon point to the result
                    var inter = Vec2d.FindIntersection(orig2d0, orig2d1, edge0, edge1);
                    /*
                    If a point of intersection occurs at a hexagon vertex, then each
                    adjacent hexagon edge will lie completely on a single icosahedron
                    face, and no additional vertex is required.
                    */
                    bool isIntersectionAtVertex = orig2d0 == inter || orig2d1 == inter;
                    if (!isIntersectionAtVertex)
                    {
                        g.Verts[g.NumVerts] = inter.ToGeoCoord(centerIjk.Face, adjRes, 1);
                        g.NumVerts++;
                    }
                }

                // convert vertex to lat/lon and add to the result
                // vert == start + NUM_HEX_VERTS is only used to test for possible
                // intersection on last edge
                if (vert < start + Constants.H3.NUM_HEX_VERTS)
                {
                    var vec = fijk.Coord.ToHex2d();
                    g.Verts[g.NumVerts] = vec.ToGeoCoord(fijk.Face, adjRes, 1);
                    g.NumVerts++;
                }

                lastFace = fijk.Face;
                lastOverage = overage;
            }

            return g;
        }

        /// <summary>
        /// Generates the cell boundary in spherical coordinates for a pentagonal cell
        /// given by a FaceIJK address at a specified resolution.
        /// </summary>
        /// <param name="h">The FaceIJK address of the pentagonal cell.</param>
        /// <param name="res">The H3 resolution of the cell.</param>
        /// <param name="start">The first topological vertex to return.</param>
        /// <param name="length">The number of topological vertexes to return.</param>
        /// <returns>The spherical coordinates of the cell boundary.</returns>
        /// <!--
        /// faceijk.c
        /// void _faceIjkPentToGeoBoundary
        /// -->
        public static GeoBoundary PentToGeoBoundary(this FaceIjk h, int res, int start, int length)
        {
            var gb = new GeoBoundary();
            int adjRes = res;
            var centerIjk = h;

            IList<FaceIjk> fijkVerts = new FaceIjk[Constants.H3.NUM_PENT_VERTS];
            (_, adjRes, fijkVerts) = centerIjk.PentToVerts(adjRes, fijkVerts);

            // If we're returning the entire loop, we need one more iteration in case
            // of a distortion vertex on the last edge
            int additionalIteration = length == Constants.H3.NUM_PENT_VERTS
                                          ? 1
                                          : 0;

            // convert each vertex to lat/lon
            // adjust the face of each vertex as appropriate and introduce
            // edge-crossing vertices as needed
            gb.NumVerts = 0;
            var lastFijk = new FaceIjk();

            for (int vert = start; vert < start + length + additionalIteration; vert++)
            {
                int v = vert % Constants.H3.NUM_PENT_VERTS;
                var fijk = fijkVerts[v];
                (_, fijk) = fijk.AdjustPentOverage(adjRes); 
                
                // all Class III pentagon edges cross icosahedron edges
                // note that Class II pentagons have vertices on the edge,
                // not edge intersections
                if (res.IsResClassIii() && vert > start)
                {
                    // find hex2d of the two vertexes on the last face
                    var tmpFijk = fijk;
                    var orig2d0 = lastFijk.Coord.ToHex2d();

                    int currentToLastDir = Constants.FaceIjk.AdjacentFaceDir[tmpFijk.Face, lastFijk.Face];

                    var fijkOrient = Constants.FaceIjk.FaceNeighbors[tmpFijk.Face, currentToLastDir];

                    tmpFijk = tmpFijk.ReplaceFace(fijkOrient.Face);
                    //  Borrow ijk
                    var ijk = tmpFijk.Coord;

                    // rotate and translate for adjacent face
                    for (var i = 0; i < fijkOrient.Ccw60Rotations; i++)
                    {
                        ijk = ijk.Rotate60CounterClockwise();
                    }

                    var transVec = fijkOrient.Translate;
                    var scaleRes = Constants.FaceIjk.UnitScaleByCiiRes[adjRes] * 3;
                    transVec *= scaleRes;
                    ijk += transVec;
                    ijk = ijk.Normalized();
                    
                    var orig2d1 = ijk.ToHex2d();

                    // find the appropriate icosahedron face edge vertexes
                    int maxDim = Constants.FaceIjk.MaxDimByCiiRes[adjRes];
                    var v0 = new Vec2d(3.0 * maxDim, 0.0);
                    var v1 = new Vec2d(-1.5 * maxDim, 3.0 * Constants.H3.M_SQRT3_2 * maxDim);
                    var v2 = new Vec2d(-1.5 * maxDim, -3.0 *Constants. H3.M_SQRT3_2 * maxDim);

                    Vec2d edge0;
                    Vec2d edge1;

                    switch (Constants.FaceIjk.AdjacentFaceDir[tmpFijk.Face, fijk.Face])
                    {
                        case Constants.FaceIjk.IJ:
                            edge0 = v0;// new Vec2d(v0.X, v0.Y);
                            edge1 = v1;//new Vec2d(v1.X, v1.Y);
                            break;
                        case Constants.FaceIjk.JK:
                            edge0 = v1;//new Vec2d(v1.X, v1.Y);
                            edge1 = v2;//new Vec2d(v2.X, v2.Y);
                            break;
                        default:
                            if (Constants.FaceIjk.AdjacentFaceDir[tmpFijk.Face, fijk.Face] != Constants.FaceIjk.KI)
                            {
                                throw new Exception("assert(adjacentFaceDir[tmpFijk.face][fijk.face] == KI);");
                            }

                            edge0 = v2;//new Vec2d(v2.X, v2.Y);
                            edge1 = v0;//new Vec2d(v0.X, v0.Y);
                            break;
                    }

                    // find the intersection and add the lat/lon point to the result
                    var inter = Vec2d.FindIntersection(orig2d0, orig2d1, edge0, edge1);
                    gb.Verts[gb.NumVerts] = inter.ToGeoCoord(tmpFijk.Face, adjRes, 1);
                    gb.NumVerts++;
                }

                // convert vertex to lat/lon and add to the result
                // vert == start + NUM_PENT_VERTS is only used to test for possible
                // intersection on last edge
                if (vert < start + Constants.H3.NUM_PENT_VERTS)
                {
                    gb.Verts[gb.NumVerts] = fijk.Coord
                                                .ToHex2d()
                                                .ToGeoCoord(fijk.Face, adjRes, 1);
                    gb.NumVerts++;
                }

                lastFijk = new FaceIjk(fijk);
            }

            return gb;
        }
    }
}
