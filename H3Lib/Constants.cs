// ReSharper disable InconsistentNaming
using System.Runtime.CompilerServices;
using System.Collections.Generic;

[assembly: InternalsVisibleTo("TestSuite")]
namespace H3Lib
{
    /// <summary>
    /// Collection of constants used throughout the library.
    /// </summary>
    public static class Constants
    {
        /*
         * The following are taken from https://github.com/uber/h3/issues/148
         * to indicate the functional equivalence of this C# library.
         */

        /// <summary>
        /// Major version
        /// </summary>
        internal const int H3_VERSION_MAJOR = 3;
        /// <summary>
        /// Minor version
        /// </summary>
        internal const int H3_VERSION_MINOR = 7;
        /// <summary>
        /// Patch version
        /// </summary>
        internal const int H3_VERSION_PATCH = 1;

        /// <summary>
        /// Internal values for all of H3
        /// </summary>
        public static class H3
        {
            /// <summary>
            /// Pi
            /// </summary>
            internal const decimal M_PI = 3.14159265358979323846m;

            /// <summary>
            /// Pi / 2.0
            /// </summary>
            internal const decimal M_PI_2 = 1.5707963267948966m;

            /// <summary>
            /// Pi * 2.0
            /// </summary>
            internal const decimal M_2PI = 6.28318530717958647692528676655900576839433m;

            /// <summary>
            /// Pi / 180
            /// </summary>
            internal const decimal M_PI_180 = 0.0174532925199432957692369076848861271111m;

            /// <summary>
            /// 180 / Pi
            /// </summary>
            internal const decimal M_180_PI = 57.29577951308232087679815481410517033240547m;

            /// <summary>
            /// Threshold epsilon
            /// </summary>
            internal const decimal EPSILON = 0.0000000000000001m;

            /// <summary>
            /// Sqrt(3) / 2.0
            /// </summary>
            internal const decimal M_SQRT3_2 = 0.8660254037844386467637231707529361834714m;

            /// <summary>
            /// sin(60 degrees)
            /// </summary>
            internal const decimal M_SIN60 = M_SQRT3_2;

            /// <summary>
            /// Rotation angle between Class II and Class III resolution axes
            /// asin(sqrt(3.0 / 28.0 ))
            /// </summary>
            internal const decimal M_AP7_ROT_RADS = 0.333473172251832115336090755351601070065900389m;

            /// <summary>
            /// sin(<see cref="M_AP7_ROT_RADS"/>
            /// </summary>
            internal const decimal M_SIN_AP7_ROT = 0.3273268353539885718950318m;

            /// <summary>
            /// cos(<see cref="M_AP7_ROT_RADS"/>
            /// </summary>
            internal const decimal M_COS_AP7_ROT = 0.9449111825230680680167902m;

            /// <summary>
            /// Earth radius in kilometers using WGS84 authalic radius
            /// </summary>
            internal const decimal EARTH_RADIUS_KM = 6371.007180918475m;

            /// <summary>
            /// Scaling factor from hex2d resolution 0 unit length
            /// (or distance between adjacent cell center points on the place)
            /// to gnomonic unit length.
            /// </summary>
            internal const decimal RES0_U_GNOMONIC = 0.38196601125010500003m;

            /// <summary>
            /// H3 resolution; H3 version 1 has 16 resolutions, numbered 0 through 15
            /// </summary>
            internal const int MAX_H3_RES = 15;

            /// <summary>
            /// The number of faces on an icosahedron
            /// </summary>
            internal const int NUM_ICOSA_FACES = 20;

            /// <summary>
            /// The number of H3 base cells
            /// </summary>
            internal const int NUM_BASE_CELLS = 122;

            /// <summary>
            /// The number of vertices in a hexagon;
            /// </summary>
            internal const int NUM_HEX_VERTS = 6;

            /// <summary>
            /// The number of vertices in a pentagon
            /// </summary>
            internal const int NUM_PENT_VERTS = 5;

            /// <summary>
            /// H3 Index modes
            /// </summary>
            internal const int H3_HEXAGON_MODE = 1;

            internal const int H3_UNIEDGE_MODE = 2;

            /// <summary>
            /// epsilon of ~0.1mm in degrees
            /// </summary>
            internal const decimal EPSILON_DEG =  0.00000001m;
            
            /// <summary>
            /// epsilon of ~0.1mm in radians
            /// </summary>
            internal const decimal EPSILON_RAD = EPSILON_DEG * M_PI_180;

            internal const int MAX_CELL_BNDRY_VERTS = 10;

            // /// <summary>
            // /// Return codes from Algos.hexRange and related functions.
            // /// </summary>
            // internal const int HEX_RANGE_SUCCESS = 0;
            //
            // internal const int HEX_RANGE_PENTAGON = 1;
            // internal const int HEX_RANGE_K_SUBSEQUENCE = 1;

            /// <summary>
            /// General margin of error in differences between double values
            /// Original value was 2.2204460492503131e-16;
            ///
            /// This is relevant to accuracy's interest, methinks
            /// https://github.com/dotnet/runtime/issues/8528
            /// 
            /// </summary>
            internal const decimal DBL_EPSILON = 0.0000000001m;

            /// <summary>
            /// Direction used for traversing to the next outward hexagonal ring. 
            /// </summary>
            internal const Direction NEXT_RING_DIRECTION = Direction.I_AXES_DIGIT;

            internal const int NUM_PENTAGONS = 12;
        }

        internal static class Algos
        {
            /*
             * Return codes from hexRange and related functions.
            */
            internal const int HexRangeSuccess = 0;
            internal const int HexRangePentagon = 1;
            internal const int HexRangeKSubsequence = 2;
            internal const int MaxOneRingSize = 7;
            internal const int HexHashOverflow = -1;
            internal const int PolyfillBuffer = 12;

            /// <summary>
            /// <code>
            ///      _
            ///    _/ \_      Directions used for traversing a        
            ///   / \5/ \     hexagonal ring counterclockwise
            ///   \0/ \4/     around {1, 0, 0}
            ///   / \_/ \
            ///   \1/ \3/
            ///     \2/
            /// </code>
            /// </summary>
            /// <!--
            /// algos.c
            /// -->
            internal static readonly Direction[] Directions =
            {
                Direction.J_AXES_DIGIT, Direction.JK_AXES_DIGIT,
                Direction.K_AXES_DIGIT, Direction.IK_AXES_DIGIT,
                Direction.I_AXES_DIGIT, Direction.IJ_AXES_DIGIT
            };

            /// <summary>
            /// Direction used for traversing to the next outward hexagonal ring.
            /// </summary>
            /// <!--
            /// Algos.c
            /// NEXT_RING_DIRECTION
            /// -->
            internal const Direction NextRingDirection = Direction.I_AXES_DIGIT;

            /// <summary>
            /// New digit when traversing along class II grids.
            ///
            /// Current digit -> direction -> new digit.
            /// </summary>
            /// <!--
            /// Algos.c
            /// NEW_DIGIT_II
            /// -->
            internal static readonly Direction[,] NewDigitIi =
            {
                {
                    Direction.CENTER_DIGIT, Direction.K_AXES_DIGIT, Direction.J_AXES_DIGIT,
                    Direction.JK_AXES_DIGIT, Direction.I_AXES_DIGIT, Direction.IK_AXES_DIGIT,
                    Direction.IJ_AXES_DIGIT
                },
                {
                    Direction.K_AXES_DIGIT, Direction.I_AXES_DIGIT, Direction.JK_AXES_DIGIT,
                    Direction.IJ_AXES_DIGIT, Direction.IK_AXES_DIGIT, Direction.J_AXES_DIGIT,
                    Direction.CENTER_DIGIT
                },
                {
                    Direction.J_AXES_DIGIT, Direction.JK_AXES_DIGIT, Direction.K_AXES_DIGIT,
                    Direction.I_AXES_DIGIT, Direction.IJ_AXES_DIGIT, Direction.CENTER_DIGIT,
                    Direction.IK_AXES_DIGIT
                },
                {
                    Direction.JK_AXES_DIGIT, Direction.IJ_AXES_DIGIT, Direction.I_AXES_DIGIT,
                    Direction.IK_AXES_DIGIT, Direction.CENTER_DIGIT, Direction.K_AXES_DIGIT,
                    Direction.J_AXES_DIGIT
                },
                {
                    Direction.I_AXES_DIGIT, Direction.IK_AXES_DIGIT, Direction.IJ_AXES_DIGIT,
                    Direction.CENTER_DIGIT, Direction.J_AXES_DIGIT, Direction.JK_AXES_DIGIT,
                    Direction.K_AXES_DIGIT
                },
                {
                    Direction.IK_AXES_DIGIT, Direction.J_AXES_DIGIT, Direction.CENTER_DIGIT,
                    Direction.K_AXES_DIGIT, Direction.JK_AXES_DIGIT, Direction.IJ_AXES_DIGIT,
                    Direction.I_AXES_DIGIT
                },
                {
                    Direction.IJ_AXES_DIGIT, Direction.CENTER_DIGIT, Direction.IK_AXES_DIGIT,
                    Direction.J_AXES_DIGIT, Direction.K_AXES_DIGIT, Direction.I_AXES_DIGIT,
                    Direction.JK_AXES_DIGIT
                }
            };

            /// <summary>
            /// New traversal direction when traversing along class II grids.
            ///
            /// Current digit -> direction -> new ap7 move (at coarser level).
            /// </summary>
            /// <!--
            /// Algos.c
            /// NEW_ADJUSTMENT_II
            /// -->
            internal static readonly Direction[,] NewAdjustmentIi =
            {
                {
                    Direction.CENTER_DIGIT, Direction.CENTER_DIGIT, Direction.CENTER_DIGIT,
                    Direction.CENTER_DIGIT, Direction.CENTER_DIGIT, Direction.CENTER_DIGIT,
                    Direction.CENTER_DIGIT
                },
                {
                    Direction.CENTER_DIGIT, Direction.K_AXES_DIGIT, Direction.CENTER_DIGIT,
                    Direction.K_AXES_DIGIT, Direction.CENTER_DIGIT, Direction.IK_AXES_DIGIT,
                    Direction.CENTER_DIGIT
                },
                {
                    Direction.CENTER_DIGIT, Direction.CENTER_DIGIT, Direction.J_AXES_DIGIT,
                    Direction.JK_AXES_DIGIT, Direction.CENTER_DIGIT, Direction.CENTER_DIGIT,
                    Direction.J_AXES_DIGIT
                },
                {
                    Direction.CENTER_DIGIT, Direction.K_AXES_DIGIT, Direction.JK_AXES_DIGIT,
                    Direction.JK_AXES_DIGIT, Direction.CENTER_DIGIT, Direction.CENTER_DIGIT,
                    Direction.CENTER_DIGIT
                },
                {
                    Direction.CENTER_DIGIT, Direction.CENTER_DIGIT, Direction.CENTER_DIGIT,
                    Direction.CENTER_DIGIT, Direction.I_AXES_DIGIT, Direction.I_AXES_DIGIT,
                    Direction.IJ_AXES_DIGIT
                },
                {
                    Direction.CENTER_DIGIT, Direction.IK_AXES_DIGIT, Direction.CENTER_DIGIT,
                    Direction.CENTER_DIGIT, Direction.I_AXES_DIGIT, Direction.IK_AXES_DIGIT,
                    Direction.CENTER_DIGIT
                },
                {
                    Direction.CENTER_DIGIT, Direction.CENTER_DIGIT, Direction.J_AXES_DIGIT,
                    Direction.CENTER_DIGIT, Direction.IJ_AXES_DIGIT, Direction.CENTER_DIGIT,
                    Direction.IJ_AXES_DIGIT
                }
            };

