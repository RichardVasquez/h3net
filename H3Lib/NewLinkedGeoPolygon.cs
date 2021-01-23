using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Threading;

namespace H3Lib
{
    public class NewLinkedGeoPolygon
    {
        private readonly LinkedList<NewLinkedGeoLoop> _geoLoops;

        public int CountLoops => _geoLoops.Count;
        public int CountPolygons => TotalPolygons();
        public NewLinkedGeoLoop First => GetFirst();
        public NewLinkedGeoLoop Last => GetLast();

        public NewLinkedGeoPolygon Next;

        public ReadOnlyCollection<NewLinkedGeoPolygon> LinkedPolygons => GetPolygons();
        
        public NewLinkedGeoPolygon()
        {
            _geoLoops = new LinkedList<NewLinkedGeoLoop>();
        }

        public List<NewLinkedGeoLoop> Loops => _geoLoops.ToList();
        
        /// <summary>
        /// This is potentially dangerous, thus why it's
        /// a private method and provided as read only.
        /// </summary>
        private ReadOnlyCollection<NewLinkedGeoPolygon> GetPolygons()
        {
            List<NewLinkedGeoPolygon> temp = new List<NewLinkedGeoPolygon>();
            temp.Add(this);
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
        public NewLinkedGeoLoop AddNewLinkedLoop()
        {
            var loop = new NewLinkedGeoLoop();
            return AddLinkedLoop(loop);
        }

        /// <summary>
        /// Add an existing linked loop to the current polygon
        /// </summary>
        /// <param name="loop">Reference to loop</param>
        /// <returns></returns>
        public NewLinkedGeoLoop AddLinkedLoop(NewLinkedGeoLoop loop)
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
        public NewLinkedGeoPolygon AddNewLinkedGeoPolygon()
        {
            if (Next != null)
            {
                throw new Exception("polygon.Next must be null");
            }

            Next = new NewLinkedGeoPolygon();
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

        private NewLinkedGeoLoop GetFirst()
        {
            if (_geoLoops == null || _geoLoops.Count < 1)
            {
                return null;
            }

            return _geoLoops.First();
        }
        
        private NewLinkedGeoLoop GetLast()
        {
            if (_geoLoops == null || _geoLoops.Count < 1)
            {
                return null;
            }

            return _geoLoops.Last();
        }

    }
}
