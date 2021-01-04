using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using H3Lib.Extensions;

namespace H3Lib
{
    /// <summary>
    /// Functions for working with icosahedral face-centered hex IJK
    /// coordinate systems.
    /// </summary>
    [DebuggerDisplay("Face: {Face} Coord: {Coord}")]
    public class FaceIjk
    {
        /// <summary>
        /// face number
        /// </summary>
        public int Face;
        /// <summary>
        /// ijk coordinates on that face
        /// </summary>
        public CoordIjk Coord;

        public FaceIjk()
        {
            Face = 0;
            Coord = new CoordIjk(0, 0, 0);
        }

        public FaceIjk(int f, CoordIjk cijk)
        {
            Face = f;
            Coord = cijk;
        }

        public FaceIjk(FaceIjk fijk)
        {
            Face = fijk.Face;
            Coord = new CoordIjk(fijk.Coord);
        }

        /// <summary>
        /// Invalid face index
        /// </summary>
        public const int INVALID_FACE = -1;
        /// <summary>
        /// Invalid faceNeighbors table direction
        /// </summary>
        public const int INVALID = -1;
        /// <summary>
        /// Center faceNeighbors table direction
        /// </summary>
        public const int CENTER = 0;
        /// <summary>
        /// IJ quadrant faceNeighbors table direction
        /// </summary>
        public const int IJ = 1;
        /// <summary>
        /// KI quadrant faceNeighbors table direction
        /// </summary>
        public const int KI = 2;
        /// <summary>
        /// JK quadrant faceNeighbors table direction
        /// </summary>
        public const int JK = 3;

        /// <summary>
        /// Square root of 7
        /// </summary>
        public static double M_SQRT7 = 2.6457513110645905905016157536392604257102;

        /// <summary>
        /// icosahedron face centers in lat/lon radians
        /// </summary>
        public static readonly GeoCoord[] faceCenterGeo =
        {
            new GeoCoord(0.803582649718989942, 1.248397419617396099), // face  0
            new GeoCoord(1.307747883455638156, 2.536945009877921159), // face  1
            new GeoCoord(1.054751253523952054, -1.347517358900396623), // face  2
            new GeoCoord(0.600191595538186799, -0.450603909469755746), // face  3
            new GeoCoord(0.491715428198773866, 0.401988202911306943), // face  4
            new GeoCoord(0.172745327415618701, 1.678146885280433686), // face  5
            new GeoCoord(0.605929321571350690, 2.953923329812411617), // face  6
            new GeoCoord(0.427370518328979641, -1.888876200336285401), // face  7
            new GeoCoord(-0.079066118549212831, -0.733429513380867741), // face  8
            new GeoCoord(-0.230961644455383637, 0.506495587332349035), // face  9
            new GeoCoord(0.079066118549212831, 2.408163140208925497), // face 10
            new GeoCoord(0.230961644455383637, -2.635097066257444203), // face 11
            new GeoCoord(-0.172745327415618701, -1.463445768309359553), // face 12
            new GeoCoord(-0.605929321571350690, -0.187669323777381622), // face 13
            new GeoCoord(-0.427370518328979641, 1.252716453253507838), // face 14
            new GeoCoord(-0.600191595538186799, 2.690988744120037492), // face 15
            new GeoCoord(-0.491715428198773866, -2.739604450678486295), // face 16
            new GeoCoord(-0.803582649718989942, -1.893195233972397139), // face 17
            new GeoCoord(-1.307747883455638156, -0.604647643711872080), // face 18
            new GeoCoord(-1.054751253523952054, 1.794075294689396615) // face 19
        };

        /// <summary>
        /// icosahedron face centers in x/y/z on the unit sphere
        /// </summary>
        public static readonly Vec3d[] faceCenterPoint =
        {
            new Vec3d(0.2199307791404606, 0.6583691780274996, 0.7198475378926182), // face  0
            new Vec3d(-0.2139234834501421, 0.1478171829550703, 0.9656017935214205), // face  1
            new Vec3d(0.1092625278784797, -0.4811951572873210, 0.8697775121287253), // face  2
            new Vec3d(0.7428567301586791, -0.3593941678278028, 0.5648005936517033), // face  3
            new Vec3d(0.8112534709140969, 0.3448953237639384, 0.4721387736413930), // face  4
            new Vec3d(-0.1055498149613921, 0.9794457296411413, 0.1718874610009365), // face  5
            new Vec3d(-0.8075407579970092, 0.1533552485898818, 0.5695261994882688), // face  6
            new Vec3d(-0.2846148069787907, -0.8644080972654206, 0.4144792552473539), // face  7
            new Vec3d(0.7405621473854482, -0.6673299564565524, -0.0789837646326737), // face  8
            new Vec3d(0.8512303986474293, 0.4722343788582681, -0.2289137388687808), // face  9
            new Vec3d(-0.7405621473854481, 0.6673299564565524, 0.0789837646326737), // face 10
            new Vec3d(-0.8512303986474292, -0.4722343788582682, 0.2289137388687808), // face 11
            new Vec3d(0.1055498149613919, -0.9794457296411413, -0.1718874610009365), // face 12
            new Vec3d(0.8075407579970092, -0.1533552485898819, -0.5695261994882688), // face 13
            new Vec3d(0.2846148069787908, 0.8644080972654204, -0.4144792552473539), // face 14
            new Vec3d(-0.7428567301586791, 0.3593941678278027, -0.5648005936517033), // face 15
            new Vec3d(-0.8112534709140971, -0.3448953237639382, -0.4721387736413930), // face 16
            new Vec3d(-0.2199307791404607, -0.6583691780274996, -0.7198475378926182), // face 17
            new Vec3d(0.2139234834501420, -0.1478171829550704, -0.9656017935214205), // face 18
            new Vec3d(-0.1092625278784796, 0.4811951572873210, -0.8697775121287253) // face 19
        };