            /// <summary>
            /// New traversal direction when traversing along class III grids.
            ///
            /// Current digit -&gt; direction -&gt; new ap7 move (at coarser level).
            /// </summary>
            /// <!--
            /// Algos.c
            /// NEW_DIGIT_III
            /// -->
            internal static readonly Direction[,] NewDigitIii =
            {
                {
                    Direction.CENTER_DIGIT, Direction.K_AXES_DIGIT, Direction.J_AXES_DIGIT,
                    Direction.JK_AXES_DIGIT, Direction.I_AXES_DIGIT, Direction.IK_AXES_DIGIT,
                    Direction.IJ_AXES_DIGIT
                },
                {
                    Direction.K_AXES_DIGIT, Direction.J_AXES_DIGIT, Direction.JK_AXES_DIGIT,
                    Direction.I_AXES_DIGIT, Direction.IK_AXES_DIGIT, Direction.IJ_AXES_DIGIT,
                    Direction.CENTER_DIGIT
                },
                {
                    Direction.J_AXES_DIGIT, Direction.JK_AXES_DIGIT, Direction.I_AXES_DIGIT,
                    Direction.IK_AXES_DIGIT, Direction.IJ_AXES_DIGIT, Direction.CENTER_DIGIT,
                    Direction.K_AXES_DIGIT
                },
                {
                    Direction.JK_AXES_DIGIT, Direction.I_AXES_DIGIT, Direction.IK_AXES_DIGIT,
                    Direction.IJ_AXES_DIGIT, Direction.CENTER_DIGIT, Direction.K_AXES_DIGIT,
                    Direction.J_AXES_DIGIT
                },
                {
                    Direction.I_AXES_DIGIT, Direction.IK_AXES_DIGIT, Direction.IJ_AXES_DIGIT,
                    Direction.CENTER_DIGIT, Direction.K_AXES_DIGIT, Direction.J_AXES_DIGIT,
                    Direction.JK_AXES_DIGIT
                },
                {
                    Direction.IK_AXES_DIGIT, Direction.IJ_AXES_DIGIT, Direction.CENTER_DIGIT,
                    Direction.K_AXES_DIGIT, Direction.J_AXES_DIGIT, Direction.JK_AXES_DIGIT,
                    Direction.I_AXES_DIGIT
                },
                {
                    Direction.IJ_AXES_DIGIT, Direction.CENTER_DIGIT, Direction.K_AXES_DIGIT,
                    Direction.J_AXES_DIGIT, Direction.JK_AXES_DIGIT, Direction.I_AXES_DIGIT,
                    Direction.IK_AXES_DIGIT
                }
            };

            /// <summary>
            /// New traversal direction when traversing along class III grids.
            ///
            /// Current digit -gt; direction -gt; new ap7 move (at coarser level).
            /// </summary>
            /// <!--
            /// algos.c
            /// NEW_ADJUSTMENT_III
            /// -->
            internal static readonly Direction[,] NewAdjustmentIii =
            {
                {
                    Direction.CENTER_DIGIT, Direction.CENTER_DIGIT, Direction.CENTER_DIGIT,
                    Direction.CENTER_DIGIT, Direction.CENTER_DIGIT, Direction.CENTER_DIGIT,
                    Direction.CENTER_DIGIT
                },
                {
                    Direction.CENTER_DIGIT, Direction.K_AXES_DIGIT, Direction.CENTER_DIGIT,
                    Direction.JK_AXES_DIGIT, Direction.CENTER_DIGIT, Direction.K_AXES_DIGIT,
                    Direction.CENTER_DIGIT
                },
                {
                    Direction.CENTER_DIGIT, Direction.CENTER_DIGIT, Direction.J_AXES_DIGIT,
                    Direction.J_AXES_DIGIT, Direction.CENTER_DIGIT, Direction.CENTER_DIGIT,
                    Direction.IJ_AXES_DIGIT
                },
                {
                    Direction.CENTER_DIGIT, Direction.JK_AXES_DIGIT, Direction.J_AXES_DIGIT,
                    Direction.JK_AXES_DIGIT, Direction.CENTER_DIGIT, Direction.CENTER_DIGIT,
                    Direction.CENTER_DIGIT
                },
                {
                    Direction.CENTER_DIGIT, Direction.CENTER_DIGIT, Direction.CENTER_DIGIT,
                    Direction.CENTER_DIGIT, Direction.I_AXES_DIGIT, Direction.IK_AXES_DIGIT,
                    Direction.I_AXES_DIGIT
                },
                {
                    Direction.CENTER_DIGIT, Direction.K_AXES_DIGIT, Direction.CENTER_DIGIT,
                    Direction.CENTER_DIGIT, Direction.IK_AXES_DIGIT, Direction.IK_AXES_DIGIT,
                    Direction.CENTER_DIGIT
                },
                {
                    Direction.CENTER_DIGIT, Direction.CENTER_DIGIT, Direction.IJ_AXES_DIGIT,
                    Direction.CENTER_DIGIT, Direction.I_AXES_DIGIT, Direction.CENTER_DIGIT,
                    Direction.IJ_AXES_DIGIT
                }
            };

        }

        internal static class BaseCells
        {
            internal const int InvalidBaseCell = 127;

            /// <summary>
            /// Maximum input for any component to face-to-base-cell lookup functions
            /// </summary>
            internal const int MaxFaceCoord = 2;

            /// <summary>
            /// Invalid number of rotations
            /// </summary>
            internal const int InvalidRotations = -1;

            /// <summary>
            /// Neighboring base cell ID in each IJK direction.
            ///
            /// For each base cell, for each direction, the neighboring base
            /// cell ID is given. 127 indicates there is no neighbor in that direction.
            /// </summary>
            internal static readonly int[,] BaseCellNeighbors =
            {
                {0, 1, 5, 2, 4, 3, 8}, // base cell 0
                {1, 7, 6, 9, 0, 3, 2}, // base cell 1
                {2, 6, 10, 11, 0, 1, 5}, // base cell 2
                {3, 13, 1, 7, 4, 12, 0}, // base cell 3
                {4, InvalidBaseCell, 15, 8, 3, 0, 12}, // base cell 4 (pentagon)
                {5, 2, 18, 10, 8, 0, 16}, // base cell 5
                {6, 14, 11, 17, 1, 9, 2}, // base cell 6
                {7, 21, 9, 19, 3, 13, 1}, // base cell 7
                {8, 5, 22, 16, 4, 0, 15}, // base cell 8
                {9, 19, 14, 20, 1, 7, 6}, // base cell 9
                {10, 11, 24, 23, 5, 2, 18}, // base cell 10
                {11, 17, 23, 25, 2, 6, 10}, // base cell 11
                {12, 28, 13, 26, 4, 15, 3}, // base cell 12
                {13, 26, 21, 29, 3, 12, 7}, // base cell 13
                {14, InvalidBaseCell, 17, 27, 9, 20, 6}, // base cell 14 (pentagon)
                {15, 22, 28, 31, 4, 8, 12}, // base cell 15
                {16, 18, 33, 30, 8, 5, 22}, // base cell 16
                {17, 11, 14, 6, 35, 25, 27}, // base cell 17
                {18, 24, 30, 32, 5, 10, 16}, // base cell 18
                {19, 34, 20, 36, 7, 21, 9}, // base cell 19
                {20, 14, 19, 9, 40, 27, 36}, // base cell 20
                {21, 38, 19, 34, 13, 29, 7}, // base cell 21
                {22, 16, 41, 33, 15, 8, 31}, // base cell 22
                {23, 24, 11, 10, 39, 37, 25}, // base cell 23
                {24, InvalidBaseCell, 32, 37, 10, 23, 18}, // base cell 24 (pentagon)
                {25, 23, 17, 11, 45, 39, 35}, // base cell 25
                {26, 42, 29, 43, 12, 28, 13}, // base cell 26
                {27, 40, 35, 46, 14, 20, 17}, // base cell 27
                {28, 31, 42, 44, 12, 15, 26}, // base cell 28
                {29, 43, 38, 47, 13, 26, 21}, // base cell 29
                {30, 32, 48, 50, 16, 18, 33}, // base cell 30
                {31, 41, 44, 53, 15, 22, 28}, // base cell 31
                {32, 30, 24, 18, 52, 50, 37}, // base cell 32
                {33, 30, 49, 48, 22, 16, 41}, // base cell 33
                {34, 19, 38, 21, 54, 36, 51}, // base cell 34
                {35, 46, 45, 56, 17, 27, 25}, // base cell 35
                {36, 20, 34, 19, 55, 40, 54}, // base cell 36
                {37, 39, 52, 57, 24, 23, 32}, // base cell 37
                {38, InvalidBaseCell, 34, 51, 29, 47, 21}, // base cell 38 (pentagon)
                {39, 37, 25, 23, 59, 57, 45}, // base cell 39
                {40, 27, 36, 20, 60, 46, 55}, // base cell 40
                {41, 49, 53, 61, 22, 33, 31}, // base cell 41
                {42, 58, 43, 62, 28, 44, 26}, // base cell 42
                {43, 62, 47, 64, 26, 42, 29}, // base cell 43
                {44, 53, 58, 65, 28, 31, 42}, // base cell 44
                {45, 39, 35, 25, 63, 59, 56}, // base cell 45
                {46, 60, 56, 68, 27, 40, 35}, // base cell 46
                {47, 38, 43, 29, 69, 51, 64}, // base cell 47
                {48, 49, 30, 33, 67, 66, 50}, // base cell 48
                {49, InvalidBaseCell, 61, 66, 33, 48, 41}, // base cell 49 (pentagon)
                {50, 48, 32, 30, 70, 67, 52}, // base cell 50
                {51, 69, 54, 71, 38, 47, 34}, // base cell 51
                {52, 57, 70, 74, 32, 37, 50}, // base cell 52
                {53, 61, 65, 75, 31, 41, 44}, // base cell 53
                {54, 71, 55, 73, 34, 51, 36}, // base cell 54
                {55, 40, 54, 36, 72, 60, 73}, // base cell 55
                {56, 68, 63, 77, 35, 46, 45}, // base cell 56
                {57, 59, 74, 78, 37, 39, 52}, // base cell 57
                {58, InvalidBaseCell, 62, 76, 44, 65, 42}, // base cell 58 (pentagon)
                {59, 63, 78, 79, 39, 45, 57}, // base cell 59
                {60, 72, 68, 80, 40, 55, 46}, // base cell 60
                {61, 53, 49, 41, 81, 75, 66}, // base cell 61
                {62, 43, 58, 42, 82, 64, 76}, // base cell 62
                {63, InvalidBaseCell, 56, 45, 79, 59, 77}, // base cell 63 (pentagon)
                {64, 47, 62, 43, 84, 69, 82}, // base cell 64
                {65, 58, 53, 44, 86, 76, 75}, // base cell 65
                {66, 67, 81, 85, 49, 48, 61}, // base cell 66
                {67, 66, 50, 48, 87, 85, 70}, // base cell 67
                {68, 56, 60, 46, 90, 77, 80}, // base cell 68
                {69, 51, 64, 47, 89, 71, 84}, // base cell 69
                {70, 67, 52, 50, 83, 87, 74}, // base cell 70
                {71, 89, 73, 91, 51, 69, 54}, // base cell 71
                {72, InvalidBaseCell, 73, 55, 80, 60, 88}, // base cell 72 (pentagon)
                {73, 91, 72, 88, 54, 71, 55}, // base cell 73
                {74, 78, 83, 92, 52, 57, 70}, // base cell 74
                {75, 65, 61, 53, 94, 86, 81}, // base cell 75
                {76, 86, 82, 96, 58, 65, 62}, // base cell 76
                {77, 63, 68, 56, 93, 79, 90}, // base cell 77
                {78, 74, 59, 57, 95, 92, 79}, // base cell 78
                {79, 78, 63, 59, 93, 95, 77}, // base cell 79
                {80, 68, 72, 60, 99, 90, 88}, // base cell 80
                {81, 85, 94, 101, 61, 66, 75}, // base cell 81
                {82, 96, 84, 98, 62, 76, 64}, // base cell 82
                {83, InvalidBaseCell, 74, 70, 100, 87, 92}, // base cell 83 (pentagon)
                {84, 69, 82, 64, 97, 89, 98}, // base cell 84
                {85, 87, 101, 102, 66, 67, 81}, // base cell 85
                {86, 76, 75, 65, 104, 96, 94}, // base cell 86
                {87, 83, 102, 100, 67, 70, 85}, // base cell 87
                {88, 72, 91, 73, 99, 80, 105}, // base cell 88
                {89, 97, 91, 103, 69, 84, 71}, // base cell 89
                {90, 77, 80, 68, 106, 93, 99}, // base cell 90
                {91, 73, 89, 71, 105, 88, 103}, // base cell 91
                {92, 83, 78, 74, 108, 100, 95}, // base cell 92
                {93, 79, 90, 77, 109, 95, 106}, // base cell 93
                {94, 86, 81, 75, 107, 104, 101}, // base cell 94
                {95, 92, 79, 78, 109, 108, 93}, // base cell 95
                {96, 104, 98, 110, 76, 86, 82}, // base cell 96
                {97, InvalidBaseCell, 98, 84, 103, 89, 111}, // base cell 97 (pentagon)
                {98, 110, 97, 111, 82, 96, 84}, // base cell 98
                {99, 80, 105, 88, 106, 90, 113}, // base cell 99
                {100, 102, 83, 87, 108, 114, 92}, // base cell 100
                {101, 102, 107, 112, 81, 85, 94}, // base cell 101
                {102, 101, 87, 85, 114, 112, 100}, // base cell 102
                {103, 91, 97, 89, 116, 105, 111}, // base cell 103
                {104, 107, 110, 115, 86, 94, 96}, // base cell 104
                {105, 88, 103, 91, 113, 99, 116}, // base cell 105
                {106, 93, 99, 90, 117, 109, 113}, // base cell 106
                {107, InvalidBaseCell, 101, 94, 115, 104, 112}, // base cell 107 (pentagon)
                {108, 100, 95, 92, 118, 114, 109}, // base cell 108
                {109, 108, 93, 95, 117, 118, 106}, // base cell 109
                {110, 98, 104, 96, 119, 111, 115}, // base cell 110
                {111, 97, 110, 98, 116, 103, 119}, // base cell 111
                {112, 107, 102, 101, 120, 115, 114}, // base cell 112
                {113, 99, 116, 105, 117, 106, 121}, // base cell 113
                {114, 112, 100, 102, 118, 120, 108}, // base cell 114
                {115, 110, 107, 104, 120, 119, 112}, // base cell 115
                {116, 103, 119, 111, 113, 105, 121}, // base cell 116
                {117, InvalidBaseCell, 109, 118, 113, 121, 106}, // base cell 117 (pentagon)
                {118, 120, 108, 114, 117, 121, 109}, // base cell 118
                {119, 111, 115, 110, 121, 116, 120}, // base cell 119
                {120, 115, 114, 112, 121, 119, 118}, // base cell 120
                {121, 116, 120, 119, 117, 113, 118}, // base cell 121
            };

