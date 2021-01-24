using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using H3Lib.Extensions;

namespace H3Lib
{
    public static class Api
    {
        /// <summary>
        /// find the H3 index of the resolution res cell containing the lat/lng
        /// </summary>
        public static H3Index GeoToH3(GeoCoord g, int r)
        {
            return g.ToH3Index(r);
        }

        /// <summary>
        /// find the lat/lon center point g of the cell h3
        /// </summary>
        public static void H3ToGeo(H3Index h3, out GeoCoord g)
        {
            g = h3.ToGeoCoord();
        }

        /// <summary>
        /// give the cell boundary in lat/lon coordinates for the cell h3
        /// </summary>
        public static void H3ToGeoBoundary(H3Index h3, out GeoBoundary gb)
        {
            gb = h3.ToGeoBoundary();
        }

        /// <summary>
        /// maximum number of hexagons in k-ring
        /// </summary>
        public static int MaxKringSize(int k)
        {
            return k.MaxKringSize();
        }

        /// <summary>
        /// hexagons neighbors in all directions, assuming no pentagons
        /// </summary>
        public static int HexRange(H3Index origin, int k, out List<H3Index> outHex)
        {
            (int status, var tempHex) = origin.HexRange(k);
            outHex = tempHex;
            return status;
        }

        /// <summary>
        /// hexagons neighbors in all directions, assuming no pentagons,
        /// reporting distance from origin
        /// </summary>
        public static int HexRangeDistances(H3Index origin, int k, out List<H3Index> outCells, out List<int> distances)
        {
            (int status, var values) = origin.HexRangeDistances(k);

            outCells = new List<H3Index>();
            distances = new List<int>();
            foreach (var tuple in values)
            {
                outCells.Add(tuple.Item1);
                distances.Add(tuple.Item2);
            }

            return status;
        }

        /// <summary>
        /// collection of hex rings sorted by ring for all given hexagons
        /// </summary>
        public static int HexRanges(List<H3Index> h3Set, int length, int k, out List<H3Index> outCells)
        {
            (int status, var values) = h3Set.HexRanges(k);
            outCells = values;
            return status;
        }

        /// <summary>
        /// hexagon neighbors in all directions
        /// </summary>
        public static void KRing(H3Index origin, int k, out List<H3Index> outCells)
        {
            outCells = origin.KRing(k);
        }

        /// <summary>
        /// hexagon neighbors in all directions, reporting distance from origin
        /// </summary>
        public static void KRingDistances(H3Index origin, int k, out List<H3Index> outCells, out List<int> distances)
        {
            var lookup = origin.KRingDistances(k);
            outCells = lookup.Keys.ToList();
            distances = lookup.Values.ToList();
        }

        /// <summary>
        /// hollow hexagon ring at some origin
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="k"></param>
        /// <param name="outCells"></param>
        /// <returns></returns>
        public static int HexRing(H3Index origin, int k, out List<H3Index> outCells)
        {
            (int status, List<H3Index> cells) = origin.HexRing(k);
            outCells = cells;
            return status;
        }

        /// <summary>
        /// maximum number of hexagons in the geofence
        /// </summary>
        /// <param name="polygon"></param>
        /// <param name="r"></param>
        /// <returns></returns>
        public static int MaxPolyFillSize(GeoPolygon polygon, int r)
        {
            return polygon.MaxPolyFillSize(r);
        }

        /// <summary>
        /// hexagons within the given geofence
        /// </summary>
        /// <param name="polygon"></param>
        /// <param name="r"></param>
        /// <param name="outCells"></param>
        public static void PolyFill(GeoPolygon polygon, int r, out List<H3Index> outCells)
        {
            outCells = polygon.Polyfill(r);
        }

        /// <summary>
        /// Create a LinkedGeoPolygon from a set of contiguous hexagons
        /// </summary>
        /// <param name="h3Set"></param>
        /// <param name="outPolygon"></param>
        public static void H3SetToLinkedGeo(List<H3Index> h3Set, out LinkedGeoPolygon outPolygon)
        {
            outPolygon = h3Set.ToLinkedGeoPolygon();
        }


