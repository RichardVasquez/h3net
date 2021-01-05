using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace H3Lib
{
    /// <summary>
    /// Header file for CoordIJK functions including conversion from lat/lon
    /// </summary>
    /// <remarks>
    /// References two Vec2d cartesian coordinate systems:
    ///
    /// 1. gnomonic: face-centered polyhedral gnomonic projection space with
    ///    traditional scaling and x-axes aligned with the face Class II
    ///    i-axes
    ///
    /// 2. hex2d: local face-centered coordinate system scaled a specific H3 grid
    ///    resolution unit length and with x-axes aligned with the local i-axes
    /// </remarks>
    [DebuggerDisplay("IJK: ({I}, {J}, {K})")]
    public readonly struct CoordIjk:IEquatable<CoordIjk>
    {
        public readonly int I;
        public readonly int J;
        public readonly int K;

        /// <summary>
        /// IJK hexagon coordinates
        /// </summary>
        public CoordIjk(int i, int j, int k):this()
        {
            I = i;
            J = j;
            K = k;
        }

        public CoordIjk(CoordIjk coord)
        {
            I = coord.I;
            J = coord.J;
            K = coord.K;
        }

        /// <summary>
        /// CoordIJK unit vectors corresponding to the 7 H3 digits.
        /// </summary>
        public static readonly CoordIjk[] xUNIT_VECS =
        {
            new CoordIjk(0, 0, 0),  // direction 0
            new CoordIjk(0, 0, 1),  // direction 1
            new CoordIjk(0, 1, 0),  // direction 2
            new CoordIjk(0, 1, 1),  // direction 3
            new CoordIjk(1, 0, 0),  // direction 4
            new CoordIjk(1, 0, 1),  // direction 5
            new CoordIjk(1, 1, 0)   // direction 6
        };

        public static readonly Dictionary<Direction, CoordIjk> UnitVectors =
            new Dictionary<Direction, CoordIjk>
            {
                {Direction.CENTER_DIGIT, new CoordIjk(0, 0, 0)},
                {Direction.K_AXES_DIGIT, new CoordIjk(0, 0, 1)},
                {Direction.J_AXES_DIGIT, new CoordIjk(0, 1, 0)},
                {Direction.JK_AXES_DIGIT, new CoordIjk(0, 1, 1)},
                {Direction.I_AXES_DIGIT, new CoordIjk(1, 0, 0)},
                {Direction.IK_AXES_DIGIT, new CoordIjk(1, 0, 1)},
                {Direction.IJ_AXES_DIGIT, new CoordIjk(1, 1, 0)},
            };

        /// <summary>
        /// Given cube coords as doubles, round to valid integer coordinates. Algorithm
        /// from https://www.redblobgames.com/grids/hexagons/#rounding
        /// </summary>
        /// <param name="i">Floating-point I coord</param>
        /// <param name="j">Floating-point J coord</param>
        /// <param name="k">Floating-point K coord</param>
        /// <returns>IJK coord struct</returns>
        /// <!--
        /// localij.c
        /// static void cubeRound
        /// -->
        public static CoordIjk CubeRound(double i, double j, double k)
        {
            var ri = (int) Math.Round(i, MidpointRounding.AwayFromZero);
            var rj = (int) Math.Round(j, MidpointRounding.AwayFromZero);
            var rk = (int) Math.Round(k, MidpointRounding.AwayFromZero);

            double iDiff = Math.Abs(ri - i);
            double jDiff = Math.Abs(rj - j);
            double kDiff = Math.Abs(rk - k);

            // Round, maintaining valid cube coords
            if (iDiff > jDiff && iDiff > kDiff)
            {
                ri = -rj - rk;
            }
            else if (jDiff > kDiff)
            {
                rj = -ri - rk;
            }
            else
            {
                rk = -ri - rj;
            }

            return new CoordIjk(ri, rj, rk);
        }
        
        public bool Equals(CoordIjk other)
        {
            return I == other.I && J == other.J && K == other.K;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            return obj is CoordIjk ijk && Equals(ijk);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(I, J, K);
        }

        public static bool operator ==(CoordIjk left, CoordIjk right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(CoordIjk left, CoordIjk right)
        {
            return !Equals(left, right);
        }
        
        public static CoordIjk operator+(CoordIjk c1,CoordIjk c2)
        {
            return new CoordIjk(c1.I + c2.I, c1.J + c2.J, c1.K + c2.K);
        }

        public static CoordIjk operator-(CoordIjk c1,CoordIjk c2)
        {
            return new CoordIjk(c1.I - c2.I, c1.J - c2.J, c1.K - c2.K);
        }

        public static CoordIjk operator *(CoordIjk c, int scalar)
        {
            return new CoordIjk(c.I * scalar, c.J * scalar, c.K * scalar);
        }
    }
}
