using System;
using System.Collections.Generic;
using System.Linq;

namespace H3Lib.Support
{
    public class MyLinkedGeoLoop:PolygonAlgorithm<MyLinkedGeoLoop, GeoCoord>, IEquatable<MyLinkedGeoLoop>
    {
#region PolygonAlgorithms variables
        private LinkedGeoCoord currentCoord;
        private LinkedGeoCoord nextCoord;
#endregion
                
        
        
        
        /// <summary>
        /// Where the GeoCoords are stored
        /// </summary>
        private LinkedList<MyLinkedGeoCoord> _coords;

        public int Count => _coords?.Count ?? -1;

        /// <summary>
        /// Add a new linked coordinate to the current loop
        /// </summary>
        /// <param name="gc">Coordinate to add</param>
        /// <returns>Reference to the coordinate</returns>
        public MyLinkedGeoCoord Add(GeoCoord gc)
        {
            var lgc = new MyLinkedGeoCoord(gc);

            _coords ??= new LinkedList<MyLinkedGeoCoord>();

            _coords.AddLast(lgc);
            return lgc;
        }

        /// <summary>
        /// Clears the LinkedList of all <see cref="MyLinkedGeoCoord"/> contents
        /// </summary>
        public void Clear()
        {
            _coords.Clear();
            _coords = null;
        }
        
        /// <summary>
        /// Alias for <see cref="Clear"/>
        /// </summary>
        public void Destroy()
        {
            Clear();            
        }

        /// <summary>
        /// Count the number of polygons containing a given loop.
        /// </summary>
        /// <param name="polygons">Polygons to test</param>
        /// <param name="boxes">
        /// Bounding boxes for polygons, used in point-in-poly check
        /// </param>
        /// <returns>Number of polygons containing the loop</returns>
        public void CountContainers(
                IList<MyLinkedPolygon> polygons,
                IList<BBox> boxes
            )
        {
            int containerCount = 0;
            for (int i = 0; i < polygons.Count; i++)
            {
                if(this=polygons[i].First &&
                        pointInsideLinkedGeoLoop(polygons[i]->first, bboxes[i],
                                                 &loop->first->vertex)) {
                    containerCount++;
                }
            }
            return containerCount;

        }

        /// <summary>
        /// A pretty expensive equality test, goes through each vertex
        /// to check its equality.
        /// </summary>
        public bool Equals(MyLinkedGeoLoop other)
        {
            if (ReferenceEquals(null, other))
                return false;
            if (ReferenceEquals(this, other))
                return true;

            //  nulls are never equal to anything.
            if (other._coords == null || _coords == null)
            {
                return false;
            }

            if (other._coords.Count != _coords.Count)
            {
                return false;
            }

            var me = Enumerable.Range(1, _coords.Count)
                               .Select(s => new MyLinkedGeoCoord())
                               .ToArray();            
            
            var them = Enumerable.Range(1, other._coords.Count)
                                 .Select(s => new MyLinkedGeoCoord())
                                 .ToArray();            

            _coords.CopyTo(me,0);
            other._coords.CopyTo(them,0);

            for (var i = 0; i < _coords.Count; i++)
            {
                if (me[i].Vertex != them[i].Vertex)
                {
                    return false;
                }
            }

            return true;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            return obj.GetType() == GetType() && Equals((MyLinkedGeoLoop) obj);
        }

        public override int GetHashCode()
        {
            // ReSharper disable once NonReadonlyMemberInGetHashCode
            return _coords?.GetHashCode() ?? 0;
        }

        public static bool operator ==(MyLinkedGeoLoop left, MyLinkedGeoLoop right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(MyLinkedGeoLoop left, MyLinkedGeoLoop right)
        {
            return !Equals(left, right);
        }

        protected override void InitializeIteration()
        {
            throw new NotImplementedException();
        }

        protected override void Iterate(PolygonAlgorithm<MyLinkedGeoLoop, GeoCoord> loop, GeoCoord pointA, GeoCoord pointB)
        {
            currentCoord = GetNextCoord(loop, currentCoord);
            
            currentCoord = GET_NEXT_COORD(loop, currentCoord);    \
            if (currentCoord == NULL) break;                      \
            vertexA = currentCoord->vertex;                       \
            nextCoord = GET_NEXT_COORD(loop, currentCoord->next); \
            vertexB = nextCoord->vertex

            
            
            
            throw new NotImplementedException();
        }

        protected override bool IsEmpty()
        {
            throw new NotImplementedException();
        }

        protected override bool IsPolyClockwise()
        {
            throw new NotImplementedException();
        }

        protected override MyLinkedGeoLoop GetNextCoord(MyLinkedGeoLoop current, MyLinkedGeoLoop next)
        {
            throw new NotImplementedException();
        }

    }
}
