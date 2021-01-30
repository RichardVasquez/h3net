using System.Collections.Generic;
using System.Linq;

namespace H3Lib
{
    /// <summary>
    /// The faces in each axial direction of a given pentagon base cell
    /// </summary>
    public readonly struct PentagonDirectionFace
    {
        /// <summary>
        /// base cell number
        /// </summary>
        public readonly int BaseCell;
        /// <summary>
        /// face numbers for each axial direction, in order, starting with J
        /// </summary>
        public readonly int[] Faces;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="bc"></param>
        /// <param name="faces"></param>
        public PentagonDirectionFace(int bc, IList<int> faces)
        {
            BaseCell = bc;
            Faces = faces.Take(Constants.H3.NUM_PENT_VERTS).ToArray();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="raw"></param>
        public PentagonDirectionFace(IList<int> raw)
        {
            BaseCell = raw[0];
            Faces = raw.Skip(1).Take(Constants.H3.NUM_PENT_VERTS).ToArray();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="bc"></param>
        public PentagonDirectionFace(int bc, int f1, int f2, int f3, int f4, int f5)
        {
            BaseCell = bc;
            Faces = new[] {f1, f2, f3, f4, f5};
        }
    }
}