        /// <summary>
        /// icosahedron face ijk axes as azimuth in radians from face center to
        /// vertex 0/1/2 respectively
        /// </summary>
        public static readonly double[,] faceAxesAzRadsCII =
        {
            {5.619958268523939882, 3.525563166130744542, 1.431168063737548730}, // face  0
            {5.760339081714187279, 3.665943979320991689, 1.571548876927796127}, // face  1
            {0.780213654393430055, 4.969003859179821079, 2.874608756786625655}, // face  2
            {0.430469363979999913, 4.619259568766391033, 2.524864466373195467}, // face  3
            {6.130269123335111400, 4.035874020941915804, 1.941478918548720291}, // face  4
            {2.692877706530642877, 0.598482604137447119, 4.787272808923838195}, // face  5
            {2.982963003477243874, 0.888567901084048369, 5.077358105870439581}, // face  6
            {3.532912002790141181, 1.438516900396945656, 5.627307105183336758}, // face  7
            {3.494305004259568154, 1.399909901866372864, 5.588700106652763840}, // face  8
            {3.003214169499538391, 0.908819067106342928, 5.097609271892733906}, // face  9
            {5.930472956509811562, 3.836077854116615875, 1.741682751723420374}, // face 10
            {0.138378484090254847, 4.327168688876645809, 2.232773586483450311}, // face 11
            {0.448714947059150361, 4.637505151845541521, 2.543110049452346120}, // face 12
            {0.158629650112549365, 4.347419854898940135, 2.253024752505744869}, // face 13
            {5.891865957979238535, 3.797470855586042958, 1.703075753192847583}, // face 14
            {2.711123289609793325, 0.616728187216597771, 4.805518392002988683}, // face 15
            {3.294508837434268316, 1.200113735041072948, 5.388903939827463911}, // face 16
            {3.804819692245439833, 1.710424589852244509, 5.899214794638635174}, // face 17
            {3.664438879055192436, 1.570043776661997111, 5.758833981448388027}, // face 18
            {2.361378999196363184, 0.266983896803167583, 4.455774101589558636}, // face 19
        };

        /// <summary>
        /// Definition of which faces neighbor each other.
        /// </summary>
        public static readonly FaceOrientIjk[,] faceNeighbors =
        {
            {
                // face 0
                new FaceOrientIjk(0, 0, 0, 0, 0), // central face
                new FaceOrientIjk(4, 2, 0, 2, 1), // ij quadrant
                new FaceOrientIjk(1, 2, 2, 0, 5), // ki quadrant
                new FaceOrientIjk(5, 0, 2, 2, 3) // jk quadrant
            },
            {
                // face 1
                new FaceOrientIjk(1, 0, 0, 0, 0), // central face
                new FaceOrientIjk(0, 2, 0, 2, 1), // ij quadrant
                new FaceOrientIjk(2, 2, 2, 0, 5), // ki quadrant
                new FaceOrientIjk(6, 0, 2, 2, 3) // jk quadrant
            },
            {
                // face 2
                new FaceOrientIjk(2, 0, 0, 0, 0), // central face
                new FaceOrientIjk(1, 2, 0, 2, 1), // ij quadrant
                new FaceOrientIjk(3, 2, 2, 0, 5), // ki quadrant
                new FaceOrientIjk(7, 0, 2, 2, 3) // jk quadrant
            },
            {
                // face 3
                new FaceOrientIjk(3, 0, 0, 0, 0), // central face
                new FaceOrientIjk(2, 2, 0, 2, 1), // ij quadrant
                new FaceOrientIjk(4, 2, 2, 0, 5), // ki quadrant
                new FaceOrientIjk(8, 0, 2, 2, 3) // jk quadrant
            },
            {
                // face 4
                new FaceOrientIjk(4, 0, 0, 0, 0), // central face
                new FaceOrientIjk(3, 2, 0, 2, 1), // ij quadrant
                new FaceOrientIjk(0, 2, 2, 0, 5), // ki quadrant
                new FaceOrientIjk(9, 0, 2, 2, 3) // jk quadrant
            },
            {
                // face 5
                new FaceOrientIjk(5, 0, 0, 0, 0), // central face
                new FaceOrientIjk(10, 2, 2, 0, 3), // ij quadrant
                new FaceOrientIjk(14, 2, 0, 2, 3), // ki quadrant
                new FaceOrientIjk(0, 0, 2, 2, 3) // jk quadrant
            },
            {
                // face 6
                new FaceOrientIjk(6, 0, 0, 0, 0), // central face
                new FaceOrientIjk(11, 2, 2, 0, 3), // ij quadrant
                new FaceOrientIjk(10, 2, 0, 2, 3), // ki quadrant
                new FaceOrientIjk(1, 0, 2, 2, 3) // jk quadrant
            },
            {
                // face 7
                new FaceOrientIjk(7, 0, 0, 0, 0), // central face
                new FaceOrientIjk(12, 2, 2, 0, 3), // ij quadrant
                new FaceOrientIjk(11, 2, 0, 2, 3), // ki quadrant
                new FaceOrientIjk(2, 0, 2, 2, 3) // jk quadrant
            },
            {
                // face 8
                new FaceOrientIjk(8, 0, 0, 0, 0), // central face
                new FaceOrientIjk(13, 2, 2, 0, 3), // ij quadrant
                new FaceOrientIjk(12, 2, 0, 2, 3), // ki quadrant
                new FaceOrientIjk(3, 0, 2, 2, 3) // jk quadrant
            },
            {
                // face 9
                new FaceOrientIjk(9, 0, 0, 0, 0), // central face
                new FaceOrientIjk(14, 2, 2, 0, 3), // ij quadrant
                new FaceOrientIjk(13, 2, 0, 2, 3), // ki quadrant
                new FaceOrientIjk(4, 0, 2, 2, 3) // jk quadrant
            },
            {
                // face 10
                new FaceOrientIjk(10, 0, 0, 0, 0), // central face
                new FaceOrientIjk(5, 2, 2, 0, 3), // ij quadrant
                new FaceOrientIjk(6, 2, 0, 2, 3), // ki quadrant
                new FaceOrientIjk(15, 0, 2, 2, 3) // jk quadrant
            },
            {
                // face 11
                new FaceOrientIjk(11, 0, 0, 0, 0), // central face
                new FaceOrientIjk(6, 2, 2, 0, 3), // ij quadrant
                new FaceOrientIjk(7, 2, 0, 2, 3), // ki quadrant
                new FaceOrientIjk(16, 0, 2, 2, 3) // jk quadrant
            },
            {
                // face 12
                new FaceOrientIjk(12, 0, 0, 0, 0), // central face
                new FaceOrientIjk(7, 2, 2, 0, 3), // ij quadrant
                new FaceOrientIjk(8, 2, 0, 2, 3), // ki quadrant
                new FaceOrientIjk(17, 0, 2, 2, 3) // jk quadrant
            },
            {
                // face 13
                new FaceOrientIjk(13, 0, 0, 0, 0), // central face
                new FaceOrientIjk(8, 2, 2, 0, 3), // ij quadrant
                new FaceOrientIjk(9, 2, 0, 2, 3), // ki quadrant
                new FaceOrientIjk(18, 0, 2, 2, 3) // jk quadrant
            },
            {
                // face 14
                new FaceOrientIjk(14, 0, 0, 0, 0), // central face
                new FaceOrientIjk(9, 2, 2, 0, 3), // ij quadrant
                new FaceOrientIjk(5, 2, 0, 2, 3), // ki quadrant
                new FaceOrientIjk(19, 0, 2, 2, 3) // jk quadrant
            },
            {
                // face 15
                new FaceOrientIjk(15, 0, 0, 0, 0), // central face
                new FaceOrientIjk(16, 2, 0, 2, 1), // ij quadrant
                new FaceOrientIjk(19, 2, 2, 0, 5), // ki quadrant
                new FaceOrientIjk(10, 0, 2, 2, 3) // jk quadrant
            },
            {
                // face 16
                new FaceOrientIjk(16, 0, 0, 0, 0), // central face
                new FaceOrientIjk(17, 2, 0, 2, 1), // ij quadrant
                new FaceOrientIjk(15, 2, 2, 0, 5), // ki quadrant
                new FaceOrientIjk(11, 0, 2, 2, 3) // jk quadrant
            },
            {
                // face 17
                new FaceOrientIjk(17, 0, 0, 0, 0), // central face
                new FaceOrientIjk(18, 2, 0, 2, 1), // ij quadrant
                new FaceOrientIjk(16, 2, 2, 0, 5), // ki quadrant
                new FaceOrientIjk(12, 0, 2, 2, 3) // jk quadrant
            },
            {
                // face 18
                new FaceOrientIjk(18, 0, 0, 0, 0), // central face
                new FaceOrientIjk(19, 2, 0, 2, 1), // ij quadrant
                new FaceOrientIjk(17, 2, 2, 0, 5), // ki quadrant
                new FaceOrientIjk(13, 0, 2, 2, 3) // jk quadrant
            },
            {
                // face 19
                new FaceOrientIjk(19, 0, 0, 0, 0), // central face
                new FaceOrientIjk(15, 2, 0, 2, 1), // ij quadrant
                new FaceOrientIjk(18, 2, 2, 0, 5), // ki quadrant
                new FaceOrientIjk(14, 0, 2, 2, 3) // jk quadrant
            }
        };

