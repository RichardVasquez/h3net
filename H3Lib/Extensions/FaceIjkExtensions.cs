using System.Collections.Generic;
using System.Reflection.Metadata;

namespace H3Lib.Extensions
{
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
        public static (Overage, FaceIjk) AdjustOverageClassIi(
                this FaceIjk fijk, int res, int pentLeading4, int substrate
            )
        {
            var overage = Overage.NO_OVERAGE;
            var ijk = fijk.Coord;

            // get the maximum dimension value; scale if a substrate grid
            int maxDim = FaceIjk.maxDimByCIIres[res];
            if (substrate != 0)
            {
                maxDim *= 3;
            }

            // check for overage
            if (substrate != 0 && ijk.I + ijk.J + ijk.K == maxDim) // on edge
            {
                overage = Overage.FACE_EDGE;
            }
            else if (ijk.I + ijk.J + ijk.K > maxDim)  // overage
            {
                overage = Overage.NEW_FACE;
                FaceOrientIjk fijkOrient;

                if (ijk.K > 0)
                {
                    if (ijk.J > 0) // jk "quadrant"
                    {
                        fijkOrient = FaceIjk.faceNeighbors[fijk.Face, FaceIjk.JK];
                    }
                    else  // ik "quadrant"
                    {
                        fijkOrient = FaceIjk.faceNeighbors[fijk.Face, FaceIjk.KI];

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
                    fijkOrient = FaceIjk.faceNeighbors[fijk.Face, FaceIjk.IJ];
                }

                fijk = new FaceIjk(fijkOrient.Face, fijk.Coord);

                // rotate and translate for adjacent face
                for (int i = 0; i < fijkOrient.Ccw60Rotations; i++)
                {
                    ijk = ijk.Rotate60CounterClockwise();
                }

                var transVec = fijkOrient.Translate;
                int unitScale = FaceIjk.unitScaleByCIIres[res];
                if (substrate != 0)
                {
                    unitScale *= 3;
                }

                transVec *= unitScale;
                ijk += transVec;
                ijk = ijk.Normalized();

                // overage points on pentagon boundaries can end up on edges
                if (substrate != 0 && ijk.I + ijk.J + ijk.K == maxDim) // on edge
                {
                    overage = Overage.FACE_EDGE;
                }
            }

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
                new CoordIjk(5, 4, 0),  // 0
                new CoordIjk(1, 5, 0),  // 1
                new CoordIjk(0, 5, 4),  // 2
                new CoordIjk(0, 1, 5),  // 3
                new CoordIjk(4, 0, 5),  // 4
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
            for (var v = 0; v < Constants.NUM_PENT_VERTS; v++)
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
        public static (FaceIjk, int, IList<FaceIjk>) ToVerts(this FaceIjk fijk, int res, IList<FaceIjk> fijkVerts)
        {
            // the vertexes of an origin-centered cell in a Class II resolution on a
            // substrate grid with aperture sequence 33r. The aperture 3 gets us the
            // vertices, and the 3r gets us back to Class II.
            // vertices listed ccw from the i-axes
            CoordIjk[] vertsCii =
            {
                new CoordIjk(2, 1, 0),  // 0
                new CoordIjk(1, 2, 0),  // 1
                new CoordIjk(0, 2, 1),  // 2
                new CoordIjk(0, 1, 2),  // 3
                new CoordIjk(1, 0, 2),  // 4
                new CoordIjk(2, 0, 1)   // 5
            };

            // the vertexes of an origin-centered cell in a Class III resolution on a
            // substrate grid with aperture sequence 33r7r. The aperture 3 gets us the
            // vertices, and the 3r7r gets us to Class II.
            // vertices listed ccw from the i-axes
            CoordIjk[] vertsCiii =
            {
                new CoordIjk(5, 4, 0),  // 0
                new CoordIjk(1, 5, 0),  // 1
                new CoordIjk(0, 5, 4),  // 2
                new CoordIjk(0, 1, 5),  // 3
                new CoordIjk(4, 0, 5),  // 4
                new CoordIjk(5, 0, 1)   // 5
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
            // cell vertices. Add the center point substate coordinates
            // to each vertex to translate the vertices to that cell.
            for (int v = 0; v < Constants.NUM_HEX_VERTS; v++)
            {
                int newFace = fijk.Face;
                var newCoord = (fijk.Coord + verts[v]).Normalized();
                fijkVerts[v] = new FaceIjk(newFace, newCoord);
            }

            return (fijk, res, fijkVerts);
        }
    }
}