            /// <summary>
            /// Neighboring base cell rotations in each IJK direction.
            ///
            /// For each base cell, for each direction, the number of 60 degree
            /// CCW rotations to the coordinate system of the neighbor is given.
            /// -1 indicates there is no neighbor in that direction.
            /// </summary>
            /// <!--
            /// baseCells.c
            /// baseCellNeighbor60CCWRots
            /// -->
            internal static readonly int[,] BaseCellNeighbor60CounterClockwiseRotation =
            {
                {0, 5, 0, 0, 1, 5, 1}, // base cell 0
                {0, 0, 1, 0, 1, 0, 1}, // base cell 1
                {0, 0, 0, 0, 0, 5, 0}, // base cell 2
                {0, 5, 0, 0, 2, 5, 1}, // base cell 3
                {0, -1, 1, 0, 3, 4, 2}, // base cell 4 (pentagon)
                {0, 0, 1, 0, 1, 0, 1}, // base cell 5
                {0, 0, 0, 3, 5, 5, 0}, // base cell 6
                {0, 0, 0, 0, 0, 5, 0}, // base cell 7
                {0, 5, 0, 0, 0, 5, 1}, // base cell 8
                {0, 0, 1, 3, 0, 0, 1}, // base cell 9
                {0, 0, 1, 3, 0, 0, 1}, // base cell 10
                {0, 3, 3, 3, 0, 0, 0}, // base cell 11
                {0, 5, 0, 0, 3, 5, 1}, // base cell 12
                {0, 0, 1, 0, 1, 0, 1}, // base cell 13
                {0, -1, 3, 0, 5, 2, 0}, // base cell 14 (pentagon)
                {0, 5, 0, 0, 4, 5, 1}, // base cell 15
                {0, 0, 0, 0, 0, 5, 0}, // base cell 16
                {0, 3, 3, 3, 3, 0, 3}, // base cell 17
                {0, 0, 0, 3, 5, 5, 0}, // base cell 18
                {0, 3, 3, 3, 0, 0, 0}, // base cell 19
                {0, 3, 3, 3, 0, 3, 0}, // base cell 20
                {0, 0, 0, 3, 5, 5, 0}, // base cell 21
                {0, 0, 1, 0, 1, 0, 1}, // base cell 22
                {0, 3, 3, 3, 0, 3, 0}, // base cell 23
                {0, -1, 3, 0, 5, 2, 0}, // base cell 24 (pentagon)
                {0, 0, 0, 3, 0, 0, 3}, // base cell 25
                {0, 0, 0, 0, 0, 5, 0}, // base cell 26
                {0, 3, 0, 0, 0, 3, 3}, // base cell 27
                {0, 0, 1, 0, 1, 0, 1}, // base cell 28
                {0, 0, 1, 3, 0, 0, 1}, // base cell 29
                {0, 3, 3, 3, 0, 0, 0}, // base cell 30
                {0, 0, 0, 0, 0, 5, 0}, // base cell 31
                {0, 3, 3, 3, 3, 0, 3}, // base cell 32
                {0, 0, 1, 3, 0, 0, 1}, // base cell 33
                {0, 3, 3, 3, 3, 0, 3}, // base cell 34
                {0, 0, 3, 0, 3, 0, 3}, // base cell 35
                {0, 0, 0, 3, 0, 0, 3}, // base cell 36
                {0, 3, 0, 0, 0, 3, 3}, // base cell 37
                {0, -1, 3, 0, 5, 2, 0}, // base cell 38 (pentagon)
                {0, 3, 0, 0, 3, 3, 0}, // base cell 39
                {0, 3, 0, 0, 3, 3, 0}, // base cell 40
                {0, 0, 0, 3, 5, 5, 0}, // base cell 41
                {0, 0, 0, 3, 5, 5, 0}, // base cell 42
                {0, 3, 3, 3, 0, 0, 0}, // base cell 43
                {0, 0, 1, 3, 0, 0, 1}, // base cell 44
                {0, 0, 3, 0, 0, 3, 3}, // base cell 45
                {0, 0, 0, 3, 0, 3, 0}, // base cell 46
                {0, 3, 3, 3, 0, 3, 0}, // base cell 47
                {0, 3, 3, 3, 0, 3, 0}, // base cell 48
                {0, -1, 3, 0, 5, 2, 0}, // base cell 49 (pentagon)
                {0, 0, 0, 3, 0, 0, 3}, // base cell 50
                {0, 3, 0, 0, 0, 3, 3}, // base cell 51
                {0, 0, 3, 0, 3, 0, 3}, // base cell 52
                {0, 3, 3, 3, 0, 0, 0}, // base cell 53
                {0, 0, 3, 0, 3, 0, 3}, // base cell 54
                {0, 0, 3, 0, 0, 3, 3}, // base cell 55
                {0, 3, 3, 3, 0, 0, 3}, // base cell 56
                {0, 0, 0, 3, 0, 3, 0}, // base cell 57
                {0, -1, 3, 0, 5, 2, 0}, // base cell 58 (pentagon)
                {0, 3, 3, 3, 3, 3, 0}, // base cell 59
                {0, 3, 3, 3, 3, 3, 0}, // base cell 60
                {0, 3, 3, 3, 3, 0, 3}, // base cell 61
                {0, 3, 3, 3, 3, 0, 3}, // base cell 62
                {0, -1, 3, 0, 5, 2, 0}, // base cell 63 (pentagon)
                {0, 0, 0, 3, 0, 0, 3}, // base cell 64
                {0, 3, 3, 3, 0, 3, 0}, // base cell 65
                {0, 3, 0, 0, 0, 3, 3}, // base cell 66
                {0, 3, 0, 0, 3, 3, 0}, // base cell 67
                {0, 3, 3, 3, 0, 0, 0}, // base cell 68
                {0, 3, 0, 0, 3, 3, 0}, // base cell 69
                {0, 0, 3, 0, 0, 3, 3}, // base cell 70
                {0, 0, 0, 3, 0, 3, 0}, // base cell 71
                {0, -1, 3, 0, 5, 2, 0}, // base cell 72 (pentagon)
                {0, 3, 3, 3, 0, 0, 3}, // base cell 73
                {0, 3, 3, 3, 0, 0, 3}, // base cell 74
                {0, 0, 0, 3, 0, 0, 3}, // base cell 75
                {0, 3, 0, 0, 0, 3, 3}, // base cell 76
                {0, 0, 0, 3, 0, 5, 0}, // base cell 77
                {0, 3, 3, 3, 0, 0, 0}, // base cell 78
                {0, 0, 1, 3, 1, 0, 1}, // base cell 79
                {0, 0, 1, 3, 1, 0, 1}, // base cell 80
                {0, 0, 3, 0, 3, 0, 3}, // base cell 81
                {0, 0, 3, 0, 3, 0, 3}, // base cell 82
                {0, -1, 3, 0, 5, 2, 0}, // base cell 83 (pentagon)
                {0, 0, 3, 0, 0, 3, 3}, // base cell 84
                {0, 0, 0, 3, 0, 3, 0}, // base cell 85
                {0, 3, 0, 0, 3, 3, 0}, // base cell 86
                {0, 3, 3, 3, 3, 3, 0}, // base cell 87
                {0, 0, 0, 3, 0, 5, 0}, // base cell 88
                {0, 3, 3, 3, 3, 3, 0}, // base cell 89
                {0, 0, 0, 0, 0, 0, 1}, // base cell 90
                {0, 3, 3, 3, 0, 0, 0}, // base cell 91
                {0, 0, 0, 3, 0, 5, 0}, // base cell 92
                {0, 5, 0, 0, 5, 5, 0}, // base cell 93
                {0, 0, 3, 0, 0, 3, 3}, // base cell 94
                {0, 0, 0, 0, 0, 0, 1}, // base cell 95
                {0, 0, 0, 3, 0, 3, 0}, // base cell 96
                {0, -1, 3, 0, 5, 2, 0}, // base cell 97 (pentagon)
                {0, 3, 3, 3, 0, 0, 3}, // base cell 98
                {0, 5, 0, 0, 5, 5, 0}, // base cell 99
                {0, 0, 1, 3, 1, 0, 1}, // base cell 100
                {0, 3, 3, 3, 0, 0, 3}, // base cell 101
                {0, 3, 3, 3, 0, 0, 0}, // base cell 102
                {0, 0, 1, 3, 1, 0, 1}, // base cell 103
                {0, 3, 3, 3, 3, 3, 0}, // base cell 104
                {0, 0, 0, 0, 0, 0, 1}, // base cell 105
                {0, 0, 1, 0, 3, 5, 1}, // base cell 106
                {0, -1, 3, 0, 5, 2, 0}, // base cell 107 (pentagon)
                {0, 5, 0, 0, 5, 5, 0}, // base cell 108
                {0, 0, 1, 0, 4, 5, 1}, // base cell 109
                {0, 3, 3, 3, 0, 0, 0}, // base cell 110
                {0, 0, 0, 3, 0, 5, 0}, // base cell 111
                {0, 0, 0, 3, 0, 5, 0}, // base cell 112
                {0, 0, 1, 0, 2, 5, 1}, // base cell 113
                {0, 0, 0, 0, 0, 0, 1}, // base cell 114
                {0, 0, 1, 3, 1, 0, 1}, // base cell 115
                {0, 5, 0, 0, 5, 5, 0}, // base cell 116
                {0, -1, 1, 0, 3, 4, 2}, // base cell 117 (pentagon)
                {0, 0, 1, 0, 0, 5, 1}, // base cell 118
                {0, 0, 0, 0, 0, 0, 1}, // base cell 119
                {0, 5, 0, 0, 5, 5, 0}, // base cell 120
                {0, 0, 1, 0, 1, 5, 1}, // base cell 121
            };

