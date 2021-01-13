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
        public LinkedList<GeoCoord> GeoCoordList;

        /// <summary>
        /// Count the number of coordinates in a loop
        /// </summary>
        /// <returns>Count</returns>
        /// <!--
        /// linkedGeo.c
        /// int countLinkedCoords
        /// -->
        public int CountCoords => GeoCoordList.Count;

        public bool IsEmpty => GeoCoordList.Count == 0;

        public LinkedGeoLoop()
        {
            GeoCoordList = new LinkedList<GeoCoord>();
        }
        
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
            GeoCoordList.AddLast(vertex);
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
            GeoCoordList.Clear();
        }
    }
}
