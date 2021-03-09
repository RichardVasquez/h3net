using System.Collections.Generic;
using System.Linq;
using H3Lib.Extensions;

namespace H3Lib
{
    /// <summary>
    /// Primary H3 core library entry points.
    /// </summary>
    public static class Api
    {
        /// <summary>
        /// Find the H3 index of the resolution res cell containing the lat/lng
        /// </summary>
        public static H3Index GeoToH3(GeoCoord g, int r)
        {
            return g.ToH3Index(r);
        }

        /// <summary>
        /// Find the lat/lon center point g of the cell h3
        /// </summary>
        public static void H3ToGeo(H3Index h3, out GeoCoord g)
        {
            g = h3.ToGeoCoord();
        }

        /// <summary>
        /// Give the cell boundary in lat/lon coordinates for the cell h3
        /// </summary>
        public static void H3ToGeoBoundary(H3Index h3, out GeoBoundary gb)
        {
            gb = h3.ToGeoBoundary();
        }

        /// <summary>
        /// Maximum number of hexagons in k-ring
        /// </summary>
        public static int MaxKringSize(int k)
        {
            return k.MaxKringSize();
        }

        /// <summary>
        /// Hexagons neighbors in all directions, assuming no pentagons
        /// </summary>
        public static int HexRange(H3Index origin, int k, out List<H3Index> outHex)
        {
            (int status, var tempHex) = origin.HexRange(k);
            outHex = tempHex;
            return status;
        }

        /// <summary>
        /// Hexagons neighbors in all directions, assuming no pentagons,
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
        /// Collection of hex rings sorted by ring for all given hexagons
        /// </summary>
        public static int HexRanges(List<H3Index> h3Set, int length, int k, out List<H3Index> outCells)
        {
            (int status, var values) = h3Set.HexRanges(k);
            outCells = values;
            return status;
        }

        /// <summary>
        /// Hexagon neighbors in all directions
        /// </summary>
        public static void KRing(H3Index origin, int k, out List<H3Index> outCells)
        {
            outCells = origin.KRing(k);
        }

        /// <summary>
        /// Hexagon neighbors in all directions, reporting distance from origin
        /// </summary>
        public static void KRingDistances(H3Index origin, int k, out List<H3Index> outCells, out List<int> distances)
        {
            var lookup = origin.KRingDistances(k);
            outCells = lookup.Keys.ToList();
            distances = lookup.Values.ToList();
        }

        /// <summary>
        /// Hollow hexagon ring at some origin
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
        /// Maximum number of hexagons in the geofence
        /// </summary>
        /// <param name="polygon"></param>
        /// <param name="r"></param>
        /// <returns></returns>
        public static int MaxPolyFillSize(GeoPolygon polygon, int r)
        {
            return polygon.MaxPolyFillSize(r);
        }

        /// <summary>
        /// Hexagons within the given geofence
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
        /// Converts degrees to radians
        /// </summary>
        /// <param name="degrees"></param>
        /// <returns></returns>
        public static decimal DegreesToRadians(decimal degrees)
        {
            return degrees.DegreesToRadians();
        }


        /// <summary>
        /// Vonverts radians to degrees
        /// </summary>
        /// <returns></returns>
        public static decimal RadiansToDegrees(decimal radians)
        {
            return radians.RadiansToDegrees();
        }

        /// <summary>
        /// "Great circle distance" between pairs of GeoCoord points in radians
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static decimal PointDistRads(GeoCoord a, GeoCoord b)
        {
            return a.DistanceToRadians(b);
        }

        /// <summary>
        /// "Great circle distance" between pairs of GeoCoord points in kilometers
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static decimal PointDistKm(GeoCoord a, GeoCoord b)
        {
            return a.DistanceToKm(b);
        }

        /// <summary>
        /// "Great circle distance" between pairs of GeoCoord points in meters
        /// </summary>
        public static decimal PointDistM(GeoCoord a, GeoCoord b)
        {
           return a.DistanceToM(b);
        }