            /// <summary>
            /// Resolution 0 base cell lookup table for each face.
            ///
            /// Given the face number and a resolution 0 ijk+ coordinate in that face's
            /// face-centered ijk coordinate system, gives the base cell located at that
            /// coordinate and the number of 60 ccw rotations to rotate into that base
            /// cell's orientation.
            ///
            /// Valid lookup coordinates are from (0, 0, 0) to (2, 2, 2).
            ///
            /// This table can be accessed using the functions BaseCells._faceIjkToBaseCell
            /// and BaseCells.ToBaseCellCounterClockwiseRotate60
            /// </summary>
            internal static readonly BaseCellRotation[,,,] FaceIjkBaseCells =
            {
                {
                    // face 0
                    {
                        // i 0
                        {new BaseCellRotation(16, 0), new BaseCellRotation(18, 0), new BaseCellRotation(24, 0)}, // j 0
                        {new BaseCellRotation(33, 0), new BaseCellRotation(30, 0), new BaseCellRotation(32, 3)}, // j 1
                        {new BaseCellRotation(49, 1), new BaseCellRotation(48, 3), new BaseCellRotation(50, 3)} // j 2
                    },
                    {
                        // i 1
                        {new BaseCellRotation(8, 0), new BaseCellRotation(5, 5), new BaseCellRotation(10, 5)}, // j 0
                        {new BaseCellRotation(22, 0), new BaseCellRotation(16, 0), new BaseCellRotation(18, 0)}, // j 1
                        {new BaseCellRotation(41, 1), new BaseCellRotation(33, 0), new BaseCellRotation(30, 0)} // j 2
                    },
                    {
                        // i 2
                        {new BaseCellRotation(4, 0), new BaseCellRotation(0, 5), new BaseCellRotation(2, 5)}, // j 0
                        {new BaseCellRotation(15, 1), new BaseCellRotation(8, 0), new BaseCellRotation(5, 5)}, // j 1
                        {new BaseCellRotation(31, 1), new BaseCellRotation(22, 0), new BaseCellRotation(16, 0)} // j 2
                    }
                },
                {
                    // face 1
                    {
                        // i 0
                        {new BaseCellRotation(2, 0), new BaseCellRotation(6, 0), new BaseCellRotation(14, 0)}, // j 0
                        {new BaseCellRotation(10, 0), new BaseCellRotation(11, 0), new BaseCellRotation(17, 3)}, // j 1
                        {new BaseCellRotation(24, 1), new BaseCellRotation(23, 3), new BaseCellRotation(25, 3)} // j 2
                    },
                    {
                        // i 1
                        {new BaseCellRotation(0, 0), new BaseCellRotation(1, 5), new BaseCellRotation(9, 5)}, // j 0
                        {new BaseCellRotation(5, 0), new BaseCellRotation(2, 0), new BaseCellRotation(6, 0)}, // j 1
                        {new BaseCellRotation(18, 1), new BaseCellRotation(10, 0), new BaseCellRotation(11, 0)} // j 2
                    },
                    {
                        // i 2
                        {new BaseCellRotation(4, 1), new BaseCellRotation(3, 5), new BaseCellRotation(7, 5)}, // j 0
                        {new BaseCellRotation(8, 1), new BaseCellRotation(0, 0), new BaseCellRotation(1, 5)}, // j 1
                        {new BaseCellRotation(16, 1), new BaseCellRotation(5, 0), new BaseCellRotation(2, 0)} // j 2
                    }
                },
                {
                    // face 2
                    {
                        // i 0
                        {new BaseCellRotation(7, 0), new BaseCellRotation(21, 0), new BaseCellRotation(38, 0)}, // j 0
                        {new BaseCellRotation(9, 0), new BaseCellRotation(19, 0), new BaseCellRotation(34, 3)}, // j 1
                        {new BaseCellRotation(14, 1), new BaseCellRotation(20, 3), new BaseCellRotation(36, 3)} // j 2
                    },
                    {
                        // i 1
                        {new BaseCellRotation(3, 0), new BaseCellRotation(13, 5), new BaseCellRotation(29, 5)}, // j 0
                        {new BaseCellRotation(1, 0), new BaseCellRotation(7, 0), new BaseCellRotation(21, 0)}, // j 1
                        {new BaseCellRotation(6, 1), new BaseCellRotation(9, 0), new BaseCellRotation(19, 0)} // j 2
                    },
                    {
                        // i 2
                        {new BaseCellRotation(4, 2), new BaseCellRotation(12, 5), new BaseCellRotation(26, 5)}, // j 0
                        {new BaseCellRotation(0, 1), new BaseCellRotation(3, 0), new BaseCellRotation(13, 5)}, // j 1
                        {new BaseCellRotation(2, 1), new BaseCellRotation(1, 0), new BaseCellRotation(7, 0)} // j 2
                    }
                },
                {
                    // face 3
                    {
                        // i 0
                        {new BaseCellRotation(26, 0), new BaseCellRotation(42, 0), new BaseCellRotation(58, 0)}, // j 0
                        {new BaseCellRotation(29, 0), new BaseCellRotation(43, 0), new BaseCellRotation(62, 3)}, // j 1
                        {new BaseCellRotation(38, 1), new BaseCellRotation(47, 3), new BaseCellRotation(64, 3)} // j 2
                    },
                    {
                        // i 1
                        {new BaseCellRotation(12, 0), new BaseCellRotation(28, 5), new BaseCellRotation(44, 5)}, // j 0
                        {new BaseCellRotation(13, 0), new BaseCellRotation(26, 0), new BaseCellRotation(42, 0)}, // j 1
                        {new BaseCellRotation(21, 1), new BaseCellRotation(29, 0), new BaseCellRotation(43, 0)} // j 2
                    },
                    {
                        // i 2
                        {new BaseCellRotation(4, 3), new BaseCellRotation(15, 5), new BaseCellRotation(31, 5)}, // j 0
                        {new BaseCellRotation(3, 1), new BaseCellRotation(12, 0), new BaseCellRotation(28, 5)}, // j 1
                        {new BaseCellRotation(7, 1), new BaseCellRotation(13, 0), new BaseCellRotation(26, 0)} // j 2
                    }
                },
                {
                    // face 4
                    {
                        // i 0
                        {new BaseCellRotation(31, 0), new BaseCellRotation(41, 0), new BaseCellRotation(49, 0)}, // j 0
                        {new BaseCellRotation(44, 0), new BaseCellRotation(53, 0), new BaseCellRotation(61, 3)}, // j 1
                        {new BaseCellRotation(58, 1), new BaseCellRotation(65, 3), new BaseCellRotation(75, 3)} // j 2
                    },
                    {
                        // i 1
                        {new BaseCellRotation(15, 0), new BaseCellRotation(22, 5), new BaseCellRotation(33, 5)}, // j 0
                        {new BaseCellRotation(28, 0), new BaseCellRotation(31, 0), new BaseCellRotation(41, 0)}, // j 1
                        {new BaseCellRotation(42, 1), new BaseCellRotation(44, 0), new BaseCellRotation(53, 0)} // j 2
                    },
                    {
                        // i 2
                        {new BaseCellRotation(4, 4), new BaseCellRotation(8, 5), new BaseCellRotation(16, 5)}, // j 0
                        {new BaseCellRotation(12, 1), new BaseCellRotation(15, 0), new BaseCellRotation(22, 5)}, // j 1
                        {new BaseCellRotation(26, 1), new BaseCellRotation(28, 0), new BaseCellRotation(31, 0)} // j 2
                    }
                },
                {
                    // face 5
                    {
                        // i 0
                        {new BaseCellRotation(50, 0), new BaseCellRotation(48, 0), new BaseCellRotation(49, 3)}, // j 0
                        {new BaseCellRotation(32, 0), new BaseCellRotation(30, 3), new BaseCellRotation(33, 3)}, // j 1
                        {new BaseCellRotation(24, 3), new BaseCellRotation(18, 3), new BaseCellRotation(16, 3)} // j 2
                    },
                    {
                        // i 1
                        {new BaseCellRotation(70, 0), new BaseCellRotation(67, 0), new BaseCellRotation(66, 3)}, // j 0
                        {new BaseCellRotation(52, 3), new BaseCellRotation(50, 0), new BaseCellRotation(48, 0)}, // j 1
                        {new BaseCellRotation(37, 3), new BaseCellRotation(32, 0), new BaseCellRotation(30, 3)} // j 2
                    },
                    {
                        // i 2
                        {new BaseCellRotation(83, 0), new BaseCellRotation(87, 3), new BaseCellRotation(85, 3)}, // j 0
                        {new BaseCellRotation(74, 3), new BaseCellRotation(70, 0), new BaseCellRotation(67, 0)}, // j 1
                        {new BaseCellRotation(57, 1), new BaseCellRotation(52, 3), new BaseCellRotation(50, 0)} // j 2
                    }
                },
                {
                    // face 6
                    {
                        // i 0
                        {new BaseCellRotation(25, 0), new BaseCellRotation(23, 0), new BaseCellRotation(24, 3)}, // j 0
                        {new BaseCellRotation(17, 0), new BaseCellRotation(11, 3), new BaseCellRotation(10, 3)}, // j 1
                        {new BaseCellRotation(14, 3), new BaseCellRotation(6, 3), new BaseCellRotation(2, 3)} // j 2
                    },
                    {
                        // i 1
                        {new BaseCellRotation(45, 0), new BaseCellRotation(39, 0), new BaseCellRotation(37, 3)}, // j 0
                        {new BaseCellRotation(35, 3), new BaseCellRotation(25, 0), new BaseCellRotation(23, 0)}, // j 1
                        {new BaseCellRotation(27, 3), new BaseCellRotation(17, 0), new BaseCellRotation(11, 3)} // j 2
                    },
                    {
                        // i 2
                        {new BaseCellRotation(63, 0), new BaseCellRotation(59, 3), new BaseCellRotation(57, 3)}, // j 0
                        {new BaseCellRotation(56, 3), new BaseCellRotation(45, 0), new BaseCellRotation(39, 0)}, // j 1
                        {new BaseCellRotation(46, 3), new BaseCellRotation(35, 3), new BaseCellRotation(25, 0)} // j 2
                    }
                },
                {
                    // face 7
                    {
                        // i 0
                        {new BaseCellRotation(36, 0), new BaseCellRotation(20, 0), new BaseCellRotation(14, 3)}, // j 0
                        {new BaseCellRotation(34, 0), new BaseCellRotation(19, 3), new BaseCellRotation(9, 3)}, // j 1
                        {new BaseCellRotation(38, 3), new BaseCellRotation(21, 3), new BaseCellRotation(7, 3)} // j 2
                    },
                    {
                        // i 1
                        {new BaseCellRotation(55, 0), new BaseCellRotation(40, 0), new BaseCellRotation(27, 3)}, // j 0
                        {new BaseCellRotation(54, 3), new BaseCellRotation(36, 0), new BaseCellRotation(20, 0)}, // j 1
                        {new BaseCellRotation(51, 3), new BaseCellRotation(34, 0), new BaseCellRotation(19, 3)} // j 2
                    },
                    {
                        // i 2
                        {new BaseCellRotation(72, 0), new BaseCellRotation(60, 3), new BaseCellRotation(46, 3)}, // j 0
                        {new BaseCellRotation(73, 3), new BaseCellRotation(55, 0), new BaseCellRotation(40, 0)}, // j 1
                        {new BaseCellRotation(71, 3), new BaseCellRotation(54, 3), new BaseCellRotation(36, 0)} // j 2
                    }
                },
                {
                    // face 8
                    {
                        // i 0
                        {new BaseCellRotation(64, 0), new BaseCellRotation(47, 0), new BaseCellRotation(38, 3)}, // j 0
                        {new BaseCellRotation(62, 0), new BaseCellRotation(43, 3), new BaseCellRotation(29, 3)}, // j 1
                        {new BaseCellRotation(58, 3), new BaseCellRotation(42, 3), new BaseCellRotation(26, 3)} // j 2
                    },
                    {
                        // i 1
                        {new BaseCellRotation(84, 0), new BaseCellRotation(69, 0), new BaseCellRotation(51, 3)}, // j 0
                        {new BaseCellRotation(82, 3), new BaseCellRotation(64, 0), new BaseCellRotation(47, 0)}, // j 1
                        {new BaseCellRotation(76, 3), new BaseCellRotation(62, 0), new BaseCellRotation(43, 3)} // j 2
                    },
                    {
                        // i 2
                        {new BaseCellRotation(97, 0), new BaseCellRotation(89, 3), new BaseCellRotation(71, 3)}, // j 0
                        {new BaseCellRotation(98, 3), new BaseCellRotation(84, 0), new BaseCellRotation(69, 0)}, // j 1
                        {new BaseCellRotation(96, 3), new BaseCellRotation(82, 3), new BaseCellRotation(64, 0)} // j 2
                    }
                },
                {
                    // face 9
                    {
                        // i 0
                        {new BaseCellRotation(75, 0), new BaseCellRotation(65, 0), new BaseCellRotation(58, 3)}, // j 0
                        {new BaseCellRotation(61, 0), new BaseCellRotation(53, 3), new BaseCellRotation(44, 3)}, // j 1
                        {new BaseCellRotation(49, 3), new BaseCellRotation(41, 3), new BaseCellRotation(31, 3)} // j 2
                    },
                    {
                        // i 1
                        {new BaseCellRotation(94, 0), new BaseCellRotation(86, 0), new BaseCellRotation(76, 3)}, // j 0
                        {new BaseCellRotation(81, 3), new BaseCellRotation(75, 0), new BaseCellRotation(65, 0)}, // j 1
                        {new BaseCellRotation(66, 3), new BaseCellRotation(61, 0), new BaseCellRotation(53, 3)} // j 2
                    },
                    {
                        // i 2
                        {
                            new BaseCellRotation(107, 0), new BaseCellRotation(104, 3), new BaseCellRotation(96, 3)
                        }, // j 0
                        {new BaseCellRotation(101, 3), new BaseCellRotation(94, 0), new BaseCellRotation(86, 0)}, // j 1
                        {new BaseCellRotation(85, 3), new BaseCellRotation(81, 3), new BaseCellRotation(75, 0)} // j 2
                    }
                },
                {
                    // face 10
                    {
                        // i 0
                        {new BaseCellRotation(57, 0), new BaseCellRotation(59, 0), new BaseCellRotation(63, 3)}, // j 0
                        {new BaseCellRotation(74, 0), new BaseCellRotation(78, 3), new BaseCellRotation(79, 3)}, // j 1
                        {new BaseCellRotation(83, 3), new BaseCellRotation(92, 3), new BaseCellRotation(95, 3)} // j 2
                    },
                    {
                        // i 1
                        {new BaseCellRotation(37, 0), new BaseCellRotation(39, 3), new BaseCellRotation(45, 3)}, // j 0
                        {new BaseCellRotation(52, 0), new BaseCellRotation(57, 0), new BaseCellRotation(59, 0)}, // j 1
                        {new BaseCellRotation(70, 3), new BaseCellRotation(74, 0), new BaseCellRotation(78, 3)} // j 2
                    },
                    {
                        // i 2
                        {new BaseCellRotation(24, 0), new BaseCellRotation(23, 3), new BaseCellRotation(25, 3)}, // j 0
                        {new BaseCellRotation(32, 3), new BaseCellRotation(37, 0), new BaseCellRotation(39, 3)}, // j 1
                        {new BaseCellRotation(50, 3), new BaseCellRotation(52, 0), new BaseCellRotation(57, 0)} // j 2
                    }
                },
                {
                    // face 11
                    {
                        // i 0
                        {new BaseCellRotation(46, 0), new BaseCellRotation(60, 0), new BaseCellRotation(72, 3)}, // j 0
                        {new BaseCellRotation(56, 0), new BaseCellRotation(68, 3), new BaseCellRotation(80, 3)}, // j 1
                        {new BaseCellRotation(63, 3), new BaseCellRotation(77, 3), new BaseCellRotation(90, 3)} // j 2
                    },
                    {
                        // i 1
                        {new BaseCellRotation(27, 0), new BaseCellRotation(40, 3), new BaseCellRotation(55, 3)}, // j 0
                        {new BaseCellRotation(35, 0), new BaseCellRotation(46, 0), new BaseCellRotation(60, 0)}, // j 1
                        {new BaseCellRotation(45, 3), new BaseCellRotation(56, 0), new BaseCellRotation(68, 3)} // j 2
                    },
                    {
                        // i 2
                        {new BaseCellRotation(14, 0), new BaseCellRotation(20, 3), new BaseCellRotation(36, 3)}, // j 0
                        {new BaseCellRotation(17, 3), new BaseCellRotation(27, 0), new BaseCellRotation(40, 3)}, // j 1
                        {new BaseCellRotation(25, 3), new BaseCellRotation(35, 0), new BaseCellRotation(46, 0)} // j 2
                    }
                },
                {
                    // face 12
                    {
                        // i 0
                        {new BaseCellRotation(71, 0), new BaseCellRotation(89, 0), new BaseCellRotation(97, 3)}, // j 0
                        {new BaseCellRotation(73, 0), new BaseCellRotation(91, 3), new BaseCellRotation(103, 3)}, // j 1
                        {new BaseCellRotation(72, 3), new BaseCellRotation(88, 3), new BaseCellRotation(105, 3)} // j 2
                    },
                    {
                        // i 1
                        {new BaseCellRotation(51, 0), new BaseCellRotation(69, 3), new BaseCellRotation(84, 3)}, // j 0
                        {new BaseCellRotation(54, 0), new BaseCellRotation(71, 0), new BaseCellRotation(89, 0)}, // j 1
                        {new BaseCellRotation(55, 3), new BaseCellRotation(73, 0), new BaseCellRotation(91, 3)} // j 2
                    },
                    {
                        // i 2
                        {new BaseCellRotation(38, 0), new BaseCellRotation(47, 3), new BaseCellRotation(64, 3)}, // j 0
                        {new BaseCellRotation(34, 3), new BaseCellRotation(51, 0), new BaseCellRotation(69, 3)}, // j 1
                        {new BaseCellRotation(36, 3), new BaseCellRotation(54, 0), new BaseCellRotation(71, 0)} // j 2
                    }
                },
                {
                    // face 13
                    {
                        // i 0
                        {
                            new BaseCellRotation(96, 0), new BaseCellRotation(104, 0), new BaseCellRotation(107, 3)
                        }, // j 0
                        {
                            new BaseCellRotation(98, 0), new BaseCellRotation(110, 3), new BaseCellRotation(115, 3)
                        }, // j 1
                        {new BaseCellRotation(97, 3), new BaseCellRotation(111, 3), new BaseCellRotation(119, 3)} // j 2
                    },
                    {
                        // i 1
                        {new BaseCellRotation(76, 0), new BaseCellRotation(86, 3), new BaseCellRotation(94, 3)}, // j 0
                        {new BaseCellRotation(82, 0), new BaseCellRotation(96, 0), new BaseCellRotation(104, 0)}, // j 1
                        {new BaseCellRotation(84, 3), new BaseCellRotation(98, 0), new BaseCellRotation(110, 3)} // j 2
                    },
                    {
                        // i 2
                        {new BaseCellRotation(58, 0), new BaseCellRotation(65, 3), new BaseCellRotation(75, 3)}, // j 0
                        {new BaseCellRotation(62, 3), new BaseCellRotation(76, 0), new BaseCellRotation(86, 3)}, // j 1
                        {new BaseCellRotation(64, 3), new BaseCellRotation(82, 0), new BaseCellRotation(96, 0)} // j 2
                    }
                },
                {
                    // face 14
                    {
                        // i 0
                        {new BaseCellRotation(85, 0), new BaseCellRotation(87, 0), new BaseCellRotation(83, 3)}, // j 0
                        {
                            new BaseCellRotation(101, 0), new BaseCellRotation(102, 3), new BaseCellRotation(100, 3)
                        }, // j 1
                        {
                            new BaseCellRotation(107, 3), new BaseCellRotation(112, 3), new BaseCellRotation(114, 3)
                        } // j 2
                    },
                    {
                        // i 1
                        {new BaseCellRotation(66, 0), new BaseCellRotation(67, 3), new BaseCellRotation(70, 3)}, // j 0
                        {new BaseCellRotation(81, 0), new BaseCellRotation(85, 0), new BaseCellRotation(87, 0)}, // j 1
                        {new BaseCellRotation(94, 3), new BaseCellRotation(101, 0), new BaseCellRotation(102, 3)} // j 2
                    },
                    {
                        // i 2
                        {new BaseCellRotation(49, 0), new BaseCellRotation(48, 3), new BaseCellRotation(50, 3)}, // j 0
                        {new BaseCellRotation(61, 3), new BaseCellRotation(66, 0), new BaseCellRotation(67, 3)}, // j 1
                        {new BaseCellRotation(75, 3), new BaseCellRotation(81, 0), new BaseCellRotation(85, 0)} // j 2
                    }
                },
                {
                    // face 15
                    {
                        // i 0
                        {new BaseCellRotation(95, 0), new BaseCellRotation(92, 0), new BaseCellRotation(83, 0)}, // j 0
                        {new BaseCellRotation(79, 0), new BaseCellRotation(78, 0), new BaseCellRotation(74, 3)}, // j 1
                        {new BaseCellRotation(63, 1), new BaseCellRotation(59, 3), new BaseCellRotation(57, 3)} // j 2
                    },
                    {
                        // i 1
                        {
                            new BaseCellRotation(109, 0), new BaseCellRotation(108, 0), new BaseCellRotation(100, 5)
                        }, // j 0
                        {new BaseCellRotation(93, 1), new BaseCellRotation(95, 0), new BaseCellRotation(92, 0)}, // j 1
                        {new BaseCellRotation(77, 1), new BaseCellRotation(79, 0), new BaseCellRotation(78, 0)} // j 2
                    },
                    {
                        // i 2
                        {
                            new BaseCellRotation(117, 4), new BaseCellRotation(118, 5), new BaseCellRotation(114, 5)
                        }, // j 0
                        {
                            new BaseCellRotation(106, 1), new BaseCellRotation(109, 0), new BaseCellRotation(108, 0)
                        }, // j 1
                        {new BaseCellRotation(90, 1), new BaseCellRotation(93, 1), new BaseCellRotation(95, 0)} // j 2
                    }
                },
                {
                    // face 16
                    {
                        // i 0
                        {new BaseCellRotation(90, 0), new BaseCellRotation(77, 0), new BaseCellRotation(63, 0)}, // j 0
                        {new BaseCellRotation(80, 0), new BaseCellRotation(68, 0), new BaseCellRotation(56, 3)}, // j 1
                        {new BaseCellRotation(72, 1), new BaseCellRotation(60, 3), new BaseCellRotation(46, 3)} // j 2
                    },
                    {
                        // i 1
                        {new BaseCellRotation(106, 0), new BaseCellRotation(93, 0), new BaseCellRotation(79, 5)}, // j 0
                        {new BaseCellRotation(99, 1), new BaseCellRotation(90, 0), new BaseCellRotation(77, 0)}, // j 1
                        {new BaseCellRotation(88, 1), new BaseCellRotation(80, 0), new BaseCellRotation(68, 0)} // j 2
                    },
                    {
                        // i 2
                        {
                            new BaseCellRotation(117, 3), new BaseCellRotation(109, 5), new BaseCellRotation(95, 5)
                        }, // j 0
                        {
                            new BaseCellRotation(113, 1), new BaseCellRotation(106, 0), new BaseCellRotation(93, 0)
                        }, // j 1
                        {new BaseCellRotation(105, 1), new BaseCellRotation(99, 1), new BaseCellRotation(90, 0)} // j 2
                    }
                },
                {
                    // face 17
                    {
                        // i 0
                        {new BaseCellRotation(105, 0), new BaseCellRotation(88, 0), new BaseCellRotation(72, 0)}, // j 0
                        {new BaseCellRotation(103, 0), new BaseCellRotation(91, 0), new BaseCellRotation(73, 3)}, // j 1
                        {new BaseCellRotation(97, 1), new BaseCellRotation(89, 3), new BaseCellRotation(71, 3)} // j 2
                    },
                    {
                        // i 1
                        {new BaseCellRotation(113, 0), new BaseCellRotation(99, 0), new BaseCellRotation(80, 5)}, // j 0
                        {
                            new BaseCellRotation(116, 1), new BaseCellRotation(105, 0), new BaseCellRotation(88, 0)
                        }, // j 1
                        {new BaseCellRotation(111, 1), new BaseCellRotation(103, 0), new BaseCellRotation(91, 0)} // j 2
                    },
                    {
                        // i 2
                        {
                            new BaseCellRotation(117, 2), new BaseCellRotation(106, 5), new BaseCellRotation(90, 5)
                        }, // j 0
                        {
                            new BaseCellRotation(121, 1), new BaseCellRotation(113, 0), new BaseCellRotation(99, 0)
                        }, // j 1
                        {
                            new BaseCellRotation(119, 1), new BaseCellRotation(116, 1), new BaseCellRotation(105, 0)
                        } // j 2
                    }
                },
                {
                    // face 18
                    {
                        // i 0
                        {
                            new BaseCellRotation(119, 0), new BaseCellRotation(111, 0), new BaseCellRotation(97, 0)
                        }, // j 0
                        {
                            new BaseCellRotation(115, 0), new BaseCellRotation(110, 0), new BaseCellRotation(98, 3)
                        }, // j 1
                        {new BaseCellRotation(107, 1), new BaseCellRotation(104, 3), new BaseCellRotation(96, 3)} // j 2
                    },
                    {
                        // i 1
                        {
                            new BaseCellRotation(121, 0), new BaseCellRotation(116, 0), new BaseCellRotation(103, 5)
                        }, // j 0
                        {
                            new BaseCellRotation(120, 1), new BaseCellRotation(119, 0), new BaseCellRotation(111, 0)
                        }, // j 1
                        {
                            new BaseCellRotation(112, 1), new BaseCellRotation(115, 0), new BaseCellRotation(110, 0)
                        } // j 2
                    },
                    {
                        // i 2
                        {
                            new BaseCellRotation(117, 1), new BaseCellRotation(113, 5), new BaseCellRotation(105, 5)
                        }, // j 0
                        {
                            new BaseCellRotation(118, 1), new BaseCellRotation(121, 0), new BaseCellRotation(116, 0)
                        }, // j 1
                        {
                            new BaseCellRotation(114, 1), new BaseCellRotation(120, 1), new BaseCellRotation(119, 0)
                        } // j 2
                    }
                },
                {
                    // face 19
                    {
                        // i 0
                        {
                            new BaseCellRotation(114, 0), new BaseCellRotation(112, 0), new BaseCellRotation(107, 0)
                        }, // j 0
                        {
                            new BaseCellRotation(100, 0), new BaseCellRotation(102, 0), new BaseCellRotation(101, 3)
                        }, // j 1
                        {new BaseCellRotation(83, 1), new BaseCellRotation(87, 3), new BaseCellRotation(85, 3)} // j 2
                    },
                    {
                        // i 1
                        {
                            new BaseCellRotation(118, 0), new BaseCellRotation(120, 0), new BaseCellRotation(115, 5)
                        }, // j 0
                        {
                            new BaseCellRotation(108, 1), new BaseCellRotation(114, 0), new BaseCellRotation(112, 0)
                        }, // j 1
                        {new BaseCellRotation(92, 1), new BaseCellRotation(100, 0), new BaseCellRotation(102, 0)} // j 2
                    },
                    {
                        // i 2
                        {
                            new BaseCellRotation(117, 0), new BaseCellRotation(121, 5), new BaseCellRotation(119, 5)
                        }, // j 0
                        {
                            new BaseCellRotation(109, 1), new BaseCellRotation(118, 0), new BaseCellRotation(120, 0)
                        }, // j 1
                        {new BaseCellRotation(95, 1), new BaseCellRotation(108, 1), new BaseCellRotation(114, 0)} // j 2
                    }
                }
            };

