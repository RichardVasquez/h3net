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
            Assert.AreEqual(H3Lib.StaticData.H3Index.H3_NULL, notEdge);
        }

        [Test]
        public void GetOriginH3IndexFromUnidirectionalEdgeBadInput()
        {
            H3Index hexagon = 0x891ea6d6533ffff;

            Assert.AreEqual(hexagon.OriginFromUniDirectionalEdge(), H3Lib.StaticData.H3Index.H3_NULL);
            Assert.AreEqual(hexagon.DestinationFromUniDirectionalEdge(), H3Lib.StaticData.H3Index.H3_NULL);
        }

        [Test]
        public void GetDestinationH3IndexFromUnidirectionalEdge()
        {
            H3Index hexagon = 0x891ea6d6533ffff;

            Assert.AreEqual(hexagon.DestinationFromUniDirectionalEdge(), H3Lib.StaticData.H3Index.H3_NULL);
            var z = (H3Index) 0;
            Assert.AreEqual(((H3Index) 0)
                           .DestinationFromUniDirectionalEdge(),
                            H3Lib.StaticData.H3Index.H3_NULL
                           );
        }

        [Test]
        public void GetH3UnidirectionalEdgeFromPentagon()
        {
            //var pentagons = Enumerable.Range(1, Constants.NUM_PENTAGONS).Select((H3Index) 0).ToList();
            //var ring = new List<H3Index> {0, 0, 0, 0, 0, 0, 0};

            H3Index pentagon;
            H3Index edge;

            for (int res = 0; res < Constants.MAX_H3_RES; res++)
            {
                var pentagons = res.GetPentagonIndexes();

                for (int p = 0; p < Constants.NUM_PENTAGONS; p++)
                {
                    pentagon = pentagons[p];
                    var ring = pentagon.KRing(1);

                    foreach (var neighbor in ring
                       .Where(neighbor => neighbor != pentagon &&
                                          neighbor != H3Lib.StaticData.H3Index.H3_NULL))
                    {
                        edge = pentagon.UniDirectionalEdgeTo(neighbor);
                        Assert.IsTrue(edge.IsValidUniEdge());
                        edge = neighbor.UniDirectionalEdgeTo(pentagon);
                        Assert.IsTrue(edge.IsValidUniEdge());
                    }
                }
            }

        }
    }
}
