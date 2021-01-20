using System.Collections.Generic;
using System.Linq;

namespace H3Lib.Support
{
    public class MyLinkedPolygon
    {
        private LinkedList<MyLinkedGeoLoop> _loops;
        private MyLinkedPolygon _next = null;

        /// <summary>
        /// Count the number of polygons in a linked list
        /// </summary>
        public int CountPolygons => CountAllPolygons();
        
        /// <summary>
        /// Count the number of linked loops in a polygon
        /// </summary>
        public int CountLoops => _loops?.Count ?? -1;

        public MyLinkedGeoLoop First => 
            _loops == null
                ? null 
                : _loops.Count == 0
                    ? null 
                    : _loops.First();

        public List<MyLinkedGeoLoop> AllLoops()
        {
            List<MyLinkedGeoLoop> result = new List<MyLinkedGeoLoop>();
            if (CountLoops <= 0)
            {
                return result;
            }

            result.AddRange(_loops);
            return result;
        }
        
        /// <summary>
        /// Helper to grab all the linked polygons starting from
        /// the designated polygon;
        /// </summary>
        /// <returns></returns>
        public List<MyLinkedPolygon> AllPolygons()
        {
            List<MyLinkedPolygon> result = new List<MyLinkedPolygon> {this};

            var next = _next;
            while (next != null)
            {
                result.Add(next);
                next = next._next;
            }

            return result;
        }

        /// <summary>
        /// Add a linked polygon to the current polygon
        /// </summary>
        /// <param name="polygon">Polygon to add link to</param>
        /// <returns>Reference to added polygon</returns>
        public MyLinkedPolygon AddPolygon(MyLinkedPolygon polygon)
        {
            var next = _next;
            while (next != null)
            {
                next = next._next;
            }

            if (next != null)
            {
                next._next = polygon;
            }

            return polygon;
        }
        
        /// <summary>
        /// Add a new empty linked loop to the current polygon
        /// </summary>
        public MyLinkedGeoLoop AddNewLoop()
        {
            var loop = new MyLinkedGeoLoop();
            return AddLoop(loop);
        }
        
        /// <summary>
        /// Add an existing linked loop to the current polygon
        /// </summary>
        /// <param name="loop">Loop to add to polygon</param>
        public MyLinkedGeoLoop AddLoop(MyLinkedGeoLoop loop)
        {
            _loops ??= new LinkedList<MyLinkedGeoLoop>();
            _loops.AddLast(loop);
            return loop;
        }

        /// <summary>
        /// Cleans up the contents of the linked loops
        /// </summary>
        public void Clear()
        {
            //  Hop to the end
            while (_next != null)
            {
                _next.Clear();
                _next = null;
            }

            //  There's no more, so let's clean up.
            if (_loops == null)
            {
                return;
            }
            foreach (var loop in _loops)
            {
                loop.Clear();
            }

            _loops = null;
        }

        /// <summary>
        /// Alias for <see cref="Clear"/>
        /// </summary>
        public void Destroy()
        {
            Clear();
        }

        private int CountAllPolygons()
        {
            int count = 1;
            var next = _next;
            while (next != null)
            {
                count++;
                next = next._next;
            }

            return count;
        }

    }
}