            /// <summary>
            /// Resolution 0 base cell data table.
            ///
            /// For each base cell, gives the "home" face and ijk+ coordinates on that face,
            /// whether or not the base cell is a pentagon. Additionally, if the base cell
            /// is a pentagon, the two cw offset rotation adjacent faces are given (-1
            /// indicates that no cw offset rotation faces exist for this base cell).
            /// </summary>
            internal static readonly BaseCellData[] BaseCellData =
            {
                new BaseCellData(1, 1, 0, 0, 0, 0, 0), // base cell 0
                new BaseCellData(2, 1, 1, 0, 0, 0, 0), // base cell 1
                new BaseCellData(1, 0, 0, 0, 0, 0, 0), // base cell 2
                new BaseCellData(2, 1, 0, 0, 0, 0, 0), // base cell 3
                new BaseCellData(0, 2, 0, 0, 1, -1, -1), // base cell 4
                new BaseCellData(1, 1, 1, 0, 0, 0, 0), // base cell 5
                new BaseCellData(1, 0, 0, 1, 0, 0, 0), // base cell 6
                new BaseCellData(2, 0, 0, 0, 0, 0, 0), // base cell 7
                new BaseCellData(0, 1, 0, 0, 0, 0, 0), // base cell 8
                new BaseCellData(2, 0, 1, 0, 0, 0, 0), // base cell 9
                new BaseCellData(1, 0, 1, 0, 0, 0, 0), // base cell 10
                new BaseCellData(1, 0, 1, 1, 0, 0, 0), // base cell 11
                new BaseCellData(3, 1, 0, 0, 0, 0, 0), // base cell 12
                new BaseCellData(3, 1, 1, 0, 0, 0, 0), // base cell 13
                new BaseCellData(11, 2, 0, 0, 1, 2, 6), // base cell 14
                new BaseCellData(4, 1, 0, 0, 0, 0, 0), // base cell 15
                new BaseCellData(0, 0, 0, 0, 0, 0, 0), // base cell 16
                new BaseCellData(6, 0, 1, 0, 0, 0, 0), // base cell 17
                new BaseCellData(0, 0, 0, 1, 0, 0, 0), // base cell 18
                new BaseCellData(2, 0, 1, 1, 0, 0, 0), // base cell 19
                new BaseCellData(7, 0, 0, 1, 0, 0, 0), // base cell 20
                new BaseCellData(2, 0, 0, 1, 0, 0, 0), // base cell 21
                new BaseCellData(0, 1, 1, 0, 0, 0, 0), // base cell 22
                new BaseCellData(6, 0, 0, 1, 0, 0, 0), // base cell 23
                new BaseCellData(10, 2, 0, 0, 1, 1, 5), // base cell 24
                new BaseCellData(6, 0, 0, 0, 0, 0, 0), // base cell 25
                new BaseCellData(3, 0, 0, 0, 0, 0, 0), // base cell 26
                new BaseCellData(11, 1, 0, 0, 0, 0, 0), // base cell 27
                new BaseCellData(4, 1, 1, 0, 0, 0, 0), // base cell 28
                new BaseCellData(3, 0, 1, 0, 0, 0, 0), // base cell 29
                new BaseCellData(0, 0, 1, 1, 0, 0, 0), // base cell 30
                new BaseCellData(4, 0, 0, 0, 0, 0, 0), // base cell 31
                new BaseCellData(5, 0, 1, 0, 0, 0, 0), // base cell 32
                new BaseCellData(0, 0, 1, 0, 0, 0, 0), // base cell 33
                new BaseCellData(7, 0, 1, 0, 0, 0, 0), // base cell 34
                new BaseCellData(11, 1, 1, 0, 0, 0, 0), // base cell 35
                new BaseCellData(7, 0, 0, 0, 0, 0, 0), // base cell 36
                new BaseCellData(10, 1, 0, 0, 0, 0, 0), // base cell 37
                new BaseCellData(12, 2, 0, 0, 1, 3, 7), // base cell 38
                new BaseCellData(6, 1, 0, 1, 0, 0, 0), // base cell 39
                new BaseCellData(7, 1, 0, 1, 0, 0, 0), // base cell 40
                new BaseCellData(4, 0, 0, 1, 0, 0, 0), // base cell 41
                new BaseCellData(3, 0, 0, 1, 0, 0, 0), // base cell 42
                new BaseCellData(3, 0, 1, 1, 0, 0, 0), // base cell 43
                new BaseCellData(4, 0, 1, 0, 0, 0, 0), // base cell 44
                new BaseCellData(6, 1, 0, 0, 0, 0, 0), // base cell 45
                new BaseCellData(11, 0, 0, 0, 0, 0, 0), // base cell 46
                new BaseCellData(8, 0, 0, 1, 0, 0, 0), // base cell 47
                new BaseCellData(5, 0, 0, 1, 0, 0, 0), // base cell 48
                new BaseCellData(14, 2, 0, 0, 1, 0, 9), // base cell 49
                new BaseCellData(5, 0, 0, 0, 0, 0, 0), // base cell 50
                new BaseCellData(12, 1, 0, 0, 0, 0, 0), // base cell 51
                new BaseCellData(10, 1, 1, 0, 0, 0, 0), // base cell 52
                new BaseCellData(4, 0, 1, 1, 0, 0, 0), // base cell 53
                new BaseCellData(12, 1, 1, 0, 0, 0, 0), // base cell 54
                new BaseCellData(7, 1, 0, 0, 0, 0, 0), // base cell 55
                new BaseCellData(11, 0, 1, 0, 0, 0, 0), // base cell 56
                new BaseCellData(10, 0, 0, 0, 0, 0, 0), // base cell 57
                new BaseCellData(13, 2, 0, 0, 1, 4, 8), // base cell 58
                new BaseCellData(10, 0, 0, 1, 0, 0, 0), // base cell 59
                new BaseCellData(11, 0, 0, 1, 0, 0, 0), // base cell 60
                new BaseCellData(9, 0, 1, 0, 0, 0, 0), // base cell 61
                new BaseCellData(8, 0, 1, 0, 0, 0, 0), // base cell 62
                new BaseCellData(6, 2, 0, 0, 1, 11, 15), // base cell 63
                new BaseCellData(8, 0, 0, 0, 0, 0, 0), // base cell 64
                new BaseCellData(9, 0, 0, 1, 0, 0, 0), // base cell 65
                new BaseCellData(14, 1, 0, 0, 0, 0, 0), // base cell 66
                new BaseCellData(5, 1, 0, 1, 0, 0, 0), // base cell 67
                new BaseCellData(16, 0, 1, 1, 0, 0, 0), // base cell 68
                new BaseCellData(8, 1, 0, 1, 0, 0, 0), // base cell 69
                new BaseCellData(5, 1, 0, 0, 0, 0, 0), // base cell 70
                new BaseCellData(12, 0, 0, 0, 0, 0, 0), // base cell 71
                new BaseCellData(7, 2, 0, 0, 1, 12, 16), // base cell 72
                new BaseCellData(12, 0, 1, 0, 0, 0, 0), // base cell 73
                new BaseCellData(10, 0, 1, 0, 0, 0, 0), // base cell 74
                new BaseCellData(9, 0, 0, 0, 0, 0, 0), // base cell 75
                new BaseCellData(13, 1, 0, 0, 0, 0, 0), // base cell 76
                new BaseCellData(16, 0, 0, 1, 0, 0, 0), // base cell 77
                new BaseCellData(15, 0, 1, 1, 0, 0, 0), // base cell 78
                new BaseCellData(15, 0, 1, 0, 0, 0, 0), // base cell 79
                new BaseCellData(16, 0, 1, 0, 0, 0, 0), // base cell 80
                new BaseCellData(14, 1, 1, 0, 0, 0, 0), // base cell 81
                new BaseCellData(13, 1, 1, 0, 0, 0, 0), // base cell 82
                new BaseCellData(5, 2, 0, 0, 1, 10, 19), // base cell 83
                new BaseCellData(8, 1, 0, 0, 0, 0, 0), // base cell 84
                new BaseCellData(14, 0, 0, 0, 0, 0, 0), // base cell 85
                new BaseCellData(9, 1, 0, 1, 0, 0, 0), // base cell 86
                new BaseCellData(14, 0, 0, 1, 0, 0, 0), // base cell 87
                new BaseCellData(17, 0, 0, 1, 0, 0, 0), // base cell 88
                new BaseCellData(12, 0, 0, 1, 0, 0, 0), // base cell 89
                new BaseCellData(16, 0, 0, 0, 0, 0, 0), // base cell 90
                new BaseCellData(17, 0, 1, 1, 0, 0, 0), // base cell 91
                new BaseCellData(15, 0, 0, 1, 0, 0, 0), // base cell 92
                new BaseCellData(16, 1, 0, 1, 0, 0, 0), // base cell 93
                new BaseCellData(9, 1, 0, 0, 0, 0, 0), // base cell 94
                new BaseCellData(15, 0, 0, 0, 0, 0, 0), // base cell 95
                new BaseCellData(13, 0, 0, 0, 0, 0, 0), // base cell 96
                new BaseCellData(8, 2, 0, 0, 1, 13, 17), // base cell 97
                new BaseCellData(13, 0, 1, 0, 0, 0, 0), // base cell 98
                new BaseCellData(17, 1, 0, 1, 0, 0, 0), // base cell 99
                new BaseCellData(19, 0, 1, 0, 0, 0, 0), // base cell 100
                new BaseCellData(14, 0, 1, 0, 0, 0, 0), // base cell 101
                new BaseCellData(19, 0, 1, 1, 0, 0, 0), // base cell 102
                new BaseCellData(17, 0, 1, 0, 0, 0, 0), // base cell 103
                new BaseCellData(13, 0, 0, 1, 0, 0, 0), // base cell 104
                new BaseCellData(17, 0, 0, 0, 0, 0, 0), // base cell 105
                new BaseCellData(16, 1, 0, 0, 0, 0, 0), // base cell 106
                new BaseCellData(9, 2, 0, 0, 1, 14, 18), // base cell 107
                new BaseCellData(15, 1, 0, 1, 0, 0, 0), // base cell 108
                new BaseCellData(15, 1, 0, 0, 0, 0, 0), // base cell 109
                new BaseCellData(18, 0, 1, 1, 0, 0, 0), // base cell 110
                new BaseCellData(18, 0, 0, 1, 0, 0, 0), // base cell 111
                new BaseCellData(19, 0, 0, 1, 0, 0, 0), // base cell 112
                new BaseCellData(17, 1, 0, 0, 0, 0, 0), // base cell 113
                new BaseCellData(19, 0, 0, 0, 0, 0, 0), // base cell 114
                new BaseCellData(18, 0, 1, 0, 0, 0, 0), // base cell 115
                new BaseCellData(18, 1, 0, 1, 0, 0, 0), // base cell 116
                new BaseCellData(19, 2, 0, 0, 1, -1, -1), // base cell 117
                new BaseCellData(19, 1, 0, 0, 0, 0, 0), // base cell 118
                new BaseCellData(18, 0, 0, 0, 0, 0, 0), // base cell 119
                new BaseCellData(19, 1, 0, 1, 0, 0, 0), // base cell 120
                new BaseCellData(18, 1, 0, 0, 0, 0, 0) // base cell 121
            };
        }

