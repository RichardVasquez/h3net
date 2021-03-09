using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Threading;

namespace H3Lib
{
    /// <summary>
    /// A polygon node in a linked geo structure, part of a linked list.
    /// </summary>
    public class LinkedGeoPolygon
    {
        /// <summary>
        /// Linked list of loops that make up the polygon
        /// </summary>
        private readonly LinkedList<LinkedGeoLoop> _geoLoops;

        /// <summary>
        /// Count of loops in polygon
        /// </summary>
        public int CountLoops => _geoLoops.Count;
        /// <summary>
        /// Gets the count of polygons associated
        /// </summary>
        public int CountPolygons => TotalPolygons();
        /// <summary>
        /// Returns reference to the first loop
        /// </summary>
        public LinkedGeoLoop First => GetFirst();
        /// <summary>
        /// Returns reference to the last loop
        /// </summary>
        public LinkedGeoLoop Last => GetLast();

        /// <summary>
        /// Returns reference to next polygon
        /// </summary>
        public LinkedGeoPolygon Next;

        /// <summary>
        /// Returns all linked polygons to this one as a linear list
        /// </summary>
        public ReadOnlyCollection<LinkedGeoPolygon> LinkedPolygons => GetPolygons();
        
        /// <summary>
        /// Constructor
        /// </summary>
        public LinkedGeoPolygon()
        {
            _geoLoops = new LinkedList<LinkedGeoLoop>();
        }

        /// <summary>
        /// Linear list of all loops in this specific polygon
        /// </summary>
        public List<LinkedGeoLoop> Loops => _geoLoops.ToList();
        
        /// <summary>
        /// This is potentially dangerous, thus why it's
        /// a private method and provided as read only.
        /// </summary>
        private ReadOnlyCollection<LinkedGeoPolygon> GetPolygons()
        {
            var temp = new List<LinkedGeoPolygon> {this};
            var next = Next;
            while (next != null)
            {
                temp.Add(next);
                next = next.Next;
            }

            return temp.AsReadOnly();
        }
        
        /// <summary>
        /// Add a new linked loop to the current polygon
        /// </summary>
        /// <returns>Reference to loop</returns>
        public LinkedGeoLoop AddNewLinkedLoop()
        {
            var loop = new LinkedGeoLoop();
            return AddLinkedLoop(loop);
        }

        /// <summary>
        /// Add an existing linked loop to the current polygon
        /// </summary>
        /// <param name="loop">Reference to loop</param>
        /// <returns></returns>
        public LinkedGeoLoop AddLinkedLoop(LinkedGeoLoop loop)
        {
            _geoLoops.AddLast(loop);
            return loop;
        }

        /// <summary>
        /// <see cref="Clear"/>
        /// </summary>
        public void Destroy()
        {
            Clear();
        }

        /// <summary>
        /// Free all the geoloops and propagate to the next polygon until
        /// there's no more polygons.
        /// </summary>
        public void Clear()
        {
            foreach (var loop in _geoLoops)
            {
                loop.Clear();
            }

            Next?.Clear();
            Next = null;
        }

        /// <summary>
        /// Add a newly constructed polygon to current polygon.
        /// </summary>
        /// <returns>Reference to new polygon</returns>
        /// <exception cref="Exception"></exception>
        public LinkedGeoPolygon AddNewLinkedGeoPolygon()
        {
            if (Next != null)
            {
                throw new Exception("polygon.Next must be null");
            }

            Next = new LinkedGeoPolygon();
            return Next;
        }
        
        /// <summary>
        /// Count the number of polygons in a linked list
        /// </summary>
        private int TotalPolygons()
        {
            var count = 1;
            var next = Next;
            while (next != null)
            {
                count++;
                next = next.Next;
            }

            return count;
        }

        /// <summary>
        /// Returns first loop if any exist, null otherwise
        /// </summary>
        /// <returns></returns>
        private LinkedGeoLoop GetFirst()
        {
            if (_geoLoops == null || _geoLoops.Count < 1)
            {
                return null;
            }

            return _geoLoops.First();
        }
        
        /// <summary>
        /// Gets last loop in polygon, null if none exist.
        /// </summary>
        /// <returns></returns>
        private LinkedGeoLoop GetLast()
        {
            if (_geoLoops == null || _geoLoops.Count < 1)
            {
                return null;
            }

            return _geoLoops.Last();
        }

    }
}
