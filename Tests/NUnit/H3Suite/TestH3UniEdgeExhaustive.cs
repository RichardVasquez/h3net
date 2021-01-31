using System;
using System.Net;
using H3Lib;
using H3Lib.Extensions;
using NUnit.Framework;
using TestSuite.Lib;

namespace TestSuite
{
    [TestFixture]
    public class TestH3UniEdgeExhaustive
    {
        private static void H3UniEdgeCorrectnessAssertions(H3Index h3)
        {
            bool isPentagon = h3.IsPentagon();
            var edges = h3.GetUniEdgesFromCell();
            
            for (var i = 0; i < 6; i++)
            {
                if (isPentagon && i == 0)
                {
                    Assert.AreEqual(Constants.H3Index.H3_NULL, edges[i]);
                    continue;
                }
                
                Assert.IsTrue(edges[i].IsValidUniEdge());
                Assert.AreEqual(h3, edges[i].OriginFromUniDirectionalEdge());
                var destination = edges[i].DestinationFromUniDirectionalEdge();
                Assert.IsTrue(h3.IsNeighborTo(destination));
            }
        }
        
        private static void H3UniEdgeBoundaryAssertions(H3Index h3)
        {
            var edges = h3.GetUniEdgesFromCell();

            for (var i = 0; i < 6; i++)
            {
                if (edges[i] == Constants.H3Index.H3_NULL)
                {
                    continue;
                }

                var edgeBoundary = edges[i].UniEdgeToGeoBoundary();
                var destination = edges[i].DestinationFromUniDirectionalEdge();
                var revEdge = destination.UniDirectionalEdgeTo(h3);

                var revEdgeBoundary = revEdge.UniEdgeToGeoBoundary();

                Assert.AreEqual(edgeBoundary.NumVerts, revEdgeBoundary.NumVerts);

                for (var j = 0; j < edgeBoundary.NumVerts; j++)
                {
                    if (Math.Abs
                            (
                             edgeBoundary.Verts[j].Latitude -
                             revEdgeBoundary.Verts[revEdgeBoundary.NumVerts - 1 - j].Latitude
                            ) >= .000001m)
                    {
                        var k = 0;
                    }

                    Assert.IsTrue
                        (
                         Math.Abs(
                         edgeBoundary.Verts[j].Latitude -
                         revEdgeBoundary.Verts[revEdgeBoundary.NumVerts - 1 - j].Latitude
                         ) < 0.000001m
                        );
                    Assert.IsTrue
                    (
                            Math.Abs
                                (
                                 edgeBoundary.Verts[j].Longitude -
                                 revEdgeBoundary.Verts[revEdgeBoundary.NumVerts - 1 - j].Longitude
                                ) <
                            0.000001m
                        );
                }
            }
        }
        
        [Test]
        public void H3UniEdgeCorrectness()
        {
            Utility.IterateAllIndexesAtRes(0, H3UniEdgeCorrectnessAssertions);
            Utility.IterateAllIndexesAtRes(1, H3UniEdgeCorrectnessAssertions);
            Utility.IterateAllIndexesAtRes(2, H3UniEdgeCorrectnessAssertions);
            Utility.IterateAllIndexesAtRes(3, H3UniEdgeCorrectnessAssertions);
            Utility.IterateAllIndexesAtRes(4, H3UniEdgeCorrectnessAssertions);
        }
        
        [Test]
        public void h3UniEdgeBoundary()
        {
            Utility.IterateAllIndexesAtRes(0, H3UniEdgeBoundaryAssertions);
            Utility.IterateAllIndexesAtRes(1, H3UniEdgeBoundaryAssertions);
            Utility.IterateAllIndexesAtRes(2, H3UniEdgeBoundaryAssertions);
            Utility.IterateAllIndexesAtRes(3, H3UniEdgeBoundaryAssertions);
            Utility.IterateAllIndexesAtRes(4, H3UniEdgeBoundaryAssertions);
            // Res 5: normal base cell
            Utility.IterateBaseCellIndexesAtRes(5, H3UniEdgeBoundaryAssertions, 0);
            // Res 5: pentagon base cell
            Utility.IterateBaseCellIndexesAtRes(5, H3UniEdgeBoundaryAssertions, 14);
            // Res 5: polar pentagon base cell
            Utility.IterateBaseCellIndexesAtRes(5, H3UniEdgeBoundaryAssertions, 117);
            // Res 6: Test one pentagon just to check for new edge cases
            Utility.IterateBaseCellIndexesAtRes(6, H3UniEdgeBoundaryAssertions, 14);
        }


    }
}
