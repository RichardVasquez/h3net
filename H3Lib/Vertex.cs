using System.Collections.Generic;
using H3Lib.Extensions;

namespace H3Lib
{
    public class Vertex
    {
        public static int VertexRotations(H3Index cell)
        {
            // Get the face and other info for the origin
            FaceIjk fijk = new FaceIjk();
            H3Index._h3ToFaceIjk(cell, ref fijk);
            int baseCell = cell.BaseCell;
            int cellLeadingDigit =(int) H3Index._h3LeadingNonZeroDigit(cell.H3Value);

            // get the base cell face
            FaceIjk baseFijk = new FaceIjk();
            baseFijk = BaseCells.ToFaceIjk(baseCell);

            int ccwRot60 = BaseCells.ToCounterClockwiseRotate60(baseCell, fijk.Face);

            if (BaseCells.IsBaseCellPentagon(baseCell))
            {
                // Find the appropriate direction-to-face mapping
                int[] dirFaces = { };
                if (PentagonDirectionFaces.ContainsKey(baseCell))
                {
                    dirFaces = PentagonDirectionFaces[baseCell];
                }

                // additional CCW rotation for polar neighbors or IK neighbors
                if (fijk.Face != baseFijk.Face &&
                    (BaseCells.IsBaseCellPolarPentagon(baseCell) ||
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
            bool isPentagon = origin.IsPentagon();
            // Check for invalid directions
            if (direction == Direction.CENTER_DIGIT || direction >= Direction.INVALID_DIGIT ||
                (isPentagon && direction == Direction.K_AXES_DIGIT))
                return INVALID_VERTEX_NUM;

            // Determine the vertex rotations for this cell
            int rotations = VertexRotations(origin);

            // Find the appropriate vertex, rotating CCW if necessary
            if (isPentagon)
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