        /// <summary>
        /// direction from the origin face to the destination face, relative to
        /// the origin face's coordinate system, or -1 if not adjacent.
        /// </summary>
        public static readonly int[,] adjacentFaceDir =
        {
            {
                0, KI, -1, -1, IJ, JK, -1, -1, -1, -1,
                -1, -1, -1, -1, -1, -1, -1, -1, -1, -1
            }, // face 0
            {
                IJ, 0, KI, -1, -1, -1, JK, -1, -1, -1,
                -1, -1, -1, -1, -1, -1, -1, -1, -1, -1
            }, // face 1
            {
                -1, IJ, 0, KI, -1, -1, -1, JK, -1, -1,
                -1, -1, -1, -1, -1, -1, -1, -1, -1, -1
            }, // face 2
            {
                -1, -1, IJ, 0, KI, -1, -1, -1, JK, -1,
                -1, -1, -1, -1, -1, -1, -1, -1, -1, -1
            }, // face 3
            {
                KI, -1, -1, IJ, 0, -1, -1, -1, -1, JK,
                -1, -1, -1, -1, -1, -1, -1, -1, -1, -1
            }, // face 4
            {
                JK, -1, -1, -1, -1, 0, -1, -1, -1, -1,
                IJ, -1, -1, -1, KI, -1, -1, -1, -1, -1
            }, // face 5
            {
                -1, JK, -1, -1, -1, -1, 0, -1, -1, -1,
                KI, IJ, -1, -1, -1, -1, -1, -1, -1, -1
            }, // face 6
            {
                -1, -1, JK, -1, -1, -1, -1, 0, -1, -1,
                -1, KI, IJ, -1, -1, -1, -1, -1, -1, -1
            }, // face 7
            {
                -1, -1, -1, JK, -1, -1, -1, -1, 0, -1,
                -1, -1, KI, IJ, -1, -1, -1, -1, -1, -1
            }, // face 8
            {
                -1, -1, -1, -1, JK, -1, -1, -1, -1, 0,
                -1, -1, -1, KI, IJ, -1, -1, -1, -1, -1
            }, // face 9
            {
                -1, -1, -1, -1, -1, IJ, KI, -1, -1, -1,
                0, -1, -1, -1, -1, JK, -1, -1, -1, -1
            }, // face 10
            {
                -1, -1, -1, -1, -1, -1, IJ, KI, -1, -1,
                -1, 0, -1, -1, -1, -1, JK, -1, -1, -1
            }, // face 11
            {
                -1, -1, -1, -1, -1, -1, -1, IJ, KI, -1,
                -1, -1, 0, -1, -1, -1, -1, JK, -1, -1
            }, // face 12
            {
                -1, -1, -1, -1, -1, -1, -1, -1, IJ, KI,
                -1, -1, -1, 0, -1, -1, -1, -1, JK, -1
            }, // face 13
            {
                -1, -1, -1, -1, -1, KI, -1, -1, -1, IJ,
                -1, -1, -1, -1, 0, -1, -1, -1, -1, JK
            }, // face 14
            {
                -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
                JK, -1, -1, -1, -1, 0, IJ, -1, -1, KI
            }, // face 15
            {
                -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
                -1, JK, -1, -1, -1, KI, 0, IJ, -1, -1
            }, // face 16
            {
                -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
                -1, -1, JK, -1, -1, -1, KI, 0, IJ, -1
            }, // face 17
            {
                -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
                -1, -1, -1, JK, -1, -1, -1, KI, 0, IJ
            }, // face 18
            {
                -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
                -1, -1, -1, -1, JK, IJ, -1, -1, KI, 0
            } // face 19
        };

