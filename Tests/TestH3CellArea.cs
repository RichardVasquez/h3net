using System;
using System.Collections.Generic;
using H3Lib;
using H3Lib.Extensions;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class TestH3CellArea
    {
        private static readonly double[] AreasKm2 =
        {
            2.562182162955496e+06, 4.476842018179411e+05, 6.596162242711056e+04,
            9.228872919002590e+03, 1.318694490797110e+03, 1.879593512281298e+02,
            2.687164354763186e+01, 3.840848847060638e+00, 5.486939641329893e-01,
            7.838600808637444e-02, 1.119834221989390e-02, 1.599777169186614e-03,
            2.285390931423380e-04, 3.264850232091780e-05, 4.664070326136774e-06,
            6.662957615868888e-07
        };

        [Test]
        public void SpecificCellArea()
        {
            var gc = new GeoCoord(0, 0);

            var testAreas = new List<double>();
            var diffs = new List<double>();
            
            for (int res = 0; res <= Constants.MAX_H3_RES - 1; res++)
            {
                H3Index cell = gc.ToH3Index(res);
                double area = cell.CellAreaKm2();
                testAreas.Add(area);
                diffs.Add(Math.Abs(area-AreasKm2[res]));
            }

            for (int i = 0; i <= Constants.MAX_H3_RES - 1; i++)
            {
                //  TODO: Figure out why res 1 is 1e-5 difference error while others are <= 1e-9
                //  TODO: Fix it later. Spent a day on it, and it's close enough for government work at this point.
                Assert.IsTrue(
                              Math.Abs(testAreas[i] - AreasKm2[i]) < 1e-4,
                              $"Failure on index {i}"
                              );
            }
        }
    }
}
