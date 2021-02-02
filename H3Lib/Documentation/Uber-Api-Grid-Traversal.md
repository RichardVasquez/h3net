# Grid Traversal functions

Grid traversal allows finding cells in the
vicinity of an origin cell, and determining
how to traverse the grid from one cell to
another.

## KRing
```c#
void Api.KRing(H3Index origin, int k, out List<H3Index> outcells)
```
### Summary
k-rings produces indices within k distance of the origin
index.

k-ring 0 is defined as the origin index, k-ring 1
is defined as k-ring 0 and all neighboring indices,
and so on.

Output is placed in the provided List<H3Index> in no
particular order. Elements of the output array may be
left zero, as can happen when crossing a pentagon.
### Parameters
| Name | Type | Description |
|------|------|-------------|
| origin | H3Lib.H3Index | center H3Index cell |
| k | int | Radius of cells from origin |
| outCells | **out** List<H3Lib.H3Index> | All H3Index that make up the kRing |

## MaxKringSize
```c#
int Api.MaxKringSize(int k)
```
### Summary
Maximum number of indices that result
from the kRing algorithm with the given k.
### Parameters
| Name | Type | Description |
|------|------|-------------|
| k | int | The radius for KRings|
## KRingDistances
```c#
void Api.KRingDistances(H3Index origin, int k, out List<H3Index> outCells, out List<int> distances)
```
### Summary
k-rings produces indices within k distance of the origin index.

k-ring 0 is defined as the origin index, k-ring 1 is defined
as k-ring 0 and all neighboring indices, and so on.

Output is placed in the provided List&lt;H3Index&gt;/List&lt;int&gt;
in no particular order, though index[i] of each list will provide data
matching to the specified H3Index/distance. Elements of the output
array may be left zero, as can happen when crossing a pentagon.
### Parameters
| Name | Type | Description |
|------|------|-------------|
|origin|H3Lib.H3Index|Origin point of KRings|
|k|int|Radius of KRings|
|outCells|**out** List<H3Lib.H3Index>|H3Index values tha make up the KRings|
|distances|**out** List&lt;int&gt;|Distance of each H3Index cell from the origin|

## HexRange
```c#
int Api.HexRange(H3Index origin, int k, out List<H3Index> outHex)
```
### Summary
HexRange produces indexes within k distance of the origin index.
Output behavior is undefined when one of the indexes returned by
this function is a pentagon or is in the pentagon distortion area.

k-ring 0 is defined as the origin index, k-ring 1 is defined as
k-ring 0 and all neighboring indexes, and so on.

Output is placed in the List&lt;H3Index&gt; in order of increasing
distance from the origin.
### Parameters
| Name | Type | Description |
|------|------|-------------|
|origin|H3Lib.H3Index|Origin point of HexRange|
|k|int|Radius of HexRange|
|outHex|**out** List&lt;H3Lib.Hindex&gt;|H3Index cells in increasing distance|

## HexRangeDistances
```c#
int Api.HexRangeDistances(H3Index origin, int k, out List<H3Index> outCells, out List<int> distances)
```
### Summary
HexRangeDistances produces two sets of indexes within k distance of
the origin index.  One contains the H3Index values, and the other
contains the corresponding distances from the origin.  Output behavior
is undefined when one of the indexes returned by this function is a
pentagon or is in the pentagon distortion area.

k-ring 0 is defined as the origin index, k-ring 1 is defined as k-ring
0 and all neighboring indexes, and so on.

Output is placed in the provided List&lt;H3Index&lt; outCells
in order of increasing distance from the origin. The distances in
hexagons is placed in the List&lt;int&gt; distances at the same
offset.

Returns 0 if no pentagonal distortion is encountered.
### Parameters
| Name | Type | Description |
|------|------|-------------|
|origin|H3Lib.H3Index|Origin point of HexRangeDistances|
|k|int|Radius of HexRangeDistances|
|outCells|**out** List&lt;H3Lib.H3Index&gt;|H3Index cells in increasing distance|
|distances|**out** List&lt;int&gt;|Distance of H3Index cells from origin|

## HexRanges
```c#
int Api.HexRanges(List<H3Index> h3Set, int length, int k, out List<H3Index> outCells)
```
### Summary
HexRanges takes a collection of input hex IDs and a max k-ring and
returns a collection of hexagon IDs sorted first by the original hex
IDs and then by the k-ring (0 to max), with no guaranteed sorting
within each k-ring group.

