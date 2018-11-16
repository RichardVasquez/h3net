using System;
using System.Collections.Generic;
using H3Net;
using H3Net.Code;
using NUnit.Framework;

namespace h3tests
{
    [TestFixture]
    public class TestNewH3Api
    {
        [Test]
        public void New_geoToH3_res()
        {
            GeoCoord anywhere = new GeoCoord(0, 0);

            //Assert.True(H3Index.geoToH3(ref anywhere, -1) == 0, "resolution below 0 is invalid");
            //Assert.True(H3Index.geoToH3(ref anywhere, 16) == 0, "resolution above 15 is invalid");

            Assert.True(Api.GeoToH3(anywhere, -1).Value == 0);
            Assert.True(Api.GeoToH3(anywhere, 16).Value == 0);

        }

        [Test]
        public void New_geoToH3_coord()
        {
            GeoCoord invalidLat = new GeoCoord(Double.NaN, 0);
            GeoCoord invalidLon = new GeoCoord(0, Double.NaN);
            GeoCoord invalidLatLon = new GeoCoord(Double.PositiveInfinity, Double.NegativeInfinity);

            //Assert.True(H3Index.geoToH3(ref invalidLat, 1) == 0, "invalid latitude is rejected");
            //Assert.True(H3Index.geoToH3(ref invalidLon, 1) == 0, "invalid longitude is rejected");
            //Assert.True(H3Index.geoToH3(ref invalidLatLon, 1) == 0, "coordinates with infinity are rejected");


            Assert.True(Api.GeoToH3(invalidLat, 1).Value == 0);
            Assert.True(Api.GeoToH3(invalidLon, 1).Value == 0);
            Assert.True(Api.GeoToH3(invalidLatLon, 1).Value == 0);
        }

        [Test]
        public void New_h3ToGeoBoundary_classIIIEdgeVertex()
        {
            // Bug test for https://github.com/uber/h3/issues/45
            string[] hexes =
            {
                "894cc5349b7ffff", "894cc534d97ffff", "894cc53682bffff",
                "894cc536b17ffff", "894cc53688bffff", "894cead92cbffff",
                "894cc536537ffff", "894cc5acbabffff", "894cc536597ffff"
            };
            int numHexes = hexes.Length;
            H3Index h3;
            GeoBoundary b = new GeoBoundary();
            for (int i = 0; i < numHexes; i++)
            {
                h3 = H3Index.stringToH3(hexes[i]);
//                H3Index.h3ToGeoBoundary(h3, ref b);
//                Assert.True(b.numVerts == 7, "got expected vertex count");
                var newGB = Api.H3ToGeoBoundary(h3);
                Assert.True(newGB.VertexCount == 7);
            }
        }

        [Test]
        public void h3ToGeoBoundary_classIIIEdgeVertex_exact()
        {
            // Bug test for https://github.com/uber/h3/issues/45
            H3Index h3 = H3Index.stringToH3("894cc536537ffff");
            GeoBoundary boundary = new GeoBoundary();
            boundary.numVerts = 7;
            boundary.verts = new List<GeoCoord>();


            var v0 = new GeoCoord();
            var v1 = new GeoCoord();
            var v2 = new GeoCoord();
            var v3 = new GeoCoord();
            var v4 = new GeoCoord();
            var v5 = new GeoCoord();
            var v6 = new GeoCoord();

            GeoCoord.setGeoDegs(ref v0, 18.043333154, -66.27836523500002);

            GeoCoord.setGeoDegs(ref v1, 18.042238363, -66.27929062800001);
            GeoCoord.setGeoDegs(ref v2, 18.040818259, -66.27854193899998);
            GeoCoord.setGeoDegs(ref v3, 18.040492975, -66.27686786700002);
            GeoCoord.setGeoDegs(ref v4, 18.041040385, -66.27640518300001);
            GeoCoord.setGeoDegs(ref v5, 18.041757122, -66.27596711500001);
            GeoCoord.setGeoDegs(ref v6, 18.043007860, -66.27669118199998);

            boundary.verts.Add(v0);
            boundary.verts.Add(v1);
            boundary.verts.Add(v2);
            boundary.verts.Add(v3);
            boundary.verts.Add(v4);
            boundary.verts.Add(v5);
            boundary.verts.Add(v6);

            t_assertBoundary(h3, boundary);
        }

        void t_assertBoundary(H3Index h3, GeoBoundary b1)
        {
            // Generate cell boundary for the h3 index
            GeoBoundary b2 = new GeoBoundary();
            H3Index.h3ToGeoBoundary(h3, ref b2);
            Assert.True(b1.numVerts == b2.numVerts, "expected cell boundary count");
            for (int v = 0; v < b1.numVerts; v++)
            {
                Assert.True
                    (
                     GeoCoord.geoAlmostEqual(b1.verts[v], b2.verts[v]),
                     "got expected vertex"
                    );
            }
        }
    }
}

