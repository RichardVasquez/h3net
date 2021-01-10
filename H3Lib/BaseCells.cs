using System.Collections.Generic;

namespace H3Lib
{
    
    /// <summary>
    /// Base cell related lookup tables and access functions.
    /// </summary>
    public class BaseCells
    {
        /// <summary>
        /// res0IndexCount returns the number of resolution 0 indexes
        /// </summary>
        /// <!--
        /// baseCells.c
        /// int H3_EXPORT(res0IndexCount)
        /// -->
        public static int res0IndexCount()
        {
            return Constants.NUM_BASE_CELLS;
        }

        /// <summary>
        /// Generates all base cells
        /// </summary>
        public static List<H3Index> getRes0Indexes()
        {
            var results = new List<H3Index>();
            for (var bc = 0; bc < Constants.NUM_BASE_CELLS; bc++)
            {
                H3Index baseCell = StaticData.H3Index.H3_INIT;
                baseCell.Mode = H3Mode.Hexagon;
                baseCell.BaseCell = bc;
                results.Add(baseCell);
            }

            return results;
        }
        
        /// <summary>
        /// Return whether or not the indicated base cell is a pentagon.
        /// </summary>
        /// <!--
        /// basecells.c
        /// _isBaseCellPentagon
        /// -->
        public static bool IsBaseCellPentagon(int baseCell)
        {
            return StaticData.BaseCells.BaseCellData[baseCell].IsPentagon == 1;
        }

        /// <summary>
        /// Return whether the indicated base cell is a pentagon where all
        /// neighbors are oriented towards it.
        /// </summary>
        /// <!--
        /// baseCells.c
        /// bool _isBaseCellPolarPentagon
        /// -->
        public static bool IsBaseCellPolarPentagon(int baseCell)
        {
            return baseCell == 4 || baseCell == 117;
        }

        /// <summary>
        /// Find the FaceIJK given a base cell.
        /// </summary>
        /// <!--
        /// baseCells.c
        /// _baseCellToFaceIjk
        /// -->
        public static FaceIjk ToFaceIjk(int baseCell)
        {
            return new FaceIjk(StaticData.BaseCells.BaseCellData[baseCell].HomeFijk);
        }

        /// <summary>
        /// Given a base cell and the face it appears on, return
        /// the number of 60' ccw rotations for that base cell's
        /// coordinate system.
        /// </summary>
        /// <returns>
        /// The number of rotations, or INVALID_ROTATIONS if the base
        /// cell is not found on the given face
        /// </returns>
        /// <!--
        /// baseCells.c
        /// int _baseCellToCCWrot60
        /// -->
        public static int ToCounterClockwiseRotate60(int baseCell, int face)
        {
            if (face < 0 || face > Constants.NUM_ICOSA_FACES)
            {
                return StaticData.BaseCells.InvalidRotations;
            }

            var cellRotations = (BaseCellRotation[,,]) StaticData.BaseCells.FaceIjkBaseCells.GetValue(face);
            if (cellRotations == null)
            {
                return StaticData.BaseCells.InvalidRotations;
            }

            foreach (var rotation in cellRotations)
            {
                if (rotation.BaseCell == baseCell)
                {
                    return rotation.CounterClockwiseRotate60;
                }
            }

            return StaticData.BaseCells.InvalidRotations;
        }
        
        /// <summary>
        /// Find base cell given FaceIJK.
        ///
        /// Given the face number and a resolution 0 ijk+ coordinate in that face's
        /// face-centered ijk coordinate system, return the base cell located at that
        /// coordinate.
        /// </summary>
        public static int _faceIjkToBaseCell(FaceIjk h)
        {
            return StaticData.BaseCells.FaceIjkBaseCells[h.Face,h.Coord.I,h.Coord.J,h.Coord.K].BaseCell;
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
        public static int ToBaseCellCounterClockwiseRotate60( FaceIjk h)
        {
            return StaticData.BaseCells.FaceIjkBaseCells[h.Face, h.Coord.I, h.Coord.J, h.Coord.K].CounterClockwiseRotate60;
        }

        /// <summary>
        /// Return the neighboring base cell in the given direction.
        /// </summary>
        public static int _getBaseCellNeighbor(int baseCell, Direction dir)
        {
            return StaticData.BaseCells.BaseCellNeighbors[baseCell, (int) dir];
        }

        /// <summary>
        /// Return the neighboring base cell in the given direction.
        /// </summary>
        /// <!--
        /// baseCells.c
        /// _getBaseCellNeighbor
        /// -->
        public static int GetNeighbor(int baseCell, Direction dir)
        {
            return StaticData.BaseCells.BaseCellNeighbors[baseCell, (int) dir];
        }

        /// <summary>
        /// Return the direction from the origin base cell to the neighbor.
        /// </summary>
        /// <returns>INVALID_DIGIT if the base cells are not neighbors.</returns>
        public static Direction _getBaseCellDirection(int originBaseCell, int neighboringBaseCell)
        {
            for (var dir = Direction.CENTER_DIGIT; dir <Direction.NUM_DIGITS; dir++) 
            {
                var testBaseCell = GetNeighbor(originBaseCell, dir);
                if (testBaseCell == neighboringBaseCell)
                {
                    return dir;
                }
            }
            return Direction.INVALID_DIGIT;
        }
        
        /// <summary>
        /// Return whether or not the tested face is a cw offset face.
        /// </summary>
        /// <!--
        /// baseCells.c
        /// bool _baseCellIsCwOffset
        /// -->
        public static bool IsClockwiseOffset(int baseCell, int testFace)
        {
            return StaticData.BaseCells.BaseCellData[baseCell].ClockwiseOffsetPentagon[0] == testFace || StaticData.BaseCells.BaseCellData[baseCell].ClockwiseOffsetPentagon[1] == testFace;
        }
    }

}
