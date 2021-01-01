namespace H3Lib
{
    /// <summary>
    /// Base cell related lookup tables and access functions.
    /// </summary>
    /// <!-- Based off 3.1.1 -->
    public class BaseCells
    {
        /// <summary>
        /// Information on a single base cell
        /// </summary>
        /// <!-- Based off 3.1.1 -->
        public struct BaseCellData
        {
            /// <summary>
            /// "Home" face and normalized ijk coordinates on that face
            /// </summary>
            public FaceIjk homeFijk;
            /// <summary>
            /// Is this base cell a pentagon?
            /// </summary>
            public int isPentagon;
            /// <summary>
            /// If it's a pentagon, what are its two clockwise offset faces?
            /// </summary>
            public int[] cwOffsetPent;// [2]

            public BaseCellData(int face, int faceI, int faceJ, int faceK, int isPenta, int offset1, int offset2) : this()
            {
                homeFijk = new FaceIjk(face, new CoordIjk(faceI, faceJ, faceK));
                isPentagon = isPenta;
                cwOffsetPent = new[] {offset1, offset2};
            }
        }

        /// <summary>
        /// base cell at a given ijk and required rotations into its system
        /// </summary>
        /// <!-- Based off 3.1.1 -->
        public struct BaseCellOrient
        {
            /// <summary>
            /// base cell number
            /// </summary>
            public int baseCell;
            /// <summary>
            /// number of ccw 60 degree rotations relative to current face
            /// </summary>
            public int ccwRot60;

            public BaseCellOrient(int b, int c) : this()
            {
                baseCell = b;
                ccwRot60 = c;
            }
        }

        /// <summary>
        /// Neighboring base cell ID in each IJK direction.
        ///
        /// For each base cell, for each direction, the neighboring base
        /// cell ID is given. 127 indicates there is no neighbor in that direction.
        /// </summary>
        /// <!-- Based off 3.1.1 -->
        public static readonly int[,] baseCellNeighbors = {
            {0, 1, 5, 2, 4, 3, 8},                          // base cell 0
            {1, 7, 6, 9, 0, 3, 2},                          // base cell 1
            {2, 6, 10, 11, 0, 1, 5},                        // base cell 2
            {3, 13, 1, 7, 4, 12, 0},                        // base cell 3
            {4, INVALID_BASE_CELL, 15, 8, 3, 0, 12},        // base cell 4 (pentagon)
            {5, 2, 18, 10, 8, 0, 16},                       // base cell 5
            {6, 14, 11, 17, 1, 9, 2},                       // base cell 6
            {7, 21, 9, 19, 3, 13, 1},                       // base cell 7
            {8, 5, 22, 16, 4, 0, 15},                       // base cell 8
            {9, 19, 14, 20, 1, 7, 6},                       // base cell 9
            {10, 11, 24, 23, 5, 2, 18},                     // base cell 10
            {11, 17, 23, 25, 2, 6, 10},                     // base cell 11
            {12, 28, 13, 26, 4, 15, 3},                     // base cell 12
            {13, 26, 21, 29, 3, 12, 7},                     // base cell 13
            {14, INVALID_BASE_CELL, 17, 27, 9, 20, 6},      // base cell 14 (pentagon)
            {15, 22, 28, 31, 4, 8, 12},                     // base cell 15
            {16, 18, 33, 30, 8, 5, 22},                     // base cell 16
            {17, 11, 14, 6, 35, 25, 27},                    // base cell 17
            {18, 24, 30, 32, 5, 10, 16},                    // base cell 18
            {19, 34, 20, 36, 7, 21, 9},                     // base cell 19
            {20, 14, 19, 9, 40, 27, 36},                    // base cell 20
            {21, 38, 19, 34, 13, 29, 7},                    // base cell 21
            {22, 16, 41, 33, 15, 8, 31},                    // base cell 22
            {23, 24, 11, 10, 39, 37, 25},                   // base cell 23
            {24, INVALID_BASE_CELL, 32, 37, 10, 23, 18},    // base cell 24 (pentagon)
            {25, 23, 17, 11, 45, 39, 35},                   // base cell 25
            {26, 42, 29, 43, 12, 28, 13},                   // base cell 26
            {27, 40, 35, 46, 14, 20, 17},                   // base cell 27
            {28, 31, 42, 44, 12, 15, 26},                   // base cell 28
            {29, 43, 38, 47, 13, 26, 21},                   // base cell 29
            {30, 32, 48, 50, 16, 18, 33},                   // base cell 30
            {31, 41, 44, 53, 15, 22, 28},                   // base cell 31
            {32, 30, 24, 18, 52, 50, 37},                   // base cell 32
            {33, 30, 49, 48, 22, 16, 41},                   // base cell 33
            {34, 19, 38, 21, 54, 36, 51},                   // base cell 34
            {35, 46, 45, 56, 17, 27, 25},                   // base cell 35
            {36, 20, 34, 19, 55, 40, 54},                   // base cell 36
            {37, 39, 52, 57, 24, 23, 32},                   // base cell 37
            {38, INVALID_BASE_CELL, 34, 51, 29, 47, 21},    // base cell 38 (pentagon)
            {39, 37, 25, 23, 59, 57, 45},                   // base cell 39
            {40, 27, 36, 20, 60, 46, 55},                   // base cell 40
            {41, 49, 53, 61, 22, 33, 31},                   // base cell 41
            {42, 58, 43, 62, 28, 44, 26},                   // base cell 42
            {43, 62, 47, 64, 26, 42, 29},                   // base cell 43
            {44, 53, 58, 65, 28, 31, 42},                   // base cell 44
            {45, 39, 35, 25, 63, 59, 56},                   // base cell 45
            {46, 60, 56, 68, 27, 40, 35},                   // base cell 46
            {47, 38, 43, 29, 69, 51, 64},                   // base cell 47
            {48, 49, 30, 33, 67, 66, 50},                   // base cell 48
            {49, INVALID_BASE_CELL, 61, 66, 33, 48, 41},    // base cell 49 (pentagon)
            {50, 48, 32, 30, 70, 67, 52},                   // base cell 50
            {51, 69, 54, 71, 38, 47, 34},                   // base cell 51
            {52, 57, 70, 74, 32, 37, 50},                   // base cell 52
            {53, 61, 65, 75, 31, 41, 44},                   // base cell 53
            {54, 71, 55, 73, 34, 51, 36},                   // base cell 54
            {55, 40, 54, 36, 72, 60, 73},                   // base cell 55
            {56, 68, 63, 77, 35, 46, 45},                   // base cell 56
            {57, 59, 74, 78, 37, 39, 52},                   // base cell 57
            {58, INVALID_BASE_CELL, 62, 76, 44, 65, 42},    // base cell 58 (pentagon)
            {59, 63, 78, 79, 39, 45, 57},                   // base cell 59
            {60, 72, 68, 80, 40, 55, 46},                   // base cell 60
            {61, 53, 49, 41, 81, 75, 66},                   // base cell 61
            {62, 43, 58, 42, 82, 64, 76},                   // base cell 62
            {63, INVALID_BASE_CELL, 56, 45, 79, 59, 77},    // base cell 63 (pentagon)
            {64, 47, 62, 43, 84, 69, 82},                   // base cell 64
            {65, 58, 53, 44, 86, 76, 75},                   // base cell 65
            {66, 67, 81, 85, 49, 48, 61},                   // base cell 66
            {67, 66, 50, 48, 87, 85, 70},                   // base cell 67
            {68, 56, 60, 46, 90, 77, 80},                   // base cell 68
            {69, 51, 64, 47, 89, 71, 84},                   // base cell 69
            {70, 67, 52, 50, 83, 87, 74},                   // base cell 70
            {71, 89, 73, 91, 51, 69, 54},                   // base cell 71
            {72, INVALID_BASE_CELL, 73, 55, 80, 60, 88},    // base cell 72 (pentagon)
            {73, 91, 72, 88, 54, 71, 55},                   // base cell 73
            {74, 78, 83, 92, 52, 57, 70},                   // base cell 74
            {75, 65, 61, 53, 94, 86, 81},                   // base cell 75
            {76, 86, 82, 96, 58, 65, 62},                   // base cell 76
            {77, 63, 68, 56, 93, 79, 90},                   // base cell 77
            {78, 74, 59, 57, 95, 92, 79},                   // base cell 78
            {79, 78, 63, 59, 93, 95, 77},                   // base cell 79
            {80, 68, 72, 60, 99, 90, 88},                   // base cell 80
            {81, 85, 94, 101, 61, 66, 75},                  // base cell 81
            {82, 96, 84, 98, 62, 76, 64},                   // base cell 82
            {83, INVALID_BASE_CELL, 74, 70, 100, 87, 92},   // base cell 83 (pentagon)
            {84, 69, 82, 64, 97, 89, 98},                   // base cell 84
            {85, 87, 101, 102, 66, 67, 81},                 // base cell 85
            {86, 76, 75, 65, 104, 96, 94},                  // base cell 86
            {87, 83, 102, 100, 67, 70, 85},                 // base cell 87
            {88, 72, 91, 73, 99, 80, 105},                  // base cell 88
            {89, 97, 91, 103, 69, 84, 71},                  // base cell 89
            {90, 77, 80, 68, 106, 93, 99},                  // base cell 90
            {91, 73, 89, 71, 105, 88, 103},                 // base cell 91
            {92, 83, 78, 74, 108, 100, 95},                 // base cell 92
            {93, 79, 90, 77, 109, 95, 106},                 // base cell 93
            {94, 86, 81, 75, 107, 104, 101},                // base cell 94
            {95, 92, 79, 78, 109, 108, 93},                 // base cell 95
            {96, 104, 98, 110, 76, 86, 82},                 // base cell 96
            {97, INVALID_BASE_CELL, 98, 84, 103, 89, 111},  // base cell 97 (pentagon)
            {98, 110, 97, 111, 82, 96, 84},                 // base cell 98
            {99, 80, 105, 88, 106, 90, 113},                // base cell 99
            {100, 102, 83, 87, 108, 114, 92},               // base cell 100
            {101, 102, 107, 112, 81, 85, 94},               // base cell 101
            {102, 101, 87, 85, 114, 112, 100},              // base cell 102
            {103, 91, 97, 89, 116, 105, 111},               // base cell 103
            {104, 107, 110, 115, 86, 94, 96},               // base cell 104
            {105, 88, 103, 91, 113, 99, 116},               // base cell 105
            {106, 93, 99, 90, 117, 109, 113},                   // base cell 106
            {107, INVALID_BASE_CELL, 101, 94, 115, 104, 112},   // base cell 107 (pentagon)
            {108, 100, 95, 92, 118, 114, 109},                  // base cell 108
            {109, 108, 93, 95, 117, 118, 106},                  // base cell 109
            {110, 98, 104, 96, 119, 111, 115},                  // base cell 110
            {111, 97, 110, 98, 116, 103, 119},                  // base cell 111
            {112, 107, 102, 101, 120, 115, 114},                // base cell 112
            {113, 99, 116, 105, 117, 106, 121},                 // base cell 113
            {114, 112, 100, 102, 118, 120, 108},                // base cell 114
            {115, 110, 107, 104, 120, 119, 112},                // base cell 115
            {116, 103, 119, 111, 113, 105, 121},                // base cell 116
            {117, INVALID_BASE_CELL, 109, 118, 113, 121, 106},  // base cell 117 (pentagon)
            {118, 120, 108, 114, 117, 121, 109},                // base cell 118
            {119, 111, 115, 110, 121, 116, 120},                // base cell 119
            {120, 115, 114, 112, 121, 119, 118},                // base cell 120
            {121, 116, 120, 119, 117, 113, 118},                // base cell 121
        };

        /// <summary>
        /// Neighboring base cell rotations in each IJK direction.
        ///
        /// For each base cell, for each direction, the number of 60 degree
        /// CCW rotations to the coordinate system of the neighbor is given.
        /// -1 indicates there is no neighbor in that direction.
        /// </summary>
        /// <!-- Based off 3.1.1 -->
        public static readonly int[,] baseCellNeighbor60CCWRots =
        {
            {0, 5, 0, 0, 1, 5, 1},   // base cell 0
            {0, 0, 1, 0, 1, 0, 1},   // base cell 1
            {0, 0, 0, 0, 0, 5, 0},   // base cell 2
            {0, 5, 0, 0, 2, 5, 1},   // base cell 3
            {0, -1, 1, 0, 3, 4, 2},  // base cell 4 (pentagon)
            {0, 0, 1, 0, 1, 0, 1},   // base cell 5
            {0, 0, 0, 3, 5, 5, 0},   // base cell 6
            {0, 0, 0, 0, 0, 5, 0},   // base cell 7
            {0, 5, 0, 0, 0, 5, 1},   // base cell 8
            {0, 0, 1, 3, 0, 0, 1},   // base cell 9
            {0, 0, 1, 3, 0, 0, 1},   // base cell 10
            {0, 3, 3, 3, 0, 0, 0},   // base cell 11
            {0, 5, 0, 0, 3, 5, 1},   // base cell 12
            {0, 0, 1, 0, 1, 0, 1},   // base cell 13
            {0, -1, 3, 0, 5, 2, 0},  // base cell 14 (pentagon)
            {0, 5, 0, 0, 4, 5, 1},   // base cell 15
            {0, 0, 0, 0, 0, 5, 0},   // base cell 16
            {0, 3, 3, 3, 3, 0, 3},   // base cell 17
            {0, 0, 0, 3, 5, 5, 0},   // base cell 18
            {0, 3, 3, 3, 0, 0, 0},   // base cell 19
            {0, 3, 3, 3, 0, 3, 0},   // base cell 20
            {0, 0, 0, 3, 5, 5, 0},   // base cell 21
            {0, 0, 1, 0, 1, 0, 1},   // base cell 22
            {0, 3, 3, 3, 0, 3, 0},   // base cell 23
            {0, -1, 3, 0, 5, 2, 0},  // base cell 24 (pentagon)
            {0, 0, 0, 3, 0, 0, 3},   // base cell 25
            {0, 0, 0, 0, 0, 5, 0},   // base cell 26
            {0, 3, 0, 0, 0, 3, 3},   // base cell 27
            {0, 0, 1, 0, 1, 0, 1},   // base cell 28
            {0, 0, 1, 3, 0, 0, 1},   // base cell 29
            {0, 3, 3, 3, 0, 0, 0},   // base cell 30
            {0, 0, 0, 0, 0, 5, 0},   // base cell 31
            {0, 3, 3, 3, 3, 0, 3},   // base cell 32
            {0, 0, 1, 3, 0, 0, 1},   // base cell 33
            {0, 3, 3, 3, 3, 0, 3},   // base cell 34
            {0, 0, 3, 0, 3, 0, 3},   // base cell 35
            {0, 0, 0, 3, 0, 0, 3},   // base cell 36
            {0, 3, 0, 0, 0, 3, 3},   // base cell 37
            {0, -1, 3, 0, 5, 2, 0},  // base cell 38 (pentagon)
            {0, 3, 0, 0, 3, 3, 0},   // base cell 39
            {0, 3, 0, 0, 3, 3, 0},   // base cell 40
            {0, 0, 0, 3, 5, 5, 0},   // base cell 41
            {0, 0, 0, 3, 5, 5, 0},   // base cell 42
            {0, 3, 3, 3, 0, 0, 0},   // base cell 43
            {0, 0, 1, 3, 0, 0, 1},   // base cell 44
            {0, 0, 3, 0, 0, 3, 3},   // base cell 45
            {0, 0, 0, 3, 0, 3, 0},   // base cell 46
            {0, 3, 3, 3, 0, 3, 0},   // base cell 47
            {0, 3, 3, 3, 0, 3, 0},   // base cell 48
            {0, -1, 3, 0, 5, 2, 0},  // base cell 49 (pentagon)
            {0, 0, 0, 3, 0, 0, 3},   // base cell 50
            {0, 3, 0, 0, 0, 3, 3},   // base cell 51
            {0, 0, 3, 0, 3, 0, 3},   // base cell 52
            {0, 3, 3, 3, 0, 0, 0},   // base cell 53
            {0, 0, 3, 0, 3, 0, 3},   // base cell 54
            {0, 0, 3, 0, 0, 3, 3},   // base cell 55
            {0, 3, 3, 3, 0, 0, 3},   // base cell 56
            {0, 0, 0, 3, 0, 3, 0},   // base cell 57
            {0, -1, 3, 0, 5, 2, 0},  // base cell 58 (pentagon)
            {0, 3, 3, 3, 3, 3, 0},   // base cell 59
            {0, 3, 3, 3, 3, 3, 0},   // base cell 60
            {0, 3, 3, 3, 3, 0, 3},   // base cell 61
            {0, 3, 3, 3, 3, 0, 3},   // base cell 62
            {0, -1, 3, 0, 5, 2, 0},  // base cell 63 (pentagon)
            {0, 0, 0, 3, 0, 0, 3},   // base cell 64
            {0, 3, 3, 3, 0, 3, 0},   // base cell 65
            {0, 3, 0, 0, 0, 3, 3},   // base cell 66
            {0, 3, 0, 0, 3, 3, 0},   // base cell 67
            {0, 3, 3, 3, 0, 0, 0},   // base cell 68
            {0, 3, 0, 0, 3, 3, 0},   // base cell 69
            {0, 0, 3, 0, 0, 3, 3},   // base cell 70
            {0, 0, 0, 3, 0, 3, 0},   // base cell 71
            {0, -1, 3, 0, 5, 2, 0},  // base cell 72 (pentagon)
            {0, 3, 3, 3, 0, 0, 3},   // base cell 73
            {0, 3, 3, 3, 0, 0, 3},   // base cell 74
            {0, 0, 0, 3, 0, 0, 3},   // base cell 75
            {0, 3, 0, 0, 0, 3, 3},   // base cell 76
            {0, 0, 0, 3, 0, 5, 0},   // base cell 77
            {0, 3, 3, 3, 0, 0, 0},   // base cell 78
            {0, 0, 1, 3, 1, 0, 1},   // base cell 79
            {0, 0, 1, 3, 1, 0, 1},   // base cell 80
            {0, 0, 3, 0, 3, 0, 3},   // base cell 81
            {0, 0, 3, 0, 3, 0, 3},   // base cell 82
            {0, -1, 3, 0, 5, 2, 0},  // base cell 83 (pentagon)
            {0, 0, 3, 0, 0, 3, 3},   // base cell 84
            {0, 0, 0, 3, 0, 3, 0},   // base cell 85
            {0, 3, 0, 0, 3, 3, 0},   // base cell 86
            {0, 3, 3, 3, 3, 3, 0},   // base cell 87
            {0, 0, 0, 3, 0, 5, 0},   // base cell 88
            {0, 3, 3, 3, 3, 3, 0},   // base cell 89
            {0, 0, 0, 0, 0, 0, 1},   // base cell 90
            {0, 3, 3, 3, 0, 0, 0},   // base cell 91
            {0, 0, 0, 3, 0, 5, 0},   // base cell 92
            {0, 5, 0, 0, 5, 5, 0},   // base cell 93
            {0, 0, 3, 0, 0, 3, 3},   // base cell 94
            {0, 0, 0, 0, 0, 0, 1},   // base cell 95
            {0, 0, 0, 3, 0, 3, 0},   // base cell 96
            {0, -1, 3, 0, 5, 2, 0},  // base cell 97 (pentagon)
            {0, 3, 3, 3, 0, 0, 3},   // base cell 98
            {0, 5, 0, 0, 5, 5, 0},   // base cell 99
            {0, 0, 1, 3, 1, 0, 1},   // base cell 100
            {0, 3, 3, 3, 0, 0, 3},   // base cell 101
            {0, 3, 3, 3, 0, 0, 0},   // base cell 102
            {0, 0, 1, 3, 1, 0, 1},   // base cell 103
            {0, 3, 3, 3, 3, 3, 0},   // base cell 104
            {0, 0, 0, 0, 0, 0, 1},   // base cell 105
            {0, 0, 1, 0, 3, 5, 1},   // base cell 106
            {0, -1, 3, 0, 5, 2, 0},  // base cell 107 (pentagon)
            {0, 5, 0, 0, 5, 5, 0},   // base cell 108
            {0, 0, 1, 0, 4, 5, 1},   // base cell 109
            {0, 3, 3, 3, 0, 0, 0},   // base cell 110
            {0, 0, 0, 3, 0, 5, 0},   // base cell 111
            {0, 0, 0, 3, 0, 5, 0},   // base cell 112
            {0, 0, 1, 0, 2, 5, 1},   // base cell 113
            {0, 0, 0, 0, 0, 0, 1},   // base cell 114
            {0, 0, 1, 3, 1, 0, 1},   // base cell 115
            {0, 5, 0, 0, 5, 5, 0},   // base cell 116
            {0, -1, 1, 0, 3, 4, 2},  // base cell 117 (pentagon)
            {0, 0, 1, 0, 0, 5, 1},   // base cell 118
            {0, 0, 0, 0, 0, 0, 1},   // base cell 119
            {0, 5, 0, 0, 5, 5, 0},   // base cell 120
            {0, 0, 1, 0, 1, 5, 1},   // base cell 121
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
        /// This table can be accessed using the functions <see cref="_faceIjkToBaseCell"/>
        /// and <see cref="_faceIjkToBaseCellCCWrot60"/>
        /// </summary>
        /// <!-- Based off 3.1.1 -->
        public static readonly BaseCellOrient[,,,] faceIjkBaseCells =
        {
            {// face 0
                {   // i 0
                    { new BaseCellOrient(16, 0), new BaseCellOrient(18, 0), new BaseCellOrient(24, 0) },  // j 0
                    { new BaseCellOrient(33, 0), new BaseCellOrient(30, 0), new BaseCellOrient(32, 3) },  // j 1
                    { new BaseCellOrient(49, 1), new BaseCellOrient(48, 3), new BaseCellOrient(50, 3) }   // j 2
                },
                {   // i 1
                    { new BaseCellOrient(8, 0), new BaseCellOrient(5, 5), new BaseCellOrient(10, 5) },    // j 0
                    { new BaseCellOrient(22, 0), new BaseCellOrient(16, 0), new BaseCellOrient(18, 0) },  // j 1
                    { new BaseCellOrient(41, 1), new BaseCellOrient(33, 0), new BaseCellOrient(30, 0) }   // j 2
                },
                {   // i 2
                    { new BaseCellOrient(4, 0), new BaseCellOrient(0, 5), new BaseCellOrient(2, 5) },    // j 0
                    { new BaseCellOrient(15, 1), new BaseCellOrient(8, 0), new BaseCellOrient(5, 5) },   // j 1
                    { new BaseCellOrient(31, 1), new BaseCellOrient(22, 0), new BaseCellOrient(16, 0) }  // j 2
                }
            },
            {// face 1
                {
                    // i 0
                    { new BaseCellOrient(2, 0), new BaseCellOrient(6, 0), new BaseCellOrient(14, 0) },    // j 0
                    { new BaseCellOrient(10, 0), new BaseCellOrient(11, 0), new BaseCellOrient(17, 3) },  // j 1
                    { new BaseCellOrient(24, 1), new BaseCellOrient(23, 3), new BaseCellOrient(25, 3) }   // j 2
                },
                {
                    // i 1
                    {new BaseCellOrient(0, 0), new BaseCellOrient(1, 5), new BaseCellOrient(9, 5) },    // j 0
                    {new BaseCellOrient(5, 0), new BaseCellOrient(2, 0), new BaseCellOrient(6, 0) },    // j 1
                    {new BaseCellOrient(18, 1), new BaseCellOrient(10, 0), new BaseCellOrient(11, 0) }  // j 2
                },
                {
                    // i 2
                    {new BaseCellOrient(4, 1), new BaseCellOrient(3, 5), new BaseCellOrient(7, 5) },  // j 0
                    {new BaseCellOrient(8, 1), new BaseCellOrient(0, 0), new BaseCellOrient(1, 5) },  // j 1
                    {new BaseCellOrient(16, 1), new BaseCellOrient(5, 0), new BaseCellOrient(2, 0) }  // j 2
                }
            },
            {// face 2
                {
                    // i 0
                    {new BaseCellOrient(7, 0), new BaseCellOrient(21, 0), new BaseCellOrient(38, 0) },  // j 0
                    {new BaseCellOrient(9, 0), new BaseCellOrient(19, 0), new BaseCellOrient(34, 3) },  // j 1
                    {new BaseCellOrient(14, 1), new BaseCellOrient(20, 3), new BaseCellOrient(36, 3) }  // j 2
                },
                {
                    // i 1
                    {new BaseCellOrient(3, 0), new BaseCellOrient(13, 5), new BaseCellOrient(29, 5) },  // j 0
                    {new BaseCellOrient(1, 0), new BaseCellOrient(7, 0), new BaseCellOrient(21, 0) },   // j 1
                    {new BaseCellOrient(6, 1), new BaseCellOrient(9, 0), new BaseCellOrient(19, 0) }    // j 2
                },
                {
                    // i 2
                    { new BaseCellOrient(4, 2), new BaseCellOrient(12, 5), new BaseCellOrient(26, 5) },  // j 0
                    { new BaseCellOrient(0, 1), new BaseCellOrient(3, 0), new BaseCellOrient(13, 5) },   // j 1
                    { new BaseCellOrient(2, 1), new BaseCellOrient(1, 0), new BaseCellOrient(7, 0) }     // j 2
                }
            },
            {// face 3
                {
                    // i 0
                    { new BaseCellOrient(26, 0), new BaseCellOrient(42, 0), new BaseCellOrient(58, 0) },  // j 0
                    { new BaseCellOrient(29, 0), new BaseCellOrient(43, 0), new BaseCellOrient(62, 3) },  // j 1
                    { new BaseCellOrient(38, 1), new BaseCellOrient(47, 3), new BaseCellOrient(64, 3) }   // j 2
                },
                {
                    // i 1
                    { new BaseCellOrient(12, 0), new BaseCellOrient(28, 5), new BaseCellOrient(44, 5) },  // j 0
                    { new BaseCellOrient(13, 0), new BaseCellOrient(26, 0), new BaseCellOrient(42, 0) },  // j 1
                    { new BaseCellOrient(21, 1), new BaseCellOrient(29, 0), new BaseCellOrient(43, 0) }   // j 2
                },
                {
                    // i 2
                    { new BaseCellOrient(4, 3), new BaseCellOrient(15, 5), new BaseCellOrient(31, 5) },  // j 0
                    { new BaseCellOrient(3, 1), new BaseCellOrient(12, 0), new BaseCellOrient(28, 5) },  // j 1
                    { new BaseCellOrient(7, 1), new BaseCellOrient(13, 0), new BaseCellOrient(26, 0) }   // j 2
                }
            },
            {// face 4
                {
                    // i 0
                    { new BaseCellOrient(31, 0), new BaseCellOrient(41, 0), new BaseCellOrient(49, 0) },  // j 0
                    { new BaseCellOrient(44, 0), new BaseCellOrient(53, 0), new BaseCellOrient(61, 3) },  // j 1
                    { new BaseCellOrient(58, 1), new BaseCellOrient(65, 3), new BaseCellOrient(75, 3) }   // j 2
                },
                {
                    // i 1
                    { new BaseCellOrient(15, 0), new BaseCellOrient(22, 5), new BaseCellOrient(33, 5) },  // j 0
                    { new BaseCellOrient(28, 0), new BaseCellOrient(31, 0), new BaseCellOrient(41, 0) },  // j 1
                    { new BaseCellOrient(42, 1), new BaseCellOrient(44, 0), new BaseCellOrient(53, 0) }   // j 2
                },
                {
                    // i 2
                    { new BaseCellOrient(4, 4), new BaseCellOrient(8, 5), new BaseCellOrient(16, 5) },    // j 0
                    { new BaseCellOrient(12, 1), new BaseCellOrient(15, 0), new BaseCellOrient(22, 5) },  // j 1
                    { new BaseCellOrient(26, 1), new BaseCellOrient(28, 0), new BaseCellOrient(31, 0) }   // j 2
                }
            },
            {// face 5
                {
                    // i 0
                    { new BaseCellOrient(50, 0), new BaseCellOrient(48, 0), new BaseCellOrient(49, 3) },  // j 0
                    { new BaseCellOrient(32, 0), new BaseCellOrient(30, 3), new BaseCellOrient(33, 3) },  // j 1
                    { new BaseCellOrient(24, 3), new BaseCellOrient(18, 3), new BaseCellOrient(16, 3) }   // j 2
                },
                {
                    // i 1
                    { new BaseCellOrient(70, 0), new BaseCellOrient(67, 0), new BaseCellOrient(66, 3) },  // j 0
                    { new BaseCellOrient(52, 3), new BaseCellOrient(50, 0), new BaseCellOrient(48, 0) },  // j 1
                    { new BaseCellOrient(37, 3), new BaseCellOrient(32, 0), new BaseCellOrient(30, 3) }   // j 2
                },
                {
                    // i 2
                    { new BaseCellOrient(83, 0), new BaseCellOrient(87, 3), new BaseCellOrient(85, 3) },  // j 0
                    { new BaseCellOrient(74, 3), new BaseCellOrient(70, 0), new BaseCellOrient(67, 0) },  // j 1
                    { new BaseCellOrient(57, 1), new BaseCellOrient(52, 3), new BaseCellOrient(50, 0) }   // j 2
                }
            },
            {// face 6
                {
                    // i 0
                    { new BaseCellOrient(25, 0), new BaseCellOrient(23, 0), new BaseCellOrient(24, 3) },  // j 0
                    { new BaseCellOrient(17, 0), new BaseCellOrient(11, 3), new BaseCellOrient(10, 3) },  // j 1
                    { new BaseCellOrient(14, 3), new BaseCellOrient(6, 3), new BaseCellOrient(2, 3) }     // j 2
                },
                {
                    // i 1
                    { new BaseCellOrient(45, 0), new BaseCellOrient(39, 0), new BaseCellOrient(37, 3) },  // j 0
                    { new BaseCellOrient(35, 3), new BaseCellOrient(25, 0), new BaseCellOrient(23, 0) },  // j 1
                    { new BaseCellOrient(27, 3), new BaseCellOrient(17, 0), new BaseCellOrient(11, 3) }   // j 2
                },
                {
                    // i 2
                    { new BaseCellOrient(63, 0), new BaseCellOrient(59, 3), new BaseCellOrient(57, 3) },  // j 0
                    { new BaseCellOrient(56, 3), new BaseCellOrient(45, 0), new BaseCellOrient(39, 0) },  // j 1
                    { new BaseCellOrient(46, 3), new BaseCellOrient(35, 3), new BaseCellOrient(25, 0) }   // j 2
                }
            },
            {// face 7
                {
                    // i 0
                    { new BaseCellOrient(36, 0), new BaseCellOrient(20, 0), new BaseCellOrient(14, 3) },  // j 0
                    { new BaseCellOrient(34, 0), new BaseCellOrient(19, 3), new BaseCellOrient(9, 3) },   // j 1
                    { new BaseCellOrient(38, 3), new BaseCellOrient(21, 3), new BaseCellOrient(7, 3) }    // j 2
                },
                {
                    // i 1
                    { new BaseCellOrient(55, 0), new BaseCellOrient(40, 0), new BaseCellOrient(27, 3) },  // j 0
                    { new BaseCellOrient(54, 3), new BaseCellOrient(36, 0), new BaseCellOrient(20, 0) },  // j 1
                    { new BaseCellOrient(51, 3), new BaseCellOrient(34, 0), new BaseCellOrient(19, 3) }   // j 2
                },
                {
                    // i 2
                    { new BaseCellOrient(72, 0), new BaseCellOrient(60, 3), new BaseCellOrient(46, 3) },  // j 0
                    { new BaseCellOrient(73, 3), new BaseCellOrient(55, 0), new BaseCellOrient(40, 0) },  // j 1
                    { new BaseCellOrient(71, 3), new BaseCellOrient(54, 3), new BaseCellOrient(36, 0) }   // j 2
                }
            },
            {// face 8
                {
                    // i 0
                    { new BaseCellOrient(64, 0), new BaseCellOrient(47, 0), new BaseCellOrient(38, 3) },  // j 0
                    { new BaseCellOrient(62, 0), new BaseCellOrient(43, 3), new BaseCellOrient(29, 3) },  // j 1
                    { new BaseCellOrient(58, 3), new BaseCellOrient(42, 3), new BaseCellOrient(26, 3) }   // j 2
                },
                {
                    // i 1
                    { new BaseCellOrient(84, 0), new BaseCellOrient(69, 0), new BaseCellOrient(51, 3) },  // j 0
                    { new BaseCellOrient(82, 3), new BaseCellOrient(64, 0), new BaseCellOrient(47, 0) },  // j 1
                    { new BaseCellOrient(76, 3), new BaseCellOrient(62, 0), new BaseCellOrient(43, 3) }   // j 2
                },
                {
                    // i 2
                    { new BaseCellOrient(97, 0), new BaseCellOrient(89, 3), new BaseCellOrient(71, 3) },  // j 0
                    { new BaseCellOrient(98, 3), new BaseCellOrient(84, 0), new BaseCellOrient(69, 0) },  // j 1
                    { new BaseCellOrient(96, 3), new BaseCellOrient(82, 3), new BaseCellOrient(64, 0) }   // j 2
                }
            },
            {// face 9
                {
                    // i 0
                    { new BaseCellOrient(75, 0), new BaseCellOrient(65, 0), new BaseCellOrient(58, 3) },  // j 0
                    { new BaseCellOrient(61, 0), new BaseCellOrient(53, 3), new BaseCellOrient(44, 3) },  // j 1
                    { new BaseCellOrient(49, 3), new BaseCellOrient(41, 3), new BaseCellOrient(31, 3) }   // j 2
                },
                {
                    // i 1
                    { new BaseCellOrient(94, 0), new BaseCellOrient(86, 0), new BaseCellOrient(76, 3) },  // j 0
                    { new BaseCellOrient(81, 3), new BaseCellOrient(75, 0), new BaseCellOrient(65, 0) },  // j 1
                    { new BaseCellOrient(66, 3), new BaseCellOrient(61, 0), new BaseCellOrient(53, 3) }   // j 2
                },
                {
                    // i 2
                    { new BaseCellOrient(107, 0), new BaseCellOrient(104, 3), new BaseCellOrient(96, 3) },  // j 0
                    { new BaseCellOrient(101, 3), new BaseCellOrient(94, 0), new BaseCellOrient(86, 0) },   // j 1
                    { new BaseCellOrient(85, 3), new BaseCellOrient(81, 3), new BaseCellOrient(75, 0) }     // j 2
                }
            },
            {// face 10
                {
                    // i 0
                    { new BaseCellOrient(57, 0), new BaseCellOrient(59, 0), new BaseCellOrient(63, 3) },  // j 0
                    { new BaseCellOrient(74, 0), new BaseCellOrient(78, 3), new BaseCellOrient(79, 3) },  // j 1
                    { new BaseCellOrient(83, 3), new BaseCellOrient(92, 3), new BaseCellOrient(95, 3) }   // j 2
                },
                {
                    // i 1
                    { new BaseCellOrient(37, 0), new BaseCellOrient(39, 3), new BaseCellOrient(45, 3) },  // j 0
                    { new BaseCellOrient(52, 0), new BaseCellOrient(57, 0), new BaseCellOrient(59, 0) },  // j 1
                    { new BaseCellOrient(70, 3), new BaseCellOrient(74, 0), new BaseCellOrient(78, 3) }   // j 2
                },
                {
                    // i 2
                    { new BaseCellOrient(24, 0), new BaseCellOrient(23, 3), new BaseCellOrient(25, 3) },  // j 0
                    { new BaseCellOrient(32, 3), new BaseCellOrient(37, 0), new BaseCellOrient(39, 3) },  // j 1
                    { new BaseCellOrient(50, 3), new BaseCellOrient(52, 0), new BaseCellOrient(57, 0) }   // j 2
                }
            },
            {// face 11
                {
                    // i 0
                    { new BaseCellOrient(46, 0), new BaseCellOrient(60, 0), new BaseCellOrient(72, 3) },  // j 0
                    { new BaseCellOrient(56, 0), new BaseCellOrient(68, 3), new BaseCellOrient(80, 3) },  // j 1
                    { new BaseCellOrient(63, 3), new BaseCellOrient(77, 3), new BaseCellOrient(90, 3) }   // j 2
                },
                {
                    // i 1
                    { new BaseCellOrient(27, 0), new BaseCellOrient(40, 3), new BaseCellOrient(55, 3) },  // j 0
                    { new BaseCellOrient(35, 0), new BaseCellOrient(46, 0), new BaseCellOrient(60, 0) },  // j 1
                    { new BaseCellOrient(45, 3), new BaseCellOrient(56, 0), new BaseCellOrient(68, 3) }   // j 2
                },
                {
                    // i 2
                    { new BaseCellOrient(14, 0), new BaseCellOrient(20, 3), new BaseCellOrient(36, 3) },  // j 0
                    { new BaseCellOrient(17, 3), new BaseCellOrient(27, 0), new BaseCellOrient(40, 3) },  // j 1
                    { new BaseCellOrient(25, 3), new BaseCellOrient(35, 0), new BaseCellOrient(46, 0) }   // j 2
                }
            },
            {// face 12
                {
                    // i 0
                    { new BaseCellOrient(71, 0), new BaseCellOrient(89, 0), new BaseCellOrient(97, 3) },   // j 0
                    { new BaseCellOrient(73, 0), new BaseCellOrient(91, 3), new BaseCellOrient(103, 3) },  // j 1
                    { new BaseCellOrient(72, 3), new BaseCellOrient(88, 3), new BaseCellOrient(105, 3) }   // j 2
                },
                {
                    // i 1
                    { new BaseCellOrient(51, 0), new BaseCellOrient(69, 3), new BaseCellOrient(84, 3) },  // j 0
                    { new BaseCellOrient(54, 0), new BaseCellOrient(71, 0), new BaseCellOrient(89, 0) },  // j 1
                    { new BaseCellOrient(55, 3), new BaseCellOrient(73, 0), new BaseCellOrient(91, 3) }   // j 2
                },
                {
                    // i 2
                    { new BaseCellOrient(38, 0), new BaseCellOrient(47, 3), new BaseCellOrient(64, 3) },  // j 0
                    { new BaseCellOrient(34, 3), new BaseCellOrient(51, 0), new BaseCellOrient(69, 3) },  // j 1
                    { new BaseCellOrient(36, 3), new BaseCellOrient(54, 0), new BaseCellOrient(71, 0) }   // j 2
                }
            },
            {// face 13
                {
                    // i 0
                    { new BaseCellOrient(96, 0), new BaseCellOrient(104, 0), new BaseCellOrient(107, 3) },  // j 0
                    { new BaseCellOrient(98, 0), new BaseCellOrient(110, 3), new BaseCellOrient(115, 3) },  // j 1
                    { new BaseCellOrient(97, 3), new BaseCellOrient(111, 3), new BaseCellOrient(119, 3) }   // j 2
                },
                {
                    // i 1
                    { new BaseCellOrient(76, 0), new BaseCellOrient(86, 3), new BaseCellOrient(94, 3) },   // j 0
                    { new BaseCellOrient(82, 0), new BaseCellOrient(96, 0), new BaseCellOrient(104, 0) },  // j 1
                    { new BaseCellOrient(84, 3), new BaseCellOrient(98, 0), new BaseCellOrient(110, 3) }   // j 2
                },
                {
                    // i 2
                    { new BaseCellOrient(58, 0), new BaseCellOrient(65, 3), new BaseCellOrient(75, 3) },  // j 0
                    { new BaseCellOrient(62, 3), new BaseCellOrient(76, 0), new BaseCellOrient(86, 3) },  // j 1
                    { new BaseCellOrient(64, 3), new BaseCellOrient(82, 0), new BaseCellOrient(96, 0) }   // j 2
                }
            },
            {// face 14
                {
                    // i 0
                    { new BaseCellOrient(85, 0), new BaseCellOrient(87, 0), new BaseCellOrient(83, 3) },     // j 0
                    { new BaseCellOrient(101, 0), new BaseCellOrient(102, 3), new BaseCellOrient(100, 3) },  // j 1
                    { new BaseCellOrient(107, 3), new BaseCellOrient(112, 3), new BaseCellOrient(114, 3) }   // j 2
                },
                {
                    // i 1
                    { new BaseCellOrient(66, 0), new BaseCellOrient(67, 3), new BaseCellOrient(70, 3) },   // j 0
                    { new BaseCellOrient(81, 0), new BaseCellOrient(85, 0), new BaseCellOrient(87, 0) },   // j 1
                    { new BaseCellOrient(94, 3), new BaseCellOrient(101, 0), new BaseCellOrient(102, 3) }  // j 2
                },
                {
                    // i 2
                    { new BaseCellOrient(49, 0), new BaseCellOrient(48, 3), new BaseCellOrient(50, 3) },  // j 0
                    { new BaseCellOrient(61, 3), new BaseCellOrient(66, 0), new BaseCellOrient(67, 3) },  // j 1
                    { new BaseCellOrient(75, 3), new BaseCellOrient(81, 0), new BaseCellOrient(85, 0) }   // j 2
                }
            },
            {// face 15
                {
                    // i 0
                    { new BaseCellOrient(95, 0), new BaseCellOrient(92, 0), new BaseCellOrient(83, 0) },  // j 0
                    { new BaseCellOrient(79, 0), new BaseCellOrient(78, 0), new BaseCellOrient(74, 3) },  // j 1
                    { new BaseCellOrient(63, 1), new BaseCellOrient(59, 3), new BaseCellOrient(57, 3) }   // j 2
                },
                {
                    // i 1
                    { new BaseCellOrient(109, 0), new BaseCellOrient(108, 0), new BaseCellOrient(100, 5) },  // j 0
                    { new BaseCellOrient(93, 1), new BaseCellOrient(95, 0), new BaseCellOrient(92, 0) },     // j 1
                    { new BaseCellOrient(77, 1), new BaseCellOrient(79, 0), new BaseCellOrient(78, 0) }      // j 2
                },
                {
                    // i 2
                    { new BaseCellOrient(117, 4), new BaseCellOrient(118, 5), new BaseCellOrient(114, 5) },  // j 0
                    { new BaseCellOrient(106, 1), new BaseCellOrient(109, 0), new BaseCellOrient(108, 0) },  // j 1
                    { new BaseCellOrient(90, 1), new BaseCellOrient(93, 1), new BaseCellOrient(95, 0) }      // j 2
                }
            },
            {// face 16
                {
                    // i 0
                    { new BaseCellOrient(90, 0), new BaseCellOrient(77, 0), new BaseCellOrient(63, 0) },  // j 0
                    { new BaseCellOrient(80, 0), new BaseCellOrient(68, 0), new BaseCellOrient(56, 3) },  // j 1
                    { new BaseCellOrient(72, 1), new BaseCellOrient(60, 3), new BaseCellOrient(46, 3) }   // j 2
                },
                {
                    // i 1
                    { new BaseCellOrient(106, 0), new BaseCellOrient(93, 0), new BaseCellOrient(79, 5) },  // j 0
                    { new BaseCellOrient(99, 1), new BaseCellOrient(90, 0), new BaseCellOrient(77, 0) },   // j 1
                    { new BaseCellOrient(88, 1), new BaseCellOrient(80, 0), new BaseCellOrient(68, 0) }    // j 2
                },
                {
                    // i 2
                    { new BaseCellOrient(117, 3), new BaseCellOrient(109, 5), new BaseCellOrient(95, 5) },  // j 0
                    { new BaseCellOrient(113, 1), new BaseCellOrient(106, 0), new BaseCellOrient(93, 0) },  // j 1
                    { new BaseCellOrient(105, 1), new BaseCellOrient(99, 1), new BaseCellOrient(90, 0) }    // j 2
                }
            },
            {// face 17
                {
                    // i 0
                    { new BaseCellOrient(105, 0), new BaseCellOrient(88, 0), new BaseCellOrient(72, 0) },  // j 0
                    { new BaseCellOrient(103, 0), new BaseCellOrient(91, 0), new BaseCellOrient(73, 3) },  // j 1
                    { new BaseCellOrient(97, 1), new BaseCellOrient(89, 3), new BaseCellOrient(71, 3) }    // j 2
                },
                {
                    // i 1
                    { new BaseCellOrient(113, 0), new BaseCellOrient(99, 0), new BaseCellOrient(80, 5) },   // j 0
                    { new BaseCellOrient(116, 1), new BaseCellOrient(105, 0), new BaseCellOrient(88, 0) },  // j 1
                    { new BaseCellOrient(111, 1), new BaseCellOrient(103, 0), new BaseCellOrient(91, 0) }   // j 2
                },
                {
                    // i 2
                    { new BaseCellOrient(117, 2), new BaseCellOrient(106, 5), new BaseCellOrient(90, 5) },  // j 0
                    { new BaseCellOrient(121, 1), new BaseCellOrient(113, 0), new BaseCellOrient(99, 0) },  // j 1
                    { new BaseCellOrient(119, 1), new BaseCellOrient(116, 1), new BaseCellOrient(105, 0) }  // j 2
                }
            },
            {// face 18
                {
                    // i 0
                    { new BaseCellOrient(119, 0), new BaseCellOrient(111, 0), new BaseCellOrient(97, 0) },  // j 0
                    { new BaseCellOrient(115, 0), new BaseCellOrient(110, 0), new BaseCellOrient(98, 3) },  // j 1
                    { new BaseCellOrient(107, 1), new BaseCellOrient(104, 3), new BaseCellOrient(96, 3) }   // j 2
                },
                {
                    // i 1
                    { new BaseCellOrient(121, 0), new BaseCellOrient(116, 0), new BaseCellOrient(103, 5) },  // j 0
                    { new BaseCellOrient(120, 1), new BaseCellOrient(119, 0), new BaseCellOrient(111, 0) },  // j 1
                    { new BaseCellOrient(112, 1), new BaseCellOrient(115, 0), new BaseCellOrient(110, 0) }   // j 2
                },
                {
                    // i 2
                    { new BaseCellOrient(117, 1), new BaseCellOrient(113, 5), new BaseCellOrient(105, 5) },  // j 0
                    { new BaseCellOrient(118, 1), new BaseCellOrient(121, 0), new BaseCellOrient(116, 0) },  // j 1
                    { new BaseCellOrient(114, 1), new BaseCellOrient(120, 1), new BaseCellOrient(119, 0) }   // j 2
                }
            },
            {// face 19
                {
                    // i 0
                    { new BaseCellOrient(114, 0), new BaseCellOrient(112, 0), new BaseCellOrient(107, 0) },  // j 0
                    { new BaseCellOrient(100, 0), new BaseCellOrient(102, 0), new BaseCellOrient(101, 3) },  // j 1
                    { new BaseCellOrient(83, 1), new BaseCellOrient(87, 3), new BaseCellOrient(85, 3) }      // j 2
                },
                {
                    // i 1
                    { new BaseCellOrient(118, 0), new BaseCellOrient(120, 0), new BaseCellOrient(115, 5) },  // j 0
                    { new BaseCellOrient(108, 1), new BaseCellOrient(114, 0), new BaseCellOrient(112, 0) },  // j 1
                    { new BaseCellOrient(92, 1), new BaseCellOrient(100, 0), new BaseCellOrient(102, 0) }    // j 2
                },
                {
                    // i 2
                    { new BaseCellOrient(117, 0), new BaseCellOrient(121, 5), new BaseCellOrient(119, 5) },  // j 0
                    { new BaseCellOrient(109, 1), new BaseCellOrient(118, 0), new BaseCellOrient(120, 0) },  // j 1
                    { new BaseCellOrient(95, 1), new BaseCellOrient(108, 1), new BaseCellOrient(114, 0) }    // j 2
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
        /// <!-- Based off 3.1.1 -->
        public static readonly BaseCellData[] baseCellData = {
            new BaseCellData(1, 1 ,0 ,0, 0, 0, 0), // base cell 0
            new BaseCellData(2, 1, 1, 0, 0, 0, 0),	// base cell 1
            new BaseCellData(1, 0, 0, 0, 0, 0, 0),	// base cell 2
            new BaseCellData(2, 1, 0, 0, 0, 0, 0),	// base cell 3
            new BaseCellData(0, 2, 0, 0, 1, -1, -1),	// base cell 4
            new BaseCellData(1, 1, 1, 0, 0, 0, 0),	// base cell 5
            new BaseCellData(1, 0, 0, 1, 0, 0, 0),	// base cell 6
            new BaseCellData(2, 0, 0, 0, 0, 0, 0),	// base cell 7
            new BaseCellData(0, 1, 0, 0, 0, 0, 0),	// base cell 8
            new BaseCellData(2, 0, 1, 0, 0, 0, 0),	// base cell 9
            new BaseCellData(1, 0, 1, 0, 0, 0, 0),	// base cell 10
            new BaseCellData(1, 0, 1, 1, 0, 0, 0),	// base cell 11
            new BaseCellData(3, 1, 0, 0, 0, 0, 0),	// base cell 12
            new BaseCellData(3, 1, 1, 0, 0, 0, 0),	// base cell 13
            new BaseCellData(11, 2, 0, 0, 1, 2, 6),	// base cell 14
            new BaseCellData(4, 1, 0, 0, 0, 0, 0),	// base cell 15
            new BaseCellData(0, 0, 0, 0, 0, 0, 0),	// base cell 16
            new BaseCellData(6, 0, 1, 0, 0, 0, 0),	// base cell 17
            new BaseCellData(0, 0, 0, 1, 0, 0, 0),	// base cell 18
            new BaseCellData(2, 0, 1, 1, 0, 0, 0),	// base cell 19
            new BaseCellData(7, 0, 0, 1, 0, 0, 0),	// base cell 20
            new BaseCellData(2, 0, 0, 1, 0, 0, 0),	// base cell 21
            new BaseCellData(0, 1, 1, 0, 0, 0, 0),	// base cell 22
            new BaseCellData(6, 0, 0, 1, 0, 0, 0),	// base cell 23
            new BaseCellData(10, 2, 0, 0, 1, 1, 5),	// base cell 24
            new BaseCellData(6, 0, 0, 0, 0, 0, 0),	// base cell 25
            new BaseCellData(3, 0, 0, 0, 0, 0, 0),	// base cell 26
            new BaseCellData(11, 1, 0, 0, 0, 0, 0),	// base cell 27
            new BaseCellData(4, 1, 1, 0, 0, 0, 0),	// base cell 28
            new BaseCellData(3, 0, 1, 0, 0, 0, 0),	// base cell 29
            new BaseCellData(0, 0, 1, 1, 0, 0, 0),	// base cell 30
            new BaseCellData(4, 0, 0, 0, 0, 0, 0),	// base cell 31
            new BaseCellData(5, 0, 1, 0, 0, 0, 0),	// base cell 32
            new BaseCellData(0, 0, 1, 0, 0, 0, 0),	// base cell 33
            new BaseCellData(7, 0, 1, 0, 0, 0, 0),	// base cell 34
            new BaseCellData(11, 1, 1, 0, 0, 0, 0),	// base cell 35
            new BaseCellData(7, 0, 0, 0, 0, 0, 0),	// base cell 36
            new BaseCellData(10, 1, 0, 0, 0, 0, 0),	// base cell 37
            new BaseCellData(12, 2, 0, 0, 1, 3, 7),	// base cell 38
            new BaseCellData(6, 1, 0, 1, 0, 0, 0),	// base cell 39
            new BaseCellData(7, 1, 0, 1, 0, 0, 0),	// base cell 40
            new BaseCellData(4, 0, 0, 1, 0, 0, 0),	// base cell 41
            new BaseCellData(3, 0, 0, 1, 0, 0, 0),	// base cell 42
            new BaseCellData(3, 0, 1, 1, 0, 0, 0),	// base cell 43
            new BaseCellData(4, 0, 1, 0, 0, 0, 0),	// base cell 44
            new BaseCellData(6, 1, 0, 0, 0, 0, 0),	// base cell 45
            new BaseCellData(11, 0, 0, 0, 0, 0, 0),	// base cell 46
            new BaseCellData(8, 0, 0, 1, 0, 0, 0),	// base cell 47
            new BaseCellData(5, 0, 0, 1, 0, 0, 0),	// base cell 48
            new BaseCellData(14, 2, 0, 0, 1, 0, 9),	// base cell 49
            new BaseCellData(5, 0, 0, 0, 0, 0, 0),	// base cell 50
            new BaseCellData(12, 1, 0, 0, 0, 0, 0),	// base cell 51
            new BaseCellData(10, 1, 1, 0, 0, 0, 0),	// base cell 52
            new BaseCellData(4, 0, 1, 1, 0, 0, 0),	// base cell 53
            new BaseCellData(12, 1, 1, 0, 0, 0, 0),	// base cell 54
            new BaseCellData(7, 1, 0, 0, 0, 0, 0),	// base cell 55
            new BaseCellData(11, 0, 1, 0, 0, 0, 0),	// base cell 56
            new BaseCellData(10, 0, 0, 0, 0, 0, 0),	// base cell 57
            new BaseCellData(13, 2, 0, 0, 1, 4, 8),	// base cell 58
            new BaseCellData(10, 0, 0, 1, 0, 0, 0),	// base cell 59
            new BaseCellData(11, 0, 0, 1, 0, 0, 0),	// base cell 60
            new BaseCellData(9, 0, 1, 0, 0, 0, 0),	// base cell 61
            new BaseCellData(8, 0, 1, 0, 0, 0, 0),	// base cell 62
            new BaseCellData(6, 2, 0, 0, 1, 11, 15),	// base cell 63
            new BaseCellData(8, 0, 0, 0, 0, 0, 0),	// base cell 64
            new BaseCellData(9, 0, 0, 1, 0, 0, 0),	// base cell 65
            new BaseCellData(14, 1, 0, 0, 0, 0, 0),	// base cell 66
            new BaseCellData(5, 1, 0, 1, 0, 0, 0),	// base cell 67
            new BaseCellData(16, 0, 1, 1, 0, 0, 0),	// base cell 68
            new BaseCellData(8, 1, 0, 1, 0, 0, 0),	// base cell 69
            new BaseCellData(5, 1, 0, 0, 0, 0, 0),	// base cell 70
            new BaseCellData(12, 0, 0, 0, 0, 0, 0),	// base cell 71
            new BaseCellData(7, 2, 0, 0, 1, 12, 16),	// base cell 72
            new BaseCellData(12, 0, 1, 0, 0, 0, 0),	// base cell 73
            new BaseCellData(10, 0, 1, 0, 0, 0, 0),	// base cell 74
            new BaseCellData(9, 0, 0, 0, 0, 0, 0),	// base cell 75
            new BaseCellData(13, 1, 0, 0, 0, 0, 0),	// base cell 76
            new BaseCellData(16, 0, 0, 1, 0, 0, 0),	// base cell 77
            new BaseCellData(15, 0, 1, 1, 0, 0, 0),	// base cell 78
            new BaseCellData(15, 0, 1, 0, 0, 0, 0),	// base cell 79
            new BaseCellData(16, 0, 1, 0, 0, 0, 0),	// base cell 80
            new BaseCellData(14, 1, 1, 0, 0, 0, 0),	// base cell 81
            new BaseCellData(13, 1, 1, 0, 0, 0, 0),	// base cell 82
            new BaseCellData(5, 2, 0, 0, 1, 10, 19),	// base cell 83
            new BaseCellData(8, 1, 0, 0, 0, 0, 0),	// base cell 84
            new BaseCellData(14, 0, 0, 0, 0, 0, 0),	// base cell 85
            new BaseCellData(9, 1, 0, 1, 0, 0, 0),	// base cell 86
            new BaseCellData(14, 0, 0, 1, 0, 0, 0),	// base cell 87
            new BaseCellData(17, 0, 0, 1, 0, 0, 0),	// base cell 88
            new BaseCellData(12, 0, 0, 1, 0, 0, 0),	// base cell 89
            new BaseCellData(16, 0, 0, 0, 0, 0, 0),	// base cell 90
            new BaseCellData(17, 0, 1, 1, 0, 0, 0),	// base cell 91
            new BaseCellData(15, 0, 0, 1, 0, 0, 0),	// base cell 92
            new BaseCellData(16, 1, 0, 1, 0, 0, 0),	// base cell 93
            new BaseCellData(9, 1, 0, 0, 0, 0, 0),	// base cell 94
            new BaseCellData(15, 0, 0, 0, 0, 0, 0),	// base cell 95
            new BaseCellData(13, 0, 0, 0, 0, 0, 0),	// base cell 96
            new BaseCellData(8, 2, 0, 0, 1, 13, 17),	// base cell 97
            new BaseCellData(13, 0, 1, 0, 0, 0, 0),	// base cell 98
            new BaseCellData(17, 1, 0, 1, 0, 0, 0),	// base cell 99
            new BaseCellData(19, 0, 1, 0, 0, 0, 0),	// base cell 100
            new BaseCellData(14, 0, 1, 0, 0, 0, 0),	// base cell 101
            new BaseCellData(19, 0, 1, 1, 0, 0, 0),	// base cell 102
            new BaseCellData(17, 0, 1, 0, 0, 0, 0),	// base cell 103
            new BaseCellData(13, 0, 0, 1, 0, 0, 0),	// base cell 104
            new BaseCellData(17, 0, 0, 0, 0, 0, 0),	// base cell 105
            new BaseCellData(16, 1, 0, 0, 0, 0, 0),	// base cell 106
            new BaseCellData(9, 2, 0, 0, 1, 14, 18),	// base cell 107
            new BaseCellData(15, 1, 0, 1, 0, 0, 0),	// base cell 108
            new BaseCellData(15, 1, 0, 0, 0, 0, 0),	// base cell 109
            new BaseCellData(18, 0, 1, 1, 0, 0, 0),	// base cell 110
            new BaseCellData(18, 0, 0, 1, 0, 0, 0),	// base cell 111
            new BaseCellData(19, 0, 0, 1, 0, 0, 0),	// base cell 112
            new BaseCellData(17, 1, 0, 0, 0, 0, 0),	// base cell 113
            new BaseCellData(19, 0, 0, 0, 0, 0, 0),	// base cell 114
            new BaseCellData(18, 0, 1, 0, 0, 0, 0),	// base cell 115
            new BaseCellData(18, 1, 0, 1, 0, 0, 0),	// base cell 116
            new BaseCellData(19, 2, 0, 0, 1, -1, -1),	// base cell 117
            new BaseCellData(19, 1, 0, 0, 0, 0, 0),	// base cell 118
            new BaseCellData(18, 0, 0, 0, 0, 0, 0),	// base cell 119
            new BaseCellData(19, 1, 0, 1, 0, 0, 0),	// base cell 120
            new BaseCellData(18, 1, 0, 0, 0, 0, 0)     // base cell 121
        };

        public const int INVALID_BASE_CELL = 127;
        /// <summary>
        /// Maximum input for any component to face-to-base-cell lookup functions
        /// </summary>
        public const int MAX_FACE_COORD = 2;

        /// <summary>
        /// Invalid number of rotations
        /// </summary>
        public const int INVALID_ROTATIONS = -1;
        
        /// <summary>
        /// Return whether or not the indicated base cell is a pentagon.
        /// </summary>
        public static bool _isBaseCellPentagon(int baseCell)
        {
            return baseCellData[baseCell].isPentagon == 1;
        }

        /// <summary>
        /// Return whether the indicated base cell is a pentagon where all
        /// neighbors are oriented towards it.
        /// </summary>
        /// <!-- Based off 3.2.0 -->
        public static bool _isBaseCellPolarPentagon(int baseCell) {
            return baseCell == 4 || baseCell == 117;
        }
        /// <summary>
        /// Find base cell given FaceIJK.
        ///
        /// Given the face number and a resolution 0 ijk+ coordinate in that face's
        /// face-centered ijk coordinate system, return the base cell located at that
        /// coordinate.
        /// </summary>
        /// <!-- Based off 3.1.1 -->
        public static int _faceIjkToBaseCell(FaceIjk h)
        {
            return faceIjkBaseCells[h.Face,h.Coord.I,h.Coord.J,h.Coord.K].baseCell;
        }

        /// <summary>
        /// Find base cell given FaceIJK.
        ///
        /// Given the face number and a resolution 0 ijk+ coordinate in that face's
        /// face-centered ijk coordinate system, return the number of 60' ccw rotations
        /// to rotate into the coordinate system of the base cell at that coordinates.
        ///
        /// Valid ijk+ lookup coordinates are from (0, 0, 0) to (2, 2, 2).
        /// </summary>
        /// <!-- Based off 3.1.1 -->
        public static int _faceIjkToBaseCellCCWrot60( FaceIjk h)
        {
            return faceIjkBaseCells[h.Face, h.Coord.I, h.Coord.J, h.Coord.K].ccwRot60;
        }

        /// <summary>
        /// Find the FaceIJK given a base cell.
        /// </summary>
        /// <!-- Based off 3.1.1 -->
        // ReSharper disable once UnusedMember.Global
        public static void _baseCellToFaceIjk(int baseCell, ref FaceIjk h)
        {
            h = baseCellData[baseCell].homeFijk;
        }

        public static int _baseCellToCCWrot60(int baseCell, int face)
        {
            if (face < 0 || face > Constants.NUM_ICOSA_FACES)
            {
                return INVALID_ROTATIONS;
            }

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    for (int k = 0; k < 3; k++)
                    {
                        if (faceIjkBaseCells[face, i, j, k].baseCell == baseCell)
                        {
                            return faceIjkBaseCells[face, i, j, k].ccwRot60;
                        }
                    }
                }
            }

            return INVALID_ROTATIONS;
        }

        /// <summary>
        /// Return whether or not the tested face is a cw offset face.
        /// </summary>
        /// <!-- Based off 3.1.1 -->
        public static bool _baseCellIsCwOffset(int baseCell, int testFace)
        {
            return baseCellData[baseCell].cwOffsetPent[0] == testFace ||
                   baseCellData[baseCell].cwOffsetPent[1] == testFace;
        }

        /// <summary>
        /// Return the neighboring base cell in the given direction.
        /// </summary>
        /// <!-- Based off 3.1.1 -->
        public static int _getBaseCellNeighbor(int baseCell, Direction dir)
        {
            return baseCellNeighbors[baseCell, (int) dir];
        }

        /// <summary>
        /// Return the direction from the origin base cell to the neighbor.
        /// </summary>
        /// <returns>INVALID_DIGIT if the base cells are not neighbors.</returns>
        /// <!-- Based off 3.1.1 -->
        public static Direction _getBaseCellDirection(int originBaseCell, int neighboringBaseCell)
        {
            for (var dir = Direction.CENTER_DIGIT; dir <Direction.NUM_DIGITS; dir++) 
            {
                var testBaseCell = _getBaseCellNeighbor(originBaseCell, dir);
                if (testBaseCell == neighboringBaseCell)
                {
                    return dir;
                }
            }
            return Direction.INVALID_DIGIT;
        }
    }

}
