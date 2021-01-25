using System;
using System.Collections.Generic;
using System.Linq;

namespace H3Lib
{
    /// TODO: Make sure all the LinkedList stuff works
    /// <summary>
    /// A polygon node in a linked geo structure, part of a linked list.
    /// </summary>
    public class OldLinkedGeoPolygon
    {
        public OldLinkedGeoPolygon Next;

        public readonly LinkedList<OldLinkedGeoLoop> GeoLoopList;
        
        public bool IsEmpty => GeoLoopList.Count == 0;

        public OldLinkedGeoPolygon()
        {
            GeoLoopList = new LinkedList<OldLinkedGeoLoop>();
            Next = null;
        }

        /// <summary>
        /// Add a new polygon to the current polygon
        /// </summary>
        /// <returns>Reference to the new polygon</returns>
        /// <!--
        /// linkedGeo.c
        /// LinkedGeoPolygon* addNewLinkedPolygon
        /// -->
        /// <remarks>
        /// Going to try this with a slightly different approach to dodge pointers
        /// and ref parameters.
        /// </remarks>
        public OldLinkedGeoPolygon AddNew()
        {
            //  Can't add a new polygon to this one if Next is already established.
            if (Next != null)
            {
                return null;
            }
            Next = new OldLinkedGeoPolygon();
            return Next;
        }

        /// <summary>
        /// Add a new linked loop to the current polygon
        /// </summary>
        /// <returns>Reference to new loop</returns>
        /// <!--
        /// linkedGeo.c
        /// LinkedGeoLoop* addNewLinkedLoop
        /// -->
        public OldLinkedGeoLoop AddNewLoop()
        {
            var loop = new OldLinkedGeoLoop();
            GeoLoopList.AddLast(loop);
            return loop;
        }

        /// <summary>
        /// Add an existing linked loop to the current polygon
        /// </summary>
        /// <param name="loop">loop to add</param>
        /// <returns>Reference to loop</returns>
        /// <exception cref="Exception">First should be null if last is null</exception>
        /// <!--
        /// linkedGeo.c
        /// LinkedGeoLoop* addLinkedLoop
        /// -->
        public OldLinkedGeoLoop AddLinkedLoop(OldLinkedGeoLoop loop)
        {
            GeoLoopList.AddLast(loop);
            return loop;
        }

        /// <summary>
        /// Going to try C# capabilities only in this.
        ///
        /// Clear all the geoLoops, then clear the linked list.
        /// </summary>
        /// <!--
        /// linkedGeo.c
        /// void H3_EXPORT(destroyLinkedPolygon)
        /// -->
        public void Clear()
        {
            foreach (var geoLoop in GeoLoopList)
            {
                geoLoop.Clear();
            }
            GeoLoopList.Clear();
        }

        /// <summary>
        /// Count the number of polygons in a linked list, starting from this
        /// </summary>
        /// <!--
        /// linkedGeo.c
        /// countLinkedPolygons
        /// -->
        public int CountPolygons()
        {
            var sum = 1;
            var next = Next;
            while (next != null)
            {
                sum++;
                next = next.Next;
            }

            return sum;
        }
        
        /// <summary>
        /// Going to try C# capabilities only in this.
        ///
        /// Count the number of loops in all the polygons.
        /// </summary>
        /// <!--
        /// linkedGeo.c
        /// int countLinkedLoops
        /// -->
        public int CountLoops()
        {
            return GeoLoopList.Count;
        }
    }
}