        /// <summary>
        /// overage distance table
        /// </summary>
        public static readonly int[] maxDimByCIIres =
        {
            2, // res  0
            -1, // res  1
            14, // res  2
            -1, // res  3
            98, // res  4
            -1, // res  5
            686, // res  6
            -1, // res  7
            4802, // res  8
            -1, // res  9
            33614, // res 10
            -1, // res 11
            235298, // res 12
            -1, // res 13
            1647086, // res 14
            -1, // res 15
            11529602 // res 16
        };

        /// <summary>
        /// unit scale distance table
        /// </summary>
        public static readonly int[] unitScaleByCIIres =
        {
            1, // res  0
            -1, // res  1
            7, // res  2
            -1, // res  3
            49, // res  4
            -1, // res  5
            343, // res  6
            -1, // res  7
            2401, // res  8
            -1, // res  9
            16807, // res 10
            -1, // res 11
            117649, // res 12
            -1, // res 13
            823543, // res 14
            -1, // res 15
            5764801 // res 16
        };

        /// <summary>
        /// Encodes a coordinate on the sphere to the FaceIJK address of the containing
        /// cell at the specified resolution.
        /// </summary>
        /// <param name="g">The spherical coordinates to encode.</param>
        /// <param name="res">The desired H3 resolution for the encoding.</param>
        /// <param name="h">The FaceIJK address of the containing cell at resolution res.</param>
        public static void _geoToFaceIjk(GeoCoord g, int res, ref FaceIjk h)
        {
            // first convert to hex2d
            var v = new Vec2d();
            _geoToHex2d(g, res, ref h.Face, ref v);

            // then convert to ijk+
            CoordIjk._hex2dToCoordIJK(ref v, ref h.Coord);
        }


        /// <summary>
        /// Encodes a coordinate on the sphere to the corresponding icosahedral face and
        /// containing 2D hex coordinates relative to that face center.
        /// </summary>
        /// <param name="g">The spherical coordinates to encode.</param>
        /// <param name="res">The desired H3 resolution for the encoding.</param>
        /// <param name="face">The icosahedral face containing the spherical coordinates.</param>
        /// <param name="v">The 2D hex coordinates of the cell containing the point.</param>
        public static void _geoToHex2d(GeoCoord g, int res, ref int face, ref Vec2d v)
        {
            Vec3d v3d = new Vec3d();
            Vec3d._geoToVec3d(g, ref v3d);

            // determine the icosahedron face
            face = 0;
            double sqd = Vec3d._pointSquareDist(faceCenterPoint[0],  v3d);
            for (int f = 1; f < Constants.NUM_ICOSA_FACES; f++)
            {
                double sqdT = Vec3d._pointSquareDist( faceCenterPoint[f],  v3d);
                if (sqdT < sqd)
                {
                    face = f;
                    sqd = sqdT;
                }
            }

            // cos(r) = 1 - 2 * sin^2(r/2) = 1 - 2 * (sqd / 4) = 1 - sqd/2
            double r = Math.Acos(1 - sqd / 2);

            if (r < double.Epsilon)
            {
                v.X = 0.0;
                v.Y = 0.0;
                return;
            }

            // now have face and r, now find CCW theta from CII i-axis
            double theta = (faceAxesAzRadsCII[face, 0] - 
                            GeoCoord._geoAzimuthRads(faceCenterGeo[face], g).NormalizeRadians()
                            ).NormalizeRadians();
            
            // adjust theta for Class III (odd resolutions)
            if (H3Index.isResClassIII(res))
            {
                theta = (theta - Constants.M_AP7_ROT_RADS).NormalizeRadians();
            }

            // perform gnomonic scaling of r
            r = Math.Tan(r);

            // scale for current resolution length u
            r /= Constants.RES0_U_GNOMONIC;
            for (int i = 0; i < res; i++)
            {
                r *= M_SQRT7;
            }

            // we now have (r, theta) in hex2d with theta ccw from x-axes
            // convert to local x,y
            v.X = r * Math.Cos(theta);
            v.Y = r * Math.Sin(theta);
        }

