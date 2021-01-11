using System;
using System.Collections.Generic;
using System.Linq;

namespace H3Lib
{
    /// <summary>
    /// A polygon node in a linked geo structure, part of a linked list.
    /// </summary>
    public class LinkedGeoPolygon
    {
        public LinkedGeoLoop First;
        public LinkedGeoLoop Last;
        public LinkedGeoPolygon Next;

        public LinkedList<LinkedGeoLoop> Loop = new LinkedList<LinkedGeoLoop>();
        
        public LinkedGeoPolygon()
        {
            First = null;
            Last = null;
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
        public LinkedGeoPolygon AddNew()
        {
            //  Can't add a new polygon to this one if Next is already established.
            if (Next != null)
            {
                return null;
            }
            Next = new LinkedGeoPolygon();
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
        public LinkedGeoLoop AddNewLoop()
        {
            var loop = new LinkedGeoLoop();
            Loop.AddLast(loop);
            return loop;
            //return AddLinkedLoop(new LinkedGeoLoop());
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
        public LinkedGeoLoop AddLinkedLoop(LinkedGeoLoop loop)
        {
            Loop.AddLast(loop);
            return loop;
            /*
            var last = Last;

            if (last == null)
            {
                if (First != null)
                {
                    throw new Exception("assert(polygon->first == NULL)");
                }

                First = loop;
            }
            else
            {
                Last.Next = loop;
            }

            //  TODO: Check to make sure we're not creating a memory leak.
            //  Above in the else condition, aren't we overwriting
            //  Last.Next with the following assignment?
            Last = loop;
            return loop;*/
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
            foreach (var geoLoop in Loop)
            {
                geoLoop.Clear();
            }
            Loop.Clear();
        }

        /// <summary>
        /// Going to try C# capabilities only in this.
        ///
        /// Count the number of polygons in a linked list
        /// </summary>
        /// <!--
        /// linkedGeo.c
        /// int countLinkedPolygons
        /// -->
        public int Count()
        {
            return Loop.Count;
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
            return Loop.Sum(geoLoop => geoLoop.Count);
        }
    }
}