        /// <summary>
        /// converts degrees to radians
        /// </summary>
        /// <param name="degrees"></param>
        /// <returns></returns>
        public static double DegreesToRadians(double degrees)
        {
            return degrees.DegreesToRadians();
        }


        /// <summary>
        /// converts radians to degrees
        /// </summary>
        /// <returns></returns>
        public static double RadiansToDegrees(double radians)
        {
            return radians.RadiansToDegrees();
        }

        /// <summary>
        /// "great circle distance" between pairs of GeoCoord points in radians*/
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static double PointDistRads(GeoCoord a, GeoCoord b)
        {
            return a.DistanceToRadians(b);
        }

        /// <summary>
        /// "great circle distance" between pairs of GeoCoord points in kilometers
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static double PointDistKm(GeoCoord a, GeoCoord b)
        {
            return a.DistanceToKm(b);
        }

        /// <summary>
        /// "great circle distance" between pairs of GeoCoord points in meters*/
        /// </summary>
        public static double PointDistM(GeoCoord a, GeoCoord b)
        {
           return a.DistanceToM(b);
        }

        /// <summary>
        /// average hexagon area in square kilometers (excludes pentagons)
        /// </summary>
        public static double HexAreaKm2(int r)
        {
            return GeoCoord.HexAreaKm2(r);
        }

        /// <summary>
        /// average hexagon area in square meters (excludes pentagons)
        /// </summary>
        public static double HexAreaM2(int r)
        {
            return GeoCoord.HexAreaM2(r);
        }


        /// <summary>
        /// exact area for a specific cell (hexagon or pentagon) in radians^2
        /// </summary>
        public static double CellAreaRads2(H3Index h)
        {
            return h.CellAreaRadians2();
        }

        /// <summary>
        /// exact area for a specific cell (hexagon or pentagon) in kilometers^2
        /// </summary>
        public static double CellAreaKm2(H3Index h)
        {
            return h.CellAreaKm2();
        }

        /// <summary>
        /// exact area for a specific cell (hexagon or pentagon) in meters^2
        /// </summary>
        public static double CellAreaM2(H3Index h)
        {
            return h.CellAreaM2();
        }

        /// <summary>
        /// average hexagon edge length in kilometers (excludes pentagons)
        /// </summary>
        public static double EdgeLengthKm(int r)
        {
            return GeoCoord.EdgeLengthKm(r);
        }

        /// <summary>
        /// average hexagon edge length in meters (excludes pentagons)
        /// </summary>
        public static double EdgeLengthM(int r)
        {
            return GeoCoord.EdgeLengthM(r);
        }

        /// <summary>
        /// exact length for a specific unidirectional edge in radians*/
        /// </summary>
        public static double ExactEdgeLengthRads(H3Index edge)
        {
            return edge.ExactEdgeLengthRads();
        }

        /// <summary>
        /// exact length for a specific unidirectional edge in kilometers*/
        /// </summary>
        public static double ExactEdgeLengthKm(H3Index edge)
        {
            return edge.ExactEdgeLengthKm();
        }

        /// <summary>
        /// exact length for a specific unidirectional edge in meters*/
        /// </summary>
        public static double ExactEdgeLengthM(H3Index edge)
        {
            return edge.ExactEdgeLengthM();
        }

        /// <summary>
        /// number of cells (hexagons and pentagons) for a given resolution
        /// </summary>
        public static long NumHexagons(int r)
        {
            return r.NumHexagons();
        }

        /// <summary>
        /// returns the number of resolution 0 cells (hexagons and pentagons)
        /// </summary>
        public static int Res0IndexCount()
        {
            return Constants.H3.NUM_BASE_CELLS;
        }

        /// <summary>
        /// provides all base cells in H3Index format*/
        /// </summary>
        public static void GetRes0Indexes(out List<H3Index> outCells)
        {
            outCells = BaseCellsExtensions.GetRes0Indexes();
        }

        /// <summary>
        /// returns the number of pentagons per resolution
        /// </summary>
        public static int PentagonIndexCount()
        {
            return H3Index.PentagonIndexCount;
        }