        /// <summary>
        /// Determines the center point in spherical coordinates of a cell given by 2D
        /// hex coordinates on a particular icosahedral face.
        /// </summary>
        /// <param name="v">The 2D hex coordinates of the cell.</param>
        /// <param name="face">The icosahedral face upon which the 2D hex coordinate system is centered</param>
        /// <param name="res">The H3 resolution of the cell</param>
        /// <param name="substrate">
        /// Indicates whether or not this grid is actually a substrate
        /// grid relative to the specified resolution.
        /// </param>
        /// <param name="g">The spherical coordinates of the cell center point.</param>
        public static void _hex2dToGeo(ref Vec2d v, int face, int res, int substrate, ref GeoCoord g)
        {
            // calculate (r, theta) in hex2d
            double r = Vec2d._v2dMag(v);

            if (r < Constants.EPSILON)
            {
                g = faceCenterGeo[face];
                return;
            }

            double theta = Math.Atan2(v.Y, v.X);

            // scale for current resolution length u
            for (int i = 0; i < res; i++)
            {
                r /= M_SQRT7;
            }

            // scale accordingly if this is a substrate grid
            if (substrate > 0)
            {
                r /= 3.0;
                if (H3Index.isResClassIII(res))
                {
                    r /= M_SQRT7;
                }
            }

            r *= Constants.RES0_U_GNOMONIC;

            // perform inverse gnomonic scaling of r
            r = Math.Atan(r);

            // adjust theta for Class III
            // if a substrate grid, then it's already been adjusted for Class III
            if (substrate == 0 && H3Index.isResClassIII(res))
            {
                theta = (theta + Constants.M_AP7_ROT_RADS).NormalizeRadians();
            }

            // find theta as an azimuth
            theta = (faceAxesAzRadsCII[face, 0] - theta).NormalizeRadians();

            // now find the point at (r,theta) from the face center
            GeoCoord._geoAzDistanceRads(ref faceCenterGeo[face], theta, r, ref g);
        }

        /// <summary>
        /// Determines the center point in spherical coordinates of a cell given by
        /// a FaceIJK address at a specified resolution.
        /// </summary>
        /// <param name="h">The FaceIJK address of the cell.</param>
        /// <param name="res">The H3 resolution of the cell.</param>
        /// <param name="g">The spherical coordinates of the cell center point.</param>
        public static void _faceIjkToGeo(FaceIjk h, int res, ref GeoCoord g)
        {
            var v = new Vec2d();
            CoordIjk._ijkToHex2d(h.Coord, ref v);
            _hex2dToGeo(ref v, h.Face, res, 0, ref g);
        }

        
        /// <summary>
        /// Generates the cell boundary in spherical coordinates for a pentagonal cell
        /// given by a FaceIJK address at a specified resolution.
        /// </summary>
        /// <param name="h">The FaceIJK address of the pentagonal cell.</param>
        /// <param name="res">The H3 resolution of the cell.</param>
        /// <param name="start">The first topological vertex to return.</param>
        /// <param name="length">The number of topological vertexes to return.</param>
        /// <param name="g">The spherical coordinates of the cell boundary.</param>
        public static void _faceIjkPentToGeoBoundary(FaceIjk h, int res, int start, int length, ref GeoBoundary g)
        {
            int adjRes = res;
            var centerIjk = new FaceIjk(h);
            var fijkVerts = Enumerable.Range(1, Constants.NUM_PENT_VERTS).Select(i => new FaceIjk()).ToArray();
            _faceIjkPentToVerts(ref centerIjk, ref adjRes, ref fijkVerts);

            // If we're returning the entire loop, we need one more iteration in case
            // of a distortion vertex on the last edge
            int additionalIteration = length == Constants.NUM_PENT_VERTS ? 1 : 0;

            // convert each vertex to lat/lon
            // adjust the face of each vertex as appropriate and introduce
            // edge-crossing vertices as needed

            g.numVerts = 0;
            var lastFijk = new FaceIjk();

            for (int vert = start; vert < start + length + additionalIteration; vert++)
            {
                int v = vert % Constants.NUM_PENT_VERTS;
                var fijk = new FaceIjk(fijkVerts[v]);
                _adjustPentVertOverage(ref fijk, adjRes);
                fijkVerts[v] = new FaceIjk(fijk);

                // all Class III pentagon edges cross icosahedron edges
                // note that Class II pentagons have vertices on the edge,
                // not edge intersections
                if (H3Index.isResClassIII(res) && vert > start)
                {
                    // find hex2d of the two vertexes on the last face
                    var tmpFijk = new FaceIjk(fijk);

                    var orig2d0 = new Vec2d();
                    CoordIjk._ijkToHex2d(lastFijk.Coord, ref orig2d0);

                    int currentToLastDir = adjacentFaceDir[tmpFijk.Face, lastFijk.Face];

                    var fijkOrient = faceNeighbors[tmpFijk.Face, currentToLastDir];

                    tmpFijk.Face = fijkOrient.FaceOrientIjk1.Face;
                    var ijk = new CoordIjk(tmpFijk.Coord);

                    // rotate and translate for adjacent face
                    for (var i = 0; i < fijkOrient.FaceOrientIjk1.CcwRot60; i++)
                    {
                        CoordIjk._ijkRotate60ccw(ref ijk);
                    }

                    var transVec = new CoordIjk(fijkOrient.FaceOrientIjk1.Translate);
                    CoordIjk._ijkScale(ref transVec, unitScaleByCIIres[adjRes] * 3);
                    CoordIjk._ijkAdd(ijk, transVec, ref ijk);
                    CoordIjk._ijkNormalize(ref ijk);

                    var orig2d1 = new Vec2d();
                    CoordIjk._ijkToHex2d(ijk, ref orig2d1);
                    tmpFijk.Coord = ijk;

                    // find the appropriate icosahedron face edge vertexes
                    int maxDim = maxDimByCIIres[adjRes];
                    var v0 = new Vec2d(3.0 * maxDim, 0.0);
                    var v1 = new Vec2d(-1.5 * maxDim, 3.0 * Constants.M_SQRT3_2 * maxDim);
                    var v2 = new Vec2d(-1.5 * maxDim, -3.0 * Constants.M_SQRT3_2 * maxDim);

                    Vec2d edge0;
                    Vec2d edge1;
                    switch (adjacentFaceDir[tmpFijk.Face,fijk.Face])
                    {
                        case IJ:
                            edge0 = new Vec2d(v0.X, v0.Y);
                            edge1 = new Vec2d(v1.X, v1.Y);
                            break;
                        case JK:
                            edge0 = new Vec2d(v1.X, v1.Y);
                            edge1 = new Vec2d(v2.X, v2.Y);
                            break;
                        default:
                            if (adjacentFaceDir[tmpFijk.Face, fijk.Face] != KI)
                            {
                                throw new Exception("assert(adjacentFaceDir[tmpFijk.face][fijk.face] == KI);");
                            }
                            edge0 = new Vec2d(v2.X, v2.Y);
                            edge1 = new Vec2d(v0.X, v0.Y);
                            break;
                    }

                    // find the intersection and add the lat/lon point to the result
                    var inter = new Vec2d();
                    Vec2d._v2dIntersect(orig2d0, orig2d1, edge0, edge1, ref inter);
                    var geo = g.verts[g.numVerts];
                    _hex2dToGeo(ref inter, tmpFijk.Face, adjRes, 1, ref geo);
                    g.verts[g.numVerts] = new GeoCoord(geo.Latitude, geo.Longitude);
                    g.numVerts++;
                }

                // convert vertex to lat/lon and add to the result
                // vert == start + NUM_PENT_VERTS is only used to test for possible
                // intersection on last edge
                if (vert < start + Constants.NUM_PENT_VERTS)
                {
                    var vec = new Vec2d();
                    CoordIjk._ijkToHex2d(fijk.Coord, ref vec);
                    var geo = g.verts[g.numVerts];
                    _hex2dToGeo(ref vec, fijk.Face, adjRes, 1, ref geo);
                    g.verts[g.numVerts] = new GeoCoord(geo.Latitude, geo.Longitude);
                    g.numVerts++;
                }

                lastFijk = new FaceIjk(fijk);
            }
        }

