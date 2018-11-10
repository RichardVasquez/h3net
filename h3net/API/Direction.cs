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

namespace h3net.API
{
    /// <summary>
    /// H3 digit representing ijk+ axes direction.
    /// Values will be within the lowest 3 bits of an integer.
    /// </summary>
    /// <!-- Based off 3.1.1 -->
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public enum Direction
    {
        /// <summary>
        /// H3 digit in center
        /// </summary>
        CENTER_DIGIT = 0,

        /// <summary>
        /// H3 digit in k-axes direction
        /// </summary>
        K_AXES_DIGIT = 1,

        /// <summary>
        /// H3 digit in j-axes direction
        /// </summary>
        J_AXES_DIGIT = 2,

        /// <summary>
        /// H3 digit in j==k direction
        /// </summary>
        JK_AXES_DIGIT = J_AXES_DIGIT | K_AXES_DIGIT,

        /// <summary>
        /// H3 digit in i-axes direction
        /// </summary>
        I_AXES_DIGIT = 4,

        /// <summary>
        /// H3 digit in i==k direction
        /// </summary>
        IK_AXES_DIGIT = I_AXES_DIGIT | K_AXES_DIGIT,

        /// <summary>
        /// H3 digit in i==j direction
        /// </summary>
        IJ_AXES_DIGIT = I_AXES_DIGIT | J_AXES_DIGIT,

        /// <summary>
        /// H3 digit in the invalid direction
        /// </summary>
        INVALID_DIGIT = 7,

        /// <summary>
        /// Valid digits will be less than this value. Same value as <see cref="INVALID_DIGIT"/>
        /// </summary>
        NUM_DIGITS = INVALID_DIGIT
    }
}
