/*
 * Copyright 2018, Richard Vasquez
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *         http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 *
 * Original version written in C, Copyright 2016-2017 Uber Technologies, Inc.
 * C version licensed under the Apache License, Version 2.0 (the "License");
 * C Source code available at: https://github.com/uber/h3
 *
 */

using System.Diagnostics.CodeAnalysis;

namespace H3Net.Code
{
    /// <summary>
    /// Collection of constants used throughout the library.
    /// </summary>
    /// <!-- Based off 3.1.1 -->
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [SuppressMessage("ReSharper", "IdentifierTypo")]
    public static class Constants
    {
        /// <summary>
        /// The following are taken from https://github.com/uber/h3/issues/148
        /// to indicate the functional equivalence of this C# library.
        /// </summary>
        public const int H3_VERSION_MAJOR = 3;
        public const int H3_VERSION_MINOR = 2;
        public const int H3_VERSION_PATCH = 0;

        /// <summary>
        /// Pi
        /// </summary>
        public const double M_PI = 3.14159265358979323846;
        /// <summary>
        /// Pi / 2.0
        /// </summary>
        public const double M_PI_2 = 1.5707963267948966;
        /// <summary>
        /// Pi * 2.0
        /// </summary>
        public const double M_2PI = 6.28318530717958647692528676655900576839433;
        /// <summary>
        /// Pi / 180
        /// </summary>
        public const double M_PI_180 = 0.0174532925199432957692369076848861271111;
        /// <summary>
        /// Pi * 180
        /// </summary>
        /// <!-- BUT IT'S NOT?!?!?! -->
        public const double M_180_PI = 57.29577951308232087679815481410517033240547;
        /// <summary>
        /// Threshold epsilon
        /// </summary>
        public const double EPSILON = 0.0000000001;
        /// <summary>
        /// Sqrt(3) / 2.0
        /// </summary>
        public const double M_SQRT3_2 = 0.8660254037844386467637231707529361834714;
        /// <summary>
        /// sin(60 degrees)
        /// </summary>
        public const double M_SIN60 = M_SQRT3_2;
        /// <summary>
        /// Rotation angle between Class II and Class III resolution axes
        /// asin(sqrt(3.0 / 28.0 ))
        /// </summary>
        public const double M_AP7_ROT_RADS = 0.333473172251832115336090755351601070065900389;
        /// <summary>
        /// sin(<see cref="M_AP7_ROT_RADS"/>
        /// </summary>
        public const double M_SIN_AP7_ROT = 0.3273268353539885718950318;
        /// <summary>
        /// cos(<see cref="M_AP7_ROT_RADS"/>
        /// </summary>
        public const double M_COS_AP7_ROT = 0.9449111825230680680167902;
        /// <summary>
        /// Earth radius in kilometers using WGS84 authalic radius
        /// </summary>
        public const double EARTH_RADIUS_KM = 6371.007180918475;
        /// <summary>
        /// Scaling factor from hex2d resolution 0 unit length
        /// (or distance between adjacent cell center points on the place)
        /// to gnomonic unit length.
        /// </summary>
        public const double RES0_U_GNOMONIC = 0.38196601125010500003;
        /// <summary>
        /// H3 resolution; H3 version 1 has 16 resolutions, numbered 0 through 15
        /// </summary>
        public const int MAX_H3_RES = 15;
        /// <summary>
        /// The number of faces on an icosahedron
        /// </summary>
        public const int NUM_ICOSA_FACES = 20;
        /// <summary>
        /// The number of H3 base cells
        /// </summary>
        public const int NUM_BASE_CELLS = 122;
        /// <summary>
        /// The number of vertices in a hexagon;
        /// </summary>
        public const int NUM_HEX_VERTS = 6;
        /// <summary>
        /// The number of vertices in a pentagon
        /// </summary>
        public const int NUM_PENT_VERTS = 5;
        /// <summary>
        /// H3 Index modes
        /// </summary>
        public const int H3_HEXAGON_MODE = 1;
        public const int H3_UNIEDGE_MODE = 2;
        /// <summary>
        /// epsilon of ~0.1mm in degrees
        /// </summary>
        public const double EPSILON_DEG = 0.000000001;
        /// <summary>
        /// epsilon of ~0.1mm in radians
        /// </summary>
        public const double EPSILON_RAD = EPSILON_DEG * M_PI_180;
        public const int MAX_CELL_BNDRY_VERTS = 10;

        /// <summary>
        /// Return codes from <see cref="Algos.hexRange"/> and related functions.
        /// </summary>
        public const int HEX_RANGE_SUCCESS = 0;
        public const int HEX_RANGE_PENTAGON = 1;
        public const int HEX_RANGE_K_SUBSEQUENCE = 1;

        public const double DBL_EPSILON = 2.2204460492503131e-16;

        /// <summary>
        /// Direction used for traversing to the next outward hexagonal ring. 
        /// </summary>
        public const Direction NEXT_RING_DIRECTION = Direction.I_AXES_DIGIT;
    }
}

