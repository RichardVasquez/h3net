# API
Primary H3 core library entry points.

## CellAreaM2() method
Summary

exact area for a specific cell (hexagon or pentagon) in meters^2
Parameters

This method has no parameters.

## CellAreaRads2() method
Summary

exact area for a specific cell (hexagon or pentagon) in radians^2
Parameters

This method has no parameters.

## Compact() method
Summary

compacts the given set of hexagons as best as possible
Parameters

This method has no parameters.

## DegreesToRadians(degrees) method
Summary

converts degrees to radians
Returns
Parameters
Name 	Type 	Description
degrees 	System.Decimal

## EdgeLengthKm() method
Summary

average hexagon edge length in kilometers (excludes pentagons)
Parameters

This method has no parameters.

## EdgeLengthM() method
Summary

average hexagon edge length in meters (excludes pentagons)
Parameters

This method has no parameters.

## ExactEdgeLengthKm() method
Summary

exact length for a specific unidirectional edge in kilometers*/
Parameters

This method has no parameters.

## ExactEdgeLengthM() method
Summary

exact length for a specific unidirectional edge in meters*/
Parameters

This method has no parameters.

## ExactEdgeLengthRads() method
Summary

exact length for a specific unidirectional edge in radians*/
Parameters

This method has no parameters.

## ExperimentalH3ToLocalIj() method
Summary

Returns two dimensional coordinates for the given index
Parameters

This method has no parameters.

## ExperimentalLocalIjToH3() method
Summary

Returns index for the given two dimensional coordinates
Parameters

This method has no parameters.


## GetDestinationH3IndexFromUnidirectionalEdge() method
Summary

Returns the destination hexagon H3Index from the unidirectional edge H3Index
Parameters

This method has no parameters.

## GetH3IndexesFromUnidirectionalEdge() method
Summary

Returns the origin and destination hexagons from the unidirectional edge H3Index
Parameters

This method has no parameters.

## GetH3UnidirectionalEdge() method
Summary

returns the unidirectional edge H3Index for the specified origin and destination
Parameters

This method has no parameters.

## GetH3UnidirectionalEdgeBoundary() method
Summary

Returns the GeoBoundary containing the coordinates of the edge
Parameters

This method has no parameters.

## GetH3UnidirectionalEdgesFromHexagon() method
Summary

Returns the 6 (or 5 for pentagons) edges associated with the H3Index
Parameters

This method has no parameters.

## GetOriginH3IndexFromUnidirectionalEdge() method
Summary

Returns the origin hexagon H3Index from the unidirectional edge
Parameters

This method has no parameters.

## GetPentagonIndexes() method
Summary

generates all pentagons at the specified resolution
Parameters

This method has no parameters.

## GetRes0Indexes() method
Summary

provides all base cells in H3Index format*/
Parameters

This method has no parameters.

## H3Distance() method
Summary

Returns grid distance between two indexes
Parameters

This method has no parameters.

## H3GetBaseCell() method
Summary

returns the base cell "number" (0 to 121) of the provided H3 cell
Parameters

This method has no parameters.

H3GetFaces() method
Summary

Find all icosahedron faces intersected by a given H3 index
Parameters

This method has no parameters.

## H3GetResolution() method
Summary

returns the resolution of the provided H3 index Works on both cells and unidirectional edges.
Parameters

This method has no parameters.

## H3IndexesAreNeighbors() method
Summary

returns whether or not the provided hexagons border
Parameters

This method has no parameters.

## H3IsPentagon() method
Summary

determines if an H3 cell is a pentagon
Parameters

This method has no parameters.

## H3IsResClassIii() method
Summary

determines if a hexagon is Class III (or Class II)
Parameters

This method has no parameters.

## H3IsValid() method
Summary

confirms if an H3Index is a valid cell (hexagon or pentagon)
Parameters

This method has no parameters.

## H3Line() method
Summary

Line of h3 indexes connecting two indexes
Parameters

This method has no parameters.

## H3LineSize() method
Summary

Number of indexes in a line connecting two indexes
Parameters

This method has no parameters.

## H3SetToLinkedGeo(h3Set,outPolygon) method
Summary

Create a LinkedGeoPolygon from a set of contiguous hexagons
Parameters
Name 	Type 	Description
h3Set 	System.Collections.Generic.List{H3Lib.H3Index} 	
outPolygon 	H3Lib.LinkedGeoPolygon@