        internal static class CoordIjk
        {
            /// <summary>
            /// CoordIJK unit vectors corresponding to the 7 H3 digits.
            /// </summary>
            internal static readonly H3Lib.CoordIjk[] UnitVecs =
            {
                new H3Lib.CoordIjk(0, 0, 0), // direction 0
                new H3Lib.CoordIjk(0, 0, 1), // direction 1
                new H3Lib.CoordIjk(0, 1, 0), // direction 2
                new H3Lib.CoordIjk(0, 1, 1), // direction 3
                new H3Lib.CoordIjk(1, 0, 0), // direction 4
                new H3Lib.CoordIjk(1, 0, 1), // direction 5
                new H3Lib.CoordIjk(1, 1, 0) // direction 6
            };

            public static readonly Dictionary<Direction, H3Lib.CoordIjk> UnitVectors =
                new Dictionary<Direction, H3Lib.CoordIjk>
                {
                    {Direction.CENTER_DIGIT, new H3Lib.CoordIjk(0, 0, 0)},
                    {Direction.K_AXES_DIGIT, new H3Lib.CoordIjk(0, 0, 1)},
                    {Direction.J_AXES_DIGIT, new H3Lib.CoordIjk(0, 1, 0)},
                    {Direction.JK_AXES_DIGIT, new H3Lib.CoordIjk(0, 1, 1)},
                    {Direction.I_AXES_DIGIT, new H3Lib.CoordIjk(1, 0, 0)},
                    {Direction.IK_AXES_DIGIT, new H3Lib.CoordIjk(1, 0, 1)},
                    {Direction.IJ_AXES_DIGIT, new H3Lib.CoordIjk(1, 1, 0)},
                };

        }

