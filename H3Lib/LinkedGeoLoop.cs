using System;
using System.Collections.Generic;

namespace H3Lib
{
    /// TODO: Make sure all the LinkedList stuff works
    /// <summary>
    /// A loop node in a linked geo structure, part of a linked list
    /// </summary>
    public class LinkedGeoLoop
    {
        public LinkedGeoCoord First;
        public LinkedGeoCoord Last;
        public LinkedGeoLoop Next;

        public LinkedList<GeoCoord> Loop = new LinkedList<GeoCoord>();

        public int Count => Loop.Count;

        public bool IsEmpty => Loop.Count == 0;

        /// <summary>
        /// Add a new linked coordinate to the current loop
        /// </summary>
        /// <param name="vertex">Coordinate to add</param>
        /// <returns>Reference to the coordinate</returns>
        /// <exception cref="Exception">First/Last requirements conflict</exception>
        /// <!--
        /// linkedGeo.c
        /// LinkedGeoCoord* addLinkedCoord
        /// -->
        //public LinkedGeoCoord AddLinkedCoord(GeoCoord vertex)
        public void AddLinkedCoord(GeoCoord vertex)
        {
            Loop.AddLast(vertex);
            /*
            var coord = new LinkedGeoCoord() {Vertex = vertex, Next = null};
            var last = Last;
            if (last == null)
            {
                if (First == null)
                {
                    throw new Exception("assert(loop->first == NULL)");
                }

                First = coord;
            }
            else
            {
                Last.Next = coord;
            }
            //  TODO: Again check for memory leaks.
            Last = coord;
            return coord;*/
            
        }

        /// <summary>
        /// Free all allocated memory for a linked geo loop.
        /// </summary>
        /// <!--
        /// linkedGeo.c
        /// void destroyLinkedGeoLoop
        /// -->
        public void Clear()
        {
            Loop.Clear();
            LinkedGeoCoord nextCoord;
            for (var currentCoord = First;
                 currentCoord != null;
                 currentCoord = nextCoord)
            {
                nextCoord = currentCoord.Next;
                currentCoord = null;
            }
        }

        /// <summary>
        /// Count the number of coordinates in a loop
        /// </summary>
        /// <returns>Count</returns>
        /// <!--
        /// linkedGeo.c
        /// int countLinkedCoords
        /// -->
        public int CountNodes()
        {
            return Loop.Count;
            /*            var coord = First;
                        var count = 0;
                        while (coord != null)
                        {
                            count++;
                            coord = coord.Next;
                        }
                        return count;
                        */
        }
    }
}
