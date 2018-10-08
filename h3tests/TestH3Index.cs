using System;
using h3net.API;
using NUnit.Framework;

namespace h3tests
{
    [TestFixture]
    public class TestH3Index
    {
        [Test]
        public void geoToH3ExtremeCoordinates()
        {
            // Check that none of these cause crashes.
            GeoCoord g = new GeoCoord{lat = 0, lon = 1E45};
            H3Index.geoToH3(ref g, 14);

            GeoCoord g2 =new GeoCoord{lat = 1E46, lon=1E45};
            H3Index.geoToH3(ref g2, 15);

            GeoCoord g4 = new GeoCoord();
            GeoCoord.setGeoDegs(ref g4, 2, -3E39);
            H3Index.geoToH3(ref g4, 0);
        }

        [Test]
        public void faceIjkToH3ExtremeCoordinates(){
            FaceIJK fijk0I = new FaceIJK{face= 0, coord = new  CoordIJK(3, 0, 0)};
            Assert.True(H3Index._faceIjkToH3(ref fijk0I, 0) == 0, "i out of bounds at res 0");
            FaceIJK fijk0J = new FaceIJK{face= 1, coord = new  CoordIJK(0, 4, 0)};
            Assert.True(H3Index._faceIjkToH3(ref fijk0J, 0) == 0, "j out of bounds at res 0");
            FaceIJK fijk0K = new FaceIJK{face= 2, coord = new  CoordIJK(2, 0, 5)};
            Assert.True(H3Index._faceIjkToH3(ref fijk0K, 0) == 0, "k out of bounds at res 0");

            FaceIJK fijk1I = new FaceIJK{face= 3, coord = new  CoordIJK(6, 0, 0)};
            Assert.True(H3Index._faceIjkToH3(ref fijk1I, 1) == 0, "i out of bounds at res 1");
            FaceIJK fijk1J = new FaceIJK{face= 4, coord = new  CoordIJK(0, 7, 1)};
            Assert.True(H3Index._faceIjkToH3(ref fijk1J, 1) == 0, "j out of bounds at res 1");
            FaceIJK fijk1K = new FaceIJK{face= 5, coord = new  CoordIJK(2, 0, 8)};
            Assert.True(H3Index._faceIjkToH3(ref fijk1K, 1) == 0, "k out of bounds at res 1");

            FaceIJK fijk2I = new FaceIJK{face= 6, coord = new  CoordIJK(18, 0, 0)};
            Assert.True(H3Index._faceIjkToH3(ref fijk2I, 2) == 0, "i out of bounds at res 2");
            FaceIJK fijk2J = new FaceIJK{face= 7, coord = new  CoordIJK(0, 19, 1)};
            Assert.True(H3Index._faceIjkToH3(ref fijk2J, 2) == 0, "j out of bounds at res 2");
            FaceIJK fijk2K = new FaceIJK{face= 8, coord = new  CoordIJK(2, 0, 20)};
            Assert.True(H3Index._faceIjkToH3(ref fijk2K, 2) == 0, "k out of bounds at res 2");
        }

        [Test]
        public void h3IsValidAtResolution()
        {
            for (int i = 0; i <= Constants.MAX_H3_RES; i++) 
            {
                GeoCoord geoCoord = new GeoCoord(0, 0);
                H3Index h3 = H3Index.geoToH3(ref geoCoord, i);
                Assert.True(H3Index.h3IsValid(h3) != 0, $"h3IsValid failed on resolution {i}");
            }
        }

        [Test]
        public void h3IsValidDigits()
        {
            GeoCoord geoCoord = new GeoCoord(0, 0);
            H3Index h3 = H3Index.geoToH3(ref geoCoord, 1);
            // Set a bit for an unused digit to something else.
            h3 ^= 1;
            Assert.True(H3Index.h3IsValid(h3) == 0,
                     "h3IsValid failed on invalid unused digits");
        }

