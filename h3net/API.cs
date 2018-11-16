using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography;
using System.Text;
using H3Net.Code;

namespace H3Net
{
    public static partial class Api
    {
        private static List<Code.H3Index> MakeEmpty(int len)
        {
            return new Code.H3Index[len]
                  .Select(v => (Code.H3Index) 0).ToList();
        }

        public static Api.H3Index GeoToH3(Code.GeoCoord gc, int res)
        {
            var h3 = Code.H3Index.geoToH3(ref gc, res);
            return new H3Index {Value = h3.value};
        }

        public static Api.GeoCoord H3ToGeo(Code.H3Index h3)
        {
            var blank = new Code.GeoCoord();
            Code.H3Index.h3ToGeo(h3, ref blank);
            return new GeoCoord {Latitude = blank.lat, Longitude = blank.lon};
        }

        public static Api.GeoBoundary H3ToGeoBoundary(Code.H3Index h3)
        {
            var blank = new Code.GeoBoundary();
            Code.H3Index.h3ToGeoBoundary(h3, ref blank);

            var newVerts = blank.verts.Select(v => new Api.GeoCoord(v)).ToArray();

            return new GeoBoundary
                   {
                       VertexCount = blank.numVerts,
                       Vertices = newVerts
                   };
        }

        public static int MaxKringSize(int k)
        {
            return Code.Algos.maxKringSize(k);
        }

        public static HexRangeResult HexRange(Code.H3Index origin, int k)
        {
            var temp = MakeEmpty(Algos.maxKringSize(k));
            var result = Algos.hexRange(origin, k, ref temp);
            return new HexRangeResult
                   {
                       Result = result,
                       Indexes = temp.Select(t=>new Api.H3Index {Value = t.value}).ToArray()
                   };
        }

        public static HexRangeDistancesResult HexRangeDistances(Code.H3Index origin, int k)
        {
            int maxSize =  Algos.maxKringSize(k);
            var outHexes = MakeEmpty(maxSize);
            var distances = new int[maxSize].ToList();

            var result = Algos.hexRangeDistances(origin, k, ref outHexes, ref distances);
            List<HexRangeMeasurement> combiner = new List<HexRangeMeasurement>();
            for (int i = 0; i < maxSize; i++)
            {
                combiner.Add
                    (
                     new HexRangeMeasurement
                     {
                         Index = new H3Index {Value = outHexes[i].value},
                         Distance = distances[i]
                     }
                    );
            }

            return new HexRangeDistancesResult
                   {
                       Result = result,
                       Values = combiner.ToArray()
                   };
        }

        public static HexRangeResult HexRanges(IEnumerable<Code.H3Index> h3Set, int k)
        {
            var hexes = h3Set.ToList();
            int maxSize = Algos.maxKringSize(k);
            var outHexes = MakeEmpty(maxSize);

            var result = Algos.hexRanges(ref hexes, maxSize, k, outHexes);

            return new HexRangeResult
                   {
                       Result = result,
                       Indexes = outHexes.Select(v => new Api.H3Index {Value = v.value}).ToArray()
                   };
        }

        public static HexRangeResult HexRanges(Code.H3Index h3Set, int k)
        {
            var tempList = new List<Code.H3Index> {h3Set};
            return HexRanges(tempList, k);
        }

        public static H3Index[] Kring(Code.H3Index origin, int k)
        {
            int maxSize = Algos.maxKringSize(k);
            var outHexes = MakeEmpty(maxSize);
            Algos.kRing(origin, k, ref outHexes);

            return outHexes.Select(v => new H3Index {Value = v.value}).ToArray();
        }

        public static HexRangeDistancesResult KringDistances(Code.H3Index origin, int k)
        {
            int maxSize = Algos.maxKringSize(k);
            var outHexes = MakeEmpty(maxSize);
            var distances = new int[maxSize].ToList();
            Algos.kRingDistances(origin,k,ref outHexes, ref distances);
            List<HexRangeMeasurement> combiner = new List<HexRangeMeasurement>();
            for (int i = 0; i < maxSize; i++)
            {
                combiner.Add
                    (
                     new HexRangeMeasurement
                     {
                         Index = new H3Index {Value = outHexes[i].value},
                         Distance = distances[i]
                     }
                    );
            }

            return new HexRangeDistancesResult
                   {
                       Result = 0,
                       Values = combiner.ToArray()
                   };
        }

