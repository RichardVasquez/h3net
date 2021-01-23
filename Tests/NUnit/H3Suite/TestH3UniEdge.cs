using System.Linq;
using H3Lib;
using H3Lib.Extensions;
using NUnit.Framework;

namespace TestSuite
{
    [TestFixture]
    public class TestH3UniEdge
    {
        private readonly GeoCoord _sfGeo = new GeoCoord(0.659966917655, -2.1364398519396);

        [Test]
        public void H3IndexesAreNeighbors()
        {
            var sf = _sfGeo.ToH3Index(9);

            var (_, ring) = sf.HexRing(1);
            Assert.AreEqual(false, sf.IsNeighborTo(sf));

            int neighbors = ring
               .Count(neighbor => neighbor != 0 && sf.IsNeighborTo(neighbor));
            Assert.AreEqual(6, neighbors);

            var (_, largerRing) = sf.HexRing(2);

            neighbors = largerRing.Count(t => t != 0 && sf.IsNeighborTo(t));
            Assert.AreEqual(0, neighbors);

            var sfBroken = sf;
            sfBroken = sfBroken.SetMode(H3Mode.UniEdge);
            Assert.IsFalse(sf.IsNeighborTo(sfBroken));
            Assert.IsFalse(sfBroken.IsNeighborTo(sf));

            var sfBigger = _sfGeo.ToH3Index(7);
            Assert.IsFalse(sf.IsNeighborTo(sfBigger));

            Assert.IsTrue(ring[2].IsNeighborTo(ring[1]));
        }

        [Test]
        public void GetH3UnidirectionalEdgeAndFriends()
        {
            H3Index sf = _sfGeo.ToH3Index(9);
            var (_, ring) = sf.HexRing(1);
            var sf2 = ring[0];

            var edge = sf.UniDirectionalEdgeTo(sf2);
            Assert.AreEqual(edge.OriginFromUniDirectionalEdge(), sf);
            Assert.AreEqual(edge.DestinationFromUniDirectionalEdge(), sf2);

            //var originDestination = new List<H3Index> {0, 0};
            var (origin, destination) = edge.GetH3IndexesFromUniEdge();
            Assert.AreEqual(sf, origin);
            Assert.AreEqual(sf2, destination);

            var (_, largerRing) = sf.HexRing(2);
            var sf3 = largerRing[0];

            var notEdge = sf.UniDirectionalEdgeTo(sf3);
            Assert.AreEqual(H3Lib.Constants.H3Index.H3_NULL, notEdge);
        }

        [Test]
        public void GetOriginH3IndexFromUnidirectionalEdgeBadInput()
        {
            H3Index hexagon = 0x891ea6d6533ffff;

            Assert.AreEqual(hexagon.OriginFromUniDirectionalEdge(), H3Lib.Constants.H3Index.H3_NULL);
            Assert.AreEqual(hexagon.DestinationFromUniDirectionalEdge(), H3Lib.Constants.H3Index.H3_NULL);
        }

        [Test]
        public void GetDestinationH3IndexFromUnidirectionalEdge()
        {
            H3Index hexagon = 0x891ea6d6533ffff;

            Assert.AreEqual(hexagon.DestinationFromUniDirectionalEdge(), H3Lib.Constants.H3Index.H3_NULL);
            var z = (H3Index) 0;
            Assert.AreEqual(((H3Index) 0)
                           .DestinationFromUniDirectionalEdge(),
                            H3Lib.Constants.H3Index.H3_NULL
                           );
        }

        [Test]
        public void GetH3UnidirectionalEdgeFromPentagon()
        {
            //var pentagons = Enumerable.Range(1, Constants.NUM_PENTAGONS).Select((H3Index) 0).ToList();
            //var ring = new List<H3Index> {0, 0, 0, 0, 0, 0, 0};

            H3Index pentagon;
            H3Index edge;

            for (int res = 0; res < Constants.H3.MAX_H3_RES; res++)
            {
                var pentagons = res.GetPentagonIndexes();

                for (int p = 0; p < Constants.H3.NUM_PENTAGONS; p++)
                {
                    pentagon = pentagons[p];
                    var ring = pentagon.KRing(1);

                    foreach (var neighbor in ring
                       .Where(neighbor => neighbor != pentagon &&
                                          neighbor != H3Lib.Constants.H3Index.H3_NULL))
                    {
                        edge = pentagon.UniDirectionalEdgeTo(neighbor);
                        Assert.IsTrue(edge.IsValidUniEdge());
                        edge = neighbor.UniDirectionalEdgeTo(pentagon);
                        Assert.IsTrue(edge.IsValidUniEdge());
                    }
                }
            }
        }

        [Test]
        public void H3UnidirectionalEdgeIsValid()
        {
            H3Index sf = _sfGeo.ToH3Index(9);
            var ring = sf.HexRing(1);
            var sf2 = ring.Item2[0];

            var edge = sf.UniDirectionalEdgeTo(sf2);
            Assert.IsTrue(edge.IsValidUniEdge());
            Assert.IsFalse(sf.IsValidUniEdge());

            var fakeEdge = sf.SetMode(H3Mode.UniEdge);
            Assert.IsFalse(fakeEdge.IsValidUniEdge());

            var invalidEdge = sf.SetMode(H3Mode.UniEdge)
                                .SetReservedBits((int) Direction.INVALID_DIGIT);
            Assert.IsFalse(invalidEdge.IsValidUniEdge());

            H3Index pentagon = 0x821c07fffffffff;
            H3Index goodPentagonalEdge = pentagon.SetMode(H3Mode.UniEdge).SetReservedBits(2);
            Assert.IsTrue(goodPentagonalEdge.IsValidUniEdge());

            H3Index badPentagonEdge = goodPentagonalEdge.SetReservedBits(1);
            Assert.IsFalse(badPentagonEdge.IsValidUniEdge());

            H3Index highBitEdge = edge.SetHighBit(1);
            Assert.IsFalse(highBitEdge.IsValidUniEdge());
        }

