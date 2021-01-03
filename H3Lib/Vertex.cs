using System.Collections.Generic;
using System.Linq;

namespace H3Lib
{
    public class Vertex
    {
        public static readonly Dictionary<int, int[]> PentagonDirectionFaces =
            new Dictionary<int, int[]>
            {
                {4, new[] {4, 0, 2, 1, 3}}, {14, new[] {6, 11, 2, 7, 1}},
                {24, new[] {5, 10, 1, 6, 0}}, {38, new[] {7, 12, 3, 8, 2}},
                {49, new[] {9, 14, 0, 5, 4}}, {58, new[] {8, 13, 4, 9, 3}},
                {63, new[] {11, 6, 15, 10, 16}}, {72, new[] {12, 7, 16, 11, 17}},
                {83, new[] {10, 5, 19, 14, 15}}, {97, new[] {13, 8, 17, 12, 18}},
                {107, new[] {14, 9, 18, 13, 19}}, {117, new[] {15, 19, 17, 18, 16}},
            };

        public const int DIRECTION_INDEX_OFFSET = 2;
        
        /// <summary>
        /// Max number of faces a base cell's descendants may appear on
        /// </summary>
        public const int INVALID_VERTEX = 5;

        /// <summary>
        /// Invalid vertex number
        /// </summary>
        public const int INVALID_VERTEX_NUM = -1;

        /// <summary>
        /// Hexagon direction to vertex number relationships (same face).
        /// Note that we don't use direction 0 (center)
        /// </summary>
        public static readonly int[] DirectionToVertexNumHex =
            {(int) Direction.INVALID_DIGIT, 3, 1, 2, 5, 4, 0};

        /// <summary>
        /// Pentagon direction to vertex number relationships (same face).
        /// Note that we don't use directions 0 (center) or 1 (deleted K axis).
        /// </summary>
        public static readonly int[] DirectionToVertexNumPent =
            {(int) Direction.INVALID_DIGIT, (int) Direction.INVALID_DIGIT, 1, 2, 4, 3, 0};

        public static int VertexRotations(H3Index cell)
        {
            // Get the face and other info for the origin
            FaceIjk fijk = new FaceIjk();
            H3Index._h3ToFaceIjk(cell, ref fijk);
            int baseCell = H3Index.h3GetBaseCell(cell);
            int cellLeadingDigit =(int) H3Index._h3LeadingNonZeroDigit(cell.Value);

            // get the base cell face
            FaceIjk baseFijk = new FaceIjk();
            BaseCells._baseCellToFaceIjk(baseCell, ref baseFijk);

            int ccwRot60 = BaseCells._baseCellToCCWrot60(baseCell, fijk.Face);

            if (BaseCells._isBaseCellPentagon(baseCell))
            {
                // Find the appropriate direction-to-face mapping
                int[] dirFaces = { };
                if (PentagonDirectionFaces.ContainsKey(baseCell))
                {
                    dirFaces = PentagonDirectionFaces[baseCell];
                }

                // additional CCW rotation for polar neighbors or IK neighbors
                if (fijk.Face != baseFijk.Face &&
                    (BaseCells._isBaseCellPolarPentagon(baseCell) ||
                     fijk.Face == dirFaces[(int)Direction.IK_AXES_DIGIT -  DIRECTION_INDEX_OFFSET]))
                {
                    ccwRot60 = (ccwRot60 + 1) % 6;
                }

                // Check whether the cell crosses a deleted pentagon subsequence
                if (cellLeadingDigit == (int)Direction.JK_AXES_DIGIT &&
                    fijk.Face == dirFaces[(int) Direction.IK_AXES_DIGIT - DIRECTION_INDEX_OFFSET])
                {
                    // Crosses from JK to IK: Rotate CW
                    ccwRot60 = (ccwRot60 + 5) % 6;
                } else if (cellLeadingDigit == (int)Direction.IK_AXES_DIGIT &&
                           fijk.Face == dirFaces[(int)Direction.JK_AXES_DIGIT - DIRECTION_INDEX_OFFSET])
                {
                    // Crosses from IK to JK: Rotate CCW
                    ccwRot60 = (ccwRot60 + 1) % 6;
                }
            }
            return ccwRot60;
        }

        public static int VertexNumForDirection(H3Index origin, Direction direction)
        {
            int isPentagon = H3Index.h3IsPentagon(origin);
            // Check for invalid directions
            if (direction == Direction.CENTER_DIGIT || direction >= Direction.INVALID_DIGIT ||
                (isPentagon == 1 && direction == Direction.K_AXES_DIGIT))
                return INVALID_VERTEX_NUM;

            // Determine the vertex rotations for this cell
            int rotations = VertexRotations(origin);

            // Find the appropriate vertex, rotating CCW if necessary
            if (isPentagon==1)
            {
                return (DirectionToVertexNumPent[(int) direction] +
                        Constants.NUM_PENT_VERTS - rotations) %
                       Constants.NUM_PENT_VERTS;
            }

            return (DirectionToVertexNumHex[(int)direction] + 
                    Constants.NUM_HEX_VERTS - rotations) %
                   Constants.NUM_HEX_VERTS;

        }
    }
}