        public static HexRangeResult HexRing(Code.H3Index origin, int k)
        {
            int maxSize = (k < 1)
                              ? 1
                              : k * 6;
            var outHexes = MakeEmpty(maxSize);

            var result = Algos.hexRing(origin, k, ref outHexes);
            return new HexRangeResult
                   {
                       Result = result,
                       Indexes = outHexes.Select(v => new H3Index {Value = v.value}).ToArray()
                   };
        }

        public static int MaxPolyFillSize(Code.GeoPolygon geoPolygon, int res)
        {
            return Code.Algos.maxPolyfillSize(ref geoPolygon, res);
        }

        public static H3Index[] PolyFill(Code.GeoPolygon geoPolygon, int res)
        {
            var maxSize = MaxPolyFillSize(geoPolygon, res);
            var outHexes = MakeEmpty(maxSize);

            Code.Algos.polyfill(geoPolygon, res, outHexes);
            return outHexes.Select(v => new H3Index {Value = v.value}).ToArray();
        }

        public static LinkedGeoPolygon H3SetToLinkedGeo(IEnumerable<Code.H3Index> h3Set)
        {
            var hexes = h3Set.ToList();
            var polygon = new LinkedGeo.LinkedGeoPolygon();
            Algos.h3SetToLinkedGeo(ref hexes, hexes.Count, ref polygon);
            return new LinkedGeoPolygon(polygon);
        }

        public static void DestroyLinkedPolygon(ref LinkedGeoPolygon polygon)
        {
            polygon.Clear();
            polygon = null;
        }

        public static double DegreesToRadians(double degrees)
        {
            return Code.GeoCoord.degsToRads(degrees);
        }

        public static double RadiansToDegrees(double radians)
        {
            return Code.GeoCoord.radsToDegs(radians);
        }

        public static double HexAreaKm2(int res)
        {
            return Code.GeoCoord.hexAreaKm2(res);
        }

        public static double HexAreaM2(int res)
        {
            return Code.GeoCoord.hexAreaM2(res);
        }

        public static double EdgeLengthKm(int res)
        {
            return Code.GeoCoord.edgeLengthKm(res);
        }

        public static double EdgeLengthM(int res)
        {
            return Code.GeoCoord.edgeLengthM(res);
        }

        public static long NumberHexagons(int res)
        {
            return Code.GeoCoord.numHexagons(res);
        }

        public static int H3GetResolution(Code.H3Index h3)
        {
            return Code.H3Index.h3GetResolution(h3);
        }

        public static int H3GetBaseCell(Code.H3Index h3)
        {
            return Code.H3Index.h3GetBaseCell(h3);
        }

        public static H3Index StringToH3(string s)
        {
            return new H3Index
                   {
                       Value = Code.H3Index.stringToH3(s).value
                   };
        }

        public static string H3ToString(Code.H3Index h3)
        {
            string s = "";
            Code.H3Index.h3ToString(h3, ref s, 17);
            return s;
        }

        public static int H3IsValid(Code.H3Index h3)
        {
            return Code.H3Index.h3IsValid(h3);
        }

        public static H3Index H3ToParent(Code.H3Index h3, int parentRes)
        {
            return new H3Index
                   {Value = Code.H3Index.h3ToParent(h3, parentRes)};
        }

        public static int MaxH3ToChildrenSize(Code.H3Index h3, int childRes)
        {
            return Code.H3Index.maxH3ToChildrenSize(h3, childRes);
        }

        public static H3Index[] H3ToChildren(Code.H3Index h3, int childRes)
        {
            var maxSize = MaxH3ToChildrenSize(h3, childRes);
            var children = MakeEmpty(maxSize);
            Code.H3Index.h3ToChildren(h3, childRes,ref children);
            return children.Select(v => new H3Index {Value = v.value}).ToArray();
        }

        public static HexRangeResult Compact(List<Code.H3Index> h3Set)
        {
            var empty = MakeEmpty(h3Set.Count);
            var result = Code.H3Index.compact(ref h3Set, ref empty, empty.Count);

            return new HexRangeResult
                   {
                       Result = result,
                       Indexes = empty.Select(v => new H3Index {Value = v.value}).ToArray()
                   };
        }

        public static int MaxUncompactSize(List<Code.H3Index> h3Set, int res)
        {
            return Code.H3Index.maxUncompactSize(ref h3Set, h3Set.Count, res);
        }

        public static HexRangeResult Uncompact(List<Code.H3Index> compactedSet, int res)
        {
            var maxUncompact = MaxUncompactSize(compactedSet, res);
            var emptySet = MakeEmpty(maxUncompact);

            var result = Code.H3Index.uncompact(ref compactedSet, compactedSet.Count, ref emptySet, maxUncompact, res);
            return new HexRangeResult
                   {
                       Result = result,
                       Indexes = emptySet.Select(v => new H3Index {Value = v.value}).ToArray()
                   };
        }

