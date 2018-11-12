using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using H3Net.Code;
using NUnit.Framework;

namespace h3tests
{
    [TestFixture]
    class TestH3UniEdge
    {
        private static GeoCoord sfGeo = new GeoCoord(0.659966917655, -2.1364398519396);
        [Test]
        public void h3IndexesAreNeighbors()
        {
            H3Index sf = H3Index.geoToH3(ref sfGeo, 9);
            List<H3Index> ring =new ulong[Algos.maxKringSize(1)].Select(cell => new H3Index(cell)).ToList();
               
            Algos.hexRing(sf, 1, ref ring);

            Assert.True
                (
                 H3UniEdge.h3IndexesAreNeighbors(sf, sf) == 0,
                 "an index does not neighbor itself"
                );

            int neighbors = 0;
            for (int i = 0; i < Algos.maxKringSize(1); i++)
            {
                if (ring[i] != 0 && H3UniEdge.h3IndexesAreNeighbors(sf, ring[i]) != 0)
                {
                    neighbors++;
                }
            }

            Assert.True
                (
                 neighbors == 6,
                 "got the expected number of neighbors from a k-ring of 1"
                );

            var largerRing = new ulong[Algos.maxKringSize(2)].Select(cell => new H3Index(cell)).ToList();
            Algos.hexRing(sf, 2, ref largerRing);

            neighbors = 0;
            for (int i = 0; i < Algos.maxKringSize(2); i++)
            {
                if (largerRing[i] != 0 &&
                    H3UniEdge.h3IndexesAreNeighbors(sf, largerRing[i]) != 0)
                {
                    neighbors++;
                }
            }

            Assert.True
                (
                 neighbors == 0,
                 "got no neighbors, as expected, from a k-ring of 2"
                );

            H3Index sfBroken = sf;
            H3Index.H3_SET_MODE(ref sfBroken, Constants.H3_UNIEDGE_MODE);
            Assert.True
                (
                 H3UniEdge.h3IndexesAreNeighbors(sf, sfBroken) == 0,
                 "broken H3Indexes can't be neighbors"
                );

            H3Index sfBigger = H3Index.geoToH3(ref sfGeo, 7);
            Assert.True
                (
                 H3UniEdge.h3IndexesAreNeighbors(sf, sfBigger) == 0,
                 "hexagons of different resolution can't be neighbors"
                );

            Assert.True
                (
                 H3UniEdge.h3IndexesAreNeighbors(ring[2], ring[1]) == 1,
                 "hexagons in a ring are neighbors"
                );
        }

        [Test]
        public void getH3UnidirectionalEdgeAndFriends()
        {
            H3Index sf = H3Index.geoToH3(ref sfGeo, 9);
            List<H3Index> ring =new ulong[Algos.maxKringSize(1)].Select(cell => new H3Index(cell)).ToList();
            
            Algos.hexRing(sf, 1, ref ring);
            H3Index sf2 = ring[0];

            H3Index edge = H3UniEdge.getH3UnidirectionalEdge(sf, sf2);
            Assert.True(sf == H3UniEdge.getOriginH3IndexFromUnidirectionalEdge(edge),
                     "can retrieve the origin from the edge");
            Assert.True(
                     sf2 == H3UniEdge.getDestinationH3IndexFromUnidirectionalEdge(edge),
                     "can retrieve the destination from the edge");

            var originDestination = new ulong[2].Select(cell => new H3Index(cell)).ToList();
            H3UniEdge.getH3IndexesFromUnidirectionalEdge(edge, ref originDestination);
            Assert.True(originDestination[0] == sf,
                     "got the origin first in the pair request");
            Assert.True(originDestination[1] == sf2,
                     "got the destination last in the pair request");

            List<H3Index> largerRing =new ulong[Algos.maxKringSize(2)].Select(cell => new H3Index(cell)).ToList();

            Algos.hexRing(sf, 2, ref largerRing);
            H3Index sf3 = largerRing[0];

            H3Index notEdge = H3UniEdge.getH3UnidirectionalEdge(sf, sf3);
            Assert.True(notEdge == 0, "Non-neighbors can't have edges");
        }

        [Test]
        public void getOriginH3IndexFromUnidirectionalEdgeBadInput()
        {
            H3Index hexagon = 0x891ea6d6533ffffL;

            Assert.True(H3UniEdge.getOriginH3IndexFromUnidirectionalEdge(hexagon) == 0,
                     "getting the origin from a hexagon index returns 0");
            Assert.True(H3UniEdge.getOriginH3IndexFromUnidirectionalEdge(0) == 0,
                     "getting the origin from a null index returns 0");
        }

        [Test]
        public void getDestinationH3IndexFromUnidirectionalEdge()
        {
            H3Index hexagon = 0x891ea6d6533ffffL;

            Assert.True(
                     H3UniEdge.getDestinationH3IndexFromUnidirectionalEdge(hexagon) == 0,
                     "getting the destination from a hexagon index returns 0");
            Assert.True(H3UniEdge.getDestinationH3IndexFromUnidirectionalEdge(0) == 0,
                     "getting the destination from a null index returns 0");
        }

