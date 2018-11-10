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
using System.Diagnostics;

namespace h3net.API
{
    /// <summary>
    /// 2D floating point vector functions.
    /// </summary>
    /// <!-- Based off 3.1.1 -->
    [DebuggerDisplay("X: {x} Y: {y}")]
    public class Vec2d
    {
        public double x;
        public double y;

        public Vec2d(double _x, double _y) 
        {
            x = _x;
            y = _y;
        }

        public Vec2d()
        {

        }

        /**
         * Finds the intersection between two lines. Assumes that the lines intersect
         * and that the intersection is not at an endpoint of either line.
         * @param p0 The first endpoint of the first line.
         * @param p1 The second endpoint of the first line.
         * @param p2 The first endpoint of the second line.
         * @param p3 The second endpoint of the second line.
         * @param inter The intersection point.
         */
        /// <summary>
        /// Finds the intersection between two lines. Assumes that the lines intersect
        /// and that the intersection is not at an endpoint of either line.
        /// </summary>
        /// <param name="p0">The first endpoint of the first line</param>
        /// <param name="p1">The second endpoint of the first line</param>
        /// <param name="p2">The first endpoint of the second line</param>
        /// <param name="p3">The first endpoint of the first line</param>
        /// <param name="inter">The second endpoint of the second line</param>
        public static void _v2dIntersect(Vec2d p0,  Vec2d p1,  Vec2d p2,
         Vec2d p3, ref Vec2d inter)
        {
            Vec2d s1 = new Vec2d(p1.x - p0.x, p1.y - p0.y);
            Vec2d s2 = new Vec2d(p3.x - p2.x, p3.y - p2.y);

            double t;
            t = (s2.x * (p0.y - p2.y) - s2.y * (p0.x - p2.x)) /
                (-s2.x * s1.y + s1.x * s2.y);

            inter.x = p0.x + (t * s1.x);
            inter.y = p0.y + (t * s1.y);
        }

        /// <summary>
        /// Whether two 2D vectors are equal. Does not consider possible false
        /// negatives due to floating-point errors.
        /// </summary>
        /// <param name="v1">First vector to compare</param>
        /// <param name="v2">Second vector to compare</param>
        /// <returns>Whether the vectors are equal</returns>
        public static bool _v2dEquals( Vec2d v1,  Vec2d v2) {
            return Math.Abs(v1.x - v2.x) < Constants.EPSILON && Math.Abs(v1.y - v2.y) < Constants.EPSILON;
        }

        /// <summary>Calculates the magnitude of a 2D Cartesian vector</summary>
        /// <param name="v">The 2d Cartesian vector.</param>
        /// <returns>The magnitude of the vector</returns>
        public static double _v2dMag(Vec2d v)
        {
            return Math.Sqrt(v.x * v.x + v.y * v.y);
        }
    }
}

