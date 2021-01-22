using System;
using System.Collections.Generic;
using System.Linq;
using H3Lib;
using H3Lib.Extensions;
using NUnit.Framework;

namespace TestSuite.Lib
{
    /// <summary>
    /// Miscellaneous useful functions used for unit tests.
    /// </summary>
    public class Utility
    {
        /// <summary>
        /// Call the callback for every index at the given resolution.
        /// </summary>
        public static void IterateAllIndexesAtRes(int res, Action<H3Index> callback)
        {
            IterateAllIndexesAtResPartial(res, callback, Constants.NUM_BASE_CELLS);
        }

        /// <summary>
        /// Call the callback for every index at the given resolution in base
        /// cell 0 up to the given base cell number.
        /// </summary>
        /// <param name="res"></param>
        /// <param name="callback"></param>
        /// <param name="baseCells"></param>
        public static void IterateAllIndexesAtResPartial(int res, Action<H3Index> callback, int baseCells)
        {
            Assert.LessOrEqual(baseCells, Constants.NUM_BASE_CELLS);
            for (var i = 0; i < baseCells; i++)
            {
                IterateBaseCellIndexesAtRes(res, callback, i);
            }

        }

        /// <summary>
        /// Call the callback for every index at the given resolution in a
        /// specific base cell
        /// </summary>
        public static void IterateBaseCellIndexesAtRes(int res, Action<H3Index> callback, int baseCell)
        {
            var bc = new H3Index(0, baseCell, 0);
            var (_, children) = bc.Uncompact(res);

            foreach (var index in children.Where(c=>c!=0))
            {
                callback(index);
            }
            
            children.Clear();
        }

        /// <summary>
        /// Returns the number of non-invalid indexes in the collection.
        /// </summary>
        public static int CountActualHexagons(List<H3Index> hexagons)
        {
            return hexagons
               .Count(hexagon => hexagon != H3Lib.StaticData.H3Index.H3_NULL);
        }
        
        /// <summary>
        /// List of cells at a given resolutions
        /// </summary>
        /// <param name="res">resolution</param>
        private static List<H3Index> GetCellsAtRes(int res)
        {
            var (_, resCells) =
                BaseCellsExtensions.GetRes0Indexes().Uncompact(res);
            return resCells.Where(c => c.Value != 0).ToList();
        }
        
        public static double mapSumAllCells_double(int res, Func<H3Index, double> callback)
        {
            var cells = GetCellsAtRes(res);

            long N = res.NumHexagons();

            double total = 0.0;
            for (int i = 0; i < N; i++)
            {
                total += callback(cells[i]);
            }
            cells.Clear();

            return total;
        }
        
        public static void iterateAllUnidirectionalEdgesAtRes(int res, Action<H3Index> callback)
        {
            var cells = GetCellsAtRes(res);

            long n = res.NumHexagons();

            for (var i = 0; i < n; i++)
            {
                bool isPentagon = cells[i].IsPentagon();
                var edges = cells[i].GetUniEdgesFromCell();

                for (var j = 0; j < 6; j++)
                {
                    if (isPentagon && j == 0)
                    {
                        continue;
                    }

                    callback(edges[j]);
                }
            }
        }

        

    }
}