        [Test]
        public void getH3UnidirectionalEdgeFromPentagon()
        {
            H3Index pentagon = new H3Index();
            H3Index.setH3Index(ref pentagon, 0, 4, 0);
            H3Index adjacent = new H3Index();
            H3Index.setH3Index(ref adjacent, 0, 8, 0);

            H3Index edge = H3UniEdge.getH3UnidirectionalEdge(pentagon, adjacent);
            Assert.True(edge != 0, "Produces a valid edge");
        }

        [Test]
        public void h3UnidirectionalEdgeIsValid()
        {
            H3Index sf = H3Index.geoToH3(ref sfGeo, 9);
            var ring = new ulong[Algos.maxKringSize(1)].Select(cell => new H3Index(cell)).ToList();
            Algos.hexRing(sf, 1, ref ring);
            H3Index sf2 = ring[0];

            H3Index edge = H3UniEdge.getH3UnidirectionalEdge(sf, sf2);
            Assert.True(H3UniEdge.h3UnidirectionalEdgeIsValid(edge) == 1,
                     "edges validate correctly");
            Assert.True(H3UniEdge.h3UnidirectionalEdgeIsValid(sf) == 0,
                     "hexagons do not validate");

            H3Index fakeEdge = sf;
            H3Index.H3_SET_MODE(ref fakeEdge, Constants.H3_UNIEDGE_MODE);
            Assert.True(H3UniEdge.h3UnidirectionalEdgeIsValid(fakeEdge) == 0,
                     "edges without an edge specified don't work");

            H3Index pentagon = 0x821c07fffffffff;
            H3Index goodPentagonalEdge = pentagon;
            H3Index.H3_SET_MODE(ref goodPentagonalEdge, Constants.H3_UNIEDGE_MODE);
            H3Index.H3_SET_RESERVED_BITS(ref goodPentagonalEdge, 2);
            Assert.True(H3UniEdge.h3UnidirectionalEdgeIsValid(goodPentagonalEdge) == 1,
                     "pentagonal edge validates");

            H3Index badPentagonalEdge = goodPentagonalEdge;
            H3Index.H3_SET_RESERVED_BITS(ref badPentagonalEdge, 1);
            Assert.True(H3UniEdge.h3UnidirectionalEdgeIsValid(badPentagonalEdge) == 0,
                     "missing pentagonal edge does not validate");
        }

        [Test]
        public void getH3UnidirectionalEdgesFromHexagon()
        {
            H3Index sf = H3Index.geoToH3(ref sfGeo, 9);
            var edges = new ulong[6].Select(cell => new H3Index(cell)).ToList();

            H3UniEdge.getH3UnidirectionalEdgesFromHexagon(sf, edges);

            for (int i = 0; i < 6; i++)
            {
                Assert.True
                    (
                     H3UniEdge.h3UnidirectionalEdgeIsValid(edges[i]) == 1,
                     "edge is an edge"
                    );
                Assert.True
                    (
                     sf == H3UniEdge.getOriginH3IndexFromUnidirectionalEdge(edges[i]),
                     "origin is correct"
                    );
                Assert.True
                    (
                     sf != H3UniEdge.getDestinationH3IndexFromUnidirectionalEdge(edges[i]),
                     "destination is not origin"
                    );
            }
        }

        [Test]
        public void getH3UnidirectionalEdgesFromPentagon()
        {
            H3Index pentagon = 0x821c07fffffffff;
            var edges = new ulong[6].Select(cell => new H3Index(cell)).ToList();
            H3UniEdge.getH3UnidirectionalEdgesFromHexagon(pentagon, edges);

            int missingEdgeCount = 0;
            for (int i = 0; i < 6; i++)
            {
                if (edges[i] == 0)
                {
                    missingEdgeCount++;
                }
                else
                {
                    Assert.True
                        (
                         H3UniEdge.h3UnidirectionalEdgeIsValid(edges[i]) == 1,
                         "edge is an edge"
                        );
                    Assert.True
                        (
                         pentagon ==
                         H3UniEdge.getOriginH3IndexFromUnidirectionalEdge(edges[i]),
                         "origin is correct"
                        );
                    Assert.True
                        (
                         pentagon !=
                         H3UniEdge.getDestinationH3IndexFromUnidirectionalEdge(edges[i]),
                         "destination is not origin"
                        );
                }
            }

            Assert.True
                (
                 missingEdgeCount == 1,
                 "Only one edge was deleted for the pentagon"
                );
        }

