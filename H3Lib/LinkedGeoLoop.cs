using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using H3Lib.Extensions;

namespace H3Lib
{
    public class LinkedGeoLoop
    {
        private LinkedList<LinkedGeoCoord> Loop;
        public int Count => Loop.Count;

        public List<LinkedGeoCoord> Nodes => CopyNodes();

        public LinkedGeoCoord First => GetFirst();

        public bool IsEmpty => Loop == null || Loop.Count == 0;

        public LinkedGeoLoop()
        {
            Loop = new LinkedList<LinkedGeoCoord>();
        }
        
        private List<LinkedGeoCoord> CopyNodes()
        {
            if (Loop == null || Loop.Count == 0)
            {
                return new List<LinkedGeoCoord>();
            }

            var temp = Enumerable
                      .Range(1, Loop.Count)
                      .Select(s => new LinkedGeoCoord())
                      .ToArray();
            Loop.CopyTo(temp, 0);
            return temp.ToList();
        }

        /// <summary>
        /// Add a new linked coordinate to the current loop
        /// </summary>
        /// <param name="vertex">Coordinate to add</param>
        /// <returns>Reference to the coordinate</returns>
        /// <!--
        /// linkedGeo.c
        /// LinkedGeoCoord* addLinkedCoord
        /// -->
        public LinkedGeoCoord AddLinkedCoord(GeoCoord vertex)
        {
            var coord = new LinkedGeoCoord(vertex);
            Loop.AddLast(coord);
            return coord;
        }

        /// <summary>
        /// Clears the list of coords
        /// </summary>
        public void Clear()
        {
            Loop.Clear();
        }
        
        /// <summary>
        /// Clears the list of coords
        /// </summary>
        public void Destroy()
        {
            Clear();
        }
        
        private LinkedGeoCoord GetFirst()
        {
            if (Loop == null || Loop.Count < 1)
            {
                return null;
            }

            return Loop.First?.Value;
        }

    }
}
