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
namespace H3Net.Code
{
    /// <summary>
    /// Math functions that should have been in math.h but aren't
    /// </summary>
    /// <!-- Based off 3.1.1 -->
    public class MathExtensions
    {
        /// <summary>
        /// _ipow does integer exponentiation efficiently. Taken from StackOverflow.
        /// </summary>
        /// <param name="nbase">the integer base</param>
        /// <param name="exp">the integer exponent</param>
        /// <!-- Based off 3.1.1 -->
        public static int _ipow(int nbase, int exp)
        {
            int result = 1;
            while (exp > 0) {
                if ((exp & 1) == 1)
                {
                    result *= nbase;
                }
                exp >>= 1;
                nbase *= nbase;
            }

            return result;
        }

    }
}