        /// <summary>
        /// Average hexagon area in square kilometers (excludes pentagons)
        /// </summary>
        public static decimal HexAreaKm2(int r)
        {
            return GeoCoord.HexAreaKm2(r);
        }

        /// <summary>
        /// Average hexagon area in square meters (excludes pentagons)
        /// </summary>
        public static decimal HexAreaM2(int r)
        {
            return GeoCoord.HexAreaM2(r);
        }


        /// <summary>
        /// Exact area for a specific cell (hexagon or pentagon) in radians^2
        /// </summary>
        public static decimal CellAreaRads2(H3Index h)
        {
            return h.CellAreaRadians2();
        }

        /// <summary>
        /// Exact area for a specific cell (hexagon or pentagon) in kilometers^2
        /// </summary>
        public static decimal CellAreaKm2(H3Index h)
        {
            return h.CellAreaKm2();
        }

        /// <summary>
        /// Exact area for a specific cell (hexagon or pentagon) in meters^2
        /// </summary>
        public static decimal CellAreaM2(H3Index h)
        {
            return h.CellAreaM2();
        }

        /// <summary>
        /// Average hexagon edge length in kilometers (excludes pentagons)
        /// </summary>
        public static decimal EdgeLengthKm(int r)
        {
            return GeoCoord.EdgeLengthKm(r);
        }

        /// <summary>
        /// Average hexagon edge length in meters (excludes pentagons)
        /// </summary>
        public static decimal EdgeLengthM(int r)
        {
            return GeoCoord.EdgeLengthM(r);
        }

        /// <summary>
        /// Exact length for a specific unidirectional edge in radians*/
        /// </summary>
        public static decimal ExactEdgeLengthRads(H3Index edge)
        {
            return edge.ExactEdgeLengthRads();
        }

        /// <summary>
        /// Exact length for a specific unidirectional edge in kilometers*/
        /// </summary>
        public static decimal ExactEdgeLengthKm(H3Index edge)
        {
            return edge.ExactEdgeLengthKm();
        }

        /// <summary>
        /// Exact length for a specific unidirectional edge in meters*/
        /// </summary>
        public static decimal ExactEdgeLengthM(H3Index edge)
        {
            return edge.ExactEdgeLengthM();
        }

        /// <summary>
        /// Number of cells (hexagons and pentagons) for a given resolution
        /// </summary>
        public static long NumHexagons(int r)
        {
            return r.NumHexagons();
        }

        /// <summary>
        /// Returns the number of resolution 0 cells (hexagons and pentagons)
        /// </summary>
        public static int Res0IndexCount()
        {
            return Constants.H3.BaseCellsCount;
        }

        /// <summary>
        /// Provides all base cells in H3Index format*/
        /// </summary>
        public static void GetRes0Indexes(out List<H3Index> outCells)
        {
            outCells = BaseCellsExtensions.GetRes0Indexes();
        }

        /// <summary>
        /// Returns the number of pentagons per resolution
        /// </summary>
        public static int PentagonIndexCount()
        {
            return H3Index.PentagonIndexCount;
        }

        /// <summary>
        /// Generates all pentagons at the specified resolution
        /// </summary>
        public static void GetPentagonIndexes(int r, out List<H3Index> outCells)
        {
            outCells = r.GetPentagonIndexes();
        }

        /// <summary>
        /// Returns the resolution of the provided H3 index
        /// Works on both cells and unidirectional edges.
        /// </summary>
        public static int H3GetResolution(H3Index h)
        {
            return h.Resolution;
        }

        /// <summary>
        /// Returns the base cell "number" (0 to 121) of the provided H3 cell
        /// </summary>
        public static int H3GetBaseCell(H3Index h)
        {
            return h.BaseCell;
        }

        /// <summary>
        /// Converts the canonical string format to H3Index format
        /// </summary>
        public static H3Index StringToH3(string s)
        {
            return new H3Index(s);
        }

        /// <summary>
        /// Converts an H3Index to a canonical string
        /// </summary>
        public static void H3ToString(H3Index h, out string str)
        {
            str = h.ToString();
        }

