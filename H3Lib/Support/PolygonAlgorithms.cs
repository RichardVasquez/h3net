using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using H3Lib.Extensions;

namespace H3Lib.Support
{
    /// <summary>
    /// LinkedList based container.
    /// </summary>
    /// <typeparam name="T">Should be reference to its own class</typeparam>
    /// <typeparam name="TU">Should be reference to what it contains</typeparam>
    public abstract class PolygonAlgorithm<T,TU>
    {
        protected abstract void InitializeIteration();
        protected abstract void Iterate(PolygonAlgorithm<T,TU> loop, TU pointA, TU pointB);
        protected abstract bool IsEmpty();
        protected abstract bool IsPolyClockwise();
        protected abstract T GetNextCoord(TU current, TU next);

        public LinkedList<TU> LinkedData;

        protected double NormalizeLongitude(double longitude, bool isTransmeridian)
        {
            return isTransmeridian && longitude < 0
                       ? longitude + Constants.M_2PI
                       : longitude;
        }
        
        public bool PointInside(BBox box, GeoCoord coord)
        {
            // fail fast if we're outside the bounding box
            if (!box.Contains(coord))
            {
                return false;
            }

            bool isTransmeridian = box.IsTransmeridian;
            bool contains = false;

            double lat = coord.Latitude;
            double lng = NormalizeLongitude(coord.Longitude, isTransmeridian);

            GeoCoord a = default;
            GeoCoord b = default;

            InitializeIteration();


            while (true)
            {
                Iterate(this, a, b);
            }
            return false;
        }

        public BBox ToBBox()
        {
            return new BBox();
        }
        
        public bool IsClockwise(bool isTransmeridian)
        {
            return false;
        }
    }

    public class LinkedGeoLoop : PolygonAlgorithm<LinkedGeoLoop, LinkedGeoCoord>
    {
        private LinkedGeoCoord _currentCoord;
        private LinkedGeoCoord _nextCoord;
        
        protected override void InitializeIteration()
        {
            _currentCoord = null;
            _nextCoord = null;
        }

        protected override void Iterate
            (PolygonAlgorithm<LinkedGeoLoop, LinkedGeoCoord> loop, LinkedGeoCoord pointA, LinkedGeoCoord pointB)
        {
            // currentCoord = GET_NEXT_COORD(loop, currentCoord);    \
            // if (currentCoord == NULL) break;                      \
            // vertexA = currentCoord->vertex;                       \
            // nextCoord = GET_NEXT_COORD(loop, currentCoord->next); \
            // vertexB = nextCoord->vertex
            //
            // throw new System. N tImplementedException();
        }

        protected override bool IsEmpty()
        {
            throw new System.NotImplementedException();
        }

        protected override bool IsPolyClockwise()
        {
            throw new System.NotImplementedException();
        }

        protected override LinkedGeoLoop GetNextCoord(LinkedGeoLoop current, LinkedGeoLoop next)
        {
            throw new System.NotImplementedException();
        }

        
    }
}