        public static int H3IsResClassIII(Code.H3Index h3)
        {
            return Code.H3Index.h3IsResClassIII(h3);
        }

        public static bool IsResClassIII(Code.H3Index h3)
        {
            return H3IsResClassIII(h3) != 0;
        }

        public static int H3IsPentagon(Code.H3Index h3)
        {
            return Code.H3Index.h3IsPentagon(h3);
        }

        public static bool IsPentagon(Code.H3Index h3)
        {
            return H3IsPentagon(h3) != 0;
        }

        public static int H3IndexesAreNeighbors(Code.H3Index origin, Code.H3Index destination)
        {
            return Code.H3UniEdge.h3IndexesAreNeighbors(origin, destination);
        }

        public static bool AreNeighbors(Code.H3Index origin, Code.H3Index destination)
        {
            return H3IndexesAreNeighbors(origin, destination) != 0;
        }

        public static H3Index GetUniDirectionalEdge(Code.H3Index origin, Code.H3Index destination)
        {
            return new H3Index
                   {
                       Value = H3UniEdge.getH3UnidirectionalEdge(origin, destination).value
                   };
        }

        public static int H3UniDirectionalEdgeIsValid(Code.H3Index edge)
        {
            return H3UniEdge.h3UnidirectionalEdgeIsValid(edge);
        }

        public static bool UniDirectionalEdgeIsValid(Code.H3Index edge)
        {
            return H3UniDirectionalEdgeIsValid(edge) != 0;
        }

        public static H3Index GetOriginH3FromUniDirectionalEdge(Code.H3Index edge)
        {
            return new H3Index
                   {
                       Value = H3UniEdge.getOriginH3IndexFromUnidirectionalEdge(edge).value
                   };
        }

        public static H3Index GetDestinationH3FromUniDirectionalEdge(Code.H3Index edge)
        {
            return new H3Index
                   {
                       Value = H3UniEdge.getDestinationH3IndexFromUnidirectionalEdge(edge)
                   };
        }

        public static H3Index[] GetH3IndexesFromUnidirectionalEdge(Code.H3Index edge)
        {
            List<Code.H3Index> cells = new List<Code.H3Index>{0, 0};
            H3UniEdge.getH3IndexesFromUnidirectionalEdge(edge, ref cells);
            return cells.Select(v => new H3Index {Value = v.value}).ToArray();
        }

        public static H3Index[] GetH3UniDirectionalEdgesFromHexagon(Code.H3Index origin)
        {
            var cells = MakeEmpty(6);
            H3UniEdge.getH3UnidirectionalEdgesFromHexagon(origin, cells);
            return cells.Where(v => v != 0)
                        .Select(v => new H3Index {Value = v.value})
                        .ToArray();
        }

        public static GeoBoundary GetH3UnidirectionalEdgeBoundary(Code.H3Index edge)
        {
            Code.GeoBoundary gb= new Code.GeoBoundary();
            H3UniEdge.getH3UnidirectionalEdgeBoundary(edge, ref gb);
            var newVerts = gb.verts.Select(v => new GeoCoord(v)).ToArray();
            return new GeoBoundary
                   {
                       VertexCount = gb.numVerts,
                       Vertices = newVerts
                   };
        }

        public static int H3Distance(Code.H3Index origin, Code.H3Index h3)
        {
            return LocalIJ.h3Distance(origin, h3);
        }

        public static ExperimentalIJ ExperimentalH3ToLocalIj(Code.H3Index origin, Code.H3Index h3)
        {
            LocalIJ.CoordIJ ij = new LocalIJ.CoordIJ();
            int result = LocalIJ.experimentalH3ToLocalIj(origin, h3, ij);
            return new ExperimentalIJ
                   {
                       Result = result,
                       IJ = new CoordIJ {I = ij.i, J = ij.j}
                   };
        }

        public static HexRangeResult experimentalLocalIjToH3(Code.H3Index origin, Code.LocalIJ.CoordIJ ij)
        {
            Code.H3Index h3 = new Code.H3Index(0);
            var result = LocalIJ.experimentalLocalIjToH3(origin, ij, ref h3);
            return new HexRangeResult
                   {
                       Result = result,
                       Indexes = new H3Index[] {new H3Index {Value = h3.value}}
                   };

        }
    }
}
