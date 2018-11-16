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
using System;

namespace H3Net.Code
{
    /// <summary>
    /// 3D floating point structure
    /// </summary>
    /// <!-- Based off 3.1.1 -->
    public class Vec3d
    {
        public double x;
        public double y;
        public double z;

        public Vec3d(double _x, double _y, double _z)
        {
            x = _x;
            y = _y;
            z = _z;
        }

        public Vec3d()
        {
        }

        /// <summary>
        /// Square of a number
        /// </summary>
        /// <param name="x">The input number</param>
        /// <returns>The square of the input number</returns>
        /// <!-- Based off 3.1.1 -->
        internal static double _square(double x)
        {
            return x * x;
        }

        /// <summary>
        /// Calculate the square of the distance between two 3D coordinates.
        /// </summary>
        /// <param name="v1">The first 3D Coordinate</param>
        /// <param name="v1">The second 3D Coordinate</param>
        /// <returns>The squared distance</returns>
        /// <!-- Based off 3.1.1 -->
        public static double _pointSquareDist(Vec3d v1,  Vec3d v2) {
            return _square(v1.x - v2.x) + _square(v1.y - v2.y) +
                   _square(v1.z - v2.z);
        }

        /// <summary>
        /// Calculate the 3D coordinate on unit sphere from the latitude and longitude.
        /// </summary>
        /// <param name="geo">The latitude and longitude of the point</param>
        /// <param name="v">The 3D coordinate of the point</param>
        /// <!-- Based off 3.1.1 -->
        public static  void _geoToVec3d(GeoCoord geo, ref Vec3d v)
        {
            double r = Math.Cos(geo.lat);

            v.z = Math.Sin(geo.lat);
            v.x = Math.Cos(geo.lon) * r;
            v.y = Math.Sin(geo.lon) * r;
        }
    }
}