        [Test]
        public void GetH3UnidirectionalEdgesFromHexagon()
        {
            H3Index sf = _sfGeo.ToH3Index(9);
            var edges = sf.GetUniEdgesFromCell();

            for (int i = 0; i < edges.Length; i++)
            {
                Assert.IsTrue(edges[i].IsValidUniEdge());
                Assert.AreEqual(sf, edges[i].OriginFromUniDirectionalEdge());
                Assert.AreNotEqual(sf,edges[i].DestinationFromUniDirectionalEdge());
            }
        }

        [Test]
        public void GetH3UnidirectionalEdgesFromPentagon()
        {
            H3Index pentagon = 0x821c07fffffffff;
            var edges = pentagon.GetUniEdgesFromCell();

            int missingEdgeCount = 0;
            for (int i = 0; i < edges.Length; i++)
            {
                if (edges[i].Value == 0)
                {
                    missingEdgeCount++;
                }
                else
                {
                    Assert.IsTrue(edges[i].IsValidUniEdge());
                    Assert.AreEqual(pentagon, edges[i].OriginFromUniDirectionalEdge());
                    Assert.AreNotEqual(pentagon, edges[i].DestinationFromUniDirectionalEdge());
                }
            }

            Assert.AreEqual(1, missingEdgeCount);
        }

        [Test]
        public void GetH3UnidirectionalEdgeBoundary()
        {
            var expectedVertices = new[,]
                                   {
                                       {3, 4}, {1, 2}, {2, 3},
                                       {5, 0}, {4, 5}, {0, 1}
                                   };

            for (var res = 0; res < Constants.H3.MAX_H3_RES; res++)
            {
                var sf = _sfGeo.ToH3Index(res);
                var boundary = sf.ToGeoBoundary();
                var edges = sf.GetUniEdgesFromCell();
            
                for (var i = 0; i < 6; i++)
                {
                    var edgeBoundary = edges[i].UniEdgeToGeoBoundary();
                    Assert.AreEqual(2, edgeBoundary.NumVerts);
                    for (var j = 0; j < edgeBoundary.NumVerts; j++)
                    {
                        Assert.AreEqual(
                                        boundary.Verts[expectedVertices[i,j]],
                                        edgeBoundary.Verts[j]);
                    }
                }
            }
        }

        [Test]
        public void GetH3UnidirectionalEdgeBoundaryPentagonClassIii()
        {
            int[,] expectedVertices =
                {
                    {-1, -1, -1}, {2, 3, 4}, {4, 5, 6},
                    {8, 9, 0}, {6, 7, 8}, {0, 1, 2}
                };

            for (var res = 1; res < Constants.H3.MAX_H3_RES; res += 2)
            {
                var pentagon = new H3Index(res, 24, 0);
                var boundary = pentagon.ToGeoBoundary();
                var edges = pentagon.GetUniEdgesFromCell();

                var missingEdgeCount = 0;
                for (var i = 0; i < 6; i++)
                {
                    if (edges[i] == 0)
                    {
                        missingEdgeCount++;
                    }
                    else
                    {
                        var edgeBoundary = edges[i].UniEdgeToGeoBoundary();
                        Assert.AreEqual(3, edgeBoundary.NumVerts);
                        for (var j = 0; j < edgeBoundary.NumVerts; j++)
                        {
                            Assert.AreEqual(boundary.Verts[expectedVertices[i, j]], edgeBoundary.Verts[j]);
                        }
                    }
                }

                Assert.AreEqual(1, missingEdgeCount);
            }
        }

        [Test]
        public void GetH3UnidirectionalEdgeBoundaryPentagonClassIi()
        {
            var expectedVertices =
                new[,]
                {
                    {-1, -1}, {1, 2}, {2, 3},
                    {4, 0}, {3, 4}, {0, 1}
                };

            for (int res = 0; res < Constants.H3.MAX_H3_RES; res += 2)
            {
                var pentagon = new H3Index(res, 24, 0);
                var boundary = pentagon.ToGeoBoundary();
                var edges = pentagon.GetUniEdgesFromCell();

                int missingEdgeCount = 0;
                for (int i = 0; i < 6; i++)
                {
                    if (edges[i] == 0)
                    {
                        missingEdgeCount++;
                    } else
                    {
                        var edgeBoundary = edges[i].UniEdgeToGeoBoundary();
                        Assert.AreEqual(2,edgeBoundary.NumVerts);
                        for (int j = 0; j < edgeBoundary.NumVerts; j++)
                        {
                            Assert.AreEqual
                                (
                                 boundary.Verts[expectedVertices[i, j]],
                                 edgeBoundary.Verts[j]
                                );
                        }
                    }
                }

                Assert.AreEqual(1, missingEdgeCount);
            }
        }

        [Test]
        public void ExactEdgeLength_invalid()
        {
            // Test that invalid inputs do not cause crashes.
            Assert.AreEqual(0, ((H3Index) 0).ExactEdgeLengthRads());

            GeoCoord zero = default;

            H3Index h3 = zero.ToH3Index(0);
            Assert.AreEqual(0, h3.ExactEdgeLengthRads());
        }
    }
   
}
