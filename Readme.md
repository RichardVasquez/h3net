# H3NET: A Hexagonal Hierarchical Geospatial Indexing System In C# #

H3Net is a geospatial indexing system using hexagonal grid that can be (approximately) subdivided into finer and finer hexagonal grids, combining the benefits of a hexagonal grid with [S2](https://code.google.com/archive/p/s2-geometry-library/)'s hierarchical subdivisions, mostly translated from the original C code from [Uber](https://github.com/uber)'s [H3](https://github.com/uber/h3) project.

# Caveat Emptor

I have a personal project I'm working on that needs hexagons all over the world, and I've been using [DGGRID](http://www.discreteglobalgrids.org/software/) for generating patches of data across the globe.  Seeing this code come out, and pre-oriented for [Dymaxion project](https://en.wikipedia.org/wiki/Dymaxion_map), I did the conversion to C# for various reasons, accepting the tradeoff in speed.  

As such, this code does what **I NEED IT TO DO**, and it may not be what you're looking for.  I've tried to make it approachable for others, but I can't guarantee that.

Updates will be provided as I tweak things, but the main project using this library needs to be worked on for a while now that I've got a decent tool to generate hexes.

Additionally, you should probably familiarize yourself with the h3net.Api structs contained in Structs.cs as that's what the API returns its results in, and the initial structs and data classes will eventually be deprecated.

## Testing
I've converted the basic test suites under the project h3tests using [Nunit](https://github.com/nunit).  polyfillTransmeridianComplex doesn't work.  PRs that address that one test will get my attention the quickest.

## Installing

In the solution, build the h3net project into a DLL.  You can then use the h3net.Api namespace to access the following list.  See [Uber's H3 API](https://uber.github.io/h3/#/documentation/api-reference/indexing) for pointers regarding use.  There's some signature differences explained after the list.

- Api.H3Index GeoToH3(Code.GeoCoord gc, int res)
- Api.GeoCoord H3ToGeo(Code.H3Index h3)
- Api.GeoBoundary H3ToGeoBoundary(Code.H3Index h3)
- int MaxKringSize(int k)
- HexRangeResult HexRange(Code.H3Index origin, int k)
- HexRangeDistancesResult HexRangeDistances(Code.H3Index origin, int k)
- HexRangeResult HexRanges(IEnumerable<Code.H3Index> h3Set, int k)
- HexRangeResult HexRanges(Code.H3Index h3Set, int k)
- H3Index[] Kring(Code.H3Index origin, int k)
- HexRangeDistancesResult KringDistances(Code.H3Index origin, int k)
- HexRangeResult HexRing(Code.H3Index origin, int k)
- int MaxPolyFillSize(Code.GeoPolygon geoPolygon, int res)
- H3Index[] PolyFill(Code.GeoPolygon geoPolygon, int res)
- LinkedGeoPolygon H3SetToLinkedGeo(IEnumerable<Code.H3Index> h3Set)
- void DestroyLinkedPolygon(ref LinkedGeoPolygon polygon)
- double DegreesToRadians(double degrees)
- double RadiansToDegrees(double radians)
- double HexAreaKm2(int res)
- double HexAreaM2(int res)
- double EdgeLengthKm(int res)
- double EdgeLengthM(int res)
- long NumberHexagons(int res)
- int H3GetResolution(Code.H3Index h3)
- int H3GetBaseCell(Code.H3Index h3)
- H3Index StringToH3(string s)
- string H3ToString(Code.H3Index h3)
- int H3IsValid(Code.H3Index h3)
- H3Index H3ToParent(Code.H3Index h3, int parentRes)
- int MaxH3ToChildrenSize(Code.H3Index h3, int childRes)
- H3Index[] H3ToChildren(Code.H3Index h3, int childRes)
- HexRangeResult Compact(List<Code.H3Index> h3Set)
- int MaxUncompactSize(List<Code.H3Index> h3Set, int res)
- HexRangeResult Uncompact(List<Code.H3Index> compactedSet, int res)
- int H3IsResClassIII(Code.H3Index h3)
- bool IsResClassIII(Code.H3Index h3)
- int H3IsPentagon(Code.H3Index h3)
- bool IsPentagon(Code.H3Index h3)
- int H3IndexesAreNeighbors(Code.H3Index origin, Code.H3Index destination)
- bool AreNeighbors(Code.H3Index origin, Code.H3Index destination)
- H3Index GetUniDirectionalEdge(Code.H3Index origin, Code.H3Index destination)
- int H3UniDirectionalEdgeIsValid(Code.H3Index edge)
- bool UniDirectionalEdgeIsValid(Code.H3Index edge)
- H3Index GetOriginH3FromUniDirectionalEdge(Code.H3Index edge)
- H3Index GetDestinationH3FromUniDirectionalEdge(Code.H3Index edge)
- H3Index[] GetH3IndexesFromUnidirectionalEdge(Code.H3Index edge)
- H3Index[] GetH3UniDirectionalEdgesFromHexagon(Code.H3Index origin)
- GeoBoundary GetH3UnidirectionalEdgeBoundary(Code.H3Index edge)
- int H3Distance(Code.H3Index origin, Code.H3Index h3)
- ExperimentalIJ ExperimentalH3ToLocalIj(Code.H3Index origin, Code.H3Index h3)
- HexRangeResult experimentalLocalIjToH3(Code.H3Index origin, Code.LocalIJ.CoordIJ ij)

The primary difference between my API implementation and Uber's is that I'm not expecting pointers to preallocated memory to store the results.  Barring the linked data types, you're going to receive a single instance/array collection of a C# value type.  Immutable, if it's a struct.

### Roadmap
There's a **lot** of code in the h3net.Code namespace where I just used ref and passed around blobs of data, and then had to finesse accesing an indexed value by duplicating it, feeding it through the translated code, then putting it back into the original collection.  I'm not proud of that, and that's likely the first part that gets steam cleaned.

#### Version
I will be keeping the version number the same as the functionality of H3 that I'm matching.
