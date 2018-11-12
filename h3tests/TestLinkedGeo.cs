using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using H3Net.Code;
using NUnit.Framework;
using NUnit.Framework.Internal;
using NUnit.Framework.Internal.Commands;

namespace h3tests
{
    class TestLinkedGeo
    {
        static GeoCoord vertex1 = new GeoCoord();
        static GeoCoord vertex2 = new GeoCoord();
        static GeoCoord vertex3 = new GeoCoord();
        static GeoCoord vertex4 = new GeoCoord();


        [SetUp]
        public void setVertices()
        {
            GeoCoord.setGeoDegs(ref vertex1, 87.372002166, 166.160981117);
            GeoCoord.setGeoDegs(ref vertex2, 87.370101364, 166.160184306);
            GeoCoord.setGeoDegs(ref vertex3, 87.369088356, 166.196239997);
            GeoCoord.setGeoDegs(ref vertex4, 87.369975080, 166.233115768);
        }

        [Test]
        public void createLinkedGeo() {
            LinkedGeo.LinkedGeoPolygon polygon = new LinkedGeo.LinkedGeoPolygon();
            LinkedGeo.LinkedGeoLoop loop = new LinkedGeo.LinkedGeoLoop();
            LinkedGeo.LinkedGeoCoord coord = new LinkedGeo.LinkedGeoCoord();

            loop = LinkedGeo.addNewLinkedLoop(ref polygon);
            Assert.True(loop != null, "Loop created");
            coord = LinkedGeo.addLinkedCoord(ref loop, ref vertex1);
            Assert.True(coord != null, "Coord created");
            coord = LinkedGeo.addLinkedCoord(ref loop, ref vertex2);
            Assert.True(coord != null, "Coord created");
            coord = LinkedGeo.addLinkedCoord(ref loop, ref vertex3);
            Assert.True(coord != null, "Coord created");

            loop = LinkedGeo.addNewLinkedLoop(ref polygon);
            Assert.True(loop != null, "Loop createed");
            coord = LinkedGeo.addLinkedCoord(ref loop, ref vertex2);
            Assert.True(coord != null, "Coord created");
            coord = LinkedGeo.addLinkedCoord(ref loop, ref vertex4);
            Assert.True(coord != null, "Coord created");

            Assert.True(LinkedGeo.countLinkedPolygons(ref polygon) == 1, "Polygon count correct");
            Assert.True(LinkedGeo.countLinkedLoops(ref polygon) == 2, "Loop count correct");
            Assert.True(LinkedGeo.countLinkedCoords(ref polygon.first) == 3,
                     "Coord count 1 correct");
            Assert.True(LinkedGeo.countLinkedCoords(ref polygon.last) == 2,
                     "Coord count 2 correct");

            LinkedGeo.LinkedGeoPolygon nextPolygon = LinkedGeo.addNewLinkedPolygon(ref polygon);
            Assert.True(nextPolygon != null, "polygon created");

            Assert.True(LinkedGeo.countLinkedPolygons(ref polygon) == 2, "Polygon count correct");

            LinkedGeo.destroyLinkedPolygon(ref polygon);
            polygon = null;
        }

    }
}
