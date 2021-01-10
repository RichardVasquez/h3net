using System;

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
        public LinkedGeoLoop AddNewLoop()
        {
            return AddLinkedLoop(new LinkedGeoLoop());
        }

        /// <summary>
        /// Add an existing linked loop to the current polygon
        /// </summary>
        /// <param name="loop">loop to add</param>
        /// <returns>Reference to loop</returns>
        /// <exception cref="Exception">First should be null if last is null</exception>
        public LinkedGeoLoop AddLinkedLoop(LinkedGeoLoop loop)
        {
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
            return loop;
        }
    }
}