        /// <summary>
        /// Get the vertices of a pentagon cell as substrate FaceIJK addresses
        /// </summary>
        /// <param name="fijk">The FaceIJK address of the cell.</param>
        /// <param name="res">
        /// The H3 resolution of the cell. This may be adjusted if
        /// necessary for the substrate grid resolution.
        /// </param>
        /// <param name="fijkVerts">Output array for the vertices</param>
        public static void _faceIjkPentToVerts(ref FaceIjk fijk, ref int res, ref FaceIjk[] fijkVerts)
        {
            // the vertexes of an origin-centered pentagon in a Class II resolution on a
            // substrate grid with aperture sequence 33r. The aperture 3 gets us the
            // vertices, and the 3r gets us back to Class II.
            // vertices listed ccw from the i-axes
            CoordIjk[] vertsCII =
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
            CoordIjk[] vertsCIII =
            {
                new CoordIjk(5, 4, 0), // 0
                new CoordIjk(1, 5, 0), // 1
                new CoordIjk(0, 5, 4), // 2
                new CoordIjk(0, 1, 5), // 3
                new CoordIjk(4, 0, 5), // 4
            };

            // get the correct set of substrate vertices for this resolution
            var verts = H3Index.isResClassIII(res)
                            ? vertsCIII
                            : vertsCII;

            // adjust the center point to be in an aperture 33r substrate grid
            // these should be composed for speed
            var coord = new CoordIjk(fijk.Coord);
            CoordIjk._downAp3(ref coord);
            CoordIjk._downAp3r(ref coord);

            if (H3Index.isResClassIII(res))
            {
                CoordIjk._downAp7r(ref coord);
                res++;
            }

            fijk.Coord = coord;
            
            // The center point is now in the same substrate grid as the origin
            // cell vertices. Add the center point substrate coordinates
            // to each vertex to translate the vertices to that cell.
            for (var v = 0; v < Constants.NUM_PENT_VERTS; v++)
            {
                fijkVerts[v].Face = fijk.Face;
                var fCoord = fijkVerts[v].Coord;
                CoordIjk._ijkAdd(coord, verts[v], ref fCoord);
                CoordIjk._ijkNormalize(ref fCoord);
                fijkVerts[v].Coord = fCoord;
            }

            fijk.Coord = coord;
        }

