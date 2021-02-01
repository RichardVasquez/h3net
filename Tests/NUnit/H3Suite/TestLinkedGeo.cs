using H3Lib;
using H3Lib.Extensions;
using NUnit.Framework;

namespace TestSuite
{
    [TestFixture]
    public class TestLinkedGeo
    {
        private static readonly GeoCoord Vertex1 =
            new GeoCoord().SetDegrees(87.372002166m, 166.160981117m);
        private static readonly GeoCoord Vertex2 =
            new GeoCoord().SetDegrees(87.370101364m, 166.160184306m);
        private static readonly GeoCoord Vertex3 = 
            new GeoCoord().SetDegrees(87.369088356m, 166.196239997m);
        private static readonly GeoCoord Vertex4 = 
            new GeoCoord().SetDegrees(87.369975080m, 166.233115768m);
        
        [Test]
        public void CreateLinkedGeo()
        {
            var polygon = new LinkedGeoPolygon();

            var loop = polygon.AddNewLinkedLoop();
            Assert.IsNotNull(loop);
            var coord = loop.AddLinkedCoord(Vertex1);
            Assert.IsNotNull(coord);
            coord = loop.AddLinkedCoord(Vertex2);
            Assert.IsNotNull(coord);
            coord = loop.AddLinkedCoord(Vertex3);
            Assert.IsNotNull(coord);

            loop = polygon.AddNewLinkedLoop();
            Assert.IsNotNull(loop);
            coord = loop.AddLinkedCoord(Vertex2);
            Assert.IsNotNull(coord);
            coord = loop.AddLinkedCoord(Vertex4);
            Assert.IsNotNull(coord);

            Assert.AreEqual(1,polygon.CountPolygons);
            Assert.AreEqual(2,polygon.CountLoops);
            Assert.AreEqual(3,polygon.First.Count);
            Assert.AreEqual(2,polygon.Last.Count);

            var nextPolygon = polygon.AddNewLinkedGeoPolygon();
            Assert.IsNotNull(nextPolygon);
            Assert.AreEqual(2,polygon.CountPolygons);
            
            polygon.Clear();
        }

    }
}