        internal static class FaceIjk
        {
            /// <summary>
            /// Invalid face index
            /// </summary>
            internal static readonly int InvalidFace = -1;

            /// <summary>
            /// IJ quadrant faceNeighbors table direction
            /// </summary>
            internal const int IJ = 1;

            /// <summary>
            /// KI quadrant faceNeighbors table direction
            /// </summary>
            internal const int KI = 2;

            /// <summary>
            /// JK quadrant faceNeighbors table direction
            /// </summary>
            internal const int JK = 3;

            /// <summary>
            /// Square root of 7
            /// </summary>
            internal static readonly decimal MSqrt7 = 2.6457513110645905905016157536392604257102m;

            /// <summary>
            /// icosahedron face centers in lat/lon radians
            /// </summary>
            internal static readonly H3Lib.GeoCoord[] FaceCenterGeo =
            {
                new H3Lib.GeoCoord(0.803582649718989942m, 1.248397419617396099m), // face  0
                new H3Lib.GeoCoord(1.307747883455638156m, 2.536945009877921159m), // face  1
                new H3Lib.GeoCoord(1.054751253523952054m, -1.347517358900396623m), // face  2
                new H3Lib.GeoCoord(0.600191595538186799m, -0.450603909469755746m), // face  3
                new H3Lib.GeoCoord(0.491715428198773866m, 0.401988202911306943m), // face  4
                new H3Lib.GeoCoord(0.172745327415618701m, 1.678146885280433686m), // face  5
                new H3Lib.GeoCoord(0.605929321571350690m, 2.953923329812411617m), // face  6
                new H3Lib.GeoCoord(0.427370518328979641m, -1.888876200336285401m), // face  7
                new H3Lib.GeoCoord(-0.079066118549212831m, -0.733429513380867741m), // face  8
                new H3Lib.GeoCoord(-0.230961644455383637m, 0.506495587332349035m), // face  9
                new H3Lib.GeoCoord(0.079066118549212831m, 2.408163140208925497m), // face 10
                new H3Lib.GeoCoord(0.230961644455383637m, -2.635097066257444203m), // face 11
                new H3Lib.GeoCoord(-0.172745327415618701m, -1.463445768309359553m), // face 12
                new H3Lib.GeoCoord(-0.605929321571350690m, -0.187669323777381622m), // face 13
                new H3Lib.GeoCoord(-0.427370518328979641m, 1.252716453253507838m), // face 14
                new H3Lib.GeoCoord(-0.600191595538186799m, 2.690988744120037492m), // face 15
                new H3Lib.GeoCoord(-0.491715428198773866m, -2.739604450678486295m), // face 16
                new H3Lib.GeoCoord(-0.803582649718989942m, -1.893195233972397139m), // face 17
                new H3Lib.GeoCoord(-1.307747883455638156m, -0.604647643711872080m), // face 18
                new H3Lib.GeoCoord(-1.054751253523952054m, 1.794075294689396615m), // face 19        };
            };

            /// <summary>
            /// icosahedron face centers in x/y/z on the unit sphere
            /// </summary>
            internal static readonly Vec3d[] FaceCenterPoint =
            {
                new Vec3d(0.2199307791404606m, 0.6583691780274996m, 0.7198475378926182m), // face  0
                new Vec3d(-0.2139234834501421m, 0.1478171829550703m, 0.9656017935214205m), // face  1
                new Vec3d(0.1092625278784797m, -0.4811951572873210m, 0.8697775121287253m), // face  2
                new Vec3d(0.7428567301586791m, -0.3593941678278028m, 0.5648005936517033m), // face  3
                new Vec3d(0.8112534709140969m, 0.3448953237639384m, 0.4721387736413930m), // face  4
                new Vec3d(-0.1055498149613921m, 0.9794457296411413m, 0.1718874610009365m), // face  5
                new Vec3d(-0.8075407579970092m, 0.1533552485898818m, 0.5695261994882688m), // face  6
                new Vec3d(-0.2846148069787907m, -0.8644080972654206m, 0.4144792552473539m), // face  7
                new Vec3d(0.7405621473854482m, -0.6673299564565524m, -0.0789837646326737m), // face  8
                new Vec3d(0.8512303986474293m, 0.4722343788582681m, -0.2289137388687808m), // face  9
                new Vec3d(-0.7405621473854481m, 0.6673299564565524m, 0.0789837646326737m), // face 10
                new Vec3d(-0.8512303986474292m, -0.4722343788582682m, 0.2289137388687808m), // face 11
                new Vec3d(0.1055498149613919m, -0.9794457296411413m, -0.1718874610009365m), // face 12
                new Vec3d(0.8075407579970092m, -0.1533552485898819m, -0.5695261994882688m), // face 13
                new Vec3d(0.2846148069787908m, 0.8644080972654204m, -0.4144792552473539m), // face 14
                new Vec3d(-0.7428567301586791m, 0.3593941678278027m, -0.5648005936517033m), // face 15
                new Vec3d(-0.8112534709140971m, -0.3448953237639382m, -0.4721387736413930m), // face 16
                new Vec3d(-0.2199307791404607m, -0.6583691780274996m, -0.7198475378926182m), // face 17
                new Vec3d(0.2139234834501420m, -0.1478171829550704m, -0.9656017935214205m), // face 18
                new Vec3d(-0.1092625278784796m, 0.4811951572873210m, -0.8697775121287253m) // face 19
            };

            /// <summary>
            /// icosahedron face ijk axes as azimuth in radians from face center to
            /// vertex 0/1/2 respectively
            /// </summary>
            internal static readonly decimal[,] FaceAxesAzRadsCii =
            {
                {5.619958268523939882m, 3.525563166130744542m, 1.431168063737548730m}, // face  0
                {5.760339081714187279m, 3.665943979320991689m, 1.571548876927796127m}, // face  1
                {0.780213654393430055m, 4.969003859179821079m, 2.874608756786625655m}, // face  2
                {0.430469363979999913m, 4.619259568766391033m, 2.524864466373195467m}, // face  3
                {6.130269123335111400m, 4.035874020941915804m, 1.941478918548720291m}, // face  4
                {2.692877706530642877m, 0.598482604137447119m, 4.787272808923838195m}, // face  5
                {2.982963003477243874m, 0.888567901084048369m, 5.077358105870439581m}, // face  6
                {3.532912002790141181m, 1.438516900396945656m, 5.627307105183336758m}, // face  7
                {3.494305004259568154m, 1.399909901866372864m, 5.588700106652763840m}, // face  8
                {3.003214169499538391m, 0.908819067106342928m, 5.097609271892733906m}, // face  9
                {5.930472956509811562m, 3.836077854116615875m, 1.741682751723420374m}, // face 10
                {0.138378484090254847m, 4.327168688876645809m, 2.232773586483450311m}, // face 11
                {0.448714947059150361m, 4.637505151845541521m, 2.543110049452346120m}, // face 12
                {0.158629650112549365m, 4.347419854898940135m, 2.253024752505744869m}, // face 13
                {5.891865957979238535m, 3.797470855586042958m, 1.703075753192847583m}, // face 14
                {2.711123289609793325m, 0.616728187216597771m, 4.805518392002988683m}, // face 15
                {3.294508837434268316m, 1.200113735041072948m, 5.388903939827463911m}, // face 16
                {3.804819692245439833m, 1.710424589852244509m, 5.899214794638635174m}, // face 17
                {3.664438879055192436m, 1.570043776661997111m, 5.758833981448388027m}, // face 18
                {2.361378999196363184m, 0.266983896803167583m, 4.455774101589558636m}, // face 19
            };

            /// <summary>
            /// Definition of which faces neighbor each other.
            /// </summary>
            internal static readonly FaceOrientIjk[,] FaceNeighbors =
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
            internal static readonly int[,] AdjacentFaceDir =
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
            internal static readonly int[] MaxDimByCiiRes =
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
            internal static readonly int[] UnitScaleByCiiRes =
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

        internal static class GeoCoord
        {
            internal static readonly decimal[] AreasKm2 =
            {
                4250546.848m,     607220.9782m,    86745.85403m,   12392.26486m,
                   1770.323552m,     252.9033645m,    36.1290521m,     5.1612932m,
                      0.7373276m,      0.1053325m,     0.0150475m,     0.0021496m,
                      0.0003071m,      0.0000439m,     0.0000063m,     0.0000009m
            };

            internal static readonly decimal[] AreasM2 =
            {
                4.25055E+12m,  6.07221E+11m, 86745854035m,   12392264862m,
                1770323552m,   252903364.5m,    36129052.1m,     5161293.2m,
                    737327.6m,    105332.5m,       15047.5m,        2149.6m,
                       307.1m,        43.9m,           6.3m,          0.9m
            };