        /// <summary>
        /// generates all pentagons at the specified resolution
        /// </summary>
        public static void GetPentagonIndexes(int r, out List<H3Index> outCells)
        {
            outCells = r.GetPentagonIndexes();
        }

        /// <summary>
        /// returns the resolution of the provided H3 index
        /// Works on both cells and unidirectional edges.
        /// </summary>
        public static int H3GetResolution(H3Index h)
        {
            return h.Resolution;
        }

        /// <summary>
        /// returns the base cell "number" (0 to 121) of the provided H3 cell
        /// </summary>
        public static int H3GetBaseCell(H3Index h)
        {
            return h.BaseCell;
        }

        /// <summary>
        /// converts the canonical string format to H3Index format
        /// </summary>
        public static H3Index StringToH3(string s)
        {
            if (s.StartsWith("0x") || s.StartsWith("0X"))
            {
                s = s.Substring(2);
            }

            if (s.StartsWith("x") || s.StartsWith("X"))
            {
                s = s.Substring(1);
            }

            return ulong.TryParse(s, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out ulong h3)
                       ? new H3Index(h3)
                       : new H3Index(Constants.H3Index.H3_NULL);
        }

        /// <summary>
        /// converts an H3Index to a canonical string
        /// </summary>
        public static void H3ToString(H3Index h, out string str)
        {
            str = h.ToString();
        }

        /// <summary>
        /// confirms if an H3Index is a valid cell (hexagon or pentagon)
        /// </summary>
        public static int H3IsValid(H3Index h)
        {
            return h.IsValid()
                       ? 1
                       : 0;
        }

        /// <summary>
        /// returns the parent (or grandparent, etc) hexagon of the given hexagon
        /// </summary>
        public static H3Index H3ToParent(H3Index h, int parentRes)
        {
            return h.ToParent(parentRes);
        }

        /// <summary>
        /// determines the maximum number of children (or grandchildren, etc)
        /// </summary>
        public static long MaxH3ToChildrenSize(H3Index h, int childRes)
        {
            return h.MaxChildrenSize(childRes);
        }

        /// <summary>
        /// provides the children (or grandchildren, etc) of the given hexagon
        /// </summary>
        public static void H3ToChildren(H3Index h, int childRes, out List<H3Index> outChildren)
        {
            outChildren = h.ToChildren(childRes);
        }

        /// <summary>
        /// returns the center child of the given hexagon at the specified
        /// </summary>
        public static H3Index H3ToCenterChild(H3Index h, int childRes)
        {
            return h.ToCenterChild(childRes);
        }

        /// <summary>
        /// compacts the given set of hexagons as best as possible
        /// </summary>
        public static int Compact(List<H3Index> h3Set, out List<H3Index> outCompacted)
        {
            int status;
            (status, outCompacted) = h3Set.Compact();
            return status;
        }

        /// <summary>
        /// determines the maximum number of hexagons that could be uncompacted
        /// from the compacted set
        /// </summary>
        public static long MaxUncompactSize(H3Index compacted, int r)
        {
            return compacted.MaxUncompactSize(r);
        }

        /// <summary>
        /// uncompacts the compacted hexagon set
        /// </summary>
        public static int Uncompact(List<H3Index> compactedSet, out List<H3Index> outCells, int res)
        {
            int status;
            (status, outCells) = compactedSet.Uncompact(res);
            return status;
        }

        /// <summary>
        /// determines if a hexagon is Class III (or Class II)
        /// </summary>
        public static int H3IsResClassIii(H3Index h)
        {
            return h.IsResClassIii
                       ? 1
                       : 0;
        }

        /// <summary>
        /// determines if an H3 cell is a pentagon
        /// </summary>
        public static int H3IsPentagon(H3Index h)
        {
            return h.IsPentagon()
                       ? 1
                       : 0;
        }

        /// <summary>
        /// Max number of icosahedron faces intersected by an index
        /// </summary>
        public static int MaxFaceCount(H3Index h3)
        {
            return h3.MaxFaceCount();
        }

        /// <summary>
        /// Find all icosahedron faces intersected by a given H3 index
        /// </summary>
        public static void H3GetFaces(H3Index h3, out List<int> outFaces)
        {
            outFaces = h3.GetFaces();
        }

