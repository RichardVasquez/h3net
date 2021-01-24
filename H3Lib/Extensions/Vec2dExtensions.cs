using System;

namespace H3Lib.Extensions
{
    public static class Vec2dExtensions
    {
        /// <summary>
        /// Determine the containing hex in ijk+ coordinates for a 2D cartesian
        /// coordinate vector (from <a href="http://www.discreteglobalgrids.org/software/">DGGRID</a>).
        /// </summary>
        /// <param name="v">The 2D cartesian coordinate vector.</param>
        /// <!--
        /// coordijk.c
        /// void _hex2dToCoordIJK
        /// -->
        public static CoordIjk ToCoordIjk(this Vec2d v)
        {
            var h = new CoordIjk();

            // quantize into the ij system and then normalize
            double a1 = Math.Abs(v.X);
            double a2 = Math.Abs(v.Y);
            
            // first do a reverse conversion
            double x2 = a2 / Constants.H3.M_SIN60;
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
                    h = h.SetI( h.I - (int)(2.0 * diff));
                }
                else
                {
                    long axisI = (long)(h.J + 1) / 2;
                    long diff = h.I - axisI;
                    h = h.SetI(h.I-(int) (2.0 * diff + 1));
                }
            }

            if (v.Y < 0.0)
            {
                h=h.SetIJ(h.I - (2 * h.J + 1) / 2, -h.J);
            }

            return h.Normalized();
        }

        /// <summary>
        /// Determines the center point in spherical coordinates of a cell given by 2D
        /// hex coordinates on a particular icosahedral face.
        /// </summary>
        /// <param name="v">The 2D hex coordinates of the cell</param>
        /// <param name="face">The icosahedral face upon which the 2D hex coordinate system is centered</param>
        /// <param name="res">The H3 resolution of the cell</param>
        /// <param name="substrate">
        /// Indicates whether or not this grid is actually a substrate
        /// grid relative to the specified resolution.
        /// </param>
        /// <returns>The spherical coordinates of the cell center point</returns>
        /// <!--
        /// faceIjk.c
        /// void _hex2dToGeo
        /// -->
        public static GeoCoord ToGeoCoord(this Vec2d v, int face, int res, int substrate)
        {
            // calculate (r, theta) in hex2d
            double r = v.Magnitude;
            bool bSubstrate = substrate != 0;

            if (r < Constants.H3.EPSILON)
            {
                return Constants.FaceIjk.FaceCenterGeo[face];
            }

            double theta = Math.Atan2(v.Y, v.X);

            // scale for current resolution length u
            for (var i = 0; i < res; i++)
            {
                r /= Constants.FaceIjk.MSqrt7;
            }

            // scale accordingly if this is a substrate grid
            if (substrate !=0)
            {
                r /= 3.0;
                if (res.IsResClassIii())
                {
                    r /= Constants.FaceIjk.MSqrt7;
                }
            }

            r *= Constants.H3.RES0_U_GNOMONIC;

            // perform inverse gnomonic scaling of r
            r = Math.Atan(r);

            // adjust theta for Class III
            // if a substrate grid, then it's already been adjusted for Class III
            if (!bSubstrate && res.IsResClassIii())
            {
                theta = (theta + Constants.H3.M_AP7_ROT_RADS).NormalizeRadians();
            }

            // find theta as an azimuth
            theta = (Constants.FaceIjk.FaceAxesAzRadsCii[face, 0] - theta).NormalizeRadians();

            // now find the point at (r,theta) from the face center
            return Constants.FaceIjk.FaceCenterGeo[face]
                            .GetAzimuthDistancePoint(theta, r);
        }
        

    }
}
