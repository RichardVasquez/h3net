using System;
using H3Lib;
using H3Lib.Extensions;
using NUnit.Framework;

namespace TestSuite
{
    [TestFixture]
    public class TestH3Api
    {
        [Test]
        public void GeoToH3Res()
        {
            GeoCoord anywhere = default;
            Assert.AreEqual(Constants.H3Index.H3_NULL, Api.GeoToH3(anywhere, -1));
            Assert.AreEqual(Constants.H3Index.H3_NULL, Api.GeoToH3(anywhere, 16));
        }

        [Test]
        public void GeoToH3Coord()
        {
            GeoCoord invalidLat = new GeoCoord(double.NaN, 0);
            GeoCoord invalidLon = new GeoCoord(0, double.NaN);
            GeoCoord invalidLatLon = new GeoCoord(double.PositiveInfinity, double.NegativeInfinity);

            Assert.AreEqual(Constants.H3Index.H3_NULL, Api.GeoToH3(invalidLat, 1));
            Assert.AreEqual(Constants.H3Index.H3_NULL, Api.GeoToH3(invalidLon, 1));
            Assert.AreEqual(Constants.H3Index.H3_NULL, Api.GeoToH3(invalidLatLon, 1));
        }
        // Bug test for https://github.com/uber/h3/issues/45
        [Test]        
        public void h3ToGeoBoundary_classIIIEdgeVertex()
        {
            var hexes = new string[]
                        {
                            "894cc5349b7ffff", "894cc534d97ffff", "894cc53682bffff",
                            "894cc536b17ffff", "894cc53688bffff", "894cead92cbffff",
                            "894cc536537ffff", "894cc5acbabffff", "894cc536597ffff"
                        };
            for (int i = 0; i < hexes.Length; i++)
            {
                var h3 = Api.StringToH3(hexes[i]);
                var b = h3.ToGeoBoundary();
                Assert.AreEqual(7, b.NumVerts);
            }
        }

        [Test]
        // Bug test for https://github.com/uber/h3/issues/45
        public void H3ToGeoBoundary_classIIIEdgeVertex_exact()
        {
            H3Index h3 = Api.StringToH3("894cc536537ffff");
            GeoBoundary boundary = new GeoBoundary();
            boundary.NumVerts = 7;
                
            boundary.Verts[0] = Api.SetGeoDegs( 18.043333154, -66.27836523500002);
            boundary.Verts[1] = Api.SetGeoDegs( 18.042238363, -66.27929062800001);
            boundary.Verts[2] = Api.SetGeoDegs( 18.040818259, -66.27854193899998);
            boundary.Verts[3] = Api.SetGeoDegs( 18.040492975, -66.27686786700002);
            boundary.Verts[4] = Api.SetGeoDegs( 18.041040385, -66.27640518300001);
            boundary.Verts[5] = Api.SetGeoDegs( 18.041757122, -66.27596711500001);
            boundary.Verts[6] = Api.SetGeoDegs( 18.043007860, -66.27669118199998);

            GeoBoundary myBoundary;
            Api.H3ToGeoBoundary(h3, out myBoundary);
            Assert.AreEqual(boundary.NumVerts,myBoundary.NumVerts);

            for (int i = 0; i < boundary.NumVerts; i++)
            {
                Assert.AreEqual(boundary.Verts[0], myBoundary.Verts[0]);
            }
        }
        
        // Bug test for https://github.com/uber/h3/issues/212
        [Test]
        public void H3ToGeoBoundary_coslonConstrain()
        {
            H3Index h3 = 0x87dc6d364ffffffL;
            GeoBoundary boundary = new GeoBoundary();
            boundary.NumVerts = 6;
            boundary.Verts[0] = Api.SetGeoDegs( -52.0130533678236091, -34.6232931343713091);
            boundary.Verts[0] = Api.SetGeoDegs( -52.0041156384652012, -34.6096733160584549);
            boundary.Verts[0] = Api.SetGeoDegs( -51.9929610229502472, -34.6165157145896387);
            boundary.Verts[0] = Api.SetGeoDegs( -51.9907410568096608, -34.6369680004259877);
            boundary.Verts[0] = Api.SetGeoDegs( -51.9996738734672377, -34.6505896528323660);
            boundary.Verts[0] = Api.SetGeoDegs( -52.0108315681413629, -34.6437571897165668);

            GeoBoundary myBoundary;
            Api.H3ToGeoBoundary(h3, out myBoundary);
            Assert.AreEqual(boundary.NumVerts,myBoundary.NumVerts);

            for (int i = 0; i < boundary.NumVerts; i++)
            {
                Assert.AreEqual(boundary.Verts[0], myBoundary.Verts[0]);
            }
        }

        [Test]
        public void Version()
        {
            Assert.GreaterOrEqual(Constants.H3_VERSION_MAJOR, 0);
            Assert.GreaterOrEqual(Constants.H3_VERSION_MINOR, 0);
            Assert.GreaterOrEqual(Constants.H3_VERSION_PATCH, 0);
        }

    }        
}
