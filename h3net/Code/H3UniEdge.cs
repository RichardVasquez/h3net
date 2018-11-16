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
using System.Collections.Generic;
using System.Linq;

namespace H3Net.Code
{
    /// <summary>
    /// H3UniEdge functions for manipulating unidirectional edge indexes.
    /// </summary>
    /// <!-- Based off 3.1.1 -->
    public class H3UniEdge
    {
        /// <summary>
        /// Returns whether or not the provided H3Indexes are neighbors.
        /// </summary>
        /// <param name="origin">The origin H3 index</param>
        /// <param name="destination">The destination H3 index</param>
        /// <returns>1 if the indexes are neighbors, 0 otherwise</returns>
        /// <!-- Based off 3.1.1 -->
        public static int h3IndexesAreNeighbors(H3Index origin, H3Index destination)
        {
            // Make sure they're hexagon indexes
            if (H3Index.H3_GET_MODE(ref origin) != Constants.H3_HEXAGON_MODE ||
                H3Index.H3_GET_MODE(ref destination) != Constants.H3_HEXAGON_MODE)
            {
                return 0;
            }

            // Hexagons cannot be neighbors with themselves
            if (origin == destination)
            {
                return 0;
            }

            // Only hexagons in the same resolution can be neighbors
            if (H3Index.H3_GET_RESOLUTION(origin) != H3Index.H3_GET_RESOLUTION(destination))
            {
                return 0;
            }

            // H3 Indexes that share the same parent are very likely to be neighbors
            // Child 0 is neighbor with all of its parent's 'offspring', the other
            // children are neighbors with 3 of the 7 children. So a simple comparison
            // of origin and destination parents and then a lookup table of the children
            // is a super-cheap way to possibly determine they are neighbors.
            int parentRes = H3Index.H3_GET_RESOLUTION(origin) - 1;
            if (parentRes > 0 && (H3Index.h3ToParent(origin, parentRes) ==
                                  H3Index.h3ToParent(destination, parentRes)))
            {
                Direction originResDigit = H3Index.H3_GET_INDEX_DIGIT(origin, parentRes + 1);
                Direction destinationResDigit =
                    H3Index.H3_GET_INDEX_DIGIT(destination, parentRes + 1);
                if (originResDigit == Direction.CENTER_DIGIT ||
                    destinationResDigit == Direction.CENTER_DIGIT)
                {
                    return 1;
                }

                // These sets are the relevant neighbors in the clockwise
                // and counter-clockwise
                Direction[] neighborSetClockwise =
                {
                    Direction.CENTER_DIGIT, Direction.JK_AXES_DIGIT, Direction.IJ_AXES_DIGIT,
                    Direction.J_AXES_DIGIT, Direction.IK_AXES_DIGIT, Direction.K_AXES_DIGIT,
                    Direction.I_AXES_DIGIT
                };
                Direction[] neighborSetCounterclockwise =
                {
                    Direction.CENTER_DIGIT, Direction.IK_AXES_DIGIT, Direction.JK_AXES_DIGIT,
                    Direction.K_AXES_DIGIT, Direction.IJ_AXES_DIGIT, Direction.I_AXES_DIGIT,
                    Direction.J_AXES_DIGIT
                };
                if (
                    neighborSetClockwise[(int) originResDigit] == destinationResDigit ||
                    neighborSetCounterclockwise[(int) originResDigit] == destinationResDigit)
                {
                    return 1;
                }
            }

            // Otherwise, we have to determine the neighbor relationship the "hard" way.
            var neighborRing = new ulong[7].Select(cell => new H3Index(cell)).ToList();
            Algos.kRing(origin, 1, ref neighborRing);
            for (int i = 0; i < 7; i++)
            {
                if (neighborRing[i] == destination)
                {
                    return 1;
                }
            }

            // Made it here, they definitely aren't neighbors
            return 0;
        }