        /// <summary>
        /// returns whether or not the provided hexagons border
        /// </summary>
        public static int H3IndexesAreNeighbors(H3Index origin, H3Index destination)
        {
            return origin.IsNeighborTo(destination)
                       ? 1
                       : 0;
        }

        /// <summary>
        /// returns the unidirectional edge H3Index for the specified origin and
        /// destination
        /// </summary>
        public static H3Index GetH3UnidirectionalEdge(H3Index origin, H3Index destination)
        {
            return origin.UniDirectionalEdgeTo(destination);
        }

        /// <summary>
        /// returns whether the H3Index is a valid unidirectional edge
        /// </summary>
        public static int H3UnidirectionalEdgeIsValid(H3Index edge)
        {
            return edge.IsValidUniEdge()
                       ? 1
                       : 0;
        }

        /// <summary>
        /// Returns the origin hexagon H3Index from the unidirectional edge
        /// </summary>
        /// H3Index
        public static H3Index GetOriginH3IndexFromUnidirectionalEdge(H3Index edge)
        {
            return edge.OriginFromUniDirectionalEdge();
        }

        /// <summary>
        /// Returns the destination hexagon H3Index from the unidirectional edge
        /// H3Index
        /// </summary>
        public static H3Index GetDestinationH3IndexFromUnidirectionalEdge(H3Index edge)
        {
            return edge.DestinationFromUniDirectionalEdge();
        }

        /// <summary>
        /// Returns the origin and destination hexagons from the unidirectional
        /// edge H3Index
        /// </summary>
        public static void GetH3IndexesFromUnidirectionalEdge(H3Index edge, out (H3Index, H3Index) originDestination)
        {
            originDestination = edge.GetH3IndexesFromUniEdge();
        }

        /// <summary>
        /// Returns the 6 (or 5 for pentagons) edges associated with the H3Index
        /// </summary>
        public static void GetH3UnidirectionalEdgesFromHexagon(H3Index origin, out List<H3Index> edges)
        {
            edges = origin.GetUniEdgesFromCell().ToList();
        }

        /// <summary>
        /// Returns the GeoBoundary containing the coordinates of the edge
        /// </summary>
        public static void GetH3UnidirectionalEdgeBoundary(H3Index edge, out GeoBoundary gb)
        {
            gb = edge.ToGeoBoundary();
        }


        /// <summary>
        /// Returns grid distance between two indexes
        /// </summary>
        public static int H3Distance(H3Index origin, H3Index h3)
        {
            return origin.DistanceTo(h3);
        }

        /// <summary>
        /// Number of indexes in a line connecting two indexes
        /// </summary>
        public static int H3LineSize(H3Index start, H3Index end)
        {
            return start.LineSize(end);
        }

        /// <summary>
        /// Line of h3 indexes connecting two indexes
        /// </summary>
        public static int H3Line(H3Index start, H3Index end, out List<H3Index> outCells)
        {
            (int status, var tempCells) = start.LineTo(end);
            outCells = tempCells.ToList();
            return status;
        }

        /// <summary>
        /// Returns two dimensional coordinates for the given index
        /// </summary>
        public static int ExperimentalH3ToLocalIj(
                H3Index origin, H3Index h3,
                out CoordIj outCoord
            )
        {
            int status;
            (status, outCoord) = origin.ToLocalIjExperimental(h3);
            return status;
        }

        /// <summary>
        /// Returns index for the given two dimensional coordinates
        /// </summary>
        public static int ExperimentalLocalIjToH3(H3Index origin, CoordIj ij, out H3Index outCell)
        {
            int status;
            (status, outCell) = ij.ToH3Experimental(origin);
            return status;
        }

        /// <summary>
        /// Winging this one.
        /// </summary>
        /// <param name="p"></param>
        /// <param name="latDegs"></param>
        /// <param name="lonDegs"></param>
        /// <returns></returns>
        public static GeoCoord SetGeoDegs(double latDegs, double lonDegs)
        {
            return new GeoCoord().SetDegrees(latDegs, lonDegs);
        }
        
    }
}