## H3ToCenterChild() method
Summary

returns the center child of the given hexagon at the specified
Parameters

This method has no parameters.

## H3ToChildren() method
Summary

provides the children (or grandchildren, etc) of the given hexagon
Parameters

This method has no parameters.



## H3ToParent() method
Summary

returns the parent (or grandparent, etc) hexagon of the given hexagon
Parameters

This method has no parameters.

## H3ToString() method
Summary

converts an H3Index to a canonical string
Parameters

This method has no parameters.

## H3UnidirectionalEdgeIsValid() method
Summary

returns whether the H3Index is a valid unidirectional edge
Parameters

This method has no parameters.

## HexAreaKm2() method
Summary

average hexagon area in square kilometers (excludes pentagons)
Parameters

This method has no parameters.

## HexAreaM2() method
Summary

average hexagon area in square meters (excludes pentagons)
Parameters

This method has no parameters.

## HexRange() method
Summary

hexagons neighbors in all directions, assuming no pentagons
Parameters

This method has no parameters.

## HexRangeDistances() method
Summary

hexagons neighbors in all directions, assuming no pentagons, reporting distance from origin
Parameters

This method has no parameters.

## HexRanges() method
Summary

collection of hex rings sorted by ring for all given hexagons
Parameters

This method has no parameters.

## HexRing(origin,k,outCells) method
Summary

hollow hexagon ring at some origin
Returns
Parameters
Name 	Type 	Description
origin 	H3Lib.H3Index 	
k 	System.Int32 	
outCells 	System.Collections.Generic.List{H3Lib.H3Index}@

## KRing() method
Summary

hexagon neighbors in all directions
Parameters

This method has no parameters.

## KRingDistances() method
Summary

hexagon neighbors in all directions, reporting distance from origin
Parameters

This method has no parameters.

## MaxFaceCount() method
Summary

Max number of icosahedron faces intersected by an index
Parameters

This method has no parameters.

## MaxH3ToChildrenSize() method
Summary

determines the maximum number of children (or grandchildren, etc)
Parameters

This method has no parameters.

## MaxKringSize() method
Summary

maximum number of hexagons in k-ring
Parameters

This method has no parameters.

## MaxPolyFillSize(polygon,r) method
Summary

maximum number of hexagons in the geofence
Returns
Parameters
Name 	Type 	Description
polygon 	H3Lib.GeoPolygon 	
r 	System.Int32

## MaxUncompactSize() method
Summary

determines the maximum number of hexagons that could be uncompacted from the compacted set
Parameters

This method has no parameters.

## NumHexagons() method
Summary

number of cells (hexagons and pentagons) for a given resolution
Parameters

This method has no parameters.

## PentagonIndexCount() method
Summary

returns the number of pentagons per resolution
Parameters

This method has no parameters.

## PointDistKm(a,b) method
Summary

"great circle distance" between pairs of GeoCoord points in kilometers
Returns
Parameters
Name 	Type 	Description
a 	H3Lib.GeoCoord 	
b 	H3Lib.GeoCoord

## PointDistM() method
Summary

"great circle distance" between pairs of GeoCoord points in meters*/
Parameters

This method has no parameters.

## PointDistRads(a,b) method
Summary

"great circle distance" between pairs of GeoCoord points in radians*/
Returns
Parameters
Name 	Type 	Description
a 	H3Lib.GeoCoord 	
b 	H3Lib.GeoCoord

## PolyFill(polygon,r,outCells) method
Summary

hexagons within the given geofence
Parameters
Name 	Type 	Description
polygon 	H3Lib.GeoPolygon 	
r 	System.Int32 	
outCells 	System.Collections.Generic.List{H3Lib.H3Index}@

## RadiansToDegrees() method
Summary

converts radians to degrees
Returns
Parameters

This method has no parameters.

## Res0IndexCount() method
Summary

returns the number of resolution 0 cells (hexagons and pentagons)
Parameters

This method has no parameters.

## SetGeoDegs() method
Summary

Winging this one, returns a GeoCoord with degree values instead of radians
Parameters

This method has no parameters.

## StringToH3() method
Summary

converts the canonical string format to H3Index format
Parameters

This method has no parameters.

## Uncompact() method
Summary

uncompacts the compacted hexagon set
Parameters

This method has no parameters.
