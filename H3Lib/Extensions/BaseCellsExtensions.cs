namespace H3Lib.Extensions
{
    public static class BaseCellsExtensions
    {
        /// <summary>
        /// Return whether or not the indicated base cell is a pentagon.
        /// </summary>
        /// <!--
        /// baseCells.c
        /// int _isBaseCellPentagon
        /// -->
        public static bool IsBaseCellPentagon(this int baseCell)
        {
            return StaticData.BaseCells.BaseCellData[baseCell].IsPentagon == 1;
        }

        /// <summary>
        /// Return the direction from the origin base cell to the neighbor.
        /// </summary>
        /// <returns>INVALID_DIGIT if the base cells are not neighbors.</returns>
        /// <!--
        /// baseCells.c
        /// Direction _getBaseCellDirection
        /// -->
        public static Direction GetBaseCellDirection(this int originBaseCell, int neighboringBaseCell)
        {
            for (var dir = Direction.CENTER_DIGIT; dir < Direction.NUM_DIGITS; dir++) {
                int testBaseCell = StaticData.BaseCells.BaseCellNeighbors[originBaseCell, (int)dir];
                if (testBaseCell == neighboringBaseCell)
                {
                    return dir;
                }
            }

            return Direction.INVALID_DIGIT;
        }

        /// <summary>
        /// Return whether the indicated base cell is a pentagon where all
        /// neighbors are oriented towards it.
        /// </summary>
        /// <!--
        /// baseCells.c
        /// bool _isBaseCellPolarPentagon
        /// -->
        public static bool IsBaseCellPolarPentagon(this int baseCell)
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
        public static FaceIjk ToFaceIjk(this int baseCell)
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
        public static int ToCounterClockwiseRotate60(this int baseCell, int face)
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
        /// Return the neighboring base cell in the given direction.
        /// </summary>
        /// <!--
        /// baseCells.c
        /// _getBaseCellNeighbor
        /// -->
        public static int GetNeighbor(this int baseCell, Direction dir)
        {
            return StaticData.BaseCells.BaseCellNeighbors[baseCell, (int) dir];
        }

        /// <summary>
        /// Return whether or not the tested face is a cw offset face.
        /// </summary>
        /// <!--
        /// baseCells.c
        /// bool _baseCellIsCwOffset
        /// -->
        public static bool IsClockwiseOffset(this int baseCell, int testFace)
        {
            return StaticData.BaseCells.BaseCellData[baseCell].ClockwiseOffsetPentagon[0] == testFace || StaticData.BaseCells.BaseCellData[baseCell].ClockwiseOffsetPentagon[1] == testFace;
        }
    }
}