        [Test]
        public void h3IsValidBaseCell()
        {
            for (int i = 0; i < Constants.NUM_BASE_CELLS; i++) 
            {
                H3Index h = H3Index.H3_INIT;
                H3Index.H3_SET_MODE(ref h, Constants.H3_HEXAGON_MODE);
                H3Index.H3_SET_BASE_CELL(ref h, i);
                string failureMessage = $"h3IsValid failed on base cell {i}";
                Assert.True(H3Index.h3IsValid(h) != 0, failureMessage);
                Assert.True(H3Index.h3GetBaseCell(h) == i, "failed to recover base cell");
            }
        }

        [Test]
        public void h3IsValidBaseCellInvalid()
        {
            H3Index hWrongBaseCell = H3Index.H3_INIT;
            H3Index.H3_SET_MODE(ref hWrongBaseCell, Constants.H3_HEXAGON_MODE);
            H3Index.H3_SET_BASE_CELL(ref hWrongBaseCell, Constants.NUM_BASE_CELLS);
            Assert.True(H3Index.h3IsValid(hWrongBaseCell) ==0, "h3IsValid failed on invalid base cell");
        }

        [Test]
        public void h3IsValidWithMode()
        {
            for (int i = 0; i <= 0xf; i++)
            {
                H3Index h = H3Index.H3_INIT;
                H3Index.H3_SET_MODE(ref h, (ulong)i);
                string failureMessage = $"h3IsValid failed on mode {i}";
                Assert.True(H3Index.h3IsValid(h) == 0 || i == 1, failureMessage);
            }
        }

        [Test]
        public void h3BadDigitInvalid()
        {
            H3Index h = H3Index.H3_INIT;
            H3Index.H3_SET_MODE(ref h, Constants.H3_HEXAGON_MODE);
            H3Index.H3_SET_RESOLUTION(ref h, 1);
            Assert.True(H3Index.h3IsValid(h) == 0, "h3IsValid failed on too large digit");
        }

        [Test]
        public void h3ToString()
        {
            const int bufSz = 17;
            string buf = "";
            H3Index.h3ToString(0x1234, ref buf, bufSz - 1);
            // Buffer should be unmodified because the size was too small
            Assert.True(buf.Length == 0, "h3ToString failed on buffer too small");
            H3Index.h3ToString(0xcafe, ref buf, bufSz);
            Assert.True(buf == "cafe", "h3ToString failed to produce base 16 results");
            H3Index.h3ToString(0xffffffffffffffff, ref buf, bufSz);
            Assert.True(buf == "ffffffffffffffff", "h3ToString failed on large input");
        }

        [Test]
        public void stringToH3()
        {
            Assert.True(H3Index.stringToH3("") == 0, "got an index from nothing");
            Assert.True(H3Index.stringToH3("**") == 0, "got an index from junk");
            Assert.True(H3Index.stringToH3("ffffffffffffffff") == 0xffffffffffffffff,
                     "failed on large input");
        }

        [Test]
        public void setH3Index()
        {
            H3Index h = new H3Index();
            H3Index.setH3Index(ref h, 5, 12, (Direction)1);
            Assert.True(H3Index.H3_GET_RESOLUTION(h) == 5, "resolution as expected");
            Assert.True(H3Index.H3_GET_BASE_CELL(h) == 12, "base cell as expected");
            Assert.True(H3Index.H3_GET_MODE(ref h) == Constants.H3_HEXAGON_MODE, "mode as expected");
            for (int i = 1; i <= 5; i++) {
                Assert.True(H3Index.H3_GET_INDEX_DIGIT(h, i) == (Direction)1, "digit as expected");
            }
            for (int i = 6; i <= Constants.MAX_H3_RES; i++) {
                Assert.True(H3Index.H3_GET_INDEX_DIGIT(h, i) == (Direction)7,
                         "blanked digit as expected");
            }
            Assert.True(h.value == 0x85184927fffffffL, "index matches expected");
        }

        [Test]
        public void h3IsResClassIII()
        {
            GeoCoord coord = new GeoCoord(0, 0);
            for (int i = 0; i <= Constants.MAX_H3_RES; i++) {
                H3Index h = H3Index.geoToH3(ref coord, i);
                Assert.True(H3Index.h3IsResClassIII(h) == (H3Index.isResClassIII(i) ? 1:0),
                         "matches existing definition");
            }
        }

    }
}