        /// <summary>
        /// Returns a unidirectional edge H3 index based on the provided origin and
        /// destination
        /// </summary>
        /// <param name="origin">The origin H3 hexagon index</param>
        /// <param name="destination">The destination H3 hexagon index</param>
        /// <returns>The unidirectional edge H3Index, or 0 on failure.</returns>
        /// <!-- Based off 3.1.1 -->
        public static H3Index getH3UnidirectionalEdge(H3Index origin, H3Index destination)
        {
            // Short-circuit and return an invalid index value if they are not neighbors
            if (h3IndexesAreNeighbors(origin, destination) == 0)
            {
                return H3Index.H3_INVALID_INDEX;
            }

            // Otherwise, determine the IJK direction from the origin to the destination
            H3Index output = origin;
            H3Index.H3_SET_MODE(ref output, Constants.H3_UNIEDGE_MODE);

            // Checks each neighbor, in order, to determine which direction the
            // destination neighbor is located. Skips CENTER_DIGIT since that
            // would be this index.
            for (var direction = Direction.K_AXES_DIGIT;
                 direction < Direction.NUM_DIGITS;
                 direction++)
            {
                int rotations = 0;
                H3Index neighbor = Algos.h3NeighborRotations(origin, direction, ref rotations);
                if (neighbor == destination)
                {
                    H3Index.H3_SET_RESERVED_BITS(ref output, (ulong) direction);
                    return output;
                }
            }

            // This should be impossible, return an invalid H3Index in this case;
            return H3Index.H3_INVALID_INDEX; // LCOV_EXCL_LINE
        }

        /// <summary>
        /// Returns the origin hexagon from the unidirectional edge H3Index
        /// </summary>
        /// <param name="edge">The edge H3 index</param>
        /// <returns>The origin H3 hexagon index</returns>
        /// <!-- Based off 3.1.1 -->
        public static H3Index getOriginH3IndexFromUnidirectionalEdge(H3Index edge)
        {
            if (H3Index.H3_GET_MODE(ref edge) != Constants.H3_UNIEDGE_MODE) {
                return H3Index.H3_INVALID_INDEX;
            }
            H3Index origin = edge.value;
            H3Index.H3_SET_MODE(ref origin, Constants.H3_HEXAGON_MODE);
            H3Index.H3_SET_RESERVED_BITS(ref origin, 0);
            return origin;
        }

        /// <summary>
        /// Returns the destination hexagon from the unidirectional edge H3Index
        /// </summary>
        /// <param name="edge">The edge H3 index</param>
        /// <returns>The destination H3 hexagon index</returns>
        /// <!-- Based off 3.1.1 -->
        public static H3Index getDestinationH3IndexFromUnidirectionalEdge(H3Index edge)
        {
            if (H3Index.H3_GET_MODE(ref edge) != Constants.H3_UNIEDGE_MODE)
            {
                return H3Index.H3_INVALID_INDEX;
            }
            Direction direction = (Direction) H3Index.H3_GET_RESERVED_BITS(edge);
            int rotations = 0;
            H3Index destination = Algos
               .h3NeighborRotations
                    (
                     getOriginH3IndexFromUnidirectionalEdge(edge),
                     direction,
                     ref rotations
                    );
            return destination;
        }

        /// <summary>
        /// Determines if the provided H3Index is a valid unidirectional edge index
        /// </summary>
        /// <param name="edge">The unidirectional edge H3Index</param>
        /// <returns>1 if it is a unidirectional edge H3Index, otherwise 0.</returns>
        /// <!-- Based off 3.1.1 -->
        public static int h3UnidirectionalEdgeIsValid(H3Index edge)
        {
            if (H3Index.H3_GET_MODE(ref edge) != Constants.H3_UNIEDGE_MODE)
            {
                return 0;
            }

            Direction neighborDirection = (Direction) H3Index.H3_GET_RESERVED_BITS(edge);
            if (neighborDirection <= Direction.CENTER_DIGIT ||
                neighborDirection >= Direction.NUM_DIGITS)
            {
                return 0;
            }

            H3Index origin = getOriginH3IndexFromUnidirectionalEdge(edge);
            if (H3Index.h3IsPentagon(origin) != 0 && neighborDirection == Direction.K_AXES_DIGIT)
            {
                return 0;
            }

            return H3Index.h3IsValid(origin);
        }

