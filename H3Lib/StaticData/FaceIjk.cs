namespace H3Lib.StaticData
{
    public static class FaceIjk
    {
        /// <summary>
        /// Invalid face index
        /// </summary>
        public static readonly int InvalidFace = -1;

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
        public static readonly double MSqrt7 = 2.6457513110645905905016157536392604257102;

        /// <summary>
        /// icosahedron face centers in lat/lon radians
        /// </summary>
        public static readonly H3Lib.GeoCoord[] FaceCenterGeo =
        {
            new H3Lib.GeoCoord(0.803582649718989942, 1.248397419617396099), // face  0
            new H3Lib.GeoCoord(1.307747883455638156, 2.536945009877921159), // face  1
            new H3Lib.GeoCoord(1.054751253523952054, -1.347517358900396623), // face  2
            new H3Lib.GeoCoord(0.600191595538186799, -0.450603909469755746), // face  3
            new H3Lib.GeoCoord(0.491715428198773866, 0.401988202911306943), // face  4
            new H3Lib.GeoCoord(0.172745327415618701, 1.678146885280433686), // face  5
            new H3Lib.GeoCoord(0.605929321571350690, 2.953923329812411617), // face  6
            new H3Lib.GeoCoord(0.427370518328979641, -1.888876200336285401), // face  7
            new H3Lib.GeoCoord(-0.079066118549212831, -0.733429513380867741), // face  8
            new H3Lib.GeoCoord(-0.230961644455383637, 0.506495587332349035), // face  9
            new H3Lib.GeoCoord(0.079066118549212831, 2.408163140208925497), // face 10
            new H3Lib.GeoCoord(0.230961644455383637, -2.635097066257444203), // face 11
            new H3Lib.GeoCoord(-0.172745327415618701, -1.463445768309359553), // face 12
            new H3Lib.GeoCoord(-0.605929321571350690, -0.187669323777381622), // face 13
            new H3Lib.GeoCoord(-0.427370518328979641, 1.252716453253507838), // face 14
            new H3Lib.GeoCoord(-0.600191595538186799, 2.690988744120037492), // face 15
            new H3Lib.GeoCoord(-0.491715428198773866, -2.739604450678486295), // face 16
            new H3Lib.GeoCoord(-0.803582649718989942, -1.893195233972397139), // face 17
            new H3Lib.GeoCoord(-1.307747883455638156, -0.604647643711872080), // face 18
            new H3Lib.GeoCoord(-1.054751253523952054, 1.794075294689396615), // face 19        };
        };

        /// <summary>
        /// icosahedron face centers in x/y/z on the unit sphere
        /// </summary>
        public static readonly Vec3d[] FaceCenterPoint =
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
        public static readonly double[,] FaceAxesAzRadsCii =
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
        public static readonly FaceOrientIjk[,] FaceNeighbors =
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
        public static readonly int[,] AdjacentFaceDir =
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
        public static readonly int[] MaxDimByCiiRes =
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
        public static readonly int[] UnitScaleByCiiRes =
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
    }
}