        /// <summary>
        /// Generates the cell boundary in spherical coordinates for a cell given by a
        /// FaceIJK address at a specified resolution.
        /// </summary>
        /// <param name="h">The FaceIJK address of the cell.</param>
        /// <param name="res">The H3 resolution of the cell.</param>
        /// <param name="start">The first topological vertex to return.</param>
        /// <param name="length">The number of topological vertexes to return.</param>
        /// <param name="g">The spherical coordinates of the cell boundary.</param>
        public static void _faceIjkToGeoBoundary(FaceIjk h, int res, int start, int length, ref GeoBoundary g)
        {
            int adjRes = res;
            FaceIjk centerIjk = new FaceIjk(h.Face, new CoordIjk(h.Coord.I, h.Coord.J, h.Coord.K));
            FaceIjk[] fijkVerts = Enumerable.Range(1, Constants.NUM_HEX_VERTS).Select(i => new FaceIjk()).ToArray();
            _faceIjkToVerts(ref centerIjk, ref res, ref fijkVerts);
            
            // If we're returning the entire loop, we need one more iteration in case
            // of a distortion vertex on the last edge
            int additionalIteration = length == Constants.NUM_HEX_VERTS
                                          ? 1
                                          : 0;

            // convert each vertex to lat/lon
            // adjust the face of each vertex as appropriate and introduce
            // edge-crossing vertices as needed
            g.numVerts = 0;
            int lastFace = -1;
            var lastOverage = Overage.NO_OVERAGE;
            for (int vert = start;
                 vert < start + length + additionalIteration;
                 vert++)
            {
                int v = vert % Constants.NUM_HEX_VERTS;

                var fijk = fijkVerts[v];

                const int pentLeading4 = 0;
                var overage = _adjustOverageClassII(ref fijk, adjRes, pentLeading4, 1);

                /*
                Check for edge-crossing. Each face of the underlying icosahedron is a
                different projection plane. So if an edge of the hexagon crosses an
                icosahedron edge, an additional vertex must be introduced at that
                intersection point. Then each half of the cell edge can be projected
                to geographic coordinates using the appropriate icosahedron face
                projection. Note that Class II cell edges have vertices on the face
                edge, with no edge line intersections.
                */
                if (H3Index.isResClassIII(res) && vert > start && fijk.Face != lastFace &&
                    lastOverage != Overage.FACE_EDGE)
                {
                    // find hex2d of the two vertexes on original face
                    int lastV = (v + 5) % Constants.NUM_HEX_VERTS;
                    var orig2d0 = new Vec2d();
                    CoordIjk._ijkToHex2d(fijkVerts[lastV].Coord, ref orig2d0);

                    Vec2d orig2d1 = new Vec2d();
                    CoordIjk._ijkToHex2d(fijkVerts[v].Coord, ref orig2d1);

                    // find the appropriate icosahedron face edge vertexes
                    int maxDim = maxDimByCIIres[adjRes];
                    Vec2d v0 = new Vec2d(3.0 * maxDim, 0.0);
                    Vec2d v1 = new Vec2d(-1.5 * maxDim, 3.0 * Constants.M_SQRT3_2 * maxDim);
                    Vec2d v2 = new Vec2d(-1.5 * maxDim, -3.0 * Constants.M_SQRT3_2 * maxDim);

                    int face2 = ((lastFace == centerIjk.Face)
                                     ? fijk.Face
                                     : lastFace);
                    Vec2d edge0;
                    Vec2d edge1;
                    switch (adjacentFaceDir[centerIjk.Face,face2])
                    {
                        case IJ:
                            edge0 = v0;
                            edge1 = v1;
                            break;
                        case JK:
                            edge0 = v1;
                            edge1 = v2;
                            break;
                        // case KI:
                        default:
                            if (adjacentFaceDir[centerIjk.Face, face2] != KI)
                            {
                                throw new Exception("adjacentFaceDir[centerIJK.face][face2] != KI");
                            }
                            edge0 = v2;
                            edge1 = v0;
                            break;
                    }

                    // find the intersection and add the lat/lon point to the result
                    Vec2d inter = new Vec2d();
                    Vec2d._v2dIntersect(orig2d0, orig2d1, edge0, edge1, ref inter);
                    /*
                    If a point of intersection occurs at a hexagon vertex, then each
                    adjacent hexagon edge will lie completely on a single icosahedron
                    face, and no additional vertex is required.
                    */
                    bool isIntersectionAtVertex =
                        Vec2d._v2dEquals(orig2d0, inter) || Vec2d._v2dEquals(orig2d1, inter);
                    if (!isIntersectionAtVertex)
                    {
                        var geo = g.verts[g.numVerts];
                        _hex2dToGeo
                            (ref inter, centerIjk.Face, adjRes, 1, ref geo);
                        g.verts[g.numVerts] = geo;
                        g.numVerts++;
                    }
                }

                // convert vertex to lat/lon and add to the result
                // vert == start + NUM_HEX_VERTS is only used to test for possible
                // intersection on last edge
                if (vert < start + Constants.NUM_HEX_VERTS)
                {
                    Vec2d vec = new Vec2d();
                    CoordIjk._ijkToHex2d(fijk.Coord, ref vec);
                    var geo = g.verts[g.numVerts];
                    _hex2dToGeo(ref vec, fijk.Face, adjRes, 1, ref geo);
                    g.verts[g.numVerts] = geo;
                    g.numVerts++;
                }

                lastFace = fijk.Face;
                lastOverage = overage;
            }
        }

        public static void _faceIjkToVerts(ref FaceIjk fijk, ref int res, ref FaceIjk[] fijkVerts)
        {
            // the vertexes of an origin-centered cell in a Class II resolution on a
            // substrate grid with aperture sequence 33r. The aperture 3 gets us the
            // vertices, and the 3r gets us back to Class II.
            // vertices listed ccw from the i-axes
            CoordIjk[] vertsCII =
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
            CoordIjk[] vertsCIII =
            {
                new CoordIjk(5, 4, 0),  // 0
                new CoordIjk(1, 5, 0),  // 1
                new CoordIjk(0, 5, 4),  // 2
                new CoordIjk(0, 1, 5),  // 3
                new CoordIjk(4, 0, 5),  // 4
                new CoordIjk(5, 0, 1)   // 5
            };

            // get the correct set of substrate vertices for this resolution
            var verts = H3Index.isResClassIII(res)
                            ? vertsCIII
                            : vertsCII;

            // adjust the center point to be in an aperture 33r substrate grid
            // these should be composed for speed
            var fCoord = fijk.Coord;
            CoordIjk._downAp3(ref fCoord);
            CoordIjk._downAp3r(ref fCoord);
            fijk.Coord = new CoordIjk(fCoord.I, fCoord.J, fCoord.K);

            // if res is Class III we need to add a cw aperture 7 to get to
            // icosahedral Class II
            if (H3Index.isResClassIII(res))
            {
                fCoord = fijk.Coord;
                CoordIjk._downAp7r(ref fCoord);
                fijk.Coord = new CoordIjk(fCoord.I, fCoord.J, fCoord.K);
                res++;
            }

            // The center point is now in the same substrate grid as the origin
            // cell vertices. Add the center point substrate coordinates
            // to each vertex to translate the vertices to that cell.
            for (int v = 0; v < Constants.NUM_HEX_VERTS; v++)
            {
                fijkVerts[v].Face = fijk.Face;
                var fvCoord = fijkVerts[v].Coord;
                CoordIjk._ijkAdd(fijk.Coord, verts[v], ref fvCoord);
                CoordIjk._ijkNormalize(ref fvCoord);
                fijkVerts[v].Coord = new CoordIjk(fvCoord.I, fvCoord.J, fvCoord.K);
            }
        }
    