        /// <summary>
        /// Returns the origin, destination pair of hexagon IDs for the given edge ID
        /// </summary>
        /// <param name="edge">The unidirectional edge H3Index</param>
        /// <param name="originDestination">
        /// Pointer to memory to store origin and destination IDs
        /// </param>
        /// 
        /// <!-- Based off 3.1.1 -->
        public static void getH3IndexesFromUnidirectionalEdge(H3Index edge,
            ref List<H3Index> originDestination)
        {
            originDestination[0] =
                getOriginH3IndexFromUnidirectionalEdge(edge);
            originDestination[1] =
                getDestinationH3IndexFromUnidirectionalEdge(edge);
        }

        /// <summary>
        /// Provides all of the unidirectional edges from the current H3Index.
        /// </summary>
        /// <param name="origin">The origin hexagon H3Index to find edges for.</param>
        /// <param name="edges">The memory to store all of the edges inside.</param>
        /// <!-- Based off 3.1.1 -->
        public static void getH3UnidirectionalEdgesFromHexagon(H3Index origin,
            List<H3Index> edges) {
            // Determine if the origin is a pentagon and special treatment needed.
            int isPentagon = H3Index.h3IsPentagon(origin);

            // This is actually quite simple. Just modify the bits of the origin
            // slightly for each direction, except the 'k' direction in pentagons,
            // which is zeroed.
            for (int i = 0; i < 6; i++)
            {
                if (isPentagon!=0 && i == 0) {
                    edges[i] = H3Index.H3_INVALID_INDEX;
                } else {
                    edges[i] = origin;
                    var ei = edges[i];
                    H3Index.H3_SET_MODE(ref ei, Constants.H3_UNIEDGE_MODE);
                    H3Index.H3_SET_RESERVED_BITS(ref ei, (ulong)i + 1);
                    edges[i] = ei;
                }
            }
        }

        /// <summary>
        /// Whether the given coordinate has a matching vertex in the given geo boundary.
        /// </summary>
        /// <param name="vertex">Coordinate to check</param>
        /// <param name="boundary">Geo boundary to look in</param>
        /// <returns>Whether a match was found</returns>
        /// <!-- Based off 3.1.1 -->
        public static bool _hasMatchingVertex(GeoCoord vertex, GeoBoundary boundary)
        {
            for (int i = 0; i < boundary.numVerts; i++)
            {
                if (GeoCoord.geoAlmostEqualThreshold(vertex, boundary.verts[i], 0.000001))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Provides the coordinates defining the unidirectional edge.
        /// </summary>
        /// <param name="edge">The unidirectional edge H3Index</param>
        /// <param name="gb">
        /// The geoboundary object to store the edge coordinates.
        /// </param>
        /// <!-- Based off 3.1.1 -->
        public static void getH3UnidirectionalEdgeBoundary(H3Index edge, ref GeoBoundary gb)
        {
            // TODO: More efficient solution :)
            GeoBoundary origin = new GeoBoundary();
            GeoBoundary destination = new GeoBoundary();
            GeoCoord postponedVertex = new GeoCoord();
            bool hasPostponedVertex = false;

            H3Index.h3ToGeoBoundary(getOriginH3IndexFromUnidirectionalEdge(edge), ref origin);
            H3Index.h3ToGeoBoundary(getDestinationH3IndexFromUnidirectionalEdge(edge), ref destination);

            int k = 0;
            for (int i = 0; i < origin.numVerts; i++)
            {
                if (_hasMatchingVertex(origin.verts[i], destination))
                {
                    // If we are on vertex 0, we need to handle the case where it's the
                    // end of the edge, not the beginning.
                    if (i == 0 &&
                        !_hasMatchingVertex(origin.verts[i + 1], destination))
                    {
                        postponedVertex = origin.verts[i];
                        hasPostponedVertex = true;
                    }
                    else
                    {
                        gb.verts[k] = origin.verts[i];
                        k++;
                    }
                }
            }

            // If we postponed adding the last vertex, add it now
            if (hasPostponedVertex) {
                gb.verts[k] = postponedVertex;
                k++;
            }
            gb.numVerts = k;
        }
    }
}