        [Test]
        public void getH3UnidirectionalEdgeBoundary()
        {
            H3Index sf = 0;
            GeoBoundary boundary = new GeoBoundary();
            GeoBoundary edgeBoundary = new GeoBoundary();
            var edges = new ulong[6].Select(cell => new H3Index(cell)).ToList();

            int[,] expectedVertices =
            {
                {3, 4}, {1, 2}, {2, 3},
                {5, 0}, {4, 5}, {0, 1}
            };

            // TODO: The current implementation relies on lat/lon comparison and fails
            // on resolutions finer than 12
            for (int res = 0; res < 13; res++) {
                sf = H3Index.geoToH3(ref sfGeo, res);
                H3Index.h3ToGeoBoundary(sf, ref boundary);
                H3UniEdge.getH3UnidirectionalEdgesFromHexagon(sf, edges);

                for (int i = 0; i < 6; i++) {
                    H3UniEdge.getH3UnidirectionalEdgeBoundary(edges[i], ref edgeBoundary);
                    Assert.True(edgeBoundary.numVerts == 2,
                             "Got the expected number of vertices back");
                    for (int j = 0; j < edgeBoundary.numVerts; j++)
                    {
                        Assert.True(
                                 GeoCoord.geoAlmostEqual(edgeBoundary.verts[j],
                                                boundary.verts[expectedVertices[i,j]]),
                                 "Got expected vertex");
                    }
                }
            }
        }


        [Test]
        public void getH3UnidirectionalEdgeBoundaryPentagonClassIII()
        {
            H3Index pentagon = 0;
            GeoBoundary boundary = new GeoBoundary();
            GeoBoundary edgeBoundary = new GeoBoundary();
            var edges = new ulong[6].Select(cell => new H3Index(cell)).ToList();

            int[,] expectedVertices =
            {
                {-1, -1, -1}, {2, 3, 4},
                { 4,  5,  6}, {8, 9, 0},
                { 6,  7,  8}, {0, 1, 2}
            };

            // TODO: The current implementation relies on lat/lon comparison and fails
            // on resolutions finer than 12
            for (int res = 1; res < 13; res += 2)
            {
                H3Index.setH3Index(ref pentagon, res, 24, 0);
                H3Index.h3ToGeoBoundary(pentagon, ref boundary);
                H3UniEdge.getH3UnidirectionalEdgesFromHexagon(pentagon, edges);

                int missingEdgeCount = 0;
                for (int i = 0; i < 6; i++)
                {
                    if (edges[i] == 0)
                    {
                        missingEdgeCount++;
                    }
                    else
                    {
                        H3UniEdge.getH3UnidirectionalEdgeBoundary(edges[i], ref edgeBoundary);
                        Assert.True
                            (edgeBoundary.numVerts == 3,
                             "Got the expected number of vertices back for a Class III pentagon"
                            );
                        for (int j = 0; j < edgeBoundary.numVerts; j++)
                        {
                            Assert.True
                                (
                                 GeoCoord.geoAlmostEqual(edgeBoundary.verts[j], boundary.verts[expectedVertices[i, j]]),
                                 "Got expected vertex"
                                );
                        }
                    }
                }

                Assert.True
                    (missingEdgeCount == 1,
                     "Only one edge was deleted for the pentagon"
                    );
            }
        }

        [Test]
        public void getH3UnidirectionalEdgeBoundaryPentagonClassII()
        {
            H3Index pentagon = 0;
            GeoBoundary boundary = new GeoBoundary();
            GeoBoundary edgeBoundary = new GeoBoundary();
            var edges = new ulong[6].Select(cell => new H3Index(cell)).ToList();

            int[,] expectedVertices =
            {
                {-1, -1}, {1, 2}, {2, 3},
                { 4,  0}, {3, 4}, {0, 1}
            };

            // TODO: The current implementation relies on lat/lon comparison and fails
            // on resolutions finer than 12
            for (int res = 0; res < 12; res += 2)
            {
                H3Index.setH3Index(ref pentagon, res, 24, 0);
                H3Index.h3ToGeoBoundary(pentagon, ref boundary);
                H3UniEdge.getH3UnidirectionalEdgesFromHexagon(pentagon, edges);

                int missingEdgeCount = 0;
                for (int i = 0; i < 6; i++) {
                    if (edges[i] == 0) {
                        missingEdgeCount++;
                    } else {
                        H3UniEdge.getH3UnidirectionalEdgeBoundary(edges[i], ref edgeBoundary);
                        Assert.True(
                                 edgeBoundary.numVerts == 2,
                                 "Got the expected number of vertices back for a Class II pentagon");
                        for (int j = 0; j < edgeBoundary.numVerts; j++) {
                            Assert.True(
                                     GeoCoord.geoAlmostEqual(edgeBoundary.verts[j],
                                                    boundary.verts[expectedVertices[i,j]]),
                                     "Got expected vertex");
                        }
                    }
                }
                Assert.True(missingEdgeCount == 1,
                         "Only one edge was deleted for the pentagon");
            }
        }

    }
}