using System;
using System.Collections.Generic;
using System.Linq;
using H3Lib;
using H3Lib.Extensions;
using NUnit.Framework;

namespace Tests
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
            IterateAllIndexesAtResPartial(res, callback, H3Lib.Constants.NUM_BASE_CELLS);
        }

        /// <summary>
        /// Call the callback for every index at the given resolution in base
        /// cell 0 up to the given base cell number.
        /// </summary>
        /// <param name="res"></param>
        /// <param name="callback"></param>
        /// <param name="baseCells"></param>
        private static void IterateAllIndexesAtResPartial(int res, Action<H3Index> callback, int baseCells)
        {
            Assert.LessOrEqual(baseCells, Constants.NUM_BASE_CELLS);
            for (var i = 0; i < baseCells; i++)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"IterateAllIndexesAtRes: RES: {res} BASECELL: {i}");
                Console.ForegroundColor = ConsoleColor.Gray;
                IterateBasCellIndexesAtRes(res, callback, i);
            }

        }

        /// <summary>
        /// Call the callback for every index at the given resolution in a
        /// specific base cell
        /// </summary>
        private static void IterateBasCellIndexesAtRes(int res, Action<H3Index> callback, int baseCell)
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
    }
}