        /// <summary>
        /// Adjusts a FaceIJK address in place so that the resulting cell address is
        /// relative to the correct icosahedral face.
        /// </summary>
        /// <param name="fijk">The FaceIJK address of the cell.</param>
        /// <param name="res">The H3 resolution of the cell.</param>
        /// <param name="pentLeading4">Whether or not the cell is a pentagon with a leading digit 4</param>
        /// <param name="substrate">Whether or not the cell is in a substrate grid.</param>
        /// <returns>
        /// 0 if on original face (no overage); 1 if on face edge (only occurs
        /// on substrate grids); 2 if overage on new face interior
        /// </returns>
        public static Overage _adjustOverageClassII(ref FaceIjk fijk, int res, int pentLeading4, int substrate)
        {
            var overage = Overage.NO_OVERAGE;
            var ijk = new CoordIjk(fijk.Coord.I, fijk.Coord.J, fijk.Coord.K);

            // get the maximum dimension value; scale if a substrate grid
            int maxDim = maxDimByCIIres[res];
            if (substrate != 0)
            {
                maxDim *= 3;
            }

            // check for overage
            if ((substrate != 0) && ijk.I + ijk.J + ijk.K == maxDim) // on edge
            {
                overage = Overage.FACE_EDGE;
            }
            else if (ijk.I + ijk.J + ijk.K > maxDim)  // overage
            {
                overage = Overage.FACE_EDGE;

                FaceOrientIjk fijkOrient;
                if (ijk.K > 0)
                {
                    if (ijk.J > 0) // jk "quadrant"
                    {
                        fijkOrient = new FaceOrientIjk(
                                     faceNeighbors[fijk.Face,JK].FaceOrientIjk1.Face,
                                     faceNeighbors[fijk.Face,JK].FaceOrientIjk1.Translate.I,
                                     faceNeighbors[fijk.Face,JK].FaceOrientIjk1.Translate.J,
                                     faceNeighbors[fijk.Face,JK].FaceOrientIjk1.Translate.K,
                                     faceNeighbors[fijk.Face,JK].FaceOrientIjk1.CcwRot60);
                    }
                    else  // ik "quadrant"
                    {
                        fijkOrient = new FaceOrientIjk(
                                                       faceNeighbors[fijk.Face,KI].FaceOrientIjk1.Face,
                                                       faceNeighbors[fijk.Face,KI].FaceOrientIjk1.Translate.I,
                                                       faceNeighbors[fijk.Face,KI].FaceOrientIjk1.Translate.J,
                                                       faceNeighbors[fijk.Face,KI].FaceOrientIjk1.Translate.K,
                                                       faceNeighbors[fijk.Face,KI].FaceOrientIjk1.CcwRot60);

                        // adjust for the pentagonal missing sequence
                        if (pentLeading4!=0)
                        {
                            // translate origin to center of pentagon
                            var origin = new CoordIjk();
                            CoordIjk._setIJK(ref origin, maxDim, 0, 0);
                            var tmp = new CoordIjk();
                            CoordIjk._ijkSub(ref ijk, ref origin, ref tmp);
                            // rotate to adjust for the missing sequence
                            CoordIjk._ijkRotate60cw(ref tmp);
                            // translate the origin back to the center of the triangle
                            CoordIjk._ijkAdd(tmp, origin, ref ijk);
                        }
                    }
                }
                else // ij "quadrant"
                {
                    fijkOrient = new FaceOrientIjk(
                                                   faceNeighbors[fijk.Face,IJ].FaceOrientIjk1.Face,
                                                   faceNeighbors[fijk.Face,IJ].FaceOrientIjk1.Translate.I,
                                                   faceNeighbors[fijk.Face,IJ].FaceOrientIjk1.Translate.J,
                                                   faceNeighbors[fijk.Face,IJ].FaceOrientIjk1.Translate.K,
                                                   faceNeighbors[fijk.Face,IJ].FaceOrientIjk1.CcwRot60);

                }

                fijk.Face = fijkOrient.FaceOrientIjk1.Face;

                // rotate and translate for adjacent face
                for (var i = 0; i < fijkOrient.FaceOrientIjk1.CcwRot60; i++)
                {
                    CoordIjk ._ijkRotate60ccw(ref ijk);
                }

                var transVec = new CoordIjk
                    (
                     fijkOrient.FaceOrientIjk1.Translate.I,
                     fijkOrient.FaceOrientIjk1.Translate.J,
                     fijkOrient.FaceOrientIjk1.Translate.K
                    );
                int unitScale = unitScaleByCIIres[res];
                if (substrate!=0)
                {
                    unitScale *= 3;
                }
                CoordIjk._ijkScale(ref transVec, unitScale);
                CoordIjk._ijkAdd(ijk, transVec, ref ijk);
                CoordIjk._ijkNormalize(ref ijk);

                // overage points on pentagon boundaries can end up on edges
                if ((substrate != 0) && ijk.I + ijk.J + ijk.K == maxDim) // on edge
                {
                    overage = Overage.FACE_EDGE;
                }
            }

            fijk.Coord = ijk;
            return overage;
        }

        /// <summary>
        /// Adjusts a FaceIJK address for a pentagon vertex in a substrate grid in
        /// place so that the resulting cell address is relative to the correct
        /// icosahedral face.
        /// </summary>
        /// <param name="fijk">The FaceIJK address of the cell.</param>
        /// <param name="res">The H3 resolution of the cell.</param>
        public static Overage _adjustPentVertOverage(ref FaceIjk fijk, int res)
        {
            int pentLeading4 = 0;
            Overage overage;
            do
            {
                overage = _adjustOverageClassII(ref fijk, res, pentLeading4, 1);
            } while (overage == Overage.NEW_FACE);

            return overage;
        }
    }
    
}
