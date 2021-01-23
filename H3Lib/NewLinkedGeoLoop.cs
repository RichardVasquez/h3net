using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using H3Lib.Extensions;

namespace H3Lib
{
    public class NewLinkedGeoLoop
    {
        private LinkedList<NewLinkedGeoCoord> Loop;
        public int Count => Loop.Count;

        public List<NewLinkedGeoCoord> Nodes => CopyNodes();

        public NewLinkedGeoCoord First => GetFirst();

        public bool IsEmpty => Loop == null || Loop.Count == 0;

        public NewLinkedGeoLoop()
        {
            Loop = new LinkedList<NewLinkedGeoCoord>();
        }
        
        private List<NewLinkedGeoCoord> CopyNodes()
        {
            if (Loop == null || Loop.Count == 0)
            {
                return new List<NewLinkedGeoCoord>();
            }

            var temp = Enumerable
                      .Range(1, Loop.Count)
                      .Select(s => new NewLinkedGeoCoord())
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
        public NewLinkedGeoCoord AddLinkedCoord(GeoCoord vertex)
        {
            var coord = new NewLinkedGeoCoord(vertex);
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
        
        private NewLinkedGeoCoord GetFirst()
        {
            if (Loop == null || Loop.Count < 1)
            {
                return null;
            }

            return Loop.First?.Value;
        }

    }
}
