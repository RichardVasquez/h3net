using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using H3Lib.Extensions;

namespace H3Lib
{
    /// <summary>
    /// A loop node in a linked geo structure, part of a linked list 
    /// </summary>
    public class LinkedGeoLoop
    {
        /// <summary>
        /// Linked list that stores the vertices 
        /// </summary>
        private LinkedList<LinkedGeoCoord> Loop;
        
        /// <summary>
        /// Counts how many vetices in this loop
        /// </summary>
        public int Count => Loop.Count;

        /// <summary>
        /// Presents a copy of the vertices in a linear list 
        /// </summary>
        public List<LinkedGeoCoord> Nodes => CopyNodes();

        /// <summary>
        /// Gets the first vertex in the list
        /// </summary>
        public LinkedGeoCoord First => GetFirst();

        /// <summary>
        /// Indicates if there's any vertices in the loop
        /// </summary>
        public bool IsEmpty => Loop == null || Loop.Count == 0;

        /// <summary>
        /// Constructor
        /// </summary>
        public LinkedGeoLoop()
        {
            Loop = new LinkedList<LinkedGeoCoord>();
        }
        
        /// <summary>
        /// Makes a copy of the vertices in the loop 
        /// </summary>
        /// <returns></returns>
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
        
        /// <summary>
        /// Returns first vertex or null if there are none.
        /// </summary>
        /// <returns></returns>
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
