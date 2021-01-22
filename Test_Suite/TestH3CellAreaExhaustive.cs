using System;
using H3Lib;
using H3Lib.Extensions;
using NUnit.Framework;
using TestSuite.Lib;
using GeoCoord = H3Lib.StaticData.GeoCoord;

namespace TestSuite
{
    [TestFixture]
    public class TestH3CellAreaExhaustive
    {
        /// <summary>
        /// Basic checks around the great circle distance between the centers of two
        /// neighboring cells. Tests positivity and commutativity.
        ///
        /// Tests the functions:
        ///     pointDistRads
        ///     pointDistKm
        ///     pointDistM
        /// </summary>
        private void haversine_assert(H3Index edge)
        {
            var origin = edge.OriginFromUniDirectionalEdge();
            var a = origin.ToGeoCoord();

            var destination = edge.DestinationFromUniDirectionalEdge();
            var b = destination.ToGeoCoord();

            double ab, ba;

            ab = a.DistanceToRadians(b);
            ba = b.DistanceToRadians(a);
            Assert.Greater(ab, 0);
            Assert.AreEqual(ab, ba);

            ab = a.DistanceToKm(b);
            ba = b.DistanceToKm(a);
            Assert.Greater(ab, 0);
            Assert.AreEqual(ab, ba);

            ab = a.DistanceToM(b);
            ba = b.DistanceToM(a);
            Assert.Greater(ab, 0);
            Assert.AreEqual(ab, ba);

            Assert.Greater(a.DistanceToKm(b), a.DistanceToRadians(b));
            Assert.Greater(a.DistanceToM(b), a.DistanceToKm(b));
        }
        
        /// <summary>
        /// Tests positivity of edge length calculation for the functions:
        /// 
        ///     exactEdgeLengthRads
        ///     exactEdgeLengthKm
        ///     exactEdgeLengthM
        /// </summary>
        private void edge_length_assert(H3Index edge)
        {
            Assert.Greater(edge.ExactEdgeLengthRads(), 0);
            Assert.Greater(edge.ExactEdgeLengthKm(), 0);
            Assert.Greater(edge.ExactEdgeLengthM(), 0);
        }
        
        /// <summary>
        /// Test that cell area calculations are positive for the functions:
        /// 
        ///     cellAreaRads2
        ///     cellAreaKm2
        ///     cellAreaM2
        /// </summary>
        private void cell_area_assert(H3Index cell)
        {
            Assert.Greater(cell.CellAreaRadians2(),0);
            Assert.Greater(cell.CellAreaKm2(),0);
            Assert.Greater(cell.CellAreaM2(),0);
        }
        
        /// <summary>
        /// Apply a cell area calculation function to every cell on the earth at a given
        /// resolution, and check that it sums up the total earth area.
        /// </summary>
        /// <param name="res">resolution of the cells</param>
        /// <param name="cellArea">callback to compute area of each cell</param>
        /// <param name="target">expected earth area in some units</param>
        /// <param name="tolerance">error tolerance allowed between expected and actual</param>
        private void earth_area_test
            (int res, Func<H3Index, double> cellArea, double target, double tolerance)
        {
            double area = Utility.mapSumAllCells_double(res, cellArea);
            Assert.Less(Math.Abs(area-target), tolerance);
        }
        
        /// <summary>
        /// Apply callback for every unidirectional edge at the given resolution.
        /// </summary>
        [Test]
        public void haversine_distances()
        {
            Utility.iterateAllUnidirectionalEdgesAtRes(0, haversine_assert);
            Utility.iterateAllUnidirectionalEdgesAtRes(1, haversine_assert);
            Utility.iterateAllUnidirectionalEdgesAtRes(2, haversine_assert);
            Utility.iterateAllUnidirectionalEdgesAtRes(3, haversine_assert);
        }
        
        [Test]
        public void edge_length()
        {
            Utility.iterateAllUnidirectionalEdgesAtRes(0, edge_length_assert);
            Utility.iterateAllUnidirectionalEdgesAtRes(1, edge_length_assert);
            Utility.iterateAllUnidirectionalEdgesAtRes(2, edge_length_assert);
            Utility.iterateAllUnidirectionalEdgesAtRes(3, edge_length_assert);
        }
        
        [Test]
        public void cell_area_positive()
        {
            Utility.IterateAllIndexesAtRes(0, cell_area_assert);
            Utility.IterateAllIndexesAtRes(1, cell_area_assert);
            Utility.IterateAllIndexesAtRes(2, cell_area_assert);
            Utility.IterateAllIndexesAtRes(3, cell_area_assert);
        }
        
        [Test]
        public void cell_area_earth()
        {
            // earth area in different units
            double rads2 = 4 * Constants.M_PI;
            double km2 = rads2 * Constants.EARTH_RADIUS_KM * Constants.EARTH_RADIUS_KM;
            double m2 = km2 * 1000 * 1000;

            // Notice the drop in accuracy at resolution 1.
            // I think this has something to do with Class II vs Class III
            // resolutions.

            
            earth_area_test(0, H3IndexExtensions.CellAreaRadians2, rads2, 1e-14);
            earth_area_test(0, H3IndexExtensions.CellAreaKm2, km2, 1e-6);
            earth_area_test(0, H3IndexExtensions.CellAreaM2, m2, 1e0);

            earth_area_test(1, H3IndexExtensions.CellAreaRadians2, rads2, 1e-9);
            earth_area_test(1, H3IndexExtensions.CellAreaKm2, km2, 1e-1);
            earth_area_test(1, H3IndexExtensions.CellAreaM2, m2, 1e5);

            earth_area_test(2, H3IndexExtensions.CellAreaRadians2, rads2, 1e-12);
            earth_area_test(2, H3IndexExtensions.CellAreaKm2, km2, 1e-5);
            earth_area_test(2, H3IndexExtensions.CellAreaM2, m2, 1e0);

            earth_area_test(3, H3IndexExtensions.CellAreaRadians2, rads2, 1e-11);
            earth_area_test(3, H3IndexExtensions.CellAreaKm2, km2, 1e-3);
            earth_area_test(3, H3IndexExtensions.CellAreaM2, m2, 1e3);

            earth_area_test(4, H3IndexExtensions.CellAreaRadians2, rads2, 1e-11);
            earth_area_test(4, H3IndexExtensions.CellAreaKm2, km2, 1e-3);
            earth_area_test(4, H3IndexExtensions.CellAreaM2, m2, 1e2);
        }
    }
}
