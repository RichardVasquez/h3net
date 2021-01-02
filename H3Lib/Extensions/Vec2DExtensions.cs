using System;

namespace H3Lib.Extensions
{
    public static class Vec2DExtensions
    {
        /// <summary>
        /// Determine the containing hex in ijk+ coordinates for a 2D cartesian
        /// coordinate vector (from <a href="http://www.discreteglobalgrids.org/software/">DGGRID</a>).
        /// </summary>
        /// <param name="v">The 2D cartesian coordinate vector.</param>
        public static CoordIjk ToCoordIjk(this Vec2d v)
        {
            var h = new CoordIjk();

            // quantize into the ij system and then normalize
            double a1 = Math.Abs(v.X);
            double a2 = Math.Abs(v.Y);

            // first do a reverse conversion
            double x2 = a2 / Constants.M_SIN60;
            double x1 = a1 + x2 / 2.0;

            // check if we have the center of a hex
            var m1 = (int)x1;
            var m2 = (int)x2;

            // otherwise round correctly
            double r1 = x1 - m1;
            double r2 = x2 - m2;

            if (r1 < 0.5) {
                if (r1 < 1.0 / 3.0)
                {
                    h = r2 < (1.0 + r1) / 2.0
                            ? h.SetIJ(m1,m2)
                            : h.SetIJ(m1,m2 + 1);
                } else
                {
                    h = r2 < (1.0 - r1)
                            ? h.SetJ(m2)
                            : h.SetJ(m2 + 1);

                    h = (1.0 - r1) <= r2 && r2 < (2.0 * r1)
                            ? h.SetI(m1 + 1)
                            : h.SetI(m1);
                }
            } else {
                if (r1 < 2.0 / 3.0)
                {
                    h = r2 < (1.0 - r1)
                            ? h.SetJ( m2)
                            : h.SetJ(m2 + 1);

                    h = (2.0 * r1 - 1.0) < r2 && r2 < (1.0 - r1)
                            ? h.SetI(m1)
                            : h.SetI(m1 + 1);
                }
                else
                {
                    h = r2 < (r1 / 2.0)
                            ? h.SetIJ(m1 + 1, m2)
                            : h.SetIJ(m1 + 1, m2 + 1);
                }
            }

            // now fold across the axes if necessary
            if (v.X < 0.0) 
            {
                if (h.J % 2 == 0)  // even
                {
                    long axisI = (long)h.J / 2;
                    long diff = h.I - axisI;
                    h = h.SetI((int) (h.I - 2.0 * diff));
                }
                else
                {
                    long axisI = (long)(h.J + 1) / 2;
                    long diff = h.I - axisI;
                    h = h.SetI((int) (2.0 * diff + 1));
                }
            }

            if (v.Y < 0.0)
            {
                h.SetIJ(h.I - (2 * h.J + 1) / 2, -h.J);
            }

            return h.Normalized();
        }

    }
}
