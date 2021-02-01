using H3Lib;
using H3Lib.Extensions;
using NUnit.Framework;

namespace TestSuite
{
    [TestFixture]
    public class TestH3Index
    {
        [Test]
        public void GeoToH3ExtremeCoordinates()
        {
            // Check that none of these cause crashes.
            var g = new GeoCoord(0m, 1E28m);
            var h = g.ToH3Index(14);

            var g2 = new GeoCoord(1E28m, 1E28m);
            h = g2.ToH3Index(15);

            var g4 = new GeoCoord().SetDegrees(2, -1e28m);
            h = g4.ToH3Index(0);
        }

        [Test]
        public void FaceIjkToH3ExtremeCoordinates()
        {
            var fijk0I = new FaceIjk(0, new CoordIjk(3, 0, 0));
            Assert.AreEqual(0, fijk0I.ToH3(0).Value);
            var fijk0J = new FaceIjk(1, new CoordIjk(0, 4, 0));
            Assert.AreEqual(0, fijk0J.ToH3(0).Value);
            var fijk0K = new FaceIjk(2, new CoordIjk(2, 0, 5));
            Assert.AreEqual(0, fijk0K.ToH3(0).Value);

            var fijk1I = new FaceIjk(3, new CoordIjk(6, 0, 0));
            Assert.AreEqual(0, fijk1I.ToH3(0).Value);
            var fijk1J = new FaceIjk(4, new CoordIjk(0, 7, 1));
            Assert.AreEqual(0, fijk1J.ToH3(0).Value);
            var fijk1K = new FaceIjk(5, new CoordIjk(2, 0, 8));
            Assert.AreEqual(0, fijk1K.ToH3(0).Value);

            var fijk2I = new FaceIjk(3, new CoordIjk(18, 0, 0));
            Assert.AreEqual(0, fijk2I.ToH3(0).Value);
            var fijk2J = new FaceIjk(4, new CoordIjk(0, 19, 1));
            Assert.AreEqual(0, fijk2J.ToH3(0).Value);
            var fijk2K = new FaceIjk(5, new CoordIjk(2, 0, 20));
            Assert.AreEqual(0, fijk2K.ToH3(0).Value);
        }
        
        [Test]
        public void H3IsValidAtResolution()
        {
            for (int i = 0; i <= Constants.H3.MAX_H3_RES; i++)
            {
                GeoCoord geoCoord = default;
                H3Index h3 = geoCoord.ToH3Index(i);
                Assert.IsTrue(h3.IsValid());
            }
        }

        [Test]
        public void H3IsValidDigits()
        {
            GeoCoord geoCoord = default;
            var h3 = geoCoord.ToH3Index(1);
            h3 ^= 1;
            Assert.IsFalse(h3.IsValid());
        }

        [Test]
        public void H3IsValidBaseCell()
        {
            for (int i = 0; i < Constants. H3.NUM_BASE_CELLS; i++)
            {
                H3Index h = H3Lib.Constants.H3Index.H3_INIT;
                h = h.SetMode(H3Mode.Hexagon).SetBaseCell(i);
                Assert.IsTrue(h.IsValid());
                Assert.AreEqual(i, h.BaseCell);
            }
        }

        [Test]
        public void H3IsValidBaseCellInvalid()
        {
            H3Index hWrongBaseCell = H3Lib.Constants.H3Index.H3_INIT;
            hWrongBaseCell.SetMode(H3Mode.Hexagon).SetBaseCell(Constants.H3.NUM_BASE_CELLS);
            Assert.IsFalse(hWrongBaseCell.IsValid());
        }

        [Test]
        public void H3IsValidWithMode()
        {
            for (var i = 0; i <= 15; i++)
            {
                H3Index h = H3Lib.Constants.H3Index.H3_INIT;
                h = h.SetMode((H3Mode) i);
                if (i == (int) H3Mode.Hexagon)
                {
                    Assert.IsTrue(h.IsValid());
                }
                else
                {
                    Assert.IsFalse(h.IsValid());
                }
            }
        }

        [Test]
        public void H3IsValidReservedBits()
        {
            for (int i = 0; i < 8; i++)
            {
                H3Index h = H3Lib.Constants.H3Index.H3_INIT;
                h = h.SetMode(H3Mode.Hexagon).SetReservedBits(i);

                if (i == 0)
                {
                    Assert.IsTrue(h.IsValid());
                }
                else
                {
                    Assert.IsFalse(h.IsValid());
                }
            }
        }

        [Test]
        public void H3IsValidHighBit()
        {
            H3Index h = H3Lib.Constants.H3Index.H3_INIT;
            h = h.SetMode(H3Mode.Hexagon).SetHighBit(1);
            Assert.IsFalse(h.IsValid());
        }

        [Test]
        public void H3BadDigitInvalid()
        {
            H3Index h = H3Lib.Constants.H3Index.H3_INIT;
            // By default the first index digit is out of range.
            h = h.SetMode(H3Mode.Hexagon).SetResolution(1);
            Assert.IsFalse(h.IsValid());
        }

        [Test]
        public void H3DeletedSubsequenceInvalid()
        {
            // Create an index located in a deleted subsequence of a pentagon.
            H3Index h = new H3Index(1, 4, Direction.K_AXES_DIGIT);
            Assert.IsFalse(h.IsValid());
        }

        [Test]
        public void H3ToString()
        {
            //  Not really applicable since H3Index already has
            //  its own ToString(), though there may be some
            //  options added to it in the future.
            
            /*
                const size_t bufSz = 17;
                char buf[17] = {0};
                H3_EXPORT(h3ToString)(0x1234, buf, bufSz - 1);
                // Buffer should be unmodified because the size was too small
                t_assert(buf[0] == 0, "h3ToString failed on buffer too small");
                H3_EXPORT(h3ToString)(0xcafe, buf, bufSz);
                t_assert(strcmp(buf, "cafe") == 0,
                         "h3ToString failed to produce base 16 results");
                H3_EXPORT(h3ToString)(0xffffffffffffffff, buf, bufSz);
                t_assert(strcmp(buf, "ffffffffffffffff") == 0,
                         "h3ToString failed on large input");
                t_assert(buf[bufSz - 1] == 0, "didn't null terminate");
             */
        }

        [Test]
        public void StringToH3()
        {
            //  NOTE: This one was skipped in implementation.
            //  However, it's not impossible to work around.
            if (ulong.TryParse("", out ulong result1))
            {
                H3Index h = result1;
                Assert.AreEqual(0, h.Value);
            }
            if (ulong.TryParse("**", out ulong result2))
            {
                H3Index h = result2;
                Assert.AreEqual(0, h.Value);
            }
        }

        [Test]
        public void SetH3Index()
        {
            var h = new H3Index(5, 12, 1);
            Assert.AreEqual(5, h.Resolution);
            Assert.AreEqual(12, h.BaseCell);
            Assert.AreEqual(H3Mode.Hexagon, h.Mode);

            for (int i = 1; i <= 5; i++)
            {
                Assert.AreEqual(1, (int) h.GetIndexDigit(i));
            }

            for (int i = 6; i <= Constants.H3.MAX_H3_RES; i++)
            {
                Assert.AreEqual(7, (int) h.GetIndexDigit(i));
            }

            Assert.AreEqual(0x85184927fffffffL, h.Value);
        }

        [Test]
        public void H3IsResClassIii()
        {
            GeoCoord coord = default;
            for (int i = 0; i <= Constants.H3.MAX_H3_RES; i++)
            {
                var h = coord.ToH3Index(i);
                Assert.AreEqual(h.IsResClassIii, i.IsResClassIii());
            }
        }
    }
}
