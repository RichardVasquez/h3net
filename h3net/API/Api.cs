using System;

namespace h3net.API
{
    public class Api
    {
        //  This will actually be a minor annoyance, but we'll collate the various class methods here
        //  to match the api.h function declarations, so where the C code might refer to geoToH3, and
        //  internally, the C# code during the first iteration of translation uses H3Index.geotoH3,
        //  The final result wil be something like Api.GeoToH3.

        //public ulong geoToH3(GeoCoord g, int res)
        //{
        //    return H3Index.geoToH3(ref g, res);
        //}

        //public void h3ToGeo(H3Index h3, GeoCoord g)
        //{
        //    ret
        //}

        //public void h3ToGeoBoundary(ulong h3,  GeoBoundary gp)
        //{
        //}

        //public int maxKringSize(int k)
        //{
        //    return 0;
        //}

        //public int hexRange(ulong origin, int k, ulong outhex)
        //{
        //    return 0;
        //}

        //public int hexRangeDistances(ulong origin, int k, ulong outindex, int distances)
        //{
        //    return 0;
        //}

        //public int hexRanges(ulong h3set, int length, int k, ulong outhex)
        //{
        //    return 0;
        //}

        //public void kRing(ulong origin, int k, ulong outhex)
        //{

        //}

        //public void kRingDistances(ulong origin, int k, ulong outhex, int distances)
        //{

        //}

        //public int hexRing(ulong origin, int k, ulong outhex)
        //{
        //    return 0;
        //}

        //public int maxPolyfillSize(GeoPolygon geoPolygon, int res)
        //{
        //    return 0;
        //}

        //public void polyfill(GeoPolygon geoPolygon, int res, ulong outhex)
        //{
        //}

        //public void h3SetToLinkedGeo(ulong h3Set, int numHexes, LinkedGeoPolygon outhex)
        //{
        //}

        //public void destroyLinkedPolygon(LinkedGeoPolygon polygon)
        //{
        //}

        //public double degsToRads(double degrees)
        //{
        //    return 0.0;
        //}

        //public double radsToDegs(double radians)
        //{
        //    return 0.0;}

        //public double hexAreaKm2(int res)
        //{
        //    return 0.0;}

        //public double hexAreaM2(int res)
        //{
        //    return 0.0;
        //}

        //public double edgeLengthKm(int res)
        //{
        //    return 0.0;
        //}

        //public double edgeLengthM(int res)
        //{
        //    return 0.0;}

        //public long numHexagons(int res)
        //{
        //    return 0;
        //}
        //public int h3GetResolution(ulong h)
        //{
        //    return 0;
        //}

        //public int h3GetBaseCell(ulong h)
        //{
        //    return 0;
        //}

        //public ulong stringToH3(char instr)
        //{
        //    return 0;
        //}

        //public void h3ToString(ulong h, char str, UIntPtr sz)
        //{
        //}

        //public int h3IsValid(ulong h)
        //{
        //    return 0;

        //}

        //public ulong h3ToParent(ulong h, int parentRes)
        //{
        //    return 0;

        //}

        //public int maxH3ToChildrenSize(ulong h, int childRes)
        //{
        //    return 0;

        //}

        //public void h3ToChildren(ulong h, int childRes, ulong children)
        //{
        //}

        //public int compact(ulong h3Set, ulong compactedSet, int numHexes)
        //{
        //    return 0;
        //}

        //public int maxUncompactSize(ulong compactedSet, int numHexes,int res)
        //{
        //    return 0;
        //}

        //public int uncompact(ulong compactedSet, int numHexes, ulong h3Set, int maxHexes, int res)
        //{
        //    return 0;
        //}

        //public int h3IsResClassIII(ulong h)
        //{
        //    return 0;
        //}

        //public static  int h3IsPentagon(ulong h)
        //{
        //    return 0;
        //}

        //public int h3IndexesAreNeighbors(ulong origin, ulong destination)
        //{
        //    return 0;
        //}

        //public ulong getH3UnidirectionalEdge(ulong origin, ulong destination)
        //{
        //    return 0;
        //}

        //public int h3UnidirectionalEdgeIsValid(ulong edge)
        //{
        //    return 0;
        //}

        //public ulong getOriginH3IndexFromUnidirectionalEdge(ulong edge)
        //{
        //    return 0;
        //}

        //public ulong getDestinationH3IndexFromUnidirectionalEdge(ulong edge){return 0;}

        //public void getH3IndexesFromUnidirectionalEdge(ulong edge, ulong originDestination)
        //{
        //}

        //public void getH3UnidirectionalEdgesFromHexagon(ulong origin, ulong edges)
        //{
        //}

        //public void getH3UnidirectionalEdgeBoundary(ulong edge, GeoBoundary gb)
        //{
        //}

        //public int h3Distance(ulong origin, ulong h3)
        //{
        //    return 0;
        //}
    }
}