            internal static readonly decimal[] EdgeLengthKm =
            {
                1107.712591m,    418.6760055m,   158.2446558m,   59.81085794m,
                  22.6063794m,     8.544408276m,   3.229482772m,  1.220629759m,
                   0.461354684m,   0.174375668m,   0.065907807m,  0.024910561m,
                   0.009415526m,   0.003559893m,   0.001348575m,  0.000509713m
            };

            internal static readonly decimal[] EdgeLengthM =
            {
                1107712.591m,       418676.0055m,       158244.6558m,      59810.85794m,
                  22606.3794m,        8544.408276m,       3229.482772m,     1220.629759m,
                    461.3546837m,       174.3756681m,       65.90780749m,     24.9105614m,
                      9.415526211m,       3.559893033m,      1.348574562m,     0.509713273m
            };
        }

        internal static class H3Index
        {
            /// <summary>
            /// Invalid index used to indicate an error from geoToH3 and related functions.
            /// </summary>
            internal static readonly ulong H3_INVALID_INDEX = 0;

            /// <summary>
            /// Invalid index used to indicate an error from geoToH3 and related functions
            /// or missing data in arrays of h3 indices. Analogous to NaN in floating point.
            /// </summary>
            internal static readonly ulong H3_NULL = 0;

            /// <summary>
            /// The number of bits in an H3 index.
            /// </summary>
            internal static readonly int H3_NUM_BITS = 64;

            /// <summary>
            /// The bit offset of the max resolution digit in an H3 index.
            /// </summary>
            internal static readonly int H3_MAX_OFFSET = 63;

            /// <summary>
            /// The bit offset of the mode in an H3 index.
            /// </summary>
            internal static readonly int H3_MODE_OFFSET = 59;

            /// <summary>
            /// The bit offset of the base cell in an H3 index.
            /// </summary>
            internal static readonly int H3_BC_OFFSET = 45;

            /// <summary>
            /// The bit offset of the resolution in an H3 index.
            /// </summary>
            internal static readonly int H3_RES_OFFSET = 52;

            /// <summary>
            /// The bit offset of the reserved bits in an H3 index.
            /// </summary>
            internal static readonly int H3_RESERVED_OFFSET = 56;

            /// <summary>
            /// The number of bits in a single H3 resolution digit.
            /// </summary>
            internal static readonly int H3_PER_DIGIT_OFFSET = 3;

            /// <summary>
            /// 1 in the highest bit, 0's everywhere else.
            /// </summary>
            internal static readonly ulong H3_HIGH_BIT_MASK = (ulong) 1 << H3_MAX_OFFSET;

            /// <summary>
            /// 0 in the highest bit, 1's everywhere else.
            /// </summary>
            internal static readonly ulong H3_HIGH_BIT_MASK_NEGATIVE = ~H3_HIGH_BIT_MASK;

            /// <summary>
            /// 1's in the 4 mode bits, 0's everywhere else.
            /// </summary>
            internal static readonly ulong H3_MODE_MASK = (ulong) 15 << H3_MODE_OFFSET;

            /// <summary>
            /// 0's in the 4 mode bits, 1's everywhere else.
            /// </summary>
            internal static readonly ulong H3_MODE_MASK_NEGATIVE = ~H3_MODE_MASK;

            /// <summary>
            /// 1's in the 7 base cell bits, 0's everywhere else.
            /// </summary>
            internal static readonly ulong H3_BC_MASK = (ulong) 127 << H3_BC_OFFSET;

            /// <summary>
            /// 0's in the 7 base cell bits, 1's everywhere else.
            /// </summary>
            internal static readonly ulong H3_BC_MASK_NEGATIVE = ~H3_BC_MASK;

            ///H3 index with mode 0, res 0, base cell 0, and 7 for all index digits.
            ///Typically used to initialize the creation of an H3 cell index, which
            ///expects all direction digits to be 7 beyond the cell's resolution.
            internal static readonly ulong H3_INIT = 35184372088831;

            /// <summary>
            /// 1's in the 4 resolution bits, 0's everywhere else.
            /// </summary>
            internal static readonly ulong H3_RES_MASK = (ulong) 15 << H3_RES_OFFSET;

            /// <summary>
            /// 0's in the 4 resolution bits, 1's everywhere else.
            /// </summary>
            internal static readonly ulong H3_RES_MASK_NEGATIVE = ~H3_RES_MASK;

            /// <summary>
            /// 1's in the 3 reserved bits, 0's everywhere else.
            /// </summary>
            internal static readonly ulong H3_RESERVED_MASK = (ulong) 7 << H3_RESERVED_OFFSET;

            /// <summary>
            /// 0's in the 3 reserved bits, 1's everywhere else.
            /// </summary>
            internal static readonly ulong H3_RESERVED_MASK_NEGATIVE = ~H3_RESERVED_MASK;

            /// <summary>
            /// 1's in the 3 bits of res 15 digit bits, 0's everywhere else.
            /// </summary>
            internal static readonly ulong H3_DIGIT_MASK = 7;

            /// <summary>
            /// 0's in the 7 base cell bits, 1's everywhere else.
            /// </summary>
            internal static readonly ulong H3_DIGIT_MASK_NEGATIVE = ~H3_DIGIT_MASK;

            /*
             * Return codes for compact
             */
            internal const int COMPACT_SUCCESS = 0;
            internal const int COMPACT_LOOP_EXCEEDED = -1;
            internal const int COMPACT_DUPLICATE = -2;
            internal const int COMPACT_ALLOC_FAILED = -3;
            internal const int COMPACT_BAD_DATA = -10;
        }

        internal static class LinkedGeo
        {
            internal const int NormalizationSuccess = 0;
            internal const int NormalizationErrMultiplePolygons = 1;
            internal const int NormalizationErrUnassignedHoles = 2;
        }

        internal static class LocalIJ
        {
            /// <summary>
            /// Origin leading digit -&gt; index leading digit -&gt; rotations 60 cw
            /// Either being 1 (K axis) is invalid.
            /// No good default at 0.
            /// </summary>
            internal static readonly int[,] PENTAGON_ROTATIONS =
            {
                {0, -1, 0, 0, 0, 0, 0}, // 0
                {-1, -1, -1, -1, -1, -1, -1}, // 1
                {0, -1, 0, 0, 0, 1, 0}, // 2
                {0, -1, 0, 0, 1, 1, 0}, // 3
                {0, -1, 0, 5, 0, 0, 0}, // 4
                {0, -1, 5, 5, 0, 0, 0}, // 5
                {0, -1, 0, 0, 0, 0, 0} // 6
            };

            /// <summary>
            /// Reverse base cell direction -&gt; leading index digit -&gt; rotations 60 ccw.
            /// For reversing the rotation introduced in PENTAGON_ROTATIONS when the index is
            /// on a pentagon and the origin is not.
            /// </summary>
            internal static readonly int[,] PENTAGON_ROTATIONS_REVERSE =
            {
                {0, 0, 0, 0, 0, 0, 0}, // 0
                {-1, -1, -1, -1, -1, -1, -1}, // 1
                {0, 1, 0, 0, 0, 0, 0}, // 2
                {0, 1, 0, 0, 0, 1, 0}, // 3
                {0, 5, 0, 0, 0, 0, 0}, // 4
                {0, 5, 0, 5, 0, 0, 0}, // 5
                {0, 0, 0, 0, 0, 0, 0} // 6
            };

            /// <summary>
            /// Reverse base cell direction -&gt; leading index digit -&gt; rotations 60 ccw.
            /// For reversing the rotation introduced in PENTAGON_ROTATIONS when the index is
            /// on a pentagon and the origin is not.
            /// </summary>
            internal static readonly int[,] PENTAGON_ROTATIONS_REVERSE_NONPOLAR =
            {
                {0, 0, 0, 0, 0, 0, 0}, // 0
                {-1, -1, -1, -1, -1, -1, -1}, // 1
                {0, 1, 0, 0, 0, 0, 0}, // 2
                {0, 1, 0, 0, 0, 1, 0}, // 3
                {0, 5, 0, 0, 0, 0, 0}, // 4
                {0, 1, 0, 5, 1, 1, 0}, // 5
                {0, 0, 0, 0, 0, 0, 0}, // 6
            };

            /// <summary>
            /// Reverse base cell direction -&gt; leading index digit -&gt; rotations 60 ccw.
            /// For reversing the rotation introduced in PENTAGON_ROTATIONS when the index is
            /// on a polar pentagon and the origin is not.
            /// </summary>
            internal static readonly int[,] PENTAGON_ROTATIONS_REVERSE_POLAR =
            {
                {0, 0, 0, 0, 0, 0, 0}, // 0
                {-1, -1, -1, -1, -1, -1, -1}, // 1
                {0, 1, 1, 1, 1, 1, 1}, // 2
                {0, 1, 0, 0, 0, 1, 0}, // 3
                {0, 1, 0, 0, 1, 1, 1}, // 4
                {0, 1, 0, 5, 1, 1, 0}, // 5
                {0, 1, 1, 0, 1, 1, 1} // 6
            };

            /**
         * Prohibited directions when unfolding a pentagon.
         *
         * Indexes by two directions, both relative to the pentagon base cell. The first
         * is the direction of the origin index and the second is the direction of the
         * index to unfold. Direction refers to the direction from base cell to base
         * cell if the indexes are on different base cells, or the leading digit if
         * within the pentagon base cell.
         *
         * This previously included a Class II/Class III check but these were removed
         * due to failure cases. It's possible this could be restricted to a narrower
         * set of a failure cases. Currently, the logic is any unfolding across more
         * than one icosahedron face is not permitted.
         */
            internal static readonly bool[,] FAILED_DIRECTIONS =
            {
                {false, false, false, false, false, false, false}, // 0
                {false, false, false, false, false, false, false}, // 1
                {false, false, false, false, true, true, false}, // 2
                {false, false, false, false, true, false, true}, // 3
                {false, false, true, true, false, false, false}, // 4
                {false, false, true, false, false, false, true}, // 5
                {false, false, false, true, false, true, false}, // 6
            };
        }

        internal static class Vertex
        {
            /// <summary>
            /// Invalid vertex number
            /// </summary>
            internal const int INVALID_VERTEX_NUM = -1;

            /// <summary>
            /// Max number of faces a base cell's descendants may appear on
            /// </summary>
            internal const int MAX_BASE_CELL_FACES = 5;

            internal const int DIRECTION_INDEX_OFFSET = 2;

            /// <summary>
            /// Table of direction-to-face mapping for each pentagon
            ///
            /// Note that faces are in directional order, starting at J_AXES_DIGIT.
            /// This table is generated by the generatePentagonDirectionFaces script.
            /// </summary>
            /// <remarks>
            /// TODO: Need to create generatePentagonDirectionFaces script equivalent
            /// </remarks>
            /// <!--
            /// vertex.c
            /// -->
            internal static readonly PentagonDirectionFace[] PentagonDirectionFaces =
            {
                new PentagonDirectionFace(4, 4, 0, 2, 1, 3),
                new PentagonDirectionFace(14, 6, 11, 2, 7, 1),
                new PentagonDirectionFace(24, 5, 10, 1, 6, 0),
                new PentagonDirectionFace(38, 7, 12, 3, 8, 2),
                new PentagonDirectionFace(49, 9, 14, 0, 5, 4),
                new PentagonDirectionFace(58, 8, 13, 4, 9, 3),
                new PentagonDirectionFace(63, 11, 6, 15, 10, 16),
                new PentagonDirectionFace(72, 12, 7, 16, 11, 17),
                new PentagonDirectionFace(83, 10, 5, 19, 14, 15),
                new PentagonDirectionFace(97, 13, 8, 17, 12, 18),
                new PentagonDirectionFace(107, 14, 9, 18, 13, 19),
                new PentagonDirectionFace(117, 15, 19, 17, 18, 16),
            };

            /// <summary>
            /// Hexagon direction to vertex number relationships (same face).
            ///
            /// Note that we don't use direction 0 (center).
            /// </summary>
            internal static int[] DirectionToVertexNumHex =
                {(int) Direction.INVALID_DIGIT, 3, 1, 2, 5, 4, 0};

            /// <summary>
            /// Pentagon direction to vertex number relationships (same face).
            /// Note that we don't use directions 0 (center) or 1 (deleted K axis).
            /// </summary>
            internal static int[] DirectionToVertexNumPent =
                {(int) Direction.INVALID_DIGIT, (int) Direction.INVALID_DIGIT, 1, 2, 4, 3, 0};

        }

    }
}
