using System.Collections.Generic;

namespace H3Lib.Extensions
{
    /// <summary>
    /// Extension methods for BaseCellsCount
    /// </summary>
    public static class BaseCellsExtensions
    {
        /// <summary>
        /// Return whether or not the indicated base cell is a pentagon.
        /// </summary>
        /// <remarks>
        /// 3.7.1
        /// baseCells.c
        /// int _isBaseCellPentagon
        /// </remarks>
        public static bool IsBaseCellPentagon(this int baseCell)
        {
            return Constants.BaseCells.BaseCellData[baseCell].IsPentagon == 1;
        }

        /// <summary>
        /// Return the direction from the origin base cell to the neighbor.
        /// </summary>
        /// <returns><see cref="Direction.InvalidDigit"/> if the base cells are not neighbors.</returns>
        /// <remarks>
        /// 3.7.1
        /// baseCells.c
        /// Direction _getBaseCellDirection
        /// </remarks>
        public static Direction GetBaseCellDirection(this int originBaseCell, int neighboringBaseCell)
        {
            for (var dir = Direction.CenterDigit; dir < Direction.NumDigits; dir++) {
                int testBaseCell = Constants.BaseCells.BaseCellNeighbors[originBaseCell, (int)dir];
                if (testBaseCell == neighboringBaseCell)
                {
                    return dir;
                }
            }

            return Direction.InvalidDigit;
        }

        /// <summary>
        /// Return whether the indicated base cell is a pentagon where all
        /// neighbors are oriented towards it.
        /// </summary>
        /// <remarks>
        /// 3.7.1
        /// baseCells.c
        /// bool _isBaseCellPolarPentagon
        /// </remarks>
        internal static bool IsBaseCellPolarPentagon(this int baseCell)
        {
            return baseCell == 4 || baseCell == 117;
        }

        /// <summary>
        /// Find the FaceIJK given a base cell.
        /// </summary>
        /// <remarks>
        /// 3.7.1
        /// baseCells.c
        /// _baseCellToFaceIjk
        /// </remarks>
        internal static FaceIjk ToFaceIjk(this int baseCell)
        {
            return new FaceIjk(Constants.BaseCells.BaseCellData[baseCell].HomeFijk);
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
        /// <remarks>
        /// 3.7.1
        /// baseCells.c
        /// int _baseCellToCCWrot60
        /// </remarks>
        internal static int ToCounterClockwiseRotate60(this int baseCell, int face)
        {
            if (face < 0 || face > Constants.H3.IcosahedronFaces)
            {
                return Constants.BaseCells.InvalidRotations;
            }

            for (var i = 0; i < 3; i++)
            {
                for (var j = 0; j < 3; j++)
                {
                    for (var k = 0; k < 3; k++)
                    {
                        if (Constants.BaseCells.FaceIjkBaseCells[face,i,j,k].BaseCell == baseCell)
                        {
                            return Constants.BaseCells.FaceIjkBaseCells[face, i, j, k].CounterClockwiseRotate60;
                        }
                    }
                }
            }

            return Constants.BaseCells.InvalidRotations;
        }

        /// <summary>
        /// Return the neighboring base cell in the given direction.
        /// </summary>
        /// <remarks>
        /// 3.7.1
        /// baseCells.c
        /// _getBaseCellNeighbor
        /// </remarks>
        public static int GetNeighbor(this int baseCell, Direction dir)
        {
            return Constants.BaseCells.BaseCellNeighbors[baseCell, (int) dir];
        }

        /// <summary>
        /// Return whether or not the tested face is a cw offset face.
        /// </summary>
        /// <remarks>
        /// 3.7.1
        /// baseCells.c
        /// bool _baseCellIsCwOffset
        /// </remarks>
        internal static bool IsClockwiseOffset(this int baseCell, int testFace)
        {
            return Constants.BaseCells.BaseCellData[baseCell].ClockwiseOffsetPentagon[0] == testFace ||
                   Constants.BaseCells.BaseCellData[baseCell].ClockwiseOffsetPentagon[1] == testFace;
        }
        
        /// <summary>
        /// getRes0Indexes generates all base cells
        /// </summary>
        public static List<H3Index> GetRes0Indexes()
        {
            var results = new List<H3Index>();
            for (var bc = 0; bc < Constants.H3.BaseCellsCount; bc++)
            {
                var baseCell = new H3Index(Constants.H3Index.Init).SetMode(H3Mode.Hexagon).SetBaseCell(bc);
                results.Add(baseCell);
            }

            return results;
        }
    }
}