        /// <summary>
        /// Confirms if an H3Index is a valid cell (hexagon or pentagon)
        /// </summary>
        public static int H3IsValid(H3Index h)
        {
            return h.IsValid()
                       ? 1
                       : 0;
        }

        /// <summary>
        /// Returns the parent (or grandparent, etc) hexagon of the given hexagon
        /// </summary>
        public static H3Index H3ToParent(H3Index h, int parentRes)
        {
            return h.ToParent(parentRes);
        }

        /// <summary>
        /// Determines the maximum number of children (or grandchildren, etc)
        /// </summary>
        public static long MaxH3ToChildrenSize(H3Index h, int childRes)
        {
            return h.MaxChildrenSize(childRes);
        }

        /// <summary>
        /// Provides the children (or grandchildren, etc) of the given hexagon
        /// </summary>
        public static void H3ToChildren(H3Index h, int childRes, out List<H3Index> outChildren)
        {
            outChildren = h.ToChildren(childRes);
        }

        /// <summary>
        /// Returns the center child of the given hexagon at the specified resolution
        /// </summary>
        public static H3Index H3ToCenterChild(H3Index h, int childRes)
        {
            return h.ToCenterChild(childRes);
        }

        /// <summary>
        /// Compacts the given set of hexagons as best as possible
        /// </summary>
        public static int Compact(List<H3Index> h3Set, out List<H3Index> outCompacted)
        {
            int status;
            (status, outCompacted) = h3Set.Compact();
            return status;
        }

        /// <summary>
        /// Determines the maximum number of hexagons that could be uncompacted
        /// from the compacted set
        /// </summary>
        public static long MaxUncompactSize(List<H3Index> compacted, int r)
        {
            return compacted.MaxUncompactSize(r);
        }

        /// <summary>
        /// Uncompacts the compacted hexagon set
        /// </summary>
        public static int Uncompact(List<H3Index> compactedSet, out List<H3Index> outCells, int res)
        {
            int status;
            (status, outCells) = compactedSet.Uncompact(res);
            return status;
        }

        /// <summary>
        /// Determines if a hexagon is Class III (or Class II)
        /// </summary>
        public static int H3IsResClassIii(H3Index h)
        {
            return h.IsResClassIii
                       ? 1
                       : 0;
        }

        /// <summary>
        /// Determines if an H3 cell is a pentagon
        /// </summary>
        public static int H3IsPentagon(H3Index h)
        {
            return h.IsPentagon
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
        /// Returns whether or not the provided hexagons border
        /// </summary>
        public static int H3IndexesAreNeighbors(H3Index origin, H3Index destination)
        {
            return origin.IsNeighborTo(destination)
                       ? 1
                       : 0;
        }

        /// <summary>
        /// Returns the unidirectional edge H3Index for the specified origin and
        /// destination
        /// </summary>
        public static H3Index GetH3UnidirectionalEdge(H3Index origin, H3Index destination)
        {
            return origin.UniDirectionalEdgeTo(destination);
        }

        /// <summary>
        /// Returns whether the H3Index is a valid unidirectional edge
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
            gb = edge.UniEdgeToGeoBoundary();
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
        /// Winging this one, returns a GeoCoord with degree values instead of radians
        /// </summary>
        public static GeoCoord SetGeoDegs(decimal latDegs, decimal lonDegs)
        {
            return new GeoCoord().SetDegrees(latDegs, lonDegs);
        }

        /// <summary>
        /// Erases all the information for a linkedgeopolygon
        /// </summary>
        /// <param name="polygon"></param>
        public static void DestroyLinkedPolygon(LinkedGeoPolygon polygon)
        {
            polygon.Clear();
        }

        /// <summary>
        /// Converts degrees to radians.
        /// </summary>
        public static decimal DegsToRads(decimal degrees)
        {
            return degrees.DegreesToRadians();
        }
        
        /// <summary>
        /// Converts radians to degrees
        /// </summary>
        public static decimal RadsToDegs(decimal radians)
        {
            return radians.RadiansToDegrees();
        }
    }
}