Returns 0 if no pentagonal distortion was encountered. Otherwise,
output is undefined
### Parameters
| Name | Type | Description |
|------|------|-------------|
|h3Set|List&lt;H3Lib.H3Index&gt;|Collection of initial H3Index cells|
|length|int|Maximum k-ring length|
|outCells|**out** List<H3Lib.H3Index>|Resulting H3Index cells|

## HexRing
```c#
int Api.HexRing(H3Index origin, int k, out List<H3Index> outCells)
```
### Summary
Produces the hollow hexagonal ring centered at origin with sides
of length k.

Returns 0 if no pentagonal distortion was encountered.
### Parameters
| Name | Type | Description |
|------|------|-------------|
|origin|H3Lib.H3Index|Origin cell for HexRing|
|k|int|Radius of HexRing|
|outCells|List&lt;H3Lib.H3Index&gt;|Collection of H3Index cells making the HexRing|

## H3Line
```c#
int Api.H3Line(H3Index start, H3Index end, out List<H3Index> outCells)
```
### Summary
Given two H3 indexes, return the line of indexes between them (inclusive).

This function may fail to find the line between two indexes, for
example if they are very far apart. It may also fail when finding
distances for indexes on opposite sides of a pentagon.

Notes:

  * The specific output of this function should not be considered
    stable across library versions. The only guarantees the library
    provides are that the line length will be
    h3Distance(start, end) + 1 and that every index in the line will
    be a neighbor of the preceding index.
  * Lines are drawn in grid space, and may not correspond exactly to
    either Cartesian lines or great arcs.

### Parameters
| Name | Type | Description |
|------|------|-------------|
|start|H3Lib.H3Index|Staring H3Index cell for H3Line|
|end|H3Lib.H3Index|Ending H3Index cell for H3Line|
|outCells|**out** List<H3Lib.H3Index>|Collection of cells that make the H3Line|

## H3LineSize
```c#
int Api.H3LineSize(H3Index start, H3Index end)
```
### Summary
Number of indexes in a line from the start index to the end index,
to be used for allocating memory.

Returns a negative number if the line cannot be computed.
### Parameters
| Name | Type | Description |
|------|------|-------------|
|start|H3Lib.H3Index|Starting H3Index cell|
|end|H3Lib.H3Index|Ending H3Index cell|

## h3Distance
```c#
int Api.H3Distance(H3Index origin, H3Index h3)
```
### Summary
Returns the distance in grid cells between the two indexes.

Returns a negative number if finding the distance failed.
Finding the distance can fail because the two indexes are not
comparable (different resolutions), too far apart, or are
separated by pentagonal distortion. This is the same set of
limitations as the local IJ coordinate space functions.
### Parameters
| Name | Type | Description |
|------|------|-------------|
|origin|H3Lib.H3Index|origin|
|h3|H3Lib.H3Index|destination|
int h3Distance(H3Index origin, H3Index h3);

## ExperimentalH3ToLocalIj
```c#
int Api.ExperimentalH3ToLocalIj(
    H3Index origin, H3Index h3, out CoordIj outCoord)
```
### Summary
Produces local IJ coordinates for an H3 index anchored by an origin.

This function is experimental, and its output is not guaranteed to be
compatible across different versions of H3.
### Parameters
| Name | Type | Description |
|------|------|-------------|
|origin|H3Lib.H3Index|Anchor H3Index|
|h3|H3Lib.H3Index|H3Index to convert|
|outCoord|**out** H3Lib.CoordIj|Converted CoordIJ|

## ExperimentalLocalIjToH3
```c#
int Api.ExperimentalLocalIjToH3(H3Index origin, CoordIj ij, out H3Index outCell)
```
### Summary
Produces an H3 index from local IJ coordinates anchored by an origin.

This function is experimental, and its output is not guaranteed to be
compatible across different versions of H3.
### Parameters
| Name | Type | Description |
|------|------|-------------|
|orgin|H3Lib.H3Index|Anchor H3Index cell|
|ij|H3Lib.CoordIj|IJ Coordinate|
|outcell|H3Lib.H3Index|H3Index cell converted from CoordIj|

<hr>

[Return to Uber API Table of Contents](Uber-Api.md)
